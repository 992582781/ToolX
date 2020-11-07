using ConfigX;
using Dapper;
using DapperExtensions;
using DapperExtensions.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbHelperX
{
     
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class DapperHelper
    {
        static string connStrRead = ConfigUtil.GetConnectionString("CloudClearance9053");
        static string connStrWrite = ConfigUtil.GetConnectionString("CloudClearance9053");
        public static SqlConnection Connection;
        public static int commandTimeout = 3000;
        public static IDbConnection GetConnection(bool useWriteConn)
        {
            if (useWriteConn)
            {
                Connection = new SqlConnection(connStrWrite);
                return Connection;
            }
            else
            {

                Connection = new SqlConnection(connStrRead);
                return Connection;
            }
        }


        /// <summary>
        /// 事务处理
        /// </summary>
        /// <returns></returns>
        private static SqlConnection GetOpenConnection()
        {
            var conn = new SqlConnection(connStrWrite);
            conn.Open();
            return conn;
        }

        public static IDbTransaction TranStart()
        {
            //因为生命周期被注释
            if (GetOpenConnection().State == ConnectionState.Closed)
                GetOpenConnection().Open();
            return GetOpenConnection().BeginTransaction();
        }


        public static void TranRollBack(IDbTransaction tran)
        {
            if (tran.Connection != null)
            {
                tran.Rollback();
                //因为生命周期被注释
                //if (tran.Connection.State == ConnectionState.Open)
                //    tran.Connection.Close();
            }
        }


        public static void TranCommit(IDbTransaction tran)
        {
            tran.Commit();
            //因为生命周期被注释
            if (tran.Connection.State == ConnectionState.Open)
                tran.Connection.Close();
        }


        /// <summary>
        /// 执行sql返回DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useWriteConn"></param>
        /// <returns></returns>
        public static DataTable ExecuteSqlReturnDataTable(string sql, object param = null, bool useWriteConn = false, IDbTransaction transaction = null)
        {
            using (IDbConnection conn = GetConnection(useWriteConn))
            {
                try
                {

                    conn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(conn.ExecuteReader(sql));
                    return dt;
                }
                catch (Exception ex)
                {
                    conn.Close();
                    conn.Dispose();
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行存储过程返回DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useWriteConn"></param>
        /// <returns></returns>
        public static DataTable ExecuteProcedureReturnDataTable(string sql, object param = null, bool useWriteConn = false, IDbTransaction transaction = null)
        {
            using (IDbConnection conn = GetConnection(useWriteConn))
            {
                try
                {

                    conn.Open();
                    DataTable dt = new DataTable();
                    IDataReader idr = conn.ExecuteReader(sql, param, commandTimeout: commandTimeout, transaction: transaction, commandType: CommandType.StoredProcedure);
                    dt.Load(idr);
                    return dt;
                }
                catch (Exception ex)
                {
                    conn.Close();
                    conn.Dispose();
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }


        /// <summary>
        /// 执行存储过程返回多个对象
        /// DynamicParameters para = new DynamicParameters();
        /// para.Add("@CurrentStatus", "");
        /// para.Add("@Start", "");
        /// para.Add("@End", "");
        /// para.Add("@IsList", IsList);
        /// //调用存储过程
        ///return DapperHelper.ExecuteProcedureReturnList<T>(ProDuce, para);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useWriteConn"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static List<T> ExecuteProcedureReturnList<T>(string sql, object param = null, bool useWriteConn = false, IDbTransaction transaction = null)
        {
            using (IDbConnection conn = GetConnection(useWriteConn))
            {
                try
                {

                    conn.Open();
                    return conn.Query<T>(sql, param, commandTimeout: commandTimeout, transaction: transaction, commandType: CommandType.StoredProcedure).ToList();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    conn.Dispose();
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }


        /// <summary>
        ///  执行sql返回一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useWriteConn"></param>
        /// <returns></returns>
        public static T ExecuteReaderRetT<T>(string sql, object param = null, bool useWriteConn = false, IDbTransaction transaction = null)
        {
            if (transaction == null)
            {
                using (IDbConnection conn = GetConnection(useWriteConn))
                {
                    try
                    {
                        conn.Open();
                        return conn.QueryFirstOrDefault<T>(sql, param, commandTimeout: commandTimeout);
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        conn.Dispose();
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            else
            {
                var conn = transaction.Connection;
                return conn.QueryFirstOrDefault<T>(sql, param, commandTimeout: commandTimeout, transaction: transaction);
            }

        }
        /// <summary>
        /// 执行sql返回多个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useWriteConn"></param>
        /// <returns></returns>
        public static List<T> ExecuteReaderRetList<T>(string sql, object param = null, bool useWriteConn = false, IDbTransaction transaction = null)
        {
            using (IDbConnection conn = GetConnection(useWriteConn))
            {
                try
                {
                    conn.Open();
                    return conn.Query<T>(sql, param, commandTimeout: commandTimeout, transaction: transaction).ToList();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    conn.Dispose();
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        /// <summary>
        /// 执行sql返回一个对象--异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useWriteConn"></param>
        /// <returns></returns>
        public static async Task<T> ExecuteReaderRetTAsync<T>(string sql, object param = null, bool useWriteConn = false)
        {
            using (IDbConnection conn = GetConnection(useWriteConn))
            {
                try
                {
                    conn.Open();
                    return await conn.QueryFirstOrDefaultAsync<T>(sql, param, commandTimeout: commandTimeout).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    conn.Close();
                    conn.Dispose();
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }

            }
        }
        /// <summary>
        /// 执行sql返回多个对象--异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useWriteConn"></param>
        /// <returns></returns>
        public static async Task<List<T>> ExecuteReaderRetListAsync<T>(string sql, object param = null, bool useWriteConn = false)
        {
            using (IDbConnection conn = GetConnection(useWriteConn))
            {
                try
                {
                    conn.Open();
                    var list = await conn.QueryAsync<T>(sql, param, commandTimeout: commandTimeout).ConfigureAwait(false);
                    return list.ToList();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    conn.Dispose();
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        /// <summary>
        /// 执行sql，返回影响行数 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int ExecuteSqlInt(string sql, object param = null, IDbTransaction transaction = null)
        {
            if (transaction == null)
            {
                using (IDbConnection conn = GetConnection(true))
                {
                    try
                    {
                        conn.Open();
                        return conn.Execute(sql, param, commandTimeout: commandTimeout, commandType: CommandType.Text);
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        conn.Dispose();
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            else
            {
                var conn = transaction.Connection;
                return conn.Execute(sql, param, transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.Text);
            }
        }
        /// <summary>
        /// 执行sql，返回影响行数--异步
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteSqlIntAsync(string sql, object param = null, IDbTransaction transaction = null)
        {
            if (transaction == null)
            {
                using (IDbConnection conn = GetConnection(true))
                {
                    try
                    {
                        conn.Open();
                        return await conn.ExecuteAsync(sql, param, commandTimeout: commandTimeout, commandType: CommandType.Text).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        conn.Dispose();
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            else
            {
                var conn = transaction.Connection;
                return await conn.ExecuteAsync(sql, param, transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.Text).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// 根据id获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="transaction"></param>
        /// <param name="useWriteConn"></param>
        /// <returns></returns>
        public static T GetById<T>(int id, IDbTransaction transaction = null, bool useWriteConn = false) where T : class
        {
            if (transaction == null)
            {
                using (IDbConnection conn = GetConnection(useWriteConn))
                {
                    try
                    {
                        conn.Open();
                        return conn.Get<T>(id, commandTimeout: commandTimeout);
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        conn.Dispose();
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            else
            {
                var conn = transaction.Connection;
                return conn.Get<T>(id, transaction: transaction, commandTimeout: commandTimeout);
            }
        }
        /// <summary>
        /// 根据id获取实体--异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="transaction"></param>
        /// <param name="useWriteConn"></param>
        /// <returns></returns>
        public static async Task<T> GetByIdAsync<T>(int id, IDbTransaction transaction = null, bool useWriteConn = false) where T : class
        {
            if (transaction == null)
            {
                using (IDbConnection conn = GetConnection(useWriteConn))
                {
                    try
                    {
                        conn.Open();
                        return await conn.GetAsync<T>(id, commandTimeout: commandTimeout);
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        conn.Dispose();
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            else
            {
                var conn = transaction.Connection;
                return await conn.GetAsync<T>(id, transaction: transaction, commandTimeout: commandTimeout);
            }
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int ExecuteInsert<T>(T item, IDbTransaction transaction = null) where T : class
        {
            if (transaction == null)
            {
                using (IDbConnection conn = GetConnection(true))
                {
                    try
                    {
                        conn.Open();
                        var res = conn.Insert<T>(item, commandTimeout: commandTimeout);
                        return res;
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        conn.Dispose();
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            else
            {
                var conn = transaction.Connection;
                return conn.Insert(item, transaction: transaction, commandTimeout: commandTimeout);
            }
        }

        
        /// <summary>
        /// 更新单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool ExecuteUpdate<T>(T item, IDbTransaction transaction = null) where T : class
        {
            if (transaction == null)
            {
                using (IDbConnection conn = GetConnection(true))
                {
                    try
                    {
                        conn.Open();
                        return conn.Update(item, commandTimeout: commandTimeout);
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        conn.Dispose();
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            else
            {
                var conn = transaction.Connection;
                return conn.Update(item, transaction: transaction, commandTimeout: commandTimeout);
            }
        }



        /// <summary>
        /// 多表操作--事务
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="databaseOption"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static Tuple<bool, string> ExecuteTransaction(List<Tuple<string, object>> trans, int databaseOption = 1, int? commandTimeout = null)
        {
            if (!trans.Any()) return new Tuple<bool, string>(false, "执行事务SQL语句不能为空！");
            using (IDbConnection conn = GetConnection(true))
            {
                //开启事务
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var sb = new StringBuilder("ExecuteTransaction 事务： ");
                        foreach (var tran in trans)
                        {
                            //sb.Append("SQL语句:" + tran.Item1 + "  \n SQL参数: " + JsonConvert.SerializeObject(tran.Item2) + " \n");
                            // 根据业务添加纪录日志logger.Info("SQL语句:" + tran.Item1 + "  \n SQL参数: " + JsonConvert.SerializeObject(tran.Item2) + " \n");
                            //执行事务
                            conn.Execute(tran.Item1, tran.Item2, transaction, commandTimeout);
                        }
                        var sw = new Stopwatch();
                        sw.Start();
                        //提交事务
                        transaction.Commit();
                        sw.Stop();
                        //根据业务添加纪录日志 logger.Info(sb.ToString() + "耗时:" + sw.ElapsedMilliseconds + (sw.ElapsedMilliseconds > 1000 ?"#####" : string.Empty) + "\n");
                        return new Tuple<bool, string>(true, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        //logger.Info(ex);
                        //回滚事务
                        transaction.Rollback();
                        conn.Close();
                        conn.Dispose();
                        return new Tuple<bool, string>(false, ex.ToString());
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
        }




        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">主sql 不带 order by</param>
        /// <param name="sort">排序内容 id desc，add_time asc</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页多少条</param>
        /// <param name="useWriteConn">是否主库</param>
        /// <returns></returns>
        public static List<T> ExecutePageList<T>(string sql, string sort, int pageIndex, int pageSize, bool useWriteConn = false, object param = null)
        {
            string pageSql = @"SELECT TOP {0} * FROM (SELECT ROW_NUMBER() OVER (ORDER BY {1}) _row_number_,*  FROM 
              ({2})temp )temp1 WHERE temp1._row_number_>{3} ORDER BY _row_number_";
            string execSql = string.Format(pageSql, pageSize, sort, sql, pageSize * (pageIndex - 1));
            using (IDbConnection conn = GetConnection(useWriteConn))
            {
                try
                {
                    conn.Open();

                    return conn.Query<T>(execSql, param, commandTimeout: commandTimeout).ToList();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    conn.Dispose();
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }


        /// <summary>
        /// 批量转移数据(利用SqlBulkCopy实现快速大批量插入到指定的目的表及SqlDataAdapter的批量删除)
        /// </summary>
        public static bool BatchMoveData(string srcSelectSql, string srcTableName, List<SqlParameter> srcPrimarykeyParams, string destConnName, string destTableName)
        {

            using (SqlDataAdapter srcSqlDataAdapter = new SqlDataAdapter(srcSelectSql, GetConnection(true).ConnectionString))
            {
                DataTable srcTable = new DataTable();
                SqlCommand deleteCommand = null;
                try
                {
                    srcSqlDataAdapter.AcceptChangesDuringFill = true;
                    srcSqlDataAdapter.AcceptChangesDuringUpdate = false;
                    srcSqlDataAdapter.Fill(srcTable);

                    if (srcTable == null || srcTable.Rows.Count <= 0) return true;

                    string notExistsDestSqlWhere = null;
                    string deleteSrcSqlWhere = null;

                    for (int i = 0; i < srcPrimarykeyParams.Count; i++)
                    {
                        string keyColName = srcPrimarykeyParams[i].ParameterName.Replace("@", "");
                        notExistsDestSqlWhere += string.Format(" AND told.{0}=tnew.{0}", keyColName);
                        deleteSrcSqlWhere += string.Format(" AND {0}=@{0}", keyColName);
                    }

                    string dbProviderName2 = null;
                    using (var destConn = new SqlConnection(GetConnection(true).ConnectionString))
                    {
                        destConn.Open();

                        string tempDestTableName = "#temp_" + destTableName;
                        destConn.Execute(string.Format("select top 0 * into {0} from {1}", tempDestTableName, destTableName));
                        string destInsertCols = null;
                        using (var destSqlBulkCopy = new SqlBulkCopy(destConn))
                        {
                            try
                            {
                                destSqlBulkCopy.BulkCopyTimeout = 120;
                                destSqlBulkCopy.DestinationTableName = tempDestTableName;
                                foreach (DataColumn col in srcTable.Columns)
                                {
                                    destSqlBulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                                    destInsertCols += "," + col.ColumnName;
                                }

                                destSqlBulkCopy.BatchSize = 1000;
                                destSqlBulkCopy.WriteToServer(srcTable);
                            }
                            catch (Exception ex)
                            {
                                //LogUtil.Error("SqlDapperUtil.BatchMoveData.SqlBulkCopy:" + ex.ToString(), "SqlDapperUtil.BatchMoveData");
                            }

                            destInsertCols = destInsertCols.Substring(1);

                            destConn.Execute(string.Format("insert into {1}({0}) select {0} from {2} tnew where not exists(select 1 from {1} told where {3})",
                                             destInsertCols, destTableName, tempDestTableName, notExistsDestSqlWhere.Trim().Substring(3)), null, null, 100);
                        }
                        destConn.Close();
                    }

                    deleteCommand = new SqlCommand(string.Format("DELETE FROM {0} WHERE {1}", srcTableName, deleteSrcSqlWhere.Trim().Substring(3)), srcSqlDataAdapter.SelectCommand.Connection);
                    deleteCommand.Parameters.AddRange(srcPrimarykeyParams.ToArray());
                    deleteCommand.UpdatedRowSource = UpdateRowSource.None;
                    deleteCommand.CommandTimeout = 200;

                    srcSqlDataAdapter.DeleteCommand = deleteCommand;
                    foreach (DataRow row in srcTable.Rows)
                    {
                        row.Delete();
                    }

                    srcSqlDataAdapter.UpdateBatchSize = 1000;
                    srcSqlDataAdapter.Update(srcTable);
                    srcTable.AcceptChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    //LogUtil.Error("SqlDapperUtil.BatchMoveData:" + ex.ToString(), "SqlDapperUtil.BatchMoveData");
                    return false;
                }
                finally
                {
                    if (deleteCommand != null)
                    {
                        deleteCommand.Parameters.Clear();
                    }
                }
            }

        }

        /// <summary>
        /// 批量复制数据（把源DB中根据SQL语句查出的结果批量COPY插入到目的DB的目的表中）
        /// </summary>
        public static TResult BatchCopyData<TResult>(string srcSelectSql, string destConnName, string destTableName, IDictionary<string, string> colMappings, Func<IDbConnection, TResult> afterCoppyFunc)
        {

            using (SqlDataAdapter srcSqlDataAdapter = new SqlDataAdapter(srcSelectSql, GetConnection(true).ConnectionString))
            {
                DataTable srcTable = new DataTable();
                TResult copyResult = default(TResult);
                try
                {
                    srcSqlDataAdapter.AcceptChangesDuringFill = true;
                    srcSqlDataAdapter.AcceptChangesDuringUpdate = false;
                    srcSqlDataAdapter.Fill(srcTable);

                    if (srcTable == null || srcTable.Rows.Count <= 0) return copyResult;


                    string dbProviderName2 = null;
                    using (var destConn = new SqlConnection(GetConnection(true).ConnectionString))
                    {
                        destConn.Open();
                        string tempDestTableName = "#temp_" + destTableName;
                        destConn.Execute(string.Format("select top 0 * into {0} from {1}", tempDestTableName, destTableName));
                        bool bcpResult = false;
                        using (var destSqlBulkCopy = new SqlBulkCopy(destConn))
                        {
                            try
                            {
                                destSqlBulkCopy.BulkCopyTimeout = 120;
                                destSqlBulkCopy.DestinationTableName = tempDestTableName;
                                foreach (var col in colMappings)
                                {
                                    destSqlBulkCopy.ColumnMappings.Add(col.Key, col.Value);
                                }

                                destSqlBulkCopy.BatchSize = 1000;
                                destSqlBulkCopy.WriteToServer(srcTable);
                                bcpResult = true;
                            }
                            catch (Exception ex)
                            {
                                //LogUtil.Error("SqlDapperUtil.BatchMoveData.SqlBulkCopy:" + ex.ToString(), "SqlDapperUtil.BatchMoveData");
                            }
                        }

                        if (bcpResult)
                        {
                            copyResult = afterCoppyFunc(destConn);
                        }

                        destConn.Close();
                    }

                    return copyResult;
                }
                catch (Exception ex)
                {
                    //LogUtil.Error("SqlDapperUtil.BatchCopyData:" + ex.ToString(), "SqlDapperUtil.BatchCopyData");
                    return copyResult;
                }
            }

        }


        /// <summary>
        /// 批量插入功能
        /// </summary>
        public static void InsertBatch<T>(IEnumerable<T> entityList, IDbTransaction transaction = null) where T : class
        {
            var tblName = string.Format("dbo.{0}", typeof(T).Name);
            var tran = (SqlTransaction)transaction;
            IDbConnection conn = GetConnection(true);

            using (var bulkCopy = new SqlBulkCopy(conn as SqlConnection, SqlBulkCopyOptions.TableLock, tran))
            {
                bulkCopy.BatchSize = entityList.Count();
                bulkCopy.DestinationTableName = tblName;
                var table = new DataTable();
                DapperExtensions.Sql.ISqlGenerator sqlGenerator = new SqlGeneratorImpl(new DapperExtensionsConfiguration());
                var classMap = sqlGenerator.Configuration.GetMap<T>();
                var props = classMap.Properties.Where(x => x.Ignored == false).ToArray();
                foreach (var propertyInfo in props)
                {
                    bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                    table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyInfo.PropertyType) ?? propertyInfo.PropertyInfo.PropertyType);
                }
                var values = new object[props.Count()];
                foreach (var itemm in entityList)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].PropertyInfo.GetValue(itemm, null);
                    }
                    table.Rows.Add(values);
                }
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                bulkCopy.WriteToServer(table);
            }
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
    }
}
