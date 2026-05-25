using Microsoft.EntityFrameworkCore;
using GBS.Api.DbModels;

namespace GBS.Api.Data
{
    public class GBS_DbContext : DbContext
    {
        public GBS_DbContext(DbContextOptions<GBS_DbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<TeamPlayer> TeamPlayers { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentTeam> TournamentTeams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
        public DbSet<Innings> Innings { get; set; }
        public DbSet<Ball> Balls { get; set; }
        public DbSet<BattingScore> BattingScores { get; set; }
        public DbSet<BowlingFigure> BowlingFigures { get; set; }
        public DbSet<FallOfWicket> FallOfWickets { get; set; }
        public DbSet<Commentary> Commentaries { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<OutboxBall> OutboxBalls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Team>().ToTable("Teams");
            modelBuilder.Entity<Player>().ToTable("Players");
            modelBuilder.Entity<TeamPlayer>().ToTable("TeamPlayers");
            modelBuilder.Entity<Tournament>().ToTable("Tournaments");
            modelBuilder.Entity<TournamentTeam>().ToTable("TournamentTeams");
            modelBuilder.Entity<Match>().ToTable("Matches");
            modelBuilder.Entity<ApprovalRequest>().ToTable("ApprovalRequests");
            modelBuilder.Entity<Innings>().ToTable("Innings");
            modelBuilder.Entity<Ball>().ToTable("Balls");
            modelBuilder.Entity<BattingScore>().ToTable("BattingScores");
            modelBuilder.Entity<BowlingFigure>().ToTable("BowlingFigures");
            modelBuilder.Entity<FallOfWicket>().ToTable("FallOfWickets");
            modelBuilder.Entity<Commentary>().ToTable("Commentaries");
            modelBuilder.Entity<Sponsor>().ToTable("Sponsors");
            modelBuilder.Entity<Notification>().ToTable("Notifications");
            modelBuilder.Entity<OutboxBall>().ToTable("OutboxBalls");

            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Team>().HasIndex(t => t.ShortCode).IsUnique();
        }
    }
}
