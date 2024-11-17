using System.Threading.Tasks;

namespace Awards.Application.Core.Abstractions.Importation
{
    public interface ISeedService
    {
        Task Load();
    }
}
