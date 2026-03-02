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
	internal class MembershipConfigration : IEntityTypeConfiguration<MemberShip>
	{
		public void Configure(EntityTypeBuilder<MemberShip> builder)
		{
			builder.Property(X => X.CreatedAt).HasColumnName("StartDate").HasDefaultValueSql("GETDATE()");

			builder.HasKey(x=> new
			{
				x.MemberID,
				x.PlaneId
			});
			builder.Ignore(X=>X.Id);
		}
	}
}
