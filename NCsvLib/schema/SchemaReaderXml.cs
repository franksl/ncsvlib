using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using NCsvLib.Formatters;

namespace NCsvLib
{
  public class SchemaReaderXml : ISchemaReader
  {
    public string FileName;
    private XmlReader _Rdr;
    private Schema _Sch;

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
      _Sch = new Schema();
      
      _Rdr = XmlReader.Create(FileName);
      _Rdr.ReadStartElement("ncsvlib");
      try
      {
        while (_Rdr.Read())
        {
          if (_Rdr.IsStartElement("options"))
          {
            try
            {
              ReadOptions();
            }
            catch (Exception ex)
            {
              throw new NCsvLibSchemaException("Error while reading schema options." 
                + Environment.NewLine + ex.Message);
            }
          }
          else if (_Rdr.IsStartElement("schema"))
          {
            try
            {
              ReadRecords();
            }
            catch (Exception ex)
            {
              throw new NCsvLibSchemaException("Error while reading schema records."
                + Environment.NewLine + ex.Message);
            }
          }
        }
      }
      finally
      {
        _Rdr.Close();
      }
      return _Sch;
    }

    private void ReadOptions()
    {
      string s;
      while (_Rdr.Read() && _Rdr.MoveToContent() != XmlNodeType.EndElement &&
            _Rdr.Name != "options")
      {
        if (_Rdr.IsStartElement("fieldseparator"))
        {
          s = _Rdr.GetAttribute("usedefault");
          if (s != null && s.ToLower().Trim() == "false")
          {
            _Sch.Options.FieldSeparator = _Rdr.GetAttribute("value");
          }
        }
        else if (_Rdr.IsStartElement("eol"))
        {
          s = _Rdr.GetAttribute("usedefault");
          if (s != null && s.ToLower().Trim() == "false")
          {
            _Sch.Options.Eol = _Rdr.GetAttribute("value");
          }
        }
        else if (_Rdr.IsStartElement("quotes"))
        {
          s = _Rdr.GetAttribute("usedefault");
          if (s != null && s.ToLower().Trim() == "false")
          {
            _Sch.Options.Quotes = _Rdr.GetAttribute("value");
          }
        }
        else if (_Rdr.IsStartElement("encoding"))
        {
          s = _Rdr.GetAttribute("value");
          if (s != null)
            _Sch.Options.Enc = Encoding.GetEncoding(s);          
        }
      }
    }

    private void ReadRecords()
    {
      SchemaRecordBase rec = null;
      Stack<SchemaRecordComposite> stk = new Stack<SchemaRecordComposite>();

      while (_Rdr.Read() && _Rdr.MoveToContent() != XmlNodeType.EndElement &&
            _Rdr.Name != "schema")
      {
        if (_Rdr.IsStartElement("recordgroup"))
        {
          rec = new SchemaRecordComposite();
          //rec.Id = Rdr.GetAttribute("id");
          stk.Push((SchemaRecordComposite)rec);
          ReadRecordGroup(stk);
          _Sch.Add(rec);
        }
        else if (_Rdr.IsStartElement("record"))
        {
          SchemaRecord r = new SchemaRecord();
          ReadRecord(r);
          _Sch.Add(r);
        }
      }
      if (_Rdr.MoveToContent() == XmlNodeType.EndElement && _Rdr.Name == "schema")
        _Rdr.ReadEndElement();
    }

