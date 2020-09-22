using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MySeasonice.Pages.Product
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class IndexModel : PageModel
    {
        public string Message { get; set; }
        public void OnGet()
        {
            Message = "Product";
            ViewData["Hello"] = "Hello Suyu ";

        }
    }
}
