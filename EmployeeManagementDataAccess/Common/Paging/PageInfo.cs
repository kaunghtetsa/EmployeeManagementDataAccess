using System.Collections.Generic;

namespace ASM.EmployeeManagement.DataAccess.Common.Paging
{
	/// <summary>
	/// Paging Related information
	/// </summary>
	public class PageInfo : PageInfoBase
	{
		#region Properties

        /// <summary>
        /// Single Sort Key
        /// </summary>
        public string SortKey { get; set; }

		/// <summary>
		/// Multiple Sort Key
		/// </summary>
		public List<string> SortKeys { get; set; }

		/// <summary>
		/// Sort Order
		/// </summary>
		public int? SortOrder { get; set; }

		#endregion
	}
}
