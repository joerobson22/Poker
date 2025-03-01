using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace NEA_Computer_Science_Poker__️__️__️__️
{
    public partial class Stats : Form
    {
        private const string EXAMPLEDB = "PokerDatabase.mdb";
        private const string CONNECTION_STRING = @"Provider=Microsoft Jet 4.0 OLE DB Provider;Data Source = " + EXAMPLEDB + ";"; //connection string 
        public Player player;
        public Stats(Player player)
        {
            this.player = player;
            InitializeComponent();
        }


        //Load and setup subroutines
        private void StatsWindow_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();
            string[] HandRankWords = { "High Card", "Pair", "Two Pair", "Three Of A Kind", "Straight", "Flush", "Full House", "Four Of A Kind", "Straight Flush", "Royal Flush" };
            List<double> MoneyList = new List<double>();
            List<double> PotList = new List<double>();
            List<int> WonList = new List<int>();
            List<int> PlayedList = new List<int>();
            List<int> HandRankList = new List<int>();
            double MoneyMean = 0;
            double PotMean = 0;
            double WonMean = 0;
            double PlayedMean = 0;
            double MoneySD = 0;
            double PotSD = 0;
            double WonSD = 0;
            double PlayedSD = 0;

            LoadDataLoginTracker(MoneyList, PotList, WonList, PlayedList);

            LoadDataHandRanks(HandRankList);

            //Calculate and display win percentages for each hand rank
            List<double> WinPercentages = CalculateWinPercentages();

            DisplayData(MoneyList, PotList, WonList, PlayedList, HandRankWords, HandRankList, WinPercentages);

            //------------------------------------------------------------------------------------
            //calculate and display mean and standard deviations of each value
            MoneyMean = Math.Round(CalculateMean("BankMoney"), 2);
            PotMean = Math.Round(CalculateMean("LargestPot"), 2);
            WonMean = Math.Round(CalculateMean("HandsWon"), 2);
            PlayedMean = Math.Round(CalculateMean("HandsPlayed"), 2);


            MoneySD = Math.Round(CalculateSD(MoneyMean, "BankMoney"), 2);
            PotSD = Math.Round(CalculateSD(PotMean, "LargestPot"), 2);
            WonSD = Math.Round(CalculateSD(WonMean, "HandsWon"), 2);
            PlayedSD = Math.Round(CalculateSD(PlayedMean, "HandsPlayed"), 2);

            DisplayMeanAndSD(MoneyMean, PotMean, WonMean, PlayedMean, MoneySD, PotSD, WonSD, PlayedSD);
            //--------------------------------------------------------------------------------------

            

        }  //startup of window

        private void LoadDataLoginTracker(List<double> MoneyList, List<double> PotList, List<int> WonList, List<int> PlayedList)
        {
            //loads all coordinating columns from the database into their lists
            string SQLString = "SELECT * FROM LoginTracker "; // return all fields from the correct row
            OleDbConnection conn = new OleDbConnection(CONNECTION_STRING);
            OleDbCommand cmd = new OleDbCommand(SQLString, conn);
            conn.Open();
            OleDbDataReader DataReader = cmd.ExecuteReader(); //reads through each field in player's row
            try
            {
                while (DataReader.Read()) //while there are still rows to be read from, continue
                {
                    string username = Convert.ToString(DataReader["Username"]);
                    if (username == player.GetUsername()) //if this field is the player's, continue
                    {
                        MoneyList.Add(Convert.ToDouble(DataReader["BankMoney"]));
                        PotList.Add(Convert.ToDouble(DataReader["LargestPot"]));
                        WonList.Add(Convert.ToInt32(DataReader["HandsWon"]));
                        PlayedList.Add(Convert.ToInt32(DataReader["HandsPlayed"]));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            conn.Close();

        }

        private void LoadDataHandRanks(List<int> HandRankList)
        {
            //loads all data from the HandRankData table to be shown
            string[] FieldNames = { "HighCard", "Pair", "TwoPair", "ThreeOfAKind", "Straight", "Flush", "FullHouse", "FourOfAKind", "StraightFlush", "RoyalFlush" };

            string SQLString = "SELECT * FROM HandRankData "; // return all fields from the correct row
            OleDbConnection conn = new OleDbConnection(CONNECTION_STRING);
            OleDbCommand cmd = new OleDbCommand(SQLString, conn);
            conn.Open();
            OleDbDataReader DataReader = cmd.ExecuteReader(); //reads through each field in player's row
            try
            {
                while (DataReader.Read()) //while there are still rows to be read from, continue
                {
                    string username = Convert.ToString(DataReader["Username"]);
                    if (username == player.GetUsername()) //if this field is the player's, continue
                    {
                        for(int i = 0; i < 10; i++)
                        {
                            HandRankList.Add(Convert.ToInt32(DataReader[FieldNames[i]]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            conn.Close();
        }

        //-------------------------------------------

        //Calculation subroutines
        private double CalculateMean(string Field)
        {
            string SQLString = "SELECT AVG(" + Field + ") FROM PlayerData "; // return all fields from the correct row
            return Convert.ToDouble(DatabaseUtils.SqlQuery(SQLString));
        }

        private double CalculateSD(double Mean, string Field)
        {
            double SD = 0;
            //SD = SQRT(total of((x - mean)^2) / n)

            List<double> List = new List<double>();


            string SQLString = "SELECT * FROM PlayerData "; // return all fields from the correct row
            OleDbConnection conn = new OleDbConnection(CONNECTION_STRING);
            OleDbCommand cmd = new OleDbCommand(SQLString, conn);
            conn.Open();
            OleDbDataReader DataReader = cmd.ExecuteReader(); //reads through each field in player's row
            try
            {
                while (DataReader.Read()) //while there are still rows to be read from, continue
                {
                    List.Add(Convert.ToDouble(DataReader[Field]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            conn.Close();


            //find total difference of all items from all the means squared
            double TotalDifferenceSquared = 0;

            double Count = 0;
            for (int i = 0; i < List.Count; i++)
            {
                Count += 1;

                double Diff = List[i] - Mean;
                Diff = Diff * Diff;
                TotalDifferenceSquared += Diff;
            }

            TotalDifferenceSquared = TotalDifferenceSquared / Count;

            SD = Math.Sqrt(TotalDifferenceSquared);
            return SD;
        }


        private List<double> CalculateWinPercentages()
        {
            List<double> WinPercentages = new List<double>();
            string[] FieldNames = { "HighCard", "Pair", "TwoPair", "ThreeOfAKind", "Straight", "Flush", "FullHouse", "FourOfAKind", "StraightFlush", "RoyalFlush" };

            for (int i = 0; i < FieldNames.Length; i++)
            {
                //for every field name/ hand rank, loop through
                int Played = 0;
                int Wins = 0;

                string SQLString = "SELECT * FROM HandPlayerData"; // return all fields from the correct row
                OleDbConnection conn = new OleDbConnection(CONNECTION_STRING);
                OleDbCommand cmd = new OleDbCommand(SQLString, conn);
                conn.Open();
                OleDbDataReader DataReader = cmd.ExecuteReader();
                try
                {
                    while (DataReader.Read()) //while there are still rows to be read from, continue
                    {
                        string HandRank = Convert.ToString(DataReader["HandRank"]);
                        if (HandRank == FieldNames[i] && player.GetUsername() == Convert.ToString(DataReader["Username"]))
                        {
                            Played += 1;
                            if (Convert.ToDouble(DataReader["AmountWon"]) > 0)
                            {
                                //if the player won money from that hand, that is classed as a win
                                Wins += 1;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                conn.Close();


                //calculate percentages and add them to the list
                double Percentage = 0;

                if (Played > 0)
                {
                    Percentage = Convert.ToDouble(Convert.ToDouble(Wins) / Convert.ToDouble(Played));
                    Percentage *= 100.00;
                }


                WinPercentages.Add(Percentage);
            }


            return WinPercentages;
        }


        //----------------------------------------------

        //Display data subroutines

        private void DisplayData(List<double> MoneyList, List<double> PotList, List<int> WonList, List<int> PlayedList, string[] HandRankWords, List<int> HandRankList, List<double> WinPercentages)
        {
            //Uses the graph nodes to display the data we have obtained from the database
            string[] FieldNames = { "HighCard", "Pair", "TwoPair", "ThreeOfAKind", "Straight", "Flush", "FullHouse", "FourOfAKind", "StraightFlush", "RoyalFlush" };
            string[] Abbreviations = { "HC", "1P", "2P", "3P", "S", "F", "FH", "4P", "SF", "RF" };
            Random rnd = new Random();

            //as every list has the same number of entries we can loop round once and add a new point from each list to their graphs
            for (int i = 0; i < MoneyList.Count; i++)
            {
                MoneyGraph.Series["Money"].Points.AddXY(Convert.ToString(i), MoneyList[i]);
                LargestPotGraph.Series["LargestPotWon"].Points.AddXY(Convert.ToString(i), PotList[i]);
                HandsWonGraph.Series["HandsWon"].Points.AddXY(Convert.ToString(i), WonList[i]);
                HandsPlayedGraph.Series["HandsPlayed"].Points.AddXY(Convert.ToString(i), PlayedList[i]);
            }

            for(int i = 0; i < 10; i++)
            {
                DonutGraphHandRanks.Series["HandRankFrequency"].Points.AddXY(HandRankWords[i], HandRankList[i]);
            }

            for(int i = 0; i < WinPercentages.Count; i++)
            {
                WinPercentageGraph.Series[FieldNames[i]].Points.AddXY("", WinPercentages[i]);
            }
        }

        private void DisplayMeanAndSD(double MoneyMean, double PotMean, double WonMean, double PlayedMean, double MoneySD, double PotSD, double WonSD, double PlayedSD)
        {
            LabelMeanMoney.Text = "$" + Convert.ToString(MoneyMean);
            LabelMeanLargest.Text = "$" + Convert.ToString(PotMean);
            LabelMeanWon.Text = Convert.ToString(WonMean);
            LabelMeanPlayed.Text = Convert.ToString(PlayedMean);

            LabelSDMoney.Text = "$" + Convert.ToString(MoneySD);
            LabelSDLargest.Text = "$" + Convert.ToString(PotSD);
            LabelSDWon.Text = Convert.ToString(WonSD);
            LabelSDPlayed.Text = Convert.ToString(PlayedSD);

            LabelYoursMoney.Text = "$" + Convert.ToString(player.GetMoney());
            LabelYoursLargest.Text = "$" + Convert.ToString(player.GetLargestPotWon());
            LabelYoursWon.Text = Convert.ToString(player.GetHandsWon());
            LabelYoursPlayed.Text = Convert.ToString(player.GetHandsPlayed());
        }

        //--------------------------------------

        //Button subroutines

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form MM = new MainMenu(player);
            MM.ShowDialog();
            this.Close();
        }

        //------------------------------------
    }
}
