namespace Keops.AspNetCore.DataTables.NameConvention
{
    /// <summary>
    /// Represents CamelCase response naming convention for DataTables.AspNet.AspNetCore.
    /// </summary>
    public class CamelCaseResponseNameConvention : IResponseNameConvention
    {
        public string Draw => "draw";

        public string TotalRecords => "recordsTotal";

        public string TotalRecordsFiltered => "recordsFiltered";

        public string Data => "data";

        public string Error => "error";
    }
}
