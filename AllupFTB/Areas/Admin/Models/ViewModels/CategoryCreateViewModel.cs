﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace AllupFTB.Areas.Admin.Models.ViewModels
{
    public class CategoryCreateViewModel
    {
        public string Name   { get; set; }
        public bool IsMain { get; set; }
        public IFormFile? Image { get; set; }

        public int ParentId { get; set; }

        public List<SelectListItem> ParentCategories { get; set; } = new();
    }
}
