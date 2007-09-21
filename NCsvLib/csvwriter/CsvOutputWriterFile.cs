using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NCsvLib
{
  public class CsvOutputWriterFile : CsvOutputWriterStream
  {
    public CsvOutputWriterFile(string fileName)
      : base(new FileStream(fileName, FileMode.Create))
    {
    }
  }
}
