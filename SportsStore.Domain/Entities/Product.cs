using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SportsStore.Domain.Entities
{
     public class Product
    {
        [HiddenInput(DisplayValue =false)]
        public int ProductID { get; set; }

        [Display(Name="产品名称")]
        [Required(ErrorMessage ="请输入产品名称吧")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "产品描述")]
        [Required(ErrorMessage ="请输入产品描述")]
        public string Description { get; set; }

        [Display(Name = "产品价格")]
        [Required(ErrorMessage = "请输入价格")]
        [Range(0.01,double.MaxValue,ErrorMessage ="请输入正确的价格")]
        public decimal Price { get; set; }

        [Display(Name = "产品类目")]
        [Required(ErrorMessage ="请选择类目")]
        public string Category { get; set; }

        
        public byte[] ImageData { get; set; }

        
        public string ImageMimeType { get; set; }
    }
}
