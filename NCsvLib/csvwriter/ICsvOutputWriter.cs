using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  public interface ICsvOutputWriter
  {
    Encoding Enc { get; set; }
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
    void WriteFieldValue(string value);
    /// <summary>
    /// Writes the field separator
    /// </summary>
    void WriteSeparator(string sep);
    /// <summary>
    /// Writes the end of line character(s)
    /// </summary>
    void WriteEol(string sEol);
    /// <summary>
    /// Writes column headers on a single line
    /// </summary>
    /// <param name="rec">Record that contains fields with colhdr attribute defined</param>
    /// <param name="sep">Field separator</param>
    void WriteColHeaders(SchemaRecord rec, SchemaOptions opt);
  }
}
