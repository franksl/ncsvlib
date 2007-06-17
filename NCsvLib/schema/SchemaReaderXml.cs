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
        throw new NCsvLibSchemaException("Xml schema file not found");
      FileName = file_name;
    }

    public Schema GetSchema()
    {
      Schema sch = new Schema();
      
      Rdr = XmlReader.Create(FileName);
      Rdr.ReadStartElement("ncsvlib");
      while (Rdr.Read())
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
        else if (Rdr.IsStartElement("field"))
        {
          sch.Add(ReadField());
        }
      }
      Rdr.Close();
      return sch;
    }


    private SchemaField ReadField()
    {
      SchemaField rec;
      string s;

      rec = new SchemaField();
      rec.Name = Rdr.GetAttribute("name");
      s = Rdr.GetAttribute("type");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "int":
          rec.FldType = SchemaFieldType.Int;
          break;
        case "string":
          rec.FldType = SchemaFieldType.String;
          break;
        case "double":
          rec.FldType = SchemaFieldType.Double;
          break;
        case "decimal":
          rec.FldType = SchemaFieldType.Decimal;
          break;
      }
      rec.Format = Rdr.GetAttribute("format");
      s = Rdr.GetAttribute("alignment");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "left":
        case null:
          rec.Alignment = SchemaValueAlignment.Left;
          break;
        case "right":
          rec.Alignment = SchemaValueAlignment.Right;
          break;
      }
      s = Rdr.GetAttribute("fixedlen");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "true":
          rec.FixedLen = true;
          break;
        case "false":
        case null:
          rec.FixedLen = false;
          break;
      }

      s = Rdr.GetAttribute("size");
      if (s != null)
        rec.Size = XmlConvert.ToInt32(s);
      else
        rec.Size = 0;
      
      s = Rdr.GetAttribute("addquotes");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "true":
          rec.AddQuotes = true;
          break;
        case "false":
        case null:
          rec.AddQuotes = false;
          break;
      }
      s = Rdr.GetAttribute("hasfixedvalue");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "true":
          rec.HasFixedValue = true;
          break;
        case "false":
        case null:
          rec.HasFixedValue = false;
          break;
      }
      rec.FixedValue = Rdr.GetAttribute("fixedvalue");
      return rec;
    }

    private void ReadOptions(Schema sch)
    {

    }
  }
}
