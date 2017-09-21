using DotNetCore.Core.Domain.Messages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetCore.Data.Mapping.Messages
{
    public class QueuedEmailMap : IEntityTypeConfiguration<QueuedEmail>
    {
        public void Configure(EntityTypeBuilder<QueuedEmail> builder)
        {
            builder.HasKey(qe => qe.Id);
            builder.Property(qe => qe.From).IsRequired().HasMaxLength(500);
            builder.Property(qe => qe.FromName).HasMaxLength(500);
            builder.Property(qe => qe.To).IsRequired().HasMaxLength(500);
            builder.Property(qe => qe.ToName).HasMaxLength(500);
            builder.Property(qe => qe.ReplyTo).HasMaxLength(500);
            builder.Property(qe => qe.ReplyToName).HasMaxLength(500);
            builder.Property(qe => qe.CC).HasMaxLength(500);
            builder.Property(qe => qe.Bcc).HasMaxLength(500);
            builder.Property(qe => qe.Subject).HasMaxLength(1000);
            builder.Ignore(qe => qe.Priority);
            builder.HasOne(qe => qe.EmailAccount)
                   .WithMany()
                   .HasForeignKey(qe => qe.EmailAccountId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
