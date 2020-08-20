﻿namespace Guide.Models
{
    public class TaskUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Task { get; set; }

        public virtual User User { get; set; }

    }
}