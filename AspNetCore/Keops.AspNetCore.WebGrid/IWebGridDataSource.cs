using System.Collections.Generic;

namespace EsGiris.Admin.WebGrid
{
    internal interface IWebGridDataSource
    {
        int TotalRowCount { get; }

        IList<WebGridRow> GetRows(SortInfo sortInfo, int pageIndex);
    }
}
