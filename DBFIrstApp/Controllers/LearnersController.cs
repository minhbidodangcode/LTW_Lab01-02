using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBFIrstApp.Models;

namespace DBFIrstApp.Controllers
{
    public class LearnersController : Controller
    {
        private readonly SchoolContext _context;
        private int pageSize = 3;

        public LearnersController(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var learners = await _context.Learners
                                    .Include(m => m.Major!)
                                    .OrderBy(l => l.LastName)
                                    .ToListAsync();
            int pageNum = (int)Math.Ceiling(learners.Count / (double)pageSize);
            ViewBag.PageNum = pageNum;
            var result = learners.Take(pageSize).ToList();
            return View(result);
        }

        public async Task<IActionResult> LearnerByMajorID(int? mid)
        {
            List<Learner> learners;

            if (mid == null || mid == 0)
            {
                learners = await _context.Learners
                    .Include(l => l.Major!) 
                    .OrderBy(l => l.LastName)
                    .ToListAsync(); 
            }
            else
            {
                learners = await _context.Learners
                    .Where(l => l.MajorId == mid)
                    .Include(l => l.Major!)
                    .OrderBy(l => l.LastName)
                    .ToListAsync();
            }
            return PartialView("LearnerTable", learners);
        }

        // GET: Learners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .Include(l => l.Major)
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        // GET: Learners/Create
        public IActionResult Create()
        {
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorName");
            return View();
        }

        // POST: Learners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearnerId,LastName,FirstMidName,EnrollmentDate,MajorId")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorName", learner.MajorId);
            return View(learner);
        }

        // GET: Learners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners.FindAsync(id);
            if (learner == null)
            {
                return NotFound();
            }
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorName", learner.MajorId);
            return View(learner);
        }

        // POST: Learners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LearnerId,LastName,FirstMidName,EnrollmentDate,MajorId")] Learner learner)
        {
            if (id != learner.LearnerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learner.LearnerId))
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
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorName", learner.MajorId);
            return View(learner);
        }

        // GET: Learners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .Include(l => l.Major)
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        // POST: Learners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learner = await _context.Learners.FindAsync(id);
            if (learner != null)
            {
                _context.Learners.Remove(learner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearnerExists(int id)
        {
            return _context.Learners.Any(e => e.LearnerId == id);
        }
    public async Task<IActionResult> LearnerFilter(int? mid, string? keyword, int? pageIndex)
        {
            var learners = (IQueryable<Learner>) _context.Learners;
            int page = (int) (pageIndex == null || pageIndex <= 0 ? 1 : pageIndex);
            if (mid != null && mid != 0)
            {
                learners = learners.Where(l => l.MajorId == mid);
                ViewBag.mid = mid;
            }
            if(keyword != null && keyword.Trim().Length > 0)
            {
                learners = learners.Where(l => l.FirstMidName.ToLower().Contains(keyword.ToLower()));
                ViewBag.keyword = keyword;
            }
            int pageNum = (int)Math.Ceiling(learners.Count() / (double)pageSize);
            ViewBag.PageNum = pageNum;
            var result = learners.Skip(pageSize * (page - 1))
                                 .Take(pageSize)
                                 .Include(m => m.Major!);             
            return PartialView("LearnTable", result);
        }
    }
    
}
