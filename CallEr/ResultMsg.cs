using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallEr
{

    public class ResultMsg
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 操作信息
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// 数据条数
        /// </summary>
        public int total { get; set; }


        /// <summary>
        /// 返回数据
        /// </summary>
        public object rows { get; set; }

    }
}
