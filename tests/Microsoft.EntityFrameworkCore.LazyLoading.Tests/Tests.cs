using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.LazyLoading.Tests.Configuration;
using Microsoft.EntityFrameworkCore.LazyLoading.Tests.Models;
using Xunit;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Tests
{
    public class Tests : DatabaseTestBase
    {
        [Fact]
        public void ShouldNotThrowWhenExecutingTheSameQueryOnTwoDifferentDbContextsAfterDisposingTheFirstOne()
        {
            for (var i = 0; i < 2; ++i)
            {
                using (var ctx = CreateDbContext(ConnectionStringSelector.Main, ContextInitializationOptions.SeedSampleData))
                {
                    var instructorAbercrombie = ctx.Set<Instructor>().First(x => x.LastName == "Abercrombie");
                    var coursesOfinstructorAbercrombie = instructorAbercrombie.CourseAssignments.Select(x => x.Course);

                    Assert.NotEmpty(coursesOfinstructorAbercrombie);
                }
            }
        }

        [Fact]
        public void ShouldUseTheCorrectDbContextNotTheNewestOne()
        {
            using (var ctx = CreateDbContext(ConnectionStringSelector.Main, ContextInitializationOptions.CleanupData))
            {
                var course1 = new Course { CourseId = 1, Title = "Course 1", Department = new Department() };
                var course2 = new Course { CourseId = 2, Title = "Course 2", Department = new Department() };
                var instructor = new Instructor
                {
                    FirstMidName = "Entity",
                    LastName = "Framework",
                    OfficeAssignment = new OfficeAssignment
                    {
                        Location = "Washington"
                    },
                    CourseAssignments = new List<CourseAssignment>
                    {
                        new CourseAssignment
                        {
                            Course = course1
                        },
                        new CourseAssignment
                        {
                            Course = course2
                        }
                    }
                };

                ctx.Instructors.Add(instructor);
                ctx.SaveChanges();
            }

            using (var ctx = CreateDbContext(ConnectionStringSelector.Second, ContextInitializationOptions.CleanupData))
            {
                var instructor = new Instructor
                {
                    FirstMidName = "Entity",
                    LastName = "Framework",
                    OfficeAssignment = new OfficeAssignment
                    {
                        Location = "New York"
                    },
                    CourseAssignments = new List<CourseAssignment>()
                };

                ctx.Instructors.Add(instructor);
                ctx.SaveChanges();
            }

            using (var ctx = CreateDbContext(ConnectionStringSelector.Main, ContextInitializationOptions.DoNothing))
            {
                using (var newCtx = CreateDbContext(ConnectionStringSelector.Second, ContextInitializationOptions.DoNothing))
                {
                    var newInstructor = newCtx.Instructors.FirstOrDefault();
                    Assert.NotNull(newInstructor);
                    Assert.Equal("New York", newInstructor.OfficeAssignment.Location);

                    var instructor = ctx.Instructors.FirstOrDefault();
                    Assert.NotNull(instructor);

                    Assert.Equal("Washington", instructor.OfficeAssignment.Location);
                    Assert.Equal(2, instructor.CourseAssignments.Count);
                }
            }
        }
    }
}
