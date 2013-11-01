using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  public class DataSourceReaderBase
    : Dictionary<string, IDataSourceRecordReader>, IDataSourceReader
  {
    /*public new DataSourceRecordReaderDb this[string key]
    {
      get
      {
        return (DataSourceRecordReaderDb)base[key];
      }
    }*/

    public DataSourceReaderBase()
    {
    }

    /// <summary>
    /// Simple constructor if there is only one record
    /// </summary>
    /// <param name="ref_conn"></param>
    /// <param name="command_text"></param>
    /*public DataSourceReaderDb(string id, DbConnection refConn, string commandText)
      : this()
    {
      this.Add(id, CreateRecordReader(id, refConn, commandText));
    }*/

    /*public DataSourceRecordReaderDb CreateRecordReader(string id, DbConnection refConn, string commandText)
    {
      return new DataSourceRecordReaderDb(id, refConn, commandText);
    }*/

    public void OpenAll()
    {
      Enumerator en = this.GetEnumerator();
      while (en.MoveNext())
      {
        en.Current.Value.Open();
      }
    }

    public void CloseAll()
    {
      Enumerator en = this.GetEnumerator();
      while (en.MoveNext())
      {
        en.Current.Value.Close();
      }
    }
  }
}
