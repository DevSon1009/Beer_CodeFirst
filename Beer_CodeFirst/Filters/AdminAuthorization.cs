using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Filters;
using System.Web.Mvc;

namespace Beer_CodeFirst.Filters
{
    public class AdminAuthorization : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (filterContext.HttpContext.User.IsInRole("Admin") == false)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {

        }
    }
}