using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  public abstract class CsvOutputWriterBase : ICsvOutputWriter
  {
    public CsvOutputWriterBase()
    {
      
    }

    /// <summary>
    /// Returns a string with the value convertd in csv format
    /// </summary>
    /// <param name="fld"></param>
    /// <param name="sch"></param>
    /// <returns></returns>
    protected virtual string PrepareField(InputField fld, SchemaField sch)
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
        throw new NCsvLibOutputException("Schema data type not supported");
      //Creates a stringbuilder with correct size
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

    public abstract void WriteField(InputField fld, SchemaField sch);
    public abstract void WriteSeparator(string sep);
    public abstract void WriteEol(string sEol);
  }
}
