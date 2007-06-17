using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  public interface IDataSourceReader
  {
    /// <summary>
    /// Opens a 'connection' with the data source and positions itself before the
    /// first record
    /// </summary>
    void Open();

    /// <summary>
    /// Closes te connection with the data source
    /// </summary>
    void Close();

    /// <summary>
    /// Reads next record. Analoguos of DbDataReader.Read()
    /// </summary>
    /// <returns>True if a new record is found, false if eof</returns>
    bool Read();

    /// <summary>
    /// Returns an object of class CsvInputField that contains info about the requested field
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    InputField GetField(string name);
  }
}
