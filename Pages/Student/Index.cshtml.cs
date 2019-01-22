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
    public class IndexModel : PageModel
    {
        private readonly EF_ConcurrencyTest.Data.DataContext _context;

        public IndexModel(EF_ConcurrencyTest.Data.DataContext context)
        {
            _context = context;
        }

        public IList<Student> Student { get;set; }

        public async Task OnGetAsync()
        {
            Student = await _context.Students.ToListAsync();
        }
    }
}
