using System;
using System.Collections.Generic;
using System.Text;

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

    public CsvWriterController()
    {
      _InputRdr = null;
      _OutWriter = null;
      _SchemaRdr = null;
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
        //If the output writer derives from CsvOutputWriterBase sets the options
        if (_OutWriter is CsvOutputWriterBase)
        {
          ((CsvOutputWriterBase)_OutWriter).Enc = _Sch.Options.Enc;
          ((CsvOutputWriterBase)_OutWriter).Quotes = _Sch.Options.Quotes;
        }
                
        _OutWriter.Open();
        SchemaRecordBase.ExecuteMethodDelegate em = null;
        em += new SchemaRecordBase.ExecuteMethodDelegate(this.ExecuteRecordMethod);
        _Sch.Execute(em);       
      }
      catch (Exception Ex)
      {
        throw new NCsvLibControllerException(Ex.Message);
      }
      finally
      {
        //if (_InputRdr != null)
        //  _InputRdr.Close();
        if (_OutWriter != null)
          _OutWriter.Close();
      }
    }

    public void ExecuteRecordMethod(SchemaRecordBase rec)
    {
      if (!(rec is SchemaRecord))
        return;
      SchemaRecord r = (SchemaRecord)rec;
      DataSourceField infld;

      _InputRdr.Open(r.Id);
      try
      {
        while (_InputRdr.Read(r.Id)) //TODO Implement repeat here!!
        {
          for (int i = 0; i < r.Count; i++)
          {
            if (r[i].HasFixedValue)
              infld = null;
            else
              infld = _InputRdr.GetField(r.Id, r[i].Name);
            _OutWriter.WriteField(infld, r[i]);
            _OutWriter.WriteSeparator(_Sch.Options.FieldSeparator);
          }
          _OutWriter.WriteEol(_Sch.Options.Eol);
        }
      }
      finally
      {
        _InputRdr.Close(r.Id);
      }
    }
  }
}
