using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using NCsvLib.Formatters;

namespace NCsvLib
{
	public class SchemaReaderXml : ISchemaReader
	{
		private string _FileName;
		private Stream _Str;
		private XmlReader _Rdr;
		private Schema _Sch;
		public Schema Sch
		{
			get
			{
				//If schema has not been already read, read it
				if (_Sch == null)
					ReadSchema();
				return _Sch;
			}
		}

		private SchemaReaderXml()
		{
			_FileName = null;
			_Str = null;
			_Sch = null;
		}

		public SchemaReaderXml(string file_name)
			: this()
		{
			if (!File.Exists(file_name))
				throw new NCsvLibSchemaException("Xml schema file not found: " + file_name);
			_FileName = file_name;
		}

		public SchemaReaderXml(Stream str)
			: this()
		{
			if (str == null)
				throw new NCsvLibSchemaException("Stream is null");
			if (!str.CanRead)
				throw new NCsvLibSchemaException("Cannot read from stream");
			_Str = str;
		}

		public void ReadSchema()
		{
			_Sch = new Schema();

			if (_FileName != null)
				_Rdr = XmlReader.Create(_FileName);
			else if (_Str != null)
				_Rdr = XmlReader.Create(_Str);
			else
				throw new NCsvLibSchemaException("Missing input file name or stream");
			_Rdr.ReadStartElement("ncsvlib");
			try
			{
				while (_Rdr.Read())
				{
					if (_Rdr.IsStartElement("options"))
					{
						try
						{
							ReadOptions();
						}
						catch (Exception ex)
						{
							throw new NCsvLibSchemaException("Error while reading schema options."
								+ Environment.NewLine + ex.Message);
						}
					}
					else if (_Rdr.IsStartElement("schema"))
					{
						try
						{
							ReadRecords();
						}
						catch (Exception ex)
						{
							throw new NCsvLibSchemaException("Error while reading schema records."
								+ Environment.NewLine + ex.Message);
						}
					}
				}
			}
			finally
			{
				_Rdr.Close();
			}
		}

		private void ReadOptions()
		{
			string s;
			while (_Rdr.Read() && _Rdr.MoveToContent() != XmlNodeType.EndElement &&
						_Rdr.Name != "options")
			{
				if (_Rdr.IsStartElement("fieldseparator"))
				{
					s = _Rdr.GetAttribute("usedefault");
					if (s != null && s.ToLower().Trim() == "false")
					{
						s = _Rdr.GetAttribute("value");
						if (s != null)
							_Sch.Options.FieldSeparator = s;
					}
				}
				else if (_Rdr.IsStartElement("eol"))
				{
					s = _Rdr.GetAttribute("usedefault");
					if (s != null && s.ToLower().Trim() == "false")
					{
						s = _Rdr.GetAttribute("value");
						if (s != null)
							_Sch.Options.Eol = s;
					}
				}
				else if (_Rdr.IsStartElement("quotes"))
				{
					s = _Rdr.GetAttribute("usedefault");
					if (s != null && s.ToLower().Trim() == "false")
					{
						s = _Rdr.GetAttribute("value");
						if (s != null)
							_Sch.Options.Quotes = s;
					}
				}
				else if (_Rdr.IsStartElement("encoding"))
				{
					s = _Rdr.GetAttribute("value");
					if (s != null)
						_Sch.Options.Enc = Encoding.GetEncoding(s);
				}
			}
		}

		private void ReadRecords()
		{
			SchemaRecordBase rec = null;
			Stack<SchemaRecordComposite> stk = new Stack<SchemaRecordComposite>();

			while (_Rdr.Read() && _Rdr.MoveToContent() != XmlNodeType.EndElement &&
						_Rdr.Name != "schema")
			{
				if (_Rdr.IsStartElement("recordgroup"))
				{
					rec = new SchemaRecordComposite();
					//rec.Id = Rdr.GetAttribute("id");
					stk.Push((SchemaRecordComposite)rec);
					ReadRecordGroup(stk);
					_Sch.Add(rec);
				}
				else if (_Rdr.IsStartElement("record"))
				{
					SchemaRecord r = new SchemaRecord();
					ReadRecord(r);
					_Sch.Add(r);
				}
			}
			if (_Rdr.MoveToContent() == XmlNodeType.EndElement && _Rdr.Name == "schema")
				_Rdr.ReadEndElement();
		}

