using Senparc.Core.Models;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Respository
{
  public  interface IScheduleRepository : IBaseClientRepository<Schedule>
    {
    }

    public class ScheduleRepository: BaseClientRepository<Schedule>, IScheduleRepository
    {
        private readonly ISqlClientFinanceData _sqlClientFinanceData;

        public ScheduleRepository(ISqlClientFinanceData sqlClientFinanceData) : base(sqlClientFinanceData)
        {

        }
    }
}
