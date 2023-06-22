namespace LibraryApi
{
    public class AuthenticationSettings
    {
        public string JwtKey { get; set; }
        public int JwtExpireDays { get; set; }
        public int RefreshTokenExpires { get; set; }
        public string JwtIssuer { get; set; }
    }
}
