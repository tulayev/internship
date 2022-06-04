using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Task5.Models
{
    public class Message
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        public string Body { get; set; }
        
        public string From { get; set; }

#nullable enable
        public ICollection<MessageUser>? MessageUsers { get; set; }
#nullable disable
    }
}
