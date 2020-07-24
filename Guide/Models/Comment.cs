﻿using System;

namespace Guide.Models
{
    public class Comment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AuthorId { get; set; }
        public User Author { get; set; }
        public string PostId { get; set; }
        public Post Post { get; set; }
        public string SelText { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreate { get; set; }
    }
}