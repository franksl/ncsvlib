using System;
using System.Collections.Generic;
using System.Text;
using NCsvLib.Formatters;

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

    private ICsvOutputFormatter _CustFmt;
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
    public ICsvOutputFormatter CustFmt
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

    private bool _FixedSize;
    /// <summary>
    /// True if the field has fixed length
    /// </summary>
    public bool FixedSize
    {
      get { return _FixedSize; }
      set { _FixedSize = value; }
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
    /// Valid only if FixedSize is true.
    /// </summary>
    public bool Filled
    {
      get { return _Filled; }
      set { _Filled = value; }
    }

    private char _FillChar;
    /// <summary>
    /// Character used to fill empty spaces for the field.
    /// Valid only if FixedSize and Filled are true.
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

    private string _Quotes;
    /// <summary>
    /// Character to be used if field value is to be quoted. If not defined
    /// it will be inherited from schema options
    /// </summary>
    public string Quotes
    {
      get 
      {
        if (_Quotes == null)
          return _Sch.Options.Quotes;
        else
          return _Quotes; 
      }
      set { _Quotes = value; }
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
    ///Fixed value, only considered if HasFixedValue is true.
    ///If FixedSize is specified this value must have the same length as
    ///FixedSize, otherwise it must have the Filled property set to true
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

    private string _ColHdr;
    /// <summary>
    ///Text for the column header (if Record.ColHeaders is true)
    /// </summary>
    public string ColHdr
    {
      get { return _ColHdr; }
      set { _ColHdr = value; }
    }

    private SchemaFieldBool _BoolSettings;
    /// <summary>
    /// Contains settings for bool fields
    /// </summary>
    public SchemaFieldBool BoolSettings
    {
      get
      {
        if (_BoolSettings == null)
          _BoolSettings = new SchemaFieldBool();
        return _BoolSettings; 
      }
    }

		private string _NullValueWrt;
		/// <summary>
		/// Writer: value to be assigned when data source field value is null
		/// Default: empty string
		/// </summary>
		public string NullValueWrt
		{
			get { return _NullValueWrt; }
			set { _NullValueWrt = value; }
		}
    
    private Schema _Sch;
    /// <summary>
    /// Schema that owns this field
    /// </summary>
    public Schema Sch
    {
      get { return _Sch; }
    }

    private SchemaField()
    {
    }

    public SchemaField(Schema sch)
    {
      _Sch = sch;
      Name = string.Empty;
      FldType = SchemaFieldType.String;
      Format = string.Empty;
      CustFmt = null;
      Alignment = SchemaValueAlignment.Left;
      FixedSize = false;
      Size = 0;
      Filled = false;
      FillChar = char.MinValue;
      AddQuotes = false;
      _Quotes = null;
      HasFixedValue = false;
      FixedValue = string.Empty;
      Comment = string.Empty;
      ColHdr = string.Empty;
      _BoolSettings = null;
			_NullValueWrt = string.Empty;
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
    DateTime = 5,
    Bool = 6
  }
}
