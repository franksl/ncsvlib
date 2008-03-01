using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace NCsvLib
{
  public class DataSourceRecordReaderDb : DataSourceRecordReaderBase
  {
    protected DbConnection _Conn;
    public DbConnection Connection
    {
      get { return _Conn; }
      set { _Conn = value; }
    }

    protected DbCommand _Cmd;
    public DbCommand Command
    {
      get { return _Cmd; }
    }

    protected DbDataReader _Rdr;

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

    public override void Open()
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

		public override void Close()
    {
      if (_Rdr == null)
        return;
      if (!_Rdr.IsClosed)
        _Rdr.Close();
      if (_Cmd.Connection.State == System.Data.ConnectionState.Open)
        _Cmd.Connection.Close();
    }

		public override bool Read()
    {
      if (_Rdr == null)
        throw new NCsvLibDataSourceException("DbDataReader closed");
      if (!_Rdr.IsClosed)
        return _Rdr.Read();
      else
        throw new NCsvLibDataSourceException("DataSource Record Reader closed");
    }

		public override DataSourceField GetField(string name)
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
        throw new NCsvLibDataSourceException("Field name not found in db: " + name);
      }
      DataSourceField fld = new DataSourceField();
      fld.Name = name;
      //If value in db is null it is set to null
      if (_Rdr.IsDBNull(idx))
        fld.Value = null;
      else
        fld.Value = _Rdr.GetValue(idx);
      return fld;
    }    
  }
}
