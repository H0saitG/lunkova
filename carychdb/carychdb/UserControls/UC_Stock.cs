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

namespace carychdb.UserControls
{

    enum RowState { 
        
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    
    }

    public partial class UC_Stock : UserControl
    {

        DB db = new DB();

        int selectedRow;

        public UC_Stock()
        {
            InitializeComponent();
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id", "ID");
            dataGridView1.Columns.Add("name", "Название");
            dataGridView1.Columns.Add("ven_code", "Артикул");
            dataGridView1.Columns.Add("quantity", "Количество");
            dataGridView1.Columns.Add("price", "Цена");
            dataGridView1.Columns.Add("type", "Тип товара");
            dataGridView1.Columns.Add("IsNew", String.Empty);
        }

        private void ClearFields()
        {
            textBox_id.Text = "";
            textBoxName.Text = "";
            textBoxVen.Text = "";
            textBoxQuantity.Text = "";
            textBoxPrice.Text = "";
            textBoxType.Text = "";
        }

        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetInt32(4), record.GetString(5), RowState.ModifiedNew);
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select * from stock";

            MySqlCommand command = new MySqlCommand(queryString, db.GetConnection());

            db.openConnection();

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
        }

        //Не трогать
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void UC_Stock_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0 ) {

                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                textBox_id.Text = row.Cells[0].Value.ToString();
                textBoxName.Text  = row.Cells[1].Value.ToString();
                textBoxVen.Text = row.Cells[2].Value.ToString();
                textBoxQuantity.Text = row.Cells[3].Value.ToString();
                textBoxPrice.Text = row.Cells[4].Value.ToString();
                textBoxType.Text = row.Cells[5].Value.ToString();

            }
        }


        private void buttonNewEntry_Click(object sender, EventArgs e)
        {
            Add_Form addfrm = new Add_Form();
            addfrm.Show();
        }

        //Поиск
        private void Search(DataGridView dgw) {
        
            dgw.Rows.Clear();

            string searchString = $"select * from stock where concat (id, name, ven_code, quantity, price, type) like '%" + textBoxSearch.Text + "%'";

            MySqlCommand com = new MySqlCommand(searchString, db.GetConnection());

            db.openConnection();

            MySqlDataReader read = com.ExecuteReader();

            while(read.Read())
            {
                ReadSingleRow(dgw, read);
            }

            read.Close();
        
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;
            dataGridView1.Rows[index].Cells[6].Value = RowState.Deleted;

        }

        private void Updated()
        {
            db.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                if (dataGridView1.Rows[index].Cells[6].Value != null)
                {
                    var rowState = (RowState)dataGridView1.Rows[index].Cells[6].Value;

                    if (rowState == RowState.Existed)
                        continue;

                    if (rowState == RowState.Deleted)
                    {

                        var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                        var deleteQuery = $"delete from stock where id = {id}";

                        var command = new MySqlCommand(deleteQuery, db.GetConnection());
                        command.ExecuteNonQuery();

                    }

                    if (rowState == RowState.Modified)
                    {

                        var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                        var name = dataGridView1.Rows[index].Cells[1].Value.ToString();
                        var ven_code = dataGridView1.Rows[index].Cells[2].Value.ToString();
                        var quantity = dataGridView1.Rows[index].Cells[3].Value.ToString();
                        var price = dataGridView1.Rows[index].Cells[4].Value.ToString();
                        var type = dataGridView1.Rows[index].Cells[5].Value.ToString();

                        var changeQuery = $"update stock set name = '{name}', ven_code = '{ven_code}', quantity = '{quantity}', price = '{price}', type = '{type}' where id = '{id}'";

                        var command = new MySqlCommand(changeQuery, db.GetConnection());
                        command.ExecuteNonQuery();

                    }
                }

            }

            db.closeConnection();

        }

        //Удаление
        private void buttonDelete_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить эту запись?", "Подтвердите действие", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                deleteRow();
                ClearFields();
            }
            else if (dialogResult == DialogResult.No)
            {
                
            }


        }

        //Сохранить
        private void buttonSave_Click(object sender, EventArgs e)
        {
            Updated();
        }

        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var id = textBox_id.Text;
            var name = textBoxName.Text;
            var ven_code = textBoxVen.Text;
            var quantity = textBoxQuantity.Text;
            int price;
            var type = textBoxType.Text;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if(int.TryParse(textBoxPrice.Text, out price))
                {
                    dataGridView1.Rows[selectedRowIndex].SetValues(id, name, ven_code, quantity, price, type);
                    dataGridView1.Rows[selectedRowIndex].Cells[6].Value = RowState.Modified;
                }
                else
                {
                    MessageBox.Show("Цена должна иметь числовой формат!");
                }
            }
        }

        //Изменить
        private void buttonChange_Click(object sender, EventArgs e)
        {
            Change();
            ClearFields();
        }

        //Обновление таблицы
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
            ClearFields();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
    }
}
