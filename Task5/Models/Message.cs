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

#nullable enable
        public ICollection<MessageUser>? MessageUser { get; set; }
#nullable disable
    }
}