		private void ReadRecordGroup(Stack<SchemaRecordComposite> stk)
		{
			SchemaRecordComposite comp = stk.Peek();
			comp.Id = _Rdr.GetAttribute("id");
			//SchemaRecordLimit lim = ParseLimit(_Rdr.GetAttribute("limit"));
			//comp.Limit = new SchemaRecordLimit(lim.Offset, lim.Max);
			comp.Limit = ParseLimit(_Rdr.GetAttribute("limit"));
			if (comp.Id == null || comp.Id.Trim() == string.Empty)
				throw new NCsvLibSchemaException("id not specified in recordgroup");
			while (_Rdr.Read() && _Rdr.MoveToContent() != XmlNodeType.EndElement &&
						_Rdr.Name != "recordgroup")
			{
				if (_Rdr.IsStartElement("record"))
				{
					SchemaRecord r = new SchemaRecord();
					ReadRecord(r);
					stk.Peek().Add(r);
				}
				else if (_Rdr.IsStartElement("recordgroup"))
				{
					comp = new SchemaRecordComposite();
					stk.Peek().Add(comp);
					stk.Push(comp);
					ReadRecordGroup(stk);
				}
			}
			if (_Rdr.MoveToContent() == XmlNodeType.EndElement && _Rdr.Name == "recordgroup")
				_Rdr.ReadEndElement();
			stk.Pop();
		}

		private void ReadRecord(SchemaRecord rec)
		{
			string s;
			rec.Id = _Rdr.GetAttribute("id");
			//SchemaRecordLimit lim = ParseLimit(_Rdr.GetAttribute("limit"));
			//rec.Limit = new SchemaRecordLimit(lim.Offset, lim.Max);
			rec.Limit = ParseLimit(_Rdr.GetAttribute("limit"));
			s = _Rdr.GetAttribute("colheaders");
			if (s != null && s != string.Empty)
			{
				try
				{
					rec.ColHeaders = bool.Parse(s);
				}
				catch
				{
					throw new NCsvLibSchemaException("Error reading record.colheaders");
				}
			}
			if (rec.Id == null || rec.Id.Trim() == string.Empty)
				throw new NCsvLibSchemaException("id not specified in record");
			while (_Rdr.Read() && _Rdr.MoveToContent() != XmlNodeType.EndElement &&
						_Rdr.Name != "record")
			{
				if (_Rdr.IsStartElement("field"))
				{
					rec.AddField(ReadField());
				}
			}
			if (_Rdr.NodeType == XmlNodeType.EndElement && _Rdr.Name == "record")
				_Rdr.ReadEndElement();
		}

