using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Demo_exam
{
    public partial class FormAdmin : Form
    {
        //Переменная соединения
        MySqlConnection conn;
        //DataAdapter представляет собой объект Command , получающий данные из источника данных.
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        //Объявление BindingSource, основная его задача, это обеспечить унифицированный доступ к источнику данных.
        private BindingSource bSource = new BindingSource();
        //DataSet - расположенное в оперативной памяти представление данных, обеспечивающее согласованную реляционную программную 
        //модель независимо от источника данных.DataSet представляет полный набор данных, включая таблицы, содержащие, упорядочивающие 
        //и ограничивающие данные, а также связи между таблицами.
        private DataSet ds = new DataSet();
        //Представляет одну таблицу данных в памяти.
        private DataTable table1 = new DataTable();

        //Представляет одну таблицу данных в памяти.
        private DataTable table2 = new DataTable();

        //Представляет одну таблицу данных в памяти.
        private DataTable table3 = new DataTable();

        //Переменная для ID записи в БД, выбранной в гриде. Пока она не содердит значения, лучше его инициализировать с 0
        //что бы в БД не отправлялся null
        string id_selected_rows = "0";
        public FormAdmin()
        {
            InitializeComponent();
        }

        private void сотрудникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonStatusEmpl.Visible = true;
            buttonAddEmpl.Visible = true;
            dataGridView1.Visible = true;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            label1.Visible = false;
            textBox1.Visible = false;
            dateTimePicker1.Visible = false;
            buttonAddWork.Visible = false;

            //Чистим виртуальную таблицу
            table1.Clear();
            //Вызываем метод для заполнение дата Грида
            GetListEmployees();
            //Видимость полей в гриде
            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[2].Visible = true;
            dataGridView1.Columns[3].Visible = true;
            //Ширина полей
            dataGridView1.Columns[0].FillWeight = 15;
            dataGridView1.Columns[1].FillWeight = 40;
            dataGridView1.Columns[2].FillWeight = 15;
            dataGridView1.Columns[3].FillWeight = 15;
            //Режим для полей "Только для чтения"
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            //Растягивание полей грида
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //Убираем заголовки строк
            dataGridView1.RowHeadersVisible = false;
            //Показываем заголовки столбцов
            dataGridView1.ColumnHeadersVisible = true;
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            dateTimePicker1.Visible = false;
            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            buttonAddWork.Visible = false;
            buttonAddEmpl.Visible = false;
            buttonStatusEmpl.Visible = false;
            label1.Visible = false;
            // строка подключения к БД
            string connStr = "server=localhost;port=3306;user=maryanazim;database=demo;password=48985588John;";
            // создаём объект для подключения к БД
            conn = new MySqlConnection(connStr);
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
            dataGridView1.CurrentRow.Selected = true;
            GetSelectedIDString1();
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!e.RowIndex.Equals(-1) && !e.ColumnIndex.Equals(-1) && e.Button.Equals(MouseButtons.Right))
            {
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
                dataGridView1.CurrentRow.Selected = true;
                dataGridView1.CurrentCell.Selected = true;
                GetSelectedIDString1();
            }
        }

        //Метод обновления DataGreed
        public void reloadEmpl_list()
        {
            //Чистим виртуальную таблицу
            table1.Clear();
            //Вызываем метод получения записей, который вновь заполнит таблицу
            GetListEmployees();
        }

        //Метод наполнения виртуальной таблицы и присвоение её к датагриду
        public void GetListEmployees()
        {
            //Запрос для вывода строк в БД
            string commandStr = "SELECT id_empl AS 'Код', fio AS 'ФИО', phone_number AS 'Номер телефона',status AS 'Статус' FROM employees";
            //Открываем соединение
            conn.Open();
            //Объявляем команду, которая выполнить запрос в соединении conn
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            //Заполняем таблицу записями из БД
            MyDA.Fill(table1);
            //Указываем, что источником данных в bindingsource является заполненная выше таблица
            bSource.DataSource = table1;
            //Указываем, что источником данных ДатаГрида является bindingsource 
            dataGridView1.DataSource = bSource;
            //Закрываем соединение
            conn.Close();
        }

        //Метод получения ID выделенной строки, для последующего вызова его в нужных методах
        public void GetSelectedIDString1()
        {
            //Переменная для индекс выбранной строки в гриде
            string index_selected_rows;
            //Индекс выбранной строки
            index_selected_rows = dataGridView1.SelectedCells[0].RowIndex.ToString();
            //ID конкретной записи в Базе данных, на основании индекса строки
            id_selected_rows = dataGridView1.Rows[Convert.ToInt32(index_selected_rows)].Cells[0].Value.ToString();
        }

        public void GetSelectedIDString2()
        {
            //Переменная для индекс выбранной строки в гриде
            string index_selected_rows;
            //Индекс выбранной строки
            index_selected_rows = dataGridView2.SelectedCells[0].RowIndex.ToString();
            //ID конкретной записи в Базе данных, на основании индекса строки
            id_selected_rows = dataGridView2.Rows[Convert.ToInt32(index_selected_rows)].Cells[0].Value.ToString();
        }

        public void GetSelectedIDString3()
        {
            //Переменная для индекс выбранной строки в гриде
            string index_selected_rows;
            //Индекс выбранной строки
            index_selected_rows = dataGridView3.SelectedCells[0].RowIndex.ToString();
            //ID конкретной записи в Базе данных, на основании индекса строки
            id_selected_rows = dataGridView3.Rows[Convert.ToInt32(index_selected_rows)].Cells[0].Value.ToString();
        }

        private void сменыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            dataGridView3.Visible = true;
            label1.Visible = true;
            textBox1.Visible = true;
            dateTimePicker1.Visible = true;
            buttonAddWork.Visible = true;
            buttonStatusEmpl.Visible = false;
            buttonAddEmpl.Visible = false;

            //Чистим виртуальную таблицу
            table3.Clear();
            //Вызываем метод для заполнение дата Грида
            GetListWork();
            //Видимость полей в гриде
            dataGridView3.Columns[0].Visible = true;
            dataGridView3.Columns[1].Visible = true;
            dataGridView3.Columns[2].Visible = true;
            //Ширина полей
            dataGridView3.Columns[0].FillWeight = 15;
            dataGridView3.Columns[1].FillWeight = 40;
            dataGridView3.Columns[2].FillWeight = 15;
            //Режим для полей "Только для чтения"
            dataGridView3.Columns[0].ReadOnly = true;
            dataGridView3.Columns[1].ReadOnly = true;
            dataGridView3.Columns[2].ReadOnly = true;
            //Растягивание полей грида
            dataGridView3.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView3.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView3.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //Убираем заголовки строк
            dataGridView3.RowHeadersVisible = false;
            //Показываем заголовки столбцов
            dataGridView3.ColumnHeadersVisible = true;
        }

        //Метод наполнения виртуальной таблицы и присвоение её к датагриду
        public void GetListWork()
        {
            //Запрос для вывода строк в БД
            string commandStr = "SELECT id AS 'Код', id_empl AS 'Код сотрудника', datetime AS 'Дата и время' FROM work";
            //Открываем соединение
            conn.Open();
            //Объявляем команду, которая выполнить запрос в соединении conn
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            //Заполняем таблицу записями из БД
            MyDA.Fill(table3);
            //Указываем, что источником данных в bindingsource является заполненная выше таблица
            bSource.DataSource = table3;
            //Указываем, что источником данных ДатаГрида является bindingsource 
            dataGridView3.DataSource = bSource;
            //Закрываем соединение
            conn.Close();
        }

        private void buttonAddWork_Click(object sender, EventArgs e)
        {
            string tmp = $"INSERT INTO work (id_empl, datetime) VALUES ('{Convert.ToInt32(textBox1.Text)}', '{dateTimePicker1.Value.ToString("yyyy-MM-dd hh:mm:ss")}')";
            MySqlCommand cmd = new MySqlCommand(tmp, conn);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
                MessageBox.Show("Добавление прошло успешно", "Информация");
                reloadWork_list();
            }
        }

        //Метод обновления DataGreed
        public void reloadWork_list()
        {
            //Чистим виртуальную таблицу
            table3.Clear();
            //Вызываем метод получения записей, который вновь заполнит таблицу
            GetListWork();
        }

        //Метод обновления DataGreed
        public void reloadOrder_list()
        {
            //Чистим виртуальную таблицу
            table2.Clear();
            //Вызываем метод получения записей, который вновь заполнит таблицу
            GetListOrders();
        }

        private void заказыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            dataGridView2.Visible = true;
            dataGridView3.Visible = false;
            label1.Visible = false;
            textBox1.Visible = false;
            dateTimePicker1.Visible = false;
            buttonAddWork.Visible = false;
            buttonStatusEmpl.Visible = false;
            buttonAddEmpl.Visible = false;

            //Чистим виртуальную таблицу
            table2.Clear();
            //Вызываем метод для заполнение дата Грида
            GetListOrders();
            //Видимость полей в гриде
            dataGridView2.Columns[0].Visible = true;
            dataGridView2.Columns[1].Visible = true;
            dataGridView2.Columns[2].Visible = true;
            dataGridView2.Columns[3].Visible = true;
            dataGridView2.Columns[4].Visible = true;
            dataGridView2.Columns[5].Visible = true;
            //Ширина полей
            dataGridView2.Columns[0].FillWeight = 15;
            dataGridView2.Columns[1].FillWeight = 15;
            dataGridView2.Columns[2].FillWeight = 15;
            dataGridView2.Columns[3].FillWeight = 40;
            dataGridView2.Columns[4].FillWeight = 15;
            dataGridView2.Columns[5].FillWeight = 15;
            //Режим для полей "Только для чтения"
            dataGridView2.Columns[0].ReadOnly = true;
            dataGridView2.Columns[1].ReadOnly = true;
            dataGridView2.Columns[2].ReadOnly = true;
            dataGridView2.Columns[3].ReadOnly = true;
            dataGridView2.Columns[4].ReadOnly = true;
            dataGridView2.Columns[5].ReadOnly = true;
            //Растягивание полей грида
            dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //Убираем заголовки строк
            dataGridView2.RowHeadersVisible = false;
            //Показываем заголовки столбцов
            dataGridView2.ColumnHeadersVisible = true;
        }

        //Метод наполнения виртуальной таблицы и присвоение её к датагриду
        public void GetListOrders()
        {
            //Запрос для вывода строк в БД
            string commandStr = "SELECT id_order AS 'Код', number_table AS 'Номер столика', count AS 'Количество', description AS 'Продукты и напитки', status_waiter AS 'Статус заказа(официант)', status_kitchen AS 'Статус заказа(кухня)' FROM orders";
            //Открываем соединение
            conn.Open();
            //Объявляем команду, которая выполнить запрос в соединении conn
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            //Заполняем таблицу записями из БД
            MyDA.Fill(table2);
            //Указываем, что источником данных в bindingsource является заполненная выше таблица
            bSource.DataSource = table2;
            //Указываем, что источником данных ДатаГрида является bindingsource 
            dataGridView2.DataSource = bSource;
            //Закрываем соединение
            conn.Close();
        }

        private void buttonAddEmpl_Click(object sender, EventArgs e)
        {
            //Инициализируем и вызываем форму добавления сотрудника
            AddEmploy addEmploy = new AddEmploy();
            //Вызов формы в режиме диалога
            addEmploy.ShowDialog();
            reloadEmpl_list();
        }

        private void buttonStatusEmpl_Click(object sender, EventArgs e)
        {
            //Формируем строку запроса на добавление строк
            string tmp = $"UPDATE employees SET status = ('уволен') WHERE id_empl = '" + id_selected_rows + "'";
            //Посылаем запрос на обновление данных
            MySqlCommand insertlink = new MySqlCommand(tmp, conn);
            try
            {
                conn.Open();
                insertlink.ExecuteNonQuery();
                MessageBox.Show("Сотрудник уволен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка \n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                conn.Close();
                reloadEmpl_list();
            }
        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView2.CurrentCell = dataGridView2[e.ColumnIndex, e.RowIndex];
            dataGridView2.CurrentRow.Selected = true;
            GetSelectedIDString2();
        }

        private void dataGridView2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!e.RowIndex.Equals(-1) && !e.ColumnIndex.Equals(-1) && e.Button.Equals(MouseButtons.Right))
            {
                dataGridView2.CurrentCell = dataGridView2[e.ColumnIndex, e.RowIndex];
                dataGridView2.CurrentRow.Selected = true;
                dataGridView2.CurrentCell.Selected = true;
                GetSelectedIDString2();
            }
        }

        private void dataGridView3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView3.CurrentCell = dataGridView3[e.ColumnIndex, e.RowIndex];
            dataGridView3.CurrentRow.Selected = true;
            GetSelectedIDString3();
        }

        private void dataGridView3_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!e.RowIndex.Equals(-1) && !e.ColumnIndex.Equals(-1) && e.Button.Equals(MouseButtons.Right))
            {
                dataGridView3.CurrentCell = dataGridView3[e.ColumnIndex, e.RowIndex];
                dataGridView3.CurrentRow.Selected = true;
                dataGridView3.CurrentCell.Selected = true;
                GetSelectedIDString3();
            }
        }
    }
}
