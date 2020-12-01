using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voting.Entities
{
    public class VoteDbContext : DbContext
    {
        public DbSet<State> States { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=.\sqlexpress;Database=VoteDb;Integrated Security=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<State>(e => {
                e.ToTable("States").HasKey(p => p.StateName).IsClustered();
                e.Property(p => p.StateName).IsRequired().HasMaxLength(25);
                e.Property(p => p.IsSwing).IsRequired();
                e.Property(p => p.AsOfDate).IsRequired().HasColumnType("datetime2(3)");
            });

            modelBuilder.Entity<Vote>(e => {
                e.ToTable("Votes").HasKey(p => p.Id).IsClustered(false);
                e.Property(p => p.Id).IsRequired().UseIdentityColumn(1, 1);
                e.Property(p => p.StateName).IsRequired().IsUnicode(false).HasMaxLength(25);
                e.Property(p => p.PrecinctsPercent).IsRequired();
                e.Property(p => p.VoteTimestamp).IsRequired().HasColumnType("datetime2(3)");

                e.Property(p => p.TotalVotes).IsRequired();
                e.Property(p => p.PreviousTotalVotes).IsRequired().HasDefaultValue(0);

                e.Property(p => p.TrumpPercentOfTotal).IsRequired().HasColumnType("decimal(6,3)");
                e.Property(p => p.BidenPercentOfTotal).IsRequired().HasColumnType("decimal(6,3)");
                e.Property(p => p.ThirdPartyPercentOfTotal).IsRequired().HasColumnType("decimal(6,3)");

                e.Property(p => p.PreviousTrumpPercentOfTotal).IsRequired().HasColumnType("decimal(6,3)");
                e.Property(p => p.PreviousBidenPercentOfTotal).IsRequired().HasColumnType("decimal(6,3)");
                e.Property(p => p.PreviousThirdPartyPercentOfTotal).IsRequired().HasColumnType("decimal(6,3)");
                
                e.Property(p => p.TrumpVotes).IsRequired();
                e.Property(p => p.BidenVotes).IsRequired();
                e.Property(p => p.ThirdPartyVotes).IsRequired();

                e.Property(p => p.PreviousTrumpVotes).IsRequired();
                e.Property(p => p.PreviousBidenVotes).IsRequired();
                e.Property(p => p.PreviousThirdPartyVotes).IsRequired();

                e.Property(p => p.TrumpPercentOfVoteBatch).IsRequired().HasColumnType("decimal(6,3)");
                e.Property(p => p.BidenPercentOfVoteBatch).IsRequired().HasColumnType("decimal(6,3)");
                e.Property(p => p.ThirdPartyPercentOfVoteBatch).IsRequired().HasColumnType("decimal(6,3)");

                e.Property(p => p.MinVoteSensitivity).IsRequired();

                e.Property(p => p.TotalVoteChange).HasComputedColumnSql("[TotalVotes]-[PreviousTotalVotes]");
                e.Property(p => p.TrumpVoteChange).HasComputedColumnSql("[TrumpVotes]-[PreviousTrumpVotes]");
                e.Property(p => p.BidenVoteChange).HasComputedColumnSql("[BidenVotes]-[PreviousBidenVotes]");
                e.Property(p => p.ThirdPartyVoteChange).HasComputedColumnSql("[ThirdPartyVotes]-[PreviousThirdPartyVotes]");
                e.Property(p => p.TrumpPercentChange).HasComputedColumnSql("[TrumpPercentOfTotal]-[PreviousTrumpPercentOfTotal]");
                e.Property(p => p.BidenPercentChange).HasComputedColumnSql("[BidenPercentOfTotal]-[PreviousBidenPercentOfTotal]");
                e.Property(p => p.ThirdPartyPercentChange).HasComputedColumnSql("[ThirdPartyPercentOfTotal]-[PreviousThirdPartyPercentOfTotal]");
            });
            modelBuilder.Entity<Vote>()
                .HasIndex(p => new { p.StateName, p.VoteTimestamp })
                .IsClustered()
                .HasDatabaseName("CIDX_Votes_StateNameVoteTimestamp");
        }
    }
}
