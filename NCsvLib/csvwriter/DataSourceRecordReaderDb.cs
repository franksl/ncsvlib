using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace NCsvLib
{
  public class DataSourceRecordReaderDb : DataSourceRecordReaderBase
  {
    protected IDbConnection _Conn;
    public IDbConnection Connection
    {
      get { return _Conn; }
      set { _Conn = value; }
    }

    protected IDbCommand _Cmd;
    public IDbCommand Command
    {
      get { return _Cmd; }
    }

    protected IDataReader _Rdr;

		private long _RowCount;
		private long _Idx;

    private DataSourceRecordReaderDb()
    {
    }

    public DataSourceRecordReaderDb(string id, IDbConnection refConn, string commandText)
    {
      _Id = id;
      _Conn = refConn;
      _Cmd = _Conn.CreateCommand();
      _Cmd.CommandText = commandText;
			//-2 = closed, -1 Open
			_Idx = -2;
			_RowCount = -1;
    }

    public override void Open()
    {
      //Opens a db connection and creates a datareader
      _Cmd.Connection.Open();
      try
      {
				//Gets record number using COUNT(*)
				string cmdtmp = _Cmd.CommandText;
				_Cmd.CommandText = "SELECT COUNT(*) FROM (" + cmdtmp + ")"
				 + " AS NCSVLIBTBLALIAS";
				object res = _Cmd.ExecuteScalar();
				if (res == null || res == DBNull.Value)
					_RowCount = 0;
				else
					_RowCount = (long)res;
				_Cmd.CommandText = cmdtmp;
        _Rdr = _Cmd.ExecuteReader();
      }
      catch
      {
        _Cmd.Connection.Close();
        throw;
      }
			_Idx = -1;
    }

		public override void Close()
    {
      if (_Rdr == null)
        return;
      if (!_Rdr.IsClosed)
        _Rdr.Close();
      if (_Cmd.Connection.State == System.Data.ConnectionState.Open)
        _Cmd.Connection.Close();
			_Idx = -2;
			_RowCount = -1;
    }

		public override bool Read()
    {
      if (_Rdr == null)
        throw new NCsvLibDataSourceException("DbDataReader closed");
			if (!_Rdr.IsClosed)
			{
				bool b = _Rdr.Read();
				if (_Idx == -1)
					_Idx = 0;
				else
					_Idx++;
				return b;
			}
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

		public override bool Eof()
		{
			if (_Rdr == null)
				throw new NCsvLibDataSourceException("DbDataReader not defined");
			if (_Rdr.IsClosed)
				throw new NCsvLibDataSourceException("DataSource Record Reader closed");
			return (_Idx >= _RowCount);
		}
  }
}
