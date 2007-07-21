using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  public interface IDataSourceReader : IDictionary<string, IDataSourceRecordReader>
  {
  }
}
