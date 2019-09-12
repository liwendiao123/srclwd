using Senparc.Core.Models.DataBaseModel;
using Senparc.Log;
using Senparc.Respository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Service
{

    /// <summary>
    ///saiceng
    /// </summary>
   public class ScheduleService : BaseClientService<Schedule>
    {
        public ScheduleService(ScheduleRepository scheduleRepo)
          : base(scheduleRepo)
        {

        }


        public override void SaveObject(Schedule obj)
        {

            obj.UpdateTime = DateTime.Now;
            var isInsert = base.IsInsert(obj);
            base.SaveObject(obj);
            LogUtility.WebLogger.InfoFormat("CompetitionProgram{2}：{0}（ID：{1}）", obj.Name, obj.Id, isInsert ? "新增" : "编辑");
        }
        public override void DeleteObject(Schedule obj)
        {
            LogUtility.WebLogger.InfoFormat("Schedule被删除：{0}（ID：{1}）", obj.Name, obj.Id);
            base.DeleteObject(obj);
        }
    }
}
