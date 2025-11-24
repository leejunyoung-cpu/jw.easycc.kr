using CommonLibrary.CommonModule;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DbConCommon
{
    public class ConnectGlobal
    {
        private int _intErrCode;
        private string _strErrMsg = string.Empty;
        public int LastErrorCode => _intErrCode;
        public string LastErrorMessage => _strErrMsg;

        private SqlCommand cmd;
        private SqlConnection connection;

        public bool IsConnected { get; private set; } = false;
        public string IsResult { get; private set; } = "";
        public void Open(string lo_strConnectInfo, string lo_strSPName)
        {
            try
            {
                connection = new SqlConnection(lo_strConnectInfo);
                connection.Open();
                cmd = new SqlCommand(lo_strSPName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (connection.State == ConnectionState.Open)
                {
                    IsConnected = true;
                }
            }
            catch (Exception ex)
            {
                _intErrCode = 9901;
                _strErrMsg = string.Format("[ {0} ] {1}({2})", "Open()", ex.Message, ex.StackTrace);

                SiteGlobal.WriteLog("ConnectGlobal", "Exception",
                                "\r\n\t[ex.Message] : " + ex.Message + "\r\n\t[ex.StackTrace] : " + ex.StackTrace,
                                _intErrCode);
                IsResult = ex.Message;
                IsConnected = false;
            }
            finally
            {
                ClearErrMessage();
            }
        }

        public void AddInputParam(string lo_strParamName, SqlDbType lo_dtDBType, object lo_objValue, int lo_intSize)
        {
            try
            {
                SqlParameter in_Parameter = new SqlParameter(lo_strParamName, lo_dtDBType, lo_intSize)
                {
                    Value = lo_objValue ?? DBNull.Value
                };
                cmd.Parameters.Add(in_Parameter);


            }
            catch (Exception ex)
            {
                _intErrCode = 9902;
                _strErrMsg = string.Format("[ {0} ] {1}({2})", "AddInputParam()", ex.Message, ex.StackTrace);

                SiteGlobal.WriteLog("ConnectGlobal", "Exception",
                                "\r\n\t[ex.Message] : " + ex.Message + "\r\n\t[ex.StackTrace] : " + ex.StackTrace,
                                _intErrCode);
            }
            finally
            {
                ClearErrMessage();
            }
        }
        public void AddInputParam(string lo_strParamName, SqlDbType lo_dtDBType, object lo_objValue)
        {
            try
            {
                SqlParameter in_Parameter = new SqlParameter(lo_strParamName, lo_dtDBType)
                {
                    Value = lo_objValue ?? DBNull.Value
                };
                cmd.Parameters.Add(in_Parameter);


            }
            catch (Exception ex)
            {
                _intErrCode = 9902;
                _strErrMsg = string.Format("[ {0} ] {1}({2})", "AddInputParam()", ex.Message, ex.StackTrace);

                SiteGlobal.WriteLog("ConnectGlobal", "Exception",
                                "\r\n\t[ex.Message] : " + ex.Message + "\r\n\t[ex.StackTrace] : " + ex.StackTrace,
                                _intErrCode);
            }
            finally
            {
                ClearErrMessage();
            }
        }

        public void AddOutputParam(string lo_strParamName, SqlDbType lo_dtDBType, int lo_intSize)
        {
            try
            {
                SqlParameter out_Parameter = new SqlParameter(lo_strParamName, lo_dtDBType, lo_intSize)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(out_Parameter);
            }
            catch (Exception ex)
            {
                _intErrCode = 9903;
                _strErrMsg = string.Format("[ {0} ] {1}({2})", "AddOutputParam()", ex.Message, ex.StackTrace);

                SiteGlobal.WriteLog("ConnectGlobal", "Exception",
                                "\r\n\t[ex.Message] : " + ex.Message + "\r\n\t[ex.StackTrace] : " + ex.StackTrace,
                                _intErrCode);
            }
            finally
            {
                ClearErrMessage();
            }
        }

        public void SetQuery()
        {
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception lo_ex)
            {
                _intErrCode = 9904;
                _strErrMsg = string.Format("[ {0} ] {1}({2})", "SetQuery()", lo_ex.Message, lo_ex.StackTrace);

                SiteGlobal.WriteLog("ConnectGlobal", "Exception",
                                "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                                _intErrCode);
            }
            finally
            {
                ClearErrMessage();
            }
        }

        public string GetOutputParamValue(string lo_strParamName)
        {
            try
            {
                var value = cmd.Parameters[lo_strParamName].Value;
                return value == DBNull.Value ? null : value.ToString();
            }
            catch (Exception ex)
            {
                _intErrCode = 9905;
                _strErrMsg = string.Format("[ {0} ] {1}({2})", "GetOutputParamValue()", ex.Message, ex.StackTrace);
                SiteGlobal.WriteLog("ConnectGlobal", "Exception",
                                "\r\n\t[ex.Message] : " + ex.Message + "\r\n\t[ex.StackTrace] : " + ex.StackTrace,
                                _intErrCode);
                return null;
            }
        }

        public DataTable ExecuteDataTable()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                _intErrCode = 9907;
                _strErrMsg = string.Format("[ {0} ] {1}({2})", "ExecuteDataTable()", ex.Message, ex.StackTrace);
                SiteGlobal.WriteLog("ConnectGlobal", "Exception",
                                "\r\n\t[ex.Message] : " + ex.Message + "\r\n\t[ex.StackTrace] : " + ex.StackTrace,
                                _intErrCode);
            }
            finally
            {
                ClearErrMessage();
            }
            return dt;
        }

        public void Close()
        {
            try
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                _intErrCode = 9906;
                _strErrMsg = string.Format("[ {0} ] {1}({2})", "Close()", ex.Message, ex.StackTrace);
                SiteGlobal.WriteLog("ConnectGlobal", "Exception",
                                "\r\n\t[ex.Message] : " + ex.Message + "\r\n\t[ex.StackTrace] : " + ex.StackTrace,
                                _intErrCode);
            }
            finally
            {
                ClearErrMessage();
            }
        }

        private void ClearErrMessage()
        {
            _strErrMsg = string.Empty;
            _intErrCode = 0;
        }
    }
}