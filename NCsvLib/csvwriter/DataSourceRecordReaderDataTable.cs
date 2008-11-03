using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace NCsvLib
{
  public class DataSourceRecordReaderDataTable : DataSourceRecordReaderBase
  {
    protected DataTable _Table;
    public DataTable Table
    {
      get { return _Table; }
      set { _Table = value; }
    }

		private int _Idx;

    private DataSourceRecordReaderDataTable()
    {
    }

		public DataSourceRecordReaderDataTable(string id, DataTable tbl)
    {
      _Id = id;
			_Table = tbl;
			//-2 = closed, -1 Open
			_Idx = -2;
    }

		public override void Open()
    {
			_Idx = -1;
    }

		public override void Close()
    {
			_Idx = -2;
    }

		public override bool Read()
    {
      if (_Idx == -2)
				throw new NCsvLibDataSourceException("DataSource Record Reader closed");
			if (_Idx == -1)
				_Idx = 0;
			else
				_Idx++;
			return (_Idx < _Table.Rows.Count);
    }

		public override DataSourceField GetField(string name)
    {
			if (_Idx == -2)
        throw new NCsvLibDataSourceException("DataSource Record Reader closed");
			object val = null;
			try
      {
				val = _Table.Rows[_Idx][name];
      }
      catch (ArgumentException)
      {
        throw new NCsvLibDataSourceException("Field name not found in db: " + name);
      }
      DataSourceField fld = new DataSourceField();
      fld.Name = name;
      //If value in db is null it is set to null
			if (val == DBNull.Value)
				fld.Value = null;
			else
				fld.Value = val;
      return fld;
    }

		public override bool Eof()
		{
			if (_Idx == -2)
				throw new NCsvLibDataSourceException("DataSource Record Reader closed");
			return (_Idx >= _Table.Rows.Count);
		}
  }
}
