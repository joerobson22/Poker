using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA_Computer_Science_Poker__️__️__️__️
{
    public class Player
    {
        //variables used to identify the player and keep track of their stats
        private string Username;
        private double Money;
        private int HandsWon;
        private double LargestPotWon;
        private int HandsPlayed;
        private int TotalLogins;

        //variables used to give the player functionality in the game
        private double RoundMoney = 0;
        private double HandMoney = 0;
        private double TotalWonThisRound;
        private bool Folded = false;
        private bool AllIn = false;
        private int HandRank;
        private double GameMoney;

        private int PlayerNumber = -1;
       
        private const string EXAMPLEDB = "PokerDatabase.mdb";
        private const string CONNECTION_STRING = @"Provider=Microsoft Jet 4.0 OLE DB Provider;Data Source = " + EXAMPLEDB + ";"; //connection string 


        public Player(string username, double money, int handsWon, double largestPotWon, int handsPlayed, int totalLogins)
        {
            Username = username;
            Money = money;
            HandsWon = handsWon;
            LargestPotWon = largestPotWon;
            HandsPlayed = handsPlayed;
            TotalLogins = totalLogins;  
        }


        //GET SUBROUTINES
        public double GetLargestPotWon()
        {
            return LargestPotWon;
        }


        public int GetHandsWon()
        {
            return HandsWon;
        }


        public double GetTotalWonThisRound()
        {
            return TotalWonThisRound;
        }


        public double GetHandMoney()
        {
            return HandMoney;
        }


        public bool GetAllIn()
        {
            return AllIn;
        }


        public int GetTotalLogins()
        {
            return TotalLogins;
        }

        public double GetGameMoney()
        {
            return GameMoney;
        }


        public int GetHandsPlayed()
        {
            return HandsPlayed;
        }


        public int GetPlayerNumber()
        {
            return PlayerNumber;
        }


        public int GetHandRank()
        {
            return HandRank;
        }

        public bool GetFolded()
        {
            return Folded;
        }

        public double GetRoundMoney()
        {
            return RoundMoney;
        }

        public string GetUsername()
        {
            return Username;
        }

        public double GetMoney()
        {
            return Money;
        }

        //-------------------------


        //SET SUBROUTINES
        public void SetTotalWonThisRound(double amount)
        {
            TotalWonThisRound = amount;
            TotalWonThisRound = Math.Round(TotalWonThisRound, 2);
        }


        public void SetHandMoney(double num)
        {
            HandMoney = num;
        }


        public void SetAllIn(bool state)
        {
            AllIn = state;
        }


        public void SetGameMoney(double num)
        {
            GameMoney = num;
            GameMoney = Math.Round(GameMoney, 2);
        }


        public void SetRoundMoney(double num)
        {
            RoundMoney = num;
            RoundMoney = Math.Round(RoundMoney, 2);
        }


        //------------------------


        //MODIFY SUBROUTINES
        public void ModifyTotalWonThisRound(double amount)
        {
            TotalWonThisRound += amount;
            TotalWonThisRound = Math.Round(TotalWonThisRound, 2);
        }


        public void ModifyHandMoney(double num)
        {
            HandMoney += num;
        }


        public void ModifyGameMoney(double num)
        {
            GameMoney += num;
            GameMoney = Math.Round(GameMoney, 2);
        }


        public void ModifyHandsPlayed(int num)
        {
            HandsPlayed += num;
        }


        public void ModifyHandRank(int num)
        {
            HandRank = num;
        }

        public void ModifyFolded(bool val)
        {
            Folded = val;
        }

        public void ModifyRoundMoney(double num)
        {
            RoundMoney += num;
            RoundMoney = Math.Round(RoundMoney, 2);
        }

        public void ModifyMoney(double amount)
        {
            Money += amount;
            Money = Math.Round(Money, 2);
        }

        public void ModifyHandsWon(int amount)
        {
            HandsWon += amount;
        }

        public void ModifyLargestPotWon(double amount)
        {
            if (amount > LargestPotWon)
            {
                LargestPotWon = amount;
            }
        }

        //-------------------------


        //UPDATE SUBROUTINES

        public void UpdateHandRankStats()
        {
            string[] FieldNames = { "HighCard", "Pair", "TwoPair", "ThreeOfAKind", "Straight", "Flush", "FullHouse", "FourOfAKind", "StraightFlush", "RoyalFlush" };

            OleDbConnection connection = new OleDbConnection(CONNECTION_STRING);
            Console.WriteLine(GetHandRank());
            connection.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT " + FieldNames[GetHandRank() - 1] + " FROM HandRankData WHERE Username = '" + GetUsername() + "'", connection); //select the password from player's row

            int Val = Convert.ToInt32(cmd.ExecuteScalar());
            Val += 1;
            connection.Close();

            string SQLString = "UPDATE HandRankData SET " + FieldNames[GetHandRank() - 1] + " = " + Val + " WHERE Username = '" + GetUsername() + "'";
            DatabaseUtils.ExecuteSqlNonQuery(SQLString);
        }

        //-----------------------

        //OTHER

        public void BuyIn(double BuyInAmount)
        {
            if (Money > 0.0 && BuyInAmount > GameMoney)
            {
                double diff = BuyInAmount - GameMoney;

                if (diff > Money)
                {
                    GameMoney = GameMoney + Money;
                    Money = 0;
                }
                else
                {
                    GameMoney = BuyInAmount;
                    Money -= diff;
                }

            }
        }

        //--------------------------------------


        
    }
}
