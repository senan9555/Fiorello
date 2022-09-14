using System.ComponentModel.DataAnnotations;

namespace Fiorello.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool  IsDeactive { get; set; }
        public string Role { get; set; }
    }
}
