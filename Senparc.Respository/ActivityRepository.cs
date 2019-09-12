using Senparc.Core.Models;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Respository
{
   public interface IActivityRepository : IBaseClientRepository<Activity>
    {
    }

    public class ActivityRepository : BaseClientRepository<Activity>, IActivityRepository
    {
        private readonly ISqlClientFinanceData _sqlClientFinanceData;

        public ActivityRepository(ISqlClientFinanceData sqlClientFinanceData) : base(sqlClientFinanceData)
        {

        }
    }
}
