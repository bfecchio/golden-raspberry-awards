using Awards.Domain.Entities;
using System.Collections.Generic;

namespace Awards.Application.Core.Abstractions.Importation
{
    public interface ISeedService
    {
        List<Movie> Load();
    }
}
