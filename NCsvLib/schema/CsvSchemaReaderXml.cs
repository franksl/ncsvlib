using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace NCsvLib
{
  public class CsvSchemaReaderXml : ICsvSchemaReader
  {
    public string FileName;
    private XmlReader Rdr;

    private CsvSchemaReaderXml()
    {
    }

    public CsvSchemaReaderXml(string file_name)
    {
      if (!File.Exists(file_name))
        throw new NCsvLibSchemaException("Xml schema file not found");
      FileName = file_name;
    }

    public CsvSchema GetSchema()
    {
      CsvSchema sch = new CsvSchema();
      
      Rdr = XmlReader.Create(FileName);
      Rdr.ReadStartElement("ncsvlib");
      while (Rdr.Read())
      {
        if (Rdr.IsStartElement("record"))
        {
          sch.Add(ReadRecord());
        }
      }
      Rdr.Close();
      return sch;
    }


    private CsvSchemaField ReadRecord()
    {
      CsvSchemaField rec;
      string s;

      rec = new CsvSchemaField();
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
