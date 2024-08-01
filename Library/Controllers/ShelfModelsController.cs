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
    public class ShelfModelsController : Controller
    {
        private readonly LibraryContext _context;

        public ShelfModelsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: ShelfModels
        public async Task<IActionResult> Index()
        {

            var libraryContext = _context.ShelfModel.Include(s => s.Category);
            var libraryList = await libraryContext.ToListAsync();
            ViewData["CategoryName"] = new SelectList(_context.CategoryModel, "CategoryName", "CategoryName");
            return View(libraryList);
        }

        // GET: ShelfModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelfModel = await _context.ShelfModel
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shelfModel == null)
            {
                return NotFound();
            }

            return View(shelfModel);
        }

        // GET: ShelfModels/Create
        public IActionResult Create()
        {
            ViewData["CategoryName"] = new SelectList(_context.CategoryModel, "CategoryName", "CategoryName");
            ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Id");
            return View();
        }

        // POST: ShelfModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddShelfViewModel shelfViewModel)
        {
            if (ModelState.IsValid)
            {
                var CategoryId = await _context.CategoryModel.FirstOrDefaultAsync(c => c.CategoryName == shelfViewModel.CategoryName);
                shelfViewModel.Shelf.CategoryId = CategoryId.Id;
                
                _context.Add(shelfViewModel.Shelf);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Id", shelfViewModel.CategoryId);
            ViewData["CategoryName"] = new SelectList(_context.CategoryModel, "CategoryName", "CategoryName", shelfViewModel.CategoryName);
            ViewData["message"] = "ModelState.IsValid is false...";
            return View(shelfViewModel);
        }

        // GET: ShelfModels/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var shelfModel = await _context.ShelfModel.FindAsync(id);
        //    if (shelfModel == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Id", shelfModel.CategoryId);
        //    return View(shelfModel);
        //}

        //// POST: ShelfModels/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Height,Width,CategoryId")] ShelfModel shelfModel)
        //{
        //    if (id != shelfModel.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(shelfModel);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ShelfModelExists(shelfModel.Id))
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
        //    ViewData["CategoryId"] = new SelectList(_context.CategoryModel, "Id", "Id", shelfModel.CategoryId);
        //    return View(shelfModel);
        //}

        // GET: ShelfModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelfModel = await _context.ShelfModel
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shelfModel == null)
            {
                return NotFound();
            }

            return View(shelfModel);
        }

        // POST: ShelfModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shelfModel = await _context.ShelfModel.FindAsync(id);
            if (shelfModel != null)
            {
                _context.ShelfModel.Remove(shelfModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShelfModelExists(int id)
        {
            return _context.ShelfModel.Any(e => e.Id == id);
        }
    }
}
