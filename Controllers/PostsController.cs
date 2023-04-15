using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvanceAjaxCRUD.Data;
using AdvanceAjaxCRUD.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Html2pdf;
using iText.Layout.Element;
using iText.Html2pdf.Resolver.Font;
using AdvanceAjaxCRUD.Helper;
using iText.Kernel.Events;

namespace AdvanceAjaxCRUD.Controllers
{
    public class PostsController : Controller
    {
        private readonly AppDbContext _context;

        public PostsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Post.ToListAsync());
        }
        public async Task<IActionResult> GeneratePdf(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            var title = post.Title;
            var content = post.Content;
            var Id = post.Id;
            // Concatenate the title and content into a single HTML string
            var htmlContent = $"<p>{title}</p>{content}";
            var header = "this is short header";

            var memoryStream = new MemoryStream();

            var writer = new PdfWriter(memoryStream);
            var pdfDocument = new PdfDocument(writer);
            var document = new Document(pdfDocument);
            // Set up the header handler
            // Add the header event handler to the pdf document
            pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, new HeaderEventHandler(header));

            HtmlConverter.ConvertToPdf(htmlContent, writer, new ConverterProperties());

            //pdfDocument.Close();
            //document.Close();

            return File(memoryStream.ToArray(), "application/pdf", "memo.pdf");
        }



    // GET: Posts/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Post == null)
        {
            return NotFound();
        }

        var post = await _context.Post
            .FirstOrDefaultAsync(m => m.Id == id);
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    // GET: Posts/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Posts/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Content")] Post post)
    {
        if (ModelState.IsValid)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(post);
    }

    // GET: Posts/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Post == null)
        {
            return NotFound();
        }

        var post = await _context.Post.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        return View(post);
    }

    // POST: Posts/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content")] Post post)
    {
        if (id != post.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(post);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(post);
    }

    // GET: Posts/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Post == null)
        {
            return NotFound();
        }

        var post = await _context.Post
            .FirstOrDefaultAsync(m => m.Id == id);
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    // POST: Posts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Post == null)
        {
            return Problem("Entity set 'AppDbContext.Post'  is null.");
        }
        var post = await _context.Post.FindAsync(id);
        if (post != null)
        {
            _context.Post.Remove(post);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PostExists(int id)
    {
        return _context.Post.Any(e => e.Id == id);
    }
    
}
}
