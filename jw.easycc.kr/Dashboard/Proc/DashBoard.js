$(document).ready(function () {
    var ctx = document.getElementById('myChart').getContext('2d');
    var chart = new Chart(ctx, {
        // type : 'bar' = 막대차트를 의미합니다.
        type: 'bar', // 
        data: {
            labels: ['USD', 'JPY', 'EUR'],
            datasets: [{
                label: '환율',
                backgroundColor: [
                    'rgba(255, 99, 132, 0.8)', // USD 색상
                    'rgba(54, 162, 235, 0.8)', // JPY 색상
                    'rgba(255, 206, 86, 0.8)'  // EUR 색상
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)'
                ],
                borderWidth: 1,
                data: [1432.9, 958.27, 1503.5,]
            }]
        },
        options: {
            plugins: {
                legend: {
                    display: false // 범례 숨기기
                }
            },
            responsive: true,
            maintainAspectRatio: false,
            resizeDelay: 0, // 크기 변경 감지 시 즉시 반응
            useResizeObserver: true // 부모 크기 변화를 감지하여 자동 조정
        }
    });

    $("#ReqYMD").datepicker({
        dateFormat: "yy-mm-dd",
        minDate: 0
        /*onSelect: function (dateFromText, inst) {
            var dateToText = $("#DateTo").val().replace(/-/gi, "");
            if (dateToText.length !== 8) {
                dateToText = GetDateToday("");
            }

            if (parseInt(dateFromText.replace(/-/gi, "")) > parseInt(dateToText)) {
                $("#DateTo").datepicker("setDate", dateFromText);
            }
        }*/
    });

    $("#ReqYMD").datepicker("setDate", GetDateToday("-"));
    fnCallMoneyData();
});

function fnBoardReqOpen() {
    $("#DashBoardPopUp").show();
}

function fnBoardReqClose() {
    $("#DashBoardPopUp").hide();
    fnBoardReqReset();
}

function fnBoardReqReset() {
    $("#ReqYMD").datepicker("setDate", GetDateToday("-"));
    $("#ReqTime").val("");
    $("#ReqUserName").val("");
    $("#ReqUserTelNo").val("");
    $("#ReqDetail").val("");
}

function fnBoardReqConfirm() {
    if ($("#ReqYMD").val() === "") {
        fnDefaultAlertFocus("요청일을 선택해주세요.", "ReqYMD", "warning");
        return;
    }

    if ($("#ReqTime").val() === "") {
        fnDefaultAlert("요청시간을 선택해주세요.", "warning");
        return;
    }

    if ($("#ReqUserName").val() === "") {
        fnDefaultAlertFocus("요청하신분의 성함을 입력해주세요.", "ReqUserName", "warning");
        return;
    }

    if ($("#ReqUserTelNo").val() === "") {
        fnDefaultAlertFocus("요청하신분의 연락처를 입력해주세요.", "ReqUserTelNo", "warning");
        return;
    }

    if ($("#ReqDetail").val() === "") {
        fnDefaultAlertFocus("의뢰 내용을 입력해주세요.", "ReqDetail", "warning");
        return;
    }

    var objFnParam = { CallType: "ServiceReqBoardIns" };
    fnDefaultConfirm("컨설팅을 요청하시겠습니까?", "fnInsServiceReqBoard", objFnParam);
}

function fnInsServiceReqBoard(objParam) {
    var strHandlerURL = "/DashBoard/Proc/DashBoardHandler.ashx";
    var strCallBackFunc = "fnAjaxInsServiceReqBoard";

    var objParam = {
        CallType: objParam.CallType,
        ReqYMD: $("#ReqYMD").val(),
        ReqTime: $("#ReqTime").val(),
        ReqUserName: $("#ReqUserName").val(),
        ReqUserTelNo: $("#ReqUserTelNo").val(),

        ReqUserEmail: $("#ReqUserEmail").val(),
        ReqType: $("#ReqType").val(),
        ReqGubun: 1,
        ReqDetail: $("#ReqDetail").val()
    };
    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, true, "", "", true);
}

function fnAjaxInsServiceReqBoard(objRes) {
    if (objRes) {
        if (objRes[0].RetCode === 0) {
            $("#DashBoardPopUp").hide();
            fnDefaultAlert("요청 되었습니다.<br>담당자가 확인 후 회신 예정입니다.", "success", "fnBoardReqReset");
        } else {
            fnDefaultAlert("실패하였습니다.<br>" + objRes[0].ErrMsg, "error", "fnBoardReqReset");
        }
    }
}
function fnCallMoneyData() {
    $("#divLoadingImage").show();
    var strHandlerURL = "/CustomsClearance/ImportClient/Proc/ImportClientHandler.ashx";
    var strCallBackFunc = "fnMoneySuccResult";

    var objParam = {
        CallType: "ImportMoney",
        DateFrom: "",
        DateTo: ""
    };

    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, false, "", "", true);
}
function fnMoneySuccResult(data) {
    if (data) {

        $("#TotalCnt").html(data[0].TotalCnt);
        $("#GyeljeInput").html(data[0].GyeljeInput);
        $("#CifTotalWon").html(data[0].CifTotalWon);
        $("#TotalTax").html(data[0].TotalTax);

    }
    $("#divLoadingImage").hide();
}