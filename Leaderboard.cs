using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace NEA_Computer_Science_Poker__️__️__️__️
{
    public partial class Leaderboard : Form
    {
        Player player;
        private const string EXAMPLEDB = "PokerDatabase.mdb";
        private const string CONNECTION_STRING = @"Provider=Microsoft Jet 4.0 OLE DB Provider;Data Source = " + EXAMPLEDB + ";";

        public List<Player> PlayersList = new List<Player>(); //used to store unsorted players
        public List<Player> MoneyList = new List<Player>(); //used to store player objects sorted by their bank money
        public List<Player> HandsList = new List<Player>(); //used to store player objects sorted by their hands won
        public List<Player> PotList = new List<Player>(); //used to store player objects sorted by their largest pot won

        public List<Label> MoneyLabelList = new List<Label>();
        public List<Label> HandsLabelList = new List<Label>();
        public List<Label> PotLabelList = new List<Label>();

        public Leaderboard(Player player)
        {
            this.player = player;
            InitializeComponent();
        }


        //Load and setup subroutines
        private void Leaderboard_Load(object sender, EventArgs e)
        {
            FillLists();
            FetchAndAddData();
        } //startup of the leaderboard window


        private void FillLists()
        {
            //fill every list up with every node so that they can be referenced easily later

            MoneyLabelList.Add(Money1);
            MoneyLabelList.Add(Money2);
            MoneyLabelList.Add(Money3);
            MoneyLabelList.Add(Money4);
            MoneyLabelList.Add(Money5);
            MoneyLabelList.Add(Money6);
            MoneyLabelList.Add(Money7);
            MoneyLabelList.Add(Money8);
            MoneyLabelList.Add(Money9);
            MoneyLabelList.Add(Money10);

            HandsLabelList.Add(HandsWon1);
            HandsLabelList.Add(HandsWon2);
            HandsLabelList.Add(HandsWon3);
            HandsLabelList.Add(HandsWon4);
            HandsLabelList.Add(HandsWon5);
            HandsLabelList.Add(HandsWon6);
            HandsLabelList.Add(HandsWon7);
            HandsLabelList.Add(HandsWon8);
            HandsLabelList.Add(HandsWon9);
            HandsLabelList.Add(HandsWon10);

            PotLabelList.Add(Largest1);
            PotLabelList.Add(Largest2);
            PotLabelList.Add(Largest3);
            PotLabelList.Add(Largest4);
            PotLabelList.Add(Largest5);
            PotLabelList.Add(Largest6);
            PotLabelList.Add(Largest7);
            PotLabelList.Add(Largest8);
            PotLabelList.Add(Largest9);
            PotLabelList.Add(Largest10);


        } 

        //-----------------------------------

        //Data management subroutines

        private void FetchAndAddData()
        {
            //subroutine to obtain all data from the PlayerData table and place it into lists
            PlayersList.Add(player);
            string SQLString = "SELECT * FROM PlayerData ";
            OleDbConnection conn = new OleDbConnection(CONNECTION_STRING);
            OleDbCommand cmd = new OleDbCommand(SQLString, conn);
            conn.Open();
            OleDbDataReader DataReader = cmd.ExecuteReader(); 
            try
            {
                while (DataReader.Read())
                {
                    //set variables to data in every column, then create a new player structure using those values and add it to the list
                    string username = Convert.ToString(DataReader["Username"]);
                    if(username != player.GetUsername())
                    {
                        double money = Convert.ToDouble(DataReader["BankMoney"]);
                        int handswon = Convert.ToInt32(DataReader["HandsWon"]);
                        double largestpot = Convert.ToDouble(DataReader["LargestPot"]);
                        int handsplayed = Convert.ToInt32(DataReader["HandsPlayed"]);
                        Player newPlayer = new Player(username, money, handswon, largestpot, handsplayed, 0);
                        PlayersList.Add(newPlayer);
                    }
                }
                //sort the list based on every factor (money, hands won and largest pot won)
                MergeSortLists(); 
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            conn.Close();

        }


        private void MergeSortLists()
        {
            //create 3 new arrays containing every new player created based on the data in the database and then sort them using a 3 different merge sort algorithms that sort based on the 3 different factors
            Player[] MoneyArray = new Player[PlayersList.Count];
            Player[] HandsArray = new Player[PlayersList.Count];
            Player[] PotArray = new Player[PlayersList.Count];

            for (int i = 0; i < PlayersList.Count; i++)
            {
                MoneyArray[i] = PlayersList[i];
                HandsArray[i] = PlayersList[i];
                PotArray[i] = PlayersList[i];
            }

            MoneyArray = SortArray(MoneyArray, 0, MoneyArray.Length - 1, "Money");
            HandsArray = SortArray(HandsArray, 0, MoneyArray.Length - 1, "Hands");
            PotArray = SortArray(PotArray, 0, MoneyArray.Length - 1, "Largest");

            //display data
            OutputInfo(MoneyArray, HandsArray, PotArray);
        }

        
        private Player[] SortArray(Player[] array, int LeftPointer, int RightPointer, string type)
        {
            if (LeftPointer < RightPointer) //while the front and back pointers haven't met yet
            {
                int Middle = LeftPointer + (RightPointer - LeftPointer) / 2; //find the midpoint between the two pointers
                SortArray(array, LeftPointer, Middle, type); //recall this function with updated values to further separate the bottom half of the list up
                SortArray(array, Middle + 1, RightPointer, type); //recall this function with updated values to separate top half of the list
                if(type == "Money")
                {
                    MoneyMergeArray(array, LeftPointer, Middle, RightPointer); //merge sort while comparing the money value
                }
                else if(type == "Hands")
                {
                    HandsMergeArray(array, LeftPointer, Middle, RightPointer); //merge sort while comparing the hands won value
                }
                else if(type == "Largest")
                {
                    PotMergeArray(array, LeftPointer, Middle, RightPointer); //merge sort while comparing the largest pot won value
                }
                
            }
            return array;
        }

        public void MoneyMergeArray(Player[] array, int LeftPointer, int MiddlePointer, int RightPointer)
        {
            int LeftArrayLength = MiddlePointer - LeftPointer + 1; //define lengths of temporary arrays
            int RightArrayLength = RightPointer - MiddlePointer;

            Player[] LeftArray = new Player[LeftArrayLength]; //create empty temporary arrays
            Player[] RightArray = new Player[RightArrayLength];

            //fill both temporary arrays with the sections of the actual array they represent
            for (int i = 0; i < LeftArrayLength; ++i) //++i pre-increments the value of i, which prevents index-out of bounds error
            {
                LeftArray[i] = array[LeftPointer + i]; 
            }
                
            for (int j = 0; j < RightArrayLength; ++j)
            {
                RightArray[j] = array[MiddlePointer + 1 + j];
            }
                
            int k = 0;
            int l = 0;

            int m = LeftPointer; //set k to the value of the left-most pointer


            while (k < LeftArrayLength && l < RightArrayLength) //while the two cycle integers are less than their respective array lengths- sort the two halves. So when one runs over,
                                                                //this means that all the highest elements are already added into the actual array, so we can move on
            {
                if (LeftArray[k].GetMoney() >= RightArray[l].GetMoney()) //compare the two respective player objects' money values
                {
                    array[m] = LeftArray[k]; //add highest to the actual array
                    k += 1;
                }
                else
                {
                    array[m] = RightArray[l];
                    l += 1;
                }
                m += 1;
            }


            for(int a = k; a < LeftArrayLength; a++) //this represents the 'leftover' values, the lowest values that therefore are added at the back of the array
            {
                array[m] = LeftArray[a];
                m += 1;
            }
            for (int b = l; b < RightArrayLength; b++)
            {
                array[m] = RightArray[b];
                m += 1;
            }
        }

        public void HandsMergeArray(Player[] array, int LeftPointer, int MiddlePointer, int RightPointer)
        {
            int LeftArrayLength = MiddlePointer - LeftPointer + 1; //define lengths of temporary arrays
            int RightArrayLength = RightPointer - MiddlePointer;

            Player[] LeftArray = new Player[LeftArrayLength]; //create empty temporary arrays
            Player[] RightArray = new Player[RightArrayLength];

            //fill both temporary arrays with the sections of the actual array they represent
            for (int i = 0; i < LeftArrayLength; ++i) //++i pre-increments the value of i, which prevents index-out of bounds error
            {
                LeftArray[i] = array[LeftPointer + i];
            }

            for (int j = 0; j < RightArrayLength; ++j)
            {
                RightArray[j] = array[MiddlePointer + 1 + j];
            }

            int k = 0;
            int l = 0;

            int m = LeftPointer; //set k to the value of the left-most pointer


            while (k < LeftArrayLength && l < RightArrayLength) //while the two cycle integers are less than their respective array lengths- sort the two halves. So when one runs over,
                                                                //this means that all the highest elements are already added into the actual array, so we can move on
            {
                if (LeftArray[k].GetHandsWon() >= RightArray[l].GetHandsWon()) //compare the two respective player objects' money values
                {
                    array[m] = LeftArray[k]; //add highest to the actual array
                    k += 1;
                }
                else
                {
                    array[m] = RightArray[l];
                    l += 1;
                }
                m += 1;
            }


            for (int a = k; a < LeftArrayLength; a++) //this represents the 'leftover' values, the lowest values that therefore are added at the back of the array
            {
                array[m] = LeftArray[a];
                m += 1;
            }
            for (int b = l; b < RightArrayLength; b++)
            {
                array[m] = RightArray[b];
                m += 1;
            }
        }

        public void PotMergeArray(Player[] array, int LeftPointer, int MiddlePointer, int RightPointer)
        {
            int LeftArrayLength = MiddlePointer - LeftPointer + 1; //define lengths of temporary arrays
            int RightArrayLength = RightPointer - MiddlePointer;

            Player[] LeftArray = new Player[LeftArrayLength]; //create empty temporary arrays
            Player[] RightArray = new Player[RightArrayLength];

            //fill both temporary arrays with the sections of the actual array they represent
            for (int i = 0; i < LeftArrayLength; ++i) //++i pre-increments the value of i, which prevents index-out of bounds error
            {
                LeftArray[i] = array[LeftPointer + i];
            }

            for (int j = 0; j < RightArrayLength; ++j)
            {
                RightArray[j] = array[MiddlePointer + 1 + j];
            }

            int k = 0;
            int l = 0;

            int m = LeftPointer; //set k to the value of the left-most pointer


            while (k < LeftArrayLength && l < RightArrayLength) //while the two cycle integers are less than their respective array lengths- sort the two halves. So when one runs over,
                                                                //this means that all the highest elements are already added into the actual array, so we can move on
            {
                if (LeftArray[k].GetLargestPotWon() >= RightArray[l].GetLargestPotWon()) //compare the two respective player objects' money values
                {
                    array[m] = LeftArray[k]; //add highest to the actual array
                    k += 1;
                }
                else
                {
                    array[m] = RightArray[l];
                    l += 1;
                }
                m += 1;
            }


            for (int a = k; a < LeftArrayLength; a++) //this represents the 'leftover' values, the lowest values that therefore are added at the back of the array
            {
                array[m] = LeftArray[a];
                m += 1;
            }
            for (int b = l; b < RightArrayLength; b++)
            {
                array[m] = RightArray[b];
                m += 1;
            }
        }

        //---------------------------------------

        //Data display subroutines

        private void OutputInfo(Player[] MoneyArray, Player[] HandsArray, Player[] PotArray)
        {
            MoneyYoursLabel.Text = "YOU: " + player.GetUsername() + " - $" + player.GetMoney();
            HandsYoursLabel.Text = "YOU: " + player.GetUsername() + " - " + player.GetHandsWon();
            LargestYoursLabel.Text = "YOU: " + player.GetUsername() + " - $" + player.GetLargestPotWon();

            for (int i = 0; i < 10; i++)
            {
                try
                {
                    MoneyLabelList[i].Text = Convert.ToString(i + 1) + "- " + MoneyArray[i].GetUsername() + ": $" + Convert.ToString(MoneyArray[i].GetMoney());
                    HandsLabelList[i].Text = Convert.ToString(i + 1) + "- " + HandsArray[i].GetUsername() + ": " + Convert.ToString(HandsArray[i].GetHandsWon());
                    PotLabelList[i].Text = Convert.ToString(i + 1) + "- " + PotArray[i].GetUsername() + ": $" + Convert.ToString(PotArray[i].GetLargestPotWon());
                }
                catch
                {
                    MoneyLabelList[i].Text = "";
                    HandsLabelList[i].Text = "";
                    PotLabelList[i].Text = "";
                }
            }
        }

        //---------------------------------

        //Button subroutines

        private void BackButton_Click(object sender, EventArgs e)
        {
            //Switch windows to the table customisation table
            //hide this window, instantiate new window, show new window, close current window
            this.Hide();
            Form MM = new MainMenu(player);
            MM.ShowDialog();
            this.Close();
        }
           
        //----------------------------------
    
    }
}
