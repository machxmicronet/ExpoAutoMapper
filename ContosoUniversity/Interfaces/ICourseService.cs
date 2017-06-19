using System;
using System.Collections.Generic;
using ContosoUniversity.Models;

namespace ContosoUniversity.Interfaces
{
    public interface ICourseService : IDisposable
    {
        List<Department> GetDepartments();
        List<Course> GetCourses(int? selectedDepartment);
        Course FindById(int? id);
        void Create(Course course);
        void Update(Course course);
        void DeleteById(int id);
        int UpdateCourseCredits(int? multiplier);   
    }
}