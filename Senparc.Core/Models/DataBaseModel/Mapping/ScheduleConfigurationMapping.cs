using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Core.Models.DataBaseModel.Mapping
{
    public class ScheduleConfigurationMapping : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.HasKey(z => z.Id);
            builder.HasMany(z => z.CompetitionPrograms)
              .WithOne(z => z.Schedule)
              .HasForeignKey(z => z.ScheduleId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
