using Awards.Application.Core.Abstractions.Importation;
using Awards.Domain.Entities;
using Awards.Infrastructure.Importation.Settings;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

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

        public List<Movie> Load()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                PrepareHeaderForMatch = args => args.Header.ToLower(),
                IgnoreBlankLines = true,
                HeaderValidated = null,
                MemberTypes = MemberTypes.Fields,
                Quote = '\'',
                Delimiter = ";",
                BadDataFound = null,
                IncludePrivateMembers = true,
            }; 

            using var reader = new StreamReader(_seedSettings.FilePath);
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<MovieMap>();                
                return csv.GetRecords<Movie>().ToList();
            }
        }

        #region Private Classes

        private sealed class MovieMap : ClassMap<Movie>
        {
            public MovieMap()
            {
                Map(m => m.Id).Ignore();
                Map(m => m.DomainEvents).Ignore();

                Map(m => m.Year).Name("year");
                Map(m => m.Title).Name("title");
                Map(m => m.Studios).Name("studios");
                Map(m => m.Producers).Name("producers");
                Map(m => m.Winner).Name("winner").TypeConverter<WinnerConverter<bool>>();
            }
        }

        private sealed class WinnerConverter<TValue> : DefaultTypeConverter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
                => string.Equals(text, "yes");

            public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
                => (value is bool booleanValue)
                    ? "yes"
                    : "";
        }

        #endregion
    }
}
