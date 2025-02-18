
using System;

namespace Lab4.Models.DTOs
{
    public class AuthorDTO
    {
        public int AuthorId { get; set; }
        public string Name { get; set; } = null!;
        public string? Country { get; set; }
        public int? BirthYear { get; set; }  
    }
}
