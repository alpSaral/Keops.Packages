namespace EsGiris.Admin.WebGrid
{
    public class CommonResources
    {
        // CommonResources
        public const string Argument_Must_Be_GreaterThanOrEqualTo = "Value must be greater than or equal to {0}.";
        public const string Argument_Cannot_Be_Null_Or_Empty = "Value cannot be null or an empty string.";
        public const string Argument_Must_Be_Between = "Value must be between {0} and {1}.";

        // HelpersResources
        public const string WebGrid_ColumnNotFound = "Column '{0}' does not exist.";
        public const string WebGrid_SelectLinkText = "Select";
        public const string WebGrid_ColumnNameOrFormatRequired = "The column name cannot be null or an empty string unless a custom format is specified.";
        public const string WebGrid_DataSourceBound = "The WebGrid instance is already bound to a data source.";
        public const string WebGrid_RowCountNotSpecified = "A value for 'rowCount' must be specified when 'autoSortAndPage' is set to true and paging is enabled.";
        public const string WebGrid_NotSupportedIfPagingIsDisabled = "This operation is not supported when paging is disabled for the 'WebGrid' object.";
        public const string WebGrid_NotSupportedIfSortingIsDisabled = "This operation is not supported when sorting is disabled for the 'WebGrid' object.";
        public const string WebGrid_NoDataSourceBound = "A data source must be bound before this operation can be performed.";
        public const string WebGrid_PropertySetterNotSupportedAfterDataBound = "This property cannot be set after the 'WebGrid' object has been sorted or paged. Make sure that this property is set prior to invoking the 'Rows' property directly or indirectly through other methods such as 'GetHtml', 'Pager', 'Table', etc.";
        public const string WebGrid_PagerModeMustBeEnabled = "To use this argument, pager mode '{0}' must be enabled.";
    }
}
