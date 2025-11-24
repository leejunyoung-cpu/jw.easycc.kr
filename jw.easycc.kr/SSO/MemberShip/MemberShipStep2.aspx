<%@ Page Language="C#" EnableEventValidation="true" AutoEventWireup="true" CodeBehind="MemberShipStep2.aspx.cs" Inherits="SSO.MemberShip.MemberShipStep2" %>
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
    <script src="/SSO/MemberShip/Proc/MemberShipStep.js?var=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>"></script>
    <script>
        $(document).ready(function () {
            if ($("#DisplayMode").val() === "Y") {
                fnDefaultAlert($("#ErrMsg").val(), "warning", "fnLocation()", "");
            }
        });

        function fnLocation() {
            location.href = "/SSO/MemberShip/MemberShipStep1";
        }

        function fnTermsClose() {
            $(".membership_terms").hide();
            $(".terms_body > div").hide();
        }

        function fnTermsView(n) {
            $(".membership_terms").show();
            $(".terms_body > div.terms_0" + n).show();
        }

        function fnStepInfo(n) {
            if (n === 1) {
                if ($("#ClientCode").val() === "") {
                    fnDefaultAlertFocus("고객사 사업자번호를 조회해주세요.", "CorpNo", "warning", "");
                    return;
                }
                $("div.membership_form div.form_02 > div.form_opacity").fadeOut(500);
                $("#ClientCorpNo").focus();
            } 
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="DisplayMode" />
        <asp:HiddenField runat="server" ID="ErrMsg" />
        <asp:HiddenField runat="server" ID="HidAgreeFlag" />
        <asp:HiddenField runat="server" ID="ClientCode" />
        <asp:HiddenField runat="server" ID="SecurityFlag" />
        <asp:HiddenField runat="server" ID="hidAdminIDFlag" />
        <div class="membership_area">
            <div class="membership_body">
                <ul class="membership_step">
                    <li>
                        <dl>
                            <dt>STEP 01</dt>
                            <dd>약관동의</dd>
                        </dl>
                    </li>
                    <li class="on">
                        <dl>
                            <dt>STEP 02</dt>
                            <dd>정보입력</dd>
                        </dl>
                    </li>
                </ul>
            </div>

            <div class="membership_form">
                <div style="overflow:hidden;">
                    <div class="form_01">
                        <h2>
                            <strong>
                                <span>Step1</span> 고객사 조회
                            </strong>
                            <p>고객사 정보를 조회해주세요.</p>
                        </h2>
                        <div class="form_tb">
                            <p class="title">고객사 가입은 <span>EasyCC 커넥트</span> 관리자에게 문의바랍니다.</p>
                            <table>
                                <colgroup>
                                    <col width="100%;"/>
                                </colgroup>
                                <tr>
                                    <th>사업자번호</th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="CorpNo" CssClass="type_01 OnlyNumber" placeholder="숫자입력('-'제외)"></asp:TextBox>
                                        <button type="button" class="btn_02" id="BtnCorpNoChk" onclick="fnCropNoChk();">조회</button>
                                        <button type="button" class="btn_03" id="BtnCorpNoReChk" onclick="fnCropNoChkReset();" style="display:none;">다시입력</button>
                                    </td>
                                </tr>
                                <tr>
                                    <th>고객사명</th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="ClientName" ReadOnly="true" CssClass="type_03" placeholder="고객사명"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>대표자명</th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="ClientCeoName" ReadOnly="true" CssClass="type_03" placeholder="대표자명"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <p class="cnts" style="display:none;">
                            </p>
                        </div>
                        <div class="form_btn">
                            <button type="button" class="btn_01" style="width:200px;" onclick="fnStepInfo(1);">다음</button>
                        </div>
                    </div>
                    <div class="form_02">
                        <div class="form_opacity"></div>
                        <h2>
                            <strong>
                                <span>Step3</span> 개인정보
                            </strong>
                            <p>개인정보를 입력해주세요.</p>
                        </h2>
                        <div class="form_tb">
                            <table class="tb_02">
                                <colgroup>
                                    <col width="125px;"/>
                                    <col width="*;"/>
                                </colgroup>
                                <tr>
                                    <th><span style="color:#f00">*</span> 아이디</th>
                                    <td>
                                        <asp:TextBox runat="server" ID="AdminID" CssClass="type_small onlyAlphabetNum" placeholder="영문+숫자 조합" MaxLength="20"></asp:TextBox>
                                        <button type="button" class="btn_02" id="AdminIDBtn" onclick="fnAdminIDChk();">확인</button>
                                        <button type="button" class="btn_03" id="AdminIDBtnReturn" style="display:none;" onclick="fnAdminIDChkReset();">재입력</button>
                                    </td>
                                </tr>
                                <tr>
                                    <th><span style="color:#f00">*</span> 이름</th>
                                    <td>
                                        <asp:TextBox runat="server" ID="AdminName" CssClass="type_100p" placeholder="이름을 입력하세요."></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>내선번호</th>
                                    <td>
                                        <asp:TextBox runat="server" ID="TelNo" CssClass="type_100p OnlyNumber" placeholder="내선번호를 입력하세요."></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th><span style="color:#f00">*</span> 휴대폰</th>
                                    <td>
                                        <asp:TextBox runat="server" ID="MobileNo" CssClass="type_100p OnlyNumber" placeholder="휴대폰번호를 입력하세요."></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th><span style="color:#f00">*</span> E-mail</th>
                                    <td>
                                        <asp:TextBox runat="server" ID="Email" CssClass="type_100p ml_0 essential" placeholder="이메일을 입력하세요."></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th><span style="color:#f00">*</span> 비밀번호</th>
                                    <td>
                                        <asp:TextBox runat="server" TextMode="password" ID="AdminPwd" CssClass="type_100p ml_0 essential" placeholder="비밀번호를 입력하세요"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <p class="password_text">비밀번호 영문,숫자,특수문자(!@$%^*만 허용)를 혼합 사용하여 8~16자까지</p>
                                    </td>
                                </tr>
                                <tr>
                                    <th><span style="color:#f00">*</span> 비밀번호 확인</th>
                                    <td>
                                        <asp:TextBox runat="server" TextMode="password" ID="AdminPwdConfirm" CssClass="type_100p ml_0 essential" placeholder="비밀번호 확인"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="form_btn">
                            <button type="button" class="btn_01 btn_100p" onclick="fnAdminRequestIns();">회원가입</button>
                        </div>
                    </div>
                </div>
                <div style="text-align:center; margin-top:30px;">
                    <button type="button" class="btn_b01 gray" style="height:40px; width:150px;" onclick="location.href='/SSO/Login/Login';">취소</button>
                </div>
            </div>
        </div>
        <!-- Loader //-->
        <div id="divLoadingImage" style="display: none;"><img src="/images/loading.gif" alt="Loading..." /></div>
        <!-- Loader //-->
     </form>
</body>
</html>
