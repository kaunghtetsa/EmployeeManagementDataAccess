namespace ASM.EmployeeManagement.DataAccess.Common.Paging
{
	/// <summary>
	/// Paging Related information
	/// </summary>
	public class PageInfoBase 
	{
		#region Properties

		/// <summary>
		/// Number of records wanted
		/// </summary>
		public int? Num { get; set; }

		/// <summary>
		/// Start index of the record
		/// </summary>
		public int? StartIndex { get; set; }

		#endregion
	}
}
