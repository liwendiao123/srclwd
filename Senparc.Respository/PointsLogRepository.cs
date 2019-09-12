using Senparc.Core.Models;

namespace Senparc.Repository
{
    public interface IPointsLogRepository : IBaseClientRepository<PointsLog>
    {
    }

    public class PointsLogRepository : BaseClientRepository<PointsLog>, IPointsLogRepository
    {
        private readonly ISqlClientFinanceData _sqlClientFinanceData;

        public PointsLogRepository(ISqlClientFinanceData sqlClientFinanceData) : base(sqlClientFinanceData)
        {

        }
    }
}

