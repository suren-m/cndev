using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace strings_frontend.Pages
{
    public class ErrorTriggerModel : PageModel
    {
        public void OnGet()
        {
            throw new Exception();
        }
    }
}
