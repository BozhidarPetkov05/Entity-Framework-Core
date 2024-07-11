namespace BlogDataAnnotation
{
    public class Program
    {
        static void Main(string[] args)
        {
            using BlogDbContext dbContext = new BlogDbContext();

            var author = new Author()
            {
                AuthorName = "Pesho"
            };
            
            dbContext.Authors.Add(author);

            var blog = new Blog()
            {
                AuthorId = 1,
                Name = "Pesho's blog",
                Description = "Blog, what do you expect??",
                Created = DateTime.Now,
            };
            author.Blog = blog;

            dbContext.SaveChanges();

        }
    }
}