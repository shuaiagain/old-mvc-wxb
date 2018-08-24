using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WXB.Bussiness.ViewModels;
using WXB.Bussiness.Service;
using Newtonsoft.Json;

namespace Wxb2018.Controllers
{
    public class NoticeReceiverController : Controller
    {

        #region 获取还未标记接受通知的用户

        public ActionResult GetUserNotReceiveNotice(int userId, string jsoncallback = "")
        {
            List<NoticeReceiverItemVM> list = new NoticeReceiverService().GetUserNotReceiveNotice(userId);
            var json = JsonConvert.SerializeObject(new
            {
                code = 0,
                data = list
            });

            if (!string.IsNullOrWhiteSpace(jsoncallback))
            {
                json = jsoncallback + "(" + json + ")";
            }

            return Content(json, "text/plain", System.Text.Encoding.UTF8);
        }
        #endregion

    }
}
