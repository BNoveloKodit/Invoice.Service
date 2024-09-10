using Amazon.Runtime.Internal;
using Infraction.Backend.Image.Service.Infrastructure.Utils.Enums;
using System.Text.RegularExpressions;

namespace Infraction.Backend.Image.Service.Infrastructure.Utils.Shared
{
    public static class UtilsFunctions
    {
        public static bool ValidateRegularExpresison(string expression, string value)
        {
            Regex rx = new Regex(expression, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return rx.IsMatch(value);            
        }
    }
}
