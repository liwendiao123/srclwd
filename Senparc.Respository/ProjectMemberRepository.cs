using Senparc.Core.Models;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Respository
{
   public interface IProjectMemberRepository : IBaseClientRepository<ProjectMember>
    {
    }

    public class ProjectMemberRepository : BaseClientRepository<ProjectMember>, IProjectMemberRepository
    {
        private readonly ISqlClientFinanceData _sqlClientFinanceData;

        public ProjectMemberRepository(ISqlClientFinanceData sqlClientFinanceData) : base(sqlClientFinanceData)
        {

        }
    }
}
