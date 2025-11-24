<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminMyInfo.aspx.cs" Inherits="SSO.Admin.AdminMyInfo" %>
<asp:Content ID="Scriptcontent" ContentPlaceHolderID="headscript" runat="server">
    <script src="/SSO/Admin/Proc/AdminMyInfo.js?var=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>"></script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField runat="server" id="hidErrMsg" />
    <asp:HiddenField runat="server" ID="hidDisplayMode"/>
    <asp:HiddenField runat="server" ID="Mode" />
    <asp:HiddenField runat="server" ID="HidAuthCode" />
    <asp:HiddenField runat="server" ID="HidEncCode" />
    <asp:HiddenField runat="server" ID="MobileNoAuthFlag" />
    <asp:HiddenField runat="server" ID="SecurityFlag" />
    <asp:HiddenField runat="server" ID="GradeCode" />
    
    <div id="contents">
		<table id="maintable" class="tblst1" style="width:1000px; margin:0 auto;">
            <colgroup>
                <col width="150"/>
                <col width="350"/>
                <col width="150"/>
                <col width="350"/>
            </colgroup>
            <tr style="height:60px">
                <td colspan="4" style="text-align:left;"><span style="font-size:20px;font-weight:400">내정보 관리</span></td>
            </tr>
			<tr>
                <th style="width: 200px"><span style="color:#f00">*</span> 아이디</th>
				<td colspan="3" class="lft">
					<asp:TextBox runat="server" ID="AdminID" class="type_01" style="background-color:#e7e7e7;" ReadOnly="true"></asp:TextBox>
				</td>
			</tr>
            <tr>
                <th style="width: 200px"><span style="color:#f00">*</span> 이름</th>
				<td colspan="3" class="lft">
					<asp:TextBox runat="server" ID="AdminName" class="type_01"  ReadOnly="true"></asp:TextBox>
				</td>
			</tr>
            <tr>
                <th rowspan="3"><span style="color:#f00">*</span> 비밀번호</th>
                <td colspan="3">
                    <asp:TextBox TextMode="password" runat="server" ID="OrgAdminPwd" CssClass="type_01" placeholder="현재 비밀번호"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="lft">
                    <asp:TextBox TextMode="Password" runat="server" ID="AdminResetPwd" class="type_01" MaxLength="32" style="width:250px" PlaceHolder="비밀번호 변경" Title="새 비밀번호" onKeyDown="return (event.keyCode!=13);"></asp:TextBox><br/>
                    <br />
                    <p class="info_text">비밀번호는 영문,숫자,특수문자(!@$%^*만 허용)를 혼합 사용하여 8~16자까지</p>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:TextBox TextMode="Password" runat="server" ID="AdminResetPwdChk" class="type_01" MaxLength="32" style="width:250px" PlaceHolder="비밀번호 변경확인" title="새 비밀번호 확인" onKeyDown="return (event.keyCode!=13);"></asp:TextBox>
                    <button type="button" onclick="javascript:fnResetPassword();" class="btn_file">비밀번호 변경</button>
                </td>
            </tr>
            <tr>
				<th> <span style="color:#f00">*</span>휴대폰번호</th>
				<td colspan="3" class="lft">
                    <asp:TextBox runat="server" ID="MobileNo" class="type_01 OnlyNumber" MaxLength="20" ReadOnly="true"  AutoPostBack="False"></asp:TextBox>
                    <span>* 휴대폰번호 수정은 관리자에게 요청하십시오.</span>
                    <!--
                    <button type="button" class="btn_file" onclick="fnMobileNoUpd();">수정</button>
                    <div style="margin-top:10px; display:none" id="ConfirmArea">
                        <asp:TextBox runat="server" ID="NewMobileNo" class="type_01 OnlyNumber" placeholder="변경할 휴대폰 번호" MaxLength="11" AutoPostBack="False"></asp:TextBox>
                        <button type="button" class="btn_01" id="btnMobile" onclick="fnAuthNumberGet();">인증번호</button>
                        <button type="button" class="btn_03" id="btnMobileReset" style="display:none;" onclick="fnAuthNumberReset();">다시입력</button>
                        <asp:TextBox runat="server" ID="SmsAuthNo" class="type_small OnlyNumber" MaxLength="4" style="display:none;" placeholder="인증번호" AutoPostBack="False"></asp:TextBox>
                        <button type="button" class="btn_01" id="ConfirmBtn" onclick="fnAuthMobileUpd();" style="display:none">확인</button>
                    </div>
                    -->
                </td>
			</tr>
            <tr>
	            <th>내선번호</th>
	            <td colspan="3" class="lft">
		            <asp:TextBox runat="server" ID="TelNo" class="type_01 OnlyNumber"></asp:TextBox>
	            </td>
            </tr>
            <tr>
				<th><span style="color:#f00">*</span> 이메일</th>
				<td colspan="3" class="lft">
					<asp:TextBox runat="server" ID="Email" class="type_01 OnlyEmail"></asp:TextBox>
				</td>
			</tr>
            <tr>
	            <th>부서명</th>
	            <td colspan="3" class="lft">
		            <asp:TextBox runat="server" ID="DeptName" class="type_01" placeholder="부서명" MaxLength="100"></asp:TextBox>
	            </td>
            </tr>
		</table>
        <div style="text-align:center;margin-top:10px">
            <button type="button" class="btn_b01" onclick="javascript:fnUpdAdminMyInfo();"  id="btnRegister">정보수정</button>
        </div>
    </div>
</asp:Content>
