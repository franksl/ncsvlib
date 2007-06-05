using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace NCsvLib
{
  public class DbDataSourceReader : IDataSourceReader
  {
    private DbConnection Conn;
    private DbCommand Cmd;
    private DbDataReader Rdr;

    public DbDataSourceReader(DbConnection ref_conn, string command_text)
    {
      Conn = ref_conn;
      Cmd = Conn.CreateCommand();
      Cmd.CommandText = command_text;
    }

    public void Open()
    {
      Conn.Open();
      try
      {
        Rdr = Cmd.ExecuteReader();
      }
      catch
      {
        Conn.Close();
        throw;
      }
    }

    public void Close()
    {
      if (!Rdr.IsClosed)
        Rdr.Close();
      if (Conn.State == System.Data.ConnectionState.Open)
        Conn.Close();
    }

    public bool Read()
    {
      if (!Rdr.IsClosed)
        return Rdr.Read();
      else
        throw new NCsvLibInputException("InputReader closed");
    }

    public InputField GetField(string name)
    {
      if (Rdr.IsClosed)
        throw new NCsvLibInputException("InputReader closed");
      int idx;
      try
      {
        idx = Rdr.GetOrdinal(name);
      }
      catch (IndexOutOfRangeException)
      {
        throw new NCsvLibInputException("Field name not found in db");
      }
      InputField fld = new InputField();
      fld.Name = name;
      fld.Value = Rdr.GetValue(idx);
      return fld;
    }
  }
}
