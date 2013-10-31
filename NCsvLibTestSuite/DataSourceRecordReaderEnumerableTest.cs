using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using NCsvLib;
using NUnit.Framework;
using System.IO;

namespace NCsvLibTestSuite
{
    [TestFixture]
    public class DataSourceRecordReaderEnumerableTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void OpenCloseSingle()
        {
            DataSourceReaderBase rdr = new DataSourceReaderBase();
            IEnumerable<Csvtest1> c = PrepareCsvtest1Collection();
            DataSourceRecordReaderEnumerable<Csvtest1> rec = 
                new DataSourceRecordReaderEnumerable<Csvtest1>(
                    Helpers.R1, c);
            rdr.Add(Helpers.R1, rec);
            rec.Open();
            Assert.That(rdr[Helpers.R1].Eof(), Is.False);
            rec.Close();
        }

        [Test]
        public void OpenCloseMulti()
        {
            DataSourceReaderBase rdr = new DataSourceReaderBase();
            rdr.Add(Helpers.R1, 
                new DataSourceRecordReaderEnumerable<Csvtest1>(Helpers.R1, 
                    PrepareCsvtest1Collection()));
            rdr.Add(Helpers.R2, 
                new DataSourceRecordReaderEnumerable<Csvtest2>(Helpers.R2, 
                    PrepareCsvtest2Collection()));
            rdr.Add(Helpers.R3, 
                new DataSourceRecordReaderEnumerable<Csvtest3>(Helpers.R3, 
                    PrepareCsvtest3Collection()));
            rdr.Add(Helpers.R4,
                new DataSourceRecordReaderEnumerable<Csvtest4>(Helpers.R4, 
                    PrepareCsvtest4Collection()));
            rdr.Add(Helpers.R5, 
                new DataSourceRecordReaderEnumerable<Csvtest5>(Helpers.R5, 
                    PrepareCsvtest5Collection()));
            rdr.Add(Helpers.R6, 
                new DataSourceRecordReaderEnumerable<Csvtest6>(Helpers.R6, 
                    PrepareCsvtest6Collection()));
            rdr[Helpers.R1].Open();
            Assert.That(rdr[Helpers.R1].Eof(), Is.False);
            rdr[Helpers.R1].Close();
            rdr[Helpers.R2].Open();
            Assert.That(rdr[Helpers.R2].Eof(), Is.False);
            rdr[Helpers.R2].Close();
            rdr[Helpers.R3].Open();
            Assert.That(rdr[Helpers.R3].Eof(), Is.False);
            rdr[Helpers.R3].Close();
            rdr[Helpers.R4].Open();
            Assert.That(rdr[Helpers.R4].Eof(), Is.False);
            rdr[Helpers.R4].Close();
            rdr[Helpers.R5].Open();
            Assert.That(rdr[Helpers.R5].Eof(), Is.False);
            rdr[Helpers.R5].Close();
            rdr[Helpers.R6].Open();
            Assert.That(rdr[Helpers.R6].Eof(), Is.False);
            rdr[Helpers.R6].Close();
        }
        
        [Test]
        public void OpenCloseAll()
        {
            DataSourceReaderBase rdr = new DataSourceReaderBase();
            rdr.Add(Helpers.R1,
                new DataSourceRecordReaderEnumerable<Csvtest1>(Helpers.R1,
                    PrepareCsvtest1Collection()));
            rdr.Add(Helpers.R2,
                new DataSourceRecordReaderEnumerable<Csvtest2>(Helpers.R2,
                    PrepareCsvtest2Collection()));
            rdr.Add(Helpers.R3,
                new DataSourceRecordReaderEnumerable<Csvtest3>(Helpers.R3,
                    PrepareCsvtest3Collection()));
            rdr.Add(Helpers.R4,
                new DataSourceRecordReaderEnumerable<Csvtest4>(Helpers.R4,
                    PrepareCsvtest4Collection()));
            rdr.Add(Helpers.R5,
                new DataSourceRecordReaderEnumerable<Csvtest5>(Helpers.R5,
                    PrepareCsvtest5Collection()));
            rdr.Add(Helpers.R6,
                new DataSourceRecordReaderEnumerable<Csvtest6>(Helpers.R6,
                    PrepareCsvtest6Collection()));
            rdr.OpenAll();
            rdr.CloseAll();
        }
        
