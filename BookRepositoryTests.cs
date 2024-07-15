using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LibraryAPI.Tests
{
    public class BookRepositoryTests : IDisposable
    {
        private readonly LibraryContext _context;
        private readonly IBookRepository _bookRepository;

        public BookRepositoryTests()
        {
            // Initialize in-memory database for testing
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase("TestLibraryDB")
                .Options;
            _context = new LibraryContext(options);
            _bookRepository = new BookRepository(_context);
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnsAllBooks()
        {
            // Currently added books
            var currentlyBooks = await _bookRepository.GetAllBooksAsync();

            // Arrange
            await _context.Books.AddRangeAsync(
                new Book { Title = "Book 1", Author = "Author 1", Copies = 3 },
                new Book { Title = "Book 2", Author = "Author 2", Copies = 2 }
            );
            await _context.SaveChangesAsync();

            // Recently added books
            var books = await _bookRepository.GetAllBooksAsync();

            // Assert
            Assert.NotNull(books);
            Assert.Equal(2 + currentlyBooks.ToList().Count, books.ToList().Count);
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsBookWithMatchingId()
        {
            // Arrange
            var book = new Book { Title = "Book 1", Author = "Author 1", Copies = 3 };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            // Act
            var retrievedBook = await _bookRepository.GetBookByIdAsync(book.Id);

            // Assert
            Assert.NotNull(retrievedBook);
            Assert.Equal(book.Id, retrievedBook.Id);
            Assert.Equal(book.Title, retrievedBook.Title);
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsNullForNonExistingBook()
        {
            // Act
            var book = await _bookRepository.GetBookByIdAsync(100);

            // Assert
            Assert.Null(book);
        }

        [Fact]
        public async Task AddBookAsync_AddsNewBookToDatabase()
        {
            // Arrange
            var book = new Book { Title = "Book 1", Author = "Author 1", Copies = 3 };

            // Act
            await _bookRepository.AddBookAsync(book);

            // Assert
            var retrievedBook = await _context.Books.FindAsync(book.Id);
            Assert.NotNull(retrievedBook);
            Assert.Equal(book.Title, retrievedBook.Title);
        }

        [Fact]
        public async Task DeleteBookAsync_RemovesBookFromDatabase()
        {
            // Arrange
            var book = new Book { Title = "Book 1", Author = "Author 1", Copies = 3 };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            // Act
            await _bookRepository.DeleteBookAsync(book.Id);

            // Assert
            var retrievedBook = await _context.Books.FindAsync(book.Id);
            Assert.Null(retrievedBook);
        }

        [Fact]
        public async Task LendBookAsync_DecreasesAvailableCopies()
        {
            // Arrange
            var book = new Book { Title = "Book 1", Author = "Author 1", Copies = 3, AvailableCopies = 3 };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            // Act
            await _bookRepository.LendBookAsync(book.Id);

            // Assert
            var retrievedBook = await _context.Books.FindAsync(book.Id);
            Assert.Equal(2, retrievedBook.AvailableCopies);
        }

        [Fact]
        public async Task LendBookAsync_ThrowsKeyNotFoundExceptionForNonExistingBook()
        {
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookRepository.LendBookAsync(100));
        }

        [Fact]
        public async Task LendBookAsync_ThrowsExceptionWhenNoCopiesAvailable()
        {
            // Arrange
            var book = new Book { Title = "Book 1", Author = "Author 1", Copies = 0 };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookRepository.LendBookAsync(book.Id));
        }

        [Fact]
        public async Task ReturnBookAsync_IncreasesAvailableCopies()
        {
            // Arrange
            var book = new Book { Title = "Book 1", Author = "Author 1", Copies = 3, AvailableCopies = 2 };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            // Act
            await _bookRepository.ReturnBookAsync(book.Id);

            // Assert
            var retrievedBook = await _context.Books.FindAsync(book.Id);
            Assert.Equal(3, retrievedBook.AvailableCopies);
        }

        [Fact]
        public async Task ReturnBookAsync_ThrowsKeyNotFoundExceptionForNonExistingBook()
        {
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookRepository.ReturnBookAsync(100));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
