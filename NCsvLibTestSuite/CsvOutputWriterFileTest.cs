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
    public void WriteSimpleFile()
    {
      /*CsvWriterController ctrl = new CsvWriterController();
      ctrl.SchemaRdr = new SchemaReaderXml(Helpers.SchFileName);
      ctrl.InputRdr = new DataSourceReaderDb(Helpers.GetDbConnectionFromFile(Helpers.ConnStrFileName), 
                                                          Helpers.QryCsvTest1);
      ctrl.OutWriter = new CsvOutputWriterFile(Helpers.OutFileName);
      ctrl.Execute();

      //Opens the eventual file and reads values
      Assert.That(File.Exists(Helpers.OutFileName));
      //File tests
      List<string> ln = Helpers.GetFileLines(Helpers.OutFileName, Helpers.Enc);
      Assert.That(ln.Count == 4);
      Assert.That(ln[0] == "    1|\"aaa                 \"|        100,10|\"      1.000,11\"|AAA|");
      Assert.That(ln[1] == "    2|\"bbb                 \"|        200,20|\"      2.000,22\"|AAA|");
      Assert.That(ln[2] == "    3|\"ccc                 \"|        300,30|\"      3.000,33\"|AAA|");
      Assert.That(ln[3] == "    4|\"ddd                 \"|        400,40|\"      4.000,44\"|AAA|");
       */
    }
  }
}
