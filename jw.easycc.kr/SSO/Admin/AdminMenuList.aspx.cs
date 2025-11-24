using CommonLibrary.DBModel;
using CommonLibrary.CommonModule;
using CommonLibrary.CommonUtils;
using CommonLibrary.DBServices;
using CommonLibrary.Extensions;
using System;
using System.Data;
using System.Web.UI.WebControls;
using CommonLibrary.CommonModel;

namespace SSO.Admin
{
    public partial class AdminMenuList : PageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            _pageAccessType = PageAccessType.ReadOnly;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MenuGroupDDLB(DDLMenuGroup);
            }
            else
            {
                DisplayData();
            }           
        }

        protected void MenuGroupDDLB(DropDownList DDLID)
        {

            DDLID.Items.Clear();

            DDLID.Items.Add(new ListItem("메뉴 그룹명", ""));

            AdminMenuService                     lo_objAdminMenuService  = new AdminMenuService();
            ReqAdminMenuGroupList                lo_objReqAdminMenuGroupList = null;
            ServiceResult<ResAdminMenuGroupList> lo_objResAdminMenuGroupList = null;

            try
            {
                lo_objReqAdminMenuGroupList = new ReqAdminMenuGroupList
                {
                    MenuGroupNo = null
                };

                lo_objResAdminMenuGroupList = lo_objAdminMenuService.GetAdminMenuGroupInfo(lo_objReqAdminMenuGroupList);

                foreach (var item in lo_objResAdminMenuGroupList.data.list)
                {

                    DDLID.Items.Add(new ListItem("[" + item.MenuGroupKindM + "] " + item.MenuGroupName, item.MenuGroupNo.ToString()));
                }

            }
            catch (Exception lo_ex)
            {
                SiteGlobal.WriteLog("AdminMenuList", "Exception",
                    "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " + lo_ex.StackTrace,
                    9988);
            }
        }
        
        protected void DisplayData()
        {
            ReqAdminMenuList                lo_objReqAdminMenuList = null;
            ServiceResult<ResAdminMenuList> lo_objResAdminMenuList = null;
            AdminMenuService                lo_objAdminMenuService = null;
            DataTable                       lo_objDt = null;

            try
            {
                if (DDLMenuGroup.SelectedValue == "")
                {
                    return;
                }

                lo_objAdminMenuService = new AdminMenuService();

                lo_objReqAdminMenuList     = new ReqAdminMenuList
                {
                    MenuGroupNo = DDLMenuGroup.SelectedValue
                };

                lo_objResAdminMenuList = lo_objAdminMenuService.GetAdminMenuList(lo_objReqAdminMenuList);
                if (lo_objResAdminMenuList.result.ErrorCode.IsFail())
                {
                    hidDisplayMode.Value = "Y";
                    hidErrMsg.Value = "메뉴 정보를 조회하지 못했습니다.";
                    return;
                }

                RecordCnt.Value    = lo_objResAdminMenuList.data.RecordCnt.ToString();

                lo_objDt           = lo_objResAdminMenuList.data.list.GetConvertListToDataTable();
                repList.DataSource = lo_objDt;
                repList.DataBind();

            }
            catch (Exception lo_ex)
            {
                SiteGlobal.WriteLog("AdminMenuList", "Exception",
                                    "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " +
                                    lo_ex.StackTrace, 9100);
            }
        }

    }
}