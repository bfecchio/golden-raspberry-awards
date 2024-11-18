using Awards.Domain.Entities;
using System.Collections.Generic;

namespace Awards.Api.Tests.Fixtures
{
    internal static class MovieFixture
    {
        public static IEnumerable<Movie> SetFixture()
        {
            yield return new Movie("Movie A", 2008, "Producer 1", "Studio A", true);
            yield return new Movie("Movie B", 2009, "Producer 1", "Studio B", true);
            yield return new Movie("Movie C", 2010, "Producer 2", "Studio C", true);
            yield return new Movie("Movie D", 2015, "Producer 2", "Studio D", true);
            yield return new Movie("Movie E", 2020, "Producer 3", "Studio E", false);
        }
    }
}
