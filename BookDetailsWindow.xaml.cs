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
            LoadBookDetails();
        }

        private void LoadBookDetails()
        {
            BookImage.Source = new BitmapImage(new Uri(book.ImagePath, UriKind.Relative));
            BookTitle.Text = book.Title;
            BookGenre.Text = $"Genre: {book.Genre}";
            BookDescription.Text = book.Description;
            BookPrice.Text = $"Price: ${book.Price}";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
