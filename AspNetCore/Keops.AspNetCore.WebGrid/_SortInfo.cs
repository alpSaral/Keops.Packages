using Keops.AspNetCore.WebGrid.Model;
using System;

namespace Keops.AspNetCore.WebGrid
{
    internal sealed class SortInfo : IEquatable<SortInfo>
    {
        public string SortColumn { get; set; }

        public SortDirection SortDirection { get; set; }

        public bool Equals(SortInfo other) => other != null && string.Equals(SortColumn, other.SortColumn, StringComparison.OrdinalIgnoreCase) && SortDirection == other.SortDirection;

        public override bool Equals(object obj)
        {
            if (obj is SortInfo sortInfo)
                return Equals(sortInfo);
            return base.Equals(obj);
        }

        public override int GetHashCode() => SortColumn.GetHashCode();
    }
}
