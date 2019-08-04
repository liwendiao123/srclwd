﻿using Senparc.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Senparc.Core.Cache
{
    using Models;
    using Senparc.CO2NET.Extensions;

    public class FullAccountCache : BaseStringDictionaryCache<FullAccount, Account> //, IFullAccountCache
    {
        /// <summary>
        /// UserId和UserName的映射关系
        /// </summary>
        public static Dictionary<int, string> UserIdNameRelationshop = new Dictionary<int, string>();

        public const string CACHE_KEY = "FullAccountCache";
        private const int timeout = 1440;

        /// <summary>
        /// 根据判断条件获取User
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        private Account GetAccount(Expression<Func<Account, bool>> where)
        {
            var account = base._db.DataContext.Accounts
                .FirstOrDefault(where);
            return account;
        }

        public FullAccountCache(ISqlClientFinanceData db)
            : base(CACHE_KEY, db, timeout)
        {
        }

        public override FullAccount Update()
        {
            return null;
        }

        public override FullAccount InsertObjectToCache(string key)
        {
            var account = this.GetAccount(z => z.UserName.Equals(key, StringComparison.OrdinalIgnoreCase));
            var fullUser = this.InsertObjectToCache(key, account);
            return fullUser;
        }

        public override FullAccount GetObject(string key)

        {
            return base.GetObject(key);
        }

        public override FullAccount InsertObjectToCache(string key, Account obj)
        {
            var fullUser = base.InsertObjectToCache(key, obj);
            if (fullUser == null)
            {
                return null;
            }
            UserIdNameRelationshop[fullUser.Id] = fullUser.UserName;//TODO:需要使用分布式缓存

            this.UpdateToCache(key, fullUser); //需要更新对象到Redis
            return fullUser;
        }

        public void ForceLogout(string userName)
        {
            if (userName.IsNullOrEmpty())
            {
                return;
            }
            var fullUser = GetObject(userName);
            if (fullUser == null)
            {
                return;
            }
            fullUser.ForceLogout = true;
            fullUser.LastActiveTime = DateTime.MinValue;
        }

        public string GetUserName(int accountId)
        {
            var userName = UserIdNameRelationshop.ContainsKey(accountId)
                ? UserIdNameRelationshop[accountId]
                : null;
            return userName;
        }
        public FullAccount GetFullAccount(int accountId)
        {
            var userName = GetUserName(accountId);
            if (userName == null)
            {
                //未命中，查找数据库
                var account = this.GetAccount(z => z.Id == accountId);
                if (account == null)
                {
                    return null;
                }
                var fullUser = this.InsertObjectToCache(account.UserName, account);
                return fullUser;
            }
            return GetObject(userName);
        }

        public override void RemoveCache()
        {
            throw new Exception("不允许调用此方法！");
        }
    }
}