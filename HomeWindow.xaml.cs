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
    /// Логика взаимодействия для HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        private List<Book> allBooks;
        private List<Book> displayedBooks;

        public HomeWindow()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void LoadBooks()
        {
            allBooks = new List<Book>
            {
                new Book { Id = 1, Title = "To Kill a Mockingbird", Genre = "Fiction", Price = 10.99 },
                new Book { Id = 2, Title = "1984", Genre = "Dystopian", Price = 9.99 },
                new Book { Id = 3, Title = "Moby Dick", Genre = "Classic", Price = 11.99 },
                new Book { Id = 4, Title = "The Great Gatsby", Genre = "Classic", Price = 10.99 },
                new Book { Id = 5, Title = "War and Peace", Genre = "Historical", Price = 12.99 },
                new Book { Id = 6, Title = "Pride and Prejudice", Genre = "Romance", Price = 8.99 },
                new Book { Id = 7, Title = "The Catcher in the Rye", Genre = "Fiction", Price = 7.99 },
                new Book { Id = 8, Title = "The Hobbit", Genre = "Fantasy", Price = 9.99 },
                new Book { Id = 9, Title = "Crime and Punishment", Genre = "Mystery", Price = 10.99 },
                new Book { Id = 10, Title = "The Lord of the Rings", Genre = "Fantasy", Price = 15.99 }
            };

            displayedBooks = new List<Book>(allBooks);
            LstView.ItemsSource = displayedBooks;
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterBooks();
        }

        private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterCB.SelectedItem is ComboBoxItem selectedItem)
            {
                if (selectedItem.Content.ToString() == "Genre")
                {
                    FilterByGenre();
                }
                else if (selectedItem.Content.ToString() == "Price")
                {
                    FilterByPrice();
                }
            }
        }

        private void FilterByGenre()
        {
            var genres = allBooks.Select(b => b.Genre).Distinct().ToList();
            var genreMenu = new ContextMenu();
            foreach (var genre in genres)
            {
                var menuItem = new MenuItem { Header = genre };
                menuItem.Click += (s, e) => {
                    FilterBooksByGenre(genre);
                };
                genreMenu.Items.Add(menuItem);
            }

            genreMenu.IsOpen = true;
        }

        private void FilterByPrice()
        {
            var priceMenu = new ContextMenu();
            var priceRanges = new List<string> { "до 5 $", "от 5$ до 10$", "больше 10$" };

            foreach (var range in priceRanges)
            {
                var menuItem = new MenuItem { Header = range };
                menuItem.Click += (s, e) => {
                    FilterBooksByPrice(range);
                };
                priceMenu.Items.Add(menuItem);
            }

            priceMenu.IsOpen = true;
        }

        private void FilterBooksByGenre(string genre)
        {
            displayedBooks = allBooks.Where(book => book.Genre == genre).ToList();
            LstView.ItemsSource = displayedBooks;
        }

        private void FilterBooksByPrice(string range)
        {
            switch (range)
            {
                case "до 5 $":
                    displayedBooks = allBooks.Where(book => book.Price < 5).ToList();
                    break;
                case "от 5$ до 10$":
                    displayedBooks = allBooks.Where(book => book.Price >= 5 && book.Price <= 10).ToList();
                    break;
                case "больше 10$":
                    displayedBooks = allBooks.Where(book => book.Price > 10).ToList();
                    break;
            }

            LstView.ItemsSource = displayedBooks;
        }

        private void FilterBooks()
        {
            string searchText = SearchTB.Text.ToLower();
            displayedBooks = allBooks
                .Where(book => book.Title.ToLower().Contains(searchText))
                .ToList();
            LstView.ItemsSource = displayedBooks;
        }

        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortCB.SelectedItem is ComboBoxItem selectedItem)
            {
                if (selectedItem.Content.ToString() == "Title")
                {
                    displayedBooks = displayedBooks.OrderBy(book => book.Title).ToList();
                }
                else if (selectedItem.Content.ToString() == "Price")
                {
                    displayedBooks = displayedBooks.OrderBy(book => book.Price).ToList();
                }

                LstView.ItemsSource = displayedBooks;
            }
        }

        private void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTB.Text = string.Empty;
            FilterCB.SelectedIndex = -1;
            SortCB.SelectedIndex = -1;
            displayedBooks = new List<Book>(allBooks);
            LstView.ItemsSource = displayedBooks;
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public double Price { get; set; }
    }
}
