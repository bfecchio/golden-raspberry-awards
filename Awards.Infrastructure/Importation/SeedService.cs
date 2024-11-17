using Awards.Application.Core.Abstractions.Importation;
using Awards.Infrastructure.Importation.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Awards.Infrastructure.Importation
{
    internal sealed class SeedService : ISeedService
    {
        #region Read-Only Fields

        private readonly SeedSettings _seedSettings;

        #endregion

        #region Constructors

        public SeedService(IOptions<SeedSettings> seedSettingsOptions)
            => _seedSettings = seedSettingsOptions.Value ?? throw new ArgumentNullException(nameof(seedSettingsOptions));

        #endregion

        public Task Load()
        {
            throw new NotImplementedException();
        }
    }
}
