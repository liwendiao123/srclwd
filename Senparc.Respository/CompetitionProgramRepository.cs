using Senparc.Core.Models;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Respository
{
   public interface ICompetitionProgramRepository : IBaseClientRepository<CompetitionProgram>
    {
    }

    public class CompetitionProgramRepository : BaseClientRepository<CompetitionProgram>, ICompetitionProgramRepository
    {
        private readonly ISqlClientFinanceData _sqlClientFinanceData;

        public CompetitionProgramRepository(ISqlClientFinanceData sqlClientFinanceData) : base(sqlClientFinanceData)
        {
            _sqlClientFinanceData = sqlClientFinanceData;
        }
    }
}
