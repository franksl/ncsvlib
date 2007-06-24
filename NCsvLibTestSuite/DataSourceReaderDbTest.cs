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
      rdr.Open(Helpers.R1);
      rdr.Close(Helpers.R1);
    }

    [Test]
    public void OpenCloseMulti()
    {
      DataSourceReaderDb rdr = new DataSourceReaderDb();
      rdr.AddCommand(Helpers.R1, new DataSourceReaderDbCommand(Conn[0], Helpers.Qry1));
      rdr.AddCommand(Helpers.R2, new DataSourceReaderDbCommand(Conn[1], Helpers.Qry2));
      rdr.AddCommand(Helpers.R3, new DataSourceReaderDbCommand(Conn[2], Helpers.Qry3));
      rdr.AddCommand(Helpers.R4, new DataSourceReaderDbCommand(Conn[3], Helpers.Qry4));
      rdr.Open(Helpers.R1);
      rdr.Close(Helpers.R1);
      rdr.Open(Helpers.R2);
      rdr.Close(Helpers.R2);
      rdr.Open(Helpers.R3);
      rdr.Close(Helpers.R3);
      rdr.Open(Helpers.R4);
      rdr.Close(Helpers.R4);
    }
      

    [Test]
    public void ReadSingle()
    {
      DataSourceReaderDb rdr = new DataSourceReaderDb(Helpers.R1, Conn[0], Helpers.Qry1);
      rdr.Open(Helpers.R1);
      for (int i = 0; i < 4; i++)
        Assert.That(rdr.Read(Helpers.R1), Is.True);
      Assert.That(rdr.Read(Helpers.R1), Is.False);
      rdr.Close(Helpers.R1);
    }

    [Test]
    public void ReadMulti()
    {
      DataSourceReaderDb rdr = new DataSourceReaderDb();
      rdr.AddCommand(Helpers.R1, new DataSourceReaderDbCommand(Conn[0], Helpers.Qry1));
      rdr.AddCommand(Helpers.R2, new DataSourceReaderDbCommand(Conn[1], Helpers.Qry2));
      rdr.AddCommand(Helpers.R3, new DataSourceReaderDbCommand(Conn[2], Helpers.Qry3));
      rdr.AddCommand(Helpers.R4, new DataSourceReaderDbCommand(Conn[3], Helpers.Qry4));
      rdr.Open(Helpers.R1);
      rdr.Open(Helpers.R2);
      rdr.Open(Helpers.R3);
      rdr.Open(Helpers.R4);
      for (int i = 0; i < 4; i++)
      {
        Assert.That(rdr.Read(Helpers.R1), Is.True);
        Assert.That(rdr.Read(Helpers.R2), Is.True);
        Assert.That(rdr.Read(Helpers.R3), Is.True);
        Assert.That(rdr.Read(Helpers.R4), Is.True);
      }
      Assert.That(rdr.Read(Helpers.R1), Is.False);
      Assert.That(rdr.Read(Helpers.R2), Is.False);
      Assert.That(rdr.Read(Helpers.R3), Is.False);
      Assert.That(rdr.Read(Helpers.R4), Is.False);
      rdr.Close(Helpers.R1);
      rdr.Close(Helpers.R2);
      rdr.Close(Helpers.R3);
      rdr.Close(Helpers.R4);
    }

    [Test]
    public void GetFieldSingle()
    {
      DataSourceReaderDb rdr = new DataSourceReaderDb(Helpers.R1, Conn[0], Helpers.Qry1);
      DataSourceField fld;
      rdr.Open(Helpers.R1);
      rdr.Read(Helpers.R1);
      fld = rdr.GetField(Helpers.R1, "intfld");
      Assert.That(fld.Name, Is.EqualTo("intfld"));
      Assert.That((int)fld.Value, Is.EqualTo(1));
      fld = rdr.GetField(Helpers.R1, "strfld");
      Assert.That((string)fld.Value, Is.EqualTo("aaa"));
      fld = rdr.GetField(Helpers.R1, "doublefld");
      Assert.That((double)fld.Value, Is.EqualTo(100.1));
      fld = rdr.GetField(Helpers.R1, "decimalfld");
      Assert.That((decimal)fld.Value, Is.EqualTo((decimal)1000.11));
      rdr.Read(Helpers.R1);
      rdr.Read(Helpers.R1);
      fld = rdr.GetField(Helpers.R1, "intfld");
      Assert.That((int)fld.Value, Is.EqualTo(3));
      fld = rdr.GetField(Helpers.R1, "strfld");
      Assert.That((string)fld.Value, Is.EqualTo("ccc"));
      fld = rdr.GetField(Helpers.R1, "doublefld");
      Assert.That((double)fld.Value, Is.EqualTo(300.3));
      fld = rdr.GetField(Helpers.R1, "decimalfld");
      Assert.That((decimal)fld.Value, Is.EqualTo((decimal)3000.33));
      rdr.Close(Helpers.R1);
    }

    [Test]
    public void GetFieldMulti()
    {
      DataSourceField fld;
      DataSourceReaderDb rdr = new DataSourceReaderDb();
      rdr.AddCommand(Helpers.R1, new DataSourceReaderDbCommand(Conn[0], Helpers.Qry1));
      rdr.AddCommand(Helpers.R2, new DataSourceReaderDbCommand(Conn[1], Helpers.Qry2));
      rdr.AddCommand(Helpers.R3, new DataSourceReaderDbCommand(Conn[2], Helpers.Qry3));
      rdr.AddCommand(Helpers.R4, new DataSourceReaderDbCommand(Conn[3], Helpers.Qry4));
      rdr.Open(Helpers.R1);
      rdr.Open(Helpers.R2);
      rdr.Open(Helpers.R3);
      rdr.Open(Helpers.R4);

      rdr.Read(Helpers.R2);
      fld = rdr.GetField(Helpers.R2, "intr2");
      Assert.That(fld.Name, Is.EqualTo("intr2"));
      Assert.That((int)fld.Value, Is.EqualTo(1));
      fld = rdr.GetField(Helpers.R2, "strr2");
      Assert.That((string)fld.Value, Is.EqualTo("r2_1"));
      rdr.Read(Helpers.R2);
      rdr.Read(Helpers.R2);
      fld = rdr.GetField(Helpers.R2, "intr2");
      Assert.That(fld.Name, Is.EqualTo("intr2"));
      Assert.That((int)fld.Value, Is.EqualTo(3));
      fld = rdr.GetField(Helpers.R2, "strr2");
      Assert.That((string)fld.Value, Is.EqualTo("r2_3"));

      rdr.Read(Helpers.R3);
      fld = rdr.GetField(Helpers.R3, "intr3");
      Assert.That(fld.Name, Is.EqualTo("intr3"));
      Assert.That((int)fld.Value, Is.EqualTo(1));
      fld = rdr.GetField(Helpers.R3, "strr3");
      Assert.That((string)fld.Value, Is.EqualTo("r3_1"));
      rdr.Read(Helpers.R3);
      rdr.Read(Helpers.R3);
      fld = rdr.GetField(Helpers.R3, "intr3");
      Assert.That(fld.Name, Is.EqualTo("intr3"));
      Assert.That((int)fld.Value, Is.EqualTo(3));
      fld = rdr.GetField(Helpers.R3, "strr3");
      Assert.That((string)fld.Value, Is.EqualTo("r3_3"));

      rdr.Read(Helpers.R4);
      fld = rdr.GetField(Helpers.R4, "intr4");
      Assert.That(fld.Name, Is.EqualTo("intr4"));
      Assert.That((int)fld.Value, Is.EqualTo(1));
      fld = rdr.GetField(Helpers.R4, "doubler4");
      Assert.That((double)fld.Value, Is.EqualTo(11.1));
      fld = rdr.GetField(Helpers.R4, "decimalr4");
      Assert.That((decimal)fld.Value, Is.EqualTo((decimal)111.11));
      rdr.Read(Helpers.R4);
      rdr.Read(Helpers.R4);
      fld = rdr.GetField(Helpers.R4, "intr4");
      Assert.That(fld.Name, Is.EqualTo("intr4"));
      Assert.That((int)fld.Value, Is.EqualTo(3));
      fld = rdr.GetField(Helpers.R4, "doubler4");
      Assert.That((double)fld.Value, Is.EqualTo(33.3));
      fld = rdr.GetField(Helpers.R4, "decimalr4");
      Assert.That((decimal)fld.Value, Is.EqualTo((decimal)333.33));
    }
  }
}
