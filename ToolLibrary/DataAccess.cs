using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

namespace ToolLibrary
{
    public class DataAccess
    {
#region 
        private static DataAccess m_Single = null;
        private static readonly object SynObject = new object();
        public static DataAccess GetInstance
        {
            get
            {
                if (m_Single == null)
                {
                    lock (SynObject)
                    {
                        return m_Single ?? (m_Single = new DataAccess());
                    }
                }
                return m_Single;
            }
        }
#endregion
        private string m_strConnect = string.Empty;
        public DataAccess()
        {
            string dbserver = System.Configuration.ConfigurationManager.AppSettings["DbServer"];
		    string database = System.Configuration.ConfigurationManager.AppSettings["DataBase"];
		    string user = System.Configuration.ConfigurationManager.AppSettings["DbUser"];
		    string pwd = System.Configuration.ConfigurationManager.AppSettings["DbPwd"];
            m_strConnect = "server=" + dbserver + ";database=" + database + ";uid=" + user + ";pwd=" + pwd;
        }
        public int Update(string sql)
        {
            int result = 0;
            using (SqlConnection myConnection = new SqlConnection(m_strConnect))
            {
                SqlCommand myCommand = new SqlCommand(sql, myConnection);
                myConnection.Open();
                result = myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
            return result;
        }

        public DataSet GetList(string sql)
        {
            DataSet ds = new DataSet();
            using (SqlConnection myConnection = new SqlConnection(m_strConnect))
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
                da.Fill(ds);
            }            
            return ds;            
        }
        public int ExecStoredProcedure(string strName,System.Data.SqlClient.SqlParameterCollection param)
        {
            try
            {
                using (SqlConnection myConnection = new SqlConnection(m_strConnect))
                {
                    System.Data.SqlClient.SqlCommand myCommand =
                                new System.Data.SqlClient.SqlCommand(strName, myConnection);
                    myCommand.CommandType = CommandType.StoredProcedure;
                    //添加输入查询参数、赋予值
                    myCommand.Parameters.Clear();
                    foreach (SqlParameter sp in param)
                    {                        
                        myCommand.Parameters.Add(new SqlParameter(sp.ParameterName,sp.SqlDbType,sp.Size
                            ,sp.Direction,sp.IsNullable,sp.Precision,sp.Scale,sp.SourceColumn,sp.SourceVersion
                            ,sp.Value));
                    }
                    myConnection.Open();
                    int res = myCommand.ExecuteNonQuery();
                    myConnection.Close();
                    return res;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }
        public DataSet ExecStoredProcedureForDs(string strName, System.Data.SqlClient.SqlParameterCollection param)
        {
            try
            {
                using (SqlConnection myConnection = new SqlConnection(m_strConnect))
                {
                    System.Data.SqlClient.SqlCommand myCommand =
                                new System.Data.SqlClient.SqlCommand(strName, myConnection);
                    myCommand.CommandType = CommandType.StoredProcedure;
                    //添加输入查询参数、赋予值
                    myCommand.Parameters.Clear();
                    foreach (SqlParameter sp in param)
                    {
                        myCommand.Parameters.Add(new SqlParameter(sp.ParameterName, sp.SqlDbType, sp.Size
                            , sp.Direction, sp.IsNullable, sp.Precision, sp.Scale, sp.SourceColumn, sp.SourceVersion
                            , sp.Value));
                    }
                    SqlDataAdapter da = new SqlDataAdapter(myCommand);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}