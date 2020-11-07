using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqX
{
    public class DatatableEX
    {
        #region datatable去重


        /// <summary>
        /// datatable去重
        /// </summary>
        /// <param name="dtSource">需要去重的datatable</param>
        /// <param name="columnNames">依据哪些列去重</param>
        /// <returns></returns>
        public static DataTable GetDistinctTable(DataTable dtSource, params string[] columnNames)
        {
            DataTable distinctTable = dtSource.Clone();
            try
            {
                if (dtSource != null && dtSource.Rows.Count > 0)
                {
                    DataView dv = new DataView(dtSource);
                    distinctTable = dv.ToTable(true, columnNames);
                }
            }
            catch (Exception ex)
            {
                
            }
            return distinctTable;
        }



        /// <summary>
        /// datatable去重
        /// </summary>
        /// <param name="dtSource">需要去重的datatable</param>
        /// <returns></returns>
        public static DataTable GetDistinctTable(DataTable dtSource)
        {
            DataTable distinctTable = null;
            try
            {
                if (dtSource != null && dtSource.Rows.Count > 0)
                {
                    string[] columnNames = GetTableColumnName(dtSource);
                    DataView dv = new DataView(dtSource);
                    distinctTable = dv.ToTable(true, columnNames);
                }
            }
            catch (Exception ex)
            {
                 
            }
            return distinctTable;
        }

        #endregion

        #region 获取表中所有列名
        public static string[] GetTableColumnName(DataTable dt)
        {
            string cols = string.Empty;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                cols += (dt.Columns[i].ColumnName + ",");
            }
            cols = cols.TrimEnd(',');
            return cols.Split(',');
        }
        #endregion
    }
}
