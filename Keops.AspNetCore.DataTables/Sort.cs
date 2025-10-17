using System;

namespace Keops.AspNetCore.DataTables
{
    /// <summary>
    /// Represents sort/ordering for columns.
    /// </summary>
    /// <remarks>
    /// Creates a new sort instance.
    /// </remarks>
    /// <param name="field">Data field to be bound.</param>
    /// <param name="order">Sort order for multi-sorting.</param>
    /// <param name="direction">Sort direction</param>
    public class Sort(int order, string direction) : ISort
    {
        /// <summary>
        /// Gets sort direction.
        /// </summary>
        public SortDirection Direction { get; private set; } = (direction ?? string.Empty).Equals(Configuration.Options.RequestNameConvention.SortDescending, StringComparison.InvariantCultureIgnoreCase) ? SortDirection.Descending : SortDirection.Ascending;

        /// <summary>
        /// Gets sort order.
        /// </summary>
        public int Order { get; private set; } = order;
    }
}
