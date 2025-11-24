<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="easycc.co.kr.Index" %>
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
            /*대메뉴 클릭 Start*/
            $('#menu ul.main li span').click(function () {
                if ($("ul.sub_depth1").hasClass("open")) {
                    if ($(this).next("ul.sub_depth1").hasClass("open")) {
                        $(this).next("ul.sub_depth1").stop().animate({ 'width': '0px' }, 200);
                        //$("ul.sub_depth1 li").not($(this)).hide(50);
                        $(this).next("ul.sub_depth1").removeClass('open').animate({ 'left': '-1000%' }, 200);
                        $(this).parent("li").removeClass("on");
                        $("div.menu_fade").hide();
                    } else {
                        $("ul.sub_depth1").not($(this)).css({ 'width': '0px', 'left': '-1000%' });
                        //$("ul.sub_depth1 li").not($(this)).hide();
                        $("ul.sub_depth1").not($(this)).removeClass('open');
                        $("#menu ul.main li.m").removeClass("on");
                        $(this).next("ul.sub_depth1").addClass("open");
                        //$(this).next("ul.open").children("li").show();
                        $(this).next("ul.sub_depth1").css("left", "160px");
                        $(this).next("ul.sub_depth1").css('width', '180px');
                        $(this).parent("li").addClass("on");
                        $("div.menu_fade").show();
                    }
                } else {
                    if (!$(this).next("ul.sub_depth1").hasClass("open")) {
                        $("ul.sub_depth1").not($(this)).css({ 'width': '0px', 'left': '-1000%' });
                        $("ul.sub_depth1").not($(this)).removeClass('open');
                        $("#menu ul.main li.m").removeClass("on");
                        $(this).next("ul.sub_depth1").addClass("open");
                        //$(this).next("ul.sub_depth1").children("li").show(200);
                        $(this).next("ul.sub_depth1").css("left", "160px");
                        $(this).next("ul.sub_depth1").stop().animate({ 'width': '180px' }, 200);
                        $(this).parent("li").addClass("on");
                        $("div.menu_fade").show();
                    }
                }
            });

            $("div#menu ul.main ul.sub_depth1 li dl dt").click(function () {
                if ($(this).hasClass("on")) {
                    $(this).removeClass("on");
                    $(this).next("dd").slideUp();
                } else {
                    $("div#menu ul.main ul.sub_depth1 li dl dt").not($(this)).next("dd").slideUp();
                    $("div#menu ul.main ul.sub_depth1 li dl dt").not($(this)).removeClass("on");
                    $(this).addClass("on");
                    $(this).next("dd").slideDown();
                }
            });
            /*대메뉴 클릭 Start*/

            /*메뉴 닫기*/
            $("header, div.menu_fade").click(function (e) {
                $("ul.sub_depth1.open").css({ 'width': '0', 'left': '-1000%' });
                $("ul.sub_depth1").removeClass("open");
                $("div.menu_fade").hide();
            });

            $("div#menu ul.main ul.sub_depth1 li dl dd ul li").click(function (e) {
                $("div#menu ul.main ul.sub_depth1 li dl dd ul li").removeClass("on");
                $(this).addClass("on");
            });

        });

        function fnGoPage(GroupTitle, SubGroupTitle, MenuName, strUrl, GroupNo, MenuNo) {
            $("#iframePageLoading").show();
            $("div#SiteMap").hide();
            $("div.menu_fade").hide();
            var MenuList = "";
            var selected = "";
            $("header div.page_navi a").removeClass("on");
            $("ul.menu" + GroupNo).children("li").each(function (index) {
                if (MenuNo === index) {
                    selected = "on";
                } else {
                    selected = "";
                }
                MenuList += "<a class=\"" + selected + "\" href=\"" + $(this).children("a").attr("href") + "\">" + $(this).children("a").text() + "</a>";
            });
            if (GroupTitle === "내정보") {
                $("div.page_items").html("<span>" + GroupTitle + "</span>");
            } else {
                $("div.page_items").html("<span>" + GroupTitle + "</span>" + " <span class='arrow'></span> " + "<span>" + SubGroupTitle + "</span>" + " <span class='arrow'></span> " + MenuList);
            }
            
            document.getElementById("Content_frame").src = strUrl;
            $("ul.sub_depth1.open").css({ 'width': '0', 'left': '-1000%' });
            $("ul.sub_depth1").removeClass("open");
            $('#Content_frame').contents().find('div.data_list h1.title').text("사용자 관리");
        }

        $(function () {
            $('#Content_frame').on("load", function () {
                $("#iframePageLoading").hide();
            });
        });

        function fnGoLogout() {
            __doPostBack("logout", "");
        }
    </script>
</head>
<body>
<form id="form1" runat="server">
    <asp:HiddenField runat="server" ID="HidGradeCode"/>
    <div class="body_wrap">
        <!-- header // - start -->
        <header class="header">		
	        <div class="hd_inner">
		        <h1 class="logo">
			        <a href="/"><img src="images/logo.png" alt="정운관세법인" /></a>	
		        </h1>
		        <div id="gnb" class="gnb">
			        <ul class="ic_menu">
				        <!--li><a href=""><img src="images/hd_ic_01.png" title="영상"></a> <span class="alm">2</span></li-->
				        <li style="color: #fff; font-weight: 700; vertical-align: super; font-size: 16px;"><%=Server.HtmlEncode(SiteGlobal.SERVER_TITLE)%></li>
				        <li class="on"><a title="내정보" href="javascript:fnGoPage('내정보','','','/SSO/Admin/AdminMyInfo.aspx',0);"><img src="/images/icon/info_icon_wh_03.png" title="내정보" /></a>   </li>
                        <li class="on"><a title="로그아웃" href="javascript:fnDefaultConfirm('로그아웃 하시겠습니까?', 'fnGoLogout', '');"><img src="/images/icon/info_icon_wh_01.png" title="로그아웃" /></a> </li>
			        </ul>
		        </div>
	        </div>
            <div class="page_navi">
                <div class="page_items">
                    <span>Main</span>
                </div>
            </div>
        </header>
        <div id="menu">
            <ul class="main">
                <%=strLeftMenu_B_List%>
            </ul>
        </div>
        <section class="content_section">
            <iframe id="Content_frame" name="Content_frame" style="width: 100%; height:100%; border:none;" src="/DashBoard/DashBoard_bak" scrolling="no" border="0"></iframe>
            <div class="menu_fade"></div>
            <div id="iframePageLoading" ><img src="/images/loading.gif" alt="Loading..." /></div>
        </section>
    </div>
</form>
</body>
</html>
