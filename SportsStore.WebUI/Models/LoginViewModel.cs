using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.WebUI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="请输入账号")]
        [Display(Name ="账号")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [Display(Name = "密码")]
        public string Password { get; set; }
    }
}