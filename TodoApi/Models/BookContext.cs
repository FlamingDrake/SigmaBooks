using Microsoft.EntityFrameworkCore;

namespace SigmaBooks.Models
{
	public class BookContext : DbContext
	{

		public BookContext(DbContextOptions<BookContext> options)
			: base(options)
		{
		}

		public DbSet<BookItem> BookItem { get; set; }
	}
}
