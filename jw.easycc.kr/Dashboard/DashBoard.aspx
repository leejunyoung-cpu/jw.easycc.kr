<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" Inherits="easycc.co.kr.DashBoard" %>
<%@ Import Namespace="CommonLibrary.CommonUtils" %>
<asp:Content ID="Scriptcontent" ContentPlaceHolderID="headscript" Runat="Server">
    <link rel="stylesheet" href="/css/dashboard.css?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>" />
    <script src="/js/lib/chart.js"></script>
    <script src="/DashBoard/Proc/DashBoard.js?ver=<%=DateTime.Now.ToString("yyyyMMddHHmmss")%>"></script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField runat="server" ID="hidDisplayMode" />
    <asp:HiddenField runat="server" ID="hidErrMsg" />    
    <div id="DashBoard">
        <div class="count_area">
            <dl class="total_01">
                <dt id="TotalCnt"></dt>
                <dd>총 수입건수</dd>
            </dl>
            <dl class="total_02">
                <dt id="GyeljeInput"></dt>
                <dd>총 수입금액</dd>
            </dl>
            <dl class="total_03">
                <dt id="CifTotalWon"></dt>
                <dd>총 납부금액</dd>
            </dl>
            <dl class="total_04">
                <dt id="TotalTax"></dt>
                <dd>총 부가세</dd>
            </dl>
        </div>
        <div class="table_area">
            <div class="left">
                <dl class="table_title">
                    <dt>통관</dt>
                    <dd>
                        <button type="button" onclick="location.href='/CustomsClearance/ImportClient/ImportClientList'">수입의뢰</button>
                    </dd>
                </dl>
                <table>
                    <colgroup>
                        <col style="width:5%"/>
                        <col style="width:8%"/>
                        <col style="width:10%"/>
                        <col style="width:15%"/>
                        <col style="width:15%"/>
                        <col style="width:12%"/>
                        <col style="width:12%"/>
                        <col style="width:12%"/>
                        <col style="width:11%"/>
                    </colgroup>
                    <thead>
                        <tr>
                            <th>No</th>
                            <th>의뢰일</th>
                            <th>BL No.</th>
                            <th>수입자</th>
                            <th>납제자</th>
                            <th>결제금액</th>
                            <th>신고금액</th>
                            <th>부가세</th>
                            <th>신고상태</th>
                        </tr>
                    </thead>
                    <tbody>
                        <!-- DISPLAY LIST START -->
                        <asp:Repeater ID="repList" runat="server" EnableViewState="false">
                            <ItemTemplate>
                                <tr>
                                    <td>#</td>
                                    <td><%# Server.HtmlEncode(Utils.ConvertDateFormat(Eval("OrderDate").ToString()))%></td>
                                    <td><%# Server.HtmlEncode(Eval("Impo_bl_no").ToString())%></td>
                                    <td><%# Server.HtmlEncode(Eval("Impo_suipja_sangho").ToString())%></td>
                                    <td><%# Server.HtmlEncode(Eval("Impo_napse_sangho").ToString())%></td>
                                    <td><%# Server.HtmlEncode(Eval("GyeljeInput").ToString())%></td>
                                    <td><%# Server.HtmlEncode(Eval("CifTotalWon").ToString())%></td>
                                    <td><%# Server.HtmlEncode(Eval("TotalTax").ToString())%></td>
                                    <td><%# Server.HtmlEncode(Eval("StatusName").ToString())%></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <!-- DISPLAY LIST END -->
                    </tbody>
                </table>
            </div>
            <div class="right">
                <div class="top">
                    <h2>환율</h2>
                    <div style="position:relative; padding:25px;">
                        <canvas id="myChart"></canvas>
                    </div>
                </div>
                <div class="bottom">
                    <h2>월 건수 추이</h2>

                </div>
            </div>
        </div>
        <div class="etc_layer">
            <div class="etc_layer_01">
                <dl>
                    <dt><a href="javascript:fnBoardReqOpen();">전문가 의뢰 요청</a> </dt>
                    <dd>컨설팅 / FTA / 관세환급 / 관세조사 / 교육</dd>
                </dl>
            </div>
            <div class="etc_layer_02">
                <dl>
                    <dt><a href="javascript:void(0);">교육 의뢰</a> </dt>
                    <dd>Educational Request</dd>
                </dl>
            </div>
            <div class="etc_layer_03">
                <h2>유관기관 사이트</h2>
                <div class="link-box">
                    <a href="https://www.customs.go.kr" target="_blank" class="box">
                        <img src="/images/icon/logo1.jpg" alt="관세청">
                    </a>
                    <a href="https://unipass.customs.go.kr" target="_blank" class="box">
                        <img src="/images/icon/logo2.jpg" alt="유니패스">
                    </a>
                    <a href="https://aeo.or.kr" target="_blank" class="box">
                        <img src="/images/icon/logo3.jpg" alt="한국AEO진흥협회">
                    </a>
                </div>
            </div>
        </div>
    </div>
    <!--대시보드 팝업-->
    <div id="DashBoardPopUp">
        <div class="dashboard_layer">
            <h2>컨설팅 의뢰</h2>
            <table>
                <colgroup>
                    <col style="width:105px;"/>
                    <col style="width:215px;"/>
                    <col style="width:105px;"/>
                    <col style="width:215px;"/>
                </colgroup>
                <tr>
                    <th><span style="color:#f00">*</span> 요청일</th>
                    <td>
                        <asp:TextBox runat="server" ID="ReqYMD" CssClass="type_100p"></asp:TextBox>
                    </td>
                    <th><span style="color:#f00">*</span> 요청시간</th>
                    <td>
                        <asp:TextBox runat="server" ID="ReqTime" TextMode="Time" CssClass="type_100p"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><span style="color:#f00">*</span> 요청담당자</th>
                    <td>
                        <asp:TextBox runat="server" ID="ReqUserName" CssClass="type_100p" placeholder="요청자 성함 입력"></asp:TextBox>
                    </td>
                    <th><span style="color:#f00">*</span> 연락처</th>
                    <td>
                        <asp:TextBox runat="server" ID="ReqUserTelNo" CssClass="type_100p OnlyNumber" MaxLength="11" placeholder="숫자만 입력('-'제외)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><span>　</span>이메일</th>
                    <td>
                        <asp:TextBox runat="server" TextMode="Email" ID="ReqUserEmail" CssClass="type_100p" placeholder="요청자 이메일 주소"></asp:TextBox>
                    </td>
                    <th><span style="color:#f00">*</span> 요청 종류</th>
                    <td>
                        <asp:DropDownList runat="server" ID="ReqType" CssClass="type_01"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><span style="color:#f00">*</span> 요청 내용</th>
                    <td colspan="3">
                        <asp:TextBox runat="server" oninput="this.value = this.value.replace(/<script.*?>.*?<\/script>/gi, '')" ID="ReqDetail" style="padding:10px; height:98px;" MaxLength="2000" placeholder="2000자 이내" TextMode="MultiLine" CssClass="type_100p"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <p>* 요청일 : 컨설팅 의뢰를 받고 싶은 날짜 / 요청시간 : 컨설팅 의뢰를 받고 싶은 시간<br />의뢰서가 접수되면 요청하신 분 연락처를 통해 담당자가 연락을 드립니다.</p>
            <div class="req_btn">
                <button type="button" onclick="fnBoardReqConfirm();" class="btn_01">등록</button>
                &nbsp;&nbsp;&nbsp;
                <button type="button" onclick="fnBoardReqClose();" class="btn_03">닫기</button>
            </div>
        </div>

    </div>
</asp:Content>
