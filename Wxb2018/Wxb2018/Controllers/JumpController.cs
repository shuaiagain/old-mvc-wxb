using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WXB.Bussiness.Common;
using WXB.Bussiness.Service;
using WXB.Bussiness.ViewModels;

namespace Wxb2018.Controllers
{
    public class JumpController : Controller
    {

        #region cookie登录跳转页
        public ActionResult Index()
        {
            var cookie = Request.Cookies["USERSESSIONS"];
            if (cookie == null) return RedirectToAction("Login", "Account");

            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            if (ticket == null || string.IsNullOrEmpty(ticket.UserData)) return RedirectToAction("Login", "Account");

            var userId = Convert.ToInt32(ticket.UserData.Split('$')[0]);
            UserVM user = new UserService().GetUserById(userId);
            if (user == null) return RedirectToAction("Login", "Account");

            #region 登录日志
            var log = new LogVM()
            {
                Operator = user.Name,
                OperatorID = user.ID,
                RoleType = user.RoleType,
                OperateTime = DateTime.Now,
                OperateType = (int)EnumOperateType.登录
            };

            log.OperateDescribe = ((EnumOperateType)log.OperateType).ToString();
            Logger.AddLog(log);
            #endregion

            var noticeId = Request["noticeId"];
            var noticeReceiverId = Request["noticeReceiverId"];
            if (noticeId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("ViewNotice", "Home");
            }
        }
        #endregion
    }
}
