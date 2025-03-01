namespace NEA_Computer_Science_Poker__️__️__️__️
{
    partial class MainMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LargestPotWonLabel = new System.Windows.Forms.Label();
            this.HandsWonLabel = new System.Windows.Forms.Label();
            this.MoneyLabel = new System.Windows.Forms.Label();
            this.UsernameBankLabel = new System.Windows.Forms.Label();
            this.PlayButton = new System.Windows.Forms.Button();
            this.LeaderboardButton = new System.Windows.Forms.Button();
            this.Stats = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(507, 86);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "     ♠️ ♥️ POKER ♦️ ♣️     ";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LargestPotWonLabel);
            this.panel1.Controls.Add(this.HandsWonLabel);
            this.panel1.Controls.Add(this.MoneyLabel);
            this.panel1.Controls.Add(this.UsernameBankLabel);
            this.panel1.Location = new System.Drawing.Point(342, 171);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(519, 237);
            this.panel1.TabIndex = 4;
            // 
            // LargestPotWonLabel
            // 
            this.LargestPotWonLabel.AutoSize = true;
            this.LargestPotWonLabel.Location = new System.Drawing.Point(90, 169);
            this.LargestPotWonLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LargestPotWonLabel.Name = "LargestPotWonLabel";
            this.LargestPotWonLabel.Size = new System.Drawing.Size(200, 20);
            this.LargestPotWonLabel.TabIndex = 6;
            this.LargestPotWonLabel.Text = "Largest Pot Won: {amount}";
            // 
            // HandsWonLabel
            // 
            this.HandsWonLabel.AutoSize = true;
            this.HandsWonLabel.Location = new System.Drawing.Point(90, 126);
            this.HandsWonLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.HandsWonLabel.Name = "HandsWonLabel";
            this.HandsWonLabel.Size = new System.Drawing.Size(165, 20);
            this.HandsWonLabel.TabIndex = 8;
            this.HandsWonLabel.Text = "Hands Won: {amount}";
            // 
            // MoneyLabel
            // 
            this.MoneyLabel.AutoSize = true;
            this.MoneyLabel.Location = new System.Drawing.Point(90, 86);
            this.MoneyLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MoneyLabel.Name = "MoneyLabel";
            this.MoneyLabel.Size = new System.Drawing.Size(137, 20);
            this.MoneyLabel.TabIndex = 7;
            this.MoneyLabel.Text = "Money: ${amount}";
            // 
            // UsernameBankLabel
            // 
            this.UsernameBankLabel.AutoSize = true;
            this.UsernameBankLabel.Location = new System.Drawing.Point(186, 28);
            this.UsernameBankLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UsernameBankLabel.Name = "UsernameBankLabel";
            this.UsernameBankLabel.Size = new System.Drawing.Size(145, 20);
            this.UsernameBankLabel.TabIndex = 6;
            this.UsernameBankLabel.Text = "{Username}\'s Bank";
            // 
            // PlayButton
            // 
            this.PlayButton.Location = new System.Drawing.Point(483, 437);
            this.PlayButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(237, 117);
            this.PlayButton.TabIndex = 0;
            this.PlayButton.Text = "PLAY";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click_1);
            // 
            // LeaderboardButton
            // 
            this.LeaderboardButton.Location = new System.Drawing.Point(298, 468);
            this.LeaderboardButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LeaderboardButton.Name = "LeaderboardButton";
            this.LeaderboardButton.Size = new System.Drawing.Size(177, 54);
            this.LeaderboardButton.TabIndex = 5;
            this.LeaderboardButton.Text = "Leaderboards";
            this.LeaderboardButton.UseVisualStyleBackColor = true;
            this.LeaderboardButton.Click += new System.EventHandler(this.LeaderboardButton_Click);
            // 
            // Stats
            // 
            this.Stats.Location = new System.Drawing.Point(727, 468);
            this.Stats.Name = "Stats";
            this.Stats.Size = new System.Drawing.Size(178, 54);
            this.Stats.TabIndex = 6;
            this.Stats.Text = "Stats";
            this.Stats.UseVisualStyleBackColor = true;
            this.Stats.Click += new System.EventHandler(this.Stats_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.Stats);
            this.Controls.Add(this.LeaderboardButton);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainMenu";
            this.Text = "MainMenu";
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Button LeaderboardButton;
        private System.Windows.Forms.Label LargestPotWonLabel;
        private System.Windows.Forms.Label HandsWonLabel;
        private System.Windows.Forms.Label MoneyLabel;
        private System.Windows.Forms.Label UsernameBankLabel;
        private System.Windows.Forms.Button Stats;
    }
}

