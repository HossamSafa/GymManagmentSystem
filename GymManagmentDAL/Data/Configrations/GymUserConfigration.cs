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
	internal class GymUserConfigration<T> : IEntityTypeConfiguration<T> where T : GymUser
	{
		public void Configure(EntityTypeBuilder<T> builder)
		{
			builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(50);
			builder.Property(x => x.Email).HasColumnType("varchar").HasMaxLength(100);
			builder.Property(x => x.Phone).HasColumnType("varchar").HasMaxLength(11);


			builder.ToTable(tb =>
			{
				tb.HasCheckConstraint("GymUserValidEmailCheck", "Email Like '_%@_%._%'");
				tb.HasCheckConstraint("GymUserValidphoneCheck", "Phone Like '01%' and Phone Not Like '%[^0-9]%'");


			});

			builder.HasIndex(X => X.Email).IsUnique();
			builder.HasIndex(X => X.Phone).IsUnique();

			builder.OwnsOne(x => x.Address, addressbuilder =>
			{
				addressbuilder.Property(x => x.Street).HasColumnName("Street").HasColumnType("varchar").HasMaxLength(30);
				addressbuilder.Property(x => x.City).HasColumnName("City").HasColumnType("varchar").HasMaxLength(30);
				addressbuilder.Property(x => x.BuildingNumber).HasColumnName("BuildingNumber");



			});


		}
	}
}
