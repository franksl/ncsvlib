using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  public interface IDataSourceRecordReader
  {
    /// <summary>
    /// Id of this record
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Opens a 'connection' with the data source for the given record id
    /// and positions itself before the first row
    /// </summary>
    void Open();

    /// <summary>
    /// Closes connection with the data source for the given record id
    /// </summary>
    void Close();

    /// <summary>
    /// Reads next row for the given record id. Analoguos of DbDataReader.Read()
    /// </summary>
    /// <returns>True if a new record is found, false if eof</returns>
    bool Read();

    /// <summary>
    /// Returns an object of class CsvInputField that contains info about the requested field
    /// </summary>
    /// <param name="name">Name of the requested field</param>
    /// <returns></returns>
    DataSourceField GetField(string name);
  }
}
