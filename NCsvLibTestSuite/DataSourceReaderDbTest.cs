using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using NCsvLib;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.IO;

namespace NCsvLibTestSuite
{
  [TestFixture]
  public class DbInputReaderTest
  {
    DbConnection[] Conn;
    

    [SetUp]
    public void SetUp()
    {
      Helpers.CreateEnvironment();
      Conn = new DbConnection[4];
      for (int i=0; i<Conn.Length; i++)
        Conn[i] = Helpers.GetDbConnectionFromFile(Helpers.ConnStrFileName);
    }

    [TearDown]
    public void TearDown()
    {
      for (int i = 0; i < Conn.Length; i++)
      {
        if (Conn[i].State == System.Data.ConnectionState.Open)
          Conn[i].Close();
      }
    }

    [Test]
    public void OpenCloseSingle()
    {
      DataSourceReaderBase rdr = new DataSourceReaderBase();
      DataSourceRecordReaderDb rec = new DataSourceRecordReaderDb(Helpers.R1, Conn[0], Helpers.Qry1);
      rdr.Add(Helpers.R1, rec);
      rec.Open();
      rec.Close();
    }

    [Test]
    public void OpenCloseMulti()
    {
      DataSourceReaderBase rdr = new DataSourceReaderBase();
      rdr.Add(Helpers.R1, new DataSourceRecordReaderDb(Helpers.R1, Conn[0], Helpers.Qry1));
      rdr.Add(Helpers.R2, new DataSourceRecordReaderDb(Helpers.R2, Conn[1], Helpers.Qry2));
      rdr.Add(Helpers.R3, new DataSourceRecordReaderDb(Helpers.R3, Conn[2], Helpers.Qry3));
      rdr.Add(Helpers.R4, new DataSourceRecordReaderDb(Helpers.R4, Conn[3], Helpers.Qry4));
      rdr[Helpers.R1].Open();
      rdr[Helpers.R1].Close();
      rdr[Helpers.R2].Open();
      rdr[Helpers.R2].Close();
      rdr[Helpers.R3].Open();
      rdr[Helpers.R3].Close();
      rdr[Helpers.R4].Open();
      rdr[Helpers.R4].Close();
    }

    [Test]
    public void OpenCloseAll()
    {
      DataSourceReaderBase rdr = new DataSourceReaderBase();
      rdr.OpenAll();
      rdr.CloseAll();
    }

    [Test]
    public void ReadSingle()
    {
      DataSourceReaderBase rdr = new DataSourceReaderBase();
      DataSourceRecordReaderDb rec = new DataSourceRecordReaderDb(Helpers.R1, Conn[0], Helpers.Qry1);
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
      rdr.Add(Helpers.R1, new DataSourceRecordReaderDb(Helpers.R1, Conn[0], Helpers.Qry1));
      rdr.Add(Helpers.R2, new DataSourceRecordReaderDb(Helpers.R2, Conn[1], Helpers.Qry2));
      rdr.Add(Helpers.R3, new DataSourceRecordReaderDb(Helpers.R3, Conn[2], Helpers.Qry3));
      rdr.Add(Helpers.R4, new DataSourceRecordReaderDb(Helpers.R4, Conn[3], Helpers.Qry4));
      rdr[Helpers.R1].Open();
      rdr[Helpers.R2].Open();
      rdr[Helpers.R3].Open();
      rdr[Helpers.R4].Open();
      for (int i = 0; i < 4; i++)
      {
        Assert.That(rdr[Helpers.R1].Read(), Is.True);
        Assert.That(rdr[Helpers.R2].Read(), Is.True);
        Assert.That(rdr[Helpers.R3].Read(), Is.True);
        Assert.That(rdr[Helpers.R4].Read(), Is.True);
      }
      Assert.That(rdr[Helpers.R1].Read(), Is.False);
      //Records 2 and 3 have 8 records in db
      Assert.That(rdr[Helpers.R2].Read(), Is.True);
      Assert.That(rdr[Helpers.R3].Read(), Is.True);
      //Record 4 has 9 records in db
      for (int i = 0; i < 5; i++)
        Assert.That(rdr[Helpers.R4].Read(), Is.True);
      Assert.That(rdr[Helpers.R4].Read(), Is.False);
      rdr[Helpers.R1].Close();
      rdr[Helpers.R2].Close();
      rdr[Helpers.R3].Close();
      rdr[Helpers.R4].Close();
    }

    [Test]
    public void GetFieldSingle()
    {
      DataSourceField fld;
      DataSourceReaderBase rdr = new DataSourceReaderBase();
      rdr.Add(Helpers.R1, new DataSourceRecordReaderDb(Helpers.R1, Conn[0], Helpers.Qry1));      
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
      rdr.Add(Helpers.R1, new DataSourceRecordReaderDb(Helpers.R1, Conn[0], Helpers.Qry1));
      rdr.Add(Helpers.R2, new DataSourceRecordReaderDb(Helpers.R2, Conn[1], Helpers.Qry2));
      rdr.Add(Helpers.R3, new DataSourceRecordReaderDb(Helpers.R3, Conn[2], Helpers.Qry3));
      rdr.Add(Helpers.R4, new DataSourceRecordReaderDb(Helpers.R4, Conn[3], Helpers.Qry4));
      rdr[Helpers.R1].Open();
      rdr[Helpers.R2].Open();
      rdr[Helpers.R3].Open();
      rdr[Helpers.R4].Open();

      rdr[Helpers.R2].Read();
      fld = rdr[Helpers.R2].GetField("intr2");
      Assert.That(fld.Name, Is.EqualTo("intr2"));
      Assert.That((int)fld.Value, Is.EqualTo(1));
      fld = rdr[Helpers.R2].GetField("strr2");
      Assert.That((string)fld.Value, Is.EqualTo("r2_1"));
      fld = rdr[Helpers.R2].GetField("bool2");
      Assert.That((string)fld.Value, Text.Matches("T"));
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
      Assert.That((string)fld.Value, Text.Matches("T"));

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
    }
  }
}
