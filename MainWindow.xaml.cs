using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string filePath = "users.txt";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    if (parts.Length == 2 && parts[0] == login && parts[1] == password)
                    {
                        MessageBox.Show("Успешный вход", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Information);
                        OpenHomeWindow();
                        return;
                    }
                }
            }
            MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = RegLoginTextBox.Text;
            string password = RegPasswordBox.Password;
            string confirmPassword = RegConfirmPasswordBox.Password;

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                if (lines.Any(line => line.Split(';')[0] == login))
                {
                    MessageBox.Show("Логин уже используется", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine($"{login};{password}");
            }

            MessageBox.Show("Регистрация успешна", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы вошли как гость", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Information);
            OpenHomeWindow();
        }

        private void OpenHomeWindow()
        {
            HomeWindow homeWindow = new HomeWindow();
            homeWindow.Show();
            this.Close();
        }
    }
}