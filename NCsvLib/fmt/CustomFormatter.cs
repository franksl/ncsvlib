using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib.Formatters
{
  /// <summary>
  /// Base class for custom formatters. It implements IFormatProvider and
  /// ICustomFormatter but with an abstract ICustomFormatter.Format method
  /// that must be implemented by derived classes to create custom string
  /// representations of data source values
  /// </summary>
  public abstract class CustomFormatter : IFormatProvider, ICustomFormatter
  {
    public object GetFormat(Type formatType)
    {
      if (formatType == typeof(ICustomFormatter))
        return this;
      else
        return System.Threading.Thread.CurrentThread.CurrentCulture.GetFormat(formatType);
    }

    public abstract string Format(string format, object arg, IFormatProvider formatProvider);
  }
}
