using System.Linq;
using System.Reflection;

namespace Senparc.Core.Models
{
    using Microsoft.EntityFrameworkCore;
    using Senparc.Core.Models.DataBaseModel;
    using Senparc.Core.Models.DataBaseModel.Mapping;

    public partial class SenparcEntities : DbContext
    {
        public SenparcEntities(DbContextOptions<SenparcEntities> dbContextOptions) : base(dbContextOptions)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<AdminUserInfo> AdminUserInfos { get; set; }

        public DbSet<SystemConfig> SystemConfigs { get; set; }

        public DbSet<FeedBack> FeedBacks { get; set; }

        public virtual DbSet<PointsLog> PointsLogs { get; set; }

        public virtual DbSet<AccountPayLog> AccountPayLogs { get; set; }

        /// <summary>
        /// 活动
        /// </summary>
        public virtual DbSet<Activity> Activities { get; set; }

        /// <summary>
        /// 项目成员
        /// </summary>
        public virtual DbSet<ProjectMember> ProjectMembers { get; set; }

        /// <summary>
        /// 参赛项目
        /// </summary>
        public virtual DbSet<CompetitionProgram> CompetitionPrograms { get; set; }

        /// <summary>
        /// 赛程
        /// </summary>
        public virtual DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfigurationMapping());
            modelBuilder.ApplyConfiguration(new AdminUserInfoConfigurationMapping());
            modelBuilder.ApplyConfiguration(new FeedbackConfigurationMapping());
            modelBuilder.ApplyConfiguration(new AccountPayLogConfigurationMapping());
            modelBuilder.ApplyConfiguration(new PointsLogConfigurationMapping());
            modelBuilder.ApplyConfiguration(new ActivityConfigurationMapping());
            modelBuilder.ApplyConfiguration(new ScheduleConfigurationMapping());
            modelBuilder.ApplyConfiguration(new CompetitionProgramConfigurationMapping());
            modelBuilder.ApplyConfiguration(new ProjectMemberConfigurationMapping());
        }

        /// <summary>
        /// 
        /// </summary>
        private static readonly MethodInfo SetGlobalQueryMethodInfo = typeof(SenparcEntities)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        public void SetGlobalQuery<T>(ModelBuilder builder) where T : BaseModel
        {
            builder.Entity<T>().HasQueryFilter(z => z.Flag);
        }
    }
}
