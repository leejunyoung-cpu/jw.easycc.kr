$(document).ready(function () {
    if ($("#HidMode").val() === "Update") {
        fnCallApiWhiteDetail();
        $("#ServiceCode").attr("readonly", true);
        $("#Company").attr("readonly", true);
        $("#IpAddress").attr("readonly", true);
    } else {
        $(".active").remove();
    }
})

function fnInsApiWhiteConfirm() {
    var strConfMsg = "";
    var strCallType = "";

    if ($("#ServiceCode").val() === "") {
        fnDefaultAlertFocus("서비스코드는 필수입니다.", "ServiceCode", "warning");
        return;
    }

    if ($("#Company").val() === "") {
        fnDefaultAlertFocus("업체코드는 필수입니다.", "Company", "warning");
        return;
    }

    if ($("#IpAddress").val() === "") {
        fnDefaultAlertFocus("IP 입력은 필수입니다.", "IpAddress", "warning");
        return;
    }
    

    if ($("#HidMode").val() === "Update") {
        if ($("#IsActive").val() === "") {
            fnDefaultAlertFocus("활성화여부를 선택해주세요.", "IsActive", "warning");
            return;
        }

        strCallType = "ApiWhiteUpd"
        strConfMsg = "해당 정보를 수정 하시겠습니까?";
    } else {
        strCallType = "ApiWhiteIns"
        strConfMsg = "해당 정보를 등록 하시겠습니까?";
    }

    

    var objFnParam = { CallType: strCallType};
    fnDefaultConfirm(strConfMsg, "fnApiWhiteIns", objFnParam);
    return;
}

function fnApiWhiteIns(objParam) {
    var strHandlerURL = "/SSO/API/Proc/ApiWhiteHandler.ashx";
    var strCallBackFunc = "fnAjaxResultInsApiWhite";
    var strFailFunc = "fnAjaxResultFailInsApiWhite";

    var objParam = {
        CallType        : objParam.CallType,
        SeqNo           : $("#SeqNo").val(),
        ServiceCode     : $("#ServiceCode").val(),
        Company         : $("#Company").val(),
        IpAddress       : $("#IpAddress").val(),
        IsActive        : $("#IsActive").val(),
        ServiceUrl      : $("#ServiceUrl").val(),
        IpNote          : $("#IpNote").val()
    };
    
    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, true, strFailFunc, "", true);
}

function fnAjaxResultInsApiWhite(objData) {
    if (objData[0].RetCode !== 0) {
        fnDefaultAlert("나중에 다시 시도해 주세요.<br />(" + objData[0].ErrMsg + ")", "warning");
        return;
    } else {
        FileBlUpload = [];
        FileBlPath = [];
        fnDefaultAlert("처리되었습니다.", "success", "fnAjaxInsApiWhiteComplete", { SeqNo: objData[0].SeqNo });
        parent.fnCallGridData();
        return;
    }
}

function fnAjaxResultFailInsApiWhite(objData) {
    fnDefaultAlert("오류가 있어 담당자에게 문의바랍니다.");
    console.log(objData);
    return;
}

function fnAjaxInsApiWhiteComplete(objParam) {
    if (typeof objParam.SeqNo === "undefined") {
        window.location.reload();
        return false;
    }

    document.location.replace("/SSO/API/ApiWhiteIns?HidMode=Update&SeqNo=" + objParam.SeqNo);
}

//정보관리 상세조회 시작----------------------------------------------------------------------------------------------------------
function fnCallApiWhiteDetail() {

    var strHandlerURL = "/SSO/API/Proc/ApiWhiteHandler.ashx";
    var strCallBackFunc = "fnOrderDetailSuccResult";
    var strFailCallBackFunc = "fnCallDetailFailResult";

    var objParam = {
        CallType: "ApiWhiteList",
        SeqNo: $("#SeqNo").val()
    }

    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, true, strFailCallBackFunc, "", true);
}

function fnOrderDetailSuccResult(objRes) {

    if (objRes) {
        if (objRes[0].result.ErrorCode !== 0) {
            fnCallDetailFailResult();
            return false;
        }

        if (objRes[0].data.RecordCnt !== 1) {
            fnCallDetailFailResult();
            return false;
        }

        var item = objRes[0].data.List[0];

        //Hidden
        $.each($("input[type='hidden']"),
            function (index, input) {
                if (eval("item." + $(input).attr("id")) != null) {
                    $("#" + $(input).attr("id")).val(eval("item." + $(input).attr("id")));
                }
            });

        //Textbox
        $.each($("input[type='text'], input[type='email']"),
            function (index, input) {
                if (eval("item." + $(input).attr("id")) != null) {
                    if ($(input).attr("id").indexOf("YMD") > -1) {
                        if (eval("item." + $(input).attr("id")).length == 8) {
                            $("#" + $(input).attr("id")).val(fnGetStrDateFormat(eval("item." + $(input).attr("id")), "-"));
                        } else {
                            $("#" + $(input).attr("id")).val(eval("item." + $(input).attr("id")));
                        }
                    } else {
                        $("#" + $(input).attr("id")).val(eval("item." + $(input).attr("id")));
                    }
                }
            });



        //Textarea
        $.each($("textarea"),
            function (index, input) {
                if (eval("item." + $(input).attr("id")) != null) {
                    $("#" + $(input).attr("id")).val(eval("item." + $(input).attr("id")));
                }
            });

        //Select

        $.each($("select"),
            function (index, input) {
                if (eval("item." + $(input).attr("id")) != null) {
                    if ($("#" + $(input).attr("id") + " option[value='" + eval("item." + $(input).attr("id")) + "']").length > 0) {
                        $("#" + $(input).attr("id")).val(eval("item." + $(input).attr("id")));
                    }
                }
            });
    }
    else {
        fnCallDetailFailResult();
    }
}

function fnCallDetailFailResult() {
    fnDefaultAlert("데이터를 불러오는데 실패했습니다. 잠시 후 시도해 주세요.", "error", "window.close();");
}
//정보관리 상세조회 끝----------------------------------------------------------------------------------------------------------
