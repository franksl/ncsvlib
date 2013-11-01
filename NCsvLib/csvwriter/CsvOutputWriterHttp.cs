using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;

namespace NCsvLib
{
  public class CsvOutputWriterHttp : CsvOutputWriterBase
  {
    private HttpResponse _Resp;

    private string _FileName;
    public string FileName
    {
      get { return _FileName; }
      set { _FileName = value; }
    }

    public CsvOutputWriterHttp(HttpResponse resp)
      : base()
    {
      _Resp = resp;
    }

    public CsvOutputWriterHttp(HttpResponse resp, string fileName)
      : this(resp)
    {
      _FileName = fileName;
    }

    public override void Open()
    {
      //Setting http headers
      _Resp.ContentType = "application/octet-stream";
      if (_FileName == null || _FileName == string.Empty)
        _Resp.AddHeader("Content-Disposition", "attachment");
      else
        _Resp.AddHeader("Content-Disposition", "attachment; filename=\"" + _FileName + "\"");
    }

    public override void Close()
    {
      
    }

    public override void WriteFieldValue(string val)
    {
      if (val != string.Empty)
        WriteString(val);
    }

    public override void WriteSeparator(string sep)
    {
      if (sep != string.Empty)
        WriteString(sep);
    }

    public override void WriteEol(string sEol)
    {
      if (sEol != string.Empty)
        WriteString(sEol);
    }

    public override void WriteColHeaders(SchemaRecord rec, SchemaOptions opt)
    {
      string s = PrepareColHeaders(rec, opt);
      if (s != string.Empty)
        WriteString(s);
    }

    private void WriteString(string s)
    {
      //Checks if client is still connected
      if (_Resp.IsClientConnected)
      {
        byte[] bts = _Enc.GetBytes(s);
        if (s != string.Empty)
          _Resp.OutputStream.Write(bts, 0, bts.Length);
      }
      else
        throw new NCsvLibOutputException("Client disconnected");
    }
  }
}
