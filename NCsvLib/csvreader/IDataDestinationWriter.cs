using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
	public interface IDataDestinationWriter : IDictionary<string, IDataDestinationRecordWriter>
	{
		/// <summary>
		/// Opens all the contained record writers
		/// </summary>
		void OpenAll();

		/// <summary>
		/// Closes all the contained record writers
		/// </summary>
		void CloseAll();
	}
}
