using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Core.Models.DataBaseModel.Mapping
{
    public class ActivityConfigurationMapping : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(z => z.Id);
            builder.Property(e => e.CoverUrl)
               .IsUnicode(false);
            builder.HasMany(z => z.Schedules)
                .WithOne(z => z.Activity)
                .HasForeignKey(z => z.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
