using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqX
{
    /// <summary>
    /// 家庭费用情况
    /// </summary>
    public class House
    {
        /// <summary>
        /// 户主姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属行政区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 电费金额
        /// </summary>
        public double DfMoney { get; set; }

        /// <summary>
        /// 水费金额
        /// </summary>
        public double SfMoney { get; set; }

        /// <summary>
        /// 燃气金额
        /// </summary>
        public double RqfMoney { get; set; }
    }
}
