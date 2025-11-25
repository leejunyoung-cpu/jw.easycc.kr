// 그리드
var GridID = "#ApiHuaweiListGrid";
var GridData = [];
var GridSet;

$(document).ready(function () {

    $("#DateFrom").datepicker({
        dateFormat: "yy-mm-dd",
        onSelect: function (dateFromText, inst) {
            var dateToText = $("#DateTo").val().replace(/-/gi, "");
            if (dateToText.length !== 8) {
                dateToText = GetDateToday("");
            }

            if (parseInt(dateFromText.replace(/-/gi, "")) > parseInt(dateToText)) {
                $("#DateTo").datepicker("setDate", dateFromText);
            }
        }
    });
    $("#DateFrom").datepicker("setDate", GetDateToday("-"));

    $("#DateTo").datepicker({
        dateFormat: "yy-mm-dd",
        onSelect: function (dateToText, inst) {
            var dateFromText = $("#DateFrom").val().replace(/-/gi, "");
            if (dateFromText.length !== 8) {
                dateFromText = GetDateToday("");
            }

            if (parseInt(dateFromText) > parseInt(dateToText.replace(/-/gi, ""))) {
                $("#DateFrom").datepicker("setDate", dateToText);
            }
        }
    });
    $("#DateTo").datepicker("setDate", GetDateToday("-"));

    fnCallGridData();
});

var columns = [
    { id: "HuaweiHeaderId", name: "HuaweiHeaderId", field: "HuaweiHeaderId", width: 0, cssClass: "slick-cell-tc", visible: false },
    { id: "InboxId", name: "InboxId", field: "InboxId", width: 0, cssClass: "slick-cell-tc", visible: false},
    { id: "declarationHeaderId", name: "세관신고서 ID", field: "declarationHeaderId", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "impExpFlag", name: "수출입통관서표지", field: "impExpFlag", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "preDeclarationNumber", name: "통관초안번호", field: "preDeclarationNumber", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "declarationCompanyCode", name: "신고회사코드", field: "declarationCompanyCode", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "declarationCompanyName", name: "통관회사명", field: "declarationCompanyName", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "countryCode", name: "국가 부호", field: "countryCode", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "shipper", name: "수취인 번호", field: "shipper", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "shipperName", name: "발송인명", field: "shipperName", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "consignee", name: "수입자", field: "consignee", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "consigneeName", name: "수입자명", field: "consigneeName", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "tradeMode", name: "무역방식", field: "tradeMode", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "tradeTerm", name: "무역용어", field: "tradeTerm", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "transportMode", name: "운송방식", field: "transportMode", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "transportNo", name: "운송수단 번호", field: "transportNo", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "transportRefNo", name: "수송편", field: "transportRefNo", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "departureCountryCode", name: "선적국 2자", field: "departureCountryCode", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "departureCountryShortName", name: "출발국", field: "departureCountryShortName", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "customsBroker", name: "통관 번호", field: "customsBroker", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "customsBrokerName", name: "통관대행업체명", field: "customsBrokerName", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "seller", name: "판매자 번호", field: "seller", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "sellerName", name: "판매자명", field: "sellerName", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "buyer", name: "구매자 코드", field: "buyer", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "buyerName", name: "구매자명", field: "buyerName", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "invoiceCurrency", name: "송장통화", field: "invoiceCurrency", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "invoiceAmount", name: "송장 금액", field: "invoiceAmount", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "exchangeRate", name: "환율", field: "exchangeRate", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "containerSpecification", name: "컨테이너 규격", field: "containerSpecification", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "hawb", name: "hawb", field: "운송장 번호", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "mawb", name: "mawb", field: "마스터 송장 번호", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "departurePort", name: "선적항", field: "departurePort", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "destinationPort", name: "도착항", field: "destinationPort", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "currency", name: "통관통화", field: "currency", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "totalAmount", name: "통관신고금액", field: "totalAmount", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "freight", name: "운임(운송료)", field: "freight", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "insurance", name: "보험료", field: "insurance", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "otherFee", name: "잡비", field: "otherFee", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "totalPackages", name: "총 건수", field: "totalPackages", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "totalNetWeight", name: "총순중량", field: "totalNetWeight", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "totalGrossWeight", name: "총중량", field: "totalGrossWeight", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "totalVolume", name: "총부피", field: "totalVolume", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "fobAmount", name: "신고서 FOB금액", field: "fobAmount", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "cifAmount", name: "CIF 금액", field: "cifAmount", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "invnumber", name: "인보이스번호", field: "invnumber", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "plNo", name: "상자 번호", field: "plNo", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "contractNumber", name: "계약번호", field: "contractNumber", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "shipmentId", name: "운송 방안 번호", field: "shipmentId", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "HuaweiLineId", name: "HuaweiLineId", field: "HuaweiLineId", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "lineNo", name: "행번", field: "lineNo", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "itemCode", name: "재료코드", field: "itemCode", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "itemDescription", name: "재료설명", field: "itemDescription", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "hsCode", name: "세번부호", field: "hsCode", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "goodsName", name: "세관 상품", field: "goodsName", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "goodsNameEn", name: "세관 상품영문", field: "goodsNameEn", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "brand", name: "브랜드", field: "brand", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "specification", name: "모델", field: "specification", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "originCountryCode", name: "원산지", field: "originCountryCode", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "originCountryName", name: "원산국명", field: "originCountryName", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "unit", name: "unit", field: "단위", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "quantity", name: "수량", field: "quantity", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "price", name: "단가", field: "price", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "amount", name: "금액", field: "amount", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "netWeight", name: "순중량", field: "netWeight", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "totalGrossWeight2", name: "재료 총중량", field: "totalGrossWeight2", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "invoiceFobAmount", name: "인보이스 FOB 금액", field: "invoiceFobAmount", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "licnumber", name: "영업 허가증 번호", field: "licnumber", width: 150, cssClass: "slick-cell-tc", visible: true },
    { id: "RegDate", name: "등록일자", field: "RegDate", width: 150, cssClass: "slick-cell-tc", visible: true },
];

