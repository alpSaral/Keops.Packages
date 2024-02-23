using Microsoft.AspNetCore.Html;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace EsGiris.Admin.WebGrid
{
    public class WebGridRow : DynamicObject, IEnumerable<object>
    {
        private const string RowIndexMemberName = "ROW";
        private const BindingFlags BindFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase;

        private WebGrid _grid;
        private IDynamicMetaObjectProvider _dynamic;
        private int _rowIndex;
        private object _value;
        private IEnumerable<dynamic> _values;

        public WebGridRow(WebGrid webGrid, object value, int rowIndex)
        {
            _grid = webGrid;
            _value = value;
            _rowIndex = rowIndex;
            _dynamic = value as IDynamicMetaObjectProvider;
        }

        public dynamic Value => _value;

        public WebGrid WebGrid => _grid;

        public object this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof(name));

                if (!TryGetMember(name, out object value))
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CommonResources.WebGrid_ColumnNotFound, name));

                return value;
            }
        }

        public object this[int index]
        {
            get
            {
                if ((index < 0) || (index >= _grid.ColumnNames.Count()))
                    throw new ArgumentOutOfRangeException(nameof(index));
                return this.Skip(index).First();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object> GetEnumerator()
        {
            _values ??= _grid.ColumnNames.Select(c => WebGrid.GetMember(this, c));
            return _values.GetEnumerator();
        }

        public IHtmlContent GetSelectLink(string text = null)
        {
            if (string.IsNullOrEmpty(text))
                text = CommonResources.WebGrid_SelectLinkText;

            return WebGridRenderer.GridLink(_grid, GetSelectUrl(), text);
        }

        public string GetSelectUrl()
        {
            var queryString = new NameValueCollection(1)
            {
                [WebGrid.SelectionFieldName] = (_rowIndex + 1L).ToString(CultureInfo.CurrentCulture)
            };

            return WebGrid.GetPath(queryString);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            // Try to get the row index
            if (TryGetRowIndex(binder.Name, out result))
                return true;

            // Try to evaluate the dynamic member based on the binder
            if (_dynamic != null && DynamicHelper.TryGetMemberValue(_dynamic, binder, out result))
                return true;

            return TryGetComplexMember(_value, binder.Name, out result);
        }

        internal bool TryGetMember(string memberName, out object result)
        {
            result = null;

            // Try to get the row index
            if (TryGetRowIndex(memberName, out result))
                return true;

            // Try to evaluate the dynamic member based on the name
            if (_dynamic != null && DynamicHelper.TryGetMemberValue(_dynamic, memberName, out result))
                return true;

            // Support '.' for navigation properties
            return TryGetComplexMember(_value, memberName, out result);
        }

        public override string ToString() => _value.ToString();

        private bool TryGetRowIndex(string memberName, out object result)
        {
            result = null;
            if (string.IsNullOrEmpty(memberName))
                return false;

            if (memberName == RowIndexMemberName)
            {
                result = _rowIndex;
                return true;
            }

            return false;
        }

        private static bool TryGetComplexMember(object obj, string name, out object result)
        {
            result = null;

            var names = name.Split('.');

            for (var i = 0; i < names.Length; i++)
            {
                if ((obj == null) || !TryGetMember(obj, names[i], out result))
                {
                    result = null;
                    return false;
                }
                obj = result;
            }
            return true;
        }

        private static bool TryGetMember(object obj, string name, out object result)
        {
            var property = obj.GetType().GetProperty(name, BindFlags);
            if ((property != null) && (property.GetIndexParameters().Length == 0))
            {
                result = property.GetValue(obj, null);
                return true;
            }
            result = null;
            return false;
        }
    }
}
