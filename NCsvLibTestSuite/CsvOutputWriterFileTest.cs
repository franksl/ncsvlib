using System;
using System.Collections.Generic;
using System.Text;
using NCsvLib;
using NUnit.Framework;
using System.IO;

namespace NCsvLibTestSuite
{
    [TestFixture]
    public class CsvOutputWriterFileTest
    {
        [SetUp]
        public void SetUp()
        {
            Helpers.CreateEnvironment();
            if (File.Exists(Helpers.OutFileName))
                File.Delete(Helpers.OutFileName);
        }

        /*[TearDown]
        public void TearDown()
        {
          if (File.Exists(_FileName))
            File.Delete(_FileName);
        }*/

        [Test]
        public void WriteFileMulti()
        {
            int i;

            ExecuteWrite(false, null);
            //Opens the eventual file and reads values
            Assert.That(File.Exists(Helpers.OutFileName), Is.True);

            //File tests
            List<string> ln = Helpers.GetFileLines(Helpers.OutFileName, Helpers.Enc);
            i = 0;
            Assert.That(ln.Count, Is.EqualTo(42));
            //R1 (contains headers)
            Assert.That(ln[i++], Is.EqualTo("int1|str1|dbl1|dec1|dt1|fix1|str1_2|bool1"));
            Assert.That(ln[i++], Is.EqualTo("00001|\"aaa                 \"|        100,10|\"        100011\"|11/01/2001 10:11:12|AAA|DUMMY|true"));
            Assert.That(ln[i++], Is.EqualTo("00002|\"bbb                 \"|        200,20|\"        200022\"|12/02/2002 12:13:14|AAA|DUMMY|false"));
            Assert.That(ln[i++], Is.EqualTo("00003|\"ccc                 \"|        300,30|\"        300033\"|13/03/2003 13:14:15|AAA|DUMMY|true"));
            Assert.That(ln[i++], Is.EqualTo("00004|\"ddd                 \"|        400,40|\"        400044\"|14/04/2004 14:15:16|AAA|DUMMY|false"));
            //Record group that contains records 2 and 3 has limit = 2
            Assert.That(ln[i++], Is.EqualTo("FLDR2|001|11000|\"                r2_1\"|TTT"));
            Assert.That(ln[i++], Is.EqualTo("FLDR2|002|22000|\"                r2_2\"|FFF"));
            Assert.That(ln[i++], Is.EqualTo("FLDR2|003|33000|\"                r2_3\"|TTT"));
            Assert.That(ln[i++], Is.EqualTo("FLDR2|004|44000|\"                ABCD\"|FFF"));
            Assert.That(ln[i++], Is.EqualTo("FLDR3|'1'|\"                r3_1\""));
            Assert.That(ln[i++], Is.EqualTo("FLDR3|'2'|\"                r3_2\""));
            Assert.That(ln[i++], Is.EqualTo("FLDR3|'3'|\"                r3_3\""));
            Assert.That(ln[i++], Is.EqualTo("FLDR3|'4'|\"                r3_4\""));
            Assert.That(ln[i++], Is.EqualTo("FLDR2|005|55000|\"                r2_5\"|TTT"));
            Assert.That(ln[i++], Is.EqualTo("FLDR2|006|66000|\"                r2_6\"|FFF"));
            Assert.That(ln[i++], Is.EqualTo("FLDR2|007|77000|\"                r2_7\"|TTT"));
            Assert.That(ln[i++], Is.EqualTo("FLDR2|008|88000|\"                r2_8\"|FFF"));
            Assert.That(ln[i++], Is.EqualTo("FLDR3|'5'|\"                r3_5\""));
            Assert.That(ln[i++], Is.EqualTo("FLDR3|'6'|\"                r3_6\""));
            Assert.That(ln[i++], Is.EqualTo("FLDR3|'7'|\"                r3_7\""));
            Assert.That(ln[i++], Is.EqualTo("FLDR3|'8'|\"                r3_8\""));
            //Limit is 3,5
            Assert.That(ln[i++], Is.EqualTo("FLDR4|    4|         44,40|        444,44"));
            Assert.That(ln[i++], Is.EqualTo("FLDR4|    5|         55,50|        555,55"));
            Assert.That(ln[i++], Is.EqualTo("FLDR4|    6|         66,60|        666,66"));
            Assert.That(ln[i++], Is.EqualTo("FLDR4|    7|         77,70|        777,77"));
            Assert.That(ln[i++], Is.EqualTo("FLDR4|    8|         88,80|        888,88"));
            //Record group with limit=0
            Assert.That(ln[i++], Is.EqualTo("    1|AA   "));
            Assert.That(ln[i++], Is.EqualTo("    2|BB   "));
            Assert.That(ln[i++], Is.EqualTo("   11|AAA  "));
            Assert.That(ln[i++], Is.EqualTo("   22|BBB  "));
            Assert.That(ln[i++], Is.EqualTo("    3|CC   "));
            Assert.That(ln[i++], Is.EqualTo("    4|DD   "));
            Assert.That(ln[i++], Is.EqualTo("   33|CCC  "));
            Assert.That(ln[i++], Is.EqualTo("   44|DDD  "));
            Assert.That(ln[i++], Is.EqualTo("    5|EE   "));
            Assert.That(ln[i++], Is.EqualTo("    6|FF   "));
            Assert.That(ln[i++], Is.EqualTo("   55|EEE  "));
            Assert.That(ln[i++], Is.EqualTo("   66|FFF  "));
            Assert.That(ln[i++], Is.EqualTo("    7|GG   "));
            Assert.That(ln[i++], Is.EqualTo("    8|HH   "));
            Assert.That(ln[i++], Is.EqualTo("   77|GGG  "));
            Assert.That(ln[i++], Is.EqualTo("   88|HHH  "));
        }

