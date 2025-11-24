<%@ Page Language="C#" AutoEventWireup="true" ClientIDMode="Static" CodeBehind="Login.aspx.cs" Inherits="SSO.Login.Login" %>
<%@ Import Namespace="CommonLibrary.Constants" %>
<%@ Import Namespace="CommonLibrary.CommonModule" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, user-scalable=" />
    <meta name="Keywords" lang="ko" content="EasyCC" />
    <meta name="keywords" lang="ko" content="EasyCC" />
    <meta name="keywords" lang="en" content="EasyCC" />
    <meta name="keywords" lang="en" content="EasyCC" />
    <meta property="og:title" content="EasyCC" />
    <meta property="og:type" content="website" />
    <meta property="og:type" content="website" />
    <meta property="og:site_name" content="EasyCC" />
    <meta property="og:description" content="EasyCC" />
    <meta name="description" content="EasyCC" />

    <title><%=Server.HtmlEncode(CommonConstant.SITE_TITLE)%></title>
    <%: Scripts.Render("~/bundles/LibJS") %>
    <script type="text/javascript" src="/js/lib/jquery-ui/jquery-ui.js?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>"></script>
    <script type="text/javascript" src="/js/common.js?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>"></script>
    <script type="text/javascript" src="/js/utils.js?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>"></script>
    <script type="text/javascript" src="/SSO/Login/Proc/Login.js?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>"></script>
    <%: Scripts.Render("~/js/lib/sweetalert2/sweetalert2.js") %>
    <link rel="icon" href="/images/favicon.png" type="image/x-icon"/>
    <link rel="stylesheet" href="/css/sweetalert2.min.css" />
    <link rel="stylesheet" href="/css/admin.css?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>" />
    <link rel="stylesheet" href="/css/reset.css?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>" />
    <script>
        $(document).ready(function () {
            if ($.cookie("LoginId") != "") {
                $("#AdminID").val($.cookie("LoginId"));
                $('#SaveId').prop('checked', true);
                $("#AdminPwd").focus();
            }

            $("input[ID=AdminID]").keypress(function (event) {
                if (event.which == 13) {
                    event.preventDefault();
                    fnGoLogin();
                }
            });

            $("input[ID=AdminPwd]").keypress(function (event) {
                if (event.which == 13) {
                    event.preventDefault();
                    fnGoLogin();
                }
            });
        });
        
    </script>
</head>
<body>
    <form id="MainForm" runat="server">
        <asp:HiddenField runat="server" ID="returnurl" />
        <asp:HiddenField runat="server" ID="errmsg" />
        <asp:HiddenField runat="server" ID="asd" />
        <!-- login_wrap//start================================//-->
        <div class="login_wrap">
	        <div class="login_logo"><img src="/images/logo_login.png" alt="티엘링크"></div>
	        <div class="login_title">티엘링크 로그인</div>	  	
	        <div class="login_body">	    	
		        <ul>
			        <li><asp:TextBox runat="server" ID="AdminID" CssClass="id" placeholder="아이디"/></li>
			        <li><asp:TextBox runat="server" TextMode="Password" ID="AdminPwd" CssClass="pw" placeholder="비밀번호"/></li>
                    <li><input type="checkbox" name="SaveId" id="SaveId"><label for="SaveId" class="wh"><span></span>    아이디 저장</label></li>
		        </ul>
                
		        <button type="button" class="btn_login" id="btnLogin">로그인</button>
	        </div>
            <!--p class="member_btn"><a href="/SSO/MemberShip/MemberShipStep1">[회원가입]</a></!--p-->
	        <div class="footer">ⓒEASYCC All Right Reserved.</div>
        </div>
        <!-- login_wrap//end================================//-->
    </form>
    <!-- Loader //-->
    <div id="divLoadingImage" style="display: none;"><img src="/images/loading.gif" alt="Loading..." /></div>
    <!-- Loader //-->
</body>
</html>
