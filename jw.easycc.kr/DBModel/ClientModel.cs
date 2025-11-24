using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommonLibrary.DBModel
{
    public class ReqClientList
    {
        public int      ClientCode      { get; set; }               // 고객사코드
        public string   AdminID         { get; set; }               // 사용자 아이디
        public string   SiteID          { get; set; }               // 거래처코드
        public string   ClientName      { get; set; }               // 고객사명
        public string   ClientCeoName   { get; set; }               // 고객사명
        public string   CorpNo          { get; set; }               // 사업자번호
        public string   UseFlag         { get; set; }                  
        public int      PageSize        { get; set; }
        public int      PageNo          { get; set; }
    }

    public class ResClientList
    {
        public List<ClientViewModel> list { get; set; }
        public int RecordCnt { get; set; }
    }

    public class ClientViewModel
    {
        public int ClientCode       { get; set; }   // 고객사코드
        public string SiteID        { get; set; }   // 거래처코드
        public string ClientName    { get; set; }   // 고객사명
        public string ClientCeoName { get; set; }   // 대표자명
        public string CorpNo        { get; set; }   // 사업자번호
        public string ClientTel     { get; set; }   // 고객사 전화번호
        public string Email         { get; set; }   // 이메일
        public string Addr          { get; set; }   // 이메일
        public int    ClientType    { get; set; }   // 고객사 유형
        public string EncomFlag     { get; set; }   // 엔컴연동여부
        public string BtoBYN        { get; set; }   // B2B여부
        public string BtoCYN        { get; set; }   // B2C여부
        public string ClientLogoFile { get; set; }   // 고객사 로고
        public string UseFlag       { get; set; }   // 사용여부
        public string UseFlagM      { get; set; }   // 사용여부
        public string ClientServerIP { get; set; }   // 연동서버IP
        public string RegAdminID    { get; set; }   // 등록아이디
        public string RegDate       { get; set; }   // 등록일
        public string UpdAdminID    { get; set; }   // 수정아이디
        public string UpdDate       { get; set; }   // 수정일
    }
}