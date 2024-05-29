using Newtonsoft.Json;

namespace BookRental.Models
{
	public class UserProfile
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Prenom { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }

		public UserProfile()
		{
			Id = 0;
			Name = null;
			Prenom = null;
			Email = null;
			Role = null;
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}

	}
}