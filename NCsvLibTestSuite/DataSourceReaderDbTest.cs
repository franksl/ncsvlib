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
      DataSourceReaderDb rdr = new DataSourceReaderDb(Helpers.R1, Conn[0], Helpers.Qry1);
      DataSourceRecordReaderDb rec = rdr[Helpers.R1];
      rec.Open();
      rec.Close();
    }

    [Test]
    public void OpenCloseMulti()
    {
      DataSourceReaderDb rdr = new DataSourceReaderDb();
      rdr.Add(Helpers.R1, rdr.CreateRecordReader(Helpers.R1, Conn[0], Helpers.Qry1));
      rdr.Add(Helpers.R2, rdr.CreateRecordReader(Helpers.R2, Conn[1], Helpers.Qry2));
      rdr.Add(Helpers.R3, rdr.CreateRecordReader(Helpers.R3, Conn[2], Helpers.Qry3));
      rdr.Add(Helpers.R4, rdr.CreateRecordReader(Helpers.R4, Conn[3], Helpers.Qry4));
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
    public void ReadSingle()
    {
      DataSourceReaderDb rdr = new DataSourceReaderDb(Helpers.R1, Conn[0], Helpers.Qry1);
      DataSourceRecordReaderDb rec = rdr[Helpers.R1];
      rec.Open();
      for (int i = 0; i < 4; i++)
        Assert.That(rec.Read(), Is.True);
      Assert.That(rec.Read(), Is.False);
      rec.Close();
    }

    [Test]
    public void ReadMulti()
    {
      DataSourceReaderDb rdr = new DataSourceReaderDb();
      rdr.Add(Helpers.R1, rdr.CreateRecordReader(Helpers.R1, Conn[0], Helpers.Qry1));
      rdr.Add(Helpers.R2, rdr.CreateRecordReader(Helpers.R2, Conn[1], Helpers.Qry2));
      rdr.Add(Helpers.R3, rdr.CreateRecordReader(Helpers.R3, Conn[2], Helpers.Qry3));
      rdr.Add(Helpers.R4, rdr.CreateRecordReader(Helpers.R4, Conn[3], Helpers.Qry4));
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
      Assert.That(rdr[Helpers.R2].Read(), Is.False);
      Assert.That(rdr[Helpers.R3].Read(), Is.False);
      Assert.That(rdr[Helpers.R4].Read(), Is.False);
      rdr[Helpers.R1].Close();
      rdr[Helpers.R2].Close();
      rdr[Helpers.R3].Close();
      rdr[Helpers.R4].Close();
    }

    [Test]
    public void GetFieldSingle()
    {
      DataSourceReaderDb rdr = new DataSourceReaderDb(Helpers.R1, Conn[0], Helpers.Qry1);
      DataSourceField fld;
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
      rdr[Helpers.R1].Close();
    }

    [Test]
    public void GetFieldMulti()
    {
      DataSourceField fld;
      DataSourceReaderDb rdr = new DataSourceReaderDb();
      rdr.Add(Helpers.R1, rdr.CreateRecordReader(Helpers.R1, Conn[0], Helpers.Qry1));
      rdr.Add(Helpers.R2, rdr.CreateRecordReader(Helpers.R2, Conn[1], Helpers.Qry2));
      rdr.Add(Helpers.R3, rdr.CreateRecordReader(Helpers.R3, Conn[2], Helpers.Qry3));
      rdr.Add(Helpers.R4, rdr.CreateRecordReader(Helpers.R4, Conn[3], Helpers.Qry4));
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
