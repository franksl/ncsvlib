using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
  public class NCsvLibException : Exception
  {
    public NCsvLibException(string message)
      : base(message)
    {
    }

    public NCsvLibException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
  }

  public class NCsvLibSchemaException : NCsvLibException
  {
    public NCsvLibSchemaException(string message)
      : base(message)
    {
    }

      public NCsvLibSchemaException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
  }

  public class NCsvLibDataSourceException : NCsvLibException
  {
    public NCsvLibDataSourceException(string message)
      : base(message)
    {
    }

      public NCsvLibDataSourceException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
  }

  public class NCsvLibOutputException : NCsvLibException
  {
    public NCsvLibOutputException(string message)
      : base(message)
    {
    }

      public NCsvLibOutputException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
  }

  public class NCsvLibControllerException : NCsvLibException
  {
    public NCsvLibControllerException(string message)
      : base(message)
    {
    }

      public NCsvLibControllerException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
  }
}
