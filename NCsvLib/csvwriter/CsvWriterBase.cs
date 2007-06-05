using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  public abstract class CsvWriterBase : ICsvWriter
  {
    private string _Separator;
    public string Separator
    {
      get { return _Separator; }
      set { _Separator = value; }
    }

    private string _Eol;
    public string Eol
    {
      get { return _Eol; }
      set { _Eol = value; }
    }

    private char _Quotes;
    public char Quotes
    {
      get { return _Quotes; }
      set { _Quotes = value; }
    }

    public CsvWriterBase()
    {
      _Separator = "|";
      _Eol = Environment.NewLine;
      _Quotes = '"';
    }

    /// <summary>
    /// Returns a string with the value convertd in csv format
    /// </summary>
    /// <param name="fld"></param>
    /// <param name="sch"></param>
    /// <returns></returns>
    protected virtual string PrepareField(InputField fld, CsvSchemaField sch)
    {
      //If field has fixed value returns the fixed value immediately
      if (sch.HasFixedValue)
        return sch.FixedValue;

      string s = string.Empty;
      int sz = 0;
      //Converts value to string
      if (sch.FldType == CsvFieldType.Decimal)
        s = ((decimal)fld.Value).ToString(sch.Format);
      else if (sch.FldType == CsvFieldType.Double)
        s = ((double)fld.Value).ToString(sch.Format);
      else if (sch.FldType == CsvFieldType.Int)
        s = ((int)fld.Value).ToString(sch.Format);
      else if (sch.FldType == CsvFieldType.String)
        s = fld.Value.ToString();
      else 
        throw new NCsvLibOutputException("Tipo dati schema non supportato");
      //Crea a stringbuilder with correct size
      if (sch.AddQuotes)
        sz = 2;
      if (sch.FixedLen)
        sz += sch.Size;
      else
        sz += s.Length;
      StringBuilder sb = new StringBuilder(sz);
      //Writes the value based on alignment
      
      return sb.ToString();
    }

    public abstract void WriteField(InputField fld, CsvSchemaField sch);
    public abstract void WriteSeparator();
    public abstract void WriteEol();
  }
}
