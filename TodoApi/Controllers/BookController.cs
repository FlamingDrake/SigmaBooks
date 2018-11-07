using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using SigmaBooks.Models;

namespace SigmaBooks.Controllers
{
	[Route("api/book")]
	[ApiController]
	public class BookController : ControllerBase
	{
		private readonly BookContext _context;

		public BookController(BookContext context)
		{
			_context = context;

			// Reads the data from the file.
			if (_context.BookItem.Count() != 0) return;
			XmlDocument bookDoc =  new XmlDocument();
			bookDoc.Load("books.xml");

			foreach (XmlNode node in bookDoc.DocumentElement.ChildNodes)
			{
				var nodeList = node.ChildNodes;


				_context.Add(new BookItem
				{
					Id=node.Attributes["id"].InnerText,
					Author = nodeList[0].InnerText,
					Title = nodeList[1].InnerText,
					Genre = nodeList[2].InnerText,
					Price = double.Parse(nodeList[3].InnerText),
					PublishDate = Convert.ToDateTime(nodeList[4].InnerText),
					Description = nodeList[5].InnerText
				});
			}

			_context.SaveChanges();
		}

		[HttpGet]
		public ActionResult<List<BookItem>> GetAll()
		{
			return _context.BookItem.ToList();
		}
		
		[HttpGet("search")]
		public ActionResult<List<BookItem>> GetByTitle(string title, string author, string genre)
		{
			var temp = _context.BookItem.ToList();
			var result = new List<BookItem>();
			if (title == null && author == null && genre == null)
			{
				// If the search query is empty, I find it more useful to return everything.
				return temp;
			}

			foreach (var node in temp)
			{
				if (title != null && node.Title.ToLower().Contains(title.ToLower()))
				{
					result.Add(node);
				} else if (author != null && node.Author.ToLower().Contains(author.ToLower()))
				{
					result.Add(node);
				}
				else if (genre != null && node.Genre.ToLower().Contains(genre.ToLower()))
				{
					result.Add(node);
				}
			}
			return result;
		}
		
	}
}