        [Test]
        public void WriteFileMultiLastSep()
        {
            int i;

            ExecuteWrite(false, true);
            //Opens the eventual file and reads values
            Assert.That(File.Exists(Helpers.OutFileName), Is.True);

            //File tests
            List<string> ln = Helpers.GetFileLines(Helpers.OutFileName, Helpers.Enc);
            i = 0;
            Assert.That(ln.Count, Is.EqualTo(42));
            //R1 (contains headers)
            Assert.That(ln[i++], Is.EqualTo("int1|str1|dbl1|dec1|dt1|fix1|str1_2|bool1|"));
            Assert.That(ln[i++], Is.EqualTo("00001|\"aaa                 \"|        100,10|\"        100011\"|11/01/2001 10:11:12|AAA|DUMMY|true|"));
            Assert.That(ln[i++], Is.EqualTo("00002|\"bbb                 \"|        200,20|\"        200022\"|12/02/2002 12:13:14|AAA|DUMMY|false|"));
            Assert.That(ln[i++], Is.EqualTo("00003|\"ccc                 \"|        300,30|\"        300033\"|13/03/2003 13:14:15|AAA|DUMMY|true|"));
            Assert.That(ln[i++], Is.EqualTo("00004|\"ddd                 \"|        400,40|\"        400044\"|14/04/2004 14:15:16|AAA|DUMMY|false|"));
            //Record group that contains records 2 and 3 has limit = 2
            Assert.That(ln[i++], Is.EqualTo("FLDR2|001|11000|\"                r2_1\"|TTT|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR2|002|22000|\"                r2_2\"|FFF|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR2|003|33000|\"                r2_3\"|TTT|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR2|004|44000|\"                ABCD\"|FFF|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR3|'1'|\"                r3_1\"|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR3|'2'|\"                r3_2\"|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR3|'3'|\"                r3_3\"|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR3|'4'|\"                r3_4\"|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR2|005|55000|\"                r2_5\"|TTT|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR2|006|66000|\"                r2_6\"|FFF|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR2|007|77000|\"                r2_7\"|TTT|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR2|008|88000|\"                r2_8\"|FFF|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR3|'5'|\"                r3_5\"|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR3|'6'|\"                r3_6\"|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR3|'7'|\"                r3_7\"|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR3|'8'|\"                r3_8\"|"));
            //Limit is 3,5
            Assert.That(ln[i++], Is.EqualTo("FLDR4|    4|         44,40|        444,44|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR4|    5|         55,50|        555,55|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR4|    6|         66,60|        666,66|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR4|    7|         77,70|        777,77|"));
            Assert.That(ln[i++], Is.EqualTo("FLDR4|    8|         88,80|        888,88|"));
            //Record group with limit=0
            Assert.That(ln[i++], Is.EqualTo("    1|AA   |"));
            Assert.That(ln[i++], Is.EqualTo("    2|BB   |"));
            Assert.That(ln[i++], Is.EqualTo("   11|AAA  |"));
            Assert.That(ln[i++], Is.EqualTo("   22|BBB  |"));
            Assert.That(ln[i++], Is.EqualTo("    3|CC   |"));
            Assert.That(ln[i++], Is.EqualTo("    4|DD   |"));
            Assert.That(ln[i++], Is.EqualTo("   33|CCC  |"));
            Assert.That(ln[i++], Is.EqualTo("   44|DDD  |"));
            Assert.That(ln[i++], Is.EqualTo("    5|EE   |"));
            Assert.That(ln[i++], Is.EqualTo("    6|FF   |"));
            Assert.That(ln[i++], Is.EqualTo("   55|EEE  |"));
            Assert.That(ln[i++], Is.EqualTo("   66|FFF  |"));
            Assert.That(ln[i++], Is.EqualTo("    7|GG   |"));
            Assert.That(ln[i++], Is.EqualTo("    8|HH   |"));
            Assert.That(ln[i++], Is.EqualTo("   77|GGG  |"));
            Assert.That(ln[i++], Is.EqualTo("   88|HHH  |"));
        }


