using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace NCsvLib
{
  public class SchemaReaderXml : ISchemaReader
  {
    public string FileName;
    private XmlReader Rdr;

    private SchemaReaderXml()
    {
    }

    public SchemaReaderXml(string file_name)
    {
      if (!File.Exists(file_name))
        throw new NCsvLibSchemaException("Xml schema file not found: " + file_name);
      FileName = file_name;
    }

    public Schema GetSchema()
    {
      Schema sch = new Schema();
      
      Rdr = XmlReader.Create(FileName);
      Rdr.ReadStartElement("ncsvlib");
      try
      {
        while (Rdr.Read())
        {
          if (Rdr.IsStartElement("options"))
          {
            ReadOptions(sch);
          }
          else if (Rdr.IsStartElement("schema"))
          {
            ReadRecords(sch);
          }
        }
      }
      finally
      {
        Rdr.Close();
      }
      return sch;
    }

    private void ReadOptions(Schema sch)
    {
      while (Rdr.Read() && Rdr.MoveToContent() != XmlNodeType.EndElement &&
            Rdr.Name != "options")
      {
        if (Rdr.IsStartElement("fieldseparator"))
        {
          if (Rdr.GetAttribute("usedefault").ToLower().Trim() == "false")
          {
            sch.Options.FieldSeparator = Rdr.ReadElementContentAsString();
          }
        }
        else if (Rdr.IsStartElement("eol"))
        {
          if (Rdr.GetAttribute("usedefault").ToLower().Trim() == "false")
          {
            sch.Options.Eol = Rdr.ReadElementContentAsString();
          }
        }
        else if (Rdr.IsStartElement("quotes"))
        {
          if (Rdr.GetAttribute("usedefault").ToLower().Trim() == "false")
          {
            sch.Options.Quotes = Rdr.ReadElementContentAsString();
          }
        }
        else if (Rdr.IsStartElement("encoding"))
        {
          string s = Rdr.GetAttribute("value");
          sch.Options.Enc = Encoding.GetEncoding(s);          
        }
      }
    }

    private void ReadRecords(Schema sch)
    {
      SchemaRecordBase rec = null;
      Stack<SchemaRecordComposite> stk = new Stack<SchemaRecordComposite>();

      while (Rdr.Read() && Rdr.MoveToContent() != XmlNodeType.EndElement &&
            Rdr.Name != "schema")
      {
        if (Rdr.IsStartElement("recordgroup"))
        {
          rec = new SchemaRecordComposite();
          //rec.Id = Rdr.GetAttribute("id");
          stk.Push((SchemaRecordComposite)rec);
          ReadRecordGroup(stk);
          sch.Add(rec);
        }
        else if (Rdr.IsStartElement("record"))
        {
          SchemaRecord r = new SchemaRecord();
          ReadRecord(r);
          sch.Add(r);
        }
      }
      if (Rdr.MoveToContent() == XmlNodeType.EndElement && Rdr.Name == "schema")
        Rdr.ReadEndElement();
    }

    private void ReadRecordGroup(Stack<SchemaRecordComposite> stk)
    {
      SchemaRecordComposite comp = stk.Peek();
      comp.Id = Rdr.GetAttribute("id");
      comp.Repeat = int.Parse(Rdr.GetAttribute("repeat"));
      if (comp.Id == null || comp.Id.Trim() == string.Empty)
        throw new NCsvLibSchemaException("id not specified in recordgroup");
      while (Rdr.Read() && Rdr.MoveToContent() != XmlNodeType.EndElement &&
            Rdr.Name != "recordgroup")
      {
        if (Rdr.IsStartElement("record"))
        {
          SchemaRecord r = new SchemaRecord();          
          ReadRecord(r);
          stk.Peek().Add(r);
        }
        else if (Rdr.IsStartElement("recordgroup"))
        {
          comp = new SchemaRecordComposite();
          stk.Peek().Add(comp);
          stk.Push(comp);
          ReadRecordGroup(stk);
        }
      }
      if (Rdr.MoveToContent() == XmlNodeType.EndElement && Rdr.Name == "recordgroup")
        Rdr.ReadEndElement();
      stk.Pop();
    }

    private void ReadRecord(SchemaRecord rec)
    {
      rec.Id = Rdr.GetAttribute("id");
      rec.Repeat = int.Parse(Rdr.GetAttribute("repeat"));
      if (rec.Id == null || rec.Id.Trim() == string.Empty)
        throw new NCsvLibSchemaException("id not specified in record");
      while (Rdr.Read() && Rdr.MoveToContent() != XmlNodeType.EndElement &&
            Rdr.Name != "record")
      {      
        if (Rdr.IsStartElement("field"))
        {
          rec.AddField(ReadField());
        }
      }
      if (Rdr.NodeType == XmlNodeType.EndElement && Rdr.Name == "record")
        Rdr.ReadEndElement();
    }
    
    private SchemaField ReadField()
    {
      SchemaField fld;
      string s;

      fld = new SchemaField();
      //Name
      fld.Name = Rdr.GetAttribute("name");
      //FldType
      s = Rdr.GetAttribute("type");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "int":
          fld.FldType = SchemaFieldType.Int;
          break;
        case "string":
          fld.FldType = SchemaFieldType.String;
          break;
        case "double":
          fld.FldType = SchemaFieldType.Double;
          break;
        case "decimal":
          fld.FldType = SchemaFieldType.Decimal;
          break;
        case "datetime":
          fld.FldType = SchemaFieldType.DateTime;
          break;
      }
      //Format
      fld.Format = Rdr.GetAttribute("format");
      //CustFmt (uses Activator to create an ICustomFormatter instance)
      s = Rdr.GetAttribute("custfmt");
      if (s != null)
      {
        char sep = '|';
        try
        {
          if (s.IndexOf(sep) >= 0)
          {
            string[] sarr = s.Split(new char[] { sep }, 2);
            fld.CustFmt = (IFormatProvider)Activator.CreateInstanceFrom(sarr[0], sarr[1]).Unwrap();            
          }
          else
            fld.CustFmt = (IFormatProvider)Activator.CreateInstance(Type.GetType(s, true, true));

          //fld.CustFmt = (IFormatProvider)Activator.CreateInstance(Type.GetType(s, true, true));
          //fld.CustFmt = (IFormatProvider)AppDomain.CurrentDomain.CreateInstanceAndUnwrap(
          //fld.CustFmt = (IFormatProvider)Activator.CreateInstance(Type.GetType(s, true, true));
          
          /*if (s.ToLower().StartsWith("ncsvlib."))
            fld.CustFmt = (IFormatProvider)Activator.CreateInstance(Type.GetType(s, true, true));
          else
            fld.CustFmt = (IFormatProvider)System.Reflection.Assembly.GetEntryAssembly().CreateInstance(s, true);*/
        }
        catch (Exception ex)
        {
          throw new NCsvLibSchemaException("Invalid custom formatter: " + s + " " + ex.Message);
        }
      }
      //Alignment
      s = Rdr.GetAttribute("alignment");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "left":
        case null:
          fld.Alignment = SchemaValueAlignment.Left;
          break;
        case "right":
          fld.Alignment = SchemaValueAlignment.Right;
          break;
      }
      //FixedLen
      s = Rdr.GetAttribute("fixedlen");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "true":
          fld.FixedLen = true;
          break;
        case "false":
        case null:
          fld.FixedLen = false;
          break;
      }
      //Size
      s = Rdr.GetAttribute("size");
      if (s != null)
        fld.Size = XmlConvert.ToInt32(s);
      else
        fld.Size = 0;
      //Filled
      s = Rdr.GetAttribute("filled");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "true":
          fld.Filled = true;
          break;
        case "false":
        case null:
          fld.Filled = false;
          break;
      }
      //FillChar
      s = Rdr.GetAttribute("fillchar");
      if (s == null || s.Length == 0)
        fld.FillChar = ' ';
      else
        fld.FillChar = s[0];
      //AddQuotes
      s = Rdr.GetAttribute("addquotes");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "true":
          fld.AddQuotes = true;
          break;
        case "false":
        case null:
          fld.AddQuotes = false;
          break;
      }
      //HasFixedValue
      s = Rdr.GetAttribute("hasfixedvalue");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "true":
          fld.HasFixedValue = true;
          break;
        case "false":
        case null:
          fld.HasFixedValue = false;
          break;
      }
      //FixedValue
      fld.FixedValue = Rdr.GetAttribute("fixedvalue");
      //Comment
      fld.Comment = Rdr.GetAttribute("comment");

      return fld;
    }
  }
}
