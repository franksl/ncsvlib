using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  /// <summary>
  /// Contains parameters for bool fields
  /// </summary>
  public class SchemaFieldBool
  {
    private string _TrueValue;
    /// <summary>
    /// String representing the value 'true' (used only for bool type) in csv
    /// stream.
    /// Default value: "true"
    /// </summary>
    public string TrueValue
    {
      get { return _TrueValue; }
      set { _TrueValue = value; }
    }

    private string _FalseValue;
    /// <summary>
    /// String representing the value 'false' (used only for bool type) in csv
    /// stream.
    /// Default value: "false"
    /// </summary>
    public string FalseValue
    {
      get { return _FalseValue; }
      set { _FalseValue = value; }
    }

    private SchemaFieldBoolIoType _BoolIoType;
    /// <summary>
    /// Type used when reading from data source or writing to data destination
    /// Default: typeof(int)
    /// </summary>
    public SchemaFieldBoolIoType BoolIoType
    {
      get { return _BoolIoType; }
      set { _BoolIoType = value; }
    }

    private object _TrueIoValue;
    /// <summary>
    /// Represents the value 'true' (used only for bool type) in data source
    /// (writing) and data destination (reading)
    /// Default value: 1 (int)
    /// </summary>
    public object TrueIoValue
    {
      get { return _TrueIoValue; }
      set { _TrueIoValue = value; }
    }

    private object _FalseIoValue;
    /// <summary>
    /// Represents the value 'false' (used only for bool type) in data source
    /// (writing) and data destination (reading)
    /// Default value: 0 (int)
    /// </summary>
    public object FalseIoValue
    {
      get { return _FalseIoValue; }
      set { _FalseIoValue = value; }
    }

    /// <summary>
    /// Compares given value with TrueIoValue
    /// </summary>
    /// <param name="val"></param>
    /// <returns>True if val equals TrueIoValue, false if val equals FalseIoValue</returns>
    public bool CompareIoValue(object val)
    {
      bool b = CompareIoValueInternal(val, TrueIoValue);
      if (b)
        return true;
      else
      {
        b = CompareIoValueInternal(val, FalseIoValue);
        if (!b)
          throw new NCsvLibSchemaException("Bool compare error");
        else
          return false;
      }
    }

    private bool CompareIoValueInternal(object val1, object val2)
    {
      switch (BoolIoType)
      {
        case SchemaFieldBoolIoType.Int:
          return (int)val1 == (int)val2;
        case SchemaFieldBoolIoType.String:
          return (string.Compare(val1.ToString(), val2.ToString()) == 0);
				case SchemaFieldBoolIoType.Bool:
					return (bool)val1 == (bool)val2;
        default:
          throw new NCsvLibSchemaException("Bool types mismatch");
      }
    }

    public SchemaFieldBool()
    {
      TrueValue = "true";
      FalseValue = "false";
      BoolIoType = SchemaFieldBoolIoType.Int;
      TrueIoValue = (int)1;
      FalseIoValue = (int)0;
    }
  }

  public enum SchemaFieldBoolIoType
  {
    Int = 1,
    String = 2,
		Bool = 3
  }
}
