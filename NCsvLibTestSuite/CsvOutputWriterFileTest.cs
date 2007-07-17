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
      Assert.That(File.Exists(Helpers.OutFileName), Is.True);
      
      //File tests
      List<string> ln = Helpers.GetFileLines(Helpers.OutFileName, Helpers.Enc);
      Assert.That(ln.Count, Is.EqualTo(16));
      Assert.That(ln[0],  Is.EqualTo("00001|\"aaa                 \"|        100,10|\"      1.000,11\"|AAA|"));
      Assert.That(ln[1],  Is.EqualTo("00002|\"bbb                 \"|        200,20|\"      2.000,22\"|AAA|"));
      Assert.That(ln[2],  Is.EqualTo("00003|\"ccc                 \"|        300,30|\"      3.000,33\"|AAA|"));
      Assert.That(ln[3],  Is.EqualTo("00004|\"ddd                 \"|        400,40|\"      4.000,44\"|AAA|"));
      Assert.That(ln[4],  Is.EqualTo("FLDR2|001|11000|\"                r2_1\"|"));
      Assert.That(ln[5],  Is.EqualTo("FLDR2|002|22000|\"                r2_2\"|"));
      Assert.That(ln[6],  Is.EqualTo("FLDR2|003|33000|\"                r2_3\"|"));
      Assert.That(ln[7],  Is.EqualTo("FLDR2|004|44000|\"                r2_4\"|"));
      Assert.That(ln[8],  Is.EqualTo("FLDR3|1|\"                r3_1\"|"));
      Assert.That(ln[9],  Is.EqualTo("FLDR3|2|\"                r3_2\"|"));
      Assert.That(ln[10], Is.EqualTo("FLDR3|3|\"                r3_3\"|"));
      Assert.That(ln[11], Is.EqualTo("FLDR3|4|\"                r3_4\"|"));
      Assert.That(ln[12], Is.EqualTo("FLDR4|    1|         11,10|        111,11|"));
      Assert.That(ln[13], Is.EqualTo("FLDR4|    2|         22,20|        222,22|"));
      Assert.That(ln[14], Is.EqualTo("FLDR4|    3|         33,30|        333,33|"));
      Assert.That(ln[15], Is.EqualTo("FLDR4|    4|         44,40|        444,44|"));
    }
  }
}
