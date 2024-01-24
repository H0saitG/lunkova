using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace carychdb
{
    public partial class Add_Form : Form
    {

        DB db = new DB();

        public Add_Form()
        {
            InitializeComponent();
        }

        private void buttonSaveEntry_Click(object sender, EventArgs e)
        {
            db.openConnection();

            var name = textBoxName.Text;
            var ven_code = textBoxVen.Text;
            var quantity = textBoxQuantity.Text;
            var type = textBoxType.Text;
            int price;

            if (int.TryParse(textBoxPrice.Text, out price)) {

                var addQuery = $"insert into stock (name, ven_code, quantity, price, type) values ('{name}', '{ven_code}', {quantity}, {price}, '{type}')";

                var command = new MySqlCommand(addQuery, db.GetConnection());
                command.ExecuteNonQuery();

                MessageBox.Show("Запись успешно создана!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            }
            else
            {
                MessageBox.Show("Цена должна иметь цифровой формат!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            db.closeConnection();

        }
    }
}
