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
	internal class SessionConfigration : IEntityTypeConfiguration<Session>
	{
		public void Configure(EntityTypeBuilder<Session> builder)
		{
			builder.ToTable(tb =>
			{
				tb.HasCheckConstraint("SessionCapactyCheck", "Capacty Between 1 and 25");
				tb.HasCheckConstraint("SessionEndDateCheck", "EndDate > StartDate");


			});

			builder.HasOne(x => x.SessionCategory).WithMany(X => X.Sessions).HasForeignKey(x => x.CategoryId);
			builder.HasOne(x => x.SessionTrainer).WithMany(x => x.TrainerSessions).HasForeignKey(x => x.TrainerId);
		}
	}
}
