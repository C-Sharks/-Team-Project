using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Interpol
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// Имя класса модели базы данных энтити "InterpolDataBaseEntities1" !!!
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Title = $"{this.Width} | {this.Height}";

            Window1 window1 = new Window1();
            window1.Show();
        }

        protected Users IsPass(string login, string pas)//проверка логина/пароля
        {
            using (InterpolDataBaseEntities1 DbModel = new InterpolDataBaseEntities1())
            {
                byte[] data = Encoding.ASCII.GetBytes(pas);
                SHA512 shaM = new SHA512Managed();
                var result = shaM.ComputeHash(data);
                var strPas = Encoding.ASCII.GetString(result);
                Users obj = DbModel.Users.Where(p => p.Login == login && p.Password == strPas).FirstOrDefault();
                return obj;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //AddUser(login.Text, password.Password);
                btnLogin.IsEnabled = false;
                var user = IsPass(login.Text, password.Password);
                if (user!=null)
                {
                    Window1 window1 = new Window1(user);
                    window1.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный пароль!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                btnLogin.IsEnabled = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { btnLogin_Click(sender, e); }  
        }
    }
}