		private SchemaField ReadField()
    {
      SchemaField fld;
      string s;
      int ival;

      fld = new SchemaField(_Sch);
      //Name
      fld.Name = _Rdr.GetAttribute("name");
      //FldType
      s = _Rdr.GetAttribute("type");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "int":
          fld.FldType = SchemaFieldType.Int;
          break;
        case "string":
          fld.FldType = SchemaFieldType.String;
          break;
        case "double":
          fld.FldType = SchemaFieldType.Double;
          break;
        case "decimal":
          fld.FldType = SchemaFieldType.Decimal;
          break;
        case "datetime":
          fld.FldType = SchemaFieldType.DateTime;
          break;
        case "bool":
          fld.FldType = SchemaFieldType.Bool;
          break;
      }
      //Format
      fld.Format = _Rdr.GetAttribute("format");
      //CustFmt (uses Activator to create an ICustomFormatter instance)
      s = _Rdr.GetAttribute("custfmt");
      if (s != null)
      {
        char sep = '|';
        try
        {
          if (s.IndexOf(sep) >= 0)
          {
            string[] sarr = s.Split(new char[] { sep }, 2);
            fld.CustFmt = (ICsvOutputFormatter)Activator.CreateInstanceFrom(sarr[0], sarr[1]).Unwrap();            
          }
          else
            fld.CustFmt = (ICsvOutputFormatter)Activator.CreateInstance(Type.GetType(s, true, true));

          //fld.CustFmt = (IFormatProvider)Activator.CreateInstance(Type.GetType(s, true, true));
          //fld.CustFmt = (IFormatProvider)AppDomain.CurrentDomain.CreateInstanceAndUnwrap(
          //fld.CustFmt = (IFormatProvider)Activator.CreateInstance(Type.GetType(s, true, true));
          
          /*if (s.ToLower().StartsWith("ncsvlib."))
            fld.CustFmt = (IFormatProvider)Activator.CreateInstance(Type.GetType(s, true, true));
          else
            fld.CustFmt = (IFormatProvider)System.Reflection.Assembly.GetEntryAssembly().CreateInstance(s, true);*/
        }
        catch (Exception ex)
        {
          throw new NCsvLibSchemaException("Invalid custom formatter: " + s + " " + ex.Message);
        }
      }
      //Alignment
      s = _Rdr.GetAttribute("alignment");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "left":
        case null:
          fld.Alignment = SchemaValueAlignment.Left;
          break;
        case "right":
          fld.Alignment = SchemaValueAlignment.Right;
          break;
      }
      //FixedSize
      s = _Rdr.GetAttribute("fixedsize");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "true":
          fld.FixedSize = true;
          break;
        case "false":
        case null:
          fld.FixedSize = false;
          break;
      }
      //Size
      s = _Rdr.GetAttribute("size");
      if (s != null)
        fld.Size = XmlConvert.ToInt32(s);
      else
        fld.Size = 0;
      //Filled
      s = _Rdr.GetAttribute("filled");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "true":
          fld.Filled = true;
          break;
        case "false":
        case null:
          fld.Filled = false;
          break;
      }
      //FillChar
      s = _Rdr.GetAttribute("fillchar");
      if (s == null || s.Length == 0)
        fld.FillChar = ' ';
      else
        fld.FillChar = s[0];
      //AddQuotes
      s = _Rdr.GetAttribute("addquotes");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "true":
          fld.AddQuotes = true;
          break;
        case "false":
        case null:
          fld.AddQuotes = false;
          break;
      }
      //Quotes (if not defined it will be inherited from schema options)
      //TODO only first char is used, could be more than one char
      s = _Rdr.GetAttribute("quotes");
      if (s != null)
        fld.Quotes = s[0].ToString();
      //HasFixedValue
      s = _Rdr.GetAttribute("hasfixedvalue");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "true":
          fld.HasFixedValue = true;
          break;
        case "false":
        case null:
          fld.HasFixedValue = false;
          break;
      }
      //FixedValue
      s = _Rdr.GetAttribute("fixedvalue");
      if (s != null)
        fld.FixedValue = s;
      //Comment
      s = _Rdr.GetAttribute("comment");
      if (s != null)
        fld.Comment = s;
      //ColHdr
      s = _Rdr.GetAttribute("colhdr");
      if (s != null)
        fld.ColHdr = s;
      //TrueValue
      s = _Rdr.GetAttribute("truevalue");
      if (s != null)
        fld.BoolSettings.TrueValue = s;
      //FalseValue
      s = _Rdr.GetAttribute("falsevalue");
      if (s != null)
        fld.BoolSettings.FalseValue = s;
      //BoolIoType
      s = _Rdr.GetAttribute("booliotype");
      if (s != null)
        s = s.ToLower();
      switch (s)
      {
        case "int":
          fld.BoolSettings.BoolIoType = SchemaFieldBoolIoType.Int;
          break;
        case "string":
          fld.BoolSettings.BoolIoType = SchemaFieldBoolIoType.String;
          break;
				case "bool":
					fld.BoolSettings.BoolIoType = SchemaFieldBoolIoType.Bool;
					break;
        case null:
          break;
        default:
          throw new NCsvLibSchemaException("Invalid value for booliotype");
      }
      //TrueIoValue
      s = _Rdr.GetAttribute("trueiovalue");
      if (s != null)
      {
        switch (fld.BoolSettings.BoolIoType)
        {
          case SchemaFieldBoolIoType.Int:
            if (int.TryParse(s, out ival))
              fld.BoolSettings.TrueIoValue = ival;
            else
              throw new NCsvLibSchemaException("Invalid value for trueiovalue: " + s);
            break;
          case SchemaFieldBoolIoType.String:
            fld.BoolSettings.TrueIoValue = s;
            break;
					case SchemaFieldBoolIoType.Bool:
						bool bval;
						if (bool.TryParse(s, out bval))
							fld.BoolSettings.TrueIoValue = bval;
						else
							throw new NCsvLibSchemaException("Invalid value for trueiovalue: " + s);
						break;
        }
      }
      //FalseIoValue
      s = _Rdr.GetAttribute("falseiovalue");
      if (s != null)
      {
        switch (fld.BoolSettings.BoolIoType)
        {
          case SchemaFieldBoolIoType.Int:
            if (int.TryParse(s, out ival))
              fld.BoolSettings.FalseIoValue = ival;
            else
              throw new NCsvLibSchemaException("Invalid value for falseiovalue: " + s);
            break;
          case SchemaFieldBoolIoType.String:
            fld.BoolSettings.FalseIoValue = s;
            break;
					case SchemaFieldBoolIoType.Bool:
						bool bval;
						if (bool.TryParse(s, out bval))
							fld.BoolSettings.FalseIoValue = bval;
						else
							throw new NCsvLibSchemaException("Invalid value for falseiovalue: " + s);
						break;
        }
      }
			//NullValueWrt
			s = _Rdr.GetAttribute("nullvaluewrt");
			if (s != null)
				fld.NullValueWrt = s;
      return fld;
    }

		private SchemaRecordLimit ParseLimit(string attr)
		{
			SchemaRecordLimit lim = new SchemaRecordLimit();
			if (attr == null || attr == string.Empty)
				return lim;
			//Two arguments
			if (attr.IndexOf(",") >= 0)
			{
				//Tries splitting and converting (0 => offset, 1 => max)
				string[] arr = attr.Split(new char[] { ',' }, 2);
				if (arr.Length != 2)
					return lim;
				if (!int.TryParse(arr[0], out lim.Offset))
					return lim;
				if (!int.TryParse(arr[1], out lim.Max))
				{
					lim.Offset = 0;
					return lim;
				}
				return lim;
			}
			else
			{
				int.TryParse(attr, out lim.Max);
				return lim;
			}
		}
	}
}
