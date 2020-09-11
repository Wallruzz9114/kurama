namespace Data
{
    public class SocialLink
    {
        public string SourceUserId { get; set; }
        public virtual AppUser SourceUser { get; set; }
        public string TargetUserId { get; set; }
        public virtual AppUser TargetUser { get; set; }
    }
}