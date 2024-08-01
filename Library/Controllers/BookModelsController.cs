using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;
using Library.ViewModels;

namespace Library.Controllers
{
    public class BookModelsController : Controller
    {
        private readonly LibraryContext _context;

        public BookModelsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: BookModels
        public async Task<IActionResult> Index()
        {
            var libraryContext = _context.BookModel.Include(b => b.Shelf);
            return View(await libraryContext.ToListAsync());
        }

        // GET: BookModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.BookModel
                .Include(b => b.Shelf)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }

        // GET: BookModels/Create
        public IActionResult Create()
        {
            //var categories = _context.CategoryModel.Select(c => c.CategoryName).ToListAsync();
            ViewData["CategoryName"] = new SelectList(_context.CategoryModel, "CategoryName", "CategoryName");
            ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Id");
            return View();
        }

        // POST: BookModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(bool IsConfirmed, AddBookViewModel bookViewModel)
        {
            if (IsConfirmed == true)
            {
                return RedirectToAction(nameof(Index));
            }
            var CategoryId = await _context.CategoryModel.FirstOrDefaultAsync(c => c.CategoryName == bookViewModel.CategoryName);
            bookViewModel.CategoryId = CategoryId.Id;

            if (ModelState.IsValid)
            {
                var shelfList =  _context.ShelfModel
                    .Select(s => s)
                    .Where(s => s.CategoryId == bookViewModel.CategoryId)
                    .ToList();
                if (shelfList.Count <= 0)
                {
                    ViewData["message"] = "this category does not contain any shelves";
                    ViewData["CategoryName"] = new SelectList(_context.CategoryModel, "CategoryName", "CategoryName");
                    ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Id");
                    return View(bookViewModel);
                }
                int i = 0;
                int totalWidthInShelf;
                while(i < shelfList.Count)
                {
                    totalWidthInShelf = await _context.BookModel
                        .Where(s => s.ShelfId == shelfList[i].Id)
                        .SumAsync(s => s.Width);
                    if (shelfList[i].Width - totalWidthInShelf >= bookViewModel.Book.Width
                        && bookViewModel.Book.Height <= shelfList[i].Height)
                    {
                        break;
                    }
                    else if(i == shelfList.Count - 1)
                    {
                        ViewData["message"] = "cannot find a shell for this book in this category";
                        ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Id");
                        ViewData["CategoryName"] = new SelectList(_context.CategoryModel, "CategoryName", "CategoryName");
                        return View(bookViewModel);
                    }
                    i++;
                }
                bookViewModel.Book.ShelfId = shelfList[i].Id;
                _context.Add(bookViewModel.Book);
                await _context.SaveChangesAsync();
                if (bookViewModel.Book.Height + 10 <= shelfList[i].Height)
                {
                    ViewData["message"] = "this book is a lot lower than the shelf - just so you know :)";
                    ViewData["redirect"] = true;
                    return View(bookViewModel);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["message"] = "ModelState.IsValid is false";
            ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Id");
            ViewData["CategoryName"] = new SelectList(_context.CategoryModel, "CategoryName", "CategoryName");
            return View(bookViewModel);
        }

        public async Task<IActionResult> Redirect()
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: BookModels/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var bookModel = await _context.BookModel.FindAsync(id);
        //    if (bookModel == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["ShelfId"] = new SelectList(_context.ShelfModel, "Id", "Id", bookModel.ShelfId);
        //    return View(bookModel);
        //}

        //// POST: BookModels/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,BookName,Height,Width,ShelfId")] BookModel bookModel)
        //{
        //    if (id != bookModel.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(bookModel);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BookModelExists(bookModel.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ShelfId"] = new SelectList(_context.ShelfModel, "Id", "Id", bookModel.ShelfId);
        //    return View(bookModel);
        //}

        // GET: BookModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.BookModel
                .Include(b => b.Shelf)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }

        // POST: BookModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookModel = await _context.BookModel.FindAsync(id);
            if (bookModel != null)
            {
                _context.BookModel.Remove(bookModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookModelExists(int id)
        {
            return _context.BookModel.Any(e => e.Id == id);
        }
    }
}
