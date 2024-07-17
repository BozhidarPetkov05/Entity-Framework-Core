namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            Console.WriteLine(RemoveBooks(db));
        }

        //P02. Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var books = context.Books
                .AsEnumerable()
                .Where(a => a.AgeRestriction.ToString().ToLower() == command.ToLower())
                .OrderBy(a => a.Title)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        //P03. Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .AsEnumerable()
                .Where(b => b.Copies < 5000 && b.EditionType == EditionType.Gold)
                .OrderBy(b => b.BookId)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        //P04. Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .AsEnumerable()
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(p => p.Price)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} - ${b.Price:f2}"));
        }

        //P05. Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .AsEnumerable()
                .Where(b => b.ReleaseDate.Value.Year != year &&
                        b.ReleaseDate.HasValue)
                .OrderBy(b => b.BookId)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => b.Title));
        }

        //P06. Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            List<string> categories = input.Split(" ").Select(c => c.ToLower()).ToList();

            var books = context.Books
                .Where(b => b.BookCategories
                .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => b.Title));
        }

        //P07. Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate < parsedDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price,
                    b.ReleaseDate
                })
                .OrderByDescending(b => b.ReleaseDate);


            return string.Join(Environment.NewLine,
                books.Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}"));
        }

        //P08. Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .AsEnumerable()
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .ToList();

            return string.Join(Environment.NewLine, authors.Select(a => $"{a.FirstName} {a.LastName}"));
        }

        //P09. Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            string lowered = input.ToLower();

            var books = context.Books
                .AsEnumerable()
                .Where(b => b.Title.ToLower().Contains(lowered))
                .OrderBy(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => b.Title));
        }

        //P10. Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var lowered = input.ToLower();

            var authors = context.Authors
                .AsEnumerable()
                .Where(a => a.LastName.ToLower().StartsWith(lowered))
                .ToList();

            var books = context.Books
                .AsEnumerable()
                .Where(b => authors.Any(a => a.FirstName == b.Author.FirstName) &&
                            authors.Any(a => a.LastName == b.Author.LastName))
                .OrderBy(b => b.BookId)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})"));
        }

        //P11. Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int booksCount = context.Books
                .AsEnumerable()
                .Where(b => b.Title.Length > lengthCheck)
                .Count();

            return booksCount;
        }

        //P12. Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new
                {
                    AuhtorName = string.Join(" ", a.FirstName, a.LastName),
                    TotalBooks = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.TotalBooks).ToList();

            return string.Join(Environment.NewLine,
                authors.Select(a => $"{a.AuhtorName} - {a.TotalBooks}"));
        }

        //P13. Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var books = context.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalSum = c.CategoryBooks
                        .Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .OrderByDescending(c => c.TotalSum)
                .ThenBy(c => c.Name);

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Name} ${b.TotalSum:f2}"));
        }

        //P14. Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var books = context.Categories
                .Select(c => new
                {
                    c.Name,
                    RecentBooks = c.CategoryBooks.Select(b => new
                    {
                        Title = b.Book.Title,
                        Date = b.Book.ReleaseDate
                    })
                    .OrderByDescending(b => b.Date)
                    .Take(3)
                    .ToList()
                })
                .OrderBy(c => c.Name);

            return string.Join(Environment.NewLine, books
                .Select(b =>$"--{b.Name}{Environment.NewLine}{string.Join(Environment.NewLine, b.RecentBooks.Select(rc => $"{rc.Title} ({rc.Date.Value.Year})"))}"));
        }

        //P15. Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        //P16. Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            context.RemoveRange(books);
            context.SaveChanges();

            return books.Count; 

        }
    }
}


