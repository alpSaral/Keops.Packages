using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Keops.AspNetCore.DataTables
{
    /// <summary>
    /// Represents a custom JsonResult that can process IDataTablesResponse accordingly to settings.
    /// </summary>
    public class DataTablesJsonResult(IDataTablesResponse response, string contentType, Encoding contentEncoding, bool allowJsonThroughHttpGet) : IActionResult
    {
        /// <summary>
        /// Defines the default result content type.
        /// </summary>
        private static readonly string DefaultContentType = "application/json; charset={0}";
        /// <summary>
        /// Defines the default result enconding.
        /// </summary>
        private static readonly Encoding DefaultContentEncoding = Encoding.UTF8;
        /// <summary>
        /// Defines the default json request behavior.
        /// </summary>
        private static readonly bool AllowJsonThroughHttpGet = false;

        private readonly string ContentType = string.Format(contentType ?? DefaultContentType, contentEncoding.WebName);
        private readonly Encoding ContentEncoding = contentEncoding ?? Encoding.UTF8;
        private readonly bool AllowGet = allowJsonThroughHttpGet;
        private readonly object Data = response;

        public DataTablesJsonResult(IDataTablesResponse response)
            : this(response, DefaultContentType, DefaultContentEncoding, AllowJsonThroughHttpGet)
        { }

        public DataTablesJsonResult(IDataTablesResponse response, bool allowJsonThroughHttpGet)
            : this(response, DefaultContentType, DefaultContentEncoding, allowJsonThroughHttpGet)
        { }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            if (!AllowGet && context.HttpContext.Request.Method.ToUpperInvariant().Equals("GET"))
                throw new NotSupportedException("This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.");

            var response = context.HttpContext.Response;

            response.ContentType = ContentType;

            if (Data != null)
            {
                var content = Data.ToString();
                var contentBytes = ContentEncoding.GetBytes(content);
                await response.Body.WriteAsync(contentBytes);
            }
        }
    }
}
