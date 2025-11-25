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
    public class ApiService
    {
        private bool bLogWrite = SiteGlobal.B_LOG_WRITE;
        public ServiceResult<ResApiIpWhiteList> GetApiIpWhiteList(ReqApiIpWhiteList objReqApiIpWhiteList) {
            SiteGlobal.WriteInformation("ApiService", "I", $"[GetClientList REQ] {JsonConvert.SerializeObject(objReqApiIpWhiteList)}", bLogWrite);

            string lo_strJson = string.Empty;
            ServiceResult<ResApiIpWhiteList> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try {
                lo_objResult = new ServiceResult<ResApiIpWhiteList>(999);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_API_IP_WHITE_AR_LST");
                lo_objDB.AddInputParam("@pi_intSeqNo",          SqlDbType.Int,          objReqApiIpWhiteList.SeqNo,         0);
                lo_objDB.AddInputParam("@pi_strServiceCode",    SqlDbType.VarChar,      objReqApiIpWhiteList.ServiceCode,   50);
                lo_objDB.AddInputParam("@pi_strCompany",        SqlDbType.NVarChar,     objReqApiIpWhiteList.Company,       50);
                lo_objDB.AddInputParam("@pi_strIsActive",       SqlDbType.NVarChar,     objReqApiIpWhiteList.IsActive,      1);
                lo_objDB.AddInputParam("@pi_strServiceUrl",     SqlDbType.VarChar,      objReqApiIpWhiteList.ServiceUrl,    100);

                lo_objDB.AddInputParam("@pi_intPageSize",       SqlDbType.Int,          objReqApiIpWhiteList.PageSize,      4);
                lo_objDB.AddInputParam("@pi_intPageNo",         SqlDbType.Int,          objReqApiIpWhiteList.PageNo,        4);
                lo_objDB.AddOutputParam("@po_intRecordCnt",     SqlDbType.Int, 4);
                lo_objDB.SetQuery();

                if (!lo_objDB.LastErrorCode.Equals(0)) {
                    lo_objResult.SetResult(996, lo_objDB.LastErrorMessage
                                         , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //Response 값 셋팅                
                lo_objResult.SetResult(0);

                lo_objResult.data = new ResApiIpWhiteList
                {
                    List = new List<ApiIpWhiteViewModel>(),
                    RecordCnt = lo_objDB.GetOutputParamValue("@po_intRecordCnt").ToInt(),
                };

                if (lo_objDB.GetOutputParamValue("@po_intRecordCnt").ToInt() > 0) {
                    lo_strJson = JsonConvert.SerializeObject(lo_objDB.ExecuteDataTable());
                    lo_objResult.data.List = JsonConvert.DeserializeObject<List<ApiIpWhiteViewModel>>(lo_strJson);
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
                SiteGlobal.WriteInformation("ApiService", "I", $"[GetClientList RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }
            return lo_objResult;
        }

        public ServiceResult<bool> SetApiIpWhiteUpd(ApiIpWhiteViewModel objApiIpWhiteViewModel) 
        {
            SiteGlobal.WriteInformation("ApiService", "I", $"[SetApiIpWhiteUpd REQ] {JsonConvert.SerializeObject(objApiIpWhiteViewModel)}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_API_IP_WHITE_TX_UPD");

                lo_objDB.AddInputParam("@pi_intSeqNo",          SqlDbType.Int,      objApiIpWhiteViewModel.SeqNo,       0);
                lo_objDB.AddInputParam("@pi_strServiceCode",    SqlDbType.VarChar,  objApiIpWhiteViewModel.ServiceCode, 50);
                lo_objDB.AddInputParam("@pi_strCompany",        SqlDbType.VarChar,  objApiIpWhiteViewModel.Company,     50);
                lo_objDB.AddInputParam("@pi_strIpAddress",      SqlDbType.VarChar, objApiIpWhiteViewModel.IpAddress,    45);
                lo_objDB.AddInputParam("@pi_strIsActive",       SqlDbType.Char,     objApiIpWhiteViewModel.IsActive,    1);

                lo_objDB.AddInputParam("@pi_strServiceUrl",     SqlDbType.VarChar,  objApiIpWhiteViewModel.ServiceUrl,  100);
                lo_objDB.AddInputParam("@pi_strIpNote",         SqlDbType.VarChar,  objApiIpWhiteViewModel.IpNote,      100);
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
                                     , 9102, "System error(fail to update admin's info)" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("ApiService", "I", $"[SetApiIpWhiteUpd RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<ApiIpWhiteViewModel> SetApiIpWhiteIns(ApiIpWhiteViewModel objApiIpWhiteViewModel)
        {
            SiteGlobal.WriteInformation("ApiService", "I", $"[SetApiIpWhiteIns REQ] {JsonConvert.SerializeObject(objApiIpWhiteViewModel)}", bLogWrite);

            ServiceResult<ApiIpWhiteViewModel> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<ApiIpWhiteViewModel>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_API_IP_WHITE_TX_INS");

                lo_objDB.AddInputParam("@pi_strServiceCode",    SqlDbType.VarChar, objApiIpWhiteViewModel.ServiceCode, 50);
                lo_objDB.AddInputParam("@pi_strCompany",        SqlDbType.VarChar, objApiIpWhiteViewModel.Company,     50);
                lo_objDB.AddInputParam("@pi_strIpAddress",      SqlDbType.VarChar, objApiIpWhiteViewModel.IpAddress,   45);
                lo_objDB.AddInputParam("@pi_strServiceUrl",     SqlDbType.VarChar, objApiIpWhiteViewModel.ServiceUrl,  100);
                lo_objDB.AddInputParam("@pi_strIpNote",         SqlDbType.VarChar, objApiIpWhiteViewModel.IpNote,      100);

                lo_objDB.AddOutputParam("@po_intSeqNo",         SqlDbType.Int,      0);
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

                lo_objResult.data = new ApiIpWhiteViewModel
                {
                    SeqNo = lo_objDB.GetOutputParamValue("@po_intSeqNo").ToInt()
                };
            }
            catch (Exception ex)
            {
                lo_objResult.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE, CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                                     , 9103, "System error(fail to update admin's info)" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("ApiService", "I", $"[SetApiIpWhiteIns RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<bool> SetApiIpWhiteDel(ApiIpWhiteViewModel objApiIpWhiteViewModel)
        {
            SiteGlobal.WriteInformation("ApiService", "I", $"[SetApiIpWhiteDel REQ] {JsonConvert.SerializeObject(objApiIpWhiteViewModel)}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_API_IP_WHITE_TX_DEL");

                lo_objDB.AddInputParam("@pi_intSeqNo", SqlDbType.VarChar, objApiIpWhiteViewModel.SeqNo, 0);
                lo_objDB.AddOutputParam("@po_strErrMsg", SqlDbType.VarChar, 256);
                lo_objDB.AddOutputParam("@po_intRetVal", SqlDbType.Int, 0);
                lo_objDB.AddOutputParam("@po_strDBErrMsg", SqlDbType.VarChar, 256);
                lo_objDB.AddOutputParam("@po_intDBRetVal", SqlDbType.Int, 0);

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
                                     , 9104, "System error(fail to update admin's info)" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("ApiService", "I", $"[SetApiIpWhiteDel RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<ResApiHuaweiList> GetApiApiHuaweiList(ReqApiHuaweiList objReqApiHuaweiList)
        {
            SiteGlobal.WriteInformation("ApiService", "I", $"[GetApiApiHuaweiList REQ] {JsonConvert.SerializeObject(objReqApiHuaweiList)}", bLogWrite);

            string lo_strJson = string.Empty;
            ServiceResult<ResApiHuaweiList> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<ResApiHuaweiList>(999);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_API_HUAWEI_HEADER_AR_LST");
                lo_objDB.AddInputParam("@pi_strDateFrom",   SqlDbType.VarChar, objReqApiHuaweiList.DateFrom, 8);
                lo_objDB.AddInputParam("@pi_strDateTo",     SqlDbType.VarChar, objReqApiHuaweiList.DateTo, 8);
                lo_objDB.AddInputParam("@pi_intPageSize",   SqlDbType.Int, objReqApiHuaweiList.PageSize, 4);
                lo_objDB.AddInputParam("@pi_intPageNo",     SqlDbType.Int, objReqApiHuaweiList.PageNo, 4);
                lo_objDB.AddOutputParam("@po_intRecordCnt", SqlDbType.Int, 4);
                lo_objDB.SetQuery();

                if (!lo_objDB.LastErrorCode.Equals(0))
                {
                    lo_objResult.SetResult(996, lo_objDB.LastErrorMessage
                                         , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //Response 값 셋팅                
                lo_objResult.SetResult(0);

                lo_objResult.data = new ResApiHuaweiList
                {
                    List = new List<ApiHuaweiViewModel>(),
                    RecordCnt = lo_objDB.GetOutputParamValue("@po_intRecordCnt").ToInt(),
                };

                if (lo_objDB.GetOutputParamValue("@po_intRecordCnt").ToInt() > 0)
                {
                    lo_strJson = JsonConvert.SerializeObject(lo_objDB.ExecuteDataTable());
                    lo_objResult.data.List = JsonConvert.DeserializeObject<List<ApiHuaweiViewModel>>(lo_strJson);
                }
            }
            catch (Exception lo_ex)
            {
                lo_objResult.SetResult(997, "An unexpected error occurred during accessing data"
                                     , 9105, "System error(GetApiApiHuaweiList)" + lo_ex.Message);
            }
            finally
            {

                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }
                SiteGlobal.WriteInformation("ApiService", "I", $"[GetApiApiHuaweiList RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }
            return lo_objResult;
        }
    }
}