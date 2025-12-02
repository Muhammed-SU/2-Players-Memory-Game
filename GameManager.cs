using HafzaOyunu;
using System.Collections.Generic;
using System.Linq;

namespace HafzaOyunu
{
    public class GameManager
    {
        public List<GameImage> Images { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public int CurrentPlayerIndex { get; set; } = 0;
        public GameImage FirstSelected { get; set; }

        public GameManager(List<GameImage> images, Player p1, Player p2)
        {
            Images = images;
            Player1 = p1;
            Player2 = p2;
        }

        public Player CurrentPlayer => CurrentPlayerIndex == 0 ? Player1 : Player2;

        public void SwitchPlayer()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % 2;
        }

        public bool CheckMatch(GameImage secondSelected)
        {
            if (FirstSelected.ImagePath == secondSelected.ImagePath)
            {
                FirstSelected.IsMatched = true;
                secondSelected.IsMatched = true;
                CurrentPlayer.AddPoint();
                return true;
            }
            return false;
        }

        public bool IsGameOver()
        {
            return Player1.Score >= 11 || Player2.Score >= 11 || Images.All(img => img.IsMatched);
        }
    }
}