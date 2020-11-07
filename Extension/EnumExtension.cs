using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension
{
    /// <summary>
    /// 枚举扩展方法
    /// </summary>
    public static class EnumExtension
    {
        private static Dictionary<string, Dictionary<string, string>> _enumCache;

        /// <summary>
        /// 缓存
        /// </summary>
        private static Dictionary<string, Dictionary<string, string>> EnumCache
        {
            get { return _enumCache ?? (_enumCache = new Dictionary<string, Dictionary<string, string>>()); }
            set { _enumCache = value; }
        }

        /// <summary>
        /// 获取枚举描述信息
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetEnumText(this Enum en)
        {
            string enString = string.Empty;
            if (null == en) return enString;

            Type type = en.GetType();
            enString = en.ToString();
            if (!EnumCache.ContainsKey(type.FullName))
            {
                var fields = type.GetFields();
                Dictionary<string, string> temp = new Dictionary<string, string>();
                foreach (var item in fields)
                {
                    object[] attrs = item.GetCustomAttributes(typeof(TextAttribute), false);
                    if (attrs.Length == 1)
                    {
                        string v = ((TextAttribute)attrs[0]).Value;
                        temp.Add(item.Name, v);
                    }
                }
                EnumCache.Add(type.FullName, temp);
            }
            if (EnumCache[type.FullName].ContainsKey(enString))
            {
                return EnumCache[type.FullName][enString];
            }
            return enString;
        }

        /// <summary>
        /// 遍历枚举对象的所有元素
        /// </summary>
        /// <typeparam name="T">枚举对象</typeparam>
        /// <returns>Dictionary：枚举值-描述</returns>
        public static Dictionary<int, string> GetEnumValues<T>()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            foreach (var code in System.Enum.GetValues(typeof(T)))
            {
                ////获取名称
                //string strName = System.Enum.GetName(typeof(T), code);

                object[] objAttrs = code.GetType().GetField(code.ToString()).GetCustomAttributes(typeof(TextAttribute), true);
                if (objAttrs.Length > 0)
                {
                    TextAttribute descAttr = objAttrs[0] as TextAttribute;
                    if (!dictionary.ContainsKey((int)code))
                    {
                        if (descAttr != null) dictionary.Add((int)code, descAttr.Value);
                    }
                    //Console.WriteLine(string.Format("[{0}]", descAttr.Value));
                }
                //Console.WriteLine(string.Format("{0}={1}", code.ToString(), Convert.ToInt32(code)));
            }
            return dictionary;
        }


        /// <summary>
        /// 获取枚举描述信息
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetEnumDescriptio(this Enum en)
        {
            string enString = string.Empty;
            if (null == en) return enString;

            Type type = en.GetType();
            enString = en.ToString();
            if (!EnumCache.ContainsKey(type.FullName))
            {
                var fields = type.GetFields();
                Dictionary<string, string> temp = new Dictionary<string, string>();
                foreach (var item in fields)
                {
                    object[] attrs = item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attrs.Length == 1)
                    {
                        string v = ((DescriptionAttribute)attrs[0]).Description;
                        temp.Add(item.Name, v);
                    }
                }
                EnumCache.Add(type.FullName, temp);
            }
            if (EnumCache[type.FullName].ContainsKey(enString))
            {
                return EnumCache[type.FullName][enString];
            }
            return enString;
        }

    }

    /// <summary>
    /// 自定义描述
    /// </summary>
    public class TextAttribute : Attribute
    {
        public TextAttribute(string value)
        {
            Value = value;
        }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Value { get; set; }
    }
}
