using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OSS.IPTV.Ministra.Admin.Pages.Account
{
    public class LoginModel : PageModel
    {
        public async Task OnGet(string returnUrl)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = returnUrl ?? "/"
            };

            await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, properties);
        }
    }
}
