﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXB.Bussiness.ViewModels
{
    public class ChooseUserVM
    {
        /// <summary>
        /// 选中的用户ID
        /// </summary>
        public string UserIds { get; set; }

            /// <summary>
        /// 指定人员方式
        /// </summary>
        public int? ChooseUserWay { get; set; }

    }
}
