using Senparc.Core.Models.DataBaseModel;
using Senparc.Log;
using Senparc.Respository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Service
{
    public class ProjectMemberService : BaseClientService<ProjectMember>
    {
        public ProjectMemberService(ProjectMemberRepository projectMemberrepo) : base(projectMemberrepo)
        {

        }

        public override void SaveObject(ProjectMember obj)
        {
            var isInsert = base.IsInsert(obj);
            base.SaveObject(obj);
            LogUtility.WebLogger.InfoFormat("CompetitionProgram{2}：{0}（ID：{1}）", obj.Name, obj.Id, isInsert ? "新增" : "编辑");
        }
        public override void DeleteObject(ProjectMember obj)
        {
            LogUtility.WebLogger.InfoFormat("Schedule被删除：{0}（ID：{1}）", obj.Name, obj.Id);
            base.DeleteObject(obj);
        }
    }
}
