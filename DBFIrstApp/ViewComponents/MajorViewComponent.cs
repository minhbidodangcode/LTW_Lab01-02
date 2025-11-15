using DBFIrstApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LTW_Lab01.ViewComponents
{
    public class MajorViewComponent:ViewComponent
    {
        SchoolContext _context ;

        public MajorViewComponent(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var majors = await _context.Majors
                                     .OrderBy(m => m.MajorName)
                                     .ToListAsync();
            return View("RenderMajor", majors);
        }
    }
}
