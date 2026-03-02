using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Entities
{
	public class MemberShip : BaseEntity
	{
		//Start Date => Created At
		public DateTime EndDate { get; set; }
		public string Staues { get
			{
				if (EndDate >= DateTime.Now)
					return "Expired";
				else
					return "Active";
			} }
		public int MemberID { get; set; }
		public Member Member { get; set; } = null!;

		public int PlaneId { get; set; }
		public Plane Plane { get; set; } = null!;

	}
}
