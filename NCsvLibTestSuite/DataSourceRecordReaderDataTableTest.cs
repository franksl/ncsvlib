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
  public class DataSourceRecordReaderDataTableTest
  {
    IDbConnection[] Conn;
    
    [SetUp]
    public void SetUp()
    {
      Helpers.CreateEnvironment();
      Conn = new IDbConnection[6];
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
			DataTable tbl = GetDataTable(Conn[0], Helpers.Qry1);
      DataSourceRecordReaderDataTable rec =
				new DataSourceRecordReaderDataTable(Helpers.R1, tbl);
      rdr.Add(Helpers.R1, rec);
      rec.Open();
			Assert.That(rdr[Helpers.R1].Eof(), Is.False);
      rec.Close();
    }

    [Test]
    public void OpenCloseMulti()
    {
      DataSourceReaderBase rdr = new DataSourceReaderBase();
      rdr.Add(Helpers.R1, new DataSourceRecordReaderDataTable(
				Helpers.R1, GetDataTable(Conn[0], Helpers.Qry1)));
      rdr.Add(Helpers.R2, new DataSourceRecordReaderDataTable(
				Helpers.R2, GetDataTable(Conn[1], Helpers.Qry2)));
      rdr.Add(Helpers.R3, new DataSourceRecordReaderDataTable(
				Helpers.R3, GetDataTable(Conn[2], Helpers.Qry3)));
      rdr.Add(Helpers.R4, new DataSourceRecordReaderDataTable(
				Helpers.R4, GetDataTable(Conn[3], Helpers.Qry4)));
			rdr.Add(Helpers.R5, new DataSourceRecordReaderDataTable(
				Helpers.R5, GetDataTable(Conn[4], Helpers.Qry5)));
			rdr.Add(Helpers.R6, new DataSourceRecordReaderDataTable(
				Helpers.R6, GetDataTable(Conn[5], Helpers.Qry6)));
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
      rdr.Add(Helpers.R1, new DataSourceRecordReaderDataTable(
              Helpers.R1, GetDataTable(Conn[0], Helpers.Qry1)));
      rdr.Add(Helpers.R2, new DataSourceRecordReaderDataTable(
                Helpers.R2, GetDataTable(Conn[1], Helpers.Qry2)));
      rdr.Add(Helpers.R3, new DataSourceRecordReaderDataTable(
                Helpers.R3, GetDataTable(Conn[2], Helpers.Qry3)));
      rdr.Add(Helpers.R4, new DataSourceRecordReaderDataTable(
                Helpers.R4, GetDataTable(Conn[3], Helpers.Qry4)));
      rdr.Add(Helpers.R5, new DataSourceRecordReaderDataTable(
          Helpers.R5, GetDataTable(Conn[4], Helpers.Qry5)));
      rdr.Add(Helpers.R6, new DataSourceRecordReaderDataTable(
          Helpers.R6, GetDataTable(Conn[5], Helpers.Qry6)));
      rdr.OpenAll();
      rdr.CloseAll();
    }

    [Test]
    public void ReadSingle()
    {
      DataSourceReaderBase rdr = new DataSourceReaderBase();
      DataSourceRecordReaderDataTable rec 
				= new DataSourceRecordReaderDataTable(Helpers.R1, GetDataTable(Conn[0], Helpers.Qry1));
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
			rdr.Add(Helpers.R1, new DataSourceRecordReaderDataTable(
				Helpers.R1, GetDataTable(Conn[0], Helpers.Qry1)));
			rdr.Add(Helpers.R2, new DataSourceRecordReaderDataTable(
				Helpers.R2, GetDataTable(Conn[1], Helpers.Qry2)));
			rdr.Add(Helpers.R3, new DataSourceRecordReaderDataTable(
				Helpers.R3, GetDataTable(Conn[2], Helpers.Qry3)));
			rdr.Add(Helpers.R4, new DataSourceRecordReaderDataTable(
				Helpers.R4, GetDataTable(Conn[3], Helpers.Qry4)));
			rdr.Add(Helpers.R5, new DataSourceRecordReaderDataTable(
				Helpers.R5, GetDataTable(Conn[4], Helpers.Qry5)));
			rdr.Add(Helpers.R6, new DataSourceRecordReaderDataTable(
				Helpers.R6, GetDataTable(Conn[5], Helpers.Qry6)));
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
      rdr.Add(Helpers.R1, 
				new DataSourceRecordReaderDataTable(Helpers.R1, GetDataTable(Conn[0], Helpers.Qry1)));
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
			rdr.Add(Helpers.R1, new DataSourceRecordReaderDataTable(
				Helpers.R1, GetDataTable(Conn[0], Helpers.Qry1)));
			rdr.Add(Helpers.R2, new DataSourceRecordReaderDataTable(
				Helpers.R2, GetDataTable(Conn[1], Helpers.Qry2)));
			rdr.Add(Helpers.R3, new DataSourceRecordReaderDataTable(
				Helpers.R3, GetDataTable(Conn[2], Helpers.Qry3)));
			rdr.Add(Helpers.R4, new DataSourceRecordReaderDataTable(
				Helpers.R4, GetDataTable(Conn[3], Helpers.Qry4)));
			rdr.Add(Helpers.R5, new DataSourceRecordReaderDataTable(
				Helpers.R5, GetDataTable(Conn[4], Helpers.Qry5)));
			rdr.Add(Helpers.R6, new DataSourceRecordReaderDataTable(
				Helpers.R6, GetDataTable(Conn[5], Helpers.Qry6)));
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
      Assert.That((string)fld.Value, Is.StringMatching("T"));
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
      Assert.That((string)fld.Value, Is.StringMatching("T"));
			rdr[Helpers.R2].Read();
			fld = rdr[Helpers.R2].GetField("strr2");
			Assert.That(fld.Value, Is.Null);

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
			Assert.That((string)fld.Value, Is.EqualTo("AA"));
			rdr[Helpers.R5].Read();
			rdr[Helpers.R5].Read();
			rdr[Helpers.R5].Read();
			rdr[Helpers.R5].Read();
			fld = rdr[Helpers.R5].GetField("intr5");
			Assert.That(fld.Name, Is.EqualTo("intr5"));
			Assert.That((int)fld.Value, Is.EqualTo(5));
			fld = rdr[Helpers.R5].GetField("strr5");
			Assert.That(fld.Name, Is.EqualTo("strr5"));
			Assert.That((string)fld.Value, Is.EqualTo("EE"));

			rdr[Helpers.R6].Read();
			fld = rdr[Helpers.R6].GetField("intr6");
			Assert.That(fld.Name, Is.EqualTo("intr6"));
			Assert.That((int)fld.Value, Is.EqualTo(11));
			fld = rdr[Helpers.R6].GetField("strr6");
			Assert.That(fld.Name, Is.EqualTo("strr6"));
			Assert.That((string)fld.Value, Is.EqualTo("AAA"));
			rdr[Helpers.R6].Read();
			rdr[Helpers.R6].Read();
			rdr[Helpers.R6].Read();
			rdr[Helpers.R6].Read();
			rdr[Helpers.R6].Read();
			fld = rdr[Helpers.R6].GetField("intr6");
			Assert.That(fld.Name, Is.EqualTo("intr6"));
			Assert.That((int)fld.Value, Is.EqualTo(66));
			fld = rdr[Helpers.R6].GetField("strr6");
			Assert.That(fld.Name, Is.EqualTo("strr6"));
			Assert.That((string)fld.Value, Is.EqualTo("FFF"));
    }

		private DataTable GetDataTable(IDbConnection cn, string sql)
		{
			DataTable tbl = new DataTable();
			IDbCommand cmd = cn.CreateCommand();
			cmd.CommandText = sql;
			cn.Open();
			try
			{
				tbl.Load(cmd.ExecuteReader());
			}
			finally
			{
				cn.Close();
			}
			return tbl;
		}
  }
}