        [Test]
        public void CheckBom()
        {
            ExecuteWrite(false, null);

            //Checks that the first three bytes are the Byte Order Mark for UTF-8
            //(EF BB BF) but the beginning of the first row
            Assert.That(File.Exists(Helpers.OutFileName), Is.True);
            using (FileStream fs = new FileStream(Helpers.OutFileName, FileMode.Open))
            {
                int bt;
                bt = fs.ReadByte();
                Assert.That(bt, Is.EqualTo(0xEF));
                bt = fs.ReadByte();
                Assert.That(bt, Is.EqualTo(0xBB));
                bt = fs.ReadByte();
                Assert.That(bt, Is.EqualTo(0xBF));
            }
        }

        [Test]
        public void CheckNoBom()
        {
            ExecuteWrite(true, null);

            //Checks that the first three bytes are NOT the Byte Order Mark for UTF-8
            //(EF BB BF)
            Assert.That(File.Exists(Helpers.OutFileName), Is.True);
            using (FileStream fs = new FileStream(Helpers.OutFileName, FileMode.Open))
            {
                int bt;
                bt = fs.ReadByte();
                Assert.That(Convert.ToChar(bt), Is.EqualTo('i'));
                bt = fs.ReadByte();
                Assert.That(Convert.ToChar(bt), Is.EqualTo('n'));
                bt = fs.ReadByte();
                Assert.That(Convert.ToChar(bt), Is.EqualTo('t'));
            }
        }

        private void ExecuteWrite(bool noBom, bool? lastsep)
        {
            CsvWriterController ctrl = new CsvWriterController();
            ctrl.SchemaRdr = new SchemaReaderXml(Helpers.SchFileName);
            if (noBom)
                ctrl.SchemaRdr.Sch.Options.SetNoBomEncoding(true);
            if (lastsep.HasValue)
                ctrl.SchemaRdr.Sch.Options.LastFieldSeparator = lastsep.Value;
            DataSourceReaderBase rdr = new DataSourceReaderBase();
            rdr.Add(Helpers.R1, new DataSourceRecordReaderDb(Helpers.R1,
                Helpers.GetDbConnectionFromFile(Helpers.ConnStrFileName), Helpers.Qry1));
            rdr.Add(Helpers.R2, new DataSourceRecordReaderDb(Helpers.R2,
                Helpers.GetDbConnectionFromFile(Helpers.ConnStrFileName), Helpers.Qry2));
            rdr.Add(Helpers.R3, new DataSourceRecordReaderDb(Helpers.R3,
                Helpers.GetDbConnectionFromFile(Helpers.ConnStrFileName), Helpers.Qry3));
            rdr.Add(Helpers.R4, new DataSourceRecordReaderDb(Helpers.R4,
                Helpers.GetDbConnectionFromFile(Helpers.ConnStrFileName), Helpers.Qry4));
            rdr.Add(Helpers.R5, new DataSourceRecordReaderDb(Helpers.R5,
                Helpers.GetDbConnectionFromFile(Helpers.ConnStrFileName), Helpers.Qry5));
            rdr.Add(Helpers.R6, new DataSourceRecordReaderDb(Helpers.R6,
                Helpers.GetDbConnectionFromFile(Helpers.ConnStrFileName), Helpers.Qry6));

            ctrl.InputRdr = rdr;
            ctrl.OutWriter = new CsvOutputWriterFile(Helpers.OutFileName);
            ctrl.Execute();
        }
    }
}
