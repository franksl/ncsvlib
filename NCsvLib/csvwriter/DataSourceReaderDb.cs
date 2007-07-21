using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace NCsvLib
{
  public class DataSourceReaderDb
    : Dictionary<string, IDataSourceRecordReader>, IDataSourceReader
  {
    public new DataSourceRecordReaderDb this[string key]
    {
      get
      {
        return (DataSourceRecordReaderDb)base[key];
      }
    }

    public DataSourceReaderDb()
    {
    }
        
    /// <summary>
    /// Simple constructor if there is only one record
    /// </summary>
    /// <param name="ref_conn"></param>
    /// <param name="command_text"></param>
    public DataSourceReaderDb(string id, DbConnection refConn, string commandText)
      : this()
    {
      this.Add(id, CreateRecordReader(id, refConn, commandText));
    }

    public DataSourceRecordReaderDb CreateRecordReader(string id, DbConnection refConn, string commandText)
    {
      return new DataSourceRecordReaderDb(id, refConn, commandText);
    }
  }



  public class DataSourceRecordReaderDb : IDataSourceRecordReader
  {
    public DbConnection _Conn;
    public DbCommand _Cmd;
    public DbDataReader _Rdr;

    private string _Id;
    public string Id
    {
      get
      {
        return _Id;
      }
    }

    private DataSourceRecordReaderDb()
    {
    }

    public DataSourceRecordReaderDb(string id, DbConnection refConn, string commandText)
    {
      _Id = id;
      _Conn = refConn;
      _Cmd = _Conn.CreateCommand();
      _Cmd.CommandText = commandText;
    }

    public void Open()
    {
      //Opens a db connection and creates a datareader
      _Cmd.Connection.Open();
      try
      {
        _Rdr = _Cmd.ExecuteReader();
      }
      catch
      {
        _Cmd.Connection.Close();
        throw;
      }
    }

    public void Close()
    {
      if (_Rdr == null)
        return;
      if (!_Rdr.IsClosed)
        _Rdr.Close();
      if (_Cmd.Connection.State == System.Data.ConnectionState.Open)
        _Cmd.Connection.Close();
    }

    public bool Read()
    {
      if (_Rdr == null)
        throw new NCsvLibDataSourceException("DbDataReader closed");
      if (!_Rdr.IsClosed)
        return _Rdr.Read();
      else
        throw new NCsvLibDataSourceException("DataSource Record Reader closed");
    }

    public DataSourceField GetField(string name)
    {
      if (_Rdr == null)
        throw new NCsvLibDataSourceException("DbDataReader not defined");
      if (_Rdr.IsClosed)
        throw new NCsvLibDataSourceException("DataSource Record Reader closed");
      int idx;
      try
      {
        idx = _Rdr.GetOrdinal(name);
      }
      catch (IndexOutOfRangeException)
      {
        throw new NCsvLibDataSourceException("Field name not found in db");
      }
      DataSourceField fld = new DataSourceField();
      fld.Name = name;
      fld.Value = _Rdr.GetValue(idx);
      return fld;
    }    
  }
}
