using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  /// <summary>
  /// Represents the 'Composite' class in Composite pattern
  /// </summary>
  public class SchemaRecordComposite : SchemaRecordBase
  {
    private List<SchemaRecordBase> _Records;
    public SchemaRecordBase this[int idx]
    {
      get
      {
        if (idx < 0 || idx >= _Records.Count)
          throw new NCsvLibSchemaException("Index out of range");
        return _Records[idx];
      }
    }

    public int Count
    {
      get { return _Records.Count; }
    }

    public SchemaRecordComposite()
      : base()
    {
      _Records = new List<SchemaRecordBase>();
    }

    public override void Add(SchemaRecordBase rec)
    {
      _Records.Add(rec);
    }

    public override void Remove(SchemaRecordBase rec)
    {
      _Records.Remove(rec);
    }

		public override ExecuteMethodResult Execute(ExecuteMethodDelegate em)
    {
			ExecuteMethodResult res = null;
			ExecuteMethodResult restot = new ExecuteMethodResult();
			restot.RecNumber = 0;

      if (this.Limit.Max == 0)
      {
				while (res == null || res.RecNumber > 0)
				{
					foreach (SchemaRecordBase rec in _Records)
					{
						res = rec.Execute(em);
						restot.RecNumber += res.RecNumber;
					}
				}
      }
      else
      {
        for (int i = 0; i < (Limit.Offset+Limit.Max); i++)
        {
          if (i < Limit.Offset)
            continue;
					foreach (SchemaRecordBase rec in _Records)
					{
						res = rec.Execute(em);
						restot.RecNumber += res.RecNumber;
					}
        }
      }
			return restot;
    }
  }
}
