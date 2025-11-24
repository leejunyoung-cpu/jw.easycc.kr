using CommonLibrary.CommonModel;
using CommonLibrary.CommonModule;
using CommonLibrary.CommonUtils;
using CommonLibrary.Constants;
using CommonLibrary.DBModel;
using CommonLibrary.DBServices;
using CommonLibrary.Extensions;
using System;
using System.Web.UI.WebControls;

namespace SSO.Admin
{
    public partial class AdminIns : PageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            _pageAccessType = PageAccessType.ReadWrite;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetInitData();
            }
        }

        protected void GetInitData()
        {
            hidAdminID.Value = SiteGlobal.GetRequestForm("AdminID");

            UseFlag.Items.Clear();
            UseFlag.Items.Add(new ListItem("사용 여부", ""));
            UseFlag.Items.Add(new ListItem("승인", "Y"));
            UseFlag.Items.Add(new ListItem("사용중지", "N"));
            UseFlag.Items.Add(new ListItem("승인대기", "D"));

            PrivateAvailFlag.Items.Clear();
            PrivateAvailFlag.Items.Add(new ListItem("동의 여부", ""));
            PrivateAvailFlag.Items.Add(new ListItem("동의",    "Y"));
            PrivateAvailFlag.Items.Add(new ListItem("미동의",    "N"));

            AdminAreaType.Items.Clear();
            AdminAreaType.Items.Add(new ListItem("미지정", "99"));
            AdminAreaType.Items.Add(new ListItem("서울", "1"));
            AdminAreaType.Items.Add(new ListItem("인천", "2"));
            AdminAreaType.Items.Add(new ListItem("부산", "3"));

            UpsCsType.Items.Clear();
            UpsCsType.Items.Add(new ListItem("선택", ""));
            UpsCsType.Items.Add(new ListItem("내부담당자", "1"));
            UpsCsType.Items.Add(new ListItem("UPS담당자", "2"));

            CommonDDLB.ADMIN_GRADE_DDLB(GradeCode, objSes.GradeCode);

            if (!string.IsNullOrWhiteSpace(hidAdminID.Value))
            {
                hidMode.Value = "update";
                DisplayData();
            }
            else
            {
                hidMode.Value  = "insert";
                ExpireYMD.Text = Convert.ToDateTime(DateTime.Now.AddYears(10)).ToString("yyyy-MM-dd");
            }

            if (objSes.GradeCode >= 6)
            {
                ClientCodes.Style.Add("display", "none") ;
                ClientCode.Style.Add("display", "block");
            }
            else {
                ClientCodes.Style.Add("display", "block");
                ClientCode.Style.Add("display", "none");
            }
        }

        protected void DisplayData()
        {
            ReqAdminList                lo_objReqAdminList     = null;
            ServiceResult<ResAdminList> lo_objResAdminList     = null;
            AdminService                lo_objAdminService     = null;

            try
            {
                lo_objAdminService = new AdminService();

                lo_objReqAdminList = new ReqAdminList
                {
                    AdminID          = hidAdminID.Value,
                    SesGradeCode     = objSes.GradeCode,
                    AccessClientCode = objSes.AccessClientCode,
                    PageSize         = CommonConstant.PAGENAVIGATION_LIST.ToInt(),
                    PageNo           = 1
                };

                lo_objResAdminList = lo_objAdminService.GetAdminList(lo_objReqAdminList);
                if (lo_objResAdminList.result.ErrorCode.IsFail() || !lo_objResAdminList.data.RecordCnt.Equals(1))
                {
                    hidDisplayMode.Value = "Y";
                    hidErrMsg.Value = "관리자 정보를 조회하지 못했습니다.";
                    return;
                }

                AdminID.Text        = lo_objResAdminList.data.list[0].AdminID;
                AdminName.Text      = lo_objResAdminList.data.list[0].AdminName;

                if (!lo_objResAdminList.data.list[0].AdminID.Equals(""))
                {
                    hidAdminIDFlag.Value = "Y";
                }

                MobileNo.Text           = lo_objResAdminList.data.list[0].MobileNo;
                DeptName.Text           = lo_objResAdminList.data.list[0].DeptName;
                Email.Text              = lo_objResAdminList.data.list[0].Email;
                TelNo.Text              = lo_objResAdminList.data.list[0].TelNo;
                GradeCode.SelectedValue = lo_objResAdminList.data.list[0].GradeCode.ToString();

                if (!string.IsNullOrWhiteSpace(lo_objResAdminList.data.list[0].AccessClientCode)) {
                    if (!lo_objResAdminList.data.list[0].AccessClientCode.Split(',').Length.Equals(0))
                    {
                        for (var iFor = 0; iFor < lo_objResAdminList.data.list[0].AccessClientCode.Split(',').Length; iFor++)
                        {
                            for (var jFor = 0; jFor < ClientCodes.Items.Count; jFor++)
                            {
                                if (lo_objResAdminList.data.list[0].AccessClientCode.Split(',')[iFor].Equals(ClientCodes.Items[jFor].Value))
                                {
                                    ClientCodes.Items.FindByValue(lo_objResAdminList.data.list[0].AccessClientCode.Split(',')[iFor].ToString()).Selected = true;
                                }
                            }
                        }
                    }
                }
                
                if (GradeCode.SelectedValue.Equals("6") || GradeCode.SelectedValue.Equals("7"))
                {
                    ClientCode.SelectedValue = lo_objResAdminList.data.list[0].AccessClientCode;
                }

                UseFlag.SelectedValue          = lo_objResAdminList.data.list[0].UseFlag;
                ExpireYMD.Text                 = lo_objResAdminList.data.list[0].ExpireYMD;
                PrivateAvailFlag.SelectedValue = lo_objResAdminList.data.list[0].PrivateAvailFlag;
                BtoBYN.Checked                 = lo_objResAdminList.data.list[0].BtoBYN.Equals("Y") ? true : false;
                BtoCYN.Checked                 = lo_objResAdminList.data.list[0].BtoCYN.Equals("Y") ? true : false;
                AdminAreaType.SelectedValue    = lo_objResAdminList.data.list[0].AdminAreaType.ToString();
                UpsCsType.SelectedValue        = lo_objResAdminList.data.list[0].UpsCsType.ToString();
            }
            catch (Exception lo_ex)
            {
                SiteGlobal.WriteLog("Admin", "Exception",
                                    "\r\n\t[ex.Message] : " + lo_ex.Message + "\r\n\t[ex.StackTrace] : " +
                                    lo_ex.StackTrace, 9100);
            }
        }
    }
}