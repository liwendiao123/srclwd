using Senparc.Core.Models.DataBaseModel;
using Senparc.Log;
using Senparc.Respository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Service
{
   public class CompetitionProgramService : BaseClientService<CompetitionProgram>
    {
        public CompetitionProgramService(CompetitionProgramRepository competitionProgramRepository) : base(competitionProgramRepository)
        {

        }

        public override void SaveObject(CompetitionProgram obj)
        {
            var isInsert = base.IsInsert(obj);
            base.SaveObject(obj);
            LogUtility.WebLogger.InfoFormat("CompetitionProgram{2}：{0}（ID：{1}）", obj.Name, obj.Id, isInsert ? "新增" : "编辑");
        }
        public override void DeleteObject(CompetitionProgram obj)
        {
            LogUtility.WebLogger.InfoFormat("CompetitionProgram被删除：{0}（ID：{1}）", obj.Name, obj.Id);
            base.DeleteObject(obj);
        }



    }
}
