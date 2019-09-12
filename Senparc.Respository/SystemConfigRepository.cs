using Senparc.Core.Models;

namespace Senparc.Repository
{
    public interface ISystemConfigRepository : IBaseClientRepository<SystemConfig>
    {
    }

    public class SystemConfigRepository : BaseClientRepository<SystemConfig>, ISystemConfigRepository
    {
        private readonly ISqlClientFinanceData _sqlClientFinanceData;

        public SystemConfigRepository(ISqlClientFinanceData sqlClientFinanceData) : base(sqlClientFinanceData)
        {
            _sqlClientFinanceData = sqlClientFinanceData;
        }
    }
}

