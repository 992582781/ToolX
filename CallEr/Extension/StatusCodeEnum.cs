using Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallEr.Extension
{
    public enum StatusCodeEnum
    {
        [Text("请求(或处理)成功")]
        Success = 200, //请求(或处理)成功

        [Text("内部请求出错")]
        Error = 500, //内部请求出错

        [Text("未授权标识")]
        Unauthorized = 401,//未授权标识

        [Text("请求参数不完整或不正确")]
        ParameterError = 400,//请求参数不完整或不正确

        [Text("请求TOKEN失效")]
        TokenInvalid = 403,//请求TOKEN失效

        [Text("HTTP请求类型不合法")]
        HttpMehtodError = 405,//HTTP请求类型不合法

        [Text("HTTP请求不合法,请求参数可能被篡改")]
        HttpRequestError = 406,//HTTP请求不合法

        [Text("该URL已经失效")]
        URLExpireError = 407,//HTTP请求不合法
    }

    /// <summary>
    /// 显示的控件枚举
    /// </summary>
    public enum Control_Type
    {
        /// <summary>
        /// 下拉框
        /// </summary>
        [Description("下拉框")]
        SelectText = 0,
        /// <summary>
        /// 文本框
        /// </summary>
        [Description("文本框")]
        Text = 1,
        /// <summary>
        /// 时间框
        /// </summary>
        [Description("时间框")]
        TimeText = 2,
        /// <summary>
        /// 数值框
        /// </summary>
        [Description("数值框")]
        NumberText = 3,
        /// <summary>
        /// 邮件框
        /// </summary>
        [Description("邮件框")]
        EmailText = 4,
        /// <summary>
        /// 网址框
        /// </summary>
        [Description("网址框")]
        UrlText = 4
    }

}
