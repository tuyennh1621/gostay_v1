using System.ComponentModel.DataAnnotations;

namespace GoStay.Web.Model
{
    public class IdentityUser
    {
        public bool IsAuthenticated { get; set; }
        [Required]
        public string Email { get; set; }
        public string Avatar { get; set; }
		public string FullName { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
