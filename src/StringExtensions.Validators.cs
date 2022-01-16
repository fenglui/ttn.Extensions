using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.Linq;

namespace System
{
    public static class StringExtensionsValidators
    {
        #region [IsAlpha]
        private static Regex IsAlphaRegex = new Regex(RegexPattern.ALPHA, RegexOptions.Compiled);

        /// <summary>
        /// Determines whether the specified eval string contains only alpha characters.
        /// </summary>
        /// <param name="evalString">The eval string.</param>
        /// <returns>
        /// 	<c>true</c> if the specified eval string is alpha; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlpha(this string evalString)
        {
            return IsAlphaRegex.IsMatch(evalString);
        }
        #endregion

        #region [IsAlphaNumeric]
        private static Regex IsAlphaNumericRegex = new Regex(RegexPattern.ALPHA_NUMERIC, RegexOptions.Compiled);

        /// <summary>
        /// Determines whether the specified eval string contains only alphanumeric characters
        /// </summary>
        /// <param name="evalString">The eval string.</param>
        /// <returns>
        /// 	<c>true</c> if the string is alphanumeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlphaNumeric(this string evalString)
        {
            return IsAlphaNumericRegex.IsMatch(evalString);
        }
        #endregion

        #region [IsNumeric]
        private static Regex isNumericRegex = new Regex(RegexPattern.NUMERIC, RegexOptions.Compiled);

        public static bool IsNumeric(this string inputString)
        {
            Match m = isNumericRegex.Match(inputString);
            return m.Success;
        }
        #endregion

        #region IsAbsolutePhysicalPath
        private static bool IsDirectorySeparatorChar(char ch)
        {
            if (ch != '\\')
            {
                return (ch == '/');
            }
            return true;
        }

        internal static bool IsUncSharePath(string path)
        {
            return (((path.Length > 2) && IsDirectorySeparatorChar(path[0])) && IsDirectorySeparatorChar(path[1]));
        }

        public static bool IsAbsolutePhysicalPath(this string path)
        {
            if ((path == null) || (path.Length < 3))
            {
                return false;
            }
            return (((path[1] == ':') && IsDirectorySeparatorChar(path[2])) || IsUncSharePath(path));
        }
        #endregion

        #region IsAppRelativePath
        public static bool IsAppRelativePath(this string path)
        {
            if (path == null)
            {
                return false;
            }
            int length = path.Length;
            if (length == 0)
            {
                return false;
            }
            if (path[0] != '~')
            {
                return false;
            }
            if ((length != 1) && (path[1] != '\\'))
            {
                return (path[1] == '/');
            }
            return true;
        }
        #endregion

        #region [IsEmailAddress]

        private static Regex isValidEmailRegex = new Regex(RegexPattern.EMAIL, RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmailAddress(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            return isValidEmailRegex.IsMatch(s);
        }

        #endregion

        #region [IsGuid]
        private static Regex isGuidRegex = new Regex(RegexPattern.GUID, RegexOptions.Compiled);

        public static bool IsGuid(this string candidate)
        {
            bool isValid = false;

            if (!string.IsNullOrEmpty(candidate))
            {
                isValid = isGuidRegex.IsMatch(candidate);
            }
            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static bool IsGuid(this string candidate, out Guid output)
        {
            bool isValid = false;
            output = Guid.Empty;
            if (candidate.IsGuid())
            {
                isValid = true;
                output = new Guid(candidate);
            }
            return isValid;
        }
        #endregion

        #region [IsIPAddress]
        private static Regex isIPAddressRegex = new Regex(RegexPattern.IP_ADDRESS, RegexOptions.Compiled);

        /// <summary>
        /// Determines whether the specified string is a valid IP address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>
        /// 	<c>true</c> if valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIPAddress(this string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
                return false;
            return isIPAddressRegex.IsMatch(ipAddress);
        }
        #endregion

        #region [IsLowerCase]

        private static Regex isLowerCaseRegex = new Regex(RegexPattern.LOWER_CASE, RegexOptions.Compiled);

        /// <summary>
        /// Determines whether the specified string is lower case.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>
        /// 	<c>true</c> if the specified string is lower case; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLowerCase(this string inputString)
        {
            return isLowerCaseRegex.IsMatch(inputString);
        }
        #endregion

        #region [IsUpperCase]
        private static Regex isUpperRegex = new Regex(RegexPattern.UPPER_CASE, RegexOptions.Compiled);
        /// <summary>
        /// Determines whether the specified string is upper case.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>
        /// 	<c>true</c> if the specified string is upper case; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUpperCase(this string inputString)
        {
            return isUpperRegex.IsMatch(inputString);
        }
        #endregion

        #region [IsUrl]
        private static Regex isUrlRegex = new Regex(RegexPattern.URL, RegexOptions.Compiled);
        /// <summary>
        /// Determines whether the specified string is url.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>
        /// 	<c>true</c> if the specified string is url; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUrl(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return false;
            return isUrlRegex.IsMatch(inputString);
        }
        #endregion

        #region DoesImageExistRemotely

        public static bool DoesUrlHasResponse(this string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            request.Method = "HEAD";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (WebException) { return false; }
            catch
            {
                return false;
            }
        }

        static string[] imageContentTypes = new string[] { 
            "image/jpeg","image/pjpeg", "image/gif", "image/bmp", "image/x-png"
        };

        /// <summary>
        /// 检查http图片是否存在
        /// </summary>
        /// <param name="uriToImage"></param>
        /// <returns></returns>
        public static bool DoesImageExistRemotely(this string uriToImage)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriToImage);

