using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
    public abstract class CsvOutputWriterBase : ICsvOutputWriter
    {
        protected Encoding _Enc;
        public Encoding Enc
        {
            get { return _Enc; }
            set { _Enc = value; }
        }

        public CsvOutputWriterBase()
        {
        }

        public CsvOutputWriterBase(Encoding enc)
            : this()
        {
            _Enc = enc;
        }

        protected virtual string PrepareColHeaders(SchemaRecord rec, SchemaOptions opt)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < rec.Count; i++)
            {
                if (rec[i].ColHdr != string.Empty)
                    sb.Append(rec[i].ColHdr);
                sb.Append(opt.FieldSeparator);
            }
            //If requested removes last field separator
            if (!opt.LastFieldSeparator)
                sb.Length = sb.Length - 1;
            sb.Append(opt.Eol);
            return sb.ToString();
        }

        public abstract void Open();
        public abstract void Close();
        public abstract void WriteFieldValue(string val);
        public abstract void WriteSeparator(string sep);
        public abstract void WriteEol(string sEol);
        public abstract void WriteColHeaders(SchemaRecord rec, SchemaOptions opt);
    }
}
