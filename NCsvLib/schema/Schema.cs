using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
    public class Schema : SchemaRecordComposite
    {
        public SchemaOptions Options;

        public Schema()
            : base()
        {
            Options = new SchemaOptions();
            //Sets limit max to 1 to execute the whole schema only one time
            _Limit = new SchemaRecordLimit(0, 1);
        }
    }
}
