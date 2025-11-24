<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogimanAgree1.aspx.cs" Inherits="logiman.cargomanager.co.kr.LogimanAgree1" %>
<%@ Import Namespace="CommonLibrary.CommonModule" %>
<%@ Import Namespace="CommonLibrary.Constants" %>

<!doctype html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, user-scalable=">
    <meta name="Keywords" lang="ko" content="EasyCC">
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
    <%: Scripts.Render("~/js/lib/sweetalert2/sweetalert2.js") %>
    <link rel="icon" href="/images/icon/favicon.ico" type="image/x-icon"/>
    <link rel="stylesheet" href="/css/sweetalert2.min.css" />
    <link rel="stylesheet" href="/css/style.css?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>" />
    <link rel="stylesheet" href="/css/notosanskr.css?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>" />
    <link rel="stylesheet" runat="server" ID="ThemeStyle" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="agree_warp">
            <h1>개인정보 처리방침</h1>
            <textarea class="agree_text" cols="30" rows="5" readonly >
            </textarea>
            <div class="agree_btn">
                <button class="btn_03" onclick="self.close();">닫기</button>
            </div>
            
        </div>
    </form>
</body>
