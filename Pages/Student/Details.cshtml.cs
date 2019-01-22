using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EF_ConcurrencyTest;
using EF_ConcurrencyTest.Data;

namespace EF_ConcurrencyTest.Pages_Student
{
    public class DetailsModel : PageModel
    {
        private readonly EF_ConcurrencyTest.Data.DataContext _context;

        public DetailsModel(EF_ConcurrencyTest.Data.DataContext context)
        {
            _context = context;
        }

        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);

            if (Student == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
