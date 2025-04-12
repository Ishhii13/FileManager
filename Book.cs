using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace FileManager
{
    internal class Book
    {
        string bookPath = "D:\\Visual Studio\\Visual Studio repos\\FileManager\\Tables\\Book.txt";

        public int id;
        public string title;
        public string author;
        public List<string> genres;
        public string date;
        public int pages;

        public Book(int newID, string newTitle, string newAuthor, List<string> newGenres, string newDate, int newPages)
        {
            id = newID;
            title = newTitle;
            author = newAuthor;
            genres = newGenres;
            date = newDate;
            pages = newPages;
        }

        private List<string> GetGenres(string line)
        {
            List<string> genres = new List<string>();

            string[] content = line.Split(',');

            foreach (string contentItem in content)
                genres.Add(contentItem);

            return genres;
        }

        public List<Book> GetAllBooks()
        {
            List<Book> allBooks = new List<Book>();

            using (StreamReader sr = new StreamReader(bookPath))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] content = line.Split('|');

                    Book book = new Book(int.Parse(content[0]), content[1], content[3], GetGenres(content[2]), content[4],int.Parse(content[5]));

                    allBooks.Add(book);
                }
            }

            return allBooks;
        }

        public Book GetBook(int inputID, List<Book> allBooks)
        {
            foreach (Book book in allBooks)
            {
                if (book.id == inputID)
                    return book;
            }

            return null;
        }
    }
}
