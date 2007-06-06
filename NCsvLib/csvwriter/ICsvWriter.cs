using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  interface ICsvWriter
  {
    /// <summary>
    /// Writes fld to the output stream
    /// </summary>
    /// <param name="fld">Field containing the value to be written</param>
    /// <param name="sch"></param>
    void WriteField(InputField fld, SchemaField sch);
    /// <summary>
    /// Writes the field separator
    /// </summary>
    void WriteSeparator();
    /// <summary>
    /// Writes the end of line character(s)
    /// </summary>
    void WriteEol();
  }
}
