using System.Data.Entity;

namespace WinTetris
{
    public class PlayersContext : DbContext
    {
        public PlayersContext() : base("DefaultConnection")
        { }
        public DbSet<Player> Players { get; set; }
    }
}
