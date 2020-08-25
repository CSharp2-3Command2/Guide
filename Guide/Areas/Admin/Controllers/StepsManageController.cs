﻿using System;
using System.Collections.Generic;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Guide.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Guide.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class StepsManageController : Controller
    {
        private readonly GuideContext _db;

        public StepsManageController(GuideContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Step> steps = _db.Steps.ToList();
            return View(steps);
        }
        
        public IActionResult Create()
        {
            return View(new Step());
        }

        [HttpPost]
        public IActionResult Create(Step step)
        {
            if (ModelState.IsValid)
            {
                _db.Steps.Add(step);
                _db.SaveChanges();
                return RedirectToAction("Index", "StepsManage", new {area = "Admin"});
            }

            return NotFound();
        }


        public IActionResult Details(int id)
        {
            Step step = _db.Steps.FirstOrDefault(s => s.Id == id);
            return View(step);
        }

        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                Step step = _db.Steps.FirstOrDefault(s => s.Id == id);
                if (step != null)
                    return View(step);
                
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Step step)
        {
            if (ModelState.IsValid)
            {
                _db.Steps.Update(step);
                _db.SaveChanges();
                return RedirectToAction("Details", "StepsManage", new {area = "Admin", step.Id});
            }

            return View(step);
        }

        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                Step step = _db.Steps.FirstOrDefault(s => s.Id == id);
                if (step != null)
                {
                    return View(step);
                }
            }
            return NotFound();
        }
        
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            if (id != 0)
            {
                Step step = _db.Steps.FirstOrDefault(s => s.Id == id);
                if (step != null)
                {
                    _db.Steps.Remove(step);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "StepsManage");
        }
    }
}