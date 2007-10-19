using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NCsvLib
{
  public class CsvOutputWriterStream : CsvOutputWriterBase
  {
    private StreamWriter _Sw;
    protected Stream _Str;
    
    public CsvOutputWriterStream(Stream stream)
      : base()
    {
      _Str = stream;
    }

    public override void Open()
    {
      _Sw = new StreamWriter(_Str, _Enc);
    }

    public override void Close()
    {
      _Sw.Close();
    }

    public override void WriteFieldValue(string val)
    {
      if (val != string.Empty)
        _Sw.Write(val);
    }

    public override void WriteSeparator(string sep)
    {
      if (sep != string.Empty)
        _Sw.Write(sep);
    }

    public override void WriteEol(string sEol)
    {
      if (sEol != string.Empty)
        _Sw.Write(sEol);
    }

    public override void WriteColHeaders(SchemaRecord rec, string sep, string eol)
    {
      string s = PrepareColHeaders(rec, sep, eol);
      if (s != string.Empty)
        _Sw.Write(s);
    }
  }
}
