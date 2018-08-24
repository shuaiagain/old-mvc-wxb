﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Devx;
using WXB.Bussiness.Models;
using WXB.Bussiness.Common;
using Devx.DbProvider;
using System.Security.Cryptography;
using WXB.Bussiness.Utils;
using System.Configuration;
using WXB.Bussiness.ViewModels;

namespace WXB.Bussiness.Service
{
    public class UserService
    {

        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserVM Login(string userName, string password)
        {

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password)) return null;

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connCMS, DbProviderTypes.MySql))
            {

                string sql = string.Format(@"select  
	                                            ud.ID,
	                                            ud.Alias,
	                                            ud.Name,
	                                            ud.Email,
                                                ud.Flag,
	                                            ur.RoleID as RoleType 
                                             from userdata ud left join userrole ur on ud.ID = ur.UserID 
                                             where ud.name = '{0}' and ud.Password = '{1}' limit 0,1 ", userName, MD5Util.GetMD5(password));
                UserVM user = dbContext.Sql(sql).QuerySingle<UserVM>(reader =>
                {

                    var vm = new UserVM()
                    {
                        ID = reader.AsInt("ID"),
                        Name = reader.AsString("Name"),
                        Alias = reader.AsString("Alias"),
                        Email = reader.AsString("Email"),
                        RoleType = reader.AsInt("RoleType")
                    };

                    if (!string.IsNullOrEmpty(reader["Flag"].ToString()))
                        vm.Flag = reader.AsInt("Flag");

                    return vm;
                });

                if (user == null) return null;

                return user;
            }
        }
        #endregion

        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserVM GetUserById(int userId)
        {

            if (userId == 0) return null;

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connCMS, DbProviderTypes.MySql))
            {

                string sql = string.Format(@"select  
	                                            ud.ID,
	                                            ud.Alias,
	                                            ud.Name,
                                                ud.Flag,
	                                            ur.RoleID as RoleType 
                                             from userdata ud left join userrole ur on ud.ID = ur.UserID 
                                             where ud.ID = '{0}' limit 0,1 ", userId);
                UserVM user = dbContext.Sql(sql).QuerySingle<UserVM>(reader =>
                {

                    var vm = new UserVM()
                    {
                        ID = reader.AsInt("ID"),
                        Name = reader.AsString("Name"),
                        Alias = reader.AsString("Alias"),
                        RoleType = reader.AsInt("RoleType")
                    };

                    if (!string.IsNullOrEmpty(reader["Flag"].ToString()))
                        vm.Flag = reader.AsInt("Flag");

                    return vm;
                });

                if (user == null) return null;

                return user;
            }
        }
        #endregion


        #region 获取用户列表
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public List<UserItemVM> GetUserList()
        {

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connCMS, DbProviderTypes.MySql))
            {
                string sql = @" select 
	                                ID,
	                                Name,
	                                Alias
                                from userdata ud 
                                where 1=1 ";

                List<UserItemVM> userList = dbContext.Sql(sql).Query<UserItemVM, List<UserItemVM>>(reader =>
                {
                    return new UserItemVM()
                    {

                        ID = reader.AsInt("ID"),
                        Name = reader.AsString("Name"),
                        Alias = reader.AsString("Alias")
                    };
                });

                if (userList == null || userList.Count == 0) return null;

                return userList;
            }
        }
        #endregion
    }
}
