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
      Assert.That(sch.Count, Is.EqualTo(3));

      //options
      Assert.That(sch.Options.FieldSeparator, Is.EqualTo("|"));
      Assert.That(sch.Options.Eol, Is.EqualTo(Environment.NewLine));
      Assert.That(sch.Options.Quotes, Is.EqualTo("\""));
      Assert.That(sch.Options.Enc.EncodingName, Text.Contains("UTF-8"));
      
      //*** First record
      Assert.That(sch[0], Is.TypeOf(typeof(SchemaRecord)));
      rec = (SchemaRecord)sch[0];
      Assert.That(rec.Count, Is.EqualTo(7));
      Assert.That(rec.Id, Is.EqualTo("R1"));
      Assert.That(rec.Repeat, Is.EqualTo(0));
      //intfld
      Assert.That(rec[0].Name, Is.EqualTo("intfld"));
      Assert.That(rec[0].AddQuotes, Is.False);
      Assert.That(rec[0].FldType, Is.EqualTo(SchemaFieldType.Int));
      Assert.That(rec[0].Filled, Is.EqualTo(true));
      Assert.That(rec[0].FillChar, Is.EqualTo('0'));
      Assert.That(rec[0].Comment, Is.EqualTo("Comment for intfld"));
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
      Assert.That(rec[3].CustFmt, Is.TypeOf(typeof(NCsvLib.Formatters.NumberDigitsFormatter)));
      //dtfld
      Assert.That(rec[4].Name, Is.EqualTo("dtfld"));
      Assert.That(rec[4].FldType, Is.EqualTo(SchemaFieldType.DateTime));
      //fixedfld
      Assert.That(rec[5].Name, Is.EqualTo("fixedfld"));
      Assert.That(rec[5].HasFixedValue, Is.True);
      Assert.That(rec[5].FixedValue, Is.EqualTo("AAA"));
      //strfld2
      Assert.That(rec[6].Name, Is.EqualTo("strfld2"));
      Assert.That(rec[6].FldType, Is.EqualTo(SchemaFieldType.String));
      Assert.That(rec[6].CustFmt.GetType().Name, Text.Matches("DummyFormatter"));
      
      //*** Second record (group RG1)
      Assert.That(sch[1], Is.TypeOf(typeof(SchemaRecordComposite)));
      comp = (SchemaRecordComposite)sch[1];
      Assert.That(comp.Count, Is.EqualTo(2));
      Assert.That(comp.Repeat, Is.EqualTo(2));
      Assert.That(comp[0], Is.TypeOf(typeof(SchemaRecord)));      
      //record R2
      rec = (SchemaRecord)comp[0];
      Assert.That(rec.Count, Is.EqualTo(4));
      Assert.That(rec.Repeat, Is.EqualTo(4));
      //fixedr2
      Assert.That(rec[0].Name, Is.EqualTo("fixedr2"));
      Assert.That(rec[0].HasFixedValue, Is.True);
      Assert.That(rec[0].FixedValue, Is.EqualTo("FLDR2"));
      //intr2
      Assert.That(rec[1].Name, Is.EqualTo("intr2"));
      Assert.That(rec[1].AddQuotes, Is.False);
      Assert.That(rec[1].FldType, Is.EqualTo(SchemaFieldType.Int));
      Assert.That(rec[1].Filled, Is.True);
      Assert.That(rec[1].FillChar, Is.EqualTo('0'));
      //intr2left
      Assert.That(rec[2].Name, Is.EqualTo("intr2left"));
      Assert.That(rec[2].AddQuotes, Is.False);
      Assert.That(rec[2].FldType, Is.EqualTo(SchemaFieldType.Int));
      Assert.That(rec[2].Alignment, Is.EqualTo(SchemaValueAlignment.Left));
      Assert.That(rec[2].Filled, Is.True);
      Assert.That(rec[2].FillChar, Is.EqualTo('0'));
      //strr2
      Assert.That(rec[3].Name, Is.EqualTo("strr2"));
      Assert.That(rec[3].AddQuotes, Is.True);
      Assert.That(rec[3].FldType, Is.EqualTo(SchemaFieldType.String));
      
      //record R3
      rec = (SchemaRecord)comp[1];
      Assert.That(rec.Count, Is.EqualTo(3));
      Assert.That(rec.Repeat, Is.EqualTo(4));
      //fixedr3
      Assert.That(rec[0].Name, Is.EqualTo("fixedr3"));
      Assert.That(rec[0].HasFixedValue, Is.True);
      Assert.That(rec[0].FixedValue, Is.EqualTo("FLDR3"));
      //intr3
      Assert.That(rec[1].Name, Is.EqualTo("intr3"));
      Assert.That(rec[1].AddQuotes, Is.False);
      Assert.That(rec[1].FldType, Is.EqualTo(SchemaFieldType.Int));
      //strr3
      Assert.That(rec[2].Name, Is.EqualTo("strr3"));
      Assert.That(rec[2].AddQuotes, Is.True);
      Assert.That(rec[2].FldType, Is.EqualTo(SchemaFieldType.String));

      //*** Third record (R4)
      Assert.That(sch[2], Is.TypeOf(typeof(SchemaRecord)));
      rec = (SchemaRecord)sch[2];
      Assert.That(rec.Repeat, Is.EqualTo(4));
      //fixedr4
      Assert.That(rec[0].Name, Is.EqualTo("fixedr4"));
      Assert.That(rec[0].HasFixedValue, Is.True);
      Assert.That(rec[0].FixedValue, Is.EqualTo("FLDR4"));
      //intr4
      Assert.That(rec[1].Name, Is.EqualTo("intr4"));
      Assert.That(rec[1].AddQuotes, Is.False);
      Assert.That(rec[1].FldType, Is.EqualTo(SchemaFieldType.Int));
      Assert.That(rec[1].Size, Is.EqualTo(5));
      //doublefld
      Assert.That(rec[2].Name, Is.EqualTo("doubler4"));
      Assert.That(rec[2].FldType, Is.EqualTo(SchemaFieldType.Double));
      //decimalfld
      Assert.That(rec[3].Name, Is.EqualTo("decimalr4"));
      Assert.That(rec[3].FldType, Is.EqualTo(SchemaFieldType.Decimal));
    }
  }
}
