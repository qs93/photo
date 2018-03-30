using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Photo.Common;

namespace Photo.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            if (!Request.Url.ToString().ToLower().Contains("localhost")) //本地的話不清除錯誤exception
            {
                var lastError = Server.GetLastError();
                if (lastError != null)
                {
                    var httpError = lastError as HttpException;
                    if (httpError != null)
                    {
                        //ASP.NET的400与404错误不记录日志，并都以自定义404页面响应
                        var httpCode = httpError.GetHttpCode();
                        if (httpCode == 400 || httpCode == 404)
                        {
                            Response.StatusCode = 404;//在IIS中配置自定义404页面
                            Server.ClearError();
                            return;
                        }

                        //保存到数据库
                        //LogHelper.WriteExceptionLog(httpError);
                    }

                    //对于路径错误不记录日志，并都以自定义404页面响应
                    if (lastError.TargetSite.ReflectedType == typeof(System.IO.Path))
                    {
                        Response.StatusCode = 404;
                        Server.ClearError();
                        return;
                    }
                    if (lastError is NotLogOutEx)
                    {
                        //不保存记录
                    }
                    else
                    {
                        //保存到记录
                    }
                    Response.StatusCode = 500;
                    Server.ClearError();
                }
            }
        }
    }
}
