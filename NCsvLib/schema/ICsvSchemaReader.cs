using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  interface ICsvSchemaReader
  {
    CsvSchema GetSchema();
  }
}