        [Test]
        public void ReadSingle()
        {
            DataSourceReaderBase rdr = new DataSourceReaderBase();
            DataSourceRecordReaderEnumerable<Csvtest1> rec = 
                new DataSourceRecordReaderEnumerable<Csvtest1>(Helpers.R1, 
                    PrepareCsvtest1Collection());
            rdr.Add(Helpers.R1, rec);
            rec.Open();
            for (int i = 0; i < 4; i++)
                Assert.That(rec.Read(), Is.True);
            Assert.That(rec.Read(), Is.False);
            rec.Close();
        }
        
        [Test]
        public void ReadMulti()
        {
            DataSourceReaderBase rdr = new DataSourceReaderBase();
            rdr.Add(Helpers.R1,
                new DataSourceRecordReaderEnumerable<Csvtest1>(Helpers.R1,
                    PrepareCsvtest1Collection()));
            rdr.Add(Helpers.R2,
                new DataSourceRecordReaderEnumerable<Csvtest2>(Helpers.R2,
                    PrepareCsvtest2Collection()));
            rdr.Add(Helpers.R3,
                new DataSourceRecordReaderEnumerable<Csvtest3>(Helpers.R3,
                    PrepareCsvtest3Collection()));
            rdr.Add(Helpers.R4,
                new DataSourceRecordReaderEnumerable<Csvtest4>(Helpers.R4,
                    PrepareCsvtest4Collection()));
            rdr.Add(Helpers.R5,
                new DataSourceRecordReaderEnumerable<Csvtest5>(Helpers.R5,
                    PrepareCsvtest5Collection()));
            rdr.Add(Helpers.R6,
                new DataSourceRecordReaderEnumerable<Csvtest6>(Helpers.R6,
                    PrepareCsvtest6Collection()));
            rdr[Helpers.R1].Open();
            rdr[Helpers.R2].Open();
            rdr[Helpers.R3].Open();
            rdr[Helpers.R4].Open();
            rdr[Helpers.R5].Open();
            rdr[Helpers.R6].Open();
            for (int i = 0; i < 4; i++)
            {
                Assert.That(rdr[Helpers.R1].Read(), Is.True);
                Assert.That(rdr[Helpers.R2].Read(), Is.True);
                Assert.That(rdr[Helpers.R3].Read(), Is.True);
                Assert.That(rdr[Helpers.R4].Read(), Is.True);
                Assert.That(rdr[Helpers.R5].Read(), Is.True);
                Assert.That(rdr[Helpers.R6].Read(), Is.True);
            }
            Assert.That(rdr[Helpers.R1].Read(), Is.False);
            //Records 2 and 3 have 8 records in db
            Assert.That(rdr[Helpers.R2].Read(), Is.True);
            Assert.That(rdr[Helpers.R3].Read(), Is.True);
            //Record 4 has 9 records in db
            for (int i = 0; i < 5; i++)
                Assert.That(rdr[Helpers.R4].Read(), Is.True);
            Assert.That(rdr[Helpers.R4].Read(), Is.False);
            //Record 5 has 8 records in db
            for (int i = 0; i < 4; i++)
                Assert.That(rdr[Helpers.R5].Read(), Is.True);
            Assert.That(rdr[Helpers.R5].Read(), Is.False);
            //Record 6 has 8 records in db
            for (int i = 0; i < 4; i++)
                Assert.That(rdr[Helpers.R6].Read(), Is.True);
            Assert.That(rdr[Helpers.R6].Read(), Is.False);
            rdr[Helpers.R1].Close();
            rdr[Helpers.R2].Close();
            rdr[Helpers.R3].Close();
            rdr[Helpers.R4].Close();
            rdr[Helpers.R5].Close();
            rdr[Helpers.R6].Close();
        }
        
