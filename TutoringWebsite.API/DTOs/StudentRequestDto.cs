﻿using System.ComponentModel.DataAnnotations;

namespace TutoringWebsite.API.DTOs
{
    public class StudentRequestDto
    {
        public string? Name { get; set; }
        
        public string? Email { get; set; }
        
        public string? UserId { get; set; }
    }
}
