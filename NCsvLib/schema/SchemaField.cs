using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  public class SchemaField
  {
    private string _Name;
    /// <summary>
    /// Unique name of the field
    /// </summary>
    public string Name
    {
      get { return _Name; }
      set { _Name = value; }
    }

    private CsvFieldType _FldType;
    /// <summary>
    /// Data type of field value
    /// </summary>
    public CsvFieldType FldType
    {
      get { return _FldType; }
      set { _FldType = value; }
    }

    private string _Format;
    /// <summary>
    /// Format of the output string
    /// </summary>
    public string Format
    {
      get { return _Format; }
      set { _Format = value; }
    }

    private CsvValueAlignment _Alignment;
    /// <summary>
    /// Text alignment applied to the output value
    /// </summary>
    public CsvValueAlignment Alignment
    {
      get { return _Alignment; }
      set { _Alignment = value; }
    }

    private bool _FixedLen;
    /// <summary>
    /// True if the field has fixed length
    /// </summary>
    public bool FixedLen
    {
      get { return _FixedLen; }
      set { _FixedLen = value; }
    }

    private int _Size;
    /// <summary>
    /// Field size (only for fixed size fields)
    /// </summary>
    public int Size
    {
      get { return _Size; }
      set { _Size = value; }
    }

    private bool _AddQuotes;
    /// <summary>
    /// True if field value must be enclosed in quotes (")
    /// </summary>
    public bool AddQuotes
    {
      get { return _AddQuotes; }
      set { _AddQuotes = value; }
    }

    private bool _HasFixedValue;
    /// <summary>
    /// True if the field has a fixed value, represented by the FixedValue property.
    /// The fixed value has precedence over any other settings
    /// </summary>
    public bool HasFixedValue
    {
      get { return _HasFixedValue; }
      set { _HasFixedValue = value; }
    }

    private string _FixedValue;
    /// <summary>
    ///Fixed value, only considered if HasFixedValue is true
    /// </summary>
    public string FixedValue
    {
      get { return _FixedValue; }
      set { _FixedValue = value; }
    }
  }

  public enum CsvValueAlignment
  {
    Left = 1,
    Right = 2
  }

  public enum CsvFieldType
  {
    Int,
    String,
    Double,
    Decimal
  }
}
