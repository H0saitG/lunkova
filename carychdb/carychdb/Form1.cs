using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace carychdb
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }


        static public string loginActive;
        static public string whois;


        //Авторизация
        private void button1_Click(object sender, EventArgs e)
        {
            
            String loginUser = loginField.Text;
            String passUser = passField.Text;

            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM users WHERE login = @uL AND pass = @uP", db.GetConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginUser;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = passUser;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            
            if(table.Rows.Count > 0)
            {
                if (loginUser == "admin" && passUser == "admin")
                {
                    MessageBox.Show("Вход выполнен успешно!", "Успех", MessageBoxButtons.OK);
                    this.Hide();
                    Form2 form2 = new Form2();
                    form2.Show();
                }
                else
                {
                    MessageBox.Show("Вход выполнен успешно", "Успех", MessageBoxButtons.OK);
                    this.Hide();
                    UserForm form = new UserForm();
                    form.Show();
                }

            }
            else
            {
                MessageBox.Show("Такого аккаунта не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            

        }

        //Не трогать
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void buttonExit_Click(object sender, EventArgs e)
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

        private void Form1_Load(object sender, EventArgs e)
        {
            DB db = new DB();
            db.GetConnection();
        }
    }
}
