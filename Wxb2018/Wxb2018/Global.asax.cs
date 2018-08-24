using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Wxb2018
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        public override void Init()
        {
            base.Init();
            //new Stockstar.StaticFileModule.MyStaticFileModule().Init(this);

            base.AuthorizeRequest += new EventHandler(MvcApplication_AuthorizeRequest);
            //base.PostAuthenticateRequest += new EventHandler(MvcApplication_PostAuthenticateRequest);
        }
        
        #region 登录时用

        void MvcApplication_AuthorizeRequest(object sender, EventArgs e)
        {
            var principal = WFFormsAuthentication.TryParsePrincipal(Context);
            if (principal != null && principal.UserData != null)
            {
                Context.User = principal;
            }
        } 
        #endregion

    }
}
