using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.IO;

namespace NCsvLibTestSuite
{
  class Helpers
  {
    public static readonly string ConnStrFileName = "../../src/NCsvLibTestSuite/dbconnstrtest.txt";
    public static readonly string SchFileName = "../../src/NCsvLibTestSuite/schematest.xml";
    public static readonly string OutFileName = "csvtest.txt";
    public static readonly string QryCsvTest1 = "SELECT * FROM csvtest1";
    public static readonly Encoding Enc = Encoding.UTF8;

    public static DbConnection GetDbConnectionFromFile(string sFileName)
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
  }
}
