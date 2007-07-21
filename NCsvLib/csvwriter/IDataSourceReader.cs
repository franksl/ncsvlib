using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  public interface IDataSourceReader : IDictionary<string, IDataSourceRecordReader>
  {
    /// <summary>
    /// Opens all the contained record readers
    /// </summary>
    void OpenAll();
    /// <summary>
    /// Closes all the contained record readers
    /// </summary>
    void CloseAll();
  }
}
