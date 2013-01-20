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
      Options.FieldSeparator = string.Empty;
      Options.Quotes = "\"";
      Options.Enc = Encoding.Default;
			//Sets limit max to 1 to execute the whole schema only one time
			_Limit = new SchemaRecordLimit(0, 1);
    }
  }

  public class SchemaOptions
  {
    public string FieldSeparator;
    private string _Eol;
    public string Eol
    {
        get { return _Eol; }
        set 
        {
            string v = value.ToLower();
            if (v == "cr")
                _Eol = Encoding.ASCII.GetString(new byte[] { 13 });
            else if (v == "lf")
                _Eol = Encoding.ASCII.GetString(new byte[] { 10 });
            else if (v == "crlf")
                _Eol = Encoding.ASCII.GetString(new byte[] { 13, 10 });
            else
                _Eol = value;
        }
    }
    public string Quotes;
    public Encoding Enc;
		
		/// <summary>
		/// If Enc is one of the common encodings (ie utf8) sets the encoding so
		/// that the byte order mark is not emitted
		/// </summary>
		/// <param name="noBom"></param>
		public void SetNoBomEncoding(bool noBom)
		{
			if (Enc == null)
				return;
			if (Enc.CodePage == Encoding.UTF8.CodePage)
			{
				if (noBom)
					Enc = new UTF8Encoding(false);
				else
					Enc = Encoding.UTF8;
			}
			else if (Enc.CodePage == Encoding.Unicode.CodePage)
			{
				if (noBom)
					Enc = new UnicodeEncoding(false, false);
				else
					Enc = Encoding.Unicode;
			}
			else if (Enc.CodePage == Encoding.BigEndianUnicode.CodePage)
			{
				if (noBom)
					Enc = new UnicodeEncoding(true, false);
				else
					Enc = Encoding.BigEndianUnicode;
			}
			else if (Enc.CodePage == Encoding.UTF32.CodePage)
			{
				if (noBom)
					Enc = new UTF32Encoding(false, false);
				else
					Enc = Encoding.UTF32;
			}
		}
  }
}
