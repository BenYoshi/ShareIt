using System.ComponentModel.DataAnnotations;
using ShareIt.Entities;

namespace ShareIt.ViewModels
{
    public class CommentEditViewModel
    {
        [MaxLength(40000)]
        public string Content { get; set; }

        public User Commenter { get; set; }

        public Post Post { get; set; }
    }
}
