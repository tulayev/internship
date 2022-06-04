using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Task5.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

#nullable enable
        public ICollection<MessageUser>? MessageUsers { get; set; }
#nullable disable
    }
}
