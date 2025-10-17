using System.Collections.Generic;

namespace Keops.AspNetCore.DataTables
{
    /// <summary>
    /// For internal use only.
    /// Represents a DataTables request.
    /// </summary>
    internal class DataTablesRequest(int draw, int start, int length, ISearch search, IEnumerable<IColumn> columns, IDictionary<string, object> additionalParameters) : IDataTablesRequest
    {
        public DataTablesRequest(int draw, int start, int length, ISearch search, IEnumerable<IColumn> columns) : this(draw, start, length, search, columns, null){ }

        public IDictionary<string, object> AdditionalParameters { get; private set; } = additionalParameters;

        public IEnumerable<IColumn> Columns { get; private set; } = columns;

        public int Draw { get; private set; } = draw;

        public int Length { get; private set; } = length;

        public ISearch Search { get; private set; } = search;

        public int Start { get; private set; } = start;
    }
}
