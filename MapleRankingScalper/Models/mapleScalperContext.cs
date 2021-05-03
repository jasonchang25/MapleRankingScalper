using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MapleRankingScalper.Models
{
    public partial class mapleScalperContext : DbContext
    {
        public mapleScalperContext()
        {
        }

        public mapleScalperContext(DbContextOptions<mapleScalperContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Character> Characters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LocalHost\\SQLExpress;database=MapleScalper;Integrated Security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>(entity =>
            {
                entity.HasKey(e => e.pkCharacterId);

                entity.Property(e => e.pkCharacterId).HasColumnName("pkCharacterId");

                entity.Property(e => e.Rank);

                entity.Property(e => e.WorldName).HasMaxLength(30)
                                                 .IsUnicode(false)
                                                 .IsFixedLength();

                entity.Property(e => e.JobName).HasMaxLength(30)
                                                 .IsUnicode(false)
                                                 .IsFixedLength();

                entity.Property(e => e.CharacterName).HasMaxLength(50)
                                                     .IsUnicode(false)
                                                     .IsFixedLength();

                entity.Property(e => e.JobId);

                entity.Property(e => e.Level);

                entity.Property(e => e.WorldId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
