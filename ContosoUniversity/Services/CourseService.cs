using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ContosoUniversity.DAL;
using ContosoUniversity.Interfaces;
using ContosoUniversity.Models;

namespace ContosoUniversity.Services
{
    public class CourseService : ICourseService
    {
        private readonly SchoolContext _db = new SchoolContext();

        public List<Department> GetDepartments()
        {
            return _db.Departments.OrderBy(q => q.Name).ToList();
        }

        public List<Course> GetCourses(int? selectedDepartment)
        {
            int departmentID = selectedDepartment.GetValueOrDefault();
            return _db.Courses
                .Where(c => !selectedDepartment.HasValue || c.DepartmentID == departmentID)
                .OrderBy(d => d.CourseID)
                .Include(d => d.Department)
                .ToList();
        }

        public Course FindById(int? id)
        {
            return _db.Courses.Find(id);
        }

        public void Create(Course course)
        {
            _db.Courses.Add(course);
            _db.SaveChanges();
        }

        public void Update(Course course)
        {
            Course dbRecord = FindById(course.CourseID);

            dbRecord.Credits = course.Credits;            
            dbRecord.DepartmentID = course.DepartmentID;
            dbRecord.Title = course.Title;

            _db.SaveChanges();
        }

        public void DeleteById(int id)
        {
            Course course = _db.Courses.Find(id);
            if (course == null)
            {
                return;
            }
            _db.Courses.Remove(course);
            _db.SaveChanges();
        }

        public int UpdateCourseCredits(int? multiplier)
        {
            int recCnt = _db.Database.ExecuteSqlCommand("UPDATE Course SET Credits = Credits * {0}", multiplier);
            return recCnt;
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}