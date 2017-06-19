using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using ContosoUniversity.Models;
using System.Data.Entity.Infrastructure;
using AutoMapper;
using ContosoUniversity.Interfaces;
using ContosoUniversity.ViewModels;

namespace ContosoUniversity.Controllers
{
    public class CourseController : Controller
    {
        private ICourseService _courseService;
        
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // GET: Course
        public ActionResult Index(int? selectedDepartment)
        {
            List<Department> departments = _courseService.GetDepartments();
            ViewBag.SelectedDepartment = new SelectList(departments, "DepartmentID", "Name", selectedDepartment);
            
            List<Course> courses = _courseService.GetCourses(selectedDepartment);
            List<CourseViewModel> courseViewModels = Mapper.Map<List<Course>, List<CourseViewModel>>(courses);

            return View(courseViewModels);
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course course = _courseService.FindById(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            
            CourseViewModel courseViewModel = Mapper.Map<Course, CourseViewModel>(course);
            return View(courseViewModel);
        }


        public ActionResult Create()
        {
            PopulateDepartmentsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits,DepartmentID")]Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _courseService.Create(course);
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", @"Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateDepartmentsDropDownList(course.DepartmentID);

            CourseViewModel courseViewModel = Mapper.Map<Course, CourseViewModel>(course);
            return View(courseViewModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course course = _courseService.FindById(id);

            if (course == null)
            {
                return HttpNotFound();
            }
            PopulateDepartmentsDropDownList(course.DepartmentID);

            CourseViewModel courseViewModel = Mapper.Map<Course, CourseViewModel>(course);
            return View(courseViewModel);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course courseToUpdate = _courseService.FindById(id);

            if (TryUpdateModel(courseToUpdate, "", new[] { "Title", "Credits", "DepartmentID" }))
            {
                try
                {
                    _courseService.Update(courseToUpdate);

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", @"Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateDepartmentsDropDownList(courseToUpdate.DepartmentID);

            CourseViewModel courseViewModel = Mapper.Map<Course, CourseViewModel>(courseToUpdate);
            return View(courseViewModel);
        }

        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            List<Department> departments = _courseService.GetDepartments();
            ViewBag.DepartmentID = new SelectList(departments, @"DepartmentID", @"Name", selectedDepartment);
        }
        
        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = _courseService.FindById(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            CourseViewModel courseViewModel = Mapper.Map<Course, CourseViewModel>(course);
            return View(courseViewModel);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _courseService.DeleteById(id);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateCourseCredits()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateCourseCredits(int? multiplier)
        {
            if (multiplier != null)
            {
                ViewBag.RowsAffected = _courseService.UpdateCourseCredits(multiplier);
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _courseService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
