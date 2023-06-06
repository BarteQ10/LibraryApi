using LibraryApi.Entites;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure your model here
        }

        public void SeedData()
        {
            if (!Books.Any())
            {
                Books.AddRange(
                new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", Genre = "Fiction", Description = "A classic novel set in the 1930s, portraying racial injustice through the eyes of a young girl.", CoverImage = "mockingbird.jpg", IsAvailable = true },
                new Book { Title = "1984", Author = "George Orwell", Genre = "Dystopian Fiction", Description = "A dystopian novel set in a totalitarian society, exploring themes of government surveillance and manipulation.", CoverImage = "1984.jpg", IsAvailable = true },
                new Book { Title = "Pride and Prejudice", Author = "Jane Austen", Genre = "Romance", Description = "A classic romance novel depicting the social norms and expectations of 19th-century British society.", CoverImage = "pride-prejudice.jpg", IsAvailable = true },
                new Book { Title = "To the Lighthouse", Author = "Virginia Woolf", Genre = "Modernist Literature", Description = "A pioneering novel that explores themes of time, consciousness, and the complexities of human relationships.", CoverImage = "lighthouse.jpg", IsAvailable = true },
                new Book { Title = "Moby-Dick", Author = "Herman Melville", Genre = "Adventure", Description = "An epic tale of obsession and revenge, following the journey of Captain Ahab and his pursuit of the great white whale.", CoverImage = "moby-dick.jpg", IsAvailable = true },
                new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Genre = "Classic Fiction", Description = "A novel set in the Jazz Age, exploring themes of wealth, love, and the American Dream.", CoverImage = "great-gatsby.jpg", IsAvailable = true },
                new Book { Title = "Brave New World", Author = "Aldous Huxley", Genre = "Science Fiction", Description = "A dystopian novel set in a futuristic society where humans are genetically engineered and conditioned for social stability.", CoverImage = "brave-new-world.jpg", IsAvailable = true },
                new Book { Title = "The Catcher in the Rye", Author = "J.D. Salinger", Genre = "Coming-of-Age Fiction", Description = "A coming-of-age novel that explores themes of alienation, identity, and teenage angst.", CoverImage = "catcher-in-the-rye.jpg", IsAvailable = true },
                new Book { Title = "Jane Eyre", Author = "Charlotte Brontë", Genre = "Gothic Fiction", Description = "A Gothic novel following the life of Jane Eyre, a young governess, as she faces various challenges and finds love.", CoverImage = "jane-eyre.jpg", IsAvailable = true },
                new Book { Title = "The Hobbit", Author = "J.R.R. Tolkien", Genre = "Fantasy", Description = "An adventure novel set in Middle-earth, preceding the events of the Lord of the Rings trilogy.", CoverImage = "hobbit.jpg", IsAvailable = true },
                new Book { Title = "Crime and Punishment", Author = "Fyodor Dostoevsky", Genre = "Psychological Fiction", Description = "A psychological novel that explores the mind of Raskolnikov, a former student who commits a heinous crime.", CoverImage = "crime-punishment.jpg", IsAvailable = true },
                new Book { Title = "The Lord of the Rings", Author = "J.R.R. Tolkien", Genre = "Fantasy", Description = "An epic high-fantasy trilogy that follows the journey of Frodo Baggins to destroy the One Ring and save Middle-earth.", CoverImage = "lord-of-the-rings.jpg", IsAvailable = true },
                new Book { Title = "Wuthering Heights", Author = "Emily Brontë", Genre = "Gothic Fiction", Description = "A Gothic novel that portrays a passionate and destructive love story set in the Yorkshire moors.", CoverImage = "wuthering-heights.jpg", IsAvailable = true },
                new Book { Title = "The Odyssey", Author = "Homer", Genre = "Epic Poetry", Description = "An ancient Greek epic poem that recounts the adventures of Odysseus as he tries to return home after the Trojan War.", CoverImage = "odyssey.jpg", IsAvailable = true },
                new Book { Title = "The Scarlet Letter", Author = "Nathaniel Hawthorne", Genre = "Historical Fiction", Description = "A novel set in 17th-century Puritan Boston, exploring themes of sin, guilt, and redemption.", CoverImage = "scarlet-letter.jpg", IsAvailable = true },
                new Book { Title = "The Picture of Dorian Gray", Author = "Oscar Wilde", Genre = "Gothic Fiction", Description = "A novel that tells the story of a young man named Dorian Gray, who remains eternally young while his portrait ages and reflects his immoral actions.", CoverImage = "dorian-gray.jpg", IsAvailable = true },
                new Book { Title = "Frankenstein", Author = "Mary Shelley", Genre = "Gothic Fiction", Description = "A novel about the consequences of scientific ambition and the nature of humanity, as Victor Frankenstein creates a monstrous creature.", CoverImage = "frankenstein.jpg", IsAvailable = true },
                new Book { Title = "The Adventures of Huckleberry Finn", Author = "Mark Twain", Genre = "Adventure", Description = "A novel that follows the adventures of Huck Finn and his friend Jim, an escaped slave, as they journey down the Mississippi River.", CoverImage = "huckleberry-finn.jpg", IsAvailable = true },
                new Book { Title = "Anna Karenina", Author = "Leo Tolstoy", Genre = "Realistic Fiction", Description = "A novel that explores themes of love, adultery, and societal norms through the story of Anna Karenina, a married aristocrat.", CoverImage = "anna-karenina.jpg", IsAvailable = true },
                new Book { Title = "The Count of Monte Cristo", Author = "Alexandre Dumas", Genre = "Adventure", Description = "An adventure novel that follows the journey of Edmond Dantès as he seeks revenge against those who wronged him.", CoverImage = "monte-cristo.jpg", IsAvailable = true }
            );
                SaveChanges();
            }
        }
    }
}
