using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace NEA_Computer_Science_Poker__️__️__️__️
{
    public partial class Login : Form
    {
        private const string EXAMPLEDB = "PokerDatabase.mdb";
        private const string CONNECTION_STRING = @"Provider=Microsoft Jet 4.0 OLE DB Provider;Data Source = " + EXAMPLEDB + ";"; //connection string 
        private string[] InvalidCharacters = { "!", "'", "£", "$", "%", "^", "&", "*", "(", ")", "+", "=", "-", "_", "{", "[", "}", "]", ":", ";", "@", "~", "#", ",", "<", ">", ".", "?", "/", "|" };
        
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            DatabaseUtils.CreateDatabase(); //calls static class to create the database

        } //startup of login window

        //Login and Account creation subroutines

        private void CreateAccountButton_Click(object sender, EventArgs e)
        {
            
            //add details to database
            string NewUsername = CreateUser.Text;
            string NewPassword = CreatePass.Text;

            //verify no invalid characters are present
            bool invalid = false;
            foreach(char letter in NewUsername)
            {
                for(int i = 0; i < InvalidCharacters.Length; i++)
                {
                    if (Convert.ToString(letter) == InvalidCharacters[i])
                    {
                        invalid = true;
                        break;
                    }
                }
                if (invalid)
                {
                    break;
                }
            }

            if(NewUsername != "" && NewPassword != "")
            {
                string CheckString;
                CheckString = DatabaseUtils.SqlQuery("SELECT UserPassword FROM PlayerData WHERE Username = '" + NewUsername + "'");

                if (CheckString.Length > 0)
                {
                    invalid = true;
                }
            }
            


            if (NewUsername != "" && NewPassword != "" && !invalid) //as long as username and password aren't empty or invalid
            {
                string SQLString;

                //INSERT DATA
                try //try catch statement will throw up an error if the username already exists in the table, meaning data validation is unnecessary for now
                {
                    //set variables to start value and then insert data into the database using these values
                    Random rnd = new Random();
                    int DefaultMoney = 10000;//rnd.Next(100000, 1000000);
                    int DefaultHandsWon = 0;//rnd.Next(0, 500);
                    int DefaultLargestPot = 0;//rnd.Next(0, 10000);
                    int HandsPlayed = 0;//rnd.Next(0, 100);
                    int TotalLogins = 0;

                    string[] HashDetails = HashPassword(NewPassword);
                    string HashedPassword = HashDetails[0];
                    string HashKey = HashDetails[1];
                    

                    SQLString = "INSERT INTO PlayerData "
                    + "VALUES('" + NewUsername + "', '" + HashedPassword + "', '" + DefaultMoney + "', '" + DefaultHandsWon + "', '" + DefaultLargestPot + "', '" + HandsPlayed + "', '" + TotalLogins + "', '" + HashKey + "')"; //insert new default data to table

                    DatabaseUtils.ExecuteSqlNonQuery(SQLString);


                    SQLString = "INSERT INTO HandRankData "
                    + "VALUES('" + NewUsername + "', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0')";

                    DatabaseUtils.ExecuteSqlNonQuery(SQLString);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid Username and/or Password", "Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            CreateUser.Text = ""; //reset the textboxes
            CreatePass.Text = "";
        }  //process that occurs when you press 'create account'- will add a new entry to the database with base information.

        private void LoginButton_Click(object sender, EventArgs e)
        {
            //check entered details with database
            string Username = LoginUser.Text;
            string Password = LoginPass.Text;

            



            //verify no invalid characters are present
            bool invalid = false;
            foreach (char letter in Username)
            {
                for (int i = 0; i < InvalidCharacters.Length; i++)
                {
                    if (Convert.ToString(letter) == InvalidCharacters[i])
                    {
                        invalid = true;
                        break;
                    }
                }
                if (invalid)
                {
                    break;
                }
            }

            try
            {
                if (Username != "" && Password != "" && !invalid) //as long as the username and passwords aren't empty of invalid
                {
                    //check password
                    string Pass = DatabaseUtils.SqlQuery("SELECT UserPassword FROM PlayerData WHERE Username = '" + Username + "'");


                    //set HashKey
                    string HashKey = Convert.ToString(DatabaseUtils.SqlQuery("SELECT HashingKey FROM PlayerData WHERE Username = '" + Username + "'"));

                    //change password from database back to unhashed value
                    Pass = UnHashPassword(Pass, HashKey);

                    if (Pass == Password) //if the password we fetched from the username primary key is the same as the password entered, we are good to go
                    {
                        //LOAD DATA


                        //set money
                        double Money = Convert.ToDouble(DatabaseUtils.SqlQuery("SELECT BankMoney FROM PlayerData WHERE Username = '" + Username + "'")); //executescalar returns first value that meets the criteria


                        //set handswon
                        int HandsWon = Convert.ToInt32(DatabaseUtils.SqlQuery("SELECT HandsWon FROM PlayerData WHERE Username = '" + Username + "'"));


                        //set largestpot
                        double LargestPotWon = Convert.ToDouble(DatabaseUtils.SqlQuery("SELECT LargestPot FROM PlayerData WHERE Username = '" + Username + "'"));


                        //set hands played
                        int HandsPlayed = Convert.ToInt32(DatabaseUtils.SqlQuery("SELECT HandsPlayed FROM PlayerData WHERE Username = '" + Username + "'"));


                        //set total logins
                        int TotalLogins = Convert.ToInt32(DatabaseUtils.SqlQuery("SELECT TotalLogins FROM PlayerData WHERE Username = '" + Username + "'"));



                        //Create new player class with all details
                        Player player = new Player(Username, Money, HandsWon, LargestPotWon, HandsPlayed, TotalLogins + 1);


                        string SQLString = "INSERT INTO LoginTracker "
                        + "VALUES('" + player.GetUsername() + "', '" + (player.GetTotalLogins() - 1) + "', '" + player.GetMoney() + "', '" + player.GetLargestPotWon() + "', '" + player.GetHandsWon() + "', '" + player.GetHandsPlayed() + "')"; //insert new default data to table

                        DatabaseUtils.ExecuteSqlNonQuery(SQLString);


                        MessageBox.Show("Username and Password are correct. Welcome, " + player.GetUsername() + "!" , "Database", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //Switch windows to the table customisation table
                        //hide this window, instantiate new window, show new window, close current window
                        this.Hide();
                        Form MM = new MainMenu(player);
                        MM.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        CreateUser.Text = "";
                        CreatePass.Text = "";
                        MessageBox.Show("Username and/or Password are incorrect", "Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }


                }
                else
                {
                    CreateUser.Text = "";
                    CreatePass.Text = "";
                    MessageBox.Show("Invalid Username and/or Password", "Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("Error: Invalid Login Data Entered", "Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        } //process that occurs when you press 'login'- will compare your details (username and password) against details in the database

        //---------------------------------------------------------------

        //Hashing and unhashing subroutines
        private string[] HashPassword(string Password)
        {
            string[] HashDetails = new string[2];
            //in the form [password, hashcode]

            Random rnd = new Random();
            int HashCode = rnd.Next(1, 6);
            HashDetails[1] = Convert.ToString(HashCode);

            //create a list with all characters
            List<string> Characters = new List<string>();


            //fetch all characters from the hashset textfile
            StreamReader SR = new StreamReader("HashSet" + Convert.ToString(HashCode) + ".txt", true);
            while (SR.Peek() != -1)
            {
                Characters.Add(SR.ReadLine());
            }
            SR.Close();


            string NewPassword = "";


            //for every character, shift it one down in the hashset, which should control-randomize the password
            foreach(char c in Password)
            {
                string Char = Convert.ToString(c);
                for(int i = 0; i < Characters.Count; i++)
                {
                    if (Characters[i] == Char)
                    {
                        try
                        {
                            Char = Characters[i - 1];
                        }
                        catch
                        {
                            Char = Characters[Characters.Count - 1];
                        }
                        break;
                    }
                }
                //don't need to check if we've shifted it or not, if the characters hasn't been shifted then it doesnt exist in the file
                //therefore when unhashing it won't be shifted either, so it can remain unchanged
                NewPassword += Char;
                
            }

            HashDetails[0] = NewPassword;

            return HashDetails;
        }   //scrambles your password before entering it into the database


        private string UnHashPassword(string Password, string HashKey)
        {
            string UnhashedPassword = "";

            //create a list with all characters
            List<string> Characters = new List<string>();


            //fetch all characters from corresponding hashset textfile
            StreamReader SR = new StreamReader("HashSet" + HashKey + ".txt", true);
            while (SR.Peek() != -1)
            {
                Characters.Add(SR.ReadLine());
            }
            SR.Close();

            //unshift each character to form the original password
            foreach (char c in Password)
            {
                string Char = Convert.ToString(c);
                for (int i = 0; i < Characters.Count; i++)
                {
                    if (Characters[i] == Char)
                    {
                        try
                        {
                            Char = Characters[i + 1];
                        }
                        catch
                        {
                            Char = Characters[0];
                        }
                        break;
                    }
                }

                UnhashedPassword += Char;

            }

            //Console.WriteLine(Password);
            //Console.WriteLine(UnhashedPassword);

            return UnhashedPassword;
        }   //unscrambles your password in the database before comparing it to the password you entered

        //---------------------------------------------------------------

    }
}

