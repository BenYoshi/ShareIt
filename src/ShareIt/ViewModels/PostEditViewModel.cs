using System.ComponentModel.DataAnnotations;
using ShareIt.Entities;

namespace ShareIt.ViewModels
{
    public class PostEditViewModel
    {
        [Required, MaxLength(80)]
        public string Title { get; set; }

        public PostType Type { get; set; }

        [MaxLength(40000)]
        public string Content { get; set; }

        public User Poster { get; set; }

        public int Upvotes { get; set; }

        public int Downvotes { get; set; }
    }
}
