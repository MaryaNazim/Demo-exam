using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_exam
{
    public partial class AddEmploy : Form
    {
        //Переменная соединения
        MySqlConnection conn;

        public AddEmploy()
        {
            InitializeComponent();
        }

        private void buttonAddEmployee_Click(object sender, EventArgs e)
        {
            // строка подключения к БД
            string connStr = "server=localhost;port=3306;user=maryanazim;database=demo;password=48985588John;";
            // создаём объект для подключения к БД
            conn = new MySqlConnection(connStr);

            string tmp = $"INSERT INTO employees (fio, phone_number, status, login, password, role) VALUES ('{textBox1.Text}', '{textBox2.Text}', 'работает', '{textBox5.Text}', '{textBox6.Text}','{textBox4.Text}')";
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
            }

            //Формируем запрос на вставку с возвратом последного вставленного ID
            string sql_update_current_empl = $"INSERT INTO `users` (fio, login, password, role) " +
                                              $"VALUES ('{textBox1.Text}','{textBox5.Text}', '{textBox6.Text}', '{textBox4.Text}'); " +
                                              $"SELECT id_empl FROM employees WHERE (id_empl = LAST_INSERT_ID());";
            // устанавливаем соединение с БД
            conn.Open();
            // объект для выполнения SQL-запроса
            MySqlCommand command = new MySqlCommand(sql_update_current_empl, conn);
            // выполняем запрос
            string new_inserted_empl_id = command.ExecuteScalar().ToString();
            //Записываем возвращённый последний добавленный ID заказа в глобальную переменную
            SomeClass.new_inserted_Employ_id = new_inserted_empl_id;

            // закрываем подключение к БД
            conn.Close();

            this.Close();
        }
    }
}
