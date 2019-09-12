using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Core.Models.DataBaseModel.Mapping
{
    public class CompetitionProgramConfigurationMapping : IEntityTypeConfiguration<CompetitionProgram>
    {
        public void Configure(EntityTypeBuilder<CompetitionProgram> builder)
        {
            builder.HasKey(z => z.Id);
            builder.Property(e => e.BdImgUrl)
             .IsUnicode(false);
            builder.Property(e => e.ImgUrl)
             .IsUnicode(false);
            builder.HasMany(z => z.ProjectMembers)
            .WithOne(z => z.CompetitionProgram)
            .HasForeignKey(z => z.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
