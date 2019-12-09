using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtensions
    {
        #region SafeTrim
        public static string SafeTrim(this string obj, params char[] trimChars)
        {
            return obj == null ? string.Empty : obj.Trim(trimChars);
        }
        #endregion

        #region [Left]
        /// <summary>
        /// Returns a string containing a specified number of characters from the left side of a string.
        /// </summary>
        /// <param name="str">Required. String expression from which the leftmost characters are returned.</param>
        /// <param name="length">Required. Integer greater than 0. Numeric expression 
        /// indicating how many characters to return. If 0, a zero-length string ("") 
        /// is returned. If greater than or equal to the number of characters in Str, 
        /// the entire string is returned. If str is null, this returns null.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if length is less than 0</exception>
        /// <exception cref="ArgumentNullException">Thrown if str is null.</exception>
        public static string Left(this string str, int length)
        {
            if (String.IsNullOrEmpty(str))
                return string.Empty;

            if (length >= str.Length)
                return str;

            return str.Substring(0, length);
        }
        #endregion

        #region [LeftBefore]

        /// <summary>
        /// Returns a string containing every character within a string before the 
        /// first occurrence of another string.
        /// </summary>
        /// <param name="str">Required. String expression from which the leftmost characters are returned.</param>
        /// <param name="search">The string where the beginning of it marks the 
        /// characters to return.  If the string is not found, the whole string is 
        /// returned.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if str or searchstring is null.</exception>
        public static string LeftBefore(this string str, string search)
        {
            return LeftBefore(str, search, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Returns a string containing every character within a string before the 
        /// first occurrence of another string.
        /// </summary>
        /// <param name="original">Required. String expression from which the leftmost characters are returned.</param>
        /// <param name="search">The string where the beginning of it marks the 
        /// characters to return.  If the string is not found, the whole string is 
        /// returned.</param>
        /// <param name="comparisonType">Determines whether or not to use case sensitive search.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if str or searchstring is null.</exception>
        public static string LeftBefore(this string original, string search, StringComparison comparisonType)
        {
            Check.Require(original, "original", Check.NotNull);
            Check.Require(search, "search", Check.NotNull);

            //Shortcut.
            if (search.Length > original.Length || search.Length == 0)
                return original;

            int searchIndex = original.IndexOf(search, 0, comparisonType);

            if (searchIndex < 0)
                return original;

            return Left(original, searchIndex);
        }

        #endregion

        #region [ReplaceFirst]
        /// <summary>
        /// Replace the first occurrence of <paramref name="find"/> (case sensitive) with
        /// <paramref name="replace"/>.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="find">The find.</param>
        /// <param name="replace">The replace.</param>
        /// <returns></returns>
        public static string ReplaceFirst(this string str, string find, string replace)
        {
            return str.ReplaceFirst(find, replace, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Replace the first occurrence of <paramref name="find"/> with
        /// <paramref name="replace"/>.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="find">The find.</param>
        /// <param name="replace">The replace.</param>
        /// <param name="findComparison">The find comparison.</param>
        /// <returns></returns>
        public static string ReplaceFirst(this string str, string find, string replace, StringComparison findComparison)
        {
            Check.Require(str, "str", Check.NotNullOrEmpty);
            Check.Require(find, "find", Check.NotNullOrEmpty);
            Check.Require(replace, "replace", Check.NotNullOrEmpty);

            int firstIndex = str.IndexOf(find, findComparison);

            if (firstIndex != -1)
            {
                if (firstIndex == 0)
                {
                    str = replace + str.Substring(find.Length);
                }
                else if (firstIndex == (str.Length - find.Length))
                {
                    str = str.Substring(0, firstIndex) + replace;
                }
                else
                {
                    str = str.Substring(0, firstIndex) + replace + str.Substring(firstIndex + find.Length);
                }
            }
            return str;
        }

        #endregion

        #region [With]
        /// <summary>
        /// replacement for String.Format
        /// </summary>
        public static string With(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
        #endregion

        #region ToCharacterSeparatedFileName
        /// <summary>
        /// 将name转换成带'_'分隔符的字符串并加上指定的后缀(extension)
        /// </summary>
        public static string ToCharacterSeparatedFileName(this string name, char separator, string extension)
        {
            /*
            MatchCollection matchs = Regex.Matches(name, @"(\P{Lu}+)|(\p{Lu}+\P{Lu}*)");
            string str = "";
            for (int i = 0; i < matchs.Count; i++)
            {
                if (i != 0)
                {
                    str = str + separator;
                }
                str = str + matchs[i].ToString().ToLower();
            }
            string format = string.IsNullOrEmpty(extension) ? "{0}{1}" : "{0}.{1}";
            return string.Format(format, str, extension);
            */
            MatchCollection matchs = Regex.Matches(name, @"([A-Z]+)[a-z]*|\d{1,}[a-z]{0,}");
            string str = "";
            for (int i = 0; i < matchs.Count; i++)
            {
                if (i != 0)
                {
                    str = str + separator;
                }
                str = str + matchs[i].ToString().ToLower();
            }
            string format = string.IsNullOrEmpty(extension) ? "{0}{1}" : "{0}.{1}";
            return string.Format(format, str, extension);

        }
        #endregion

        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="Htmlstring">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string NoHTML(this string Htmlstring)
        {
            //删除脚本
            Htmlstring = Htmlstring.Replace("\r\n", "");
            Htmlstring = Regex.Replace(Htmlstring, @"<script.*?</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<style.*?</style>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<.*?>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Htmlstring.Replace("<", "");
            Htmlstring = Htmlstring.Replace(">", "");
            Htmlstring = Htmlstring.Replace("\r\n", "");
            Htmlstring = System.Web.HttpUtility.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }
    }
}