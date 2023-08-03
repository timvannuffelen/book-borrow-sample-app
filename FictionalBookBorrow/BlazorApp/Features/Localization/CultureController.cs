using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Features.Localization;

/// <summary>
/// Accepts all requests that that deal with localization and globalization of the web app.
/// </summary>
/// <inheritdoc cref="Controller"/>
[Route("[controller]/[action]")]
public class CultureController : Controller
{
    /// <summary>
    /// Save the specified culture in a cookie and redirect the user back to the specified redirect URI, typically this is
    /// the URI from where the request originated. 
    /// </summary>
    /// <remarks>
    /// To allow for the user to select a culture via the UI, we use a redirect-based approach with a localization cookie. The
    /// web app persists the user's selected culture via a redirect to this controller. The controller sets the user's selected
    /// culture into a cookie and redirects the user back to the original URI. The process is similar to what happens when a user
    /// attempts to access a secure resource, where the user is redirected to a sign-in page and then redirected back to the
    /// original resource.
    /// More info can be found in the
    /// <a href="https://docs.microsoft.com/en-us/aspnet/core/blazor/globalization-localization?view=aspnetcore-6.0&pivots=server#dynamically-set-the-culture-by-user-preference">official Microsoft docs</a>.
    /// </remarks>
    [HttpGet]
    public IActionResult Set(string culture, string redirectUri)
    {
        if (!string.IsNullOrWhiteSpace(culture))
        {
            // Because we configured a CookiePolicyOption in Program.cs we need to set some cookie options here to
            // be able to add the culture cookie.
            var cookieOptions = new CookieOptions
            {
                // The culture cookie is essential for the application to function correctly (without the cookie the
                // language can be set). Marking the cookie as essential will effectively bypass the consent policy
                // checks, which only apply to non-essential cookies.
                IsEssential = true,

                // Because we configured a CookiePolicyOption in Program.cs with SameSite=None, we need to set the
                // secure flag, which some browsers will require for SameSite=None. Note this wil also require to
                // be running on HTTPS.
                Secure = true
            };

            HttpContext.Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                                                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, culture)),
                                                cookieOptions);
        }

        // Use the LocalRedirect action result to prevent open redirect attacks. For more information, see 
        // https://docs.microsoft.com/en-us/aspnet/core/security/preventing-open-redirects?view=aspnetcore-6.0
        return LocalRedirect(redirectUri);
    }
}

