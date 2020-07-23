﻿using System;

namespace Guide.Models
{
    public class Post
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string VirtualPath { get; set; }
        public string PhysicalPath { get; set; }
        public DateTime DateOfCreate { get; set; } = DateTime.Now;
        public DateTime DateOfUpdate { get; set; } = DateTime.Now;
        public string CategoryId { get; set; }
        public Category Category { get; set; }
        public string TypeId { get; set; }
        public Type Type { get; set; }
    }
}