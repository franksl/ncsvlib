using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace NCsvLib.Formatters
{
  /// <summary>
  /// 
  /// </summary>
  public class NumberDigitsFormatter : CustomFormatter
  {
    public override string Format(string format, object arg, IFormatProvider formatProvider)
    {
      string s;      
      if (arg is double)
        s = ((double)arg).ToString(CultureInfo.InvariantCulture);
      else if (arg is float)
        s = ((float)arg).ToString(CultureInfo.InvariantCulture);
      else if (arg is decimal)
        s = ((decimal)arg).ToString(CultureInfo.InvariantCulture);
      else
        s = arg.ToString();
      StringBuilder sb = new StringBuilder(s.Length);
      for (int i = 0; i < s.Length; i++)
      {
        if (Char.IsDigit(s[i]))
          sb.Append(s[i]);
      }
      return sb.ToString();
    }
  }
}
