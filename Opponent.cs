using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NEA_Computer_Science_Poker__️__️__️__️
{
    public class Opponent
    {
        //variables used to give the opponent individuality
        private string Name;
        private double Money;
        private int Difficulty;
        private int SpriteNumber;
        private int OpponentNumber;

        //variables used to give the opponent functionality in the game
        private double RoundMoney;
        private double HandMoney;
        private double TotalWonThisRound;
        private bool Folded;
        private bool AllIn;
        private int HandRank;

        private List<string> Cards = new List<string>();

        public Opponent(string name, double money, int difficulty, int spriteNumber, int opponentNumber)
        {
            Name = name;
            Money = money;
            Difficulty = difficulty;
            SpriteNumber = spriteNumber;
            OpponentNumber = opponentNumber;
        }

        //GET SUBROUTINES

        public bool GetAllIn()
        {
            return AllIn;
        }


        public double GetHandMoney()
        {
            return HandMoney;
        }

        public List<string> GetCards()
        {
            return Cards;
        }

        public int GetOpponentNumber()
        {
            return OpponentNumber;
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

        public string GetName()
        {
            return Name;
        }

        public double GetMoney()
        {
            return Money;
        }

        public int GetDifficulty()
        {
            return Difficulty;
        }

        public int GetSpriteNumber()
        {
            return SpriteNumber;
        }

        //-----------------------


        //SET SUBROUTINES

        public void SetTotalWonThisRound(double amount)
        {
            TotalWonThisRound = amount;
            TotalWonThisRound = Math.Round(TotalWonThisRound, 2);
        }


        public void SetAllIn(bool state)
        {
            AllIn = state;
        }


        public void SetHandMoney(double num)
        {
            HandMoney = num;
        }

        public void SetHandRank(int num)
        {
            HandRank = num;
        }


        public void SetRoundMoney(double num)
        {
            RoundMoney = num;
            RoundMoney = Math.Round(RoundMoney, 2);
        }

        public void SetMoney(double amount)
        {
            Money = amount;
        }
        public void SetDifficulty(int num)
        {
            Difficulty = num;
        }

        //------------------



        //MODIFY SUBROUTINES

        public void ModifyHandMoney(double num)
        {
            HandMoney += num;
        }

        public void ModifyCards(List<string> NewCards)
        {
            Cards.Clear();
            foreach (string card in NewCards)
            {
                Cards.Add(card);
            }
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

        //---------------------


        //TURN SUBROUTINES
        public string[] TakeTurn(double StayInMoney, double TotalPot, Random rnd)
        {
            return CalculateValues(StayInMoney, TotalPot, rnd, ChooseMove(StayInMoney, TotalPot, rnd));
        }

        public virtual List<string> ChooseMove(double StayInMoney, double TotalPot, Random rnd)
        {
            List<string> PossibleChoices = new List<string>();
            double Difference = StayInMoney - GetRoundMoney();

            PossibleChoices.Add("Fold");

            //can check?
            if (GetRoundMoney() == StayInMoney)
            {
                PossibleChoices.Add("Check");
                PossibleChoices.Add("Check");
                PossibleChoices.Add("Check");
                PossibleChoices.Add("Check");
                PossibleChoices.Add("Check");
                if (GetMoney() >= 1)
                {
                    PossibleChoices.Add("Raise");
                }
            }
            else if (GetRoundMoney() < StayInMoney && GetMoney() >= Difference)
            {
                PossibleChoices.Add("Call");
                PossibleChoices.Add("Call");
                PossibleChoices.Add("Call");
                PossibleChoices.Add("Call");
                PossibleChoices.Add("Call");
                if (GetMoney() - Difference > 0)
                {
                    PossibleChoices.Add("Raise");
                }
            }
            else if (GetRoundMoney() < StayInMoney)
            {
                if (AllIn)
                {
                    PossibleChoices.Remove("Fold");
                    PossibleChoices.Add("All In2");
                }
                else
                {
                    PossibleChoices.Add("All In");
                }

            }
            return PossibleChoices;
        }

        public virtual string[] CalculateValues(double StayInMoney, double TotalPot, Random rnd, List<string> PossibleChoices)
        {
            double Difference = StayInMoney - GetRoundMoney();
            string[] TurnDetails = new string[2];
            string choice = PossibleChoices[rnd.Next(0, PossibleChoices.Count)];
            TurnDetails[0] = choice;

            if (choice == "Fold")
            {
                ModifyFolded(true);
                TurnDetails[1] = "0";
            }
            else if (choice == "Check")
            {
                TurnDetails[1] = "0";
            }
            else if (choice == "Call")
            {
                TurnDetails[1] = Convert.ToString(Difference);
                ModifyMoney(Difference * -1);
            }
            else if (choice == "Raise")
            {
                double RaiseAmount = Difference;
                double PercentageRaise;

                PercentageRaise = rnd.Next(1, 99);
                RaiseAmount += Math.Round((GetMoney() - Difference) * (PercentageRaise / 100), 2);
                ModifyMoney(RaiseAmount * -1);
                TurnDetails[1] = Convert.ToString(RaiseAmount);
            }
            else if (choice == "All In")
            {
                TurnDetails[1] = Convert.ToString(GetMoney());
                ModifyMoney(GetMoney() * -1);
                AllIn = true;
            }
            else if (choice == "All In2")
            {
                TurnDetails[1] = "0";
                TurnDetails[0] = "All In";
            }
            return TurnDetails;
        }


        //---------------------------





        public class EasyOpponent : Opponent
        {
            //Bet aggressiveness on a scale of 1-100
            int BetAggressiveness = 20;

            //represents the number of times the following moves will appear in the final list of choices to make
            int CheckChance = 5;
            int FoldChance = 2;
            int CallChance = 5;
            int RaiseChance = 1;
            int AllInChance = 1;

            public EasyOpponent(string name, double money, int difficulty, int spritenumber, int opponentnumber) : base(name, money, difficulty, spritenumber, opponentnumber)
            {
                Name = name;
                Money = money;
                Difficulty = difficulty;
                SpriteNumber = spritenumber;
                OpponentNumber = opponentnumber;
            }

            public override List<string> ChooseMove(double StayInMoney, double TotalPot, Random rnd)
            {
                List<string> PossibleChoices = new List<string>();
                double Difference = StayInMoney - GetRoundMoney();

                for(int i = 0; i < FoldChance; i++)
                {
                    PossibleChoices.Add("Fold");
                }

                

                //can check?
                if (GetRoundMoney() == StayInMoney)
                {
                    for (int i = 0; i < CheckChance; i++)
                    {
                        PossibleChoices.Add("Check");
                    }


                    if (GetMoney() >= 1)
                    {
                        for (int i = 0; i < RaiseChance; i++)
                        {
                            PossibleChoices.Add("Raise");
                        }
                    }
                }
                else if (GetRoundMoney() < StayInMoney && GetMoney() >= Difference)
                {
                    if(GetMoney() == Difference)
                    {
                        for (int i = 0; i < AllInChance; i++)
                        {
                            PossibleChoices.Add("All In");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < CallChance; i++)
                        {
                            PossibleChoices.Add("Call");
                        }


                        if (GetMoney() - Difference > 0)
                        {
                            for (int i = 0; i < RaiseChance; i++)
                            {
                                PossibleChoices.Add("Raise");
                            }
                        }
                    }
                    
                }
                else if (GetRoundMoney() < StayInMoney)
                {
                    if (AllIn)
                    {
                        while (PossibleChoices.Contains("Fold"))
                        {
                            PossibleChoices.Remove("Fold");
                        }
                        
                        PossibleChoices.Add("All In2");
                    }
                    else
                    {
                        for (int i = 0; i < AllInChance; i++)
                        {
                            PossibleChoices.Add("All In");
                        }
                    }

                }
                return PossibleChoices;
            }

            public override string[] CalculateValues(double StayInMoney, double TotalPot, Random rnd, List<string> PossibleChoices)
            {
                double Difference = StayInMoney - GetRoundMoney();
                string[] TurnDetails = new string[2];
                string choice = PossibleChoices[rnd.Next(0, PossibleChoices.Count)];
                TurnDetails[0] = choice;

                if (choice == "Fold")
                {
                    ModifyFolded(true);
                    TurnDetails[1] = "0";
                }
                else if (choice == "Check")
                {
                    TurnDetails[1] = "0";
                }
                else if (choice == "Call")
                {
                    TurnDetails[1] = Convert.ToString(Difference);
                    ModifyMoney(Difference * -1);
                }
                else if (choice == "Raise")
                {
                    double RaiseAmount = Difference;
                    double PercentageRaise;

                    PercentageRaise = rnd.Next(1, BetAggressiveness);
                    RaiseAmount += Math.Round((GetMoney() - Difference) * (PercentageRaise / 100), 2);
                    ModifyMoney(RaiseAmount * -1);
                    TurnDetails[1] = Convert.ToString(RaiseAmount);
                }
                else if (choice == "All In")
                {
                    TurnDetails[1] = Convert.ToString(GetMoney());
                    ModifyMoney(GetMoney() * -1);
                    AllIn = true;
                }
                else if (choice == "All In2")
                {
                    TurnDetails[1] = "0";
                    TurnDetails[0] = "All In";
                }
                return TurnDetails;
            }
        }

        public class MediumOpponent : Opponent
        {
            //Bet aggressiveness on a scale of 1-100
            int[] BetAggressiveness = {1, 5, 5, 5, 10, 10, 10, 10, 10, 20, 20, 20, 20, 20, 20, 20, 25, 25, 25, 30, 30, 30, 35, 35, 40, 40, 45, 50};

            //represents the number of times the following moves will appear in the final list of choices to make
            int CheckChance = 4;
            int FoldChance = 1;
            int CallChance = 4;
            int RaiseChance = 2;
            int AllInChance = 2;

            public MediumOpponent(string name, double money, int difficulty, int spritenumber, int opponentnumber) : base(name, money, difficulty, spritenumber, opponentnumber)
            {
                Name = name;
                Money = money;
                Difficulty = difficulty;
                SpriteNumber = spritenumber;
                OpponentNumber = opponentnumber;
            }

            public override List<string> ChooseMove(double StayInMoney, double TotalPot, Random rnd)
            {
                List<string> PossibleChoices = new List<string>();
                double Difference = StayInMoney - GetRoundMoney();

                for (int i = 0; i < FoldChance; i++)
                {
                    PossibleChoices.Add("Fold");
                }



                //can check?
                if (GetRoundMoney() == StayInMoney)
                {
                    for (int i = 0; i < CheckChance; i++)
                    {
                        PossibleChoices.Add("Check");
                    }


                    if (GetMoney() >= 1)
                    {
                        for (int i = 0; i < RaiseChance; i++)
                        {
                            PossibleChoices.Add("Raise");
                        }
                    }
                }
                else if (GetRoundMoney() < StayInMoney && GetMoney() >= Difference)
                {
                    for (int i = 0; i < CallChance; i++)
                    {
                        PossibleChoices.Add("Call");
                    }


                    if (GetMoney() - Difference > 0)
                    {
                        for (int i = 0; i < RaiseChance; i++)
                        {
                            PossibleChoices.Add("Raise");
                        }
                    }
                }
                else if (GetRoundMoney() < StayInMoney)
                {
                    if (AllIn)
                    {
                        while (PossibleChoices.Contains("Fold"))
                        {
                            PossibleChoices.Remove("Fold");
                        }

                        PossibleChoices.Add("All In2");
                    }
                    else
                    {
                        for (int i = 0; i < AllInChance; i++)
                        {
                            PossibleChoices.Add("All In");
                        }
                    }

                }
                return PossibleChoices;
            }

            public override string[] CalculateValues(double StayInMoney, double TotalPot, Random rnd, List<string> PossibleChoices)
            {
                double Difference = StayInMoney - GetRoundMoney();
                string[] TurnDetails = new string[2];
                string choice = PossibleChoices[rnd.Next(0, PossibleChoices.Count)];
                TurnDetails[0] = choice;

                if (choice == "Fold")
                {
                    ModifyFolded(true);
                    TurnDetails[1] = "0";
                }
                else if (choice == "Check")
                {
                    TurnDetails[1] = "0";
                }
                else if (choice == "Call")
                {
                    TurnDetails[1] = Convert.ToString(Difference);
                    ModifyMoney(Difference * -1);
                }
                else if (choice == "Raise")
                {
                    double RaiseAmount = Difference;
                    double PercentageRaise;

                    PercentageRaise = PercentageRaise = BetAggressiveness[rnd.Next(0, BetAggressiveness.Length)];
                    RaiseAmount += Math.Round((GetMoney() - Difference) * (PercentageRaise / 100), 2);
                    ModifyMoney(RaiseAmount * -1);
                    TurnDetails[1] = Convert.ToString(RaiseAmount);
                }
                else if (choice == "All In")
                {
                    TurnDetails[1] = Convert.ToString(GetMoney());
                    ModifyMoney(GetMoney() * -1);
                    AllIn = true;
                }
                else if (choice == "All In2")
                {
                    TurnDetails[1] = "0";
                    TurnDetails[0] = "All In";
                }
                return TurnDetails;
            }
        }

        public class HardOpponent : Opponent
        {
            //Bet aggressiveness on a scale of 1-100
            int[] BetAggressiveness = {20, 20, 20, 20, 20, 20, 25, 25, 25, 25, 25, 25, 30, 30, 30, 30, 30, 30, 35, 35, 35, 35, 35, 35, 35, 40, 40, 40, 40, 50, 50, 50, 55, 55, 60, 60, 65, 65, 70, 75};

            //represents the number of times the following moves will appear in the final list of choices to make
            int CheckChance = 3;
            int FoldChance = 1;
            int CallChance = 4;
            int RaiseChance = 3;
            int AllInChance = 3;

            public HardOpponent(string name, double money, int difficulty, int spritenumber, int opponentnumber) : base(name, money, difficulty, spritenumber, opponentnumber)
            {
                Name = name;
                Money = money;
                Difficulty = difficulty;
                SpriteNumber = spritenumber;
                OpponentNumber = opponentnumber;
            }

            public override List<string> ChooseMove(double StayInMoney, double TotalPot, Random rnd)
            {
                List<string> PossibleChoices = new List<string>();
                double Difference = StayInMoney - GetRoundMoney();

                for (int i = 0; i < FoldChance; i++)
                {
                    PossibleChoices.Add("Fold");
                }



                //can check?
                if (GetRoundMoney() == StayInMoney)
                {
                    for (int i = 0; i < CheckChance; i++)
                    {
                        PossibleChoices.Add("Check");
                    }


                    if (GetMoney() >= 1)
                    {
                        for (int i = 0; i < RaiseChance; i++)
                        {
                            PossibleChoices.Add("Raise");
                        }
                    }
                }
                else if (GetRoundMoney() < StayInMoney && GetMoney() >= Difference)
                {
                    for (int i = 0; i < CallChance; i++)
                    {
                        PossibleChoices.Add("Call");
                    }


                    if (GetMoney() - Difference > 0)
                    {
                        for (int i = 0; i < RaiseChance; i++)
                        {
                            PossibleChoices.Add("Raise");
                        }
                    }
                }
                else if (GetRoundMoney() < StayInMoney)
                {
                    if (AllIn)
                    {
                        while (PossibleChoices.Contains("Fold"))
                        {
                            PossibleChoices.Remove("Fold");
                        }

                        PossibleChoices.Add("All In2");
                    }
                    else
                    {
                        for (int i = 0; i < AllInChance; i++)
                        {
                            PossibleChoices.Add("All In");
                        }
                    }

                }
                return PossibleChoices;
            }

            public override string[] CalculateValues(double StayInMoney, double TotalPot, Random rnd, List<string> PossibleChoices)
            {
                double Difference = StayInMoney - GetRoundMoney();
                string[] TurnDetails = new string[2];
                string choice = PossibleChoices[rnd.Next(0, PossibleChoices.Count)];
                TurnDetails[0] = choice;

                if (choice == "Fold")
                {
                    ModifyFolded(true);
                    TurnDetails[1] = "0";
                }
                else if (choice == "Check")
                {
                    TurnDetails[1] = "0";
                }
                else if (choice == "Call")
                {
                    TurnDetails[1] = Convert.ToString(Difference);
                    ModifyMoney(Difference * -1);
                }
                else if (choice == "Raise")
                {
                    double RaiseAmount = Difference;
                    double PercentageRaise;

                    PercentageRaise = PercentageRaise = BetAggressiveness[rnd.Next(0, BetAggressiveness.Length)];
                    RaiseAmount += Math.Round((GetMoney() - Difference) * (PercentageRaise / 100), 2);
                    ModifyMoney(RaiseAmount * -1);
                    TurnDetails[1] = Convert.ToString(RaiseAmount);
                }
                else if (choice == "All In")
                {
                    TurnDetails[1] = Convert.ToString(GetMoney());
                    ModifyMoney(GetMoney() * -1);
                    AllIn = true;
                }
                else if (choice == "All In2")
                {
                    TurnDetails[1] = "0";
                    TurnDetails[0] = "All In";
                }
                return TurnDetails;
            }
        }
    }

    
}
