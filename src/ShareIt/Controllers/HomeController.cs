using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShareIt.Entities;
using ShareIt.Services;
using ShareIt.ViewModels;

namespace ShareIt.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IPostData _postData;
        private IGreeter _greeter;
        private UserManager<User> _userManager;
        private ICommentData _commentData;

        public HomeController(IPostData postData, IGreeter greeter, UserManager<User> userManager, ICommentData commentData)
        {
            _postData = postData;
            _greeter = greeter;
            _userManager = userManager;
            _commentData = commentData;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var post = new HomePageViewModel();
            post.Posts = _postData.GetAll();

            return View(post);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var post = _postData.Get(id);
            if (post == null)
            {
                return RedirectToAction("Index");
            }
            var dvm = new DetailsViewModel()
            {
                Post = post,
                Comments = _commentData.GetByPost(post.Id)
            };

            return View(dvm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PostEditViewModel post)
        {
            if (ModelState.IsValid)
            {
                var newPost = new Post();
                newPost.Title = post.Title;
                newPost.Type = post.Type;
                newPost.Content = post.Content;
                newPost.Upvotes = 1;
                newPost.Downvotes = 0;
                newPost.Poster = _userManager.GetUserAsync(User).Result;
                newPost = _postData.Add(newPost);
                _postData.Commit();

                return RedirectToAction("Details", new {id = newPost.Id});
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _postData.Get(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PostEditViewModel model)
        {
            var post = _postData.Get(id);
            if (ModelState.IsValid)
            {
                post.Title = model.Title;
                post.Type = model.Type;
                post.Content = model.Content;
                _postData.Commit();
                return RedirectToAction("Details", new {id = post.Id});
            }
            return View(post);
        }

        [HttpPost]
        public IActionResult Vote([FromBody] VoteViewModel vm)
        {
            var post = _postData.Get(vm.Id);
            var count = 0;
            switch (vm.Submit)
            {
                case "UP":
                    post.Upvotes = post.Upvotes + 1;
                    count = post.Upvotes;
                    break;
                case "DOWN":
                    post.Downvotes = post.Downvotes + 1;
                    count = post.Downvotes;
                    break;
            }
            _postData.Commit();

            return Json(new { count });
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var user = _userManager.GetUserAsync(User).Result;
            var vm = new ProfileViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Posts = _postData.GetByPoster(user),
                Comments = _commentData.GetByCommenter(user)
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Comment(int id)
        {
            var post = _postData.Get(id);
            
            if (post == null)
            {
                return RedirectToAction("Details", new { id = post.Id });
            }
            var cvm = new CommentEditViewModel()
            {
                Post = post,
                Commenter = _userManager.GetUserAsync(User).Result,
                Content = ""
            };

            return View(cvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Comment(int id, CommentEditViewModel comment)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var post = _postData.Get(id);

            if (ModelState.IsValid)
            {
                var newComment = new Comment()
                {
                    Post = post,
                    Content = comment.Content,
                    Commenter = user
                };
                newComment = _commentData.Add(newComment);
                _commentData.Commit();
                return RedirectToAction("Details", new { id = post.Id });
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            var user = _userManager.GetUserAsync(User).Result;
            var vm = new ProfileViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(ProfileViewModel model)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (ModelState.IsValid)
            {
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.Phone;

                var updateResult = await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}