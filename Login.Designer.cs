namespace NEA_Computer_Science_Poker__️__️__️__️
{
    partial class Login
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
            this.LoginButton = new System.Windows.Forms.Button();
            this.LoginUser = new System.Windows.Forms.TextBox();
            this.LoginPass = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CreateAccountButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.CreatePass = new System.Windows.Forms.TextBox();
            this.CreateUser = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoginButton
            // 
            this.LoginButton.Location = new System.Drawing.Point(49, 113);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(94, 30);
            this.LoginButton.TabIndex = 1;
            this.LoginButton.Text = "Login";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // LoginUser
            // 
            this.LoginUser.Location = new System.Drawing.Point(61, 42);
            this.LoginUser.MaxLength = 20;
            this.LoginUser.Name = "LoginUser";
            this.LoginUser.Size = new System.Drawing.Size(125, 20);
            this.LoginUser.TabIndex = 3;
            // 
            // LoginPass
            // 
            this.LoginPass.Location = new System.Drawing.Point(61, 78);
            this.LoginPass.MaxLength = 20;
            this.LoginPass.Name = "LoginPass";
            this.LoginPass.PasswordChar = '*';
            this.LoginPass.Size = new System.Drawing.Size(125, 20);
            this.LoginPass.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.LoginButton);
            this.panel1.Controls.Add(this.LoginPass);
            this.panel1.Controls.Add(this.LoginUser);
            this.panel1.Location = new System.Drawing.Point(23, 85);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(198, 156);
            this.panel1.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Username:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Login";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.CreateAccountButton);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.CreatePass);
            this.panel2.Controls.Add(this.CreateUser);
            this.panel2.Location = new System.Drawing.Point(254, 85);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(198, 156);
            this.panel2.TabIndex = 8;
            // 
            // CreateAccountButton
            // 
            this.CreateAccountButton.Location = new System.Drawing.Point(49, 113);
            this.CreateAccountButton.Name = "CreateAccountButton";
            this.CreateAccountButton.Size = new System.Drawing.Size(94, 30);
            this.CreateAccountButton.TabIndex = 9;
            this.CreateAccountButton.Text = "Create Account";
            this.CreateAccountButton.UseVisualStyleBackColor = true;
            this.CreateAccountButton.Click += new System.EventHandler(this.CreateAccountButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Password:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Username:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(46, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Create An Account";
            // 
            // CreatePass
            // 
            this.CreatePass.Location = new System.Drawing.Point(61, 78);
            this.CreatePass.MaxLength = 20;
            this.CreatePass.Name = "CreatePass";
            this.CreatePass.PasswordChar = '*';
            this.CreatePass.Size = new System.Drawing.Size(125, 20);
            this.CreatePass.TabIndex = 4;
            // 
            // CreateUser
            // 
            this.CreateUser.Location = new System.Drawing.Point(61, 42);
            this.CreateUser.MaxLength = 20;
            this.CreateUser.Name = "CreateUser";
            this.CreateUser.Size = new System.Drawing.Size(125, 20);
            this.CreateUser.TabIndex = 3;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 397);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Login";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.TextBox LoginUser;
        private System.Windows.Forms.TextBox LoginPass;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox CreatePass;
        private System.Windows.Forms.TextBox CreateUser;
        private System.Windows.Forms.Button CreateAccountButton;
    }
}