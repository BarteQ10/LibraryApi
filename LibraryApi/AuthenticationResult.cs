﻿namespace LibraryApi
{
    public class AuthenticationResult
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
