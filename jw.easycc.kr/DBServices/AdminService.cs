using System;
using System.Collections.Generic;
using CommonLibrary.DBModel;
using CommonLibrary.CommonModel;
using CommonLibrary.CommonModule;
using Newtonsoft.Json;
using DbConCommon;
using System.Data;
using CommonLibrary.Extensions;
using CommonLibrary.Constants;

namespace CommonLibrary.DBServices
{
    public class AdminService
    {
        private bool bLogWrite = SiteGlobal.B_LOG_WRITE;
        public ServiceResult<ResAdminList> GetAdminList(ReqAdminList objReqAdminList) {
            SiteGlobal.WriteInformation("AdminService", "I", $"[GetAdminList REQ] {JsonConvert.SerializeObject(objReqAdminList)}", bLogWrite);

            string lo_strJson = string.Empty;
            ServiceResult<ResAdminList> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try {
                lo_objResult = new ServiceResult<ResAdminList>(999);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_AR_LST");
                lo_objDB.AddInputParam("@pi_strAdminID",        SqlDbType.VarChar,  objReqAdminList.AdminID,    50);
                lo_objDB.AddInputParam("@pi_strMobileNo",       SqlDbType.VarChar,  objReqAdminList.MobileNo,  20);
                lo_objDB.AddInputParam("@pi_strAdminName",      SqlDbType.NVarChar, objReqAdminList.AdminName,  50);
                lo_objDB.AddInputParam("@pi_intGradeCode",      SqlDbType.TinyInt,  objReqAdminList.GradeCode,  4);
                lo_objDB.AddInputParam("@pi_strUseFlag",        SqlDbType.Char,     objReqAdminList.UseFlag,    1);

                lo_objDB.AddInputParam("@pi_intSesGradeCode",       SqlDbType.Int,      objReqAdminList.SesGradeCode, 4);
                lo_objDB.AddInputParam("@pi_strAccessClientCode",   SqlDbType.VarChar,  objReqAdminList.AccessClientCode, 512);
                lo_objDB.AddInputParam("@pi_intUpsCsType",          SqlDbType.TinyInt,  objReqAdminList.UpsCsType, 0);
                lo_objDB.AddInputParam("@pi_intPageSize",           SqlDbType.Int,      objReqAdminList.PageSize, 4);
                lo_objDB.AddInputParam("@pi_intPageNo",             SqlDbType.Int,      objReqAdminList.PageNo, 4);

                lo_objDB.AddOutputParam("@po_intRecordCnt",         SqlDbType.Int, 4);
                lo_objDB.SetQuery();

                if (!lo_objDB.LastErrorCode.Equals(0)) {
                    lo_objResult.SetResult(996, lo_objDB.LastErrorMessage
                                         , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //Response 값 셋팅                
                lo_objResult.SetResult(0);

                lo_objResult.data = new ResAdminList
                {
                    list = new List<AdminViewModel>(),
                    RecordCnt = lo_objDB.ExecuteDataTable().Rows.Count,
                };

                if (lo_objDB.ExecuteDataTable().Rows.Count > 0) {
                    lo_strJson = JsonConvert.SerializeObject(lo_objDB.ExecuteDataTable());
                    lo_objResult.data.list = JsonConvert.DeserializeObject<List<AdminViewModel>>(lo_strJson);
                }
            }
            catch(Exception lo_ex) {
                lo_objResult.SetResult(997, "An unexpected error occurred during accessing data"
                                     , 9101, "System error(GetAdminList)" + lo_ex.Message);
            }
            finally{
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }
                SiteGlobal.WriteInformation("AdminService", "I", $"[GetAdminList RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }
            return lo_objResult;
        }

        public ServiceResult<AdminSessionInfo> GetAdminSession(ref ConnectGlobal objDB, string strSessionKey, string strIPAddr)
        {
            bool bReuseSocket = false;
            SiteGlobal.WriteInformation("AdminService", "I", $"[GetAdminSession REQ] {strSessionKey}/{strIPAddr}", bLogWrite);

            ServiceResult<AdminSessionInfo> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {

                if (objDB != null)
                {
                    lo_objDB = objDB;
                    bReuseSocket = true;
                }
                else
                {
                    lo_objDB = new ConnectGlobal();
                }
                lo_objResult = new ServiceResult<AdminSessionInfo>(999);
                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_SESSION_TX_GET");
                lo_objDB.AddInputParam("@pi_strSessionKey",         SqlDbType.VarChar,  strSessionKey, 50);
                lo_objDB.AddInputParam("@pi_strIPAddr",             SqlDbType.VarChar,  strIPAddr, 50);
                lo_objDB.AddOutputParam("@po_strAdminID",           SqlDbType.VarChar,  50);
                lo_objDB.AddOutputParam("@po_strMobileNo",          SqlDbType.VarChar,  20);
                lo_objDB.AddOutputParam("@po_strAdminName",         SqlDbType.NVarChar, 50);

                lo_objDB.AddOutputParam("@po_strPrivateAvailFlag",  SqlDbType.Char,     1);
                lo_objDB.AddOutputParam("@po_strEmail",             SqlDbType.NVarChar, 100);
                lo_objDB.AddOutputParam("@po_strAccessClientCode",  SqlDbType.VarChar,  512);
                lo_objDB.AddOutputParam("@po_intGradeCode",         SqlDbType.TinyInt,  4);
                lo_objDB.AddOutputParam("@po_strGradeName",         SqlDbType.NVarChar, 50);

                lo_objDB.AddOutputParam("@po_strLastLoginDate",     SqlDbType.VarChar,  19);
                lo_objDB.AddOutputParam("@po_strLastLoginIP",       SqlDbType.VarChar,  50);
                lo_objDB.AddOutputParam("@po_strPwdUpdDate",        SqlDbType.VarChar,  19);
                lo_objDB.AddOutputParam("@po_strExpireYMD",         SqlDbType.VarChar,  10);
                lo_objDB.AddOutputParam("@po_strErrMsg",            SqlDbType.VarChar,  256);

                lo_objDB.AddOutputParam("@po_intRetVal",            SqlDbType.Int,      4);
                lo_objDB.AddOutputParam("@po_strDBErrMsg",          SqlDbType.VarChar,  256);
                lo_objDB.AddOutputParam("@po_intDBRetVal",          SqlDbType.Int,      4);
                lo_objDB.SetQuery();

                //DAS 통신 실패이면 오류로 리턴
                if (lo_objDB.LastErrorCode.IsFail())
                {
                    lo_objResult.SetResult(998, "An unexpected error occurred during accessing data"
                                            , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //트랜잭션이없다.
                if (lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt().IsFail())
                {
                    lo_objResult.SetResult(lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strErrMsg")
                                         , lo_objDB.GetOutputParamValue("@po_intDBRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strDBErrMsg"));
                    return lo_objResult;
                }

                //Response 값 셋팅                
                lo_objResult.SetResult(0);

                lo_objResult.data = new AdminSessionInfo
                {
                    AdminID             = lo_objDB.GetOutputParamValue("@po_strAdminID").ToString(),
                    MobileNo            = lo_objDB.GetOutputParamValue("@po_strMobileNo").ToString(),
                    AdminName           = lo_objDB.GetOutputParamValue("@po_strAdminName").ToString(),
                    PrivateAvailFlag    = lo_objDB.GetOutputParamValue("@po_strPrivateAvailFlag").ToString(),
                    Email               = lo_objDB.GetOutputParamValue("@po_strEmail").ToString(),
                    AccessClientCode    = lo_objDB.GetOutputParamValue("@po_strAccessClientCode"),
                    GradeCode           = lo_objDB.GetOutputParamValue("@po_intGradeCode").ToInt(),
                    GradeName           = lo_objDB.GetOutputParamValue("@po_strGradeName").ToString(),
                    LastLoginDate       = lo_objDB.GetOutputParamValue("@po_strLastLoginDate").ToString(),
                    LastLoginIP         = lo_objDB.GetOutputParamValue("@po_strLastLoginIP").ToString(),
                    PwdUpdDate          = lo_objDB.GetOutputParamValue("@po_strPwdUpdDate").ToString(),
                    ExpireYmd           = lo_objDB.GetOutputParamValue("@po_strExpireYMD").ToString()
                };
                
            }
            catch (Exception lo_ex)
            {
                lo_objResult.SetResult(998, "An unexpected error occurred during accessing data"
                                     , 9102, "System error(GetAdminSession)" + lo_ex.Message);
            }
            finally
            {

                if (bReuseSocket.Equals(false) && lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }
                SiteGlobal.WriteInformation("AdminService", "I", $"[GetAdminSession RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<AdminPwdInfo> GetCurrentPassword(string strAdminID)
        {
            SiteGlobal.WriteInformation("AdminService", "I", $"[GetCurrentPassword REQ] {strAdminID}", bLogWrite);

            ServiceResult<AdminPwdInfo> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<AdminPwdInfo>(999);
                lo_objDB = new ConnectGlobal();
                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_PWD_NT_GET");


                lo_objDB.AddInputParam("@pi_strAdminID",            SqlDbType.VarChar, strAdminID, 50);
                lo_objDB.AddOutputParam("@po_strAdminPwd",          SqlDbType.VarChar, 256);
                lo_objDB.AddOutputParam("@po_intTodayLoginFailCnt", SqlDbType.Int, 4);
                lo_objDB.AddOutputParam("@po_intAdminLogInTryCnt",  SqlDbType.Int, 4);
                lo_objDB.AddOutputParam("@po_intGradeCode",         SqlDbType.TinyInt, 4);

                lo_objDB.AddOutputParam("@po_strUseFlag",           SqlDbType.Char, 1);
                lo_objDB.AddOutputParam("@po_strErrMsg",            SqlDbType.VarChar, 256);
                lo_objDB.AddOutputParam("@po_intRetVal",            SqlDbType.Int, 4);
                lo_objDB.AddOutputParam("@po_strDBErrMsg",          SqlDbType.VarChar, 256);
                lo_objDB.AddOutputParam("@po_intDBRetVal",          SqlDbType.Int, 4);
                lo_objDB.SetQuery();

                //DAS 통신 실패이면 오류로 리턴
                if (lo_objDB.LastErrorCode.IsFail())
                {
                    lo_objResult.SetResult(998, "An unexpected error occurred during accessing data"
                                            , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //트랜잭션이없다.
                if (lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt().IsFail())
                {
                    lo_objResult.SetResult(lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt()
                                          , lo_objDB.GetOutputParamValue("@po_strErrMsg")
                                          , lo_objDB.GetOutputParamValue("@po_intDBRetVal").ToInt()
                                          , lo_objDB.GetOutputParamValue("@po_strDBErrMsg"));
                    return lo_objResult;
                }

                //Response 값 셋팅                
                lo_objResult.SetResult(0);

                lo_objResult.data = new AdminPwdInfo
                {
                    CurrPassword    = lo_objDB.GetOutputParamValue("@po_strAdminPwd").ToString(),
                    TodayLoginFailCnt = lo_objDB.GetOutputParamValue("@po_intTodayLoginFailCnt").ToInt(),
                    AdminLogInTryCnt = lo_objDB.GetOutputParamValue("@po_intAdminLogInTryCnt").ToInt(),
                    GradeCode       = lo_objDB.GetOutputParamValue("@po_intGradeCode").ToInt(),
                    UseFlag         = lo_objDB.GetOutputParamValue("@po_strUseFlag").ToString()
                };
            }
            catch (Exception ex)
            {
                lo_objResult.SetResult(998, "An unexpected error occurred during accessing data"
                                     , 9103, "System error(fail to get admin password)" + ex.Message + "/Line : " + ex.StackTrace);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminService", "I", $"[GetCurrentPassword RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<bool> UpdAdminPwd(string strAdminID, string strEncAdminResetPwd, string strRegAdminID)
        {
            SiteGlobal.WriteInformation("AdminService", "I", $"[UpdAdminPwd REQ] {strAdminID}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(999);
                lo_objDB = new ConnectGlobal();
                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_PWD_TX_UPD");


                lo_objDB.AddInputParam("@pi_strAdminID",        SqlDbType.VarChar, strAdminID, 50);
                lo_objDB.AddInputParam("@pi_strAdminPwdNew",    SqlDbType.VarChar, strEncAdminResetPwd, 256);
                lo_objDB.AddInputParam("@pi_strRegAdminID",     SqlDbType.VarChar, strRegAdminID, 50);
                lo_objDB.AddOutputParam("@po_strErrMsg",        SqlDbType.VarChar, 256);
                lo_objDB.AddOutputParam("@po_intRetVal",        SqlDbType.Int,     4);

                lo_objDB.AddOutputParam("@po_strDBErrMsg",      SqlDbType.VarChar, 256);
                lo_objDB.AddOutputParam("@po_intDBRetVal",      SqlDbType.Int,     4);
                lo_objDB.SetQuery();

                //DAS 통신 실패이면 오류로 리턴
                if (lo_objDB.LastErrorCode.IsFail())
                {
                    lo_objResult.SetResult(998, "An unexpected error occurred during accessing data"
                                            , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //트랜잭션이없다.
                if (lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt().IsFail())
                {
                    lo_objResult.SetResult(lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strErrMsg")
                                         , lo_objDB.GetOutputParamValue("@po_intDBRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strDBErrMsg"));
                    return lo_objResult;
                }

                //Response 값 셋팅                
                lo_objResult.SetResult(0);
            }
            catch (Exception ex)
            {
                lo_objResult.SetResult(998, "An unexpected error occurred during accessing data"
                                     , 9104, "System error(fail to get admin password)" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminService", "I", $"[UpdAdminPwd RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<bool> InsAdminLoginFail(string strAdminID, string strLoginFailDesc)
        {
            SiteGlobal.WriteInformation("AdminService", "I", $"[InsAdminLoginFail REQ] {strAdminID}/{strLoginFailDesc}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(999);
                lo_objDB = new ConnectGlobal();
                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_LOGIN_FAIL_TX_INS");

                lo_objDB.AddInputParam("@pi_strAdminID", SqlDbType.VarChar, strAdminID, 50);
                lo_objDB.AddInputParam("@pi_strLoginFailDesc", SqlDbType.VarChar, strLoginFailDesc, 4000);
                lo_objDB.AddOutputParam("@po_strErrMsg", SqlDbType.VarChar, 256);
                lo_objDB.AddOutputParam("@po_intRetVal", SqlDbType.Int, 4);
                lo_objDB.AddOutputParam("@po_strDBErrMsg", SqlDbType.VarChar, 256);

                lo_objDB.AddOutputParam("@po_intDBRetVal", SqlDbType.Int, 4);
                lo_objDB.SetQuery();

                //DAS 통신 실패이면 오류로 리턴
                if (lo_objDB.LastErrorCode.IsFail())
                {
                    lo_objResult.SetResult(998, "An unexpected error occurred during accessing data"
                                            , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //트랜잭션이없다.
                if (lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt().IsFail())
                {
                    lo_objResult.SetResult(lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strErrMsg")
                                         , lo_objDB.GetOutputParamValue("@po_intDBRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strDBErrMsg"));
                    return lo_objResult;
                }

                //Response 값 셋팅                
                lo_objResult.SetResult(0);
            }
            catch (Exception ex)
            {
                lo_objResult.SetResult(998, "An unexpected error occurred during accessing data"
                                     , 9105, "System error(fail to get admin password)" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminService", "I", $"[InsAdminLoginFail RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<AdminSessionInfo> InsAdminLog(ref ConnectGlobal objDas, string strAdminID, int intMenuNo, string strMenuLink, int intMenuAuthType, int intPageType, string strCallParam)
        {
            SiteGlobal.WriteInformation("AdminService", "I", $"[InsAdminLog REQ] {strAdminID}/{strAdminID}/{intMenuNo}/{strMenuLink}/{intMenuAuthType}/{strCallParam}", bLogWrite);

            ServiceResult<AdminSessionInfo> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<AdminSessionInfo>(999);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_LOG_TX_INS");

                lo_objDB.AddInputParam("@pi_strAdminID",        SqlDbType.VarChar,  strAdminID, 50);
                lo_objDB.AddInputParam("@pi_intMenuNo",         SqlDbType.Int,      intMenuNo, 4);
                lo_objDB.AddInputParam("@pi_strMenuLink",       SqlDbType.VarChar,  strMenuLink, 100);
                lo_objDB.AddInputParam("@pi_intMenuAuthType",   SqlDbType.TinyInt,  intMenuAuthType, 4);
                lo_objDB.AddInputParam("@pi_intPageType",       SqlDbType.TinyInt,  intPageType, 4);

                lo_objDB.AddInputParam("@pi_strIPAddr",         SqlDbType.VarChar,  SiteGlobal.GetRemoteAddr(), 50);
                lo_objDB.AddInputParam("@pi_strCallParam",      SqlDbType.NVarChar, strCallParam, 4000);
                lo_objDB.AddOutputParam("@po_intSeqNo",         SqlDbType.Int,      4);
                lo_objDB.AddOutputParam("@po_strErrMsg",        SqlDbType.VarChar,  256);
                lo_objDB.AddOutputParam("@po_intRetVal",        SqlDbType.Int,      4);

                lo_objDB.AddOutputParam("@po_strDBErrMsg",      SqlDbType.VarChar,  256);
                lo_objDB.AddOutputParam("@po_intDBRetVal",      SqlDbType.Int,      4);
                lo_objDB.SetQuery();

                //DAS 통신 실패이면 오류로 리턴
                if (lo_objDB.LastErrorCode.IsFail())
                {
                    lo_objResult.SetResult(998, "An unexpected error occurred during accessing data"
                                            , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //트랜잭션이없다.
                if (lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt().IsFail())
                {
                    lo_objResult.SetResult(lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strErrMsg")
                                         , lo_objDB.GetOutputParamValue("@po_intDBRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strDBErrMsg"));
                    return lo_objResult;
                }

                //Response 값 셋팅                
                lo_objResult.SetResult(0);
            }
            catch (Exception ex)
            {
                lo_objResult.SetResult(998, "An unexpected error occurred during accessing data"
                                     , 9111, "System error(fail to get admin SessionKey)" + ex.Message + "/Line : " + ex.StackTrace);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminService", "I", $"[InsAdminLog RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        ///----------------------------------------------------------------------
        /// <summary>
        /// Insert password reset log into DB
        /// </summary>
        /// <param name="strAdminID">Administrator's ID</param>
        /// <param name="strIPAddr">Accessed IP Address</param>
        /// <returns>Result(0=Success, 0!=Failure)</returns>
        ///----------------------------------------------------------------------
        public ServiceResult<AdminSessionInfo> InsAdminSession(string strAdminID, string strIPAddr)
        {
            SiteGlobal.WriteInformation("AdminService", "I", $"[InsAdminSession REQ] {strAdminID}/{strIPAddr}", bLogWrite);

            ServiceResult<AdminSessionInfo> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<AdminSessionInfo>(999);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_SESSION_TX_INS");

                lo_objDB.AddInputParam("@pi_strAdminID",        SqlDbType.VarChar, strAdminID, 50);
                lo_objDB.AddInputParam("@pi_strIPAddr",         SqlDbType.VarChar, strIPAddr, 20);
                lo_objDB.AddOutputParam("@po_strSessionKey",    SqlDbType.VarChar, 50);
                lo_objDB.AddOutputParam("@po_strLastLoginDate", SqlDbType.VarChar, 19);
                lo_objDB.AddOutputParam("@po_strLastLoginIP",   SqlDbType.VarChar, 50);

                lo_objDB.AddOutputParam("@po_strErrMsg",        SqlDbType.VarChar, 256);
                lo_objDB.AddOutputParam("@po_intRetVal",        SqlDbType.Int, 4);
                lo_objDB.AddOutputParam("@po_strDBErrMsg",      SqlDbType.VarChar, 256);
                lo_objDB.AddOutputParam("@po_intDBRetVal",      SqlDbType.Int, 4);
                lo_objDB.SetQuery();

                //DAS 통신 실패이면 오류로 리턴
                if (lo_objDB.LastErrorCode.IsFail())
                {
                    lo_objResult.SetResult(998, "An unexpected error occurred during accessing data"
                                            , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //트랜잭션이없다.
                if (lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt().IsFail())
                {
                    lo_objResult.SetResult(lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strErrMsg")
                                         , lo_objDB.GetOutputParamValue("@po_intDBRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strDBErrMsg"));
                    return lo_objResult;
                }

                //Response 값 셋팅                
                lo_objResult.SetResult(0);

                lo_objResult.data = new AdminSessionInfo
                {
                    SessionKey = lo_objDB.GetOutputParamValue("@po_strSessionKey").ToString()
                };

                if (null != lo_objDB.GetOutputParamValue("@po_strLastLoginDate"))
                {
                    lo_objResult.data.LastLoginDate = lo_objDB.GetOutputParamValue("@po_strLastLoginDate").ToString();
                    lo_objResult.data.LastLoginIP = CommonUtils.Utils.IsNull(lo_objDB.GetOutputParamValue("@po_strLastLoginIP").ToString(), "");
                }
            }
            catch (Exception ex)
            {
                lo_objResult.SetResult(9987, "An unexpected error occurred during accessing data"
                                     , 9106, "System error(fail to get admin SessionKey)" + ex.Message + "/Line : " + ex.StackTrace);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminService", "I", $"[InsAdminSession RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        
        public ServiceResult<bool> InsAdmin(AdminViewModel objInsAdmin)
        {
            SiteGlobal.WriteInformation("AdminService", "I", $"[InsAdmin REQ] {JsonConvert.SerializeObject(objInsAdmin)}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(999);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_TX_INS");

                lo_objDB.AddInputParam("@pi_strAdminID",            SqlDbType.VarChar,  objInsAdmin.AdminID,            50);
                lo_objDB.AddInputParam("@pi_strAdminPWD",           SqlDbType.VarChar,  objInsAdmin.AdminPWD,           4000);
                lo_objDB.AddInputParam("@pi_strAdminName",          SqlDbType.NVarChar, objInsAdmin.AdminName,          50);
                lo_objDB.AddInputParam("@pi_strMobileNo",           SqlDbType.NVarChar, objInsAdmin.MobileNo,           50);
                lo_objDB.AddInputParam("@pi_strAccessClientCode",   SqlDbType.VarChar,  objInsAdmin.AccessClientCode,   512);

                lo_objDB.AddInputParam("@pi_intGradeCode",          SqlDbType.TinyInt,      objInsAdmin.GradeCode,         20);
                lo_objDB.AddInputParam("@pi_strPrivateAvailFlag",   SqlDbType.Char,         objInsAdmin.PrivateAvailFlag,  1);
                lo_objDB.AddInputParam("@pi_strDeptName",           SqlDbType.NVarChar,     objInsAdmin.DeptName,          100);
                lo_objDB.AddInputParam("@pi_strTelNo",              SqlDbType.VarChar,      objInsAdmin.TelNo,             100);
                lo_objDB.AddInputParam("@pi_strEmail",              SqlDbType.NVarChar,     objInsAdmin.Email,             100);

                lo_objDB.AddInputParam("@pi_strExpireYMD",          SqlDbType.VarChar,      objInsAdmin.ExpireYMD,          8);
                lo_objDB.AddInputParam("@pi_strBtoBYN",             SqlDbType.Char,         objInsAdmin.BtoBYN,             1);
                lo_objDB.AddInputParam("@pi_strBtoCYN",             SqlDbType.Char,         objInsAdmin.BtoCYN,             1);
                lo_objDB.AddInputParam("@pi_intAdminAreaType",      SqlDbType.TinyInt,      objInsAdmin.AdminAreaType,      4);
                lo_objDB.AddInputParam("@pi_intUpsCsType",          SqlDbType.TinyInt,      objInsAdmin.UpsCsType,          4);

                lo_objDB.AddInputParam("@pi_strUseFlag",            SqlDbType.Char,         objInsAdmin.UseFlag,            1);
                lo_objDB.AddInputParam("@pi_strRegAdminID",         SqlDbType.VarChar,      objInsAdmin.RegAdminID,         50);
                lo_objDB.AddOutputParam("@po_strErrMsg",            SqlDbType.VarChar, 256);
                lo_objDB.AddOutputParam("@po_intRetVal",            SqlDbType.Int, 4);
                lo_objDB.AddOutputParam("@po_strDBErrMsg",          SqlDbType.VarChar, 256);
                
                lo_objDB.AddOutputParam("@po_intDBRetVal",          SqlDbType.Int, 4);
                lo_objDB.SetQuery();

                //DAS 통신 실패이면 오류로 리턴
                if (lo_objDB.LastErrorCode.IsFail())
                {
                    lo_objResult.SetResult(998, "An unexpected error occurred during accessing data"
                                            , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //트랜잭션이없다.
                if (lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt().IsFail())
                {
                    lo_objResult.SetResult(lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strErrMsg")
                                         , lo_objDB.GetOutputParamValue("@po_intDBRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strDBErrMsg"));
                    return lo_objResult;
                }

                //Response 값 셋팅                
                lo_objResult.SetResult(0);
            }
            catch (Exception ex)
            {
                lo_objResult.SetResult(998, "An unexpected error occurred during accessing data"
                                     , 9110, "System error(fail to get admin SessionKey)" + ex.Message + "/Line : " + ex.StackTrace);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminService", "I", $"[InsAdmin RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<ResAdminLeftMenuAllList> GetAdminLeftMenuAllList(string strAdminID)
        {
            SiteGlobal.WriteInformation("AdminService", "I", $"[GetAdminLeftMenuAllList REQ] {strAdminID}", bLogWrite);

            string lo_strJson = string.Empty;

            ServiceResult<ResAdminLeftMenuAllList> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<ResAdminLeftMenuAllList>(999);
                lo_objDB = new ConnectGlobal();
                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_LEFTMENU_ALL_AR_LST");
                lo_objDB.AddInputParam("@pi_strAdminID", SqlDbType.VarChar, strAdminID, 50);
                lo_objDB.SetQuery();
                //DAS 통신 실패이면 오류로 리턴
                if (!lo_objDB.LastErrorCode.Equals(0))
                {
                    lo_objResult.SetResult(996, lo_objDB.LastErrorMessage
                                         , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //Response 값 셋팅                
                lo_objResult.SetResult(0);

                lo_objResult.data = new ResAdminLeftMenuAllList
                {
                    list = new List<AdminLeftMenuAllList>(),
                    RecordCnt = lo_objDB.ExecuteDataTable().Rows.Count,
                };

                if (lo_objDB.ExecuteDataTable().Rows.Count > 0)
                {
                    lo_strJson = JsonConvert.SerializeObject(lo_objDB.ExecuteDataTable());
                    lo_objResult.data.list = JsonConvert.DeserializeObject<List<AdminLeftMenuAllList>>(lo_strJson);
                }
            }
            catch (Exception ex)
            {
                lo_objResult.SetResult(999, "An unexpected error occurred during accessing data"
                                     , 9111, "System error(GetAdminLeftMenuGroupList)" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminService", "I", $"[GetAdminLeftMenuAllList RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<AdminMenuChk> CheckAdminMenu(ref ConnectGlobal objDB, string strAdminID, int intGradeCode, string strMenuLink)
        {
            bool bReuseSocket = false;
            SiteGlobal.WriteInformation("AdminService", "I", $"[CheckAdminMenu REQ] {strAdminID}/{intGradeCode}/{strMenuLink}", bLogWrite);

            ServiceResult<AdminMenuChk> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                if (objDB != null)
                {
                    lo_objDB = objDB;
                    bReuseSocket = true;
                }
                else {
                    lo_objDB = new ConnectGlobal();
                }
                lo_objResult = new ServiceResult<AdminMenuChk>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MENU_ACCESS_NT_CHK");

                lo_objDB.AddInputParam("@pi_strAdminID",    SqlDbType.VarChar,     strAdminID,     50);
                lo_objDB.AddInputParam("@pi_intGradeCode",  SqlDbType.TinyInt,     intGradeCode,   4);
                lo_objDB.AddInputParam("@pi_strMenuLink",   SqlDbType.VarChar,     strMenuLink,    100);
                lo_objDB.AddOutputParam("@po_intAuthCode",  SqlDbType.TinyInt,     4);
                lo_objDB.AddOutputParam("@po_strErrMsg",    SqlDbType.VarChar,     256);

                lo_objDB.AddOutputParam("@po_intRetVal",    SqlDbType.Int,         4);
                lo_objDB.AddOutputParam("@po_strDBErrMsg",  SqlDbType.VarChar,     256);
                lo_objDB.AddOutputParam("@po_intDBRetVal",  SqlDbType.Int,         4);

                lo_objDB.SetQuery();

                //DAS 통신 실패이면 오류로 리턴
                if (lo_objDB.LastErrorCode.IsFail())
                {
                    lo_objResult.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE, CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                                            , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //트랜잭션이없다.
                if (lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt().IsFail())
                {
                    lo_objResult.SetResult(lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strErrMsg")
                                         , lo_objDB.GetOutputParamValue("@po_intDBRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strDBErrMsg"));
                    return lo_objResult;
                }

                //Response 값 셋팅                
                lo_objResult.SetResult(CommonConstant.DB_SUCCESS_CODE);
                
                lo_objResult.data = new AdminMenuChk
                {
                    AuthCode = lo_objDB.GetOutputParamValue("@po_intAuthCode").ToInt()
                };
            }
            catch (Exception ex)
            {
                lo_objResult.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE, CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                                     , 9101, "System error(fail to check admin's menu)" + ex.Message);
            }
            finally
            {
                if (bReuseSocket.Equals(false) && lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminService", "I", $"[CheckAdminMenu RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<bool> UpdAdminPwd(string strAdminID, string strEncAdminResetPwd, string strRegAdminID, int intResetFlag)
        {
            SiteGlobal.WriteInformation("AdminService", "I", $"[UpdAdminPwd REQ] {strAdminID}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_PWD_TX_UPD");

                lo_objDB.AddInputParam("@pi_strAdminID",        SqlDbType.VarChar,  strAdminID, 50);
                lo_objDB.AddInputParam("@pi_strAdminPwdNew",    SqlDbType.VarChar,  strEncAdminResetPwd, 256);
                lo_objDB.AddInputParam("@pi_strRegAdminID",     SqlDbType.VarChar,  strRegAdminID, 50);
                lo_objDB.AddOutputParam("@po_strErrMsg",        SqlDbType.VarChar,  256);
                lo_objDB.AddOutputParam("@po_intRetVal",        SqlDbType.Int,      0);

                lo_objDB.AddOutputParam("@po_strDBErrMsg",      SqlDbType.VarChar,  256);
                lo_objDB.AddOutputParam("@po_intDBRetVal",      SqlDbType.Int,      0);

                lo_objDB.SetQuery();

                //DAS 통신 실패이면 오류로 리턴
                if (lo_objDB.LastErrorCode.IsFail())
                {
                    lo_objResult.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE, CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                                            , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //트랜잭션이없다.
                if (lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt().IsFail())
                {
                    lo_objResult.SetResult(lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strErrMsg")
                                         , lo_objDB.GetOutputParamValue("@po_intDBRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strDBErrMsg"));
                    return lo_objResult;
                }

                //Response 값 셋팅                
                lo_objResult.SetResult(CommonConstant.DB_SUCCESS_CODE);
            }
            catch (Exception ex)
            {
                lo_objResult.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE, CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                                     , 9101, "System error(fail to update admin's pwd)" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminService", "I", $"[UpdAdminPwd RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        /// <summary>
        /// 로그인 실패 초기화
        /// </summary>
        /// <param name="strAdminID"></param>
        /// <returns></returns>
        public ServiceResult<bool> ResetLoginAdmin(string strAdminID)
        {
            SiteGlobal.WriteInformation("AdminService", "I", $"[LoginResetAdmin REQ] {strAdminID}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_LOGIN_FAIL_RESET_TX_UPD");

                lo_objDB.AddInputParam("@pi_strAdminID",   SqlDbType.VarChar,  strAdminID, 50);
                lo_objDB.AddInputParam("@pi_intResetFlag", SqlDbType.TinyInt,  2, 0);  // 1:로그인, 2:RESET
                lo_objDB.AddOutputParam("@po_strErrMsg",   SqlDbType.VarChar,  256);
                lo_objDB.AddOutputParam("@po_intRetVal",   SqlDbType.Int,      0);
                lo_objDB.AddOutputParam("@po_strDBErrMsg", SqlDbType.VarChar,  256);

                lo_objDB.AddOutputParam("@po_intDBRetVal", SqlDbType.Int,      0);

                lo_objDB.SetQuery();

                //DAS 통신 실패이면 오류로 리턴
                if (lo_objDB.LastErrorCode.IsFail())
                {
                    lo_objResult.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE, CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                                            , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //트랜잭션이없다.
                if (lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt().IsFail())
                {
                    lo_objResult.SetResult(lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strErrMsg")
                                         , lo_objDB.GetOutputParamValue("@po_intDBRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strDBErrMsg"));
                    return lo_objResult;
                }

                //Response 값 셋팅                
                lo_objResult.SetResult(CommonConstant.DB_SUCCESS_CODE);
            }
            catch (Exception ex)
            {
                lo_objResult?.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE,
                    CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                    , 9101, "System error(fail to update admin's info)" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminService", "I", $"[LoginResetAdmin RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<bool> UpdAdmin(AdminViewModel objUpdAdmin)
        {
            SiteGlobal.WriteInformation("AdminService", "I", $"[UpdAdmin REQ] {JsonConvert.SerializeObject(objUpdAdmin)}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_TX_UPD");

                lo_objDB.AddInputParam("@pi_strAdminID",            SqlDbType.VarChar,  objUpdAdmin.AdminID, 50);
                lo_objDB.AddInputParam("@pi_strMobileNo",           SqlDbType.VarChar,  objUpdAdmin.MobileNo, 20);
                lo_objDB.AddInputParam("@pi_strAdminName",          SqlDbType.NVarChar, objUpdAdmin.AdminName, 50);
                lo_objDB.AddInputParam("@pi_intGradeCode",          SqlDbType.TinyInt,  objUpdAdmin.GradeCode, 0);
                lo_objDB.AddInputParam("@pi_strPrivateAvailFlag",   SqlDbType.Char,     objUpdAdmin.PrivateAvailFlag, 1);

                lo_objDB.AddInputParam("@pi_strDeptName",           SqlDbType.NVarChar, objUpdAdmin.DeptName,          100);
                lo_objDB.AddInputParam("@pi_strTelNo",              SqlDbType.VarChar,  objUpdAdmin.TelNo,              100);
                lo_objDB.AddInputParam("@pi_strEmail",              SqlDbType.NVarChar, objUpdAdmin.Email, 100);
                lo_objDB.AddInputParam("@pi_strExpireYMD",          SqlDbType.VarChar,  objUpdAdmin.ExpireYMD, 8);
                lo_objDB.AddInputParam("@pi_strBtoBYN",             SqlDbType.Char,     objUpdAdmin.BtoBYN,             1);

                lo_objDB.AddInputParam("@pi_strBtoCYN",             SqlDbType.Char,     objUpdAdmin.BtoCYN,             1);
                lo_objDB.AddInputParam("@pi_intAdminAreaType",      SqlDbType.TinyInt,  objUpdAdmin.AdminAreaType,      4);
                lo_objDB.AddInputParam("@pi_intUpsCsType",          SqlDbType.TinyInt,  objUpdAdmin.UpsCsType,          4);
                lo_objDB.AddInputParam("@pi_strAccessClientCode",   SqlDbType.VarChar,  objUpdAdmin.AccessClientCode, 512);
                lo_objDB.AddInputParam("@pi_strUseFlag",            SqlDbType.Char,     objUpdAdmin.UseFlag, 1);

                lo_objDB.AddInputParam("@pi_strUpdAdminID",         SqlDbType.VarChar,  objUpdAdmin.UpdAdminID, 50);
                lo_objDB.AddOutputParam("@po_strErrMsg",            SqlDbType.VarChar,  256);
                lo_objDB.AddOutputParam("@po_intRetVal",            SqlDbType.Int, 0);
                lo_objDB.AddOutputParam("@po_strDBErrMsg",          SqlDbType.VarChar,  256);
                lo_objDB.AddOutputParam("@po_intDBRetVal",          SqlDbType.Int, 0);

                lo_objDB.SetQuery();

                //DAS 통신 실패이면 오류로 리턴
                if (lo_objDB.LastErrorCode.IsFail())
                {
                    lo_objResult.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE, CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                                            , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //트랜잭션이없다.
                if (lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt().IsFail())
                {
                    lo_objResult.SetResult(lo_objDB.GetOutputParamValue("@po_intRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strErrMsg")
                                         , lo_objDB.GetOutputParamValue("@po_intDBRetVal").ToInt(), lo_objDB.GetOutputParamValue("@po_strDBErrMsg"));
                    return lo_objResult;
                }

                //Response 값 셋팅                
                lo_objResult.SetResult(CommonConstant.DB_SUCCESS_CODE);
            }
            catch (Exception ex)
            {
                lo_objResult.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE, CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                                     , 9101, "System error(fail to update admin's info)" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminService", "I", $"[UpdAdmin RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        /// <summary>
        /// 관리자 내정보 수정
        /// </summary>
        /// <param name="UpdAdminMyInfo"></param>
        /// <returns></returns>
        public ServiceResult<bool> UpdAdminMyInfo(AdminViewModel objUpdAdminMyInfo)
        {
            SiteGlobal.WriteInformation("AdminService", "I", $"[UpdAdminMyInfo REQ] {JsonConvert.SerializeObject(objUpdAdminMyInfo)}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDas = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDas = new ConnectGlobal();

                lo_objDas.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MYINFO_TX_UPD");

                lo_objDas.AddInputParam("@pi_strAdminID",   SqlDbType.VarChar,  objUpdAdminMyInfo.AdminID,  50);
                lo_objDas.AddInputParam("@pi_strTelNo",     SqlDbType.VarChar,  objUpdAdminMyInfo.TelNo,    20);
                lo_objDas.AddInputParam("@pi_strEmail",     SqlDbType.NVarChar, objUpdAdminMyInfo.Email,    100);
                lo_objDas.AddInputParam("@pi_strDeptName",  SqlDbType.NVarChar, objUpdAdminMyInfo.DeptName,    100);
                lo_objDas.AddInputParam("@pi_strUseFlag",   SqlDbType.Char,     objUpdAdminMyInfo.UseFlag,  1);

                lo_objDas.AddOutputParam("@po_strErrMsg",   SqlDbType.VarChar, 256);
                lo_objDas.AddOutputParam("@po_intRetVal",   SqlDbType.Int,     0);
                lo_objDas.AddOutputParam("@po_strDBErrMsg", SqlDbType.VarChar, 256);
                lo_objDas.AddOutputParam("@po_intDBRetVal", SqlDbType.Int,     0);

                lo_objDas.SetQuery();

                //DAS 통신 실패이면 오류로 리턴
                if (lo_objDas.LastErrorCode.IsFail())
                {
                    lo_objResult.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE, CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                                            , 0, $"[{lo_objDas.LastErrorCode}]{lo_objDas.LastErrorMessage}");
                    return lo_objResult;
                }

                //트랜잭션이없다.
                if (lo_objDas.GetOutputParamValue("@po_intRetVal").ToInt().IsFail())
                {
                    lo_objResult.SetResult(lo_objDas.GetOutputParamValue("@po_intRetVal").ToInt(), lo_objDas.GetOutputParamValue("@po_strErrMsg")
                                         , lo_objDas.GetOutputParamValue("@po_intDBRetVal").ToInt(), lo_objDas.GetOutputParamValue("@po_strDBErrMsg"));
                    return lo_objResult;
                }

                //Response 값 셋팅
                lo_objResult.SetResult(CommonConstant.DB_SUCCESS_CODE);
            }
            catch (Exception ex)
            {
                lo_objResult.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE, CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                                     , 9101, "System error(fail to update admin's myinfo)" + ex.Message);
            }
            finally
            {
                if (lo_objDas != null)
                {
                    lo_objDas.Close();
                    lo_objDas = null;
                }

                SiteGlobal.WriteInformation("AdminService", "I", $"[UpdAdminMyInfo RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }
    }
}