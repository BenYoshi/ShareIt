using System.ComponentModel.DataAnnotations;

namespace ShareIt.Entities
{
    public enum PostType
    {
        None,
        Funny,
        Serious,
        Happy,
        Sad
    }

    public class Post
    {
        public int Id { get; set; }

        [Required, MaxLength(80)]
        [Display(Name="Post Title")]
        public string Title { get; set; }

        public string Content { get; set; }

        public PostType Type { get; set; }

        public User Poster { get; set; }

        public int Upvotes { get; set; }

        public int Downvotes { get; set; }
    }
}