    private void ReadRecordGroup(Stack<SchemaRecordComposite> stk)
    {
      SchemaRecordComposite comp = stk.Peek();
      comp.Id = _Rdr.GetAttribute("id");
      comp.Repeat = int.Parse(_Rdr.GetAttribute("repeat"));
      if (comp.Id == null || comp.Id.Trim() == string.Empty)
        throw new NCsvLibSchemaException("id not specified in recordgroup");
      while (_Rdr.Read() && _Rdr.MoveToContent() != XmlNodeType.EndElement &&
            _Rdr.Name != "recordgroup")
      {
        if (_Rdr.IsStartElement("record"))
        {
          SchemaRecord r = new SchemaRecord();          
          ReadRecord(r);
          stk.Peek().Add(r);
        }
        else if (_Rdr.IsStartElement("recordgroup"))
        {
          comp = new SchemaRecordComposite();
          stk.Peek().Add(comp);
          stk.Push(comp);
          ReadRecordGroup(stk);
        }
      }
      if (_Rdr.MoveToContent() == XmlNodeType.EndElement && _Rdr.Name == "recordgroup")
        _Rdr.ReadEndElement();
      stk.Pop();
    }

    private void ReadRecord(SchemaRecord rec)
    {
      string s;
      rec.Id = _Rdr.GetAttribute("id");
      rec.Repeat = int.Parse(_Rdr.GetAttribute("repeat"));
      s = _Rdr.GetAttribute("colheaders");
      if (s != null && s != string.Empty)
      {
        try
        {
          rec.ColHeaders = bool.Parse(s);
        }
        catch
        {
          throw new NCsvLibSchemaException("Error reading record.colheaders");
        }
      }
      if (rec.Id == null || rec.Id.Trim() == string.Empty)
        throw new NCsvLibSchemaException("id not specified in record");
      while (_Rdr.Read() && _Rdr.MoveToContent() != XmlNodeType.EndElement &&
            _Rdr.Name != "record")
      {
        if (_Rdr.IsStartElement("field"))
        {
          rec.AddField(ReadField());
        }
      }
      if (_Rdr.NodeType == XmlNodeType.EndElement && _Rdr.Name == "record")
        _Rdr.ReadEndElement();
    }
    
    private SchemaField ReadField()
    {
      SchemaField fld;
      string s;

      fld = new SchemaField(_Sch);
      //Name
      fld.Name = _Rdr.GetAttribute("name");
      //FldType
      s = _Rdr.GetAttribute("type");
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
      fld.Format = _Rdr.GetAttribute("format");
      //CustFmt (uses Activator to create an ICustomFormatter instance)
      s = _Rdr.GetAttribute("custfmt");
      if (s != null)
      {
        char sep = '|';
        try
        {
          if (s.IndexOf(sep) >= 0)
          {
            string[] sarr = s.Split(new char[] { sep }, 2);
            fld.CustFmt = (ICsvOutputFormatter)Activator.CreateInstanceFrom(sarr[0], sarr[1]).Unwrap();            
          }
          else
            fld.CustFmt = (ICsvOutputFormatter)Activator.CreateInstance(Type.GetType(s, true, true));

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
      s = _Rdr.GetAttribute("alignment");
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
      s = _Rdr.GetAttribute("fixedlen");
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
      s = _Rdr.GetAttribute("size");
      if (s != null)
        fld.Size = XmlConvert.ToInt32(s);
      else
        fld.Size = 0;
      //Filled
      s = _Rdr.GetAttribute("filled");
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
      s = _Rdr.GetAttribute("fillchar");
      if (s == null || s.Length == 0)
        fld.FillChar = ' ';
      else
        fld.FillChar = s[0];
      //AddQuotes
      s = _Rdr.GetAttribute("addquotes");
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
      //Quotes (if not defined it will be inherited from schema options)
      //TODO only first char is used, could be more than one char
      s = _Rdr.GetAttribute("quotes");
      if (s != null)
        fld.Quotes = s[0].ToString();
      //HasFixedValue
      s = _Rdr.GetAttribute("hasfixedvalue");
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
      s = _Rdr.GetAttribute("fixedvalue");
      if (s != null)
        fld.FixedValue = s;
      //Comment
      s = _Rdr.GetAttribute("comment");
      if (s != null)
        fld.Comment = s;
      //ColHdr
      s = _Rdr.GetAttribute("colhdr");
      if (s != null)
        fld.ColHdr = s;

      return fld;
    }
  }
}
