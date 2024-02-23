using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EsGiris.Admin.WebGrid
{
    /// <summary>
    /// Source wrapper for data provided by the user that is already sorted and paged. The user provides the WebGrid the rows to bind and additionally the total number of rows that 
    /// are available.
    /// </summary>
    internal sealed class PreComputedGridDataSource : IWebGridDataSource
    {
        private readonly int _totalRows;
        private readonly IList<WebGridRow> _rows;

        public int TotalRowCount => _totalRows;

        public PreComputedGridDataSource(WebGrid grid, IEnumerable<dynamic> values, int totalRows)
        {
            Debug.Assert(grid != null);
            Debug.Assert(values != null);

            _totalRows = totalRows;
            _rows = values.Select((value, index) => new WebGridRow(grid, value: value, rowIndex: index)).ToList();
        }

        public IList<WebGridRow> GetRows(SortInfo sortInfo, int pageIndex) => _rows; // Data is already sorted and paged. Ignore parameters
    }
}
