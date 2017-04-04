using System.Linq;
using Microsoft.EntityFrameworkCore.LazyLoading.Tests.Models;
using Xunit;

namespace Microsoft.EntityFrameworkCore.LazyLoading.Tests
{
    public class Tests : DatabaseTestBase
    {
        [Fact]
        public void ShouldNotThrowWhenExecutingTheSameQueryOnTwoDifferentDbContextsAfterDisposingTheFirstOne()
        {
            for (var i = 0; i < 3; ++i)
            {
                using (var ctx = CreateDbContext())
                {
                    var instructorAbercrombie = ctx.Set<Instructor>().First(x => x.LastName == "Abercrombie");
                    var coursesOfinstructorAbercrombie = instructorAbercrombie.CourseAssignments.Select(x => x.Course);

                    Assert.NotEmpty(coursesOfinstructorAbercrombie);
                }
            }
        }
    }
}
