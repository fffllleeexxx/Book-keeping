using Microsoft.Win32;
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
    /// Логика взаимодействия для EditBookWindow.xaml
    /// </summary>
    public partial class EditBookWindow : Window
    {
        private Book book;
        private string newImagePath;

        public EditBookWindow(Book book)
        {
            InitializeComponent();
            this.book = book;
            LoadBookDetails();
        }

        private void LoadBookDetails()
        {
            BookImage.Source = new BitmapImage(new Uri(book.ImagePath, UriKind.Relative));
            BookTitleTextBox.Text = book.Title;
            BookGenreTextBox.Text = book.Genre;
            BookDescriptionTextBox.Text = book.Description;
            BookPriceTextBox.Text = book.Price.ToString();
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                newImagePath = openFileDialog.FileName;
                BookImage.Source = new BitmapImage(new Uri(newImagePath));
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to save the changes?", "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                book.Title = BookTitleTextBox.Text;
                book.Genre = BookGenreTextBox.Text;
                book.Description = BookDescriptionTextBox.Text;
                book.Price = double.Parse(BookPriceTextBox.Text);

                if (!string.IsNullOrEmpty(newImagePath))
                {
                    book.ImagePath = newImagePath;
                }

                DialogResult = true;
                this.Close();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