        [Test]
        public void GetFieldSingle()
        {
            DataSourceField fld;
            DataSourceReaderBase rdr = new DataSourceReaderBase();
            DataSourceRecordReaderEnumerable<Csvtest1> rec =
                new DataSourceRecordReaderEnumerable<Csvtest1>(Helpers.R1,
                    PrepareCsvtest1Collection());
            rdr.Add(Helpers.R1, rec);
            rdr[Helpers.R1].Open();
            rdr[Helpers.R1].Read();
            fld = rdr[Helpers.R1].GetField("intfld");
            Assert.That(fld.Name, Is.EqualTo("intfld"));
            Assert.That((int)fld.Value, Is.EqualTo(1));
            fld = rdr[Helpers.R1].GetField("strfld");
            Assert.That((string)fld.Value, Is.EqualTo("aaa"));
            fld = rdr[Helpers.R1].GetField("doublefld");
            Assert.That((double)fld.Value, Is.EqualTo(100.1));
            fld = rdr[Helpers.R1].GetField("decimalfld");
            Assert.That((decimal)fld.Value, Is.EqualTo((decimal)1000.11));
            fld = rdr[Helpers.R1].GetField("dtfld");
            Assert.That((DateTime)fld.Value, Is.EqualTo(new DateTime(2001, 1, 11, 10, 11, 12)));
            fld = rdr[Helpers.R1].GetField("strfld2");
            Assert.That((string)fld.Value, Is.EqualTo("TEST1"));
            fld = rdr[Helpers.R1].GetField("boolfld");
            Assert.That((int)fld.Value, Is.EqualTo((int)1));
            rdr[Helpers.R1].Read();
            rdr[Helpers.R1].Read();
            fld = rdr[Helpers.R1].GetField("intfld");
            Assert.That((int)fld.Value, Is.EqualTo(3));
            fld = rdr[Helpers.R1].GetField("strfld");
            Assert.That((string)fld.Value, Is.EqualTo("ccc"));
            fld = rdr[Helpers.R1].GetField("doublefld");
            Assert.That((double)fld.Value, Is.EqualTo(300.3));
            fld = rdr[Helpers.R1].GetField("decimalfld");
            Assert.That((decimal)fld.Value, Is.EqualTo((decimal)3000.33));
            fld = rdr[Helpers.R1].GetField("dtfld");
            Assert.That((DateTime)fld.Value, Is.EqualTo(new DateTime(2003, 3, 13, 13, 14, 15)));
            fld = rdr[Helpers.R1].GetField("strfld2");
            Assert.That((string)fld.Value, Is.EqualTo("TEST3"));
            fld = rdr[Helpers.R1].GetField("boolfld");
            Assert.That((int)fld.Value, Is.EqualTo((int)1));
            rdr[Helpers.R1].Close();
        }

