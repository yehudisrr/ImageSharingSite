using ImageSharePassword.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageSharePassword.Web.Models
{
    public class UploadViewModel
    {
        public Image Image { get; set; }
        public string Link { get; set; }
    }
}
