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
    /// Логика взаимодействия для HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        private List<Book> allBooks;
        private List<Book> displayedBooks;
        private string connectionString;

        public HomeWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["BooksDB"].ConnectionString;
            InitializeBooks();
        }

        private void InitializeBooks()
        {
            allBooks = new List<Book>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT B.Id, B.Title, B.Price, B.Description, B.ImagePath, G.Name AS Genre FROM Books B JOIN Genres G ON B.GenreId = G.Id";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allBooks.Add(new Book
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Price = reader.GetDecimal(2),
                            Description = reader.GetString(3),
                            ImagePath = reader.GetString(4),
                            Genre = reader.GetString(5)
                        });
                    }
                }
            }
            displayedBooks = new List<Book>(allBooks);
            LstView.ItemsSource = displayedBooks;
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchTB.Text.ToLower();
            displayedBooks = allBooks.Where(book => book.Title.ToLower().Contains(searchText)).ToList();
            LstView.ItemsSource = displayedBooks;
        }

        private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedFilter = FilterCB.SelectedItem as ComboBoxItem;
            if (selectedFilter != null)
            {
                if (selectedFilter.Content.ToString() == "Genre")
                {
                    ShowGenreFilterMenu();
                }
                else if (selectedFilter.Content.ToString() == "Price")
                {
                    ShowPriceFilterMenu();
                }
            }
        }

        private void ShowGenreFilterMenu()
        {
            var genreFilterMenu = new ContextMenu();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT Name FROM Genres";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var genre = reader.GetString(0);
                        var menuItem = new MenuItem { Header = genre };
                        menuItem.Click += (s, e) => FilterBooksByGenre(genre);
                        genreFilterMenu.Items.Add(menuItem);
                    }
                }
            }

            genreFilterMenu.IsOpen = true;
        }

        private void ShowPriceFilterMenu()
        {
            var priceFilterMenu = new ContextMenu();
            var priceRanges = new List<Tuple<string, Func<Book, bool>>>
            {
                new Tuple<string, Func<Book, bool>>("Up to $5", book => book.Price <= 5),
                new Tuple<string, Func<Book, bool>>("$5 to $10", book => book.Price > 5 && book.Price <= 10),
                new Tuple<string, Func<Book, bool>>("Above $10", book => book.Price > 10)
            };

            foreach (var range in priceRanges)
            {
                var menuItem = new MenuItem { Header = range.Item1 };
                menuItem.Click += (s, e) => FilterBooksByPrice(range.Item2);
                priceFilterMenu.Items.Add(menuItem);
            }

            priceFilterMenu.IsOpen = true;
        }

        private void FilterBooksByGenre(string genre)
        {
            displayedBooks = allBooks.Where(book => book.Genre == genre).ToList();
            LstView.ItemsSource = displayedBooks;
        }

        private void FilterBooksByPrice(Func<Book, bool> priceFilter)
        {
            displayedBooks = allBooks.Where(priceFilter).ToList();
            LstView.ItemsSource = displayedBooks;
        }

        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSort = SortCB.SelectedItem as ComboBoxItem;
            if (selectedSort != null)
            {
                if (selectedSort.Content.ToString() == "Price")
                {
                    ShowPriceSortMenu();
                }
                else if (selectedSort.Content.ToString() == "Genre")
                {
                    ShowGenreSortMenu();
                }
                else if (selectedSort.Content.ToString() == "Title")
                {
                    ShowTitleSortMenu();
                }
            }
        }

        private void ShowPriceSortMenu()
        {
            var priceSortMenu = new ContextMenu();
            var ascItem = new MenuItem { Header = "Ascending" };
            ascItem.Click += (s, e) => SortBooksByPrice(true);
            var descItem = new MenuItem { Header = "Descending" };
            descItem.Click += (s, e) => SortBooksByPrice(false);

            priceSortMenu.Items.Add(ascItem);
            priceSortMenu.Items.Add(descItem);

            priceSortMenu.IsOpen = true;
        }

        private void ShowGenreSortMenu()
        {
            var genreSortMenu = new ContextMenu();
            var ascItem = new MenuItem { Header = "A to Z" };
            ascItem.Click += (s, e) => SortBooksByGenre(true);
            var descItem = new MenuItem { Header = "Z to A" };
            descItem.Click += (s, e) => SortBooksByGenre(false);

            genreSortMenu.Items.Add(ascItem);
            genreSortMenu.Items.Add(descItem);

            genreSortMenu.IsOpen = true;
        }

        private void ShowTitleSortMenu()
        {
            var titleSortMenu = new ContextMenu();
            var ascItem = new MenuItem { Header = "A to Z" };
            ascItem.Click += (s, e) => SortBooksByTitle(true);
            var descItem = new MenuItem { Header = "Z to A" };
            descItem.Click += (s, e) => SortBooksByTitle(false);

            titleSortMenu.Items.Add(ascItem);
            titleSortMenu.Items.Add(descItem);

            titleSortMenu.IsOpen = true;
        }

        private void SortBooksByPrice(bool ascending)
        {
            if (ascending)
            {
                displayedBooks = displayedBooks.OrderBy(book => book.Price).ToList();
            }
            else
            {
                displayedBooks = displayedBooks.OrderByDescending(book => book.Price).ToList();
            }
            LstView.ItemsSource = displayedBooks;
        }

        private void SortBooksByGenre(bool ascending)
        {
            if (ascending)
            {
                displayedBooks = displayedBooks.OrderBy(book => book.Genre).ToList();
            }
            else
            {
                displayedBooks = displayedBooks.OrderByDescending(book => book.Genre).ToList();
            }
            LstView.ItemsSource = displayedBooks;
        }

        private void SortBooksByTitle(bool ascending)
        {
            if (ascending)
            {
                displayedBooks = displayedBooks.OrderBy(book => book.Title).ToList();
            }
            else
            {
                displayedBooks = displayedBooks.OrderByDescending(book => book.Title).ToList();
            }
            LstView.ItemsSource = displayedBooks;
        }

        private void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            displayedBooks = new List<Book>(allBooks);
            LstView.ItemsSource = displayedBooks;
            SearchTB.Clear();
            FilterCB.SelectedIndex = -1;
            SortCB.SelectedIndex = -1;
        }

        private void LstView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (LstView.SelectedItem != null)
            {
                LstView.ContextMenu.IsOpen = true;
            }
        }

        private void ViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = LstView.SelectedItem as Book;
            if (selectedBook != null)
            {
                var viewWindow = new BookDetailsWindow(selectedBook);
                viewWindow.Show();
                this.Close();
            }
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = LstView.SelectedItem as Book;
            if (selectedBook != null)
            {
                var editWindow = new EditBookWindow(selectedBook);
                editWindow.Show();
                this.Close();
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = LstView.SelectedItem as Book;
            if (selectedBook != null)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "DELETE FROM Books WHERE Id = @Id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", selectedBook.Id);
                        command.ExecuteNonQuery();
                    }
                }
                InitializeBooks();
            }
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}
