using Awards.Domain.Core.Primitives;
using Awards.Domain.Core.Utility;

namespace Awards.Domain.Entities
{
    public class Movie : AggregateRoot
    {
        #region Properties

        public string Title { get; private set; }
        public int Year { get; private set; }
        public string Producers { get; private set; }
        public string Studios { get; private set; }
        public bool Winner { get; private set; }

        #endregion

        #region Constructors

        protected Movie()
        { }

        public Movie(string title, int year, string producers, string studios, bool winner)
        {
            Ensure.NotEmpty(title, "The title is required.", nameof(title));
            Ensure.NotEmpty(producers, "The producers is required.", nameof(producers));
            Ensure.NotEmpty(studios, "The studios is required.", nameof(studios));

            Title = title;
            Year = year;
            Producers = producers;
            Studios = studios;
            Winner = winner;
        }

        #endregion
    }
}
