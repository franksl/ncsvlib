using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  public interface IDataSourceReader
  {
    /// <summary>
    /// Opens a 'connection' with the data source for the given record id
    /// and positions itself before the first row
    /// <param name="id">Id of the requested record</param>
    /// </summary>
    void Open(string id);

    /// <summary>
    /// Closes connection with the data source for the given record id
    /// </summary>
    ///<param name="id">Id of the requested record</param>
    void Close(string id);

    /// <summary>
    /// Reads next row for the given record id. Analoguos of DbDataReader.Read()
    /// </summary>
    /// <param name="id">Id of the requested record</param>
    /// <returns>True if a new record is found, false if eof</returns>
    bool Read(string id);

    /// <summary>
    /// Returns an object of class CsvInputField that contains info about the requested field
    /// </summary>
    /// <param name="id">Id of the requested record</param>
    /// <param name="name">Name of the requested field</param>
    /// <returns></returns>
    DataSourceField GetField(string id, string name);
  }
}
