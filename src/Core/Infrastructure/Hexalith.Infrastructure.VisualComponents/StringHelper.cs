using System.Text;

namespace Hexalith.Infrastructure.VisualComponents
{
    public static class StringHelper
    {
        public static string DashCase(this string value)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < value.Length; i++)
            {
                if (i != 0 && char.IsUpper(value[i]))
                    sb.Append('-');

                sb.Append(value[i]);
            }

            return sb.ToString().ToLowerInvariant();
        }
    }
}