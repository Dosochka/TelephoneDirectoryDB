using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkingWithDB
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection;
        public static string path = "";
        public Form1()
        {
            InitializeComponent();


            comboBox1.Items.Add("Абоненты");
            comboBox1.Items.Add("Тип абонента");
            comboBox1.Items.Add("Тип поселения");

            comboBox2.Items.Add("Абоненты");
            comboBox3.Items.Add("Абоненты");
            comboBox4.Items.Add("Абоненты");

            comboBox5.Items.Add("1"); comboBox5.Items.Add("2");
            comboBox6.Items.Add("1"); comboBox6.Items.Add("2");
            comboBox7.Items.Add("1"); comboBox7.Items.Add("2");
            comboBox8.Items.Add("1"); comboBox8.Items.Add("2");


            comboBox1.Text = "Абоненты"; comboBox2.Text = "Абоненты"; comboBox3.Text = "Абоненты"; comboBox4.Text = "Абоненты";

        }

        private async void Form1_Load(object sender, EventArgs e) // Реализация таблицы и БД при загрузке формы
        {
            MessageBox.Show("Укажите путь до БД");
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                Application.Exit();

            path = openFileDialog1.FileName;

            // Работа с таблицами и БД
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="+ path + ";Integrated Security=True";

            sqlConnection = new SqlConnection(connectionString);

            await sqlConnection.OpenAsync();

            SqlDataReader sqlReader = null;

            SqlCommand command = new SqlCommand("SELECT * FROM [Abonents]", sqlConnection);

            try
            {
                sqlReader = await command.ExecuteReaderAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
            FirstTable();
            TopMost = true;
           

        }


        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed) 
                sqlConnection.Close();
            Application.Exit(); // Выход из программы

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
        }


        private async void button2_Click(object sender, EventArgs e)  // Реализация обновления элемента таблицы
        {
            label8.Visible = true;

            if (!string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox3.Text) &&
                !string.IsNullOrEmpty(comboBox5.Text) && !string.IsNullOrWhiteSpace(comboBox5.Text) &&
                !string.IsNullOrEmpty(comboBox6.Text) && !string.IsNullOrWhiteSpace(comboBox6.Text) &&
                !string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text))
            {

                if (comboBox2.Text == "Абоненты")
                {

                    SqlCommand command = new SqlCommand("BEGIN TRANSACTION UPDATE [Abonents] SET [AbonentType_ID]=@AbonentType_ID, [Name]=@Name, [Address]=@Address, [LocalityType_ID]=@LocalityType_ID, [Phone_Number]=@Phone_Number, [Department]=@Department WHERE [Id]=@Id IF (@@error <> 0) ROLLBACK COMMIT;", sqlConnection);

                    command.Parameters.AddWithValue("Id", comboBox9.Text);
                    command.Parameters.AddWithValue("AbonentType_ID", comboBox5.Text);
                    command.Parameters.AddWithValue("Name", textBox3.Text);
                    command.Parameters.AddWithValue("Address", textBox1.Text);
                    command.Parameters.AddWithValue("LocalityType_ID", comboBox6.Text);
                    command.Parameters.AddWithValue("Phone_Number", textBox8.Text);
                    command.Parameters.AddWithValue("Department", textBox2.Text);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        label9.Text = "Успех";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    FirstTable();

                    label8.Text = "Успех!";
                }
            }
            else
            {
                label8.Text = "Ошибка!";
            }
        }

        private async void button3_Click(object sender, EventArgs e) // Реализация удаления элемента из таблицы
        {
            label9.Visible = true;

            if (!string.IsNullOrEmpty(textBox6.Text) && !string.IsNullOrWhiteSpace(textBox6.Text))
            {
                 if (comboBox3.Text == "Абоненты")
                {
                    SqlCommand command = new SqlCommand("DELETE FROM [Abonents] WHERE [Id]=@Id;", sqlConnection);

                    command.Parameters.AddWithValue("Id", textBox6.Text);
                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        label9.Text = "Успех";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    FirstTable();

                    label9.Text = "Успех";
                }    

            }
            else
            {
                label9.Text = "Ошибка!";
            }
        }

        private void button4_Click(object sender, EventArgs e) // Вызов метода при нажатии кнопки
        {
            searchTable();
        }

        void searchTable() // Метод поиска значений в datagridview
        {
            int[] array1 = new int[100];
            int[] array2 = new int[100];

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Selected = false;
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                        if (dataGridView1.Rows[i].Cells[j].Value.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Contains(textBox7.Text))
                        {
                            dataGridView1.Rows[i].Selected = true;
                            array1[i] = i;
                            break;
                        }
            }

            if (radioButton2.Checked) 
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    dataGridView1.Rows[i].Selected = false;
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                        if (dataGridView1.Rows[i].Cells[j].Value != null)
                            if (dataGridView1.Rows[i].Cells[j].Value.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Contains(textBox14.Text))
                            {
                                array2[i] = i;
                                break;
                            }
                }
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {                   
                    if (array1[i] == array2[i] && array1[i] != 0 && array2[i] != 0)
                    {
                        dataGridView1.Rows[i].Selected = true;
                    }
                }
            }
        }
        async void FirstTable() // Вывод первой таблицы
        {
            string connectString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True";

            DataSet ds = new DataSet();
            SqlConnection myConnection = new SqlConnection(connectString);

            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Abonents", myConnection);

            string query = "SELECT * FROM [Abonents]";

            SqlCommand command = new SqlCommand(query, myConnection);

            if (path != "")
            {
                dataAdapter.Fill(ds, "Abonents");
                dataGridView1.DataSource = ds.Tables["Abonents"];
            }
            
        }

        async void SecondTable() // Вывод первой таблицы
        {
            string connectString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True";

            DataSet ds = new DataSet();
            SqlConnection myConnection = new SqlConnection(connectString);

            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM [AbonentType]", myConnection);

            string query = "SELECT * FROM [AbonentType]";

            SqlCommand command = new SqlCommand(query, myConnection);

            if (path != "")
            {
                dataAdapter.Fill(ds, "AbonentType");
                dataGridView1.DataSource = ds.Tables["AbonentType"];
            }

        }


        async void ThirdTable() // Вывод первой таблицы
        {
            string connectString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True";

            DataSet ds = new DataSet();
            SqlConnection myConnection = new SqlConnection(connectString);

            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM [LocalityType]", myConnection);

            string query = "SELECT * FROM [LocalityType]";

            SqlCommand command = new SqlCommand(query, myConnection);

            if (path != "")
            {
                dataAdapter.Fill(ds, "LocalityType");
                dataGridView1.DataSource = ds.Tables["LocalityType"];
            }

        }



        async void UpdateID()
        {
            int[] mass = new int[dataGridView1.RowCount-1];
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1[0, i].Value == null)
                {
                    break;
                }
                mass[i] = int.Parse(dataGridView1[0, i].Value.ToString());
            }


            foreach (var item in mass)
            {
                comboBox9.Items.Add(item);
            }
        }

        async void updateBase()
        {
            if (!string.IsNullOrEmpty(comboBox9.Text) && !string.IsNullOrWhiteSpace(comboBox9.Text))
            {
                FirstTable();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == comboBox9.Text)
                    {
                        row.Selected = true;
                    }

                    if (row.Cells[1].Selected == true) comboBox5.Text = row.Cells[1].Value.ToString();

                    if (row.Cells[2].Selected == true) textBox3.Text = row.Cells[2].Value.ToString();

                    if (row.Cells[3].Selected == true) textBox1.Text = row.Cells[3].Value.ToString();

                    if (row.Cells[4].Selected == true) comboBox6.Text = row.Cells[4].Value.ToString();

                    if (row.Cells[5].Selected == true) textBox8.Text = row.Cells[5].Value.ToString();

                    if (row.Cells[6].Selected == true) textBox2.Text = row.Cells[6].Value.ToString();
                }
            }
        }

        async private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) // Обновление таблиц при изменении combobox
        {
            if (comboBox1.Text == "Абоненты")
            {
                FirstTable();
            } else if (comboBox1.Text == "Тип абонента")
            {
                SecondTable();
            } else if (comboBox1.Text == "Тип поселения")
            {
                ThirdTable();
            }
                

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) // Замена label при изменении таблицы
        {

        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Для отображения нужной таблицы, выберите её в выпадающем меню экрана 'Таблица'. \nДля поиска какого-либо элемента в таблице, введите его в текстовое поле и нажмите 'Поиск'. \nДля добавления нового элемента в таблицу нажмите на кнопку 'Добавить', выберите таблицу и, заполнив текстовые окна, добавьте элемент. \nДля изменения таблицы нажмите на кнопку 'Обновить' и введите соответствующие параметры, перед этим выбрав таблицу. \nДля удаления нажмите на кнопку 'Удалить', выберите таблицу и введите id, после нажав на кнопку 'Удалить'.");

        }

        async private void button1_Click(object sender, EventArgs e) // Добавление в таблицу новых элементов
        {
            label17.Visible = true;

            if (!string.IsNullOrEmpty(textBox11.Text) && !string.IsNullOrWhiteSpace(textBox11.Text) &&
                !string.IsNullOrEmpty(textBox10.Text) && !string.IsNullOrWhiteSpace(textBox10.Text))
              {
                if (comboBox4.Text == "Абоненты")
                {
                    SqlCommand command = new SqlCommand("INSERT INTO [Abonents] (AbonentType_ID, Name, Address, LocalityType_ID, Phone_Number, Department) VALUES(@AbonentType_ID, @Name, @Address, @LocalityType_ID, @Phone_Number, @Department)", sqlConnection);

                    command.Parameters.AddWithValue("AbonentType_ID", comboBox8.Text);
                    command.Parameters.AddWithValue("Name", textBox11.Text);
                    command.Parameters.AddWithValue("Address", textBox10.Text);
                    command.Parameters.AddWithValue("LocalityType_ID", comboBox7.Text);
                    command.Parameters.AddWithValue("Phone_Number", textBox4.Text);
                    command.Parameters.AddWithValue("Department", textBox9.Text);
                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        label17.Text = "Успех!";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    FirstTable();

                }
            }
            else
            {               
                label17.Text = "Ошибка!";
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            textBox14.Visible = true; // Поиск по двум параметрам (текстовое окно)
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            textBox14.Visible = false; // Поиск по двум параметрам (текстовое окно)
        }


        private void button7_Click(object sender, EventArgs e)
        {
            string connectString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True";

            DataSet ds = new DataSet();
            SqlConnection myConnection = new SqlConnection(connectString);

            SqlDataAdapter dataAdapter = new SqlDataAdapter("BEGIN TRANSACTION COMMIT;", myConnection);

            string query = "BEGIN TRANSACTION COMMIT;";

            SqlCommand command = new SqlCommand(query, myConnection);

            dataAdapter.Fill(ds, "Abonents");
            dataGridView1.DataSource = ds.Tables["Abonents"];
            FirstTable();
            comboBox1.Text = "Абоненты";
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            if (str.Contains("."))
            {
                string s = str.Replace(".", ",");
                textBox1.Clear();
                textBox1.AppendText(str.Replace(".", ","));

            }
        }

        private void comboBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox8_Leave(object sender, EventArgs e)
        {

        }


        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateBase();
        }


        private void comboBox9_Click(object sender, EventArgs e)
        {
            comboBox9.Items.Clear();
            UpdateID();
        }
    }
}

