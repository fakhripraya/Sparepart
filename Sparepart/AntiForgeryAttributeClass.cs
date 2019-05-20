using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Sparepart
{
    public class ValidateAntiForgeryTokenAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            
            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {
                if (request.IsAjaxRequest())
                {
                    var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

                    var cookieValue = antiForgeryCookie != null ? antiForgeryCookie.Value : string.Empty;

                    AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"] ?? string.Empty);
                }
                else
                {
                    base.OnAuthorization(filterContext);
                }
            }
        }

    }
}