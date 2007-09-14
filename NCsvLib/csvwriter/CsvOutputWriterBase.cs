using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  public abstract class CsvOutputWriterBase : ICsvOutputWriter
  {
    protected Encoding _Enc;
    public Encoding Enc
    {
      get { return _Enc; }
      set { _Enc = value; }
    }

    protected string _Quotes;
    public string Quotes
    {
      get { return _Quotes; }
      set { _Quotes = value; }
    }

    public CsvOutputWriterBase()
    {
      
    }

    /// <summary>
    /// Returns a string with the value convertd in csv format
    /// </summary>
    /// <param name="fld"></param>
    /// <param name="sch"></param>
    /// <returns></returns>
    protected virtual string PrepareField(DataSourceField fld, SchemaField sch)
    {
      //If field has fixed value returns the fixed value immediately
      if (sch.HasFixedValue)
        return sch.FixedValue;

      StringBuilder sb = new StringBuilder();
      string s;
      int sz = 0;
      //Converts value to string
      if (sch.FldType == SchemaFieldType.Decimal)
      {
        if (sch.CustFmt != null)
          sb.AppendFormat(sch.CustFmt, "{0}", ((decimal)fld.Value));
        else
          sb.Append(((decimal)fld.Value).ToString(sch.Format));
      }
      else if (sch.FldType == SchemaFieldType.Double)
      {
        if (sch.CustFmt != null)
          sb.AppendFormat(sch.CustFmt, "{0}", ((double)fld.Value));
        else
          sb.Append(((double)fld.Value).ToString(sch.Format));
      }
      else if (sch.FldType == SchemaFieldType.Int)
      {
        if (sch.CustFmt != null)
          sb.AppendFormat(sch.CustFmt, "{0}", ((int)fld.Value));
        else
          sb.Append(((int)fld.Value).ToString(sch.Format));
      }
      else if (sch.FldType == SchemaFieldType.String)
      {
        if (sch.CustFmt != null)
          sb.AppendFormat(sch.CustFmt, "{0}", ((string)fld.Value));
        else
          sb.Append(fld.Value.ToString());
      }
      else
        throw new NCsvLibOutputException("Schema data type not supported");
      s = sb.ToString();
      //Creates a stringbuilder with correct size
      //TODO To be improved
      if (sch.AddQuotes)
        sz = 2;
      if (sch.FixedLen)
        sz += sch.Size;
      else
        sz += s.Length;
      sb = new StringBuilder(sz);
      //Verifies if quotes are specified, otherwise use a space
      char qt;
      if (_Quotes.Length > 0)
        qt = _Quotes[0];
      else
        qt = ' ';
      //Writes the value based on alignment
      if (sch.Alignment == SchemaValueAlignment.Left)
      {
        int i = 0, j=0;
        //TODO Quotes is now single char, add possibility for it to be more than one char
        if (sch.AddQuotes)
        {
          sb.Append(qt);
          i++;
        }
        for (; i < sz; i++)
        {
          if (j < s.Length)
            sb.Append(s[j++]);
          else if (i == (sz - 1) && sch.AddQuotes)
            sb.Append(qt);
          else if (sch.Filled)
            sb.Append(sch.FillChar);
          else
            sb.Append(' ');
        }
      }
      else if (sch.Alignment == SchemaValueAlignment.Right)
      {
        int i = 0, j = 0;
        int txtlen = sch.AddQuotes ? s.Length + 1 : s.Length;
        if (sch.AddQuotes)
        {
          sb.Append(qt);
          i++;
        }
        for (; i < sz; i++)
        {
          if (i < (sz - txtlen))
          {
            if (sch.Filled)
              sb.Append(sch.FillChar);
            else
              sb.Append(' ');
          }
          else if (j < s.Length)
            sb.Append(s[j++]);
          else
            sb.Append(qt);
        }
      }
      return sb.ToString();
    }

    public abstract void Open();
    public abstract void Close();
    public abstract void WriteField(DataSourceField fld, SchemaField sch);
    public abstract void WriteSeparator(string sep);
    public abstract void WriteEol(string sEol);
  }
}
