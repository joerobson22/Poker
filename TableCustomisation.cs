using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA_Computer_Science_Poker__️__️__️__️
{
    public partial class TableCustomisation : Form
    {
        public int NumPlayers;
        Player player;
        List<PictureBox> ImagesList = new List<PictureBox>();
        List<PictureBox> PictureBoxList = new List<PictureBox>();
        List<Label> NameLabelList = new List<Label>();
        List<TrackBar> DifficultySliderList = new List<TrackBar>();
        List<string> OpponentNameList = new List<string>();
        List<Opponent> OpponentList = new List<Opponent>();
        Random random = new Random();

        public TableCustomisation(Player player)
        {
            InitializeComponent();
            this.player = player;
            
        }

        //Load and setup subroutines

        public void TableCustomisation_Load(object sender, EventArgs e)
        {
            //set up everything
            //fill the lists with every node, gather all opponent names and then create 7 random opponents
            FillLists();

            UnloadExternalFiles();

            CreateRandomOpponents();

            
            //change each image to random image, and each name to the random name
            for(int i = 0; i < PictureBoxList.Count; i++)
            {
                PictureBoxList[i].Image = ImagesList[OpponentList[i].GetSpriteNumber()].Image;
                NameLabelList[i].Text = OpponentList[i].GetName();
            }


            PlayerNameLabel.Text = player.GetUsername();
            PlayerMoneyLabel.Text = "$" + Convert.ToString(player.GetMoney());
        } //startup of window


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

            //add all different difficulty sliders to the list
            DifficultySliderList.Add(DifficultySlider1);
            DifficultySliderList.Add(DifficultySlider2);
            DifficultySliderList.Add(DifficultySlider3);
            DifficultySliderList.Add(DifficultySlider4);
            DifficultySliderList.Add(DifficultySlider5);
            DifficultySliderList.Add(DifficultySlider6);
            DifficultySliderList.Add(DifficultySlider7);
        }

        private void UnloadExternalFiles()
        {
            //load all names from external file
            StreamReader SR = new StreamReader("OpponentNames.txt", true);
            while (SR.Peek() != -1)
            {
                OpponentNameList.Add(SR.ReadLine());
            }
            SR.Close();
        }

        private void CreateRandomOpponents()
        {
            Random rnd = new Random();
            //create new opponent classes
            for (int i = 0; i < NameLabelList.Count; i++)
            {

                double StartMoney = rnd.Next(200, 1000);
                Opponent NewOpponent = new Opponent(OpponentNameList[random.Next(0, OpponentNameList.Count)], StartMoney, 0, random.Next(0, ImagesList.Count), i);
                OpponentList.Add(NewOpponent);
            }

        }


        //-------------------------------------------



        //Switching windows preparation subroutines

        private void UpdateDifficulties()
        {
            //sets the difficulties of every opponent object created
            for(int i = 0; i < OpponentList.Count; i++)
            {
                OpponentList[i].SetDifficulty(DifficultySliderList[i].Value);
            }
        }

        private bool VerifyBuyInAmount(string Amount)
        {
            //ensures the buy-in amount entered is valid
            bool valid = true;
            try
            {
                double amount = Convert.ToDouble(Amount);
                if (amount > player.GetMoney()){
                    valid = false;
                    MessageBox.Show("Buy In Amount Cannot Be More Than Your Money.", "Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                valid = false;
                MessageBox.Show("Buy In Amount Invalid. Please Enter a Whole Number of Decimal.", "Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return valid;
        }

        //--------------------------------------------


        //Button subroutines

        private void PlayButton_Click(object sender, EventArgs e)
        {
            //verifies the buy in amount, then switches the window to the table window, keeping all the data customised.
            if (VerifyBuyInAmount(BuyInAmountTextBox.Text))
            {
                double BuyInAmount = Convert.ToDouble(BuyInAmountTextBox.Text);
                UpdateDifficulties();
                NumPlayers = 8;
                //switching to poker table to actually play
                this.Hide();
                Form Tbl = new Table(NumPlayers, player, OpponentList, BuyInAmount);
                Tbl.WindowState = FormWindowState.Maximized;
                Tbl.ShowDialog();
                this.Close();
            }

            
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            //switching back to main menu (following same process as before)
            this.Hide();
            Form MM = new MainMenu(player);
            MM.ShowDialog();
            this.Close();
        }


        //--------------------------------------------------
    }
}
