using Microsoft.AspNetCore.Http;
using Senparc.Core.Enums;
using Senparc.Core.Models;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Log;
using Senparc.Respository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Senparc.Service
{
 public  class ActivityService:BaseClientService<Activity>
    {
        private readonly Lazy<IHttpContextAccessor> _httpContextAccessor;

        public ActivityService(ActivityRepository accountRepo, Lazy<IHttpContextAccessor> httpContextAccessor)
       : base(accountRepo)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void SaveObject(Activity obj)
        {
            var isInsert = base.IsInsert(obj);
            base.SaveObject(obj);
            LogUtility.WebLogger.InfoFormat("ProjectMember{2}：{0}（ID：{1}）", obj.Content, obj.Id, isInsert ? "新增" : "编辑");
        }
        public override void DeleteObject(Activity obj)
        {
            LogUtility.WebLogger.InfoFormat("ProjectMember被删除：{0}（ID：{1}）", obj.Content, obj.Id);
            base.DeleteObject(obj);
        }

        public override PagedList<Activity> GetObjectList<TK>(int pageIndex, int pageCount, Expression<Func<Activity, bool>> where, Expression<Func<Activity, TK>> orderBy, OrderingType orderingType, string[] includes = null)
        {
            return base.GetObjectList(pageIndex, pageCount, where, orderBy, orderingType, includes);
        }
    }
}
