using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace PageValidate
{
    /// <summary>
    /// 页面数据校验类
    /// Copyright (C) Maticsoft 2004-2012
    /// </summary>
    public class PageValidate
    {
        private static Regex RegPhone = new Regex("^(13[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])\\d{8}$");
        private static Regex RegNumber = new Regex("^[0-9]+$");
        private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
        private static Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
        private static Regex RegPassWord = new Regex("^[a-zA-Z]\\w{5,17}$");
        private static Regex RegLicense = new Regex("^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[A-Z0-9]{4}[A-Z0-9挂学警港澳]{1}$");


        public PageValidate()
        {
        }


        #region 数字字符串检查

        /// <summary>
        /// 电话号码验证
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsPhone(string inputData)
        {
            Match m = RegPhone.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 车牌验证
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsLicense(string inputData)
        {
            Match m = RegLicense.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否数字字符串
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string inputData)
        {
            Match m = RegNumber.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumberSign(string inputData)
        {
            Match m = RegNumberSign.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(string inputData)
        {
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimalSign(string inputData)
        {
            Match m = RegDecimalSign.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 验证密码格式
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsPassWord(string inputData)
        {
            Match m = RegPassWord.Match(inputData);
            return m.Success;
        }


        #endregion

        #region 中文检测

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsHasCHZN(string inputData)
        {
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 邮件地址
        /// <summary>
        /// 邮件地址格式判断
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsEmail(string inputData)
        {
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 日期格式判断
        /// <summary>
        /// 日期格式字符串判断
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(string str)
        {
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    DateTime.Parse(str);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 其他

        /// <summary>
        /// 检查字符串最大长度，返回指定长度的串
        /// </summary>
        /// <param name="sqlInput">输入字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>			
        public static string SqlText(string sqlInput, int maxLength)
        {
            if (sqlInput != null && sqlInput != string.Empty)
            {
                sqlInput = sqlInput.Trim();
                if (sqlInput.Length > maxLength)//按最大长度截取字符串
                    sqlInput = sqlInput.Substring(0, maxLength);
            }
            return sqlInput;
        }

        //字符串清理
        public static string InputText(string inputString, int maxLength)
        {
            StringBuilder retVal = new StringBuilder();

            // 检查是否为空
            if ((inputString != null) && (inputString != String.Empty))
            {
                inputString = inputString.Trim();

                //检查长度
                if (inputString.Length > maxLength)
                    inputString = inputString.Substring(0, maxLength);

                //替换危险字符
                for (int i = 0; i < inputString.Length; i++)
                {
                    switch (inputString[i])
                    {
                        case '"':
                            retVal.Append("&quot;");
                            break;
                        case '<':
                            retVal.Append("&lt;");
                            break;
                        case '>':
                            retVal.Append("&gt;");
                            break;
                        default:
                            retVal.Append(inputString[i]);
                            break;
                    }
                }
                retVal.Replace("'", " ");// 替换单引号
            }
            return retVal.ToString();

        }
        /// <summary>
        /// 转换成 HTML code
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string Encode(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }
        /// <summary>
        ///解析html成 普通文本
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string Decode(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }

        public static string SqlTextClear(string sqlText)
        {
            if (sqlText == null)
            {
                return null;
            }
            if (sqlText == "")
            {
                return "";
            }
            sqlText = sqlText.Replace(",", "");//去除,
            sqlText = sqlText.Replace("<", "");//去除<
            sqlText = sqlText.Replace(">", "");//去除>
            sqlText = sqlText.Replace("--", "");//去除--
            sqlText = sqlText.Replace("'", "");//去除'
            sqlText = sqlText.Replace("\"", "");//去除"
            sqlText = sqlText.Replace("=", "");//去除=
            sqlText = sqlText.Replace("%", "");//去除%
            sqlText = sqlText.Replace(" ", "");//去除空格
            return sqlText;
        }
        #endregion

        #region 是否由特定字符组成
        public static bool isContainSameChar(string strInput)
        {
            string charInput = string.Empty;
            if (!string.IsNullOrEmpty(strInput))
            {
                charInput = strInput.Substring(0, 1);
            }
            return isContainSameChar(strInput, charInput, strInput.Length);
        }

        public static bool isContainSameChar(string strInput, string charInput, int lenInput)
        {
            if (string.IsNullOrEmpty(charInput))
            {
                return false;
            }
            else
            {
                Regex RegNumber = new Regex(string.Format("^([{0}])+$", charInput));
                //Regex RegNumber = new Regex(string.Format("^([{0}]{{1}})+$", charInput,lenInput));
                Match m = RegNumber.Match(strInput);
                return m.Success;
            }
        }
        #endregion

        #region 检查输入的参数是不是某些定义好的特殊字符：这个方法目前用于密码输入的安全检查
        /// <summary>
        /// 检查输入的参数是不是某些定义好的特殊字符：这个方法目前用于密码输入的安全检查
        /// </summary>
        public static bool isContainSpecChar(string strInput)
        {
            string[] list = new string[] { "123456", "654321" };
            bool result = new bool();
            for (int i = 0; i < list.Length; i++)
            {
                if (strInput == list[i])
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        #endregion


        /// <summary>
        /// 获取客户端IP地址（无视代理）
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetHostAddress()
        {
            string userHostAddress = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 防sql注入
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string validate_sql(string Htmlstring)
        {
            if (Htmlstring == null)
            {
                return "";
            }
            else
            {
                //删除脚本
                Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                //删除HTML
                Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                //删除与数据库相关的词
                Htmlstring = Regex.Replace(Htmlstring, "select", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "insert", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "delete", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "count", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "drop table", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "truncate", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, " asc", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "mid", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "char", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "exec master", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "net localgroup administrators", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, " and", "", RegexOptions.IgnoreCase);
                Htmlstring = Htmlstring.Replace("select ", "");
                Htmlstring = Htmlstring.Replace("insert ", "");
                Htmlstring = Htmlstring.Replace("delete ", "");
                Htmlstring = Regex.Replace(Htmlstring, "where ", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "<script>", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "</script>", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "exec ", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "update", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "updateupdate", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "updateupdateupdate", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "updateupdateupdateupdate", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "chr ", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "mid ", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "master", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "truncate", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "char ", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "declare", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "from ", "", RegexOptions.IgnoreCase);
                Htmlstring = Htmlstring.Replace(";", "；");
                Htmlstring = Htmlstring.Replace("*", "x");
                //Htmlstring = Htmlstring.Replace("-", "——");
                Htmlstring = Htmlstring.Replace("'", "‘");

                return Htmlstring.Trim();
            }
        }




        #region  身份证验证

        int[] weight = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
        char[] vCode = new char[] { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };
        string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
        int age = 0;
        bool valid;

        /// <summary>
        /// 通过身份证得到性别和年龄
        /// </summary>
        /// <param name="num"></param>
        /// <param name="sex"></param>
        /// <param name="age"></param>
        /// <returns></returns>
        public bool Reg_IdentityCard(string num, out string sex_out, out int age_out)
        {
            if (num.Length == 18)
            {
                if (!CheckValidCode(num.Substring(0, 17), num[17]))
                    valid = false;
                else if (!CheckACode(num.Substring(0, 2)))
                    valid = false;
                else if (!GetAge(num.Substring(6, 4)))
                    valid = false;
                else
                    valid = true;
                if (valid)
                {
                    //生日
                    string birthday = num.Substring(6, 4) + "-" + num.Substring(10, 2) + "-" + num.Substring(12, 2);
                    int g = Convert.ToInt32(num.Substring(14, 3));
                    if (g % 2 == 0)
                        sex_out = "女";
                    else
                        sex_out = "男";
                    age_out = age;
                    return true;
                }
                else
                {
                    sex_out = "男";
                    age_out = 0;
                    return false;
                }
            }
            else if (num.Length == 15)
            {
                //生日
                string birthday = "19" + num.Substring(6, 2) + "-" + num.Substring(8, 2) + "-" + num.Substring(10, 2);
                if (!CheckACode(num.Substring(0, 2)))
                    valid = false;
                else if (!GetAge("19" + num.Substring(6, 2)))
                    valid = false;
                else
                    valid = true;
                if (valid)
                {
                    int g = Convert.ToInt32(num.Substring(12, 3));
                    if (g % 2 == 0)
                        sex_out = "女";
                    else
                        sex_out = "男";
                    age_out = age;
                    return true;
                }
                else
                {
                    sex_out = "男";
                    age_out = 0;
                    return false;
                }
            }
            else
            {
                sex_out = "男";
                age_out = 0;
                return false;
            }
        }


        bool CheckValidCode(string input17, char last)//检查最后一位校验码  
        {
            if (last == CheckValidCode(input17))
                return true;
            return false;
        }
        char CheckValidCode(string input17)//检查最后一位校验码  
        {
            int sum = 0, cur;
            for (int i = 0; i < 17; i++)
            {
                cur = Convert.ToInt32(input17[i]) - 48;
                sum += cur * weight[i];
            }
            return vCode[(sum % 11)];
        }
        bool GetAge(string input4)//算出年龄  
        {
            try
            {
                age = Convert.ToInt32(DateTime.Now.Year) - Convert.ToInt32(input4);
                return true;
            }
            catch { return false; }
        }
        bool CheckACode(string input2)//检查地区码  
        {
            if (address.IndexOf(input2) != -1)
                return true;
            return false;
        }
        #endregion

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">保存多少位字符</param>
        /// <returns></returns>
        public static string cutword(string str, int length)
        {
            int i = 0, j = 0;
            foreach (char chr in str)
            {
                if ((int)chr > 127)
                {
                    i += 2;
                }
                else
                {
                    i++;
                }
                if (i > length)
                {
                    str = str.Substring(0, j) + "...";
                    break;
                }
                j++;
            }
            if (str == "")
                str = "&nbsp;";
            return str;
        }

        /// <summary>
        /// 截取字符串(长度:全角2位,半角1位)
        /// </summary>
        /// <param name="inputString">字符串</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string cutString(string originalNews, int len)
        {
            originalNews = originalNews.Trim();
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] newsBytes = encoding.GetBytes(originalNews);

            StringBuilder sb = new StringBuilder();//最终要截出来的字符

            int narrowChars = 0;//英文和半角符号相当于一个半角字符
            int widthChars = 0;//两个半角相当于一个全角
            for (int i = 1; i < newsBytes.Length; i += 2)
            {
                if (widthChars == len)
                    break;
                byte[] temp = new byte[] { newsBytes[i - 1], newsBytes[i] };
                sb.Append(Encoding.Unicode.GetString(temp));
                //检查位置为偶数的字符是否为0，若为0，则为窄字符
                if ((int)newsBytes[i] == 0)
                {
                    narrowChars++;
                    if (narrowChars == 2)
                    {
                        narrowChars = 0;
                        widthChars++;
                    }
                }
                else
                {
                    widthChars++;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将字符串转换为日期格式，返回日期type=1时间为：00:00:00 ；type=2时间为：23:59:59 type=3 yyyy-MM-dd
        /// </summary>
        /// <param name="date">字符串</param>
        /// <param name="type">type=1时间为：00:00:00 ；type=2时间为：23:59:59type=3 yyyy-MM-dd</param></param>
        /// <returns>日期</returns>
        public static DateTime DateFormatReturnDate(string date, int type)
        {
            if (type == 1)
            {
                DateTime _newDate = DateTime.Parse(DateTime.Parse(date).ToString("yyyy-MM-dd 00:00:00"));
                return _newDate;
            }
            else if (type == 2)
            {
                DateTime _newDate = DateTime.Parse(DateTime.Parse(date).ToString("yyyy-MM-dd 23:59:59"));
                return _newDate;
            }
            else if (type == 3)
            {
                DateTime _newDate = DateTime.Parse(DateTime.Parse(date).ToString("yyyy-MM-dd"));
                return _newDate;
            }
            else
            {
                DateTime _newDate = DateTime.Parse(DateTime.Parse(date).ToString("yyyy-MM-dd HH:mm:ss"));
                return _newDate;
            }
        }

        /// <summary>
        /// 将字符串转换为日期格式，返回字符串type=1时间为：00:00:00 ；type=2时间为：23:59:59 type=3 yyyy-MM-dd
        /// </summary>
        /// <param name="date">字符串</param>
        /// <param name="type">type=1时间为：00:00:00 ；type=2时间为：23:59:59 type=3 yyyy-MM-dd</param>
        /// <returns>日期</returns>
        public static string DateFormatReturnString(string date, int type)
        {
            if (type == 1)
            {
                string _newDate = DateTime.Parse(date).ToString("yyyy-MM-dd 00:00:00");
                return _newDate;
            }
            else if (type == 2)
            {
                string _newDate = DateTime.Parse(date).ToString("yyyy-MM-dd 23:59:59");
                return _newDate;
            }
            else if (type == 3)
            {
                string _newDate = DateTime.Parse(date).ToString("yyyy-MM-dd");
                return _newDate;
            }
            else
            {
                string _newDate = DateTime.Parse(date).ToString("yyyy-MM-dd HH:mm");
                return _newDate;
            }
        }


        #region  身份证判断
        /// <summary>
        /// 是否是身份证号 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public static bool IsIdCard(string id)
        {

            if (string.IsNullOrEmpty(id))

                return true;

            if (id.Length == 18)

                return CheckIDCard18(id);

            else if (id.Length == 15)

                return CheckIDCard15(id);

            else

                return false;

        }

        /// <summary>
        /// 是否为18位身份证号 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private static bool CheckIDCard18(string Id)
        {

            long n = 0;

            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)

                return false;//数字验证 

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(Id.Remove(2)) == -1)

                return false;//省份验证 

            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");

            DateTime time = new DateTime();

            if (DateTime.TryParse(birth, out time) == false)

                return false;//生日验证 

            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');

            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');

            char[] Ai = Id.Remove(17).ToCharArray();

            int sum = 0;

            for (int i = 0; i < 17; i++)

                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());

            int y = -1;

            Math.DivRem(sum, 11, out y);

            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())

                return false;//校验码验证 

            return true;//符合GB11643-1999标准 

        }

        /// <summary>
        /// 是否为15位身份证号 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private static bool CheckIDCard15(string Id)
        {

            long n = 0;

            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))

                return false;//数字验证 

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(Id.Remove(2)) == -1)

                return false;//省份验证 

            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");

            DateTime time = new DateTime();

            if (DateTime.TryParse(birth, out time) == false)

                return false;//生日验证 

            return true;//符合15位身份证标准 

        }

        #endregion


       
    }
}
