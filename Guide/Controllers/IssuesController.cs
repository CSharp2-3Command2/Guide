﻿using System.Collections.Generic;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Guide.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Controllers
{
    [Authorize]
    public class IssuesController : Controller
    {
        private readonly GuideContext _db;

        public IssuesController(GuideContext db)
        {
            _db = db;
        }

        // GET
        public IActionResult Details(int id, string back)
        {
            IssueStepsViewModel model = new IssueStepsViewModel
            {
                Issue = _db.Issues.FirstOrDefault(i => i.Id == id),
                DesignatedSteps = _db.IssueStep.OrderBy(i => i.Id)
                    .Where(i => i.IssueId == id).Select(s => s.Step)
                    .ToList(),
                AllSteps = new List<Step>(),
                DesiredResults = _db.DesiredResultIssue.OrderBy(d => d.Id)
                    .Where(d => d.IssueId == id)
                    .Select(s => s.DesiredResult).Where(s => s.Active).ToList()
            };
            if (back != null)
                model.Back = back;
            else
            {
                model.Back = "Личный кабинет";
            }
            return View(model);
        }
    }
}