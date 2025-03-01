using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;
using ADOX;
using System.Windows.Forms;
using System.Data.SqlTypes;
using System.Security.Cryptography;

namespace NEA_Computer_Science_Poker__️__️__️__️
{
    public static class DatabaseUtils
    {
        private const string EXAMPLEDB = "PokerDatabase.mdb"; //filename
        private const string CONNECTION_STRING = @"Provider=Microsoft Jet 4.0 OLE DB Provider;Data Source = " + EXAMPLEDB + ";"; //connection string to give visual studio link to microsoft access


        //Query Subroutines
        public static string SqlQuery(string SqlString)
        {
            //Console.WriteLine(SqlString); ;
            OleDbConnection conn = new OleDbConnection(CONNECTION_STRING);
            conn.Open();
            OleDbCommand comm = new OleDbCommand(SqlString, conn);
            string data = Convert.ToString(comm.ExecuteScalar());
            conn.Close();
            return data;
        } //returns a result from the database after passing an SQL string into it

        public static bool ExecuteSqlNonQuery(String SqlString) //function to edit table / update contents- Non query means returns nothing
        {
            bool outcome = true;
            try
            {
                OleDbConnection cnn = new OleDbConnection(CONNECTION_STRING); //create connection between visual studio and the database
                cnn.Open();
                OleDbCommand cmd = new OleDbCommand(SqlString, cnn); //pass in SQL command given in parameters
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch
            {
                //MessageBox.Show(ex.Message, "Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                outcome = false;
            }
            return outcome;
        }

        //--------------------------------------------------------------------------------------

        //Database Creation Subroutines

        public static void CreateDatabase()
        {
            try
            {
                if (!File.Exists(EXAMPLEDB)) //if the database doesn't already exist
                {
                    CatalogClass cat = new CatalogClass();
                    cat.Create(CONNECTION_STRING); //creates new database using connection string with filename defined earlier
                    

                    CreateTables(); //use SQL to create tables for the database

                    MessageBox.Show("Database Created Successfully", "Database", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    cat = null;
                }
                else
                {
                    //MessageBox.Show("Database Already Exists", "Database", MessageBoxButtons.OK, MessageBoxIcon.Information); //database already exists, no need to create it
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //error
            }
        }   //create database itself

        private static void CreateTables() //create tables for database
        {
            string SQLString;

            SQLString = "CREATE TABLE PlayerData(" //creates PlayerData table with a primary key username, a password, money, handswon and largestpotwon fields
                + "Username VARCHAR(30) NOT NULL,"
                + "UserPassword VARCHAR(30) NOT NULL,"
                + "BankMoney FLOAT NOT NULL,"
                + "HandsWon INTEGER NOT NULL,"
                + "LargestPot FLOAT NOT NULL,"
                + "HandsPlayed INTEGER NOT NULL,"
                + "TotalLogins INTEGER NOT NULL,"
                + "HashingKey VARCHAR(10) NOT NULL,"
                + "PRIMARY KEY(Username)"
                + ")";
            ExecuteSqlNonQuery(SQLString);
            SQLString = "CREATE TABLE HandData(" //creates HandData table that will be used to store data from each hand- including primary key gameid, communitycards, and which player wins
                + "GameID INTEGER NOT NULL,"
                + "CommCards VARCHAR(50),"
                + "WinnerUsername VARCHAR(30) NOT NULL,"
                + "MethodWon VARCHAR(20),"
                + "PRIMARY KEY(GameID)"
                + ")";
            ExecuteSqlNonQuery(SQLString);
            SQLString = "CREATE TABLE HandPlayerData(" //creates link table with 2 primary foreign keys to store the cards, bets and win amounts for each player in each hand
                + "Username VARCHAR(30) NOT NULL,"
                + "GameID INTEGER NOT NULL,"
                + "Cards VARCHAR(50) NOT NULL,"
                + "BetTotal FLOAT,"
                + "AmountWon FLOAT,"
                + "HandRank VARCHAR(30),"
                + "FOREIGN KEY (Username) REFERENCES PlayerData(Username),"
                + "FOREIGN KEY (GameID) REFERENCES HandData(GameID),"
                + "PRIMARY KEY (Username, GameID)"
                + ")";
            ExecuteSqlNonQuery(SQLString);
            SQLString = "CREATE TABLE LoginTracker(" //creates a login tracker table with a composite key made of a local primary key and a foreign key, used to keep track of data every login
                + "Username VARCHAR(30) NOT NULL,"
                + "LoginNumber INTEGER NOT NULL,"
                + "BankMoney FLOAT NOT NULL,"
                + "LargestPot FLOAT NOT NULL,"
                + "HandsWon INTEGER NOT NULL,"
                + "HandsPlayed INTEGER NOT NULL,"
                + "FOREIGN KEY (Username) REFERENCES PlayerData(Username),"
                + "PRIMARY KEY (Username, LoginNumber)"
                + ")";
            ExecuteSqlNonQuery(SQLString);
            SQLString = "CREATE TABLE HandRankData(" //creates a hand rank data table that has a column for every different hand rank which is increased every time the player encounters it
                + "Username VARCHAR(30) NOT NULL,"
                + "HighCard INTEGER NOT NULL,"
                + "Pair INTEGER NOT NULL,"
                + "TwoPair INTEGER NOT NULL,"
                + "ThreeOfAKind INTEGER NOT NULL,"
                + "Straight INTEGER NOT NULL,"
                + "Flush INTEGER NOT NULL,"
                + "FullHouse INTEGER NOT NULL,"
                + "FourOfAKind INTEGER NOT NULL,"
                + "StraightFlush INTEGER NOT NULL,"
                + "RoyalFlush INTEGER NOT NULL,"
                + "FOREIGN KEY (Username) REFERENCES PlayerData(Username),"
                + "PRIMARY KEY (Username)"
                + ")";
            ExecuteSqlNonQuery(SQLString);

            //randomly populate player data table for testing purposes
            PopulateTablesRandom();
        }

        public static void PopulateTablesRandom()
        {
            int cycle = 0;
            while (cycle < 50)
            {
                string[] Letters = { "a", "b", "c", "d", "e", "f", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
                Random rnd = new Random();
                string NewUsername = "";
                for (int i = 0; i < rnd.Next(15, 20); i++)
                {
                    NewUsername += Letters[rnd.Next(0, Letters.Length)];
                }
                int DefaultMoney = rnd.Next(1000, 50000);
                int DefaultHandsWon = rnd.Next(0, 100);
                int DefaultLargestPot = rnd.Next(0, 10000);
                int HandsPlayed = rnd.Next(0, 500);
                int TotalLogins = 0;
                string HashKey = "1";

                string SQLString = "INSERT INTO PlayerData "
                + "VALUES('" + NewUsername + "', '" + "Password" + "', '" + DefaultMoney + "', '" + DefaultHandsWon + "', '" + DefaultLargestPot + "', '" + HandsPlayed + "', '" + TotalLogins + "', '" + HashKey + "')"; //insert new default data to table

                bool outcome = ExecuteSqlNonQuery(SQLString);
                if (outcome)
                {
                    cycle += 1;
                }
                
            }

        }   //randomly populate the database's PlayerData table with 50 entries to allow the mean and sd calculations to happen

        //---------------------------------------------------------------------------------------
    }
    }

    