/*그리드 옵션 */
var options = {
    editable: true,
    enableAddRow: false,
    enableCellNavigation: true,
    asyncEditorLoading: false,
    rowHeight: 35
};

/**
 * 버튼 이벤트
 */

/*조회*/
function actionButtonFormatter(row, cell, value, columnDef, dataContext) {
    return "<button type=\"button\" class=\"btn_b01\" data-row=\"" + row + "\" onclick=\"fnClientIns('" + dataContext.ClientCode + "')\">조회</button>";
}

/**
 * /
 * 그리드 데이터 호출
 */
function fnCallGridData(strGID) {
    $("#divLoadingImage").show();
    var strHandlerURL = "/API/Huawei/Proc/ApiHuaweiHandler.ashx";
    var strCallBackFunc = "fnGridSuccResult";

    var objParam = {
        CallType        : "ApiHuaweiList",
        DateFrom        : $("#DateFrom").val(),
        DateTo          : $("#DateTo").val(),
        PageNo          : $("#PageNo").val(),
        PageSize        : $("#PageSize").val()
    };
    
    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, false, "", "", true);
}


function fnGridSuccResult(objRes) {
    if (objRes) {
        if (objRes[0].result.ErrorCode !== 0) {
            $("#RecordCnt").val(0);
            $("#GridResult").html("");
            $("#divLoadingImage").hide();
            fnDefaultAlert(objRes[0].result.ErrorMsg, "warning");
            return false;
        }

        // 페이징
        fnCreatePagingNavigator();
        $("#RecordCnt").val(objRes[0].data.RecordCnt);
        $("#GridResult").html("[" + objRes[0].data.RecordCnt + "건]");
        // visible이 true인 컬럼만 필터링
        var visibleColumns = columns.filter(column => column.visible);
        GridData = objRes[0].data.List;
        GridSet = new Slick.Grid(GridID, GridData, visibleColumns, options);
        fnGirdDbClick(GridSet, GridData);
    }
    $("#divLoadingImage").hide();
}

/*더블클릭*/
function fnGirdDbClick(Grid, Data) {
    Grid.onDblClick.subscribe(function (e, args) {
        var cell = Grid.getCellFromEvent(e);
        var item = Data[cell.row];
        fnClientIns(item.ClientCode);
    });
}

/*버튼 이벤트*/
/**
 * 고객사 등록
 */
function fnClientIns(ClientCode) {
    var title = "고객사 등록";
    var Mode = "Insert";
    if (typeof ClientCode !== "undefined" && ClientCode !== "") {
        title = "고객사 수정";
        Mode = "Update";
    }

    fnOpenRightSubLayer(title, "/SSO/Client/ClientIns.aspx?HidMode=" + Mode +"&ClientCode=" + ClientCode, "1024px", "700px", "50%");
}

/*엑셀다운로드*/
function fnClientListExcel() {
    fnGridExcelDownload("고객사_리스트", GridData, columns);
}