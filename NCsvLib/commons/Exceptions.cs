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
  }

  public class NCsvLibSchemaException : NCsvLibException
  {
    public NCsvLibSchemaException(string message)
      : base(message)
    {
    }
  }

  public class NCsvLibInputException : NCsvLibException
  {
    public NCsvLibInputException(string message)
      : base(message)
    {
    }
  }

  public class NCsvLibOutputException : NCsvLibException
  {
    public NCsvLibOutputException(string message)
      : base(message)
    {
    }
  }

  public class NCsvLibControllerException : NCsvLibException
  {
    public NCsvLibControllerException(string message)
      : base(message)
    {
    }
  }
}
