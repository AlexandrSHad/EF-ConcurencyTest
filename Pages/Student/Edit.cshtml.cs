using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EF_ConcurrencyTest;
using EF_ConcurrencyTest.Data;

namespace EF_ConcurrencyTest.Pages_Student
{
    public class EditModel : PageModel
    {
        private readonly EF_ConcurrencyTest.Data.DataContext _context;

        public EditModel(EF_ConcurrencyTest.Data.DataContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var studentToUpdate = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);

            // null means Department was deleted by another user.
            if (studentToUpdate == null)
            {
                return await HandleDeletedStudent();
            }

            // Update the RowVersion to the value when this entity was
            // fetched. If the entity has been updated after it was
            // fetched, RowVersion won't match the DB RowVersion and
            // a DbUpdateConcurrencyException is thrown.
            // A second postback will make them match, unless a new 
            // concurrency issue happens.
            _context.Entry(studentToUpdate)
                .Property("RowVersion").OriginalValue = Student.RowVersion;

            if (await TryUpdateModelAsync<Student>(
                studentToUpdate,
                "Student",
                s => s.Name, s => s.BirthDate))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Student)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save. " +
                            "The department was deleted by another user.");
                        return Page();
                    }

                    var dbValues = (Student)databaseEntry.ToObject();
                    await SetDbErrorMessage(dbValues, clientValues, _context);

                    // Save the current RowVersion so next postback
                    // matches unless an new concurrency issue happens.
                    Student.RowVersion = (byte[])dbValues.RowVersion;
                    // Must clear the model error for the next postback.
                    ModelState.Remove("Student.RowVersion");
                }
            }

            return Page();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        private async Task<IActionResult> HandleDeletedStudent()
        {
            Student deletedDepartment = new Student();
            // ModelState contains the posted data because of the deletion error and will overide the Department instance values when displaying Page().
            ModelState.AddModelError(string.Empty,
                "Unable to save. The student was deleted by another user.");
            return Page();
        }

        private async Task SetDbErrorMessage(Student dbValues,
                Student clientValues, DataContext context)
        {

            if (dbValues.Name != clientValues.Name)
            {
                ModelState.AddModelError("Student.Name",
                    $"Current value: {dbValues.Name}");
            }
            if (dbValues.BirthDate != clientValues.BirthDate)
            {
                ModelState.AddModelError("Student.BirthDate",
                    $"Current value: {dbValues.BirthDate:d}");
            }

            ModelState.AddModelError(string.Empty,
                "The record you attempted to edit "
              + "was modified by another user after you. The "
              + "edit operation was canceled and the current values in the database "
              + "have been displayed. If you still want to edit this record, click "
              + "the Save button again.");
        }
    }
}
