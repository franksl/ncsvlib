using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using NCsvLib;
using NUnit.Framework;
using NCsvLib.Formatters;
using System.IO;

namespace NCsvLibTestSuite
{
    [TestFixture]
    public class FormatterBaseTest
    {
        [SetUp]
        public void SetUp()
        {
            //Helpers.CreateEnvironment();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void CheckQuotes()
        {
            DataSourceField dsf;
            FormatterBase fmt;
            SchemaField scf;
            Schema sch;

            fmt = new FormatterBase();
            sch = new Schema();

            dsf = new DataSourceField();
            dsf.Name = "fld1";
            dsf.Value = "quotedtext";

            scf = new SchemaField(sch);
            scf.Name = "fld1";
            scf.FldType = SchemaFieldType.String;
            Assert.That(fmt.Format(dsf, scf), Is.EqualTo("quotedtext"));

            scf.AddQuotes = true;
            scf.Quotes = "\"";
            Assert.That(fmt.Format(dsf, scf), Is.EqualTo("\"quotedtext\""));

            scf.FixedSize = true;
            scf.Size = 6;
            Assert.That(fmt.Format(dsf, scf), Is.EqualTo("\"quoted\""));

            scf.FixedSize = true;
            scf.Size = 20;
            Assert.That(fmt.Format(dsf, scf), Is.EqualTo("\"quotedtext          \""));

            scf.Alignment = SchemaValueAlignment.Right;
            scf.FixedSize = true;
            scf.Size = 6;
            Assert.That(fmt.Format(dsf, scf), Is.EqualTo("\"quoted\""));

            scf.FixedSize = true;
            scf.Size = 20;
            Assert.That(fmt.Format(dsf, scf), Is.EqualTo("\"          quotedtext\""));

            scf.FixedSize = false;
            Assert.That(fmt.Format(dsf, scf), Is.EqualTo("\"quotedtext\""));
        }

        [Test]
        public void FixedValue()
        {
            DataSourceField dsf;
            FormatterBase fmt;
            SchemaField scf;
            Schema sch;

            fmt = new FormatterBase();
            sch = new Schema();

            dsf = new DataSourceField();
            dsf.Name = "fld1";
            dsf.Value = null;

            scf = new SchemaField(sch);
            scf.Name = "fld1";
            scf.FldType = SchemaFieldType.String;
            scf.HasFixedValue = true;
            scf.FixedValue = "fixedvalue";
            Assert.That(fmt.Format(dsf, scf), Is.EqualTo("fixedvalue"));

            scf.FixedSize = true;
            scf.Size = 10;
            Assert.That(fmt.Format(dsf, scf), Is.EqualTo("fixedvalue"));

            scf.FixedValue = "123456789";
            Assert.That(delegate { fmt.Format(dsf, scf); },
                Throws.TypeOf<NCsvLibOutputException>());

            scf.FixedValue = "12345678901";
            Assert.That(delegate { fmt.Format(dsf, scf); },
                Throws.TypeOf<NCsvLibOutputException>());

            scf.Filled = true;
            scf.FillChar = ' ';

            scf.FixedValue = "12345678901";
            Assert.That(delegate { fmt.Format(dsf, scf); },
                Throws.TypeOf<NCsvLibOutputException>());

            scf.FixedValue = "1234";
            Assert.That(fmt.Format(dsf, scf), Is.EqualTo("1234      "));
        }
    }
}
