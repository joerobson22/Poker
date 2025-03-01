using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Data.OleDb;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace NEA_Computer_Science_Poker__️__️__️__️
{
    public partial class Table : Form
    {
        //create empty objects of the player and a hand rank calcualtor
        Player player;
        public HandRankCalculator HandRankCalc = new HandRankCalculator();

        //database constants
        private const string EXAMPLEDB = "PokerDatabase.mdb";
        private const string CONNECTION_STRING = @"Provider=Microsoft Jet 4.0 OLE DB Provider;Data Source = " + EXAMPLEDB + ";";

        //empty lists used to store all ui nodes
        List<PictureBox> ImagesList = new List<PictureBox>();
        List<PictureBox> PictureBoxList = new List<PictureBox>();
        List<Label> NameLabelList = new List<Label>();
        List<Label> OpponentCardLabelList = new List<Label>();
        List<Label> MoneyLabelList = new List<Label>();
        List<Label> ColourLabelList = new List<Label>();
        List<Label> TableCardsLabelList = new List<Label>();
        List<Panel> TableCardsList = new List<Panel>();
        List<Panel> OpponentPanelList = new List<Panel>();
        List<Label> RankColourLabelList = new List<Label>();
        List<Label> StatusLabelList = new List<Label>();

        //empty lists used to store all opponents
        List<Opponent> OpponentList = new List<Opponent>();
        List<Opponent> OpponentsList = new List<Opponent>();

        //variables used for the deck and cards
        public int NumPlayers; //stores the number of players at the table
        public string[] SuitCharacters = new string[4]; //will store 4 different suit symbols
        public string[] NumberCharacters = new string[13]; //will store values like 2, 6, J, A...
        public string SuitCharactersFilename = "Table_SuitCharacters.txt"; //refers to name of text file in debug ->  bin folder
        public string NumberCharactersFilename = "Table_NumberCharacters.txt";// ^^

        //empty lists used for dealing
        public List<string> Deck = new List<string>(); //will be used to store all 3 letter card values as strings. Stores all 52 cards
        public List<string> ActualDeck = new List<string>(); //will be used to store all 3 letter card values required for this table.
        public List<string[]> Hands = new List<string[]>(); //stores a number of arrays that represent player's hands
        public List<string> PlayerVALIDCards = new List<string>();

        //random
        public Random rnd = new Random(); //random variable used across the table.

        //variables used to allow the game to work properly
        public int DeckProgress; //keeps track of how far into the deck we are.
        public double TotalPotVal; //money to win
        public double StayInMoney; //keeps track of how much total money everyone will have to have put in to remain in the hand
        public int Round; //the round the hand is on
        public int Cycle; //whose turn it is
        public double PlayerRaiseAmount; //how much the player is planning on raising
        public double BuyInAmount; //keeps track of how much money opponents and the player should enter the table with
        public int PlayersLeft; //indicates how many playes are left in the hand
        public bool MoveOn; //boolean variable used to tell the program if the hand can move to the next betting round or not.

        //testing variables
        public bool ScreenShot = false; //for testing, used to try and capture a royal flush while rapidly cycling through random hands
        public bool custom = false; //for testing purposes, this allows me to pass a custom deck into the game
        public int HandNum = -1; //also for testing purposes

        //hand rank words used to translate a number returned by the handrankcalculator into its associated hand rank
        public string[] HandRankWords = { "High Card", "Pair", "Two Pair", "Three Of A Kind", "Straight", "Flush", "Full House", "Four Of A Kind", "Straight Flush", "Royal Flush" };

        public Table(int NumPlayers, Player player, List<Opponent> opponentList, double buyinamount)
        {
            this.NumPlayers = NumPlayers; //number of players are passed through when instantiating this class, and the public variable is set to this passed value
            InitializeComponent();
            this.player = player;
            OpponentList = opponentList;
            BuyInAmount = buyinamount;
        }


        //SETUP SUBROUTINES

        private void Table_Load(object sender, EventArgs e)
        {
            

            //set up everything
            FillLists();

            SetUpOpponentLists();



            //change each image to the associated image, each name label to their associated name, and each difficulty label to the associated difficulty
            for (int i = 0; i < PictureBoxList.Count; i++)
            {
                PictureBoxList[i].Image = ImagesList[OpponentList[i].GetSpriteNumber()].Image;
                NameLabelList[i].Text = OpponentList[i].GetName();
            }
            player.SetGameMoney(0);

            LoadCardDetails(); //used to fetch all data from both text files and set them all up into the Table_Deck variable
            
            
            for(int i = 0; i < OpponentsList.Count; i++)
            {
                OpponentsList[i].SetMoney(BuyInAmount);
            }


            Reset(); //reset everything

            //SequenceOfEvents();
        }

        private void FillLists()
        {
            //add all different sprites to list
            ImagesList.Add(player1Looks);
            ImagesList.Add(player2Looks);
            ImagesList.Add(player3Looks);
            ImagesList.Add(player4Looks);
            ImagesList.Add(player5Looks);
            ImagesList.Add(player6Looks);
            ImagesList.Add(player7Looks);
            ImagesList.Add(player8Looks);
            ImagesList.Add(player9Looks);
            ImagesList.Add(player10Looks);

            //add all different pictureboxes to the list
            PictureBoxList.Add(Opponent1Image);
            PictureBoxList.Add(Opponent2Image);
            PictureBoxList.Add(Opponent3Image);
            PictureBoxList.Add(Opponent4Image);
            PictureBoxList.Add(Opponent5Image);
            PictureBoxList.Add(Opponent6Image);
            PictureBoxList.Add(Opponent7Image);

            //add all different name labels to the list
            NameLabelList.Add(NameLabel1);
            NameLabelList.Add(NameLabel2);
            NameLabelList.Add(NameLabel3);
            NameLabelList.Add(NameLabel4);
            NameLabelList.Add(NameLabel5);
            NameLabelList.Add(NameLabel6);
            NameLabelList.Add(NameLabel7);

            //add all different difficulty labels to the list
            OpponentCardLabelList.Add(CardsLabel1);
            OpponentCardLabelList.Add(CardsLabel2);
            OpponentCardLabelList.Add(CardsLabel3);
            OpponentCardLabelList.Add(CardsLabel4);
            OpponentCardLabelList.Add(CardsLabel5);
            OpponentCardLabelList.Add(CardsLabel6);
            OpponentCardLabelList.Add(CardsLabel7);

            //add all different money labels to the list
            MoneyLabelList.Add(MoneyLabel1);
            MoneyLabelList.Add(MoneyLabel2);
            MoneyLabelList.Add(MoneyLabel3);
            MoneyLabelList.Add(MoneyLabel4);
            MoneyLabelList.Add(MoneyLabel5);
            MoneyLabelList.Add(MoneyLabel6);
            MoneyLabelList.Add(MoneyLabel7);

            //add the 4 different colours to the label colour list
            ColourLabelList.Add(LabelColourBlack); //spades
            ColourLabelList.Add(LabelColourRed);   //hearts
            ColourLabelList.Add(LabelColourBlue);  //diamonds
            ColourLabelList.Add(LabelColourGreen); //clubs

            //add all of the table cards to the list
            TableCardsList.Add(Table_CardBack1);
            TableCardsList.Add(Table_CardBack2);
            TableCardsList.Add(Table_CardBack3);
            TableCardsList.Add(Table_CardBack4);
            TableCardsList.Add(Table_CardBack5);

            //add all of the table card labels to the list
            TableCardsLabelList.Add(Table_Card1);
            TableCardsLabelList.Add(Table_Card2);
            TableCardsLabelList.Add(Table_Card3);
            TableCardsLabelList.Add(Table_Card4);
            TableCardsLabelList.Add(Table_Card5);

            //add all hand rank label colours
            RankColourLabelList.Add(HighCard);
            RankColourLabelList.Add(Pair);
            RankColourLabelList.Add(TwoPair);
            RankColourLabelList.Add(Three);
            RankColourLabelList.Add(Straight);
            RankColourLabelList.Add(Flush);
            RankColourLabelList.Add(FullHouse);
            RankColourLabelList.Add(Four);
            RankColourLabelList.Add(StraightFlush);
            RankColourLabelList.Add(RoyalFlush);

            //add all status labels to the list
            StatusLabelList.Add(StatusLabel1);
            StatusLabelList.Add(StatusLabel2);
            StatusLabelList.Add(StatusLabel3);
            StatusLabelList.Add(StatusLabel4);
            StatusLabelList.Add(StatusLabel5);
            StatusLabelList.Add(StatusLabel6);
            StatusLabelList.Add(StatusLabel7);

            //add all opponent panels to the list
            OpponentPanelList.Add(OpponentPanel1);
            OpponentPanelList.Add(OpponentPanel2);
            OpponentPanelList.Add(OpponentPanel3);
            OpponentPanelList.Add(OpponentPanel4);
            OpponentPanelList.Add(OpponentPanel5);
            OpponentPanelList.Add(OpponentPanel6);
            OpponentPanelList.Add(OpponentPanel7);
        }

        private void SetUpOpponentLists()
        {
            //create
            for(int i = 0; i < OpponentList.Count; i++)
            {
                string name = OpponentList[i].GetName();
                double money = OpponentList[i].GetMoney();
                int sprite = OpponentList[i].GetSpriteNumber();
                if (OpponentList[i].GetDifficulty() == 0)
                {
                    Opponent.EasyOpponent NewEasy = new Opponent.EasyOpponent(name, money, 0, sprite, i);
                    OpponentsList.Add(NewEasy);
                }
                else if (OpponentList[i].GetDifficulty() == 1)
                {
                    Opponent.MediumOpponent NewMed = new Opponent.MediumOpponent(name, money, 1, sprite, i);
                    OpponentsList.Add(NewMed);
                }
                else if (OpponentList[i].GetDifficulty() == 2)
                {
                    Opponent.HardOpponent NewHard = new Opponent.HardOpponent(name, money, 2, sprite, i);
                    OpponentsList.Add(NewHard);
                }
               
            }
        }


        private void Reset()
        {
            //Reset EVERYTHING

            ProceedButton.Visible = false;

            //if the player is completely out of money, they can't play so the window is closed
            if (player.GetGameMoney() <= 0.0 && player.GetMoney() <= 0.0)
            {
                SavePlayerData();
                this.Hide();
                Form TblCust = new TableCustomisation(player);
                TblCust.ShowDialog();
                this.Close();
            }
            else
            {
                //re buy-in the player if their money is less than the buy in amount
                if (player.GetGameMoney() <= BuyInAmount)
                {
                    player.BuyIn(BuyInAmount);
                }

                //reset all player statuses (round money, hand money, if they're all in, the amount of money in the pot when they went all in, how muhc they've won this round, and if they've folded
                player.SetRoundMoney(0);
                player.SetHandMoney(0);
                player.SetTotalWonThisRound(0);
                player.SetAllIn(false);
                player.SetTotalWonThisRound(0);
                player.ModifyFolded(false);

                //reset all labels
                GameMoneyLabel.Text = "$" + player.GetGameMoney();
                BankMoneyLabel.Text = "$" + player.GetMoney();
                StatusLabel.Text = "";

                //reset everything about the opponents
                for (int i = 0; i < OpponentsList.Count; i++)
                {
                    OpponentsList[i].SetAllIn(false);
                    OpponentsList[i].SetTotalWonThisRound(0);
                    MoneyLabelList[i].Text = "$" + OpponentsList[i].GetMoney();
                    OpponentsList[i].ModifyRoundMoney(0);
                    OpponentsList[i].SetHandMoney(0);
                    StatusLabelList[i].Text = "";

                    //if the opponents have less than $20, buy them in again to the buy in amount
                    if (OpponentsList[i].GetMoney() > 20)
                    {
                        OpponentPanelList[i].Visible = true;
                        OpponentsList[i].ModifyFolded(false);
                        OpponentCardLabelList[i].Text = "";
                    }
                    else
                    {
                        OpponentsList[i].SetMoney(BuyInAmount);
                        OpponentPanelList[i].Visible = true;
                        OpponentsList[i].ModifyFolded(false);
                        OpponentCardLabelList[i].Text = "";
                    }
                    StatusLabelList[i].Text = "";
                }

                PlayersLeft = NumPlayers;

                Round = 0;
                TotalPotVal = 0;
                StayInMoney = 0;

                TotalPotAmountLabel.Text = "$0";
                CallAmountLabel.Text = "$0";

                //reset all buttons and labels around the table
                UpdateButtonStatus();
                UpdateMoneyLabels();

                ActualDeck.Clear();
                Hands.Clear();

                ShuffleDeck();
                UpdateTableCardsLabels();
                UpdateTableCardsVisibility();
                SavePlayerData();
            }

            
        }

        //---------------------------------------------------------------------------------------------------


        //DATABASE SUBROUTINES
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
        }


        private void SaveHandData(string WinnerUsername, string MethodWon)
        {
            //save the data from this hand
            string SQLString;
            int GameID = 0;
            string CommCards = "";


            //obtain the highest GameID so that it can increment it correctly
            SQLString = "SELECT * FROM HandData";
            OleDbConnection conn = new OleDbConnection(CONNECTION_STRING);
            OleDbCommand cmd = new OleDbCommand(SQLString, conn);
            conn.Open();
            OleDbDataReader DataReader = cmd.ExecuteReader();
            try
            {
                while (DataReader.Read()) //while there are still rows to be read from, continue
                {
                    GameID = Convert.ToInt32(DataReader["GameID"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            conn.Close();

            GameID += 1;

            //create a string that will hold the data for each community card
            int cycle = 0;
            while (cycle < 5)
            {
                CommCards += ActualDeck[(NumPlayers * 2) + cycle];
                if(cycle < 4)
                {
                    CommCards += ", ";
                }
                cycle += 1;
            }


            SQLString = "INSERT INTO HandData "
                    + "VALUES('" + GameID + "', '" + CommCards + "', '" + WinnerUsername + "', '" + MethodWon + "')"; //insert new default data to table


            DatabaseUtils.ExecuteSqlNonQuery(SQLString);


            //------------------------------------------
            //Add all data for the hand in relation to how the player played it to the HandRankData

            string[] HandRankFieldNames = { "HighCard", "Pair", "TwoPair", "ThreeOfAKind", "Straight", "Flush", "FullHouse", "FourOfAKind", "StraightFlush", "RoyalFlush" };

            string Username = player.GetUsername();
            string Cards = "";
            double BetTotal = player.GetHandMoney();
            double WinTotal = player.GetTotalWonThisRound();
            string HandRank = HandRankFieldNames[player.GetHandRank() - 1];


            for(int i = 0; i < PlayerVALIDCards.Count; i++)
            {
                Cards += PlayerVALIDCards[i];
                if(i < PlayerVALIDCards.Count - 1)
                {
                    Cards += ", ";
                }
            }


            SQLString = "INSERT INTO HandPlayerData "
                + "VALUES('" + Username + "', '" + Convert.ToString(GameID) + "', '" + Cards + "', '" + Convert.ToString(BetTotal) + "', '" + Convert.ToString(WinTotal) + "', '" + HandRank + "')";

            DatabaseUtils.ExecuteSqlNonQuery(SQLString);

        }



        //---------------------------------------------------------------------------------------------------

        //DEALING SUBROUTINES

        private void LoadCardDetails()
        {
            int SuitCycle = 0;
            while (SuitCycle < 4) //repeats 4 times total, once for every suit
            {
                int NumberCycle = 0;
                string[] NumberCycleString = { "00", "01", "02", "03", "04", "05","06","07","08","09","10","11","12","13" };
                //^^ ensures all values are 2 digits long for consistency when calling them later on
                while (NumberCycle < 13) //repeats 13 times total, once for every number valkue for each suit
                {
                    Deck.Add(NumberCycleString[NumberCycle] + Convert.ToString(SuitCycle));
                    //^^ adds concantiated string to Table_Deck of actual values to use when accessing the arrays
                    NumberCycle += 1;
                }
                SuitCycle += 1;
            }

            StreamReader Table_SCSR = new StreamReader(SuitCharactersFilename, true); //reads data off the suit characters external file to access all symbols
            int Table_cyclenum = 0;
            while (Table_SCSR.Peek() != -1) //while the line looking at is not empty/null
            {
                SuitCharacters[Table_cyclenum] = Table_SCSR.ReadLine(); //adds to array
                Table_cyclenum += 1;
            }
            Table_SCSR.Close();


            StreamReader Table_NCSR = new StreamReader(NumberCharactersFilename, true);
            //^^reads data off the number characters external file to access all number and letter values
            Table_cyclenum = 0;
            while (Table_NCSR.Peek() != -1)
            {
                NumberCharacters[Table_cyclenum] = Table_NCSR.ReadLine(); //adds to array
                Table_cyclenum += 1;
            }
            Table_NCSR.Close();
        }

        private void ShuffleDeck()
        {
            if (custom && HandNum < 22)
            {
                HandNum += 1;
                CustomDeal(HandNum);
                DealHands();
                
            }
            else
            {
                int CardsNeeded = (NumPlayers * 2) + 5;
                //^^ calculates the number of cards needed for the round, as each player has 2 cards, and there are 5 community cards
                int Cycle = 0;

                while (Cycle < CardsNeeded)//loops through all cards required for the hand
                {
                    string NewCard = Deck[rnd.Next(0, Deck.Count)];
                    //^^randomises an integer, calls the item from the Table_Deck variable using this random integer, then sets the NewCard variable to this
                    if (!ActualDeck.Contains(NewCard)) //if the list we are passing this string into already contains this new card then we generate another
                    {
                        ActualDeck.Add(NewCard); //adds new card to Table_ActualDeck
                        Cycle += 1;
                    }

                }
                DealHands(); //when all is done, hands are dealt
            }
            
        }

        private void DealHands()
        {
            int Cycle = 0;
            while(Cycle < (NumPlayers * 2))//as every player has 2 cards in their hand, we have to deal a number of cards equal to players * 2
            {
                string[] Hand = new string[2]; //new array for a hand
                for(int i = 0; i < 2; i++) //for loop is used to loop twice through the hand, before then moving onto the next hand
                {
                    Hand[i] = ActualDeck[Cycle];
                    Cycle += 1;
                }
                Hands.Add(Hand); //hand is added to Table_Hands list to be referred to later
               
            }
            DeckProgress = Cycle;
            DisplayPlayerHandCards(); //the cards are then displayed via the labels on the window
            UpdateTableCardsLabels();
            SequenceOfEvents();
        }

        
        //---------------------------------------------------------------------------------------------------

        

        
        //CALCULATE SUBROUTINES

        private void CalculateHandRanks(int NumCommunityCards)
        {
            PlayerVALIDCards.Clear();
            List<string> Cards = new List<string>();


            //work out player hand rank
            Cards.Add(Hands[0][0]);
            Cards.Add(Hands[0][1]);
            int cycle = 0;
            while(cycle < NumCommunityCards)
            {
                Cards.Add(ActualDeck[(NumPlayers * 2) + cycle]);
                cycle += 1;
            }
            int num = HandRankCalc.CalculateHandRank(Cards);
            player.ModifyHandRank(num);
            foreach(string card in HandRankCalc.GetVALID())
            {
                PlayerVALIDCards.Add(card);
            }

            //work out all opponent hand ranks. as each opponent is of a different class, they are held in different lists
            // loop through all lists to assign hand ranks to each opponent

            for (int i = 0; i < OpponentsList.Count; i++)
            {
                Cards.Clear();
                int OppNumber = OpponentsList[i].GetOpponentNumber();
                //Console.WriteLine(OppNumber + 1);
                Cards.Add(Hands[OppNumber + 1][0]);
                Cards.Add(Hands[OppNumber + 1][1]);
                cycle = 0;
                while (cycle < NumCommunityCards)
                {
                    Cards.Add(ActualDeck[(NumPlayers * 2) + cycle]);
                    cycle += 1;
                }
                num = HandRankCalc.CalculateHandRank(Cards);
                OpponentsList[i].ModifyCards(HandRankCalc.GetVALID());
                OpponentsList[i].SetHandRank(num);
            }




        }


        //--------------------------------------------------------------------------------------------------

        //SORT SUBROUTINES

        private Opponent[] SortArray(Opponent[] array, int LeftPointer, int RightPointer)
        {
            if (LeftPointer < RightPointer) //while the front and back pointers haven't met yet
            {
                int middle = LeftPointer + (RightPointer - LeftPointer) / 2; //find the midpoint between the two pointers
                SortArray(array, LeftPointer, middle); //recall this function with updated values to further separate the bottom half of the list up
                SortArray(array, middle + 1, RightPointer); //recall this function with updated values to separate top half of the list
                MergeArray(array, LeftPointer, middle, RightPointer);
            }
            return array;
        }

        public void MergeArray(Opponent[] array, int LeftPointer, int MiddlePointer, int RightPointer)
        {
            int LeftArrayLength = MiddlePointer - LeftPointer + 1; //define lengths of temporary arrays
            int RightArrayLength = RightPointer - MiddlePointer;

            Opponent[] LeftArray = new Opponent[LeftArrayLength]; //create empty temporary arrays
            Opponent[] RightArray = new Opponent[RightArrayLength];

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
                if (LeftArray[k].GetHandRank() >= RightArray[l].GetHandRank()) //compare the two respective player objects' money values
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

        //---------------------------------------------------------------------------------------------------

        //GAME SUBROUTINES

        private void SequenceOfEvents()
        {
            //controls the main game loop

            if(PlayersLeft > 1)
            {
                //reset everything
                player.SetRoundMoney(0);
                for (int i = 0; i < OpponentsList.Count; i++)
                {
                    OpponentsList[i].SetRoundMoney(0);
                    MoneyLabelList[i].Text = "$" + OpponentsList[i].GetMoney();
                    if (OpponentsList[i].GetFolded())
                    {
                        OpponentPanelList[i].Visible = false;
                    }
                }

                PlayerRaiseAmount = 0;
                RaiseAmountLabel.Text = "$0";
                Cycle = -1;
                StayInMoney = 0;

                UpdateTableCardsVisibility();

                if (Round == 0)
                {
                    //calculate hand ranks, update the hand rank information, cycle through players for betting
                    CalculateHandRanks(0);
                    UpdatePlayerHandRankInformation();
                    //big and small blinds
                    CyclePlayers();
                }
                else if (Round == 1)
                {
                    //calculate hand ranks, update the hand rank information, cycle through players for betting
                    CalculateHandRanks(3);
                    UpdatePlayerHandRankInformation();
                    CyclePlayers();
                }
                else if (Round == 2)
                {
                    //calculate hand ranks, update the hand rank information, cycle through players for betting
                    CalculateHandRanks(4);
                    UpdatePlayerHandRankInformation();
                    CyclePlayers();
                }
                else if (Round == 3)
                {
                    //calculate hand ranks, update the hand rank information, cycle through players for betting
                    CalculateHandRanks(5);
                    UpdatePlayerHandRankInformation();
                    CyclePlayers();
                }
                else if (Round == 4) //CALCULATE WINNERS
                {
                    //calculate winners w/ sidepots etc
                    FinaliseHand();
                }
            }
            else
            {
                //there is only 1 player left so give them all the money
                for(int i = 0; i < OpponentsList.Count; i++)
                {
                    if (!OpponentsList[i].GetFolded())
                    {
                        OpponentsList[i].ModifyMoney(TotalPotVal);
                        StatusLabelList[i].Text = "WINNER";
                        break;
                    }
                }

                if (!player.GetFolded())
                {
                    player.ModifyHandsWon(1);
                    StatusLabel.Text = "WINNER: $" + TotalPotVal;
                    player.ModifyGameMoney(TotalPotVal);
                    if (TotalPotVal > player.GetLargestPotWon())
                    {
                        player.ModifyLargestPotWon(TotalPotVal);
                    }
                    SaveHandData(player.GetUsername(), "Bluff");
                }
                else
                {
                    SaveHandData("Bot", "Bluff");
                }

                player.UpdateHandRankStats();
                player.ModifyHandsPlayed(1);

                
                //give all money to only player left

                ProceedButton.Visible = true;
            }
        }


        private void CyclePlayers()
        {
            ProceedButton.Visible = false;
            Random rnd = new Random();
            for(int i = Cycle; i < NumPlayers - 1; i++) //cycle through all players, starting at -1, which is the player's value
            {
                StayInMoney = Math.Round(StayInMoney, 2);
                UpdateButtonStatus(); //update all player buttons
                Cycle += 1;
                if (player.GetPlayerNumber() != i) //if 'i' is not equal to the player's number (-1)
                {
                    if (!OpponentsList[i].GetFolded() && !OpponentsList[i].GetAllIn() && PlayersLeft > 1) //if the opponent hasn't folded
                    {
                        //Console.WriteLine("");
                        //Console.WriteLine("opponent number {0} is not all in and is taking his turn", OpponentsList[i].GetOpponentNumber());
                        //Console.WriteLine("");
                        Opponent opponent = OpponentsList[i];
                        string[] TurnDetails = new string[2];
                        TurnDetails = opponent.TakeTurn(StayInMoney, TotalPotVal, rnd);

                        string choice = TurnDetails[0];

                        opponent.ModifyHandMoney(Convert.ToDouble(TurnDetails[1]));

                        switch (choice)
                        {
                            case "Fold":
                                //fold the opponent, display, hide them
                                opponent.ModifyFolded(true);
                                PlayersLeft -= 1;
                                StatusLabelList[i].Text = "FOLD";
                                StatusLabelList[i].ForeColor = FoldColour.ForeColor;
                                MoneyLabelList[i].Text = "$" + opponent.GetMoney();
                                OpponentPanelList[i].Visible = false;
                                break;


                            case "Check":
                                //display this status
                                StatusLabelList[i].Text = "CHECK";
                                StatusLabelList[i].ForeColor = CheckColour.ForeColor;
                                MoneyLabelList[i].Text = "$" + opponent.GetMoney();
                                break;


                            case "Call":
                                //reduce the opponent's money by the difference between what they have put in this round and how much they need to remain in
                                //display this
                                double CallAmount = Convert.ToDouble(TurnDetails[1]);

                                opponent.ModifyRoundMoney(CallAmount);
                                TotalPotVal += CallAmount;
                                

                                StatusLabelList[i].Text = "CALL: $" + opponent.GetRoundMoney();
                                StatusLabelList[i].ForeColor = CallColour.ForeColor;

                                if (opponent.GetMoney() < 10)
                                {
                                    //Console.WriteLine("number {0} has called but gone all in", opponent.GetOpponentNumber());
                                    TotalPotVal += opponent.GetMoney();
                                    opponent.ModifyRoundMoney(opponent.GetMoney());
                                    opponent.SetMoney(0);
                                    opponent.SetAllIn(true);
                                    StatusLabelList[i].Text = "ALL IN TOTAL: $" + opponent.GetRoundMoney();
                                    StatusLabelList[i].ForeColor = AllInColour.ForeColor;
                                }

                                TotalPotAmountLabel.Text = "$" + TotalPotVal;
                                MoneyLabelList[i].Text = "$" + opponent.GetMoney();

                                break;


                            case "Raise":
                                //reduce the opponent's money by the difference between what they have put in this round and how much they need to remain in + how much they have raised by
                                //display this
                                double RaiseAmount = Convert.ToDouble(TurnDetails[1]);

                                opponent.ModifyRoundMoney(RaiseAmount);
                                StayInMoney = opponent.GetRoundMoney();
                                TotalPotVal += RaiseAmount;
                                TotalPotAmountLabel.Text = "Total Pot: $" + TotalPotVal;
                                MoneyLabelList[i].Text = "$" + opponent.GetMoney();

                                StatusLabelList[i].Text = "RAISE: $" + opponent.GetRoundMoney();
                                StatusLabelList[i].ForeColor = RaiseColour.ForeColor;

                                break;
                            case "All In":
                                //set the opponent to all in and display this
                                double AllInAmount = Math.Round(Convert.ToDouble(TurnDetails[1]), 2);
                                opponent.ModifyRoundMoney(AllInAmount);
                                TotalPotVal += AllInAmount;
                                TotalPotAmountLabel.Text = "Total Pot: $" + TotalPotVal;
                                MoneyLabelList[i].Text = "$" + opponent.GetMoney();

                                StatusLabelList[i].Text = "ALL IN TOTAL: $" + opponent.GetHandMoney();
                                StatusLabelList[i].ForeColor = AllInColour.ForeColor;
                                break;
                            default:
                                break;
                        }
                    }
                    else if(PlayersLeft == 1)
                    {
                        break;
                        //give all money to person
                    }
                }
                else
                {
                    if (!player.GetFolded() && PlayersLeft > 1)
                    {
                        if (player.GetAllIn())
                        {
                            //show the proceed button so they player can move the game on
                            ProceedButton.Visible = true;
                            break;
                        }
                        else
                        {
                            UpdateButtonStatus(); //update all player buttons
                            break;
                        }
                       
                    }
                    else if(PlayersLeft == 1 && !player.GetFolded())
                    {
                        //give all money to player
                        break;
                    }
                    else if(PlayersLeft == 1 && player.GetFolded())
                    {
                        //give all money to opponent
                        break;
                    }
                }
                
            }
            if(Cycle == NumPlayers - 1) //if this is the end of the for loop (gone through all players) then, make sure every player has put in an equal amount of money
            {
                CanMoveOn();
            }
            else if (PlayersLeft == 1)
            {
                SequenceOfEvents();
            }
        }
         
        private void CanMoveOn()
        {
            //used to calculate if the hand can move onto the next round (everyone has either put in the same amount of money or they have folded or are all in)
            MoveOn = true;

            if(PlayersLeft > 1)
            {
                for (int i = -1; i < NumPlayers - 1; i++) //cycle through all players
                {
                    if (i == player.GetPlayerNumber()) //if 'i' is the player's number, then don't look at the next opponent in opponentslist
                    {
                        if (player.GetRoundMoney() < StayInMoney && !player.GetFolded() && !player.GetAllIn()) //if the player is not folded and hasn't put in enough money, set move on to false
                        {
                            Console.WriteLine("Player has not put in enough and isnt folded or all in");
                            MoveOn = false;
                            break;
                        }
                    }
                    else if (OpponentsList[i].GetRoundMoney() < StayInMoney && !OpponentsList[i].GetFolded() && !OpponentsList[i].GetAllIn()) //if the opponent hasn't folded and hasn't
                    { //put in enough money, set move on to false
                        Console.WriteLine("an opponent has not put enough in and isnt folded or all in");
                        Console.WriteLine("opponent number {0}", i);
                        MoveOn = false;
                        break;
                    }
                }


                if (MoveOn) //if everything is ok, increase the round by one and go back to SequenceOfEvents for the next stage
                {
                    if (!player.GetFolded() && !player.GetAllIn())
                    {
                        Round += 1;
                        SequenceOfEvents();
                    }
                    else
                    {
                        ProceedButton.Visible = true;
                    }
                }
                else //otherwise reset some things and recycle players
                {
                    if (!player.GetFolded())
                    {
                        PlayerRaiseAmount = 0;
                        RaiseAmountLabel.Text = "$0";
                        Cycle = -1;
                        CyclePlayers();
                    }
                    else
                    {
                        ProceedButton.Visible = true;
                    }
                }
            }
            else
            {
                if (!player.GetFolded())
                {
                    SequenceOfEvents();
                }
                else
                {
                    ProceedButton.Visible = true;
                }
            }
            
            
        }


        private void CalculateWinner(List<Opponent> Opponents)
        {
            //calculate who has the best hand rank/ best 5 cards out of all opponents and the player
            List<Opponent> Winners = new List<Opponent>();
            List<Opponent> NonFoldedPlayersList = new List<Opponent>();
            int NonFoldedPlayers = 0;
            foreach(Opponent opp in Opponents)
            {
                if (!opp.GetFolded())
                {
                    NonFoldedPlayers += 1;
                }
            }

            if(NonFoldedPlayers > 0)
            {
                //if not all opponents have folded
                Opponent[] OppList = new Opponent[NonFoldedPlayers];
                int highest = 0;

                int cycle = 0;
                //create a list of all non-folded opponents
                foreach (Opponent opponent in Opponents)
                {
                    if (!opponent.GetFolded())
                    {
                        OppList[cycle] = opponent;
                        cycle += 1;
                    }
                }
                //sort the list in order of hand rank
                if (OppList.Length > 1)
                {
                    OppList = SortArray(OppList, 0, OppList.Length - 1);
                }

                for(int i = 0; i < OppList.Length; i++)
                {
                    NonFoldedPlayersList.Add(OppList[i]);
                }
                DisplayOpponentCards(NonFoldedPlayersList);

                //find the highest hand rank in the list
                for (int i = 0; i < OppList.Length; i++)
                {
                    StatusLabelList[OppList[i].GetOpponentNumber()].Text = HandRankWords[OppList[i].GetHandRank() - 1];
                    if (OppList[i].GetHandRank() > highest)
                    {
                        highest = OppList[i].GetHandRank();
                    }
                }

                //find all opponents with that hand rank and add them to another list
                for(int i = 0; i < OppList.Length; i++)
                {
                    if (OppList[i].GetHandRank() == highest)
                    {
                        Winners.Add(OppList[i]);
                    }
                    
                }


                //---------------------------------------------------------------------------------------------------------------------------------------------------------
                //if there are more than one opponent with the best hand rank it comes down to who has the highest 5 cards
                if (Winners.Count > 1)
                {
                    if (player.GetHandRank() > highest && !player.GetFolded())
                    {
                        //if the player's hand rank is already higher, the player wins
                        GiveMoney(Winners, "Lower");
                    }
                    else if (player.GetHandRank() == highest && !player.GetFolded())
                    {
                        //if the player's hand rank is equal then we have to find the best 5 cards, player included

                        for (int i = 0; i < 5; i++)
                        {
                            //find highest card in this 'round'
                            highest = 0;
                            for (int j = 0; j < Winners.Count; j++)
                            {
                                if (GetCardVal(Winners[j].GetCards()[i]) > highest)
                                {
                                    highest = GetCardVal(Winners[j].GetCards()[i]);
                                }
                            }

                            //find all individuals with that card, if they don't, remove them
                            for (int j = 0; j < Winners.Count; j++)
                            {
                                if (GetCardVal(Winners[j].GetCards()[i]) != highest)
                                {
                                    Winners.RemoveAt(j);
                                }
                            }
                        }
                        string Outcome = "Equal";
                        for (int i = 0; i < 5; i++)
                        {
                            if (GetCardVal(PlayerVALIDCards[i]) < GetCardVal(Winners[0].GetCards()[i]))
                            {
                                Outcome = "Higher";
                                break;
                            }
                            else if (GetCardVal(PlayerVALIDCards[i]) > GetCardVal(Winners[0].GetCards()[i]))
                            {
                                Outcome = "Lower";
                                break;
                            }
                        }



                        if (Outcome == "Higher")
                        {
                            GiveMoney(Winners, "Higher");
                        }
                        else if (Outcome == "Lower")
                        {
                            GiveMoney(Winners, "Lower");
                        }
                        else
                        {
                            GiveMoney(Winners, "Equal");
                        }


                    }
                    else
                    {
                        //winner is just other players
                        //calculate who has better cards

                        for (int i = 0; i < 5; i++)
                        {
                            if(Winners.Count == 1)
                            {
                                break;
                            }
                            else
                            {
                                //find highest card in this 'round'
                                highest = 0;
                                for (int j = 0; j < Winners.Count; j++)
                                {
                                    if (GetCardVal(Winners[j].GetCards()[i]) > highest)
                                    {
                                        highest = GetCardVal(Winners[j].GetCards()[i]);
                                    }
                                }

                                //find all players with this card value, if they don't, remove them
                                for (int j = 0; j < Winners.Count; j++)
                                {
                                    if (GetCardVal(Winners[j].GetCards()[i]) < highest)
                                    {
                                        Winners.RemoveAt(j);
                                    }
                                }
                            }
                            
                        }

                        GiveMoney(Winners, "Higher");
                    }
                }
                else if(Winners.Count == 1)
                {
                    //only one opponent with highest hand rank, so compare it with player
                    if (player.GetHandRank() > highest && !player.GetFolded())
                    {
                        GiveMoney(Winners, "Lower");
                    }
                    else if(player.GetHandRank() == highest && !player.GetFolded())
                    {
                        string Outcome = "Equal";
                        for (int i = 0; i < 5; i++)
                        {
                            if (GetCardVal(PlayerVALIDCards[i]) < GetCardVal(Winners[0].GetCards()[i]))
                            {
                                Outcome = "Higher";
                                break;
                            }
                            else if (GetCardVal(PlayerVALIDCards[i]) > GetCardVal(Winners[0].GetCards()[i]))
                            {
                                Outcome = "Lower";
                                break;
                            }
                        }



                        if (Outcome == "Higher")
                        {
                            GiveMoney(Winners, "Higher");
                        }
                        else if (Outcome == "Lower")
                        {
                            GiveMoney(Winners, "Lower");
                        }
                        else
                        {
                            GiveMoney(Winners, "Equal");
                        }
                    }
                    else
                    {
                        GiveMoney(Winners, "Higher");
                    }
                }
                
            }
            else
            {
                if (!player.GetFolded())
                {
                    GiveMoney(Winners, "Lower");
                }
            }
        }



        private void GiveMoney(List<Opponent> Winners, string PlayerWin)
        {
            //hand money out dependent on the list of opponents and the string playerwin passed

            if(PlayerWin == "Lower") //player has the best 5 cards
            {
                //increase the amount won this round, change the player's status label, increase the player's money and alter their largest pot won value if it applies
                StatusLabel.Text = "WINNER: $" + TotalPotVal;
                StatusLabel.ForeColor = WinnerColour.ForeColor;
                player.ModifyGameMoney(TotalPotVal);
                player.ModifyTotalWonThisRound(TotalPotVal);
                if (TotalPotVal > player.GetLargestPotWon())
                {
                    player.ModifyLargestPotWon(TotalPotVal);
                }
                player.ModifyHandsWon(1);

                SaveHandData(player.GetUsername(), "Best Hand");
            }
            else if(PlayerWin == "Equal") //player has an equal 5 cards to the other opponents
            {
                //calculate the number of other winners the pot must be divided between, then dish out all the money and change status labels etc, then apply the principles of the previous outcome to the player too
                int count = Winners.Count + 1;
                for (int i = 0; i < Winners.Count; i++)
                {
                    Winners[i].ModifyMoney(Math.Round(TotalPotVal / count, 2));
                    StatusLabelList[Winners[i].GetOpponentNumber()].Text += " WINNER";
                    StatusLabelList[Winners[i].GetOpponentNumber()].ForeColor = WinnerColour.ForeColor;
                } 

                player.ModifyGameMoney(Math.Round(TotalPotVal / count, 2));
                player.ModifyTotalWonThisRound(Math.Round(TotalPotVal / count, 2));
                StatusLabel.Text = "WINNER: $" + Math.Round(TotalPotVal / count, 2);
                StatusLabel.ForeColor = WinnerColour.ForeColor;
                if (player.GetTotalWonThisRound() > player.GetLargestPotWon())
                {
                    player.ModifyLargestPotWon(player.GetTotalWonThisRound());
                }
                player.ModifyHandsWon(1);
                SaveHandData(player.GetUsername(), "Best Hand");
            }
            else if(PlayerWin == "Higher") //opponents have better 5 cards than player
            {
                //apply all principles of previous outcome but leave out player events
                int count = Winners.Count;
                for (int i = 0; i < Winners.Count; i++)
                {
                    Winners[i].ModifyMoney(Math.Round(TotalPotVal / count, 2));
                    StatusLabelList[Winners[i].GetOpponentNumber()].Text += " WINNER";
                    StatusLabelList[Winners[i].GetOpponentNumber()].ForeColor = WinnerColour.ForeColor;
                }
                SaveHandData(player.GetUsername(), "Best Hand");
            }
        }



        private void FinaliseHand()
        {
            CalculateWinner(OpponentsList);

            ProceedButton.Visible = true;

            //display every opponent's new money value
            for (int i = 0; i < OpponentsList.Count; i++)
            {
                OpponentsList[i].SetRoundMoney(0);
                MoneyLabelList[i].Text = "$" + OpponentsList[i].GetMoney();
                if (OpponentsList[i].GetFolded())
                {
                    OpponentPanelList[i].Visible = false;
                }
            }
            GameMoneyLabel.Text = "$" + player.GetGameMoney();
            BankMoneyLabel.Text = "$" + player.GetMoney();

            player.UpdateHandRankStats();
            player.ModifyHandsPlayed(1);
        }


        
        private void DisplayOpponentCards(List<Opponent> Winners)
        {
            //displays the opponent's cards in the format (value + suit)
            for(int i = 0; i < Winners.Count; i++)
            {
                List<string> ValidCards = Winners[i].GetCards();
                string finalwords = "";
                for (int j = 0; j < ValidCards.Count; j++)
                {
                    int val = Convert.ToInt32(Convert.ToString(ValidCards[j][0]) + Convert.ToString(ValidCards[j][1]));
                    int suit = Convert.ToInt32(Convert.ToString(ValidCards[j][2]));

                    string final = NumberCharacters[val] + SuitCharacters[suit];
                    finalwords += final;
                }
                OpponentCardLabelList[Winners[i].GetOpponentNumber()].Text = finalwords;
            }
            
        }


        //GET SUBROUTINES
        private string GetCardValueHand(int num, int num2)
        {
            int NumberValues = Convert.ToInt32(Convert.ToString(Hands[num][num2][0]) + Convert.ToString(Hands[num][num2][1]));
            //^^ the first 2 characters are concantenated together to calculate their actual value- so 0 and 5 will become 5, 1 and 2 will become 12 etc
            int SuitValue = Convert.ToInt32(Convert.ToString(Hands[num][num2][2]));
            //^^ the third digit represents the suit
            return Convert.ToString(NumberCharacters[NumberValues]) + Convert.ToString(SuitCharacters[SuitValue]);
            //^^ the NumberValues is used to refer to the corresponding number in Table_NumberCharacters
            //^^ the SuitValue is used to refer to the corresponding symbol in Table_SuitCharacters
            //^^ these are both concantenated together to result in one card
        }


        private int GetCardSuitValue(int num, int num2)
        {
            return Convert.ToInt32(Convert.ToString(Hands[num][num2][2]));
        }


        private int GetCardVal(string card)
        {
            //returns the value of the first two characters of the card passed in as a parameter
            //done by adding the strings together than converting to an integer
            return Convert.ToInt32(Convert.ToString(card[0]) + Convert.ToString(card[1]));
        }

        private int GetCardSuit(string card)
        {
            return Convert.ToInt32(Convert.ToString(card[2]));
        }



        //---------------------------------------------------------------------------------------------------



        //UPDATE SUBROUTINES

        private void DisplayPlayerHandCards()
        {
            //displays the card value and suit in their corresponding suit colours
            Hand_Card1.Text = GetCardValueHand(0, 0); //GetCardValue is called to repeatably calculate the actual value of the hand
            Hand_Card2.Text = GetCardValueHand(0, 1); //^^

            Hand_Card1.ForeColor = ColourLabelList[GetCardSuitValue(0, 0)].ForeColor;
            Hand_Card2.ForeColor = ColourLabelList[GetCardSuitValue(0, 1)].ForeColor;
        }


        private void UpdatePlayerHandRankInformation()
        {
            //updates the 5 cards shown, the colour of the hand rank and the word used
            YouHaveLabel.Text = HandRankWords[player.GetHandRank() - 1];


            List<string> ValidCards = PlayerVALIDCards;
            string finalwords = "";
            for (int i = 0; i < ValidCards.Count; i++)
            {
                int val = Convert.ToInt32(Convert.ToString(ValidCards[i][0]) + Convert.ToString(ValidCards[i][1]));
                int suit = Convert.ToInt32(Convert.ToString(ValidCards[i][2]));

                string final = NumberCharacters[val] + SuitCharacters[suit] + " ";
                finalwords += final;
            }
            YouHaveCardsLabel.Text = finalwords;

            YouHaveLabel.ForeColor = RankColourLabelList[player.GetHandRank() - 1].ForeColor;
            YouHaveCardsLabel.ForeColor = RankColourLabelList[player.GetHandRank() - 1].ForeColor;



        }


        private void UpdateTableCardsVisibility()
        {
            //checks the current round of the hand and then makes different community cards visible or not visible
            if (Round == 0)
            {
                for (int i = 0; i < TableCardsList.Count; i++)
                {
                    TableCardsList[i].Visible = false;
                }
            }
            else if (Round == 1)
            {
                int i = 0;
                for (i = 0; i < 3; i++)
                {
                    TableCardsList[i].Visible = true;
                }

            }
            else if (Round == 2)
            {
                TableCardsList[3].Visible = true;
            }
            else
            {
                TableCardsList[4].Visible = true;
            }
        }


        private void UpdateTableCardsLabels()
        {
            //updates the colour and text on every community card label
            int Cycle = NumPlayers * 2;
            for (int i = 0; i < TableCardsLabelList.Count; i++)
            {
                string card = ActualDeck[Cycle];
                int NumberValues = Convert.ToInt32(Convert.ToString(card[0]) + Convert.ToString(card[1]));
                int SuitValue = Convert.ToInt32(Convert.ToString(card[2]));
                string ActualCard = Convert.ToString(NumberCharacters[NumberValues]) + Convert.ToString(SuitCharacters[SuitValue]);

                TableCardsLabelList[i].Text = ActualCard;
                TableCardsLabelList[i].ForeColor = ColourLabelList[SuitValue].ForeColor;
                Cycle += 1;
            }
        }


        private void UpdateMoneyLabels()
        {
            BankMoneyLabel.Text = "$" + Convert.ToString(player.GetMoney());
            TotalPotAmountLabel.Text = "$" + Convert.ToString(TotalPotVal);

            for (int i = 0; i < MoneyLabelList.Count; i++)
            {
                MoneyLabelList[i].Text = "$" + Convert.ToString(OpponentList[i].GetMoney());
            }
        }


        private void UpdateButtonStatus()
        {
            //checks a number of factors that determine what you will be able to do this round then makes those
            //buttons acitve or not
            if (player.GetFolded() || Cycle != player.GetPlayerNumber() + 1) //if you have folded or it's not your turn, turn off all the buttons
            {
                CallButton.Text = "Call";
                CheckButton.Enabled = false;
                FoldButton.Enabled = false;
                CallButton.Enabled = false;
                RaiseConfirmButton.Enabled = false;
                RaiseMoreButton.Enabled = false;
                RaiseLessButton.Enabled = false;
                RaiseMoreMoreButton.Enabled = false;
                RaiseLessLessButton.Enabled = false;
                ALLINButton.Enabled = false;
            }
            else
            {
                FoldButton.Enabled = true; //you are always able to fold

                //work out check logic
                if(player.GetRoundMoney() == StayInMoney)
                {
                    //if you've already put in as much as you need to stay in, you may check or raise
                    CheckButton.Enabled = true;
                    if(player.GetGameMoney() > 0)
                    {
                        RaiseConfirmButton.Enabled = true;
                        RaiseMoreButton.Enabled = true;
                        RaiseLessButton.Enabled = true;
                        RaiseMoreMoreButton.Enabled = true;
                        RaiseLessLessButton.Enabled = true;
                        ALLINButton.Enabled = true;
                    }
                }

                if(player.GetRoundMoney() < StayInMoney && player.GetGameMoney() >= (StayInMoney - player.GetRoundMoney()))
                {
                    //if you've put in less than as much as you need to stay in and you have enough to call,
                    //then you may call or raise
                    CallButton.Enabled = true;
                    StayInMoney = Math.Round(StayInMoney, 2);
                    CallAmountLabel.Text = "Call: $" + StayInMoney;
                    RaiseConfirmButton.Enabled = true;
                    RaiseMoreButton.Enabled = true;
                    RaiseLessButton.Enabled = true;
                    RaiseMoreMoreButton.Enabled = true;
                    RaiseLessLessButton.Enabled = true;
                    PlayerRaiseAmount = StayInMoney - player.GetRoundMoney();
                    PlayerRaiseAmount = Math.Round(PlayerRaiseAmount, 2);
                    RaiseAmountLabel.Text = "$" + PlayerRaiseAmount;
                    ALLINButton.Enabled = true;
                }
                
                else if(player.GetRoundMoney() < StayInMoney && player.GetGameMoney() < (StayInMoney - player.GetRoundMoney()))
                {
                    //if you've put in less than you need to stay in but you don't have enough, the only way
                    //to stay in is to go all in, so change the text of the call button and enable it
                    ALLINButton.Enabled = true;
                    CallButton.Enabled = false;
                    StayInMoney = Math.Round(StayInMoney, 2);
                    CallAmountLabel.Text = "$" + player.GetGameMoney();
                    RaiseConfirmButton.Enabled = false;
                    RaiseMoreButton.Enabled = false;
                    RaiseLessButton.Enabled = false;
                    RaiseMoreMoreButton.Enabled = false;
                    RaiseLessLessButton.Enabled = false;
                    RaiseAmountLabel.Text = "$0";
                    ALLINButton.Enabled = true;
                }
            }


        }


        //---------------------------------------------------------------------------------------------------



        //CLICK BUTTON SUBROUTINES
        private void BackButton_Click(object sender, EventArgs e)
        {
            player.ModifyMoney(player.GetGameMoney());
            SavePlayerData();
            this.Hide();
            Form TblCust = new TableCustomisation(player);
            TblCust.ShowDialog();
            this.Close();
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            StatusLabel.Text = "CHECK"; //change the status label to display your choice
            StatusLabel.ForeColor = CheckColour.ForeColor;
            CyclePlayers(); //return back to the cycleplayers function that will loop through the rest of the players


            //System.Threading.Thread.Sleep(1000);
            //SendKeys.Send("{PRTSC}");
            //Image myImage = Clipboard.GetImage();

            //myImage.Save("E:\\" + Convert.ToString(numtocall) + ".jpg");
        }

        private void FoldButton_Click(object sender, EventArgs e)
        {
            PlayersLeft -= 1;
            player.ModifyFolded(true); //alter the player's fold variable to true which will exclude the player from being in circulation
            StatusLabel.Text = "FOLD"; //change the status label to display your choice
            StatusLabel.ForeColor = FoldColour.ForeColor;
            CyclePlayers(); //return back to the cycleplayers function that will loop through the rest of the players
        }

        private void CallButton_Click(object sender, EventArgs e)
        {
            //reduce their money by the difference
            //then updates labels and the pot value
            double Difference = StayInMoney - player.GetRoundMoney();

            player.ModifyGameMoney(Difference * -1);
            player.ModifyRoundMoney(Difference);
            player.ModifyHandMoney(Difference);

            TotalPotVal += Difference;

            TotalPotAmountLabel.Text = "$" + TotalPotVal;
            GameMoneyLabel.Text = "$" + player.GetGameMoney();
            StatusLabel.Text = "CALL: $" + player.GetRoundMoney();
            StatusLabel.ForeColor = CallColour.ForeColor;

            PlayerRaiseAmount = 0;

            CallAmountLabel.Text = "";
            RaiseAmountLabel.Text = "";

            CyclePlayers(); //return back to the cycleplayers function that will loop through the rest of the players
        }

        private void ALLINButton_Click(object sender, EventArgs e)
        {
            //set them to all in and reduce their money to 0
            //then update money labels and the total pot
            player.SetAllIn(true);

            player.ModifyRoundMoney(player.GetGameMoney());
            player.ModifyHandMoney(player.GetGameMoney());

            TotalPotVal += player.GetGameMoney();
            TotalPotAmountLabel.Text = "$" + TotalPotVal;
            StatusLabel.Text = "ALL IN: $" + player.GetHandMoney();
            StatusLabel.ForeColor = AllInColour.ForeColor;

            if (player.GetGameMoney() > StayInMoney - player.GetRoundMoney())
            {
                StayInMoney = player.GetHandMoney();
            }

            PlayerRaiseAmount = 0;

            CallAmountLabel.Text = "";
            RaiseAmountLabel.Text = "";

            player.ModifyGameMoney(player.GetGameMoney() * -1);
            GameMoneyLabel.Text = "$0";

            CyclePlayers(); //return back to the cycleplayers function that will loop through the rest of the players
        }

        private void RaiseLessButton_Click(object sender, EventArgs e)
        {
            double DefaultDecreaseAmount = 10.00;
            double MoneyBasedDecreaseAmount = Math.Round(player.GetGameMoney() * 0.01, 2);
            double DecreaseAmount;

            if (DefaultDecreaseAmount > MoneyBasedDecreaseAmount)
            {
                DecreaseAmount = MoneyBasedDecreaseAmount;
            }
            else
            {
                DecreaseAmount = DefaultDecreaseAmount;
            }

            if (PlayerRaiseAmount - DecreaseAmount > (StayInMoney - player.GetRoundMoney())) //if the player can safely decrease their raise amount
            {
                PlayerRaiseAmount -= DecreaseAmount; //increase the player's raise amount by this
                PlayerRaiseAmount = Math.Round(PlayerRaiseAmount, 2);
                RaiseAmountLabel.Text = "$" + PlayerRaiseAmount; //display
            }
            
        }


        private void RaiseLessLessButton_Click(object sender, EventArgs e)
        {
            double DefaultDecreaseAmount = 100.00;
            double MoneyBasedDecreaseAmount = Math.Round(player.GetGameMoney() * 0.1, 2);
            double DecreaseAmount;

            if (DefaultDecreaseAmount > MoneyBasedDecreaseAmount)
            {
                DecreaseAmount = MoneyBasedDecreaseAmount;
            }
            else
            {
                DecreaseAmount = DefaultDecreaseAmount;
            }

            if (PlayerRaiseAmount - DecreaseAmount > (StayInMoney - player.GetRoundMoney())) //if the player can safely increase their raise amount
            {
                PlayerRaiseAmount -= DecreaseAmount; //increase the player's raise amount by this
                PlayerRaiseAmount = Math.Round(PlayerRaiseAmount, 2);
                RaiseAmountLabel.Text = "$" + PlayerRaiseAmount; //display
            }
        }


        private void RaiseMoreButton_Click(object sender, EventArgs e)
        {
            double DefaultIncreaseAmount = 10.00;
            double MoneyBasedIncreaseAmount = Math.Round(player.GetGameMoney() * 0.01, 2);
            double IncreaseAmount;

            if(DefaultIncreaseAmount > MoneyBasedIncreaseAmount)
            {
                IncreaseAmount = MoneyBasedIncreaseAmount;
            }
            else
            {
                IncreaseAmount = DefaultIncreaseAmount;
            }

            if (PlayerRaiseAmount + IncreaseAmount <= player.GetGameMoney()) //if the player can safely increase their raise amount
            {
                PlayerRaiseAmount += IncreaseAmount; //increase the player's raise amount by this
                PlayerRaiseAmount = Math.Round(PlayerRaiseAmount, 2);
                RaiseAmountLabel.Text = "$" + PlayerRaiseAmount; //display
            }
            else if(PlayerRaiseAmount + IncreaseAmount > player.GetGameMoney())
            {
                PlayerRaiseAmount = player.GetGameMoney();
                PlayerRaiseAmount = Math.Round(PlayerRaiseAmount, 2);
                RaiseAmountLabel.Text = "$" + PlayerRaiseAmount; //display
            }
        }


        private void RaiseMoreMoreButton_Click(object sender, EventArgs e)
        {
            double DefaultIncreaseAmount = 100.00;
            double MoneyBasedIncreaseAmount = Math.Round(player.GetGameMoney() * 0.1, 2);
            double IncreaseAmount;

            if (DefaultIncreaseAmount > MoneyBasedIncreaseAmount) //if a tenth of your money is less than 50, increase it by that instead
            {
                IncreaseAmount = MoneyBasedIncreaseAmount;
            }
            else
            {
                IncreaseAmount = DefaultIncreaseAmount;
            }

            if (PlayerRaiseAmount + IncreaseAmount <= player.GetGameMoney()) //if the player can safely increase their raise amount
            {
                PlayerRaiseAmount += IncreaseAmount; //increase the player's raise amount by this
                PlayerRaiseAmount = Math.Round(PlayerRaiseAmount, 2);
                RaiseAmountLabel.Text = "$" + PlayerRaiseAmount; //display
            }
            else if (PlayerRaiseAmount + IncreaseAmount > player.GetGameMoney())
            {
                PlayerRaiseAmount = player.GetGameMoney();
                PlayerRaiseAmount = Math.Round(PlayerRaiseAmount, 2);
                RaiseAmountLabel.Text = "$" + PlayerRaiseAmount; //display
            }
        }


        private void RaiseConfirmButton_Click(object sender, EventArgs e)
        {
            if(PlayerRaiseAmount > (StayInMoney - player.GetRoundMoney()))
            {
                StayInMoney = player.GetRoundMoney() + PlayerRaiseAmount;

                player.ModifyRoundMoney(PlayerRaiseAmount);
                player.ModifyHandMoney(PlayerRaiseAmount);

                if (PlayerRaiseAmount == player.GetGameMoney())
                {
                    StatusLabel.Text = "ALL IN: $" + player.GetRoundMoney();
                    StatusLabel.ForeColor = AllInColour.ForeColor;
                    player.SetAllIn(true);
                }
                else
                {
                    StatusLabel.Text = "RAISE: $" + player.GetRoundMoney();
                }

                player.ModifyGameMoney(PlayerRaiseAmount * -1);

                TotalPotVal += PlayerRaiseAmount;
                
                TotalPotAmountLabel.Text = "$" + TotalPotVal;
                GameMoneyLabel.Text = "$" + player.GetGameMoney();

                PlayerRaiseAmount = 0;

                CallAmountLabel.Text = "";
                RaiseAmountLabel.Text = "";

                CyclePlayers(); //return back to the cycleplayers function that will loop through the rest of the players
            }


        }


        private void ProceedButton_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("ROUND: {0}", Round);

            //calculate number of players left
            int numplayers = 0;
            foreach(Opponent opp in OpponentsList)
            {
                if (!opp.GetFolded())
                {
                    numplayers += 1;
                }
            }

            if (!player.GetFolded())
            {
                numplayers += 1;
            }

            if(Round >= 4)
            {
                //if it's the last round, then the button is here to start the new hand
                Reset();
            }
            else
            {
               // Console.WriteLine("There are {0} players", numplayers);
                //otherwise it's because the player is all in or folded
                if(numplayers > 1)
                {
                    //if there is more than one player left then the round has to continue
                    CanMoveOn();
                    if (MoveOn)
                    {
                        Console.WriteLine("MOVE ON TRUE");
                        Round += 1;
                        SequenceOfEvents();
                    }
                    else
                    {
                        Console.WriteLine("MOVE ON FALSE RECYCLE");
                        PlayerRaiseAmount = 0;
                        RaiseAmountLabel.Text = "$0";
                        if (!player.GetAllIn())
                        {
                            Cycle = -1;
                        }
                        else
                        {
                            Cycle = 0;
                        }
                        
                        CyclePlayers();
                    }
                }
                else
                {
                    //if there is only 1 player left then the round is over
                    Reset();
                }
            }
            
        }




        //CUSTOM DEAL

        private void CustomDeal(int handnum)
        {
            //contains a list of decks used in testing to display that the program can correctly calculate and deal with each hand rank and hand outcome.

            //HIGH CARD
            string[] cards = { "121", "012", "043", "012", "030", "103", "072", "033", "113", "122", "042", "011", "092", "070", "020", "011", "071", "101", "062", "022", "053" };

            //PAIR
            string[] cards2 = { "121", "122", "043", "012", "030", "103", "072", "033", "113", "123", "042", "011", "092", "070", "020", "011", "071", "101", "062", "022", "053" };

            //TWO PAIR
            string[] cards3 = { "121", "123", "043", "012", "030", "103", "072", "033", "113", "122", "042", "011", "092", "070", "020", "011", "071", "101", "062", "063", "053" };

            //THREE OF A KIND
            string[] cards4 = { "121", "123", "043", "012", "030", "103", "072", "033", "113", "120", "042", "011", "092", "070", "020", "011", "071", "101", "062", "122", "053" };

            //STRAIGHT
            string[] cards5 = { "062", "031", "043", "012", "030", "103", "072", "033", "113", "122", "042", "011", "092", "070", "020", "011", "071", "101", "042", "093", "053" };

            //FLUSH
            string[] cards6 = { "081", "031", "043", "012", "030", "103", "072", "033", "113", "122", "043", "011", "092", "070", "020", "011", "071", "101", "042", "061", "053" };

            //FULL HOUSE
            string[] cards7 = { "121", "060", "043", "012", "030", "103", "072", "033", "113", "120", "042", "011", "092", "070", "020", "011", "071", "101", "062", "123", "063" };

            //FOUR OF A KIND
            string[] cards8 = { "121", "123", "043", "012", "030", "103", "072", "033", "113", "120", "042", "011", "092", "070", "020", "011", "071", "101", "062", "122", "120" };

            //STRAIGHT FLUSH
            string[] cards9 = { "080", "050", "043", "012", "030", "103", "072", "033", "113", "120", "042", "011", "092", "071", "020", "011", "071", "090", "060", "123", "070" };

            //ROYAL FLUSH
            string[] cards10 = { "122", "112", "043", "012", "030", "103", "072", "033", "113", "120", "042", "011", "092", "071", "020", "011", "071", "082", "062", "092", "102" };


            //                  |            |             |             |             |             |            |             |              |                                  |
            //                  |            |             |             |             |             |            |             |              |                                  |
            //                  |            |             |             |             |             |            |             |              |                                  |
            //                  |            |             |             |             |             |            |             |              |                                  |
            //                  |            |             |             |             |             |            |             |              |                                  |
            //HIGH CARD WINNER
            string[] cards11 = { "121", "000", "001", "010", "002", "011", "003", "012", "013", "020", "021", "030", "022", "031", "032", "023", "110", "092", "083", "071", "041" };

            //PAIR WINNER
            string[] cards12 = { "121", "120", "102", "103", "111", "011", "093", "012", "083", "020", "021", "112", "030", "031", "032", "042", "110", "092", "082", "063", "041" };

            //TWO PAIR WINNER
            string[] cards13 = { "121", "120", "102", "103", "111", "011", "093", "012", "070", "020", "021", "112", "030", "031", "032", "042", "110", "092", "082", "083", "041" };

            //THREE OF A KIND WINNER
            string[] cards14 = { "121", "120", "102", "103", "111", "112", "083", "081", "070", "071", "072", "112", "030", "031", "032", "042", "110", "122", "082", "073", "061" };

            //STRAIGHT WINNER
            string[] cards15 = { "111", "120", "102", "103", "111", "112", "083", "081", "070", "020", "072", "112", "030", "031", "032", "042", "100", "072", "062", "093", "081" };

            //FLUSH WINNER
            string[] cards16 = { "111", "121", "101", "103", "111", "112", "083", "081", "070", "011", "072", "061", "030", "021", "032", "051", "001", "031", "041", "071", "062" };

            //FULL HOUSE WINNER
            string[] cards17 = { "122", "121", "061", "063", "060", "052", "120", "081", "050", "051", "072", "071", "030", "031", "032", "033", "111", "112", "110", "123", "062" };

            //FOUR OF A KIND WINNER
            string[] cards18 = { "123", "120", "061", "063", "060", "052", "100", "081", "050", "051", "072", "071", "030", "031", "032", "033", "111", "112", "110", "113", "062" };

            //FOUR OF A KIND WINNER
            string[] cards19 = { "113", "120", "061", "063", "060", "052", "120", "081", "050", "051", "072", "071", "030", "031", "032", "033", "111", "112", "110", "123", "062" };

            //STRAIGHT FLUSH WINNER
            string[] cards20 = { "111", "101", "062", "063", "060", "052", "120", "081", "050", "053", "072", "070", "030", "031", "032", "033", "051", "081", "061", "071", "091" };

            //STRAIGHT FLUSH WINNER
            string[] cards21 = { "111", "101", "062", "063", "060", "052", "120", "081", "050", "053", "072", "070", "030", "031", "032", "033", "000", "081", "012", "071", "091" };

            //ROYAL FLUSH WINNER
            string[] cards22 = { "081", "070", "062", "063", "060", "052", "120", "081", "050", "053", "072", "070", "030", "031", "032", "033", "103", "093", "123", "113", "083" };

            //ROYAL FLUSH WINNER
            string[] cards23 = { "083", "070", "062", "063", "060", "052", "120", "081", "050", "053", "072", "070", "030", "031", "032", "033", "103", "093", "123", "113", "081" };


            List<string[]> Decks = new List<string[]>();

            Decks.Add(cards);
            Decks.Add(cards2);
            Decks.Add(cards3);
            Decks.Add(cards4);
            Decks.Add(cards5);
            Decks.Add(cards6);
            Decks.Add(cards7);
            Decks.Add(cards8);
            Decks.Add(cards9);
            Decks.Add(cards10);


            Decks.Add(cards11);
            Decks.Add(cards12);
            Decks.Add(cards13);
            Decks.Add(cards14);
            Decks.Add(cards15);
            Decks.Add(cards16);
            Decks.Add(cards17);
            Decks.Add(cards18);
            Decks.Add(cards19);
            Decks.Add(cards20);
            Decks.Add(cards21);
            Decks.Add(cards22);
            Decks.Add(cards23);



            for (int i = 0; i < Decks[handnum].Length; i++)
            {
                ActualDeck.Add(Decks[handnum][i]);
            }
        }

        



        //---------------------------------------------------------------------------------------------------
    }
}
