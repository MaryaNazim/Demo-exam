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

namespace Demo_exam
{
    public partial class FormAuth : Form
    {
        public FormAuth()
        {
            InitializeComponent();
        }

        //строка подключения к БД
        string connStr = "server=localhost;port=3306;user=maryanazim;database=demo;password=48985588John;";
        MySqlConnection conn;

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            //Запрос в БД на предмет того, если ли строка с подходящим логином и паролем
            string sql = "SELECT * FROM `users` WHERE `login`=@un and `password`=@up";
            //Открытие соединения
            conn.Open();
            //Объявляем таблицу
            DataTable table = new DataTable();
            //Объявляем адаптер
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            //Объявляем команду
            MySqlCommand command = new MySqlCommand(sql, conn);
            //Объявляем параметры
            command.Parameters.Add("@un", MySqlDbType.VarChar, 255);
            command.Parameters.Add("@up", MySqlDbType.VarChar, 255);
            //Присваиваем параметрам значение
            command.Parameters["@un"].Value = textBox1.Text;
            command.Parameters["@up"].Value = textBox2.Text;
            //Заносим команду в адаптер
            adapter.SelectCommand = command;
            //Заполняем таблицу
            adapter.Fill(table);
            //Закрываем соединение
            conn.Close();
            //Если вернулась больше 0 строк, значит такой пользователь существует
            if (table.Rows.Count > 0)
            {
                //Присваеваем глобальный признак авторизации
                Auth.auth = true;
                //Достаем данные пользователя в случае успеха
                GetUserInfo(textBox1.Text);
                if (Auth.auth)
                {
                    MessageBox.Show("Авторизация успешна", "Информация");
                    //Вызываем метод управления ролями
                    ManagerRole(Auth.auth_role);
                }
                //Закрываем форму
                this.Close();
            }
            else
            {
                //Отобразить сообщение о том, что авторизаия неуспешна
                MessageBox.Show("Неверные данные авторизации", "Информация");
            }
        }

        //Метод запроса данных пользователя по логину для запоминания их в полях класса
        public void GetUserInfo(string login)
        {
            // устанавливаем соединение с БД
            conn.Open();
            // запрос
            string sql = $"SELECT * FROM `users` WHERE `login`='{login}'";
            // объект для выполнения SQL-запроса
            MySqlCommand command = new MySqlCommand(sql, conn);
            // объект для чтения ответа сервера
            MySqlDataReader reader = command.ExecuteReader();
            // читаем результат
            while (reader.Read())
            {
                // элементы массива [] - это значения столбцов из запроса SELECT
                Auth.auth_id = reader[0].ToString();
                Auth.auth_login = reader[2].ToString();
                Auth.auth_password = reader[3].ToString();
                Auth.auth_role = Convert.ToInt32(reader[4].ToString());
            }
            reader.Close(); // закрываем reader
            // закрываем соединение с БД
            conn.Close();
        }

        public void ManagerRole(int role)
        {
            switch (role)
            {
                //И в зависимости от того, какая роль (цифра) хранится в поле класса и передана в метод, показываются те или иные кнопки.
                case 1:
                    MessageBox.Show("Добро пожаловать, администратор");
                    //Инициализируем и вызываем форму администратора
                    FormAdmin formAdmin = new FormAdmin();
                    //Вызов формы в режиме диалога
                    formAdmin.ShowDialog();
                    break;
                case 2:
                    MessageBox.Show("Добро пожаловать, повар");
                    //Инициализируем и вызываем форму повара
                    FormKitcher formKitcher = new FormKitcher();
                    //Вызов формы в режиме диалога
                    formKitcher.ShowDialog();
                    break;
                case 3:
                    MessageBox.Show("Добро пожаловать, официант");
                    //Инициализируем и вызываем форму официанта
                    FormWaiter formWaiter = new FormWaiter();
                    //Вызов формы в режиме диалога
                    formWaiter.ShowDialog();
                    break;
                //Если по какой то причине в классе ничего не содержится, то всё отключается вообще
                default:
                    MessageBox.Show("Пользователь не найден");
                    break;
            }
        }

        private void FormAuth_Load(object sender, EventArgs e)
        {
            //Инициализируем соединение с подходящей строкой
            conn = new MySqlConnection(connStr);
            textBox2.PasswordChar = '●';
        }
    }
}
