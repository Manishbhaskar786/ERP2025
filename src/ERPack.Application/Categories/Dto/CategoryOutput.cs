﻿using Abp.AutoMapper;

namespace ERPack.Categories.Dto
{
    [AutoMapFrom(typeof(Category))]
    public class CategoryOutput
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }
}
