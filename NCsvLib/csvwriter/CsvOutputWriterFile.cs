using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NCsvLib
{
  public class CsvOutputWriterFile : CsvOutputWriterBase
  {
    private StreamWriter _Sw;
    private string _FileName;
    
    public CsvOutputWriterFile(string fileName)
      : base()
    {
      _FileName = fileName;
    }

    public override void Open()
    {
      _Sw = new StreamWriter(_FileName, false, _Enc);
    }

    public override void Close()
    {
      _Sw.Close();
    }

    public override void WriteField(DataSourceField fld, SchemaField sch)
    {
      string s = PrepareField(fld, sch);
      if (s != string.Empty)
        _Sw.Write(s);
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
  }
}
