using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WXB.Bussiness.Service;
using WXB.Bussiness.ViewModels;
using WXB.Bussiness.Common;
using WXB.Bussiness.Models;

namespace Wxb2018.Controllers
{
    public class ResponseController : BaseController
    {

        #region 获取响应状态
        /// <summary>
        /// 获取响应状态
        /// </summary>
        /// <returns></returns>
        public ActionResult GetResponseStatus()
        {
            ResponseStatusView resVM = new ResponseStatusService().GetResponseStatus();
            if (resVM == null)
            {
                return Json(new { Code = -200, Msg = "暂无数据" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Code = 200,
                Msg = "获取成功",
                Data = new
                {
                    ID = resVM.ID,
                    Status = resVM.Status,
                    StatusText = resVM.Status.HasValue ? ((EnumResponseStatus)resVM.Status).ToString() : ""
                }
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 编辑
        public ActionResult Edit(ResponseQuery query)
        {
            if (query == null) query = new ResponseQuery() { ID = 0 };

            ResponseStatusView vm = new ResponseStatusService().GetResponseStatusByID(query.ID.Value);
            return View("Edit", vm);
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public ActionResult SaveResponse(ResponseStatusView vm)
        {

            if (vm == null)
            {
                return Json(new
                {
                    Code = -400,
                    Msg = "参数不能为空",
                });
            }

            try
            {
                ResponseStatusService rpSV = new ResponseStatusService();

                vm.Inputer = UserData.Name;
                vm.InputerID = UserData.UserId;
                vm = rpSV.SaveResponse(vm);

                #region 日志

                var log = new LogVM()
                {
                    Operator = this.UserData.Name,
                    OperatorID = this.UserData.UserId,
                    RoleType = this.UserData.RoleType,
                    OperateTime = DateTime.Now,
                    OperateType = (int)EnumOperateType.编辑
                };

                log.OperateDescribe = ((EnumOperateType)log.OperateType).ToString() + "响应状态";
                Logger.AddLog(log);

                #endregion

                return Json(new
                {
                    Code = 200,
                    Msg = "保存成功",
                    Data = new
                    {
                        ID = vm.ID,
                        Status = vm.Status,
                        StatusText = ((EnumResponseStatus)vm.Status).ToString()
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
    }
}