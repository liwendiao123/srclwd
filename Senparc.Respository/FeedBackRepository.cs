using Senparc.Core.Models;

namespace Senparc.Repository
{
    public interface IFeedBackRepository : IBaseClientRepository<FeedBack>
    {
    }

    public class FeedBackRepository : BaseClientRepository<FeedBack>, IFeedBackRepository
    {
        private readonly ISqlClientFinanceData _sqlClientFinanceData;

        public FeedBackRepository(ISqlClientFinanceData sqlClientFinanceData) : base(sqlClientFinanceData)
        {

        }
    }
}