
using carychdb.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace carychdb
{
    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();

            UC_Stock uc = new UC_Stock();
            addUserControl(uc);

        }

        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(userControl);
            userControl.BringToFront();

        }

        //Открытие склада
        private void guna2Button1_Click(object sender, EventArgs e)
        {
                UC_Stock uc = new UC_Stock();
                addUserControl(uc);
        }

        //Открытие формы Pred
        private void guna2Button2_Click(object sender, EventArgs e)
        {
                UC_Pred uc = new UC_Pred();
                addUserControl(uc);
        }

        //Выход из программы
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Вы действительно хотите выйти из программы?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (dialogResult == DialogResult.No)
            {

            }

        }
    }
}
