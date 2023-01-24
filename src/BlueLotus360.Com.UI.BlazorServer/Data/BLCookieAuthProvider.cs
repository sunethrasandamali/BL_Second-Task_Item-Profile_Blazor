using BL10.CleanArchitecture.Domain.ICookieAccessor;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text;
using System.Text.Unicode;

namespace BlueLotus360.Com.UI.BlazorServer.Data
{
    public class BLCookieAuthProvider : ICookieAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BLCookieAuthProvider(IHttpContextAccessor httpContext)
        {
            _httpContextAccessor = httpContext;
        }

        public async  Task<string> GetValueAsync(string key)
        {
            var value = _httpContextAccessor.HttpContext.Request.Cookies[key];
            await Task.CompletedTask;
            return value;
        }

        public async Task SetValueAsync(string key, string value)
        {
            CookieOptions option = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1)
            };
            _httpContextAccessor.HttpContext.Session.Set(key, Encoding.UTF8.GetBytes(value));
             await Task.CompletedTask;

        }
    }
}
