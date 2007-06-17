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
      
      InputField infld;
      try
      {        
        _Sch = _SchemaRdr.GetSchema();
        //If the output writer derives from CsvOutputWriterBase sets the options
        if (_OutWriter is CsvOutputWriterBase)
        {
          ((CsvOutputWriterBase)_OutWriter).Enc = _Sch.Options.Enc;
          ((CsvOutputWriterBase)_OutWriter).Quotes = _Sch.Options.Quotes;
        }
        _InputRdr.Open();
        _OutWriter.Open();

        while (_InputRdr.Read())
        {
          foreach (SchemaField sc in _Sch)
          {
            if (sc.HasFixedValue)
              infld = null;
            else
              infld = _InputRdr.GetField(sc.Name);
            _OutWriter.WriteField(infld, sc);
            _OutWriter.WriteSeparator(_Sch.Options.FieldSeparator);
          }
          _OutWriter.WriteEol(_Sch.Options.Eol);
        }
      }
      catch (Exception Ex)
      {
        throw new NCsvLibControllerException(Ex.Message);
      }
      finally
      {
        if (_InputRdr != null)
          _InputRdr.Close();
        if (_OutWriter != null)
          _OutWriter.Close();
      }
    }
  }
}
