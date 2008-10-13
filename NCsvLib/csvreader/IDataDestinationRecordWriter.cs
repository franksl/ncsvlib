using System;
using System.Collections.Generic;
using System.Text;

namespace NCsvLib
{
	public interface IDataDestinationRecordWriter
	{
		/// <summary>
		/// Id of this record
		/// </summary>
		string Id { get; }

		/// <summary>
		/// Opens a 'connection' with the data destination for the given record id
		/// </summary>
		void Open();

		/// <summary>
		/// Closes connection with the data destination for the given record id
		/// </summary>
		void Close();

		/// <summary>
		/// Flushes the current values to the data destination
		/// </summary>
		void Write();

		/// <summary>
		/// Sets the value for the given field
		/// </summary>
		/// <param name="fld"></param>
		void SetField(DataDestinationField fld);
	}
}