        [Test]
        public void GetFieldMulti()
        {
            DataSourceField fld;
            DataSourceReaderBase rdr = new DataSourceReaderBase();
            rdr.Add(Helpers.R1,
                new DataSourceRecordReaderEnumerable<Csvtest1>(Helpers.R1,
                    PrepareCsvtest1Collection()));
            rdr.Add(Helpers.R2,
                new DataSourceRecordReaderEnumerable<Csvtest2>(Helpers.R2,
                    PrepareCsvtest2Collection()));
            rdr.Add(Helpers.R3,
                new DataSourceRecordReaderEnumerable<Csvtest3>(Helpers.R3,
                    PrepareCsvtest3Collection()));
            rdr.Add(Helpers.R4,
                new DataSourceRecordReaderEnumerable<Csvtest4>(Helpers.R4,
                    PrepareCsvtest4Collection()));
            rdr.Add(Helpers.R5,
                new DataSourceRecordReaderEnumerable<Csvtest5>(Helpers.R5,
                    PrepareCsvtest5Collection()));
            rdr.Add(Helpers.R6,
                new DataSourceRecordReaderEnumerable<Csvtest6>(Helpers.R6,
                    PrepareCsvtest6Collection()));
            rdr[Helpers.R1].Open();
            rdr[Helpers.R2].Open();
            rdr[Helpers.R3].Open();
            rdr[Helpers.R4].Open();
            rdr[Helpers.R5].Open();
            rdr[Helpers.R6].Open();

            rdr[Helpers.R2].Read();
            fld = rdr[Helpers.R2].GetField("intr2");
            Assert.That(fld.Name, Is.EqualTo("intr2"));
            Assert.That((int)fld.Value, Is.EqualTo(1));
            fld = rdr[Helpers.R2].GetField("strr2");
            Assert.That((string)fld.Value, Is.EqualTo("r2_1"));
            fld = rdr[Helpers.R2].GetField("bool2");
            Assert.That((string)fld.Value, Is.EqualTo("T"));
            rdr[Helpers.R2].Read();
            rdr[Helpers.R2].Read();
            fld = rdr[Helpers.R2].GetField("intr2");
            Assert.That(fld.Name, Is.EqualTo("intr2"));
            Assert.That((int)fld.Value, Is.EqualTo(3));
            fld = rdr[Helpers.R2].GetField("intr2left");
            Assert.That(fld.Name, Is.EqualTo("intr2left"));
            Assert.That((int)fld.Value, Is.EqualTo(33));
            fld = rdr[Helpers.R2].GetField("strr2");
            Assert.That((string)fld.Value, Is.EqualTo("r2_3"));
            fld = rdr[Helpers.R2].GetField("bool2");
            Assert.That((string)fld.Value, Is.EqualTo("T"));

            rdr[Helpers.R3].Read();
            fld = rdr[Helpers.R3].GetField("intr3");
            Assert.That(fld.Name, Is.EqualTo("intr3"));
            Assert.That((int)fld.Value, Is.EqualTo(1));
            fld = rdr[Helpers.R3].GetField("strr3");
            Assert.That((string)fld.Value, Is.EqualTo("r3_1"));
            rdr[Helpers.R3].Read();
            rdr[Helpers.R3].Read();
            fld = rdr[Helpers.R3].GetField("intr3");
            Assert.That(fld.Name, Is.EqualTo("intr3"));
            Assert.That((int)fld.Value, Is.EqualTo(3));
            fld = rdr[Helpers.R3].GetField("strr3");
            Assert.That((string)fld.Value, Is.EqualTo("r3_3"));

            rdr[Helpers.R4].Read();
            fld = rdr[Helpers.R4].GetField("intr4");
            Assert.That(fld.Name, Is.EqualTo("intr4"));
            Assert.That((int)fld.Value, Is.EqualTo(1));
            fld = rdr[Helpers.R4].GetField("doubler4");
            Assert.That((double)fld.Value, Is.EqualTo(11.1));
            fld = rdr[Helpers.R4].GetField("decimalr4");
            Assert.That((decimal)fld.Value, Is.EqualTo((decimal)111.11));
            rdr[Helpers.R4].Read();
            rdr[Helpers.R4].Read();
            fld = rdr[Helpers.R4].GetField("intr4");
            Assert.That(fld.Name, Is.EqualTo("intr4"));
            Assert.That((int)fld.Value, Is.EqualTo(3));
            fld = rdr[Helpers.R4].GetField("doubler4");
            Assert.That((double)fld.Value, Is.EqualTo(33.3));
            fld = rdr[Helpers.R4].GetField("decimalr4");
            Assert.That((decimal)fld.Value, Is.EqualTo((decimal)333.33));

            rdr[Helpers.R5].Read();
            fld = rdr[Helpers.R5].GetField("intr5");
            Assert.That(fld.Name, Is.EqualTo("intr5"));
            Assert.That((int)fld.Value, Is.EqualTo(1));
            fld = rdr[Helpers.R5].GetField("strr5");
            Assert.That(fld.Name, Is.EqualTo("strr5"));
            Assert.That(fld.Value, Is.EqualTo("AA"));
            rdr[Helpers.R5].Read();
            rdr[Helpers.R5].Read();
            rdr[Helpers.R5].Read();
            fld = rdr[Helpers.R5].GetField("intr5");
            Assert.That(fld.Name, Is.EqualTo("intr5"));
            Assert.That((int)fld.Value, Is.EqualTo(4));
            fld = rdr[Helpers.R5].GetField("strr5");
            Assert.That(fld.Name, Is.EqualTo("strr5"));
            Assert.That(fld.Value, Is.EqualTo("DD"));

            rdr[Helpers.R6].Read();
            fld = rdr[Helpers.R6].GetField("intr6");
            Assert.That(fld.Name, Is.EqualTo("intr6"));
            Assert.That((int)fld.Value, Is.EqualTo(11));
            fld = rdr[Helpers.R6].GetField("strr6");
            Assert.That(fld.Name, Is.EqualTo("strr6"));
            Assert.That(fld.Value, Is.EqualTo("AAA"));
            rdr[Helpers.R6].Read();
            rdr[Helpers.R6].Read();
            fld = rdr[Helpers.R6].GetField("intr6");
            Assert.That(fld.Name, Is.EqualTo("intr6"));
            Assert.That((int)fld.Value, Is.EqualTo(33));
            fld = rdr[Helpers.R6].GetField("strr6");
            Assert.That(fld.Name, Is.EqualTo("strr6"));
            Assert.That(fld.Value, Is.EqualTo("CCC"));
        }

