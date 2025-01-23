using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Departments.Dto
{
    [AutoMapFrom(typeof(Department))]
    public class DepartmentOutput
    {
        public int Id { get; set; }
        public string DeptName { get; set; }
        public string Description { get; set; }
    }
}
