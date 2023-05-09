using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace WinTetris
{
    public static class WorkWithRecords
    {
        /// <summary>
        /// Read players from database
        /// </summary>
        /// <returns>players</returns>
        public static List<Player> GetPlayers()
        {
            using (PlayersContext db = new PlayersContext())
            {
                db.Players.Load();
                return db.Players.ToList();
            }
        }

        /// <summary>
        /// check if it need to add player into top-15
        /// </summary>
        /// <param name="currentScore">current score for check</param>
        /// <returns>true if player enter in top-15</returns>
        public static bool IsEntered(int currentScore)
        {
            using (PlayersContext db = new PlayersContext())
            {
                //if count less then 15
                db.Players.Load();
                if (db.Players.Count() < Constants.MaxRecords)
                {
                    return true;
                }

                //if min score in db less then a current one
                int min = db.Players.Min(player => player.Score);
                return min < currentScore ? true : false;
            }
        }

        /// <summary>
        /// Add new player into the database. Check for count of players and remove if needed
        /// </summary>
        /// <param name="player">player for add</param>
        public static void AddPlayer(Player player)
        {
            using (PlayersContext db = new PlayersContext())
            {
                //add player
                db.Players.Load();
                db.Players.Add(player);

                //remove if needed
                if (db.Players.Count() == Constants.MaxRecords)
                {
                    RemoveLast(db);
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Remove last player from DBSet
        /// </summary>
        /// <param name="db">PlayersContext object</param>
        private static void RemoveLast(PlayersContext db)
        {
            int min = db.Players.Min(player => player.Score);
            Player lastPlayer = db.Players.Where(player => player.Score == min).OrderBy(player => player.Name).FirstOrDefault();
            db.Players.Remove(lastPlayer);
        }
    }
}