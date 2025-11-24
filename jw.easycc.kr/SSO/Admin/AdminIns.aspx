<%@ Page Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="AdminIns.aspx.cs" Inherits="SSO.Admin.AdminIns" %>
<asp:Content ID="Scriptcontent" ContentPlaceHolderID="headscript" Runat="Server">
    <script src="/SSO/Admin/Proc/AdminIns.js?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>"></script>
    <style>
        #CenterCodes label {margin-right:15px; font-size:14px;}
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField runat="server" ID="hidDisplayMode" />
    <asp:HiddenField runat="server" ID="hidErrMsg" />
    <asp:HiddenField runat="server" ID="hidMode" />
    <asp:HiddenField runat="server" ID="hidAdminID" />
    <asp:HiddenField runat="server" ID="hidAdminIDFlag" />
    <asp:HiddenField runat="server" ID="AdminCorpNo" />
    <asp:HiddenField runat="server" ID="AdminCorpName" />
    
    <div id="iframe_wrap">
        <div runat="server" id="POPUP_VIEW">
            <div class="popup_control">
                <div>
                    <table class="tblst1">
                        <colgroup>
                            <col style="width:180px"/> 
                            <col style="width:auto;"/> 
                        </colgroup>
                        <tr>
                            <th><span style="color:#f00">*</span>사용자 등급</th>
                            <td class="lft"><asp:DropDownList runat="server" ID="GradeCode" Width="150px" CssClass="type_01"></asp:DropDownList></td>
                        </tr>
                        <tr id="ClientCodeTr">
                            <th><span style="color:#f00">*</span> 고객사</th>
                            <td>
                                <asp:CheckBoxList runat="server" ID="ClientCodes" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:CheckBoxList>
                                <asp:DropDownList runat="server" ID="ClientCode" CssClass="type_01" style="display:none;"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th style="width:150px"><span style="color:#f00">*</span> 아이디</th>
                            <td class="lft">
                                <asp:TextBox runat="server" ID="AdminID" Width="233px" class="type_01 onlyAlphabetNum" MaxLength="20" PlaceHolder="아이디(영문/숫자)" Title="아이디(영문/숫자)"></asp:TextBox>
                                <button type="button" class="btn_b01 gray" runat="server" id="AdminIDBtn" onclick="fnChkAdminID(1);">중복확인</button>
                                <button type="button" class="btn_b01 gray" runat="server" id="AdminIDBtnReturn" onclick="fnChkAdminID(2);" style="display:none;">다시 입력</button>
                                <br />
                                <span class="id_pass" style="color:#006cb7; display:none;">사용가능한 아이디입니다.</span>
                                <span class="id_fail" style="color:#f15929; display:none;">이미 등록된 아이디입니다.</span>
                            </td>
                        </tr>
                        <tr>
                            <th><span style="color:#f00">*</span> 이름</th>
                            <td class="lft"><asp:TextBox runat="server" ID="AdminName" class="type_01" MaxLength="30" placeholder="필수입력" title="필수입력"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th><span style="color:#f00">*</span> 휴대폰 번호</th>
                            <td class="lft"><asp:TextBox runat="server" ID="MobileNo" class="type_01 OnlyNumber" MaxLength="20" placeholder="필수입력 ('-'제외)" title="필수입력 ('-'제외)"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th><span style="color:#f00">*</span> 이메일</th>
                            <td class="lft"><asp:TextBox runat="server" ID="Email" class="type_02" MaxLength="100" placeholder="이메일"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th><span style="color:#f00">*</span> 서비스 유형</th>
                            <td class="lft">
                                <asp:CheckBox runat="server" ID="BtoBYN" Text="<span></span> B2B"/>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:CheckBox runat="server" ID="BtoCYN" Text="<span></span> B2C"/>
                            </td>
                        </tr>
                        <tr>
                            <th><span style="color:#f00">*</span> 접속지역</th>
                            <td class="lft">
                                <asp:DropDownList runat="server" ID="AdminAreaType" CssClass="type_01"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>UPS담당여부</th>
                            <td class="lft">
                                <asp:DropDownList runat="server" ID="UpsCsType" CssClass="type_01"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>부서명</th>
                            <td class="lft"><asp:TextBox runat="server" ID="DeptName" class="type_02" MaxLength="100" placeholder="부서명"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>내선번호</th>
                            <td class="lft"><asp:TextBox runat="server" ID="TelNo" class="type_02" MaxLength="20" placeholder="내선번호"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>계정 만료일</th>
                            <td class="lft"><asp:TextBox runat="server" ID="ExpireYMD" class="type_01 date" title="만료일" placeholder="만료일" readonly="true" style="cursor:pointer;"/></td>
                        </tr>
                        <tr>
                            <th><span style="color:#f00">*</span> 개인정보 동의여부</th>
                            <td class="lft"><asp:DropDownList runat="server" ID="PrivateAvailFlag" Width="120px" CssClass="type_01"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <th><span style="color:#f00">*</span> 계정 사용 여부</th>
                            <td class="lft"><asp:DropDownList runat="server" ID="UseFlag" Width="120px" CssClass="type_01"></asp:DropDownList></td>
                        </tr>
                    </table>
                </div>
            </div>
            <div style="text-align:center;margin-top:10px">
                <ul><li><button type="button" class="btn_b01" onclick="javascript:fnInsAdmin();"><asp:Label runat="server" ID="lblMode" style="color:#fff;font-weight:200;font-size:15px">저장</asp:Label></button></li></ul>
            </div>
        </div>
    </div>
</asp:Content>
