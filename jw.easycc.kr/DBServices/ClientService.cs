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
    public class ClientService
    {
        private bool bLogWrite = SiteGlobal.B_LOG_WRITE;
        public ServiceResult<ResClientList> GetClientList(ReqClientList objReqClientList) {
            SiteGlobal.WriteInformation("ClientService", "I", $"[GetClientList REQ] {JsonConvert.SerializeObject(objReqClientList)}", bLogWrite);

            string lo_strJson = string.Empty;
            ServiceResult<ResClientList> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try {
                lo_objResult = new ServiceResult<ResClientList>(999);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_CLIENT_AR_LST");
                lo_objDB.AddInputParam("@pi_intClientCode",     SqlDbType.Int,          objReqClientList.ClientCode,    4);
                lo_objDB.AddInputParam("@pi_strCorpNo",         SqlDbType.VarChar,      objReqClientList.CorpNo,        20);
                lo_objDB.AddInputParam("@pi_strClientName",     SqlDbType.NVarChar,     objReqClientList.ClientName,    50);
                lo_objDB.AddInputParam("@pi_strClientCeoName",  SqlDbType.NVarChar,     objReqClientList.ClientCeoName,    50);
                lo_objDB.AddInputParam("@pi_strAdminID",        SqlDbType.VarChar,      objReqClientList.AdminID,       50);

                lo_objDB.AddInputParam("@pi_strUseFlag",        SqlDbType.Char,         objReqClientList.UseFlag,       1);
                lo_objDB.AddInputParam("@pi_intPageSize",       SqlDbType.Int,          objReqClientList.PageSize, 4);
                lo_objDB.AddInputParam("@pi_intPageNo",         SqlDbType.Int,          objReqClientList.PageNo, 4);
                lo_objDB.AddOutputParam("@po_intRecordCnt",     SqlDbType.Int, 4);
                lo_objDB.SetQuery();

                if (!lo_objDB.LastErrorCode.Equals(0)) {
                    lo_objResult.SetResult(996, lo_objDB.LastErrorMessage
                                         , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //Response 값 셋팅                
                lo_objResult.SetResult(0);

                lo_objResult.data = new ResClientList
                {
                    list = new List<ClientViewModel>(),
                    RecordCnt = lo_objDB.GetOutputParamValue("@po_intRecordCnt").ToInt(),
                };

                if (lo_objDB.GetOutputParamValue("@po_intRecordCnt").ToInt() > 0) {
                    lo_strJson = JsonConvert.SerializeObject(lo_objDB.ExecuteDataTable());
                    lo_objResult.data.list = JsonConvert.DeserializeObject<List<ClientViewModel>>(lo_strJson);
                }
            }
            catch(Exception lo_ex) {
                lo_objResult.SetResult(997, "An unexpected error occurred during accessing data"
                                     , 9101, "System error(GetAdminList)" + lo_ex.Message);
            }
            finally{

                if (lo_objDB != null) {
                    lo_objDB.Close();
                    lo_objDB = null;
                }
                SiteGlobal.WriteInformation("ClientService", "I", $"[GetClientList RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }
            return lo_objResult;
        }

        public ServiceResult<bool> UpdClient(ClientViewModel objClientViewModel) 
        {
            SiteGlobal.WriteInformation("ClientService", "I", $"[UpdClient REQ] {JsonConvert.SerializeObject(objClientViewModel)}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_CLIENT_TX_UPD");

                lo_objDB.AddInputParam("@pi_intClientCode",     SqlDbType.Int,      objClientViewModel.ClientCode,      4);
                lo_objDB.AddInputParam("@pi_strSiteID",         SqlDbType.VarChar,  objClientViewModel.SiteID,          14);
                lo_objDB.AddInputParam("@pi_strClientName",     SqlDbType.NVarChar, objClientViewModel.ClientName,      50);
                lo_objDB.AddInputParam("@pi_strClientCeoName",  SqlDbType.NVarChar, objClientViewModel.ClientCeoName,   50);
                lo_objDB.AddInputParam("@pi_strEmail",          SqlDbType.NVarChar, objClientViewModel.Email,           100);

                lo_objDB.AddInputParam("@pi_strClientTel",      SqlDbType.VarChar,  objClientViewModel.ClientTel,       20);
                lo_objDB.AddInputParam("@pi_strAddr",           SqlDbType.NVarChar, objClientViewModel.Addr,            256);
                lo_objDB.AddInputParam("@pi_intClientType",     SqlDbType.TinyInt,  objClientViewModel.ClientType,      4);
                lo_objDB.AddInputParam("@pi_strEncomFlag",      SqlDbType.Char,     objClientViewModel.EncomFlag,       1);
                lo_objDB.AddInputParam("@pi_strBtoBYN",         SqlDbType.Char,     objClientViewModel.BtoBYN, 1);

                lo_objDB.AddInputParam("@pi_strBtoCYN",         SqlDbType.Char,     objClientViewModel.BtoCYN, 1);
                lo_objDB.AddInputParam("@pi_strClientLogoFile", SqlDbType.VarChar,  objClientViewModel.ClientLogoFile,  500);
                lo_objDB.AddInputParam("@pi_strClientServerIP", SqlDbType.VarChar,  objClientViewModel.ClientServerIP,  3);
                lo_objDB.AddInputParam("@pi_strUseFlag",        SqlDbType.Char,     objClientViewModel.UseFlag,         1);
                lo_objDB.AddInputParam("@pi_strUpdAdminID",     SqlDbType.VarChar,  objClientViewModel.UpdAdminID,      50);

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
                                     , 9101, "System error(fail to update admin's info)" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("ClientService", "I", $"[UpdClient RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<ClientViewModel> InsClient(ClientViewModel objClientViewModel)
        {
            SiteGlobal.WriteInformation("ClientService", "I", $"[InsClient REQ] {JsonConvert.SerializeObject(objClientViewModel)}", bLogWrite);

            ServiceResult<ClientViewModel> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<ClientViewModel>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_CLIENT_TX_INS");

                lo_objDB.AddInputParam("@pi_strSiteID",         SqlDbType.VarChar,  objClientViewModel.SiteID,          14);
                lo_objDB.AddInputParam("@pi_strClientName",     SqlDbType.NVarChar, objClientViewModel.ClientName,      50);
                lo_objDB.AddInputParam("@pi_strClientCeoName",  SqlDbType.NVarChar, objClientViewModel.ClientCeoName,   50);
                lo_objDB.AddInputParam("@pi_strCorpNo",         SqlDbType.VarChar,  objClientViewModel.CorpNo,          20);
                lo_objDB.AddInputParam("@pi_strEmail",          SqlDbType.NVarChar, objClientViewModel.Email,           100);

                lo_objDB.AddInputParam("@pi_strClientTel",      SqlDbType.VarChar,  objClientViewModel.ClientTel,       20);
                lo_objDB.AddInputParam("@pi_strAddr",           SqlDbType.NVarChar, objClientViewModel.Addr,            256);
                lo_objDB.AddInputParam("@pi_intClientType",     SqlDbType.TinyInt,  objClientViewModel.ClientType,      0);
                lo_objDB.AddInputParam("@pi_strBtoBYN",         SqlDbType.Char,     objClientViewModel.BtoBYN,          1);
                lo_objDB.AddInputParam("@pi_strBtoCYN",         SqlDbType.Char,     objClientViewModel.BtoCYN,          1);

                lo_objDB.AddInputParam("@pi_strClientLogoFile", SqlDbType.VarChar,  objClientViewModel.ClientLogoFile,  500);
                lo_objDB.AddInputParam("@pi_strUseFlag",        SqlDbType.Char,     objClientViewModel.UseFlag,         1);
                lo_objDB.AddInputParam("@pi_strRegAdminID",     SqlDbType.VarChar,  objClientViewModel.RegAdminID,      50);
                lo_objDB.AddOutputParam("@po_intClientCode",    SqlDbType.Int,      4);
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

                lo_objResult.data = new ClientViewModel
                {
                    ClientCode = lo_objDB.GetOutputParamValue("@po_intClientCode").ToInt()
                };
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

                SiteGlobal.WriteInformation("ClientService", "I", $"[InsClient RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }
    }
}