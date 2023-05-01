using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvanceAjaxCRUD.Data;
using AdvanceAjaxCRUD.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;

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
        public FileResult GeneratePDF(int id)
        {
            // Retrieve dynamic content from the database
            string recipientName = "Md Sohanur Rahaman"; // dynamic Name of the Recipent
            string meetingNo = "1330"; // dynamic meeting no
            string date = "05 May, 2023"; // Date
            string venue = "Board Room , Head Office, Motijheel"; // Venue
            string address = "Dynamic Address"; // Address
            string startTime = "2:00 PM"; // Start Time
                                          //string logoUrl = ... // URL of the company logo
            string companyAddress = "Dilkusha Cprporate Area,Motijheel,Dhaka"; // Company Address
            string senderName = "Company Secretary"; // Name of the sender
            string jobPosition = "General Manager"; // Job Position
            string companyName = "Pubali Bank Limited"; // Company Name

            // Create a string variable to hold the invitation letter template with placeholders for the dynamic content
            string template = @"
            <p>[Date]</p>
            <p>[Company Addresss]</p>
    
            <p>[dynamic Name of the Recipent]</p>
            <p>Name of the Company</p>
            <p>Address</p>
            <br/>
            <p>Subject: To Attend in the meeting [dynamic meeting no].</p>

            <p>Dear Sir,</p>
             <br/>
            <p>It is with great pleasure that I invite you to the company’s meeting [dynamic meeting no] board meeting. The event shall be held on [Date] at [Venue] located in [Address] from [Start Time].</p>

            <p>As one of the most important people in the company, it would be such an honor if you could grace our Event with your presence. It is also an opportunity for us to thank you for all your contributions to the success of the company for the past years.</p>
            <br/>
            <p>I am looking forward to your presence at the business convention.</p>
             <br/>
            <p>Yours faithfully,</p>

            <p>Name and Signature<br>Job Position<br>Company Name</p>";

            // Replace placeholders in the template with actual values
            template = template.Replace("[dynamic Name of the Recipent]", recipientName)
                               .Replace("[dynamic meeting no]", meetingNo)
                               .Replace("[Date]", date)
                               .Replace("[Venue]", venue)
                               .Replace("[Address]", address)
                               .Replace("[Start Time]", startTime)
                               //.Replace("[LOGO]", "<img src='" + logoUrl + "' alt='Company Logo' />")
                               .Replace("[Company Addresss]", companyAddress)
                               .Replace("Name and Signature", senderName)
                               .Replace("Job Position", jobPosition)
                               .Replace("Company Name", companyName);
            // Create a new Document object
            Document document = new Document(PageSize.A4, 50, 50, 25, 25);

            // Create a new PdfWriter that will write to a MemoryStream
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);

            // Open the Document
            document.Open();
            // Load the image from the file path
            var logo = Image.GetInstance(@"C:\Users\sohan\Downloads\logo.jpg");

            // Resize the image to fit within the page
            logo.ScaleToFit(150f, 150f);

            // Set the image alignment to the right
            logo.Alignment = Element.ALIGN_RIGHT;

            // Add the image to the Document
            document.Add(logo);

            // Parse the HTML template and add it to the Document
            using (TextReader reader = new StringReader(template))
            {
                HTMLWorker worker = new HTMLWorker(document);
                worker.Parse(reader);
            }

            // Close the Document
            document.Close();

            // Convert the MemoryStream to a byte array
            byte[] fileContents = stream.ToArray();

            // Return the PDF as a FileResult
            string fileName = "invitation_letter.pdf";
            return File(fileContents, "application/pdf", fileName);
        }

    }
}
