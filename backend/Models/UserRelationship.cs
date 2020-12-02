namespace Models
{
    public class UserRelationship
    {
        public string FollowerId { get; set; }
        public virtual AppUser Follower { get; set; }
        public string UserFollowedId { get; set; }
        public virtual AppUser UserFollowed { get; set; }
    }
}