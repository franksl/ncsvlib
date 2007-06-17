using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  public interface ICsvOutputWriter
  {
    /// <summary>
    /// Opens the output writer
    /// </summary>
    void Open();
    /// <summary>
    /// Closes the output writer
    /// </summary>
    void Close();
    /// <summary>
    /// Writes fld to the output stream
    /// </summary>
    /// <param name="fld">Field containing the value to be written</param>
    /// <param name="sch"></param>
    void WriteField(InputField fld, SchemaField sch);
    /// <summary>
    /// Writes the field separator
    /// </summary>
    void WriteSeparator(string sep);
    /// <summary>
    /// Writes the end of line character(s)
    /// </summary>
    void WriteEol(string sEol);
  }
}
