using Awards.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Awards.Persistence.Configurations
{
    internal sealed class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {            
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Year);
            builder.Property(p => p.Title).IsRequired();
            builder.Property(p => p.Producers).IsRequired();
            builder.Property(p => p.Studios).IsRequired();
            builder.Property(p => p.Winner);
        }
    }
}
