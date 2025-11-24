using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommonLibrary.DBModel
{
    public class ReqAdminList
    {
        public string   AdminID      { get; set; }
        public string   AdminName    { get; set; }
        public string   MobileNo    { get; set; }
        public int      ClientCode   { get; set; }
        public int      GradeCode    { get; set; }
        public string   UseFlag             { get; set; }
        public string   AccessClientCode    { get; set; }
        public int      SesGradeCode { get; set; }
        public int      UpsCsType { get; set; }
        public int      PageSize     { get; set; }
        public int      PageNo       { get; set; }
    }

    public class ResAdminList
    {
        public List<AdminViewModel> list { get; set; }
        public int RecordCnt { get; set; }
    }

    public class AdminViewModel
    {
        public string   AdminID { get; set; }
        public string   AdminPWD { get; set; }
        public string   AdminName { get; set; }
        public string   MobileNo { get; set; }
        public string   AccessClientCode { get; set; }
        public string   ClientName { get; set; }
        public int      GradeCode { get; set; }
        public string   GradeName { get; set; }
        public string DeptName { get; set; }
        public string TelNo  { get; set; }
        public string Email  { get; set; }
        public string ExpireYMD { get; set; }
        public string BtoBYN    { get; set; }
        public string BtoCYN    { get; set; }
        public int    AdminAreaType { get; set; }
        public string AdminAreaTypeM { get; set; }
        public string PrivateAvailFlag { get; set; }
        public string PwdUpdDate { get; set; }
        public int?    UpsCsType { get; set; }
        public string UseFlag { get; set; }
        public string UseFlagM { get; set; }
        public string RegAdminID { get; set; }
        public string RegDate { get; set; }
        public string UpdAdminID { get; set; }
        public string UpdDate { get; set; }
        public string ExitDate { get; set; }
    }

    public class ResAdminMenuAccessList
    {
        public List<AdminMenuAccessViewModel> list { get; set; }
        public int RecordCnt { get; set; }
    }

    public class AdminMenuAccessViewModel
    {
        public int MenuGroupNo { get; set; }
        public int MenuNo { get; set; }
        public string MenuName { get; set; }
        public int Depth { get; set; }
        public int SortNo { get; set; }
        public int PSortNo { get; set; }
        public int UseStateCode { get; set; }
        public int AuthCode { get; set; }
        public string AdminID { get; set; }
        public int AccessTypeCode { get; set; }
        public string AddMenuList { get; set; }
        public string RmMenuList { get; set; }
        public string AllAuthCode { get; set; }
        public string RwAuthCode { get; set; }
        public string RoAuthCode { get; set; }

    }

    public class ResAdminMenuRoleAccessList
    {
        public List<AdminMenuRoleAccessViewModel> list { get; set; }
        public int RecordCnt { get; set; }
    }

    public class AdminMenuRoleAccessViewModel
    {
        public int MenuRoleNo { get; set; }
        public string MenuRoleName { get; set; }
        public string UseFlag { get; set; }
        public string RegDate { get; set; }
        public string UpdDate { get; set; }
    }

    public class AdminSessionInfo
    {
        public string   AdminID             { get; set; } = string.Empty;
        public string   AdminName           { get; set; } = string.Empty;
        public string   MobileNo            { get; set; } = string.Empty;
        public int      ClientCode          { get; set; }
        public string   AccessClientCode    { get; set; } = string.Empty;
        public string   SiteID        { get; set; } = string.Empty;
        public int      GradeCode     { get; set; }
        public string   GradeName     { get; set; } = string.Empty;
        public string   PrivateAvailFlag      { get; set; } = string.Empty;
        public string   LastLoginDate         { get; set; } = string.Empty;
        public string   LastLoginIP           { get; set; } = string.Empty;
        public string   PwdUpdDate            { get; set; } = string.Empty;
        public string   SessionKey            { get; set; } = string.Empty;
        public int      AuthCode              { get; set; } = 99;
        public string   Email                 { get; set; } = string.Empty;
        public string   ExpireYmd             { get; set; } = string.Empty;
    }

    public class AdminPwdInfo
    {
        public string CurrPassword      { get; set; }
        public int GradeCode            { get; set; }
        public int TodayLoginFailCnt    { get; set; }
        public int AdminLogInTryCnt     { get; set; }
        public string UseFlag           { get; set; }
    }

    public class AdminPrevPwdInfo
    {
        public string[] arrPrevPassword { get; set; }
    }

    public class ResAdminLeftMenuAllList
    {
        public List<AdminLeftMenuAllList> list { get; set; }
        public int RecordCnt { get; set; }
    }

    public class AdminLeftMenuAllList
    {
        public int MenuGroupNo { get; set; }
        public int MenuGroupKind { get; set; }
        public int MenuGroupSort { get; set; }
        public string MenuGroupName { get; set; }
        public string MenuName { get; set; }
        public string MenuDesc { get; set; }
        public string MenuLink { get; set; }
        public int MenuSort { get; set; }
    }

    public class AdminMenuChk
    {
        public int AuthCode { get; set; }
    }
}