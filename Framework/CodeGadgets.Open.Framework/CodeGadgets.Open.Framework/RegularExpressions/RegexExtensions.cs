using System;
using System.Text.RegularExpressions;

namespace CodeGadgets.Open.Framework.RegularExpressions
{
  public static class RegexExtensions
  {
    /// <summary>Determines if a Regex expression is valid.</summary>
		/// <param name="regexExpression">A regular expression</param>
		/// <returns>True if the expression can be parsed by the RegexParser; false otherwise.</returns>
		public static bool IsRegexSyntaxValid(string regexExpression)
    {
      return RegexExtensions.IsRegexSyntaxValid(regexExpression, RegexOptions.None);
    }
    public static bool IsRegexSyntaxValid(string regexExpression, RegexOptions options)
    {
      // Unfortunately, the RegexParser isn't an available object. It seems that the ONLY way to determine
      // if an expression is valid is to call for a match and if an argument exception is thrown, the
      // expression is not valid.  Slow. Sure, but they haven't provided any other way as I've found.
      // Ps. Google search ignores "regex" and "ArgumentException", annoying.
      try
      {
        Regex.IsMatch("", regexExpression, options);
        return true;
      }
      catch (ArgumentException)
      {
        return false;
      }
    }
    public static bool IsThisAValidRegexExpression(this string regexExpression)
    {
      return IsRegexSyntaxValid(regexExpression);
    }
    public static bool IsThisAValidRegexExpression(this string regexExpression, RegexOptions options)
    {
      return IsRegexSyntaxValid(regexExpression, options);
    }
  }
}
