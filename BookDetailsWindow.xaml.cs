using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для BookDetailsWindow.xaml
    /// </summary>
    public partial class BookDetailsWindow : Window
    {
        private Book book;

        public BookDetailsWindow(Book book)
        {
            InitializeComponent();
            this.book = book;
            InitializeBookData();
        }

        private void InitializeBookData()
        {
            TitleTextBlock.Text = book.Title;
            GenreTextBlock.Text = book.Genre;
            DescriptionTextBlock.Text = book.Description;
            PriceTextBlock.Text = $"${book.Price}";
            BookImage.Source = new BitmapImage(new Uri(book.ImagePath));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var homeWindow = new HomeWindow();
            homeWindow.Show();
            this.Close();
        }
    }
}
