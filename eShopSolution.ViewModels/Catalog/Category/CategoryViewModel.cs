﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Category
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //public string Description { get; set; }
        public int? ParentId { get; set; }
    }
}
