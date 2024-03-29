﻿using LibraryApi.Entites;

namespace LibraryApi.DTOs.User
{
    public record struct RegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; init; }
        public string ConfirmPassword { get; set; }
        public Role Role { get; set; }

    }
}
