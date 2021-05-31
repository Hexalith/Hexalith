namespace Hexalith.Infrastructure.Helpers
{
    using System.Linq;
    using System.Text.Json;
    using System.Web;

    public static class HtmlHelper
    {
        public static string ToHttpQueryString(this object value, string url = "")
        {
            var values = value.GetPropertyNotNullValues();
            return url + "?" + string.Join(
                "&",
                values
                    .Select(p => p.Key + "=" + JsonSerializer.Serialize(p.Value).UrlEncode()));
        }

        public static string UrlDecode(this string text)
            => HttpUtility.UrlDecode(text);

        public static string UrlEncode(this string text)
            => HttpUtility.UrlEncode(text);
    }
}