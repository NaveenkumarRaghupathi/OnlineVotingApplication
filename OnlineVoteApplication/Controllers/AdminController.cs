using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineVoteApplication.Migrations;
using OnlineVoteApplication.Models;
using System.Diagnostics.Metrics;
using System.IO;

namespace OnlineVoteApplication.Controllers
{
    public class AdminController : Controller
    {
        private readonly OnlineVoteApplicationContext _context;
        private IWebHostEnvironment WebHostEnvironment;


        public AdminController(OnlineVoteApplicationContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> ViewVoters()
        {
            return _context.Voters != null ?
                        View(await _context.Voters.ToListAsync()) :
                        Problem("Voters List is Empty.");
        }

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
                    var checkUser = _context.Voters.Where(x => x.EmailId == voter.EmailId).FirstOrDefault();
                    if (voter.Age >= 18 && checkUser != null)
                    {
                        voter.IDProofImage = voter.IDProofImage == null ? checkUser.IDProofImage : voter.IDProofImage;
                        voter.ModifiedDate = DateTime.Now;
                        voter.CreatedDate = checkUser.CreatedDate;
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
                return RedirectToAction(nameof(ViewVoters));
            }
            return View(voter);
        }

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

        public async Task<IActionResult> ApproveUser(int id)
        {
            var checkUser = _context.Voters.Where(x => x.Id == id).FirstOrDefault(); ;

            if (ModelState.IsValid)
            {
                try
                {
                    if (checkUser != null)
                    {
                        checkUser.ModifiedDate = DateTime.Now;
                        checkUser.IsApprovedUser = true;
                        _context.Update(checkUser);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoterExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("ViewVoters","Admin");
        }

        public async Task<IActionResult> ViewMPSeats()
        {
            return _context.Voters != null ?
                        View(await _context.Mpseats.ToListAsync()) :
                        Problem("MP Seats List is Empty.");
        }
        public IActionResult CreateMPSeats()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMPSeats(Mpseat mpSeat)
        {
            if (ModelState.IsValid)
            {
                var checkUser = _context.Mpseats.Where(x => x.State == mpSeat.State).FirstOrDefault();
                if (checkUser == null)
                {
                    mpSeat.CreatedDate = DateTime.Now;
                    _context.Add(mpSeat);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ViewMPSeats));
                }
            }
            return View(mpSeat);
        }
        public async Task<IActionResult> MPSeatsEdit(int? id)
        {
            if (id == null || _context.Mpseats == null)
            {
                return NotFound();
            }

            var voter = await _context.Mpseats.FindAsync(id);
            if (voter == null)
            {
                return NotFound();
            }
            return View(voter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MPSeatsEdit(int id, Mpseat mpseat)
        {
            if (id != mpseat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var checkUser = _context.Mpseats.Where(x => x.State == mpseat.State).FirstOrDefault();
                try
                {
                    if (checkUser != null)
                    {
                        mpseat.ModifiedDate = DateTime.Now;
                        mpseat.CreatedDate = checkUser.CreatedDate;
                        _context.Update(mpseat);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MPSeatsExists(mpseat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ViewMPSeats));
            }
            return View(mpseat);
        }
        public async Task<IActionResult> MPSeatsDetails(int? id)
        {
            if (id == null || _context.Mpseats == null)
            {
                return NotFound();
            }

            var mpseat = await _context.Mpseats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mpseat == null)
            {
                return NotFound();
            }

            return View(mpseat);
        }
        public async Task<IActionResult> MPSeatsDelete(int? id)
        {
            if (id == null || _context.Mpseats == null)
            {
                return NotFound();
            }

            var voter = await _context.Mpseats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voter == null)
            {
                return NotFound();
            }

            return View(voter);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MPSeatsDeleteConfirmed(int id)
        {
            if (_context.Mpseats == null)
            {
                return Problem("Entity set 'OnlineVoteApplicationContext.Voters'  is null.");
            }
            var mpseat = await _context.Mpseats.FindAsync(id);
            if (mpseat != null)
            {
                _context.Mpseats.Remove(mpseat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewMPSeats));
        }

        private bool MPSeatsExists(int id)
        {
            return (_context.Mpseats?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> ViewParty()
        {
            return _context.Voters != null ?
                        View(await _context.PartyManagements.ToListAsync()) :
                        Problem("Party List is Empty.");
        }
        public IActionResult CreateParty()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateParty(Party party)
        {
            PartyManagement partyManagement = new PartyManagement();
            if (ModelState.IsValid)
            {
                var checkParty = _context.PartyManagements.Where(x => x.PartyName.Contains(party.PartyName)).FirstOrDefault();
                if (checkParty == null)
                {
                    IFormFile file = party.PartyImage;
                    string ImagePath = uploadImage(file);

                    partyManagement.PartyName = party.PartyName;
                    partyManagement.Symbol = party.Symbol;
                    partyManagement.CreatedDate = DateTime.Now;
                    partyManagement.PartyImage = ImagePath;
                    _context.Add(partyManagement);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ViewParty));
                }
            }
            return View(party);
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

        public async Task<IActionResult> PartyEdit(int? id)
        {
            if (id == null || _context.PartyManagements == null)
            {
                return NotFound();
            }

            var party = await _context.PartyManagements.FindAsync(id);
            if (party == null)
            {
                return NotFound();
            }
            return View(party);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PartyEdit(int id, PartyManagement partyManagement)
        {
            if (id != partyManagement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var checkUser = _context.PartyManagements.Where(x => x.PartyName.Contains(partyManagement.PartyName)).FirstOrDefault();
                try
                {
                    if (checkUser != null)
                    {

                        partyManagement.ModifiedDate = DateTime.Now;
                        partyManagement.CreatedDate = checkUser.CreatedDate;
                        partyManagement.PartyImage = partyManagement.PartyImage == null ? checkUser.PartyImage : partyManagement.PartyImage;
                        _context.Update(partyManagement);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartyExists(partyManagement.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ViewParty));
            }
            return View(partyManagement);
        }
        public async Task<IActionResult> PartyDetails(int? id)
        {
            if (id == null || _context.PartyManagements == null)
            {
                return NotFound();
            }

            var party = await _context.PartyManagements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }
        public async Task<IActionResult> PartyDelete(int? id)
        {
            if (id == null || _context.PartyManagements == null)
            {
                return NotFound();
            }

            var party = await _context.PartyManagements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PartyDeleteConfirmed(int id)
        {
            if (_context.PartyManagements == null)
            {
                return Problem("Entity set 'OnlineVoteApplicationContext.Voters'  is null.");
            }
            var partyManagements = await _context.PartyManagements.FindAsync(id);
            if (partyManagements != null)
            {
                _context.PartyManagements.Remove(partyManagements);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewParty));
        }
        private bool PartyExists(int id)
        {
            return (_context.PartyManagements?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> ViewContestent()
        {
            return _context.Voters != null ?
                        View(await _context.ContestManagements.ToListAsync()) :
                        Problem("Party List is Empty.");
        }
        public IActionResult CreateContestent()
        {
            List<PartyManagement> partyName = new List<PartyManagement>();
            partyName.Clear();
            partyName = _context.PartyManagements.ToList();
            partyName.Insert(0, new PartyManagement { PartyName = "--Select Party Name--" });
            ViewBag.PartyName = new SelectList(partyName.Select(x=> x.PartyName).ToList());

            List<PartyManagement> symbol = new List<PartyManagement>();
            symbol.Clear();
            symbol = _context.PartyManagements.ToList();
            symbol.Insert(0, new PartyManagement { Symbol = "--Select Symbol--" });
            ViewBag.Symbol = new SelectList(symbol.Select(x => x.Symbol).ToList());

            List<Mpseat> stateList = new List<Mpseat>();
            stateList.Clear();
            stateList = _context.Mpseats.ToList();
            stateList.Insert(0, new Mpseat { State = "--Select State--" });
            ViewBag.State = new SelectList(stateList.Select(x => x.State).ToList());
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContestent(ContestManagementRequest contestManagementRequest)
        {
            ContestManagement contestManagement = new ContestManagement();
            if (ModelState.IsValid)
            {
                var checkParty = _context.ContestManagements.Where(x => x.PartyName.Contains(contestManagementRequest.PartyName) && x.EmailId == contestManagementRequest.EmailId).FirstOrDefault();
                if (checkParty == null)
                {
                    IFormFile file = contestManagementRequest.ContestentImage;
                    string ImagePath = uploadImage(file);

                    contestManagement.ContestentName = contestManagementRequest.ContestentName;
                    contestManagement.Age = contestManagementRequest.Age;
                    contestManagement.MobileNo = contestManagementRequest.MobileNo;
                    contestManagement.EmailId = contestManagementRequest.EmailId;
                    contestManagement.PartyName = contestManagementRequest.PartyName;
                    contestManagement.Symbol = contestManagementRequest.Symbol;
                    contestManagement.State = contestManagementRequest.State;
                    contestManagement.City = contestManagementRequest.City;
                    contestManagement.CreatedDate = DateTime.Now;
                    contestManagement.ContestentImage = ImagePath;
                    _context.Add(contestManagement);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ViewContestent));
                }
            }
            return View(contestManagementRequest);
        }

        public async Task<IActionResult> ContestentEdit(int? id)
        {
            if (id == null || _context.ContestManagements == null)
            {
                return NotFound();
            }

            var contestent = await _context.ContestManagements.FindAsync(id);
            if (contestent == null)
            {
                return NotFound();
            }
            return View(contestent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContestentEdit(int id, ContestManagement contestManagement)
        {
            if (id != contestManagement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var checkUser = await _context.ContestManagements.FirstOrDefaultAsync(x => x.EmailId == contestManagement.EmailId);
                try
                {
                    if (checkUser != null)
                    {
                        contestManagement.ContestentImage = contestManagement.ContestentImage == null ? checkUser.ContestentImage : contestManagement.ContestentImage;
                        contestManagement.CreatedDate = checkUser.CreatedDate;
                        contestManagement.ModifiedDate = DateTime.Now;
                        _context.Update(contestManagement);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContestentExists(contestManagement.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ViewContestent));
            }
            return View(contestManagement);
        }
        public async Task<IActionResult> ContestentDetails(int? id)
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
        public async Task<IActionResult> ContestentDelete(int? id)
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

        [HttpPost, ActionName("ContestentDeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContestentDeleteConfirmed(int id)
        {
            if (_context.ContestManagements == null)
            {
                return Problem("Entity set 'OnlineVoteApplicationContext.Voters'  is null.");
            }
            var contestent = await _context.ContestManagements.FindAsync(id);
            if (contestent != null)
            {
                _context.ContestManagements.Remove(contestent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewContestent));
        }
        private bool ContestentExists(int id)
        {
            return (_context.ContestManagements?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> ViewElectionResult()
        {
            return _context.VotingSystem != null ?
                        View(await _context.VotingSystem.ToListAsync()) :
                        Problem("Election Result List is Empty.");
        }
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.DMK = await _context.VotingSystem.CountAsync(x => x.PartyName == "DMK");
            ViewBag.Congrass = await _context.VotingSystem.CountAsync(x => x.PartyName == "Congrass");

            return View();
        }

        public async Task<IActionResult> ViewAdminUsers()
        {
            return _context.AdminUsers != null ?
                        View(await _context.AdminUsers.ToListAsync()) :
                        Problem("AdminUsers List is Empty.");
        }
        public IActionResult CreateAdminUsers()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdminUsers(AdminUsers adminUsers)
        {
            if (ModelState.IsValid)
            {
                var checkParty = _context.AdminUsers.Where(x => x.EmailID == adminUsers.EmailID).FirstOrDefault();
                if (checkParty == null)
                {
                    adminUsers.CreatedDate = DateTime.Now;
                    _context.Add(adminUsers);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ViewAdminUsers));
                }
            }
            return View(adminUsers);
        }

        public async Task<IActionResult> AdminUsersEdit(int? id)
        {
            if (id == null || _context.AdminUsers == null)
            {
                return NotFound();
            }

            var party = await _context.AdminUsers.FindAsync(id);
            if (party == null)
            {
                return NotFound();
            }
            return View(party);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminUsersEdit(int id, AdminUsers adminUsers)
        {
            if (id != adminUsers.Id)
            {
                return NotFound();
            }

            var checkUser = _context.AdminUsers.Where(x => x.EmailID == adminUsers.EmailID).FirstOrDefault();
            try
            {
                if (checkUser != null)
                {
                    adminUsers.ModifiedDate = DateTime.Now;
                    adminUsers.CreatedDate = checkUser.CreatedDate;
                    adminUsers.Password = checkUser.Password;
                    _context.Update(adminUsers);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminUsersExists(adminUsers.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(ViewAdminUsers));
        }
        public async Task<IActionResult> AdminUsersDetails(int? id)
        {
            if (id == null || _context.AdminUsers == null)
            {
                return NotFound();
            }

            var party = await _context.AdminUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }
        public async Task<IActionResult> AdminUsersDelete(int? id)
        {
            if (id == null || _context.AdminUsers == null)
            {
                return NotFound();
            }

            var party = await _context.AdminUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminUsersConfirmed(int id)
        {
            if (_context.AdminUsers == null)
            {
                return Problem("Entity set 'OnlineVoteApplicationContext.Voters'  is null.");
            }
            var adminUsers = await _context.AdminUsers.FindAsync(id);
            if (adminUsers != null)
            {
                _context.AdminUsers.Remove(adminUsers);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewAdminUsers));
        }
        private bool AdminUsersExists(int id)
        {
            return (_context.AdminUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
