﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Guide.Models
{
    public class Glossary
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public string Source { get; set; }
        public bool Active { get; set; } = true;
    }
}