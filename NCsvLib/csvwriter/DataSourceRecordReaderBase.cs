using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
	/// <summary>
	/// Abstract class that implements basic functionality of IDataSourceRecordReader
	/// </summary>
	public abstract class DataSourceRecordReaderBase : IDataSourceRecordReader
	{
		protected string _Id;
		public string Id
		{
			get
			{
				return _Id;
			}
		}

		public abstract void Open();

    public abstract void Close();

    public abstract bool Read();

		public abstract DataSourceField GetField(string name);

		public abstract bool Eof();
	}
}
