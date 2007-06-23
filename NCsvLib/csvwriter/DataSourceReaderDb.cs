using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace NCsvLib
{
  public class DataSourceReaderDb : IDataSourceReader
  {
    private Dictionary<string, DataSourceReaderDbCommand> _Cmds;

    public DataSourceReaderDb()
    {
      _Cmds = new Dictionary<string, DataSourceReaderDbCommand>();
    }
        
    /// <summary>
    /// Simple constructor if there is only one record
    /// </summary>
    /// <param name="ref_conn"></param>
    /// <param name="command_text"></param>
    public DataSourceReaderDb(string id, DbConnection refConn, string commandText)
      : base()
    {
      DataSourceReaderDbCommand cmd = new DataSourceReaderDbCommand(refConn);
      cmd.Cmd.CommandText = commandText;
      _Cmds.Add(id, cmd);
    }

    public void Open(string id)
    {
      //Tries to get the requested command
      DataSourceReaderDbCommand cmd;
      if (!_Cmds.TryGetValue(id, out cmd))
        throw new NCsvLibInputException("Invalid Id");
      //Opens a db connection and creates a datareader
      cmd.Conn.Open();
      try
      {
        cmd.Rdr = cmd.Cmd.ExecuteReader();
      }
      catch
      {
        cmd.Conn.Close();
        throw;
      }
    }

    public void Close(string id)
    {
      //Tries to get the requested command
      DataSourceReaderDbCommand cmd;
      if (!_Cmds.TryGetValue(id, out cmd))
        throw new NCsvLibInputException("Invalid Id");
      if (!cmd.Rdr.IsClosed)
        cmd.Rdr.Close();
      if (cmd.Conn.State == System.Data.ConnectionState.Open)
        cmd.Conn.Close();
    }

    public bool Read(string id)
    {
      //Tries to get the requested command
      DataSourceReaderDbCommand cmd;
      if (!_Cmds.TryGetValue(id, out cmd))
        throw new NCsvLibInputException("Invalid Id");
      if (!cmd.Rdr.IsClosed)
        return cmd.Rdr.Read();
      else
        throw new NCsvLibInputException("InputReader closed");
    }

    public DataSourceField GetField(string id, string name)
    {
      //Tries to get the requested command
      DataSourceReaderDbCommand cmd;
      if (!_Cmds.TryGetValue(id, out cmd))
        throw new NCsvLibInputException("Invalid Id");
      if (cmd.Rdr.IsClosed)
        throw new NCsvLibInputException("DataSourceReader closed");
      int idx;
      try
      {
        idx = cmd.Rdr.GetOrdinal(name);
      }
      catch (IndexOutOfRangeException)
      {
        throw new NCsvLibInputException("Field name not found in db");
      }
      DataSourceField fld = new DataSourceField();
      fld.Name = name;
      fld.Value = cmd.Rdr.GetValue(idx);
      return fld;
    }
  }

  public class DataSourceReaderDbCommand
  {
    public DbConnection Conn;
    public DbCommand Cmd;
    public DbDataReader Rdr;

    private DataSourceReaderDbCommand()
    {
    }

    public DataSourceReaderDbCommand(DbConnection conn)
    {
      Conn = conn;
      Cmd = conn.CreateCommand();
    }
  }
}
