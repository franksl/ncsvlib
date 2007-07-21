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

    protected int _Repeat;
    /// <summary>
    /// Number of times this record is repeated.
    /// If zero the record is repeated for an undefined number of times.
    /// </summary>
    public int Repeat
    {
      get { return _Repeat; }
      set { _Repeat = value; }
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
}
