using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  /// <summary>
  /// Represents the 'Component' base class in Composite pattern
  /// </summary>
  public class SchemaRecordBase
  {
    protected string _Id;
    /// <summary>
    /// Unique record identifier
    /// </summary>
    public string Id
    {
      get { return _Id; }
      set { _Id = value; }
    }

    protected SchemaRecordLimit _Limit;
    /// <summary>
    /// Number of times this record is repeated and offset
    /// If zero the record is repeated for an undefined number of times.
    /// </summary>
    public SchemaRecordLimit Limit
    {
      get { return _Limit; }
      set { _Limit = value; }
    }

    protected SchemaRecordBase()
    {
    }

    public virtual void Add(SchemaRecordBase rec)
    {

    }

    public virtual void Remove(SchemaRecordBase rec)
    {

    }

    public delegate void ExecuteMethodDelegate(SchemaRecordBase rec);

    public virtual void Execute(ExecuteMethodDelegate em)
    {

    }
  }

  /// <summary>
  /// Values for the Limit attribute, offset and max number of records
  /// </summary>
  public struct SchemaRecordLimit
  {
    public int Offset;
    public int Max;

    public SchemaRecordLimit(int offs, int max)
    {
      Offset = offs;
      Max = max;
    }
  }
}
