using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  /// <summary>
  /// Represents the 'Leaf' class in Composite Pattern
  /// </summary>
  public class SchemaRecord : SchemaRecordBase
  {
    private List<SchemaField> _Fields;
    
    public SchemaField this[int idx]
    {
      get
      {
        if (idx < 0 || idx >= _Fields.Count)
          throw new NCsvLibSchemaException("Index out of range");
        return _Fields[idx];
      }
    }

    public SchemaField this[string name]
    {
      get
      {
        foreach (SchemaField fld in _Fields)
        {
          if (fld.Name == name)
            return fld;
        }
        throw new NCsvLibSchemaException("Field requested not found");
      }
    }

    public int Count
    {
      get { return _Fields.Count; }
    }


    public SchemaRecord()
      : base()
    {
      _Fields = new List<SchemaField>();
    }

    public override void Add(SchemaRecordBase rec)
    {
      throw new NCsvLibSchemaException("Cannot add to SchemaRecord");
    }

    public override void Remove(SchemaRecordBase rec)
    {
      throw new NCsvLibSchemaException("Cannot remove from SchemaRecord");
    }

    public override void Execute(ExecuteMethodDelegate em)
    {
      em(this);
    }

    public void AddField(SchemaField fld)
    {
      _Fields.Add(fld);
    }

    public void RemoveField(SchemaField fld)
    {
      _Fields.Remove(fld);
    }
  }
}
