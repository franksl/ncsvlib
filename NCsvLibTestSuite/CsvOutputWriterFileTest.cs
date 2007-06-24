using System;
using System.Collections.Generic;
using System.Text;
using NCsvLib;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.IO;

namespace NCsvLibTestSuite
{
  [TestFixture]
  public class CsvOutputWriterFileTest
  {    
    

    [SetUp]
    public void SetUp()
    {
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
      CsvWriterController ctrl = new CsvWriterController();
      ctrl.SchemaRdr = new SchemaReaderXml(Helpers.SchFileName);
      DataSourceReaderDb rdr = new DataSourceReaderDb();
      rdr.AddCommand(Helpers.R1, new DataSourceReaderDbCommand(Helpers.GetDbConnectionFromFile(Helpers.ConnStrFileName), Helpers.Qry1));
      rdr.AddCommand(Helpers.R2, new DataSourceReaderDbCommand(Helpers.GetDbConnectionFromFile(Helpers.ConnStrFileName), Helpers.Qry2));
      rdr.AddCommand(Helpers.R3, new DataSourceReaderDbCommand(Helpers.GetDbConnectionFromFile(Helpers.ConnStrFileName), Helpers.Qry3));
      rdr.AddCommand(Helpers.R4, new DataSourceReaderDbCommand(Helpers.GetDbConnectionFromFile(Helpers.ConnStrFileName), Helpers.Qry4));
      ctrl.InputRdr = rdr;
      ctrl.OutWriter = new CsvOutputWriterFile(Helpers.OutFileName);
      ctrl.Execute();

      //Opens the eventual file and reads values
      Assert.That(File.Exists(Helpers.OutFileName));
      //File tests
      List<string> ln = Helpers.GetFileLines(Helpers.OutFileName, Helpers.Enc);
      Assert.That(ln.Count == 16);
      Assert.That(ln[0]  == "    1|\"aaa                 \"|        100,10|\"      1.000,11\"|AAA|");
      Assert.That(ln[1]  == "    2|\"bbb                 \"|        200,20|\"      2.000,22\"|AAA|");
      Assert.That(ln[2]  == "    3|\"ccc                 \"|        300,30|\"      3.000,33\"|AAA|");
      Assert.That(ln[3]  == "    4|\"ddd                 \"|        400,40|\"      4.000,44\"|AAA|");
      Assert.That(ln[4]  == "FLDR2|1|\"                r2_1\"|");
      Assert.That(ln[5]  == "FLDR2|2|\"                r2_2\"|");
      Assert.That(ln[6]  == "FLDR2|3|\"                r2_3\"|");
      Assert.That(ln[7]  == "FLDR2|4|\"                r2_4\"|");
      Assert.That(ln[8]  == "FLDR3|1|\"                r3_1\"|");
      Assert.That(ln[9]  == "FLDR3|2|\"                r3_2\"|");
      Assert.That(ln[10] == "FLDR3|3|\"                r3_3\"|");
      Assert.That(ln[11] == "FLDR3|4|\"                r3_4\"|");
      Assert.That(ln[12] == "FLDR4|    1|         11,10|        111,11|");
      Assert.That(ln[13] == "FLDR4|    2|         22,20|        222,22|");
      Assert.That(ln[14] == "FLDR4|    3|         33,30|        333,33|");
      Assert.That(ln[15] == "FLDR4|    4|         44,40|        444,44|");
    }
  }
}
