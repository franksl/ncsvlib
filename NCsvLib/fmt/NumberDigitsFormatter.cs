using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace NCsvLib.Formatters
{
  /// <summary>
  /// 
  /// </summary>
  public class NumberDigitsFormatter : FormatterBase
  {
    /*in questa classe non è definito _Sch da cui viene prelevato il carattere
    per Quotes utilizzato in Fill. 
    SchemaField potrebbe contenere un rif. allo schema?
    Il costruttore andrebbe mantenuto senza argomenti.*/
    
    public override string Format(DataSourceField fld, SchemaField sch)
    {
      string s;
      if (sch.FldType == SchemaFieldType.Double)
        s = ((double)fld.Value).ToString(CultureInfo.InvariantCulture);
      else if (sch.FldType == SchemaFieldType.Decimal)
        s = ((decimal)fld.Value).ToString(CultureInfo.InvariantCulture);
      else
        s = fld.Value.ToString();
      StringBuilder sb = new StringBuilder(s.Length);
      for (int i = 0; i < s.Length; i++)
      {
        if (Char.IsDigit(s[i]))
          sb.Append(s[i]);
      }
      return Fill(sb.ToString(), sch);
    }
  }
}
