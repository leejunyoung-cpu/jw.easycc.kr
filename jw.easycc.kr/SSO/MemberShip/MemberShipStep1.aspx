<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberShipStep1.aspx.cs" Inherits="SSO.MemberShip.MemberShipStep1" %>
<%@ Import Namespace="CommonLibrary.Constants" %>
<%@ Import Namespace="CommonLibrary.CommonModule" %>
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
    <link rel="icon" href="/images/icon/favicon.png" type="image/x-icon"/>
    <link rel="stylesheet" href="/css/sweetalert2.min.css" />
    <link rel="stylesheet" href="/css/admin.css?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>" />
    <link rel="stylesheet" href="/css/reset.css?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>" />
    <script>
        $(document).ready(function () {
            $("#AllCheck").click(function () {
                if ($("#AllCheck").is(":checked")) $("input[name=TermsCheck]").prop("checked", true);
                else $("input[name=TermsCheck]").prop("checked", false);
            });
        });

        function fnTermsView(n) {
            $(".membership_terms").show();
            $(".terms_body > div.terms_0" + n).show();
        }

        function fnTermsClose() {
            $(".membership_terms").hide();
            $(".terms_body > div").hide();
        }

        function fnTermsAgree() {
            if (!$("#Check1").is(":checked")) {
                fnDefaultAlert("서비스 이용약관에 동의해주세요", "warning");
                return;
            }
            if (!$("#Check2").is(":checked")) {
                fnDefaultAlert("개인정보 처리방침에 동의해주세요", "warning");
                return;
            }
            $("#HidAgreeFlag").val("Y");
            
            var $form = null;
            
            $form = $("<form name='layerIframeForm'></form>");
            $form.attr("action", "/SSO/MemberShip/MemberShipStep2");
            $form.attr("method", "post");
            $form.attr("target", "_self");

            var formData = $("#form1").serializeObject();
            $form.addHidden(formData);
            $form.appendTo("body");
            $form.submit();
            $form.remove();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" method="post">
        <asp:HiddenField runat="server" ID="HidAgreeFlag"/>
        <div class="membership_area">
            <div class="membership_body">
                <ul class="membership_step">
                    <li class="on">
                        <dl>
                            <dt>STEP 01</dt>
                            <dd>약관동의</dd>
                        </dl>
                    </li>
                    <li>
                        <dl>
                            <dt>STEP 02</dt>
                            <dd>정보입력</dd>
                        </dl>
                    </li>
                </ul>

                <div class="membership_agree">
                    <dl>
                        <dt>EasyCC 커넥트</dt>
                        <dd>
                            EasyCC 커넥트에 회원가입을 원하실 경우<br /> 아래의 약관에 동의해주세요.
                        </dd>
                    </dl>
                    <table>
                        <colgroup>
                            <col style="width:70%;"/>
                            <col style="width:30%;"/>
                        </colgroup>
                        <thead>
                            <tr>
                                <th colspan="2">
                                    <input type="checkbox" name="AllCheck" id="AllCheck"><label for="AllCheck"><span></span></label>
                                    이용약관 모두 동의
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th>
                                    <input type="checkbox" name="TermsCheck" id="Check1"><label for="Check1"><span></span></label>
                                    (필수) 서비스 이용약관
                                </th>
                                <td>
                                    <a href="javascript:fnTermsView(1);">내용보기</a>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <input type="checkbox" name="TermsCheck" id="Check2"><label for="Check2"><span></span></label>
                                    (필수) 개인정보 처리방침
                                </th>
                                <td>
                                    <a href="javascript:fnTermsView(2);">내용보기</a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <ul class="member_btn">
                        <li class="left">
                            <button type="button" class="btn1" onclick="history.back();">취소</button>
                        </li>
                        <li class="right">
                            <button type="button" class="btn2" onclick="fnTermsAgree();">동의 후 계속하기</button>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="membership_terms">
                <div class="terms_body">
                    <a href="javascript:fnTermsClose();" class="terms_close"></a>
                    <div class="terms_01">
                        <h1>서비스 이용약관</h1>
                        <textarea readonly></textarea>
                    </div>
                    <div class="terms_02">
                        <h1>개인정보 처리방침</h1>
                        <textarea readonly></textarea>
                    </div>
                </div>
            </div>
        </div>
     </form>
</body>
</html>
