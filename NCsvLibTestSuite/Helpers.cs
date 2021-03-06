using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace NCsvLibTestSuite
{
  class Helpers
  {
    public static readonly string ConnStrFileName = "../../src/NCsvLibTestSuite/dbconnstrtest.txt";
    public static readonly string SchFileName = "../../src/NCsvLibTestSuite/schematest.xml";
    public static readonly string OutFileName = "csvtest.txt";
    public static readonly string Qry1 = "SELECT * FROM csvtest1 ORDER BY intfld";
    public static readonly string Qry2 = "SELECT * FROM csvtest2 ORDER BY intr2";
    public static readonly string Qry3 = "SELECT * FROM csvtest3 ORDER BY intr3";
    public static readonly string Qry4 = "SELECT * FROM csvtest4 ORDER BY intr4";
		public static readonly string Qry5 = "SELECT * FROM csvtest5 ORDER BY intr5";
		public static readonly string Qry6 = "SELECT * FROM csvtest6 ORDER BY intr6";
    public static readonly string R1 = "R1";
    public static readonly string R2 = "R2";
    public static readonly string R3 = "R3";
    public static readonly string R4 = "R4";
		public static readonly string R5 = "R5";
		public static readonly string R6 = "R6";
    public static readonly Encoding Enc = Encoding.UTF8;

    public static IDbConnection GetDbConnectionFromFile(string sFileName)
    {
      string s = string.Empty;
      using (StreamReader sr = new StreamReader(sFileName))
      {
        s = sr.ReadLine();
      }
      if (s == string.Empty)
        throw new Exception("Error while reading connection string");
      return new MySqlConnection(s);
    }

    public static List<string> GetFileLines(string sFileName, Encoding enc)
    {
      List<string> ln = new List<string>();
      using (StreamReader sr = new StreamReader(sFileName, enc))
      {
        while (sr.Peek() >= 0)
          ln.Add(sr.ReadLine());
      }
      return ln;
    }

    public static void CreateEnvironment()
    {
      using (IDbConnection cn = GetDbConnectionFromFile(Helpers.ConnStrFileName))
      {
        IDbCommand cmd = cn.CreateCommand();
        cn.Open();
        cmd.CommandText = "CALL CreateTestDb()";
        cmd.ExecuteNonQuery();
      }
    }
  }

  class DummyFormatter : NCsvLib.Formatters.ICsvOutputFormatter
  {
    public string Format(NCsvLib.DataSourceField fld, NCsvLib.SchemaField sch)
    {
      return "DUMMY";
    }
  }
}
