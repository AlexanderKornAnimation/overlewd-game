using System;
using System.Collections.Generic;

namespace Nutaku.Unity
{
    /// <summary>
	/// Builder class that builds query parameters.
    /// </summary>
    public class QueryParameterBuilder
    {

		protected Dictionary<string, object> _parameters = new Dictionary<string, object>();

        /// <summary>
		/// Generate key value pairs of query parameters from property content
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Build()
        {
			var paramsList = new List<KeyValuePair<string, string>> ();
			foreach (KeyValuePair<string, object> kvp in _parameters)
				paramsList.Add (new KeyValuePair<string, string> (kvp.Key, ToString(kvp.Value)));
			return paramsList;
        }

        /// <summary>
		/// Generate a query string from the key/value pairs.
        /// </summary>
        public static string ToQueryString(IEnumerable<KeyValuePair<string, string>> queryParams)
        {
			var paramList = new List<string> ();
			foreach (KeyValuePair<string, string> kvp in queryParams)
				if (kvp.Value != null)
					paramList.Add (Uri.EscapeDataString(kvp.Key) + "=" + Uri.EscapeDataString(kvp.Value));

			return string.Join("&", paramList.ToArray());
        }

        protected string format
        {
            get
            {
                return _parameters.ContainsKey("format") ? _parameters["format"] as string : null;
            }

            set
            {
                _parameters["format"] = value;
            }
        }

        protected List<string> fields
        {
            get
            {
                if (!_parameters.ContainsKey("fields"))
                    fields = new List<string>();
                return _parameters["fields"] as List<string>;
            }

            set
            {
                _parameters["fields"] = value;
            }
        }

        protected int? count
        {
            get
            {
                return _parameters.ContainsKey("count") ? _parameters["count"] as int? : null;
            }

            set
            {
                _parameters["count"] = value;
            }
        }

        protected int? startIndex
        {
            get
            {
                return _parameters.ContainsKey("startIndex") ? _parameters["startIndex"] as int? : null;
            }

            set
            {
                _parameters["startIndex"] = value;
            }
        }

        protected string filterBy
        {
            get
            {
                return _parameters.ContainsKey("filterBy") ? _parameters["filterBy"] as string : null;
            }

            set
            {
                _parameters["filterBy"] = value;
            }
        }

        protected string filterOp
        {
            get
            {
                return _parameters.ContainsKey("filterOp") ? _parameters["filterOp"] as string : null;
            }

            set
            {
                _parameters["filterOp"] = value;
            }
        }

        protected string filterValue
        {
            get
            {
                return _parameters.ContainsKey("filterValue") ? _parameters["filterValue"] as string : null;
            }

            set
            {
                _parameters["filterValue"] = value;
            }
        }

        static string ToString(object value)
        {
            if (value == null)
                return null;

            if (value is string)
                return value as string;

            if (value is IEnumerable<string>)
                return JoinValues(value as IEnumerable<string>);

            if (value is IEnumerable<object>)
                return JoinValues(value as IEnumerable<object>);

            return value.ToString();
        }

        static string JoinValues(IEnumerable<string> values)
        {
            return string.Join(",", values);
        }

        static string JoinValues(IEnumerable<object> values)
        {
			var stringValues = new List<string> ();
			foreach (object obj in values)
				stringValues.Add(ToString(obj));
			return string.Join (",", stringValues.ToArray ());
        }
    }

    /// <summary>
	/// Build query parameters for People API
    /// </summary>
    public class PeopleQueryParameterBuilder : QueryParameterBuilder
    {
        new public List<string> fields
        {
            get
            {
                return base.fields;
            }

            set
            {
                base.fields = value;
            }
        }

        new public int? count
        {
            get
            {
                return base.count;
            }

            set
            {
                base.count = value;
            }
        }

        new public int? startIndex
        {
            get
            {
                return base.startIndex;
            }

            set
            {
                base.startIndex = value;
            }
        }

        public bool? hasApp
        {
            get
            {
                if (filterBy == "hasApp")
                    return filterValue == "true";
                return null;
            }

            set
            {
                if (value.HasValue)
                {
                    filterBy = "hasApp";
                    filterOp = "equals";
                    filterValue = value.Value ? "true" : "false";
                }
                else
                {
                    filterBy = null;
                    filterOp = null;
                    filterValue = null;
                }
            }
        }
    }

    /// <summary>
	/// Build query parameters for Payment API.
    /// </summary>
    public class PaymentQueryParameterBuilder : QueryParameterBuilder
    {
    }

    /// <summary>
	/// Build query parameters for Payment API.
    /// </summary>
    public class InspectionQueryParameterBuilder : QueryParameterBuilder
    {
    }
}
