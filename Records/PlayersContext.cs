using System.Data.Entity;

namespace BlockPuzzle
{
    public class PlayersContext : DbContext
    {
        public PlayersContext() : base("DefaultConnection")
        { }
        public DbSet<Player> Players { get; set; }
    }
}
