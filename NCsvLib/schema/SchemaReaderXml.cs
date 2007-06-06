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
        if (Rdr.IsStartElement("field"))
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
          rec.FldType = CsvFieldType.Int;
          break;
        case "string":
          rec.FldType = CsvFieldType.String;
          break;
        case "double":
          rec.FldType = CsvFieldType.Double;
          break;
        case "decimal":
          rec.FldType = CsvFieldType.Decimal;
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
          rec.Alignment = CsvValueAlignment.Left;
          break;
        case "right":
          rec.Alignment = CsvValueAlignment.Right;
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
  }
}
