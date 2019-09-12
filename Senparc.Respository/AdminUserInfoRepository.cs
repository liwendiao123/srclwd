using Senparc.Core.Models;

namespace Senparc.Repository
{
    public interface IAdminUserInfoRepository : IBaseClientRepository<AdminUserInfo>
    {
    }

    public class AdminUserInfoRepository : BaseClientRepository<AdminUserInfo>, IAdminUserInfoRepository
    {
        private readonly ISqlClientFinanceData _sqlClientFinanceData;

        public AdminUserInfoRepository(ISqlClientFinanceData sqlClientFinanceData) : base(sqlClientFinanceData)
        {
            _sqlClientFinanceData = sqlClientFinanceData;
        }
    }
}