        private List<Csvtest1> PrepareCsvtest1Collection()
        {
            List<Csvtest1> c = new List<Csvtest1>();
            c.Add(new Csvtest1(1, "aaa", 100.10, 1000.11M, 
                new DateTime(2001, 1, 11, 10, 11, 12), "TEST1", 1));
            c.Add(new Csvtest1(2, "bbb", 200.20, 2000.22M, 
                new DateTime(2002, 2, 12, 12, 13, 14), "TEST2", 0));
            c.Add(new Csvtest1(3, "ccc", 300.30, 3000.33M, 
                new DateTime(2003, 3, 13, 13, 14, 15), "TEST3", 1));
            c.Add(new Csvtest1(4, "ddd", 400.40, 4000.44M, 
                new DateTime(2004, 4, 14, 14, 15, 16), "TEST4", 0));
            return c;
        }

        private List<Csvtest2> PrepareCsvtest2Collection()
        {
            List<Csvtest2> c = new List<Csvtest2>();
            c.Add(new Csvtest2(1, 11, "r2_1", "T"));
            c.Add(new Csvtest2(2, 22, "r2_2", "F"));
            c.Add(new Csvtest2(3, 33, "r2_3", "T"));
            c.Add(new Csvtest2(4, 44, null, "F"));
            c.Add(new Csvtest2(5, 55, "r2_5", "T"));
            c.Add(new Csvtest2(6, 66, "r2_6", "F"));
            c.Add(new Csvtest2(7, 77, "r2_7", "T"));
            c.Add(new Csvtest2(8, 88, "r2_8", "F"));
            c.Add(new Csvtest2(9, 99, "r2_9", "F"));
            c.Add(new Csvtest2(10, 100, "r2_10", "F"));
            c.Add(new Csvtest2(11, 110, "r2_11", "F"));
            return c;
        }

        private List<Csvtest3> PrepareCsvtest3Collection()
        {
            List<Csvtest3> c = new List<Csvtest3>();
            c.Add(new Csvtest3(1, "r3_1"));
            c.Add(new Csvtest3(2, "r3_2"));
            c.Add(new Csvtest3(3, "r3_3"));
            c.Add(new Csvtest3(4, "r3_4"));
            c.Add(new Csvtest3(5, "r3_5"));
            c.Add(new Csvtest3(6, "r3_6"));
            c.Add(new Csvtest3(7, "r3_7"));
            c.Add(new Csvtest3(8, "r3_8"));
            c.Add(new Csvtest3(9, "r3_9"));
            c.Add(new Csvtest3(10, "r3_10"));
            c.Add(new Csvtest3(11, "r3_11"));
            c.Add(new Csvtest3(12, "r3_12"));
            return c;
        }

        private List<Csvtest4> PrepareCsvtest4Collection()
        {
            List<Csvtest4> c = new List<Csvtest4>();
            c.Add(new Csvtest4(1, 11.1, 111.11M));
            c.Add(new Csvtest4(2, 22.2, 222.22M));
            c.Add(new Csvtest4(3, 33.3, 333.33M));
            c.Add(new Csvtest4(4, 44.4, 444.44M));
            c.Add(new Csvtest4(5, 55.5, 555.55M));
            c.Add(new Csvtest4(6, 66.6, 666.66M));
            c.Add(new Csvtest4(7, 77.7, 777.77M));
            c.Add(new Csvtest4(8, 88.8, 888.88M));
            c.Add(new Csvtest4(9, 99.9, 999.99M));
            return c;
        }

        private List<Csvtest5> PrepareCsvtest5Collection()
        {
            List<Csvtest5> c = new List<Csvtest5>();
            c.Add(new Csvtest5(1, "AA"));
            c.Add(new Csvtest5(2, "BB"));
            c.Add(new Csvtest5(3, "CC"));
            c.Add(new Csvtest5(4, "DD"));
            c.Add(new Csvtest5(5, "EE"));
            c.Add(new Csvtest5(6, "FF"));
            c.Add(new Csvtest5(7, "GG"));
            c.Add(new Csvtest5(8, "HH"));
            return c;
        }

