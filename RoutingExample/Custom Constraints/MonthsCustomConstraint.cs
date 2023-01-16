using System;
using System.Text.RegularExpressions; 

namespace RoutingExample.CustomConstraints
{
	public class MonthsCustomConstraint : IRouteConstraint
	{
		public MonthsCustomConstraint()
		{
		}

        /// <summary>
        /// Checks for a match on the route endpoint. If match found, then return true, else return false
        /// </summary>
        /// <param name="httpContext">The context received in middleware. Provides request and response objects</param>
        /// <param name="route">Represents an object that represents the route in which this constraint is applied</param>
        /// <param name="routeKey">The parameter in the query string</param>
        /// <param name="values">The values received in the query string in the route</param>
        /// <param name="routeDirection">1) Checks whether this constraint matches with incoming request or 2) generate a URL based on values supplied</param>
        /// <returns>True / false value for if a match is made</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(routeKey))
            {
                return false; // no key provided
            }

            Regex regex = new Regex("^(apr|jul|oct|jan)$");
            string? month = Convert.ToString(values[routeKey]);

            if (regex.IsMatch(month))
            {
                return true; // found match
            }

            return false; // no match found
        }
    }
}

