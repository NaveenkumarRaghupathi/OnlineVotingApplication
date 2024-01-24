using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineVoteApplication.Models;

namespace OnlineVoteApplication.Controllers
{
    public class VotersController : Controller
    {
        private readonly OnlineVoteApplicationContext _context;
        private IWebHostEnvironment WebHostEnvironment;

        public VotersController(OnlineVoteApplicationContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }

        // GET: Voters
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalCount = _context.Voters.ToList().Count;
            return _context.Voters != null ?
                        View(await _context.Voters.ToListAsync()) :
                        Problem("Entity set 'OnlineVoteApplicationContext.Voters'  is null.");
        }

        // GET: Voters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Voters == null)
            {
                return NotFound();
            }

            var voter = await _context.Voters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voter == null)
            {
                return NotFound();
            }

            return View(voter);
        }

        // GET: Voters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Voters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VoterRequest voterRequest)
        {
            Voter voter = new Voter();
            if (ModelState.IsValid)
            {
                var checkUser = _context.Voters.Where(x => x.EmailId == voterRequest.EmailId).FirstOrDefault();
                if (voterRequest.Age >= 18 && checkUser == null)
                {
                    IFormFile file = voterRequest.IDProofImage;
                    string ImagePath = uploadImage(file);

                    voter.VoterName = voterRequest.VoterName;
                    voter.Age = voterRequest.Age;
                    voter.MobileNo = voterRequest.MobileNo;
                    voter.EmailId = voterRequest.EmailId;
                    voter.Password = voterRequest.Password;
                    voter.IDProofImage = ImagePath;
                    voter.CreatedDate = DateTime.Now;
                    voter.IsValidUser = true;
                    _context.Add(voter);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login","Account");
                }
                else if(voterRequest.Age < 18) {
                    ViewBag.AgeMessage = "The voter age is below 18. not valid user for this election";
                }
                else if (checkUser != null)
                {
                    ViewBag.ExistMessage = "The voter emailId is already exist";
                }
            }
            return View(voter);
        }

        // GET: Voters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Voters == null)
            {
                return NotFound();
            }

            var voter = await _context.Voters.FindAsync(id);
            if (voter == null)
            {
                return NotFound();
            }
            return View(voter);
        }

        // POST: Voters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Voter voter)
        {
            if (id != voter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (voter.Age >= 18)
                    {
                        voter.ModifiedDate = DateTime.Now;
                        voter.IsApprovedUser = false;
                        _context.Update(voter);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ViewBag.AgeMessage = "The voter age is below 18. not valid user for this election";
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoterExists(voter.Id))
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
            return View(voter);
        }

        // GET: Voters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Voters == null)
            {
                return NotFound();
            }

            var voter = await _context.Voters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voter == null)
            {
                return NotFound();
            }

            return View(voter);
        }

        // POST: Voters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Voters == null)
            {
                return Problem("Entity set 'OnlineVoteApplicationContext.Voters'  is null.");
            }
            var voter = await _context.Voters.FindAsync(id);
            if (voter != null)
            {
                _context.Voters.Remove(voter);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoterExists(int id)
        {
            return (_context.Voters?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> VotingSystem()
        {
            return _context.ContestManagements != null ?
                        View(await _context.ContestManagements.ToListAsync()) :
                        Problem("Entity set 'OnlineVoteApplicationContext.Voters'  is null.");
        }

        public async Task<IActionResult> ConfirmUser(int? id)
        {
            if (id == null || _context.ContestManagements == null)
            {
                return NotFound();
            }

            var contestent = await _context.ContestManagements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contestent == null)
            {
                return NotFound();
            }

            return View(contestent);
        }

        // POST: Voters/Delete/5
        [HttpPost, ActionName("ConfirmUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmUser(int id)
        {
            if (_context.ContestManagements == null)
            {
                return Problem("Entity set 'OnlineVoteApplicationContext.Voters'  is null.");
            }
            var contest = await _context.ContestManagements.FindAsync(id);
            int voterID = Convert.ToInt32(TempData["LoginUserID"]);
            var voter = await _context.Voters.FindAsync(voterID);
            if (voter != null && contest != null)
            {
                VotingSystem votingSystem = new VotingSystem();
                votingSystem.ContestentName = contest.ContestentName;
                votingSystem.ContestentEmail = contest.EmailId;
                votingSystem.ContestentImage = contest.ContestentImage;
                votingSystem.VoterName = voter.VoterName;
                votingSystem.VoterEmail = voter.EmailId;
                votingSystem.PartyName = contest.PartyName;
                votingSystem.Symbol = contest.Symbol;
                votingSystem.CreatedDate = DateTime.Now;
                _context.VotingSystem.Add(votingSystem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Login","Account");
        }
        private string uploadImage(IFormFile file)
        {
            string fileName = null;
            if (file != null)
            {
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return fileName;
        }

    }
}
