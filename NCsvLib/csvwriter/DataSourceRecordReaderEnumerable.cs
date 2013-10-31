using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace NCsvLib
{
    public class DataSourceRecordReaderEnumerable<T> : DataSourceRecordReaderBase
    {
        protected IEnumerable<T> _Coll;
        public IEnumerable<T> Coll
        {
            get { return _Coll; }
            set { _Coll = value; }
        }

        private Dictionary<string, PropertyInfo> _Pi;

        private IEnumerator<T> _Enum;
        private bool _IsEof;

        private DataSourceRecordReaderEnumerable()
        {
        }

        public DataSourceRecordReaderEnumerable(string id, 
            IEnumerable<T> coll)
        {
            _Id = id;
            _Coll = coll;
            _Pi = new Dictionary<string, PropertyInfo>();
            _Enum = null;
            _IsEof = false;
        }

        public override void Open()
        {
            _Enum = _Coll.GetEnumerator();
            _IsEof = false;
        }

        public override void Close()
        {
            _Enum = null;
        }

        public override bool Read()
        {
            if (_Enum == null)
                throw new NCsvLibDataSourceException("DataSource Record Reader closed");
            bool r = _Enum.MoveNext();
            if (!r)
                _IsEof = true;
            return r;
        }

        public override DataSourceField GetField(string name)
        {
            PropertyInfo pi;
            T it;

            if (_Enum == null)
                throw new NCsvLibDataSourceException("DataSource Record Reader closed");
            if (Eof())
                throw new NCsvLibDataSourceException("DataSource Record Reader reached eof");
            object val = null;
            try
            {
                it = _Enum.Current;
                if (_Pi.ContainsKey(name))
                    pi = _Pi[name];
                else
                {
                    pi = typeof(T).GetProperty(name);
                    if (pi == null)
                    {
                        throw new NCsvLibDataSourceException("Property "
                            + name + " not found");
                    }
                    _Pi[name] = pi;
                }
                val = pi.GetValue(it, null);
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
            if (_Enum == null)
                throw new NCsvLibDataSourceException("DataSource Record Reader closed");
            return _IsEof;
        }
    }
}
