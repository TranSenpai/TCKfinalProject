using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCKfinalProject.Models
{
    public class SumSaleQuantity
    {
        public string foodName { get; set; } 
        // ? dùng cho thuộc tính chưa có giá trị trong database, có thể nhận null
        public int? SumQuantity { get; set; }
    }
}