        private List<Csvtest6> PrepareCsvtest6Collection()
        {
            List<Csvtest6> c = new List<Csvtest6>();
            c.Add(new Csvtest6(11, "AAA"));
            c.Add(new Csvtest6(22, "BBB"));
            c.Add(new Csvtest6(33, "CCC"));
            c.Add(new Csvtest6(44, "DDD"));
            c.Add(new Csvtest6(55, "EEE"));
            c.Add(new Csvtest6(66, "FFF"));
            c.Add(new Csvtest6(77, "GGG"));
            c.Add(new Csvtest6(88, "HHH"));
            return c;
        }

        public class Csvtest1
        {
            public int _intfld;
            public int intfld { get { return _intfld; } set { _intfld = value; } }
            public string _strfld;
            public string strfld { get { return _strfld; } set { _strfld = value; } }
            public double _doublefld;
            public double doublefld { get { return _doublefld; } set { _doublefld = value; } }
            public decimal _decimalfld;
            public decimal decimalfld { get { return _decimalfld; } set { _decimalfld = value; } }
            public DateTime _dtfld;
            public DateTime dtfld { get { return _dtfld; } set { _dtfld = value; } }
            public string _strfld2;
            public string strfld2 { get { return _strfld2; } set { _strfld2 = value; } }
            public int _boolfld;
            public int boolfld { get { return _boolfld; } set { _boolfld = value; } }

            public Csvtest1(int intfldVal, string strfldVal, 
                double doublefldVal, decimal decimalfldVal, 
                DateTime dtfldVal, string strfld2Val, int boolfldVal)
            {
                intfld = intfldVal;
                strfld = strfldVal;
                doublefld = doublefldVal;
                decimalfld = decimalfldVal;
                dtfld = dtfldVal;
                strfld2 = strfld2Val;
                boolfld = boolfldVal;
            }
        }

        public class Csvtest2
        {
            public int _intr2;
            public int intr2 { get { return _intr2; } set { _intr2 = value; } }
            public int _intr2left;
            public int intr2left { get { return _intr2left; } set { _intr2left = value; } }
            public string _strr2;
            public string strr2 { get { return _strr2; } set { _strr2 = value; } }
            public string _bool2;
            public string bool2 { get { return _bool2; } set { _bool2 = value; } }

            public Csvtest2(int intr2Val, int intr2leftVal, string strr2Val,
                string bool2Val)
            {
                intr2 = intr2Val;
                intr2left = intr2leftVal;
                strr2 = strr2Val;
                bool2 = bool2Val;
            }
        }

        public class Csvtest3
        {
            public int _intr3;
            public int intr3 { get { return _intr3; } set { _intr3 = value; } }
            public string _strr3;
            public string strr3 { get { return _strr3; } set { _strr3 = value; } }

            public Csvtest3(int intr3Val, string strr3Val)
            {
                intr3 = intr3Val;
                strr3 = strr3Val;
            }
        }

        public class Csvtest4
        {
            public int _intr4;
            public int intr4 { get { return _intr4; } set { _intr4 = value; } }
            public double _doubler4;
            public double doubler4 { get { return _doubler4; } set { _doubler4 = value; } }
            public decimal _decimalr4;
            public decimal decimalr4 { get { return _decimalr4; } set { _decimalr4 = value; } }

            public Csvtest4(int intr4Val, double doubler4Val,
                decimal decimalr4Val)
            {
                intr4 = intr4Val;
                doubler4 = doubler4Val;
                decimalr4 = decimalr4Val;
            }
        }

        public class Csvtest5
        {
            public int _intr5;
            public int intr5 { get { return _intr5; } set { _intr5 = value; } }
            public string _strr5;
            public string strr5 { get { return _strr5; } set { _strr5 = value; } }

            public Csvtest5(int intr5Val, string strr5Val)
            {
                intr5 = intr5Val;
                strr5 = strr5Val;
            }
        }

        public class Csvtest6
        {
            public int _intr6;
            public int intr6 { get { return _intr6; } set { _intr6 = value; } }
            public string _strr6;
            public string strr6 { get { return _strr6; } set { _strr6 = value; } }

            public Csvtest6(int intr6Val, string strr6Val)
            {
                intr6 = intr6Val;
                strr6 = strr6Val;
            }
        }
    }
}
