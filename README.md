# LibraryAPI.Tests

This repository contains unit tests for the `LibraryAPI` project, focusing on the `BookRepository` class.

## Project Setup

1. **Prerequisites:**
   - .NET 8 SDK
   - Visual Studio or a compatible IDE

2. **Clone the repository:**

```bash
   git clone https://github.com/angel-hidalgo-source/LibraryAPI.Tests.git
```
3.  **Open the solution:**
    -   Navigate to the cloned directory and open the  `LibraryAPI.Tests.sln`  file.

## Running the Tests

1.  **Build the solution:**
    
    -   Right-click on the solution in the Solution Explorer and select "Rebuild Solution".
2.  **Run the tests:**
    
    -   Right-click on the  `LibraryAPI.Tests`  project and select "Run Tests".
    -   Alternatively, use the "Test Explorer" window in Visual Studio.

## Test Coverage

The tests cover the following functionalities of the  `BookRepository`:

-   **GetAllBooksAsync:**  Retrieves all books from the database.
-   **GetBookByIdAsync:**  Retrieves a book by its ID.
-   **AddBookAsync:**  Adds a new book to the database.
-   **DeleteBookAsync:**  Removes a book from the database.
-   **LendBookAsync:**  Decreases the available copies of a book.
-   **ReturnBookAsync:**  Increases the available copies of a book.

## Testing Approach

-   **In-Memory Database:**  The tests use an in-memory database (`UseInMemoryDatabase`) to isolate the tests from the actual database environment.
-   **XUnit:**  The tests are written using the XUnit testing framework.
-   **Async/Await:**  The tests utilize asynchronous methods to handle database operations.
-   **Assertions:**  The tests use assertions to verify the expected outcomes of the methods.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request if you have any suggestions or improvements.

## License

This project is licensed under the MIT License. See the  `LICENSE`  file for details.
