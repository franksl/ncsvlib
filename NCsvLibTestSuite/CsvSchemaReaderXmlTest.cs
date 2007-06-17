using System;
using System.Collections.Generic;
using System.Text;
using NCsvLib;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NCsvLibTestSuite
{
  [TestFixture]
  public class CsvSchemaReaderXmlTest
  {
    [Test]
    public void GetSchema()
    {
      SchemaReaderXml rdr = new SchemaReaderXml(Helpers.SchFileName);
      Schema sch = rdr.GetSchema();
      //options
      Assert.That(sch.Options.FieldSeparator, Is.EqualTo("|"));
      Assert.That(sch.Options.Eol, Is.EqualTo(Environment.NewLine));
      Assert.That(sch.Options.Quotes, Is.EqualTo("\""));
      //fields
      Assert.That(sch.Count, Is.EqualTo(5));
      //intfld
      Assert.That(sch[0].Name, Is.EqualTo("intfld"));
      Assert.That(sch[0].AddQuotes, Is.False);
      Assert.That(sch[0].FldType, Is.EqualTo(SchemaFieldType.Int));
      //strfld
      Assert.That(sch[1].Name, Is.EqualTo("strfld"));
      Assert.That(sch[1].AddQuotes, Is.True);
      Assert.That(sch[1].FldType, Is.EqualTo(SchemaFieldType.String));
      //doublefld
      Assert.That(sch[2].Name, Is.EqualTo("doublefld"));
      Assert.That(sch[2].FldType, Is.EqualTo(SchemaFieldType.Double));
      //decimalfld
      Assert.That(sch[3].Name, Is.EqualTo("decimalfld"));
      Assert.That(sch[3].FldType, Is.EqualTo(SchemaFieldType.Decimal));
      //fixedfld
      Assert.That(sch[4].HasFixedValue, Is.True);
      Assert.That(sch[4].FixedValue, Is.EqualTo("AAA"));
    }
  }
}
