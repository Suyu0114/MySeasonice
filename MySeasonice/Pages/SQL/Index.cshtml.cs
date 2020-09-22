using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySeasonice.Model;

namespace MySeasonice.Pages.SQL
{
    public class IndexModel : PageModel
    {
        private readonly MySeasonice.Data.SeaSonicContext _context;

        public IndexModel(MySeasonice.Data.SeaSonicContext context)
        {
            _context = context;
        }

        public IList<EmpProfile> EmpProfile { get; set; }
        public async Task OnGetAsync()
        {
            EmpProfile = await _context.EmpProfile.ToListAsync();
            Console.WriteLine(EmpProfile);
        }
    }
}
