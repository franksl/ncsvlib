using System;
using System.Collections.Generic;
using System.Text;
using NCsvLib.Formatters;

namespace NCsvLib
{
  public class CsvWriterController
  {
    private IDataSourceReader _InputRdr;
    public IDataSourceReader InputRdr
    {
      get { return _InputRdr; }
      set { _InputRdr = value; }
    }
    
    private ICsvOutputWriter _OutWriter;
    public ICsvOutputWriter OutWriter
    {
      get { return _OutWriter; }
      set { _OutWriter = value; }
    }

    private ISchemaReader _SchemaRdr;
    public ISchemaReader SchemaRdr
    {
      get { return _SchemaRdr; }
      set { _SchemaRdr = value; }
    }
    
    private Schema _Sch;

    /// <summary>
    /// Default formatter, used if no custom formatter is specified for a particular
    /// field. Stored with controller for performance reasons, to keep a single
    /// instance
    /// </summary>
    private FormatterBase _Fmt;

    public CsvWriterController()
    {
      _InputRdr = null;
      _OutWriter = null;
      _SchemaRdr = null;
      _Fmt = new FormatterBase();
    }

    public void Execute()
    {
      if (_InputRdr == null)
        throw new NCsvLibControllerException("Input reader not specified");
      if (_OutWriter == null)
        throw new NCsvLibControllerException("Output writer not specified");      
      if (_SchemaRdr == null)
        throw new NCsvLibControllerException("Schema reader not specified");
      
      try
      {        
        _Sch = _SchemaRdr.GetSchema();
        //Sets schema on default formatter (it uses encoding, quotes, etc.)
        _Fmt.Sch = _Sch;
        //If the output writer derives from CsvOutputWriterBase sets the options
        if (_OutWriter is CsvOutputWriterBase)
        {
          ((CsvOutputWriterBase)_OutWriter).Enc = _Sch.Options.Enc;
          //((CsvOutputWriterBase)_OutWriter).Quotes = _Sch.Options.Quotes;
        }
                
        _OutWriter.Open();
        SchemaRecordBase.ExecuteMethodDelegate em = null;
        em += new SchemaRecordBase.ExecuteMethodDelegate(this.ExecuteRecordMethod);
        _InputRdr.OpenAll();
        _Sch.Execute(em);
      }
      catch (Exception Ex)
      {
        throw new NCsvLibControllerException(Ex.Message);
      }
      finally
      {
        if (_OutWriter != null)
          _OutWriter.Close();
        if (_InputRdr != null)
          _InputRdr.CloseAll();
      }
    }

    public void ExecuteRecordMethod(SchemaRecordBase rec)
    {
      if (!(rec is SchemaRecord))
        return;
      SchemaRecord r = (SchemaRecord)rec;
      IDataSourceRecordReader rdr;
      if (!_InputRdr.TryGetValue(r.Id, out rdr))
        throw new NCsvLibControllerException("DataSource Reader not found for id = " + r.Id);
      //rdr.Open();
      try
      {
        //Outputs column headers, if requested
        if (r.ColHeaders)
          _OutWriter.WriteColHeaders(r, _Sch.Options.FieldSeparator, _Sch.Options.Eol);

        if (rec.Repeat == 0)
        {
          while (rdr.Read())
          {
            WriteRecord(r, rdr);
          }
        }
        else
        {
          for (int i = 0; i < rec.Repeat; i++)
          {
            if (!rdr.Read())
              break;
            WriteRecord(r, rdr);
          }
        }
      }
      finally
      {
        //rdr.Close();
      }
    }

    private void WriteRecord(SchemaRecord r, IDataSourceRecordReader rdr)
    {
      DataSourceField infld;
      string s;

      for (int i = 0; i < r.Count; i++)
      {
        if (r[i].HasFixedValue)
          infld = null;
        else
          infld = rdr.GetField(r[i].Name);
        //Uses base formatter if no custom formatter is specified
        if (r[i].CustFmt != null)
          s = r[i].CustFmt.Format(infld, r[i]);
        else
          s = _Fmt.Format(infld, r[i]);
        _OutWriter.WriteFieldValue(s);
        _OutWriter.WriteSeparator(_Sch.Options.FieldSeparator);
      }
      _OutWriter.WriteEol(_Sch.Options.Eol);
    }
  }
}
