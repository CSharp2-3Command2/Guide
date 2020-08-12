﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Guide.Models;
using Guide.Models.Data;
using Guide.Services;
using Guide.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Guide.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class MaterialManageController : Controller
    {
        private readonly GuideContext _db;
        private readonly UserManager<User> _userManager;
        private IHostEnvironment _environment;
        private UploadService _uploadService;

        public MaterialManageController(GuideContext db, UserManager<User> userManager, IHostEnvironment environment,
            UploadService uploadService)
        {
            _db = db;
            _userManager = userManager;
            _environment = environment;
            _uploadService = uploadService;
        }

        public IActionResult Index()
        {
            List<Post> posts = _db.Posts.ToList();
            return View(posts);
        }

        public IActionResult Details(string id)
        {
            Post post = _db.Posts.FirstOrDefault(p => p.Id == id);


            return View(post);
        }

        public IActionResult ValTemplate(string templatesId)
        {
            if (templatesId != null)
            {
                Template template = _db.Templates.FirstOrDefault(t => t.Id == templatesId);
                return Json(template);
            }

            return Json(null);
        }

        public IActionResult Create()
        {
            return View(new MaterialCreateViewModel());
        }

        [HttpPost]
        public IActionResult Create(MaterialCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Post post = new Post()
                {
                    Id = model.Id,
                    Title = model.Title,
                    Author = model.Author,
                    TextContent = model.TextContent,
                    CategoryId = model.CategoryId,
                    TypeId = model.TypeId,
                    PhysicalPath = model.PhysicalPath,
                    VirtualPath = Load(model.Id, model.VirtualPath)
                };
                _db.Posts.Add(post);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult CreateTemplateAjax(Template template)
        {
            if (template.Name != null)
            {
                _db.Templates.Add(template);
                _db.SaveChanges();
            }

            MaterialTemplateViewModel model = new MaterialTemplateViewModel()
            {
                Material = new MaterialCreateViewModel(),
                Templates = _db.Templates.ToList()
            };

            return PartialView("PartialViews/TemplatesPartial", model);
        }

        public IActionResult CreateCategoryAjax(Category category)
        {
            if (category.Name != null)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
            }

            MaterialCategoryViewModel model = new MaterialCategoryViewModel()
            {
                Material = new MaterialCreateViewModel(),
                Categories = _db.Categories.ToList(),
            };

            return PartialView("PartialViews/CategoriesPartial", model);
        }

        public IActionResult CreateTypeAjax(Type type)
        {
            if (type.Name != null)
            {
                _db.Types.Add(type);
                _db.SaveChanges();
            }

            MaterialTypeViewModel model = new MaterialTypeViewModel()
            {
                Material = new MaterialCreateViewModel(),
                Types = _db.Types.ToList()
            };

            return PartialView("PartialViews/TypesPartial", model);
        }

        public IActionResult DeleteCategoryAjax(string id)
        {
            Category category = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _db.Categories.Remove(category);
                _db.SaveChanges();
            }

            MaterialCategoryViewModel model = new MaterialCategoryViewModel()
            {
                Material = new MaterialCreateViewModel(),
                Categories = _db.Categories.ToList(),
            };

            return PartialView("PartialViews/CategoriesPartial", model);
        }

        public IActionResult DeleteTemplateAjax(string id)
        {
            Template template = _db.Templates.FirstOrDefault(c => c.Id == id);
            if (template != null)
            {
                _db.Templates.Remove(template);
                _db.SaveChanges();
            }

            MaterialTemplateViewModel model = new MaterialTemplateViewModel()
            {
                Material = new MaterialCreateViewModel(),
                Templates = _db.Templates.ToList(),
            };

            return PartialView("PartialViews/TemplatesPartial", model);
        }

        public IActionResult DeleteTypeAjax(string id)
        {
            Type type = _db.Types.FirstOrDefault(t => t.Id == id);
            if (type != null)
            {
                _db.Types.Remove(type);
                _db.SaveChanges();
            }

            MaterialTypeViewModel model = new MaterialTypeViewModel()
            {
                Material = new MaterialCreateViewModel(),
                Types = _db.Types.ToList()
            };

            return PartialView("PartialViews/TypesPartial", model);
        }

        private string Load(string id, IFormFile file)
        {
            if (file != null)
            {
                string path = Path.Combine(_environment.ContentRootPath + $"\\wwwroot\\PostsFiles\\{id}");
                string filePath = $"PostsFiles/{id}/{file.FileName}";
                if (!Directory.Exists($"wwwroot/PostsFiles/{id}"))
                {
                    Directory.CreateDirectory($"wwwroot/PostsFiles/{id}");
                }

                _uploadService.Upload(path, file.FileName, file);
                return filePath;
            }

            return null;
        }

        [HttpGet]
        public async Task<IActionResult> ViewComment(string id)
        {
            List<Comment> comments = _db.Comments.Where(c => c.PostId == id)
                .OrderByDescending(g => g.DateOfCreate).ToList();

            return PartialView("PartialViews/CommentsPartial", comments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(Comment model)
        {
            if (model.PostId != null && model.Description != null)
            {
                Comment comment = new Comment()
                {
                    PostId = model.PostId,
                    AuthorId = _userManager.GetUserId(User),
                    Description = model.Description,
                };
                _db.Comments.AddAsync(comment);
                await _db.SaveChangesAsync();
            }

            List<Comment> comments = await _db.Comments.Include(c => c.Author).Where(c => c.PostId == model.PostId)
                .OrderByDescending(g => g.DateOfCreate).ToListAsync();

            return PartialView("PartialViews/CommentsPartial", comments);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteComment(string id, string postId)
        {
            Comment comment = await _db.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment != null)
            {
                _db.Comments.Remove(comment);
                await _db.SaveChangesAsync();
            }

            List<Comment> comments = await _db.Comments.Where(c => c.PostId == postId)
                .OrderByDescending(g => g.DateOfCreate).ToListAsync();

            return PartialView("PartialViews/CommentsPartial", comments);
        }

        public IActionResult Edit(string id)
        {
            if (id != null)
            {
                Post post = _db.Posts.FirstOrDefault(p => p.Id == id);
                MaterialCreateViewModel model = new MaterialCreateViewModel()
                {
                    Id = post.Id,
                    Title = post.Title,
                    Author = post.Author,
                    TextContent = post.TextContent,
                    _virtualPath = post.VirtualPath,
                    CategoryId = post.CategoryId,
                    PhysicalPath = post.PhysicalPath,
                    TypeId = post.TypeId,
                };

                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(MaterialCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Post post = _db.Posts.FirstOrDefault(p => p.Id == model.Id);
                post.Id = model.Id;
                post.Title = model.Title;
                post.Author = model.Author;
                post.TextContent = model.TextContent;
                post.CategoryId = model.CategoryId;
                post.TypeId = model.TypeId;
                post.PhysicalPath = model.PhysicalPath;
                post.VirtualPath = Load(model.Id, model.VirtualPath);
                _db.Posts.Update(post);
                _db.SaveChanges();
                return RedirectToAction("Index", "MaterialManage");
            }

            return View(model);
        }

        public IActionResult Delete(string id)
        {
            if (id != null)
            {
                Post post = _db.Posts.FirstOrDefault(v => v.Id == id);
                if (post != null)
                {
                    return View(post);
                }
            }

            return NotFound();
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDeleta(string id)
        {
            if (id != null)
            {
                Post post = _db.Posts.FirstOrDefault(v => v.Id == id);
                if (post != null)
                {
                    _db.Posts.Remove(post);
                    _db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "MaterialManage");
        }
    }
}