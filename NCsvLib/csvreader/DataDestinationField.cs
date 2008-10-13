using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  public class DataDestinationField
  {
    private string _Name;
    public string Name
    {
      get { return _Name; }
      set { _Name = value; }
    }

    private object _Value;
    public object Value
    {
      get { return _Value; }
      set { _Value = value; }
    }

		private SchemaFieldType _ValueType;
		public SchemaFieldType ValueType
		{
			get { return _ValueType; }
			set { _ValueType = value; }
		}
  }
}
