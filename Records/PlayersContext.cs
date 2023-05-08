using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTetris
{
    public class PlayersContext : DbContext
    {
        public PlayersContext() : base("DefaultConnection")
        { }
        public DbSet<Player> Players { get; set; }
    }
}
