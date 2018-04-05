using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNC.Areas.Blog.Infrastructure
{
    public class IsNotNCAdminConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, 
            string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            throw new NotImplementedException();
        }
    }
}
