using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MySeasonice.Pages.Business
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
