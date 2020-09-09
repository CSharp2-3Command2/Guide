﻿using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Guide.ViewModels
{
    public class BookCreateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public bool IsRecipe { get; set; } = false;
        public IFormFile CoverPath { get; set; }
        public IFormFile VirtualPath { get; set; }
        public string PhysicalPath { get; set; }
        [Remote("CheckYear","Validation", "", ErrorMessage = "Некорректное значение")]
        public string YearOfWriting { get; set; }
        public int CategoryId { get; set; }
        public string Edition { get; set; }
    }
    
   
    
}