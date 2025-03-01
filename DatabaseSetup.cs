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
    public partial class DatabaseSetup : Form
    {
        public DatabaseSetup()
        {
            InitializeComponent();
        }

        private void DatabaseSetup_Load(object sender, EventArgs e)
        {
            DatabaseUtils.CreateDatabase();
            this.Hide();
            Form L = new Login();
            L.ShowDialog();
            this.Close();
        }
    }
}
