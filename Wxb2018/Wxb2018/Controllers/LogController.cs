using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using WXB.Bussiness.Service;
using WXB.Bussiness.ViewModels;
using Wxb2018.Filters;

namespace Wxb2018.Controllers
{
    public class LogController : BaseController
    {

        #region 获取值班记录
        /// <summary>
        /// 获取登录记录
        /// </summary>
        /// <returns></returns>
        [AdminFilter]
        public ActionResult Index()
        {
            PageVM<LogVM> loginLog = new LogService().GetLoginLogPageData(new LogQuery() { PageSize = 10 });

            if (loginLog == null) loginLog = new PageVM<LogVM>();

            return View(loginLog);
        }
        #endregion

        #region 获取登录记录(分页)
        /// <summary>
        /// 获取登录记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLoginLogPageData(LogQuery query)
        {
            if (query == null) query = new LogQuery();

            if (!query.PageIndex.HasValue)
                query.PageIndex = 1;

            if (!query.PageSize.HasValue)
                query.PageSize = 10;

            PageVM<LogVM> list = new LogService().GetLoginLogPageData(query);

            if (list == null || list.Data == null || list.Data.Count == 0)
            {
                return Json(new
                {
                    Code = -200,
                    Msg = "暂无数据"
                });
            }

            return Json(new
            {
                Code = 200,
                Msg = "获取成功",
                Data = list
            });
        }
        #endregion

    }
}