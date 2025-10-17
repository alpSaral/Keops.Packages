namespace Keops.AspNetCore.DataTables
{
    /// <summary>
    /// Represents search/filter definition and value.
    /// </summary>
    /// <remarks>
    /// Creates a new search instance.
    /// </remarks>
    /// <param name="value">Search value.</param>
    /// <param name="isRegex">True if search value is regex, False if search value is plain text.</param>
    public class Search(string value, bool isRegex) : ISearch
    {
        /// <summary>
        /// Creates a new search instance.
        /// </summary>
        public Search() : this(string.Empty, false) { }

        /// <summary>
        /// Gets an indicator if search value is regex or plain text.
        /// </summary>
        public bool IsRegex { get; private set; } = isRegex;
        /// <summary>
        /// Gets search value.
        /// </summary>
        public string Value { get; private set; } = value;
    }
}
