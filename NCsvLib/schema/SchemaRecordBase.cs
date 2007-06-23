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
    public string Id
    {
      get { return _Id; }
      set { _Id = value; }
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
