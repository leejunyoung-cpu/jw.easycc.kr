using CommonLibrary.CommonModel;
using CommonLibrary.DBServices;
using CommonLibrary.DBModel;
using System.Web.UI.WebControls;
using System;

namespace CommonLibrary.CommonModule
{
    public class CommonDDLB
    {
        public static void ADMIN_GRADE_DDLB(DropDownList DDLB, int GradeCode)
        {
            DDLB.Items.Clear();
            DDLB.Items.Add(new ListItem("회원등급", ""));
            switch (GradeCode)
            {
                case 1:
                    DDLB.Items.Add(new ListItem("슈퍼 관리자", "1"));
                    DDLB.Items.Add(new ListItem("내부관리자", "2"));
                    DDLB.Items.Add(new ListItem("최고관리자", "3"));
                    DDLB.Items.Add(new ListItem("관리자", "4"));
                    DDLB.Items.Add(new ListItem("담당자", "5"));
                    DDLB.Items.Add(new ListItem("고객사 관리자", "6"));
                    DDLB.Items.Add(new ListItem("고객사 담당자", "7"));
                    break;
                case 2:
                    DDLB.Items.Add(new ListItem("내부관리자", "2"));
                    DDLB.Items.Add(new ListItem("최고관리자", "3"));
                    DDLB.Items.Add(new ListItem("관리자", "4"));
                    DDLB.Items.Add(new ListItem("담당자", "5"));
                    DDLB.Items.Add(new ListItem("고객사 관리자", "6"));
                    DDLB.Items.Add(new ListItem("고객사 담당자", "7"));
                    break;
                case 3:
                    DDLB.Items.Add(new ListItem("최고관리자", "3"));
                    DDLB.Items.Add(new ListItem("관리자", "4"));
                    DDLB.Items.Add(new ListItem("담당자", "5"));
                    DDLB.Items.Add(new ListItem("고객사 관리자", "6"));
                    DDLB.Items.Add(new ListItem("고객사 담당자", "7"));
                    break;
                case 4:
                    DDLB.Items.Add(new ListItem("관리자", "4"));
                    DDLB.Items.Add(new ListItem("담당자", "5"));
                    DDLB.Items.Add(new ListItem("고객사 관리자", "6"));
                    DDLB.Items.Add(new ListItem("고객사 담당자", "7"));
                    break;
                case 5:
                    DDLB.Items.Add(new ListItem("담당자", "5"));
                    DDLB.Items.Add(new ListItem("고객사 관리자", "6"));
                    DDLB.Items.Add(new ListItem("고객사 담당자", "7"));
                    break;
                case 6:
                    DDLB.Items.Add(new ListItem("고객사 관리자", "6"));
                    DDLB.Items.Add(new ListItem("고객사 담당자", "7"));
                    break;
                case 7:
                    DDLB.Items.Add(new ListItem("고객사 담당자", "7"));
                    break;
            }
        }

        public static void USE_ADMIN_FLAG_DDLB(DropDownList DDLB)
        {
            DDLB.Items.Clear();
            DDLB.Items.Add(new ListItem("회원상태", ""));
            DDLB.Items.Add(new ListItem("승인", "Y"));
            DDLB.Items.Add(new ListItem("사용안함", "N"));
            DDLB.Items.Add(new ListItem("미승인", "D"));
        }

        public static void USE_FLAG_DDLB(DropDownList DDLB)
        {
            DDLB.Items.Clear();
            DDLB.Items.Add(new ListItem("사용 여부", ""));
            DDLB.Items.Add(new ListItem("사용", "Y"));
            DDLB.Items.Add(new ListItem("사용중지", "N"));
        }

        public static void DATE_CHOICE_DDLB(DropDownList DDLB)
        {
            DDLB.Items.Clear();
            DDLB.Items.Add(new ListItem("기간", ""));
            DDLB.Items.Add(new ListItem("금일", "1"));
            DDLB.Items.Add(new ListItem("익일", "2"));
            DDLB.Items.Add(new ListItem("전일", "3"));
            DDLB.Items.Add(new ListItem("금주", "6"));
            DDLB.Items.Add(new ListItem("금월", "4"));
            DDLB.Items.Add(new ListItem("전월", "5"));
        }

        public static void STATUS_DDLB(DropDownList DDLB)
        {
            DDLB.Items.Clear();
            DDLB.Items.Add(new ListItem("신고상태", ""));
            DDLB.Items.Add(new ListItem("요청", "1"));
            DDLB.Items.Add(new ListItem("접수", "2"));
            DDLB.Items.Add(new ListItem("완료", "2"));
        }

        public static void UPS_ADMIN_DDLB(DropDownList DDLB, int intUpsCsType = 1)
        {
            AdminService lo_objAdminService = new AdminService();

            ReqAdminList lo_objReqAdminList = null;
            ServiceResult<ResAdminList> lo_objResAdminList = null;

            lo_objReqAdminList = new ReqAdminList
            {
                UpsCsType = intUpsCsType,
                PageNo = 0,
                PageSize = 0
            };

            lo_objResAdminList = lo_objAdminService.GetAdminList(lo_objReqAdminList);

            if (lo_objResAdminList.data.RecordCnt > 1)
            {
                DDLB.Items.Add(new ListItem("담당자 선택", ""));
            }

            foreach (var item in lo_objResAdminList.data.list)
            {
                DDLB.Items.Add(new ListItem(item.AdminName, item.AdminID));
            }
        }
    }
}