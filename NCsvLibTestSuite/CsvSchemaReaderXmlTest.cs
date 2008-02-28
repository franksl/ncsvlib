using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NCsvLib;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NCsvLibTestSuite
{
  [TestFixture]
  public class CsvSchemaReaderXmlTest
  {
    [Test]
    public void GetSchemaFromFile()
    {
      SchemaReaderXml rdr = new SchemaReaderXml(Helpers.SchFileName);
      Schema sch = rdr.Sch;
			AssertSchema(sch);
		}

		[Test]
		public void GetSchemaFromStream()
		{
			using (StreamReader txtr = new StreamReader(Helpers.SchFileName))
			{
				SchemaReaderXml rdr = new SchemaReaderXml(txtr.BaseStream);
				Schema sch = rdr.Sch;
				AssertSchema(sch);
			}
		}

		private void AssertSchema(Schema sch)
		{
			SchemaRecord rec;
			SchemaRecordComposite comp;

      Assert.That(sch.Count, Is.EqualTo(3));

      //options
      Assert.That(sch.Options.FieldSeparator, Is.EqualTo("|"));
      Assert.That(sch.Options.Eol, Is.EqualTo(Environment.NewLine));
      Assert.That(sch.Options.Quotes, Is.EqualTo("\""));
      Assert.That(sch.Options.Enc.EncodingName, Text.Matches("UTF-8"));
      
      //*** First record
      Assert.That(sch[0], Is.TypeOf(typeof(SchemaRecord)));
      rec = (SchemaRecord)sch[0];
      Assert.That(rec.Count, Is.EqualTo(8));
      Assert.That(rec.Id, Is.EqualTo("R1"));
      Assert.That(rec.Limit.Offset, Is.EqualTo(0));
      Assert.That(rec.Limit.Max, Is.EqualTo(0));
      Assert.That(rec.ColHeaders, Is.True);
      //intfld
      Assert.That(rec[0].Name, Is.EqualTo("intfld"));
      Assert.That(rec[0].AddQuotes, Is.False);
      Assert.That(rec[0].FldType, Is.EqualTo(SchemaFieldType.Int));
      Assert.That(rec[0].Filled, Is.EqualTo(true));
      Assert.That(rec[0].FillChar, Is.EqualTo('0'));
      Assert.That(rec[0].Comment, Is.EqualTo("Comment for intfld"));
      Assert.That(rec[0].ColHdr, Text.Matches("int1"));
      //strfld
      Assert.That(rec[1].Name, Is.EqualTo("strfld"));
      Assert.That(rec[1].AddQuotes, Is.True);
      Assert.That(rec[1].FldType, Is.EqualTo(SchemaFieldType.String));
      Assert.That(rec[1].Comment, Is.EqualTo(string.Empty));
      Assert.That(rec[1].ColHdr, Text.Matches("str1"));
      //doublefld
      Assert.That(rec[2].Name, Is.EqualTo("doublefld"));
      Assert.That(rec[2].FldType, Is.EqualTo(SchemaFieldType.Double));
      Assert.That(rec[2].ColHdr, Text.Matches("dbl1"));
      //decimalfld
      Assert.That(rec[3].Name, Is.EqualTo("decimalfld"));
      Assert.That(rec[3].FldType, Is.EqualTo(SchemaFieldType.Decimal));
      Assert.That(rec[3].CustFmt, Is.TypeOf(typeof(NCsvLib.Formatters.NumberDigitsFormatter)));
      Assert.That(rec[3].ColHdr, Text.Matches("dec1"));
      //dtfld
      Assert.That(rec[4].Name, Is.EqualTo("dtfld"));
      Assert.That(rec[4].FldType, Is.EqualTo(SchemaFieldType.DateTime));
      Assert.That(rec[4].ColHdr, Text.Matches("dt1"));
      //fixedfld
      Assert.That(rec[5].Name, Is.EqualTo("fixedfld"));
      Assert.That(rec[5].HasFixedValue, Is.True);
      Assert.That(rec[5].FixedValue, Is.EqualTo("AAA"));
      Assert.That(rec[5].ColHdr, Text.Matches("fix1"));
      //strfld2
      Assert.That(rec[6].Name, Is.EqualTo("strfld2"));
      Assert.That(rec[6].FldType, Is.EqualTo(SchemaFieldType.String));
      Assert.That(rec[6].CustFmt.GetType().Name, Text.Matches("DummyFormatter"));
      Assert.That(rec[6].ColHdr, Text.Matches("str1_2"));
      //boolfld
      Assert.That(rec[7].Name, Is.EqualTo("boolfld"));
      Assert.That(rec[7].FldType, Is.EqualTo(SchemaFieldType.Bool));
      Assert.That(rec[7].BoolSettings.TrueValue, Text.Matches("true"));
      Assert.That(rec[7].BoolSettings.FalseValue, Text.Matches("false"));
      Assert.That(rec[7].BoolSettings.TrueIoValue, Is.EqualTo((int)1));
      Assert.That(rec[7].BoolSettings.FalseIoValue, Is.EqualTo((int)0));
      Assert.That(rec[7].BoolSettings.BoolIoType, Is.EqualTo(SchemaFieldBoolIoType.Int));
      Assert.That(rec[7].ColHdr, Text.Matches("bool1"));
      
      //*** Second record (group RG1)
      Assert.That(sch[1], Is.TypeOf(typeof(SchemaRecordComposite)));
      comp = (SchemaRecordComposite)sch[1];
      Assert.That(comp.Count, Is.EqualTo(2));
      Assert.That(comp.Limit.Offset, Is.EqualTo(0));
      Assert.That(comp.Limit.Max, Is.EqualTo(2));
      Assert.That(comp[0], Is.TypeOf(typeof(SchemaRecord)));      
      //record R2
      rec = (SchemaRecord)comp[0];
      Assert.That(rec.Count, Is.EqualTo(5));
      Assert.That(rec.Limit.Offset, Is.EqualTo(0));
      Assert.That(rec.Limit.Max, Is.EqualTo(4));
      Assert.That(rec.ColHeaders, Is.False);
      //fixedr2
      Assert.That(rec[0].Name, Is.EqualTo("fixedr2"));
      Assert.That(rec[0].HasFixedValue, Is.True);
      Assert.That(rec[0].FixedValue, Is.EqualTo("FLDR2"));
      Assert.That(rec[0].ColHdr, Text.Matches(string.Empty));
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
      //bool2
      Assert.That(rec[4].Name, Is.EqualTo("bool2"));
      Assert.That(rec[4].FldType, Is.EqualTo(SchemaFieldType.Bool));
      Assert.That(rec[4].BoolSettings.TrueValue, Text.Matches("TTT"));
      Assert.That(rec[4].BoolSettings.FalseValue, Text.Matches("FFF"));
      Assert.That(rec[4].BoolSettings.TrueIoValue, Text.Matches("T"));
      Assert.That(rec[4].BoolSettings.FalseIoValue, Text.Matches("F"));
      Assert.That(rec[4].BoolSettings.BoolIoType, Is.EqualTo(SchemaFieldBoolIoType.String));
      
      //record R3
      rec = (SchemaRecord)comp[1];
      Assert.That(rec.Count, Is.EqualTo(3));
      Assert.That(rec.Limit.Offset, Is.EqualTo(0));
      Assert.That(rec.Limit.Max, Is.EqualTo(4));
      Assert.That(rec.ColHeaders, Is.False);
      //fixedr3
      Assert.That(rec[0].Name, Is.EqualTo("fixedr3"));
      Assert.That(rec[0].HasFixedValue, Is.True);
      Assert.That(rec[0].FixedValue, Is.EqualTo("FLDR3"));
      //intr3
      Assert.That(rec[1].Name, Is.EqualTo("intr3"));
      Assert.That(rec[1].AddQuotes, Is.True);
      Assert.That(rec[1].Quotes, Text.Matches("'"));
      Assert.That(rec[1].FldType, Is.EqualTo(SchemaFieldType.Int));
      //strr3
      Assert.That(rec[2].Name, Is.EqualTo("strr3"));
      Assert.That(rec[2].AddQuotes, Is.True);
      Assert.That(rec[2].FldType, Is.EqualTo(SchemaFieldType.String));

      //*** Third record (R4)
      Assert.That(sch[2], Is.TypeOf(typeof(SchemaRecord)));
      rec = (SchemaRecord)sch[2];
      Assert.That(rec.Limit.Offset, Is.EqualTo(3));
      Assert.That(rec.Limit.Max, Is.EqualTo(5));
      Assert.That(rec.ColHeaders, Is.False);
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
