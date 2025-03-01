using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA_Computer_Science_Poker__️__️__️__️
{
    public partial class MainMenu : Form
    {
        Player player;
        private const string EXAMPLEDB = "PokerDatabase.mdb";
        private const string CONNECTION_STRING = @"Provider=Microsoft Jet 4.0 OLE DB Provider;Data Source = " + EXAMPLEDB + ";";


        public MainMenu(Player player)
        {
            this.player = player;
            InitializeComponent();
        }

        //Load and display details subroutines

        private void MainMenu_Load(object sender, EventArgs e)
        {
            //display player details
            UsernameBankLabel.Text = player.GetUsername() + "'s Bank:";
            MoneyLabel.Text = "Money: $" + Convert.ToString(player.GetMoney());
            HandsWonLabel.Text = "Hands Won: " + Convert.ToString(player.GetHandsWon());
            LargestPotWonLabel.Text = "Largest Pot Won: $" + Convert.ToString(player.GetLargestPotWon());


            SavePlayerData();
        } //startup of the main menu window

        //---------------------------------------

        //Save data subroutines

        private void SavePlayerData()
        {
            //update player bank money, largest pot won and hands won in the database table PlayerData
            string SQLString = "UPDATE PlayerData SET BankMoney = " + player.GetMoney() + " WHERE Username = '" + player.GetUsername() + "'";
            DatabaseUtils.ExecuteSqlNonQuery(SQLString);
            SQLString = "UPDATE PlayerData SET LargestPot = " + player.GetLargestPotWon() + " WHERE Username = '" + player.GetUsername() + "'";
            DatabaseUtils.ExecuteSqlNonQuery(SQLString);
            SQLString = "UPDATE PlayerData SET HandsWon = " + player.GetHandsWon() + " WHERE Username = '" + player.GetUsername() + "'";
            DatabaseUtils.ExecuteSqlNonQuery(SQLString);
            SQLString = "UPDATE PlayerData SET HandsPlayed = " + player.GetHandsPlayed() + " WHERE Username = '" + player.GetUsername() + "'";
            DatabaseUtils.ExecuteSqlNonQuery(SQLString);
            SQLString = "UPDATE PlayerData SET TotalLogins = " + player.GetTotalLogins() + " WHERE Username = '" + player.GetUsername() + "'";
            DatabaseUtils.ExecuteSqlNonQuery(SQLString);
        }     //saves all player data to the database


        //------------------------------------

        //Button subroutines

        private void PlayButton_Click_1(object sender, EventArgs e)
        {
            //Switch windows to the table customisation table
            //hide this window, instantiate new window, show new window, close current window
            this.Hide();
            Form TblCust = new TableCustomisation(player);
            TblCust.ShowDialog();
            this.Close();
        }  

        private void LeaderboardButton_Click(object sender, EventArgs e)
        {
            //Switch windows to the table customisation table
            //hide this window, instantiate new window, show new window, close current window
            this.Hide();
            Form LB = new Leaderboard(player);
            LB.ShowDialog();
            this.Close();
        }

        private void Stats_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form SW = new Stats(player);
            SW.ShowDialog();
            this.Close();
        }
   
        //-----------------------------------
    }
}
