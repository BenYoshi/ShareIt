using System.Collections.Generic;
using ShareIt.Entities;

namespace ShareIt.ViewModels
{
    public class HomePageViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
    }
}
