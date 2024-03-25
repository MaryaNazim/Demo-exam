using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_exam
{
    static class Auth
    {
        //переменная авторизации
        public static bool auth = false;
        //переменная id пользователя
        public static string auth_id = null;
        //переменная логина
        public static string auth_login = null;
        //переменная пароля
        public static string auth_password = null;
        //переменная ФИО
        public static string auth_fio = null;
        //переменная роль
        public static int auth_role = 0;
    }

    //Класс для передачи значений между формами
    static class SomeClass
    {
        //Статичное поле, которое хранит значение для передачи его между формами
        public static string variable_class;
        //Статичное поле, которое хранит значения ID добавленного клиента на AddEmploy
        public static string new_inserted_id;
        //Статичное поле, которое хранит значение ID добаленного заказа
        public static string new_inserted_Employ_id;
    }
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormAuth());
        }
    }
}
