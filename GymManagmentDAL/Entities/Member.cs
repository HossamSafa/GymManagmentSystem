using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Entities
{
	public class Member :GymUser
	{
		//Joindate==Createdat==> base entity
		public string phote { get; set; } = null!;

		#region Member - Healthrecord
		public HealthRecord healthRecord { get; set; } = null!;
		#endregion

		#region Member - Membership
		public ICollection<MemberShip> MemberShips { get; set; } = null!;
		#endregion

		#region Member - MemberSession
		public ICollection<MemberSession> MemberSessions { get; set; } = null!;
		#endregion

	}
}
