using System.Collections.Generic;

namespace Task4.Models.ViewModels
{
    public class UserVM
    {
        public IEnumerable<User> Users { get; set; }
        public User CurrentUser { get; set; }
#nullable enable
        public string? Block { get; set; }
        public string? UnBlock { get; set; }
        public string? Delete { get; set; }
#nullable disable
    }
}
