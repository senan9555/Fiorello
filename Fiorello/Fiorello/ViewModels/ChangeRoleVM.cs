using System.Collections.Generic;

namespace Fiorello.ViewModels
{
    public class ChangeRoleVM
    {
        public string Role { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
