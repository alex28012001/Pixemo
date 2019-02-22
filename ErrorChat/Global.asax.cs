using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace ErrorChat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //регистрация Ninject зависимостей в App_Start.NinjectWebCommon
        }

      
        //для того чтобы отлавливать на клиентской стороне(js) ajax-ом авторизирован ли пользователь на сайте
        protected void Application_EndRequest()
        {
            if (Context.Response.StatusCode == 302 &&
                Context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                Context.Response.Clear();
                Context.Response.StatusCode = 401;
            }
        }
    }
}
