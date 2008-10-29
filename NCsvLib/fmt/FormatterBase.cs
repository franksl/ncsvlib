using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib.Formatters
{
  /// <summary>
  /// Base class for custom formatters. It implements IFormatProvider and
  /// ICustomFormatter but with an abstract ICustomFormatter.Format method
  /// that must be implemented by derived classes to create custom string
  /// representations of data source values
  /// </summary>
  public class FormatterBase : ICsvOutputFormatter
  {
    private Schema _Sch;
    public Schema Sch
    {
      get { return _Sch; }
      set { _Sch = value; }
    }

    public virtual string Format(DataSourceField fld, SchemaField sch)
    {
      //If field has fixed value returns the fixed value immediately
      if (sch.HasFixedValue)
        return sch.FixedValue;

      StringBuilder sb = new StringBuilder();
      string s;
      
      //Converts value to string (empty string if value is null
			if (fld.Value != null)
			{
				if (sch.CustFmt != null)
					sb.Append(sch.CustFmt.Format(fld, sch));
				else
				{
					if (sch.FldType == SchemaFieldType.Decimal)
						sb.Append(((decimal)fld.Value).ToString(sch.Format));
					else if (sch.FldType == SchemaFieldType.Double)
						sb.Append(((double)fld.Value).ToString(sch.Format));
					else if (sch.FldType == SchemaFieldType.Int)
						sb.Append(((int)fld.Value).ToString(sch.Format));
					else if (sch.FldType == SchemaFieldType.String)
						sb.Append(fld.Value.ToString());
					else if (sch.FldType == SchemaFieldType.DateTime)
						sb.Append(((DateTime)fld.Value).ToString(sch.Format));
					else if (sch.FldType == SchemaFieldType.Bool)
					{
						if (sch.BoolSettings == null)
							throw new NCsvLibOutputException("Settings for bool field not found");
						if (sch.BoolSettings.CompareIoValue(fld.Value))
							sb.Append(sch.BoolSettings.TrueValue);
						else
							sb.Append(sch.BoolSettings.FalseValue);
					}
					else
						throw new NCsvLibOutputException("Schema data type not supported");
				}
				s = sb.ToString();
			}
			else
			{
				s = sch.NullValueWrt;
			}
      return Fill(s, sch);
    }

    protected string Fill(string s, SchemaField sch)
    {
      int sz = 0;
      //Creates a stringbuilder with correct size
      //TODO To be improved
      if (sch.AddQuotes)
        sz = 2;
      if (sch.FixedSize)
        sz += sch.Size;
      else
        sz += s.Length;
      StringBuilder sb = new StringBuilder(sz);
      //Verifies if quotes are specified, otherwise use a space
      char qt;
      //if (_Sch != null && _Sch.Options.Quotes.Length > 0)
        //qt = _Sch.Options.Quotes[0];
      if (sch.Quotes != null && sch.Quotes.Length > 0)
        qt = sch.Quotes[0];
      else
        qt = ' ';
      //Writes the value based on alignment
      if (sch.Alignment == SchemaValueAlignment.Left)
      {
        int i = 0, j=0;
        //TODO Quotes is actually single char, add possibility for it to be more than one char
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
  }
}
