namespace rapid_docs_core.Authentication
{
    public class VidaDocsContext
    {
        public string SocialId { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public long UserId { get; set; }

        public VidaDocsContext(string socialId, string email, string accessToken)
        {
            this.SocialId = socialId;
            this.Email = email;
            this.AccessToken = accessToken.Replace("Bearer ", "");
            this.UserId = 0;
        }
    }
}
