using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  public class CsvWriterController
  {
    IDataSourceReader InputRdr;
    ICsvOutputWriter OutWriter;
    ISchemaReader SchemaRdr;
    Schema Sch;

    public CsvWriterController()
    {
      InputRdr = null;
      OutWriter = null;
      SchemaRdr = null;
    }

    public void Execute()
    {
      if (InputRdr == null)
        throw new NCsvLibControllerException("Input reader not specified");
      if (OutWriter == null)
        throw new NCsvLibControllerException("Output writer not specified");
      if (SchemaRdr == null)
        throw new NCsvLibControllerException("Schema reader not specified");

      InputField infld;
      Sch = SchemaRdr.GetSchema();
      InputRdr.Open();
      while (InputRdr.Read())
      {
        foreach (SchemaField sc in Sch)
        {
          if (sc.HasFixedValue)
            infld = null;
          else
            infld = InputRdr.GetField(sc.Name);
          OutWriter.WriteField(infld, sc);
          OutWriter.WriteSeparator(Sch.Options.FieldSeparator);
        }
        OutWriter.WriteEol(Sch.Options.Eol);
      }
    }
  }
}
