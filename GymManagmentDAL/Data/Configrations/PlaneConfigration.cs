using GymManagmentDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Data.Configrations
{
	internal class PlaneConfigration : IEntityTypeConfiguration<Plane>
	{
		public void Configure(EntityTypeBuilder<Plane> builder)
		{
			builder.Property(X => X.Name).HasColumnType("varchar").HasMaxLength(50);
			builder.Property(X => X.Description).HasColumnType("varchar").HasMaxLength(100);
			builder.Property(X => X.Price).HasPrecision(10, 2);

			builder.ToTable(tb =>
			{
				tb.HasCheckConstraint("PlaneDurationCheck", "DurationDays Between 1 and 365");

			});


		}
	}
}
