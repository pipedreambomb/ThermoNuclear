namespace ThermoNuclearWar.Web.Etc
{
    public static class StringExtensions
    {

        public static bool HasValue(this string @string) 
            => string.IsNullOrWhiteSpace(@string) == false;
    }
}