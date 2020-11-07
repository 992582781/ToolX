using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension
{
    public static class StringExtensions
    {

        /// <summary>
        /// 移除空白符回车符制表符等，如有遗漏，请大家
        /// 自己修改添加
        /// </summary>
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public static string ToRemoveSpecialChar(this string Source)
        {
            if (string.IsNullOrEmpty(Source))
            {
                return "";
            }
            else
            {
                return Source.Trim()
                .Replace("\r", "")  //换行（\r）
                .Replace("\n", "")  //回车（\n）
                .Replace("\t", ""); //制表符（\t）
            }
        }


        /// <summary>
        /// 替换特殊字符
        /// /→／
        ///|→丨
        ///\→＼
        ///:→﹕
        ///*→＊
        ///?→？
        ///<→〈
        ///>→〉
        ///"→＂
        /// 如有遗漏，请大家自己修改添加
        /// </summary>
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public static string ToReplaceSpecialChar(this string Source)
        {
            if (string.IsNullOrEmpty(Source))
            {
                return "";
            }
            else
            {
                return Source.Replace("/", "／")
                        .Replace("|", "丨")
                        .Replace("\\", "＼")
                        .Replace(":", "﹕")
                        .Replace("*", "＊")
                        .Replace("?", "？")
                        .Replace("<", "〈")
                        .Replace(">", "〉")
                        .Replace("\"", "＂");
            }
        }
    }
}
