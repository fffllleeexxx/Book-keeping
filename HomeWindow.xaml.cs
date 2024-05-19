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
        private Book selectedBook;

        public HomeWindow()
        {
            InitializeComponent();
            InitializeBooks();
            LstView.ItemsSource = displayedBooks;
        }

        private void InitializeBooks()
        {
            allBooks = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", Genre = "Fiction", Price = 4.99, Description = "Description of Book 1", ImagePath = "Images/book1.jpg" },
                new Book { Id = 2, Title = "Book 2", Genre = "Non-Fiction", Price = 9.99, Description = "Description of Book 2", ImagePath = "Images/book2.jpg" },
                new Book { Id = 3, Title = "Book 3", Genre = "Mystery", Price = 15.99, Description = "Description of Book 3", ImagePath = "Images/book3.jpg" },
                new Book { Id = 4, Title = "Book 4", Genre = "Fantasy", Price = 5.99, Description = "Description of Book 4", ImagePath = "Images/book4.jpg" },
                new Book { Id = 5, Title = "Book 5", Genre = "Science Fiction", Price = 12.99, Description = "Description of Book 5", ImagePath = "Images/book5.jpg" },
                new Book { Id = 6, Title = "Book 6", Genre = "Romance", Price = 7.99, Description = "Description of Book 6", ImagePath = "Images/book6.jpg" },
                new Book { Id = 7, Title = "Book 7", Genre = "Thriller", Price = 10.99, Description = "Description of Book 7", ImagePath = "Images/book7.jpg" },
                new Book { Id = 8, Title = "Book 8", Genre = "Historical", Price = 3.99, Description = "Description of Book 8", ImagePath = "Images/book8.jpg" },
                new Book { Id = 9, Title = "Book 9", Genre = "Biography", Price = 11.99, Description = "Description of Book 9", ImagePath = "Images/book9.jpg" },
                new Book { Id = 10, Title = "Book 10", Genre = "Adventure", Price = 14.99, Description = "Description of Book 10", ImagePath = "Images/book10.jpg" },
            };
            displayedBooks = new List<Book>(allBooks);
        }

        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortCB.SelectedItem is ComboBoxItem selectedItem)
            {
                if (selectedItem.Content.ToString() == "Price")
                {
                    ShowPriceSortMenu();
                }
                else if (selectedItem.Content.ToString() == "Genre")
                {
                    ShowGenreSortMenu();
                }
                else if (selectedItem.Content.ToString() == "Title")
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
            var azItem = new MenuItem { Header = "A to Z" };
            azItem.Click += (s, e) => SortBooksByGenre(true);
            var zaItem = new MenuItem { Header = "Z to A" };
            zaItem.Click += (s, e) => SortBooksByGenre(false);

            genreSortMenu.Items.Add(azItem);
            genreSortMenu.Items.Add(zaItem);

            genreSortMenu.IsOpen = true;
        }

        private void ShowTitleSortMenu()
        {
            var titleSortMenu = new ContextMenu();
            var azItem = new MenuItem { Header = "A to Z" };
            azItem.Click += (s, e) => SortBooksByTitle(true);
            var zaItem = new MenuItem { Header = "Z to A" };
            zaItem.Click += (s, e) => SortBooksByTitle(false);

            titleSortMenu.Items.Add(azItem);
            titleSortMenu.Items.Add(zaItem);

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

        private void SortBooksByGenre(bool az)
        {
            if (az)
            {
                displayedBooks = displayedBooks.OrderBy(book => book.Genre).ToList();
            }
            else
            {
                displayedBooks = displayedBooks.OrderByDescending(book => book.Genre).ToList();
            }
            LstView.ItemsSource = displayedBooks;
        }

        private void SortBooksByTitle(bool az)
        {
            if (az)
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
            SearchTB.Text = string.Empty;
            FilterCB.SelectedIndex = -1;
            SortCB.SelectedIndex = -1;
            displayedBooks = new List<Book>(allBooks);
            LstView.ItemsSource = displayedBooks;
        }

        private void ViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (LstView.SelectedItem is Book selectedBook)
            {
                BookDetailsWindow viewBookWindow = new BookDetailsWindow(selectedBook);
                viewBookWindow.ShowDialog();
            }
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (LstView.SelectedItem is Book selectedBook)
            {
                EditBookWindow editBookWindow = new EditBookWindow(selectedBook);
                if (editBookWindow.ShowDialog() == true)
                {
                    LstView.ItemsSource = null;
                    LstView.ItemsSource = displayedBooks;
                }
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (LstView.SelectedItem is Book selectedBook)
            {
                var result = MessageBox.Show("Are you sure you want to delete this book?", "Confirm Delete", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    displayedBooks.Remove(selectedBook);
                    allBooks.Remove(selectedBook);
                    LstView.ItemsSource = null;
                    LstView.ItemsSource = displayedBooks;
                }
            }
        }

        private void LstView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListView;
            if (listView.SelectedItem == null)
            {
                return;
            }

            selectedBook = listView.SelectedItem as Book;
            ContextMenu contextMenu = listView.ContextMenu;
            contextMenu.PlacementTarget = listView;
            contextMenu.IsOpen = true;
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}
