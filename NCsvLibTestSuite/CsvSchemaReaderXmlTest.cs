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
      SchemaRecord rec;
      SchemaRecordComposite comp;
      SchemaReaderXml rdr = new SchemaReaderXml(Helpers.SchFileName);
      Schema sch = rdr.GetSchema();
      Assert.That(sch.Count, Is.EqualTo(2));

      //options
      Assert.That(sch.Options.FieldSeparator, Is.EqualTo("|"));
      Assert.That(sch.Options.Eol, Is.EqualTo(Environment.NewLine));
      Assert.That(sch.Options.Quotes, Is.EqualTo("\""));
      
      //*** First record
      Assert.That(sch[0], Is.TypeOf(typeof(SchemaRecord)));
      rec = (SchemaRecord)sch[0];
      Assert.That(rec.Count, Is.EqualTo(5));
      Assert.That(rec.Id, Is.EqualTo("R1"));
      //intfld
      Assert.That(rec[0].Name, Is.EqualTo("intfld"));
      Assert.That(rec[0].AddQuotes, Is.False);
      Assert.That(rec[0].FldType, Is.EqualTo(SchemaFieldType.Int));
      //strfld
      Assert.That(rec[1].Name, Is.EqualTo("strfld"));
      Assert.That(rec[1].AddQuotes, Is.True);
      Assert.That(rec[1].FldType, Is.EqualTo(SchemaFieldType.String));
      //doublefld
      Assert.That(rec[2].Name, Is.EqualTo("doublefld"));
      Assert.That(rec[2].FldType, Is.EqualTo(SchemaFieldType.Double));
      //decimalfld
      Assert.That(rec[3].Name, Is.EqualTo("decimalfld"));
      Assert.That(rec[3].FldType, Is.EqualTo(SchemaFieldType.Decimal));
      //fixedfld
      Assert.That(rec[4].Name, Is.EqualTo("fixedfld"));
      Assert.That(rec[4].HasFixedValue, Is.True);
      Assert.That(rec[4].FixedValue, Is.EqualTo("AAA"));

      //*** Second record (group RG1)
      Assert.That(sch[1], Is.TypeOf(typeof(SchemaRecordComposite)));
      comp = (SchemaRecordComposite)sch[1];
      Assert.That(comp.Count, Is.EqualTo(1));
      Assert.That(comp[0], Is.TypeOf(typeof(SchemaRecord)));
      //record R2
      rec = (SchemaRecord)comp[0];
      Assert.That(rec.Count, Is.EqualTo(3));
      //fixedr2
      Assert.That(rec[0].Name, Is.EqualTo("fixedr2"));
      Assert.That(rec[0].HasFixedValue, Is.True);
      Assert.That(rec[0].FixedValue, Is.EqualTo("FLDR2"));
      //intr2
      Assert.That(rec[1].Name, Is.EqualTo("intr2"));
      Assert.That(rec[1].AddQuotes, Is.False);
      Assert.That(rec[1].FldType, Is.EqualTo(SchemaFieldType.Int));
      //strr2
      Assert.That(rec[2].Name, Is.EqualTo("strr2"));
      Assert.That(rec[2].AddQuotes, Is.True);
      Assert.That(rec[2].FldType, Is.EqualTo(SchemaFieldType.String));
    }
  }
}
