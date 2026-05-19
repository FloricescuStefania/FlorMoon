using FlorMoon.Models;
using FlorMoon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FlorMoon.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ProfileController(IUserService userService, SignInManager<IdentityUser> signInManager)
        {
            _userService = userService;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var user = _userService.GetCurrentUser();

            if (user == null)
            {
                var emptyModel = new ProfileViewModel
                {
                    UserId = 0,
                    Email = User.Identity?.Name,
                    FirstName = "",
                    LastName = "",
                    PhoneNumber = "",
                    Address = ""
                };

                return View("Profile", emptyModel);
            }

            var model = new ProfileViewModel
            {
                UserId = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                ProfilePicturePath = user.ProfilePicturePath 
            };

            return View("Profile", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProfile(ProfileViewModel model, IFormFile? profilePicture)
        {
            var loggedInEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(loggedInEmail)) return Challenge();

            var user = _userService.GetUserByEmail(loggedInEmail);

            ModelState.Remove("CurrentPassword");
            ModelState.Remove("NewPassword");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("profilePicture");

            if (ModelState.IsValid)
            {
                string? currentPicturePath = user?.ProfilePicturePath;

                if (profilePicture != null && profilePicture.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(profilePicture.FileName);

                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    if (!string.IsNullOrEmpty(user?.ProfilePicturePath) && !user.ProfilePicturePath.Contains("default-avatar.png", StringComparison.OrdinalIgnoreCase))
                    {
                        string oldCleanPath = user.ProfilePicturePath.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar);
                        string oldAbsoluteFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldCleanPath);

                        if (System.IO.File.Exists(oldAbsoluteFilePath))
                        {
                            try
                            {
                                System.IO.File.Delete(oldAbsoluteFilePath);
                            }
                            catch (Exception) {  }
                        }
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        profilePicture.CopyTo(fileStream);
                    }

                    currentPicturePath = "/Images/" + uniqueFileName;
                }

                if (user == null)
                {
                    var newUser = new User
                    {
                        Email = loggedInEmail,
                        FirstName = model.FirstName ?? "",
                        LastName = model.LastName ?? "",
                        PhoneNumber = model.PhoneNumber ?? "",
                        Address = model.Address ?? "",
                        Password = "IdentityManaged",
                        ProfilePicturePath = currentPicturePath
                    };

                    _userService.CreateUser(newUser);
                    TempData["ProfileSuccess"] = "Your profile has been created and saved successfully!";
                }
                else
                {
                    user.FirstName = model.FirstName ?? "";
                    user.LastName = model.LastName ?? "";
                    user.PhoneNumber = model.PhoneNumber ?? "";
                    user.Address = model.Address ?? "";
                    user.ProfilePicturePath = currentPicturePath;

                    _userService.UpdateUser(user);
                    TempData["ProfileSuccess"] = "Profile has been updated!";
                }

                return RedirectToAction("Profile");
            }

            return View("Profile", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ProfileViewModel model)
        {
            var currentUser = _userService.GetCurrentUser();

            if (currentUser == null)
            {
                ModelState.AddModelError(string.Empty, "Please fill in your account information details first.");
                return View("Profile", model);
            }

            if (currentUser.UserID != model.UserId) return Forbid();

            var user = _userService.GetUserById(model.UserId);
            if (user == null) return NotFound();

            if (user.Password != model.CurrentPassword)
            {
                ModelState.AddModelError("CurrentPassword", "The current password is incorrect.");
            }

            if (ModelState.IsValid)
            {
                user.Password = model.NewPassword;
                _userService.UpdateUser(user);

                TempData["PasswordSuccess"] = "Password updated successfully!";
                return RedirectToAction("Profile");
            }

            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;
            model.PhoneNumber = user.PhoneNumber;
            model.Address = user.Address;
            model.ProfilePicturePath = user.ProfilePicturePath; 

            return View("Profile", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}