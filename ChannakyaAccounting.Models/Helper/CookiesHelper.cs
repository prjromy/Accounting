using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ChannakyaAccounting.Models.Helper
{
    public class CookiesHelper
    {

        public void UpdateCookie(string SessionName, string SessionValue)
        {
            HttpContext.Current.Session[SessionName] = SessionValue;
        }
        public object GetSessionValue(string SessionVariableName)
        {
            return HttpContext.Current.Session[SessionVariableName];

        }

    }
}