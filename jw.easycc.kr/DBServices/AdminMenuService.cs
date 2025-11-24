using CommonLibrary.CommonModel;
using CommonLibrary.CommonModule;
using CommonLibrary.Constants;
using CommonLibrary.DBModel;
using CommonLibrary.Extensions;
using DbConCommon;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CommonLibrary.DBServices
{
    public class AdminMenuService
    {
        private bool bLogWrite = SiteGlobal.B_LOG_WRITE;

        /// <summary>
        /// 관리자메뉴 리스트
        /// </summary>
        public ServiceResult<ResAdminMenuList> GetAdminMenuList(ReqAdminMenuList objReqAdminMenuList)
        {
            SiteGlobal.WriteInformation("AdminMenuService", "I", $"[GetAdminMenuList REQ] {JsonConvert.SerializeObject(objReqAdminMenuList)}", bLogWrite);

            string lo_strJson = string.Empty;

            ServiceResult<ResAdminMenuList> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<ResAdminMenuList>(999);
                lo_objDB = new ConnectGlobal();
                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MENU_AR_LST");
                lo_objDB.AddInputParam("@pi_intMenuGroupNo",    SqlDbType.Int, objReqAdminMenuList.MenuGroupNo, 4);
                lo_objDB.AddInputParam("@pi_intMenuNo",         SqlDbType.Int, objReqAdminMenuList.MenuNo, 4);
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

                lo_objResult.data = new ResAdminMenuList
                {
                    list = new List<AdminMenuViewModel>(),
                    RecordCnt = lo_objDB.ExecuteDataTable().Rows.Count,
                };

                if (lo_objDB.ExecuteDataTable().Rows.Count > 0)
                {
                    lo_strJson = JsonConvert.SerializeObject(lo_objDB.ExecuteDataTable());
                    lo_objResult.data.list = JsonConvert.DeserializeObject<List<AdminMenuViewModel>>(lo_strJson);
                }
            }
            catch (Exception ex)
            {
                lo_objResult.SetResult(999, "An unexpected error occurred during accessing data"
                                     , 9101, "System error(GetAdminMenuList)" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminMenuService", "I", $"[GetAdminMenuList RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }
        public ServiceResult<ResAdminMenuGroupList> GetAdminMenuGroupInfo(ReqAdminMenuGroupList objReqAdminMenuGroupList)
        {
            SiteGlobal.WriteInformation("AdminMenuService", "I", $"[GetAdminMenuGroupInfo REQ] {JsonConvert.SerializeObject(objReqAdminMenuGroupList)}", bLogWrite);

            string lo_strJson = string.Empty;

            ServiceResult<ResAdminMenuGroupList> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<ResAdminMenuGroupList>(999);
                lo_objDB = new ConnectGlobal();
                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MENU_GROUP_AR_LST");
                lo_objDB.AddInputParam("@pi_intMenuGroupNo",    SqlDbType.Int,      objReqAdminMenuGroupList.MenuGroupNo, 4);
                lo_objDB.AddInputParam("@pi_strMenuGroupName",  SqlDbType.NVarChar, objReqAdminMenuGroupList.MenuGroupName, 50);
                lo_objDB.AddInputParam("@pi_strUseFlag",        SqlDbType.Char,     objReqAdminMenuGroupList.UseFlag, 1);
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

                lo_objResult.data = new ResAdminMenuGroupList
                {
                    list = new List<AdminMenuGroupViewModel>(),
                    RecordCnt = lo_objDB.ExecuteDataTable().Rows.Count,
                };

                if (lo_objDB.ExecuteDataTable().Rows.Count > 0)
                {
                    lo_strJson = JsonConvert.SerializeObject(lo_objDB.ExecuteDataTable());
                    lo_objResult.data.list = JsonConvert.DeserializeObject<List<AdminMenuGroupViewModel>>(lo_strJson);
                }
            }
            catch (Exception ex)
            {
                lo_objResult.SetResult(999, "An unexpected error occurred during accessing data"
                                     , 9102, "System error(GetAdminMenuGroupInfo)" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminMenuService", "I", $"[GetAdminMenuGroupInfo RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<bool> InsAdminMenu(AdminMenuViewModel objInsAdminMenu)
        {
            SiteGlobal.WriteInformation("AdminMenuService", "I", $"[InsAdminMenu REQ] {JsonConvert.SerializeObject(objInsAdminMenu)}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();
                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MENU_TX_INS");
                lo_objDB.AddInputParam("@pi_intMenuGroupNo",  SqlDbType.Int,      objInsAdminMenu.MenuGroupNo,  0);
                lo_objDB.AddInputParam("@pi_strMenuName",     SqlDbType.NVarChar, objInsAdminMenu.MenuName,     50);
                lo_objDB.AddInputParam("@pi_strMenuLink",     SqlDbType.VarChar,  objInsAdminMenu.MenuLink,     100);
                lo_objDB.AddInputParam("@pi_strMenuDesc",     SqlDbType.NVarChar, objInsAdminMenu.MenuDesc,     1000);
                lo_objDB.AddInputParam("@pi_intUseStateCode", SqlDbType.TinyInt,  objInsAdminMenu.UseStateCode, 0);

                lo_objDB.AddOutputParam("@po_strErrMsg",       SqlDbType.VarChar,   256);
                lo_objDB.AddOutputParam("@po_intRetVal",       SqlDbType.Int,       0);
                lo_objDB.AddOutputParam("@po_strDBErrMsg",     SqlDbType.VarChar,   256);
                lo_objDB.AddOutputParam("@po_intDBRetVal",     SqlDbType.Int,       0);

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
                                     , 9101, "System error(fail to insert adminmenu's menugroup" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminMenuService", "I", $"[InsAdminMenu RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        /// <summary>
        /// 관리자메뉴 수정
        /// </summary>
        public ServiceResult<bool> UpdAdminMenu(AdminMenuViewModel objUpdAdminMenu)
        {
            SiteGlobal.WriteInformation("AdminMenuServices", "I", $"[UpdAdminMenu REQ] {JsonConvert.SerializeObject(objUpdAdminMenu)}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();
                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MENU_TX_UPD");
                lo_objDB.AddInputParam("@pi_intMenuNo",     SqlDbType.Int,          objUpdAdminMenu.MenuNo,     0);
                lo_objDB.AddInputParam("@pi_strMenuName",   SqlDbType.NVarChar,     objUpdAdminMenu.MenuName,   50);
                lo_objDB.AddInputParam("@pi_strMenuLink",   SqlDbType.VarChar,      objUpdAdminMenu.MenuLink,   100);
                lo_objDB.AddInputParam("@pi_intMenuSort",   SqlDbType.SmallInt,     objUpdAdminMenu.MenuSort,   0);
                lo_objDB.AddInputParam("@pi_strMenuDesc",   SqlDbType.NVarChar,     objUpdAdminMenu.MenuDesc,   1000);

                lo_objDB.AddInputParam("@pi_intUseStateCode",   SqlDbType.TinyInt,  objUpdAdminMenu.UseStateCode, 0);
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
                                     , 9101, "System error(fail to update adminmenu's menugroup" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminMenuServices", "I", $"[UpdAdminMenu RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<bool> DelAdminMenu(string strMenuNo)
        {
            SiteGlobal.WriteInformation("AdminMenuServices", "I", $"[DelAdminMenu REQ] {strMenuNo}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDB = null;
            try
            {
                lo_objResult = new ServiceResult<bool>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MENU_TX_DEL");

                lo_objDB.AddInputParam("@pi_intMenuNo",     SqlDbType.Int,      strMenuNo, 4);
                lo_objDB.AddOutputParam("@po_strErrMsg",    SqlDbType.VarChar,  256);
                lo_objDB.AddOutputParam("@po_intRetVal",    SqlDbType.Int,      0);
                lo_objDB.AddOutputParam("@po_strDBErrMsg",  SqlDbType.VarChar,  256);
                lo_objDB.AddOutputParam("@po_intDBRetVal",  SqlDbType.Int,      0);

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
                                     , 9101, "System error(fail to delete adminmenu's menu" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminMenuServices", "I", $"[DelAdminMenu RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<bool> UpAdminMenuGroup(ReqAdminMenuGroupList objReqUpAdminMenuGroup)
        {
            SiteGlobal.WriteInformation("AdminMenuSerivces", "I", $"[UpAdminMenuGroup REQ] {JsonConvert.SerializeObject(objReqUpAdminMenuGroup)}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MENU_GROUP_TX_UPD");

                lo_objDB.AddInputParam("@pi_intMenuGroupNo",   SqlDbType.Int,      objReqUpAdminMenuGroup.MenuGroupNo, 0);
                lo_objDB.AddInputParam("@pi_intMenuGroupKind", SqlDbType.TinyInt,  objReqUpAdminMenuGroup.MenuGroupKind, 0);
                lo_objDB.AddInputParam("@pi_strMenuGroupName", SqlDbType.NVarChar, objReqUpAdminMenuGroup.MenuGroupName, 50);
                lo_objDB.AddInputParam("@pi_intMenuGroupSort", SqlDbType.SmallInt, objReqUpAdminMenuGroup.MenuGroupSort, 0);
                lo_objDB.AddInputParam("@pi_strDisplayFlag",   SqlDbType.Char,     objReqUpAdminMenuGroup.DisplayFlag, 1);

                lo_objDB.AddInputParam("@pi_strUseFlag",       SqlDbType.Char,     objReqUpAdminMenuGroup.UseFlag, 1);
                lo_objDB.AddOutputParam("@po_strErrMsg",       SqlDbType.VarChar,  256);
                lo_objDB.AddOutputParam("@po_intRetVal",       SqlDbType.Int,      0);
                lo_objDB.AddOutputParam("@po_strDBErrMsg",     SqlDbType.VarChar,  256);
                lo_objDB.AddOutputParam("@po_intDBRetVal",     SqlDbType.Int,      0);

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
                                     , 9101, "System error(fail to update adminmenu's menugroup" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminMenuSerivces", "I", $"[DelAdminMenuGroup RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<bool> DelAdminMenuGroup(string strMenuGroupNo)
        {
            SiteGlobal.WriteInformation("AdminMenuSerivces", "I", $"[DelAdminMenuGroup REQ] {JsonConvert.SerializeObject(strMenuGroupNo)}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MENU_GROUP_TX_DEL");

                lo_objDB.AddInputParam("@pi_intMenuGroupNo",   SqlDbType.Int,      strMenuGroupNo, 0);
                lo_objDB.AddOutputParam("@po_strErrMsg",       SqlDbType.VarChar,  256);
                lo_objDB.AddOutputParam("@po_intRetVal",       SqlDbType.Int,      0);
                lo_objDB.AddOutputParam("@po_strDBErrMsg",     SqlDbType.VarChar,  256);
                lo_objDB.AddOutputParam("@po_intDBRetVal",     SqlDbType.Int,      0);

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
                                     , 9101, "System error(fail to delete adminmenu's menugroup" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminMenuSerivces", "I", $"[DelAdminMenuGroup RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        //메뉴역할관리리스트
        public ServiceResult<ResAdminMenuRoleList> GetAdminMenuRoleList(ReqAdminMenuRoleList objReqAdminMenuRoleList)
        {
            SiteGlobal.WriteInformation("AdminMenuSerivces", "I", $"[GetAdminMenuRoleList REQ] {JsonConvert.SerializeObject(objReqAdminMenuRoleList)}", bLogWrite);

            string lo_strJson = string.Empty;

            ServiceResult<ResAdminMenuRoleList> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<ResAdminMenuRoleList>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MENU_ROLE_AR_LST");

                lo_objDB.AddInputParam("@pi_intMenuRoleNo", SqlDbType.Int,     objReqAdminMenuRoleList.MenuRoleNo, 4);
                lo_objDB.AddInputParam("@pi_strUseFlag",    SqlDbType.Char,    DBNull.Value, 1);

                lo_objDB.SetQuery();

                //DAS 통신 실패이면 오류로 리턴
                if (lo_objDB.LastErrorCode.IsFail())
                {
                    lo_objResult.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE, CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                                         , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //Response 값 셋팅
                lo_objResult.SetResult(CommonConstant.DB_SUCCESS_CODE);

                lo_objResult.data = new ResAdminMenuRoleList
                {
                    list = new List<AdminMenuRoleViewModel>(),
                    RecordCnt = lo_objDB.ExecuteDataTable().Rows.Count
                };

                if (lo_objDB.ExecuteDataTable().Rows.Count > 0)
                {
                    lo_strJson = JsonConvert.SerializeObject(lo_objDB.ExecuteDataTable());
                    lo_objResult.data.list = JsonConvert.DeserializeObject<List<AdminMenuRoleViewModel>>(lo_strJson);
                }
            }
            catch (Exception ex)
            {
                lo_objResult.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE, CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                                     , 9101, "System error(GetAdminMenuRoleList)" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminMenuSerivces", "I", $"[GetAdminMenuRoleList RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<bool> DelAdminMenuRole(AdminMenuRoleViewModel objDelAdminMenuRole)
        {
            SiteGlobal.WriteInformation("AdminMenuSerivces", "I", $"[DelAdminMenuRole REQ] {JsonConvert.SerializeObject(objDelAdminMenuRole)}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MENU_ROLE_TX_DEL");

                lo_objDB.AddInputParam("@pi_intMenuRoleNo",    SqlDbType.Int,      objDelAdminMenuRole.MenuRoleNo, 4);
                lo_objDB.AddOutputParam("@po_strErrMsg",       SqlDbType.VarChar,  256);
                lo_objDB.AddOutputParam("@po_intRetVal",       SqlDbType.Int,      4);
                lo_objDB.AddOutputParam("@po_strDBErrMsg",     SqlDbType.VarChar,  256);
                lo_objDB.AddOutputParam("@po_intDBRetVal",     SqlDbType.Int,      4);

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
                                     , 9101, "System error(fail to update adminmenu's menugroup" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminMenuSerivces", "I", $"[UpdAdminMenu RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<bool> InsRole(string strMenuRoleName, string strMenuList, string strAuthCode, string strRwAuthCode, string strRoAuthCode, string strUseFlag)
        {
            SiteGlobal.WriteInformation("AdminMenuSerivces", "I", $"[InsRole REQ] {strMenuRoleName}/{strMenuList}/{strAuthCode}/{strRwAuthCode}/{strRoAuthCode}/{strUseFlag}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult    = new ServiceResult<bool>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB       = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MENU_ROLE_TX_INS");

                lo_objDB.AddInputParam("@pi_strMenuRoleName", SqlDbType.NVarChar,  strMenuRoleName,    50);
                lo_objDB.AddInputParam("@pi_strMenuList",     SqlDbType.VarChar,   strMenuList,        2000);
                lo_objDB.AddInputParam("@pi_strAllAuthCode",  SqlDbType.VarChar,   strAuthCode,        2000);
                lo_objDB.AddInputParam("@pi_strRwAuthCode",   SqlDbType.VarChar,   strRwAuthCode,      2000);
                lo_objDB.AddInputParam("@pi_strRoAuthCode",   SqlDbType.VarChar,   strRoAuthCode,      2000);

                lo_objDB.AddInputParam("@pi_strUseFlag",      SqlDbType.Char,      strUseFlag,         1);
                lo_objDB.AddOutputParam("@po_strErrMsg",      SqlDbType.VarChar,   256);
                lo_objDB.AddOutputParam("@po_intRetVal",      SqlDbType.Int,       0);
                lo_objDB.AddOutputParam("@po_strDBErrMsg",    SqlDbType.VarChar,   256);
                lo_objDB.AddOutputParam("@po_intDBRetVal",    SqlDbType.Int,       0);

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
                                     , 9101, "System error(fail to insert adminmenu's role" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminMenuSerivces", "I", $"[InsRole RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<bool> UpdRole(string strMenuRoleNo, string strMenuRoleName, string strAddMenuList, string strRmMenuList, string strAllAuthCode, string strRoAuthCode, string strRwAuthCode, string strUseFlag)
        {
            SiteGlobal.WriteInformation("AdminMenuSerivces", "I", $"[UpdRole REQ] {strMenuRoleNo}/{strMenuRoleName}/{strAddMenuList}/{strRmMenuList}/{strAllAuthCode}/{strRoAuthCode}/{strRwAuthCode}/{strUseFlag}", bLogWrite);

            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal       lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MENU_ROLE_TX_UPD");

                lo_objDB.AddInputParam("@pi_intMenuRoleNo",     SqlDbType.Int,      strMenuRoleNo,      0);
                lo_objDB.AddInputParam("@pi_strMenuRoleName",   SqlDbType.NVarChar, strMenuRoleName,    50);
                lo_objDB.AddInputParam("@pi_strAddMenuList",    SqlDbType.VarChar,  strAddMenuList,     2000);
                lo_objDB.AddInputParam("@pi_strRmMenuList",     SqlDbType.VarChar,  strRmMenuList,      2000);
                lo_objDB.AddInputParam("@pi_strAllAuthCode",    SqlDbType.VarChar,  strAllAuthCode,     2000);

                lo_objDB.AddInputParam("@pi_strRwAuthCode",     SqlDbType.VarChar,  strRwAuthCode,      2000);
                lo_objDB.AddInputParam("@pi_strRoAuthCode",     SqlDbType.VarChar,  strRoAuthCode,      2000);
                lo_objDB.AddInputParam("@pi_strUseFlag",        SqlDbType.Char,     strUseFlag,         1);
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
                                     , 9101, "System error(fail to update adminmenu's role" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminMenuSerivces", "I", $"[UpdRole RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<ResAdminMenuRoleDtlList> GetAdminMenuRoleDtlList(ReqAdminMenuRoleDtlList objReqAdminMenuRoleDtlList)
        {
            SiteGlobal.WriteInformation("AdminMenuSerivces", "I", $"[GetAdminMenuRoleDtlList REQ] {JsonConvert.SerializeObject(objReqAdminMenuRoleDtlList)}", bLogWrite);

            string lo_strJson = string.Empty;

            ServiceResult<ResAdminMenuRoleDtlList> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<ResAdminMenuRoleDtlList>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();
                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MENU_ROLE_DTL_AR_LST");

                lo_objDB.AddInputParam("@pi_intMenuRoleNo", SqlDbType.Int, objReqAdminMenuRoleDtlList.MenuRoleNo, 4);

                lo_objDB.SetQuery();

                //DAS 통신 실패이면 오류로 리턴
                if (lo_objDB.LastErrorCode.IsFail())
                {
                    lo_objResult.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE, CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                                         , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //Response 값 셋팅
                lo_objResult.SetResult(CommonConstant.DB_SUCCESS_CODE);

                lo_objResult.data = new ResAdminMenuRoleDtlList
                {
                    list = new List<AdminMenuRoleDtlViewModel>(),
                    RecordCnt = lo_objDB.ExecuteDataTable().Rows.Count
                };

                if (lo_objDB.ExecuteDataTable().Rows.Count > 0)
                {
                    lo_strJson = JsonConvert.SerializeObject(lo_objDB.ExecuteDataTable());
                    lo_objResult.data.list = JsonConvert.DeserializeObject<List<AdminMenuRoleDtlViewModel>>(lo_strJson);
                }

            }
            catch (Exception ex)
            {
                lo_objResult.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE, CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                                     , 9101, "System error(GetAdminMenuRoleDtlList)" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminMenuSerivces", "I", $"[GetAdminMenuRoleDtlList RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        /// <summary>
        /// 관리자 메뉴 접근 리스트
        /// </summary>
        /// <param name="strAdminId"></param>
        /// <returns></returns>
        public ServiceResult<ResAdminMenuAccessList> GetAdminMenuAccessList(string strAdminId)
        {
            SiteGlobal.WriteInformation("AdminMenuService", "I", $"[GetAdminMenuAccessList REQ] {strAdminId}", bLogWrite);

            string lo_strJson = string.Empty;
            ServiceResult<ResAdminMenuAccessList> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<ResAdminMenuAccessList>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MENU_ACCESS_AR_LST");

                lo_objDB.AddInputParam("@pi_strAdminID", SqlDbType.VarChar, strAdminId, 0);

                lo_objDB.SetQuery();

                //DAS 통신 실패이면 오류로 리턴
                if (lo_objDB.LastErrorCode.IsFail())
                {
                    lo_objResult.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE, CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                                         , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //Response 값 셋팅
                lo_objResult.SetResult(CommonConstant.DB_SUCCESS_CODE);

                lo_objResult.data = new ResAdminMenuAccessList
                {
                    list = new List<AdminMenuAccessViewModel>(),
                    RecordCnt = lo_objDB.ExecuteDataTable().Rows.Count
                };

                if (lo_objDB.ExecuteDataTable().Rows.Count > 0)
                {
                    lo_strJson = JsonConvert.SerializeObject(lo_objDB.ExecuteDataTable().Rows[0].Table);
                    lo_objResult.data.list = JsonConvert.DeserializeObject<List<AdminMenuAccessViewModel>>(lo_strJson);
                }
            }
            catch (Exception ex)
            {
                lo_objResult?.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE,
                    CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                    , 9101, "System error(GetAdminMenuAccessList)" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminMenuService", "I", $"[GetAdminMenuAccessList RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        /// <summary>
        /// 관리자 메뉴 접근 메뉴 역할 리스트
        /// </summary>
        /// <param name="strAdminId"></param>
        /// <returns></returns>
        public ServiceResult<ResAdminMenuRoleAccessList> GetAdminMenuRoleAccessList(string strAdminId)
        {
            SiteGlobal.WriteInformation("AdminMenuService", "I", $"[GetAdminMenuRoleAccessList REQ] {strAdminId}", bLogWrite);

            string lo_strJson = string.Empty;
            ServiceResult<ResAdminMenuRoleAccessList> lo_objResult = null;
            ConnectGlobal lo_objDB = null;

            try
            {
                lo_objResult = new ServiceResult<ResAdminMenuRoleAccessList>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDB = new ConnectGlobal();

                lo_objDB.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MENU_ROLE_ACCESS_AR_LST");

                lo_objDB.AddInputParam("@pi_strAdminID", SqlDbType.VarChar, strAdminId, 0);

                lo_objDB.SetQuery();

                //DAS 통신 실패이면 오류로 리턴
                if (lo_objDB.LastErrorCode.IsFail())
                {
                    lo_objResult.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE, CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                                         , 0, $"[{lo_objDB.LastErrorCode}]{lo_objDB.LastErrorMessage}");
                    return lo_objResult;
                }

                //Response 값 셋팅
                lo_objResult.SetResult(CommonConstant.DB_SUCCESS_CODE);

                lo_objResult.data = new ResAdminMenuRoleAccessList
                {
                    list = new List<AdminMenuRoleAccessViewModel>(),
                    RecordCnt = lo_objDB.ExecuteDataTable().Rows.Count
                };

                if (lo_objDB.ExecuteDataTable().Rows.Count > 0)
                {
                    lo_strJson = JsonConvert.SerializeObject(lo_objDB.ExecuteDataTable().Rows[0].Table);
                    lo_objResult.data.list = JsonConvert.DeserializeObject<List<AdminMenuRoleAccessViewModel>>(lo_strJson);
                }
            }
            catch (Exception ex)
            {
                lo_objResult?.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE,
                    CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                    , 9101, "System error(GetAdminMenuRoleAccessList)" + ex.Message);
            }
            finally
            {
                if (lo_objDB != null)
                {
                    lo_objDB.Close();
                    lo_objDB = null;
                }

                SiteGlobal.WriteInformation("AdminMenuService", "I", $"[GetAdminMenuRoleAccessList RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }

        public ServiceResult<bool> InsMenuAccessAdmin(AdminMenuAccessViewModel objAdminMenuAccessViewModel)
        {
            SiteGlobal.WriteInformation("AdminMenuService", "I", $"[InsMenuAccessAdmin REQ] {JsonConvert.SerializeObject(objAdminMenuAccessViewModel)}", bLogWrite);


            ServiceResult<bool> lo_objResult = null;
            ConnectGlobal lo_objDas = null;

            try
            {
                lo_objResult = new ServiceResult<bool>(CommonConstant.DB_RET_VAL_CODE);
                lo_objDas = new ConnectGlobal();

                lo_objDas.Open(SiteGlobal.ConnectionString, "UP_ADMIN_MENU_ACCESS_TX_INS");

                lo_objDas.AddInputParam("@pi_strAdminID",           SqlDbType.VarChar, objAdminMenuAccessViewModel.AdminID, 50);
                lo_objDas.AddInputParam("@pi_intAccessTypeCode",    SqlDbType.TinyInt, objAdminMenuAccessViewModel.AccessTypeCode, 0);
                lo_objDas.AddInputParam("@pi_strAddMenuList",       SqlDbType.VarChar, objAdminMenuAccessViewModel.AddMenuList, 4000);
                lo_objDas.AddInputParam("@pi_strRmMenuList",        SqlDbType.VarChar, objAdminMenuAccessViewModel.RmMenuList, 4000);
                lo_objDas.AddInputParam("@pi_strAllAuthCode",       SqlDbType.VarChar, objAdminMenuAccessViewModel.AllAuthCode, 4000);

                lo_objDas.AddInputParam("@pi_strRwAuthCode",        SqlDbType.VarChar, objAdminMenuAccessViewModel.RwAuthCode, 4000);
                lo_objDas.AddInputParam("@pi_strRoAuthCode",        SqlDbType.VarChar, objAdminMenuAccessViewModel.RoAuthCode, 4000);
                lo_objDas.AddOutputParam("@po_strErrMsg",           SqlDbType.VarChar, 256);
                lo_objDas.AddOutputParam("@po_intRetVal",           SqlDbType.Int,     0);
                lo_objDas.AddOutputParam("@po_strDBErrMsg",         SqlDbType.VarChar, 256);

                lo_objDas.AddOutputParam("@po_intDBRetVal",         SqlDbType.Int, 0);

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
                lo_objResult?.SetResult(CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_CODE,
                    CommonConstant.DB_DATA_ACCESS_SERVICE_ERR_MESSAGE
                    , 9101, "System error(InsMenuAccessAdmin)" + ex.Message);
            }
            finally
            {
                if (lo_objDas != null)
                {
                    lo_objDas.Close();
                    lo_objDas = null;
                }

                SiteGlobal.WriteInformation("AdminMenuService", "I", $"[InsMenuAccessAdmin RES] {JsonConvert.SerializeObject(lo_objResult)}", bLogWrite);
            }

            return lo_objResult;
        }
    }
}