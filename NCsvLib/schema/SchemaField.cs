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

    private SchemaFieldType _FldType;
    /// <summary>
    /// Data type of field value
    /// </summary>
    public SchemaFieldType FldType
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

    private IFormatProvider _CustFmt;
    /// <summary>
    /// Custom formatter to be eventually used while converting values to
    /// strings. It has precedence over Format.
    /// This value can be specified in two ways:
    /// - If it is one of the builtin custom formatters (like NumberDigitsFormatter)
    ///   the attribute should be the full type name 
    ///   (ie. "NCsvLib.Formatters.NumberDigitsFormatter")
    /// - If it is a user custom formatter the attribute must contain the assembly
    ///   name as long as the full type name, separated by a '|' (pipe char).
    ///   For example if the user has defined a MyNamespace.Formatters.MyFormatter
    ///   class (that is a subclass of NCsvLib.Formatters.CustomFormatter) in
    ///   the assembly MyAssembly.dll, the custfmt attribute should be:
    ///   custfmt="MyAssembly.dll|MyNamespace.Formatters.MyFormatter"
    /// </summary>
    public IFormatProvider CustFmt
    {
      get { return _CustFmt; }
      set { _CustFmt = value; }
    }

    private SchemaValueAlignment _Alignment;
    /// <summary>
    /// Text alignment applied to the output value
    /// </summary>
    public SchemaValueAlignment Alignment
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
    /// Field size, excluding quotes (only for fixed size fields)
    /// </summary>
    public int Size
    {
      get { return _Size; }
      set { _Size = value; }
    }

    private bool _Filled;
    /// <summary>
    /// True if empty space for the field must be filled with FillChar.
    /// Valid only if FixedLen is true.
    /// </summary>
    public bool Filled
    {
      get { return _Filled; }
      set { _Filled = value; }
    }

    private char _FillChar;
    /// <summary>
    /// Character used to fill empty spaces for the field.
    /// Valid only if FixedLen and Filled are true.
    /// </summary>
    public char FillChar
    {
      get { return _FillChar; }
      set { _FillChar = value; }
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

    private string _Comment;
    /// <summary>
    ///Free comment for this field
    /// </summary>
    public string Comment
    {
      get { return _Comment; }
      set { _Comment = value; }
    }

    public SchemaField()
    {
      Name = string.Empty;
      FldType = SchemaFieldType.String;
      Format = string.Empty;
      CustFmt = null;
      Alignment = SchemaValueAlignment.Left;
      FixedLen = false;
      Size = 0;
      Filled = false;
      FillChar = char.MinValue;
      AddQuotes = false;
      HasFixedValue = false;
      FixedValue = string.Empty;
      Comment = string.Empty;
    }
  }

  public enum SchemaValueAlignment
  {
    Left = 1,
    Right = 2
  }

  public enum SchemaFieldType
  {
    Int = 1,
    String = 2,
    Double = 3,
    Decimal = 4,
    DateTime = 5
  }
}
