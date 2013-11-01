using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib.Formatters
{
  /// <summary>
  /// Interface to be implemented by classes that convert a DataSourceField
  /// to a string to be put on the output in csv format.
  /// </summary>
  public interface ICsvOutputFormatter
  {
    /// <summary>
    /// Returns a string with the value converted to csv format
    /// </summary>
    /// <param name="fld"></param>
    /// <param name="sch"></param>
    /// <returns></returns>
    string Format(DataSourceField fld, SchemaField sch);
  }
}
