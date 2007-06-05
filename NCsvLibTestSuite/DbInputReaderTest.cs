using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using NCsvLib;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.IO;
using MySql.Data.MySqlClient;

namespace NCsvLibTestSuite
{
  [TestFixture]
  public class DbInputReaderTest
  {
    DbConnection Conn;
    private readonly string qry = "SELECT * FROM csvtest1";

    [SetUp]
    public void SetUp()
    {
      string s = string.Empty;
      using (StreamReader sr = new StreamReader("../../dbconnstrtest.txt"))
      {
        s = sr.ReadLine();
        sr.Close();
      }
      if (s == string.Empty)
        throw new Exception("Error while reading connection string");
      Conn = new MySqlConnection(s);
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
      DbDataSourceReader rdr = new DbDataSourceReader(Conn, qry);
      rdr.Open();
      rdr.Close();
    }

    [Test]
    public void Read()
    {
      bool b;
      DbDataSourceReader rdr = new DbDataSourceReader(Conn, qry);
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
      rdr.Close();
    }

    [Test]
    public void GetField()
    {
      DbDataSourceReader rdr = new DbDataSourceReader(Conn, qry);
      InputField fld;
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
      rdr.Close();
    }
  }
}
