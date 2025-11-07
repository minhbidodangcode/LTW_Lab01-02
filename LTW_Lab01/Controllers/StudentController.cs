using LTW_Lab01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace LTW_Lab01.Controllers
{
    public class StudentController : Controller
    {

        private static List<Student> listStudents = new List<Student>()
        {
            new Student() { Id = 101, Name = "Hải Nam", Branch = Branch.IT, Gender = Gender.Male, IsRegular=true, Address = "A1-2018", Email = "nam@g.com", DateOfBirth = new DateTime(2000, 1, 1) },
            new Student() { Id = 102, Name = "Minh Tú", Branch = Branch.BE, Gender = Gender.Female, IsRegular=true, Address = "A1-2019", Email = "tu@g.com", DateOfBirth = new DateTime(2001, 2, 2) },
            new Student() { Id = 103, Name = "Hoàng Phong", Branch = Branch.CE, Gender = Gender.Male, IsRegular = false, Address = "A1-2020", Email = "phong@g.com", DateOfBirth = new DateTime(2002, 3, 3) },
            new Student() { Id = 104, Name = "Xuân Mai", Branch = Branch.EE, Gender = Gender.Female, IsRegular = false, Address = "A1-2021", Email = "mai@g.com", DateOfBirth = new DateTime(2003, 4, 4) }
        };

        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public ActionResult Index()
        {
            return View(listStudents);
        }

        [HttpGet]
        public IActionResult Create()
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();

            ViewBag.AllBranches = new List<SelectListItem>()
            {
                new SelectListItem { Text = "IT (Công nghệ thông tin)", Value = Branch.IT.ToString() },
                new SelectListItem { Text = "BE (Kinh tế)", Value = Branch.BE.ToString() },
                new SelectListItem { Text = "CE (Công trình)", Value = Branch.CE.ToString() },
                new SelectListItem { Text = "EE (Điện - Điện tử)", Value = Branch.EE.ToString() }
            };

            return View();
        }

        [HttpPost]
        public IActionResult Create(Student s)
        {
            s.Id = listStudents.Last<Student>().Id + 1;
            listStudents.Add(s);

            return RedirectToAction("Index"); 
        }

        public IActionResult Details(int id)
        {
            var student = listStudents.FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }
            return View(student); 
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = listStudents.FirstOrDefault(s => s.Id == id);
            
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(Student s, IFormFile? newImageFile) 
        {

            if (ModelState.IsValid)
            {

                var studentToUpdate = listStudents.FirstOrDefault(st => st.Id == s.Id);

                if (studentToUpdate != null)
                {
                    if(newImageFile != null)
                    {
                        if(!string.IsNullOrEmpty(studentToUpdate.ImageUrl))
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, studentToUpdate.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + newImageFile.FileName;
                        string imagePath = Path.Combine(wwwRootPath, "images", uniqueFileName);
                        using (var fileStream = new FileStream(imagePath, FileMode.Create))
                        {
                            newImageFile.CopyTo(fileStream);
                        }
                        s.ImageUrl = "/images/" + uniqueFileName;
                    }

                    else if(string.IsNullOrEmpty(s.ImageUrl) && !string.IsNullOrEmpty(studentToUpdate.ImageUrl)){
                        s.ImageUrl = studentToUpdate.ImageUrl;
                    }

                    studentToUpdate.Name = s.Name;
                    studentToUpdate.DateOfBirth = s.DateOfBirth;
                    studentToUpdate.Gender = s.Gender;
                    studentToUpdate.Branch = s.Branch;
                    studentToUpdate.Address = s.Address;
                    studentToUpdate.Email = s.Email;
                    studentToUpdate.Password = s.Password;
                    studentToUpdate.IsRegular = s.IsRegular;
                    studentToUpdate.ImageUrl = s.ImageUrl;

                    return RedirectToAction("Index");
                }

                return NotFound();
            }

            return View(s);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
          
            var student = listStudents.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost, ActionName("Delete")] 
        public IActionResult DeleteConfirmed(int id)
        {
            var student = listStudents.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                listStudents.Remove(student);
            }

            return RedirectToAction("Index");
        }
    }
}