            request.Method = "HEAD";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return imageContentTypes.Contains(response.ContentType);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (WebException) { return false; }
            catch
            {
                return false;
            }
        }
        #endregion

        #region [IsHasChinese]
        private static Regex isHasChineseRegex = new Regex(RegexPattern.HasCHINESE, RegexOptions.Compiled);

        public static bool IsHasChinese(this string inputString)
        {
            Match m = isHasChineseRegex.Match(inputString);
            return m.Success;
        }
        #endregion

        #region [IsChineseLetter]
        /// <summary>
        /// 在unicode 字符串中，中文的范围是在4E00..9FFF:CJK Unified Ideographs。通过对字符的unicode编码进行判断来确定字符是否为中文。 
        /// </summary>
        /// <remarks>http://blog.csdn.net/qiujiahao/archive/2007/08/09/1733169.aspx</remarks>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsChineseLetter(this string input, int index)
        {
            int code = 0;
            int chfrom = Convert.ToInt32("4e00", 16);    //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
            int chend = Convert.ToInt32("9fff", 16);
            if (input != "")
            {
                code = Char.ConvertToUtf32(input, index);    //获得字符串input中指定索引index处字符unicode编码

                if (code >= chfrom && code <= chend)
                {
                    return true;     //当code在中文范围内返回true

                }
                else
                {
                    return false;    //当code不在中文范围内返回false
                }
            }
            return false;
        } //
        #endregion

        #region [IsGBCode]
        /// <summary>
        /// 判断一个word是否为GB2312编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsGBCode(this string word)
        {
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(word);
            if (bytes.Length <= 1)  // if there is only one byte, it is ASCII code or other code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if (byte1 >= 176 && byte1 <= 247 && byte2 >= 160 && byte2 <= 254)    //判断是否是GB2312
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region [IsGBKCode]
        /// <summary>
        /// 判断一个word是否为GBK编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsGBKCode(this string word)
        {
            byte[] bytes = Encoding.GetEncoding("GBK").GetBytes(word.ToString());
            if (bytes.Length <= 1)  // if there is only one byte, it is ASCII code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if (byte1 >= 129 && byte1 <= 254 && byte2 >= 64 && byte2 <= 254)     //判断是否是GBK编码
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region [IsBig5Code]
        /// <summary>
        /// 判断一个word是否为GBK编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsBig5Code(this string word)
        {
            byte[] bytes = Encoding.GetEncoding("Big5").GetBytes(word.ToString());
            if (bytes.Length <= 1)  // if there is only one byte, it is ASCII code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if ((byte1 >= 129 && byte1 <= 254) && ((byte2 >= 64 && byte2 <= 126) || (byte2 >= 161 && byte2 <= 254)))     //判断是否是Big5编码
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region [IsOnlyContainsChinese]
        /// <summary>
        /// 给定一个字符串，判断其是否只包含有汉字
        /// </summary>
        /// <param name="testStr"></param>
        /// <returns></returns>
        public static bool IsOnlyContainsChinese(this string testStr)
        {
            char[] words = testStr.ToCharArray();
            foreach (char word in words)
            {
                if (IsGBCode(word.ToString()) || IsGBKCode(word.ToString()))  // it is a GB2312 or GBK chinese word
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        /*
         中国电信发布中国3G号码段:中国联通185,186;中国移动188,187;中国电信189,180共6个号段。
         3G业务专属的180-189号段已基本分配给各运营商使用, 其中180、189分配给中国电信,187、188归中国移动使用,185、186属于新联通。
         中国移动拥有号码段：139、138、137、136、135、134、159、158、157（3G）、152、151、150、183、188（3G）、187（3G）;14个号段

         中国联通拥有号码段：130、131、132、155、156（3G）、186（3G）、185（3G）;6个号段
         中国电信拥有号码段：133、153、189（3G）、180（3G）;4个号码段
         181 、1700、177、173
         移动:
             2G号段(GSM网络)有139,138,137,136,135,134(0-8),159,158,152,151,150
             3G号段(TD-SCDMA网络)有157,188,187
             147是移动TD上网卡专用号段.
             
         联通:
             2G号段(GSM网络)有130,131,132,155,156
             3G号段(WCDMA网络)有186,185
             3G上网卡 145
         电信:
             2G号段(CDMA网络)有133,153
             3G号段(CDMA网络)有189,180
         
         */

        /// <summary>
        /// 是否是中国电信号码
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsChinaTeleComMobile(this string val)
        {
            return Regex.IsMatch(val, @"^(\+86|86|0)?((133)\d{8}|(153|180|181|189|177|173|170)\d{8})$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 是否是中国联通号码
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsChinaUnicomMobile(this string val)
        {
            return Regex.IsMatch(val, @"^(\+86|86|0)?((130|131|132)\d{8}|(145|155|156|185|186)\d{8})$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 是否是中国移动号码
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsChinaMobile(this string val)
        {
            return Regex.IsMatch(val, @"^(\+86|86|0)?((139|138|137|136|135|134)\d{8}|(147|150|151|152|157|158|159|183|187|188)\d{8})$", RegexOptions.IgnoreCase);
        }


        /// <summary>
        /// 是否是手机号码
        /// </summary>
        /// <param name="val"></param>
        public static bool IsMobile(this string val)
        {
            return Regex.IsMatch(val, @"^(\+86|86|0)?(13\d{9}|(145|147|150|151|152|153|155|156|157|158|159|170|173|177|180|181|182|183|185|186|187|188|189)\d{8})$", RegexOptions.IgnoreCase);
        }


    }
}
