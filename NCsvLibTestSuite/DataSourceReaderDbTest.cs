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
    DbConnection Conn;
    private static readonly string _Qry = "SELECT * FROM csvtest1";
    

    [SetUp]
    public void SetUp()
    {
      Conn = Helpers.GetDbConnectionFromFile(Helpers.ConnStrFileName);
    }

    [TearDown]
    public void TearDown()
    {
      if (Conn.State == System.Data.ConnectionState.Open)
        Conn.Close();
    }

    [Test]
    public void OpenClose()
    {
     /* DataSourceReaderDb rdr = new DataSourceReaderDb(Conn, _Qry);
      rdr.Open();
      rdr.Close();*/
    }

    [Test]
    public void Read()
    {
     /* bool b;
      DataSourceReaderDb rdr = new DataSourceReaderDb(Conn, _Qry);
      rdr.Open();
      b = rdr.Read();
      Assert.That(b, Is.True);
      b = rdr.Read();
      Assert.That(b, Is.True);
      b = rdr.Read();
      Assert.That(b, Is.True);
      b = rdr.Read();
      Assert.That(b, Is.True);
      b = rdr.Read();
      Assert.That(b, Is.False);
      rdr.Close();*/
    }

    [Test]
    public void GetField()
    {
      /*DataSourceReaderDb rdr = new DataSourceReaderDb(Conn, _Qry);
      DataSourceField fld;
      rdr.Open();
      rdr.Read();
      fld = rdr.GetField("intfld");
      Assert.That(fld.Name, Is.EqualTo("intfld"));
      Assert.That((int)fld.Value, Is.EqualTo(1));
      fld = rdr.GetField("strfld");
      Assert.That((string)fld.Value, Is.EqualTo("aaa"));
      fld = rdr.GetField("doublefld");
      Assert.That((double)fld.Value, Is.EqualTo(100.1));
      fld = rdr.GetField("decimalfld");
      Assert.That((decimal)fld.Value, Is.EqualTo((decimal)1000.11));
      rdr.Read();
      rdr.Read();
      fld = rdr.GetField("intfld");
      Assert.That((int)fld.Value, Is.EqualTo(3));
      fld = rdr.GetField("strfld");
      Assert.That((string)fld.Value, Is.EqualTo("ccc"));
      fld = rdr.GetField("doublefld");
      Assert.That((double)fld.Value, Is.EqualTo(300.3));
      fld = rdr.GetField("decimalfld");
      Assert.That((decimal)fld.Value, Is.EqualTo((decimal)3000.33));
      rdr.Close();*/
    }
  }
}
