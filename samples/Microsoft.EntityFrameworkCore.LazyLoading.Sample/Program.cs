using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.LazyLoading.Sample.Data;
using Microsoft.EntityFrameworkCore.LazyLoading.Sample.Data.Factory;
using Microsoft.EntityFrameworkCore.LazyLoading.Sample.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var lazyFactory = new LazySchoolContextFactory();
            var defaultFactory = new DefaultSchoolContextFactory();
            var factoryOptions = new DbContextFactoryOptions();

            using (var dbContext = lazyFactory.Create(factoryOptions))
            {
                DbInitializer.Initialize(dbContext);
            }

            using (var dbContext = lazyFactory.Create(factoryOptions))
            {
                var student = dbContext.Students.First();
                var deptName = student.Enrollments.First().Course.Department.Name;
                Console.WriteLine($"Department name is '{deptName}'. LazyLoading enabled, no need to call .Include().");

                Console.WriteLine("Querying instructor with an OfficeAssignment.");
                var instructorWithOfficeAssignment = dbContext.Instructors.First(x => x.OfficeAssignment != null);
                Console.WriteLine($"Instructor has {(instructorWithOfficeAssignment.OfficeAssignment == null ? "no (WTF? We just queried instructors with OfficeAssignment!)" : "an")} OfficeAssignment.");
            }

            using (var dbContext = defaultFactory.Create(factoryOptions))
            {
                var student = dbContext.Students.First();
                try
                {
                    var deptName = student.Enrollments.First().Course.Department.Name;
                    Console.WriteLine($"Oops... Something didn't work. LazyLoading should not be enabled by default.");

                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine($"{nameof(ArgumentNullException)} occured because LazyLoading was not enabled and .Include() was not called.");
                }

                Console.WriteLine("Querying instructor with an OfficeAssignment.");
                var instructorWithOfficeAssignment = dbContext.Instructors.First(x => x.OfficeAssignment != null);
                Console.WriteLine($"Instructor has {(instructorWithOfficeAssignment.OfficeAssignment == null ? "no (WTF? We just queried instructors with OfficeAssignment!)" : "an")} OfficeAssignment.");
            }
        }
    }
}