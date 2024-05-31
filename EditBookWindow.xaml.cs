using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для EditBookWindow.xaml
    /// </summary>
    public partial class EditBookWindow : Window
    {
        private Book book;
        private string connectionString;

        public EditBookWindow(Book book)
        {
            InitializeComponent();
            this.book = book;
            connectionString = ConfigurationManager.ConnectionStrings["BooksDB"].ConnectionString;
            InitializeBookData();
        }

        private void InitializeBookData()
        {
            TitleTextBox.Text = book.Title;
            PriceTextBox.Text = book.Price.ToString();
            DescriptionTextBox.Text = book.Description;
            ImagePathTextBox.Text = book.ImagePath;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT Id, Name FROM Genres";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var genre = new ComboBoxItem
                        {
                            Content = reader.GetString(1),
                            Tag = reader.GetInt32(0)
                        };
                        GenreComboBox.Items.Add(genre);

                        if (book.Genre == reader.GetString(1))
                        {
                            GenreComboBox.SelectedItem = genre;
                        }
                    }
                }
            }
        }

        private void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                ImagePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to save changes?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "UPDATE Books SET Title = @Title, Price = @Price, Description = @Description, ImagePath = @ImagePath, GenreId = @GenreId WHERE Id = @Id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", book.Id);
                        command.Parameters.AddWithValue("@Title", TitleTextBox.Text);
                        command.Parameters.AddWithValue("@Price", decimal.Parse(PriceTextBox.Text));
                        command.Parameters.AddWithValue("@Description", DescriptionTextBox.Text);
                        command.Parameters.AddWithValue("@ImagePath", ImagePathTextBox.Text);
                        command.Parameters.AddWithValue("@GenreId", ((ComboBoxItem)GenreComboBox.SelectedItem).Tag);

                        command.ExecuteNonQuery();
                    }
                }
                var homeWindow = new HomeWindow();
                homeWindow.Show();
                this.Close();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var homeWindow = new HomeWindow();
            homeWindow.Show();
            this.Close();
        }
    }
}
