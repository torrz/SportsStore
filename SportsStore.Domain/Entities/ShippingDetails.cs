using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage ="请输入名字")]
        public string Name { get; set; }

        [Required(ErrorMessage ="请输入一个地址")]
        [Display(Name="地址一")]
        public string Line1 { get; set; }
        [Display(Name = "地址二")]
        public string Line2 { get; set; }
        [Display(Name = "地址三")]
        public string Line3 { get; set; }

        [Required(ErrorMessage ="请输入省")]
        [Display(Name = "省")]
        public string City { get; set; }

        [Required(ErrorMessage ="请输入市区")]
        [Display(Name = "市区")]
        public string State { get; set; }
        [Display(Name = "镇街")]
        public string Zip { get; set; }

        [Required(ErrorMessage ="请输入详细地址")]
        [Display(Name = "详细地址")]
        public string Country { get; set; }
        public bool GiftWarp { get; set; }

    }
}
