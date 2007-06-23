using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  public class Schema : SchemaRecordComposite
  {
    public SchemaOptions Options;

    public Schema()
      : base()
    {
      Options = new SchemaOptions();
      //Default values for options
      Options.Eol = Environment.NewLine;
      Options.FieldSeparator = "|";
      Options.Quotes = "\"";
      Options.Enc = Encoding.UTF8;
    }
  }

  public struct SchemaOptions
  {
    public string FieldSeparator;
    public string Eol;
    public string Quotes;
    public Encoding Enc;
  }
}
