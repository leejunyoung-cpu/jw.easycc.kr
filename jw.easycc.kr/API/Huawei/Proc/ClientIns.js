var ClientLogoFile;
var allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;

$(document).ready(function () {
    if ($("#HidMode").val() === "Update") {
        $("#lblMode").text("수정");
        if ($("#HidFilePath").val() !== "") {
            $("#ClientLogoFile").hide();
            $("#BtnLogoDel").show();
        }
    } else {
        $("#lblMode").text("등록");
        $("#ClientLogoFilePath").hide();
    }

    fnFileUploadSet();
});

function fnFileUploadSet() {
    var maxSize = 10 * 1024 * 1024; // 10MB (10 * 1024 * 1024 bytes)
    $("#ClientLogoFile").on("change", function (e) {
        var file = $("#ClientLogoFile")[0].files[0];
        
        if (!allowedExtensions.test(file.name)) {
            fnDefaultAlert("이미지 파일만 업로드 가능합니다! (jpg, jpeg, png)", "warning");
            $("#ClientLogoFile").val(""); // 파일 선택 초기화
            return;
        }

        if (file.size > maxSize) {
            fnDefaultAlert("파일 크기가 10MB를 초과할 수 없습니다!<br>(현재 파일 크기: " + (file.size / (1024 * 1024)).toFixed(2) + "MB)" + ")", "warning");
            $("#ClientLogoFile").val(""); // 파일 선택 초기화
            return;
        }

        var formData = new FormData();
        formData.append('file', file);

        $.ajax({
            url: "/SSO/Client/Proc/ClientFileUploadHandler.ashx?CallType=ClientFileUpload",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            beforeSend: function () {
                $("#divLoadingImage").show();
            },
            success: function (objRes) {
                if (objRes[0].RetCode === 0) {
                    $("#BtnLogoDel").show();
                    $("#ClientLogoFile").css("pointer-events", "none").css("background-color", "#e9ecef");
                    $("#HidFilePath").val(objRes[0].FilePath);
                }
            },
            error: function () {
                fnDefaultAlert("업로드 중 오류가 발생했습니다.");
                return;
            },
            complete: function () {
                // 요청이 완료되면 로딩 숨기기
                $("#divLoadingImage").hide();
            }
        });
    });

}

function fnLogoUploadDelConfirm() {
    fnDefaultConfirm("파일을 삭제하시겠습니까?", "fnLogoUploadDel", "");
}
function fnLogoUploadDel() {

    var strHandlerURL = "/SSO/Client/Proc/ClientFileUploadHandler.ashx";
    var strCallBackFunc = "fnAjaxLogoDelSuccess";

    var objParam = {
        CallType: "ClientFileDel",
        HidFilePath: encodeURIComponent($("#HidFilePath").val())
    };
    
    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, true, "", "", true);
}

function fnAjaxLogoDelSuccess(objRes) {
    if (objRes) {
        if (objRes[0].RetCode === 0) {
            $("#HidFilePath").val("");
            $("#ClientLogoFile").val("");
            $("#ClientLogoFile").css("pointer-events", "").css("background-color", "");
            $("#BtnLogoDel").hide();
            $("#ClientLogoFile").show();
            $("#ClientLogoFilePath").attr("src", "");

            if ($("#HidMode").val() !== "Update") {
                fnDefaultAlert(objRes[0].ErrMsg, "success");
            } else {
                var objFnParam = { CallType: "ClientUpdate" };
                fnInsClientProc(objFnParam);
            }
        }
    }
}

function fnChkCorpNo() {
    var strHandlerURL = "/SSO/Client/Proc/ClientHandler.ashx";
    var strCallBackFunc = "fnAjaxCorpChk";

    if ($("#CorpNo").val() === "") {
        fnDefaultAlertFocus("사업자번호 입력해주세요.", "CorpNo");
        return;
    }

    var objParam = {
        CallType: "ClientList",
        CorpNo: $("#CorpNo").val()
    };

    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, true, "", "", true);

}

function fnAjaxCorpChk(objData) {
    if (objData) {
        if (objData[0].data.RecordCnt === 1) {
            $("#CorpNoBtn").show();
            $("#CorpNoBtnReturn").hide();
            $(".id_fail").show();
            $(".id_pass").hide();
            $("#CorpCheck").val("");
            return;
        } else if (objData[0].data.RecordCnt > 1) {
            fnDefaultAlert("중복으로 등록된 고객사가 존재합니다.");
            return;
        } else if (objData[0].data.RecordCnt === 0) {
            $("#CorpNoBtn").hide();
            $("#CorpNoBtnReturn").show();
            $(".id_pass").show();
            $(".id_fail").hide();
            $("#CorpCheck").val("Y");
            $("#CorpNo").attr("readonly", true);
        }
    }
}

function fnClientReset() {
    $("#CorpNoBtn").show();
    $("#CorpNoBtnReturn").hide();
    $(".id_fail").hide();
    $(".id_pass").hide();
    $("#CorpCheck").val("");
    $("#CorpNo").val("");
    $("#CorpNo").focus();
    $("#CorpNo").attr("readonly", false);
}

function fnInsClient() {
    var strConfMsg = "";
    var strCallType;

    if ($("#CorpNo").val() === "") {
        fnDefaultAlertFocus("사업자번호 입력은 필수입니다.", "CorpNo");
        return;
    }
    if ($("#SiteID").val() === "") {
        fnDefaultAlertFocus("고객사 고유번호 입력은 필수입니다.", "SiteID");
        return;
    }
    if ($("#CorpCheck").val() !== "Y") {
        fnDefaultAlert("사업자 번호 확인이 되지 않았습니다.");
        returnl;
    }
    if ($("#ClientName").val() === "") {
        fnDefaultAlertFocus("고객사명 입력은 필수입니다.", "ClientName");
        return;
    }
    if ($("#ClientCeoName").val() === "") {
        fnDefaultAlertFocus("대표자명 입력은 필수입니다.", "ClientCeoName");
        return;
    }
    if ($("#Email").val()) {
        if (!UTILJS.Util.fnValidEmail($("#Email").val())) {
            fnDefaultAlertFocus("이메일이 올바르지 않습니다.", "Email", "warning");
            return;
        }
    }
    if (!$("#BtoBYN").prop("checked") && !$("#BtoCYN").prop("checked")) {
        fnDefaultAlert("서비스 유형을 체크해주세요.(BtoB 또는 BtoC)", "warning");
        return;
    }
    if ($("#ClientType").val() === "") {
        fnDefaultAlertFocus("고객사 유형은 필수입니다.", "ClientType");
        return;
    }
    if ($("#EncomFlag").val() === "") {
        fnDefaultAlertFocus("엔컴연동 여부 선택은 필수입니다.", "EncomFlag");
        return;
    }
    if ($("#UseFlag").val() === "") {
        fnDefaultAlertFocus("사용여부 선택은 필수입니다.", "UseFlag");
        return;
    }

    if ($("#HidMode").val() === "Update") {
        strCallType = "ClientUpdate";
        strConfMsg = "수정하시겠습니까?";
    }
    else {
        strCallType = "ClientInsert";
        strConfMsg = "등록하시겠습니까?";
    }

    //Confirm
    var objFnParam = { CallType: strCallType };
    fnDefaultConfirm(strConfMsg, "fnInsClientProc", objFnParam);

    return;
}

function fnInsClientProc(objParam) {
    var strHandlerURL = "/SSO/Client/Proc/ClientHandler.ashx";
    var strCallBackFunc = "fnAjaxInsClient";

    var objParam = {
        CallType: objParam.CallType,
        ClientCode: $("#ClientCode").val(),
        CorpNo: $("#CorpNo").val(),
        SiteID: $("#SiteID").val(),
        ClientName: $("#ClientName").val(),
        ClientCeoName: $("#ClientCeoName").val(),
        ClientTel: $("#ClientTel").val(),
        Email: $("#Email").val(),
        Addr: $("#Addr").val(),
        ClientType: $("#ClientType").val(),
        EncomFlag: $("#EncomFlag").val(),
        BtoBYN: $("#BtoBYN").prop("checked") ? "Y" : "N",
        BtoCYN: $("#BtoCYN").prop("checked") ? "Y" : "N",
        HidFilePath: encodeURIComponent($("#HidFilePath").val()),
        UseFlag: $("#UseFlag").val(),
        ClientServerIP: $("#ClientServerIP").val()
    };
    
    UTILJS.Ajax.fnHandlerRequest(objParam, strHandlerURL, strCallBackFunc, true, "", "", true);
}

function fnAjaxInsClient(objData) {
    if (objData[0].RetCode !== 0) {
        fnDefaultAlert("나중에 다시 시도해 주세요.(" + objData[0].ErrMsg + ")");
        return;
    } else {
        fnDefaultAlert("저장되었습니다.", "success", "fnAjaxInsClientComplete", { ClientCode: objData[0].ClientCode });
        parent.fnCallGridData("#ClientListGrid");
        return;
    }
}

function fnAjaxInsClientComplete(objParam) {

    if (typeof objParam.ClientCode === "undefined") {
        return false;
    }

    document.location.replace("/SSO/Client/ClientIns?HidMode=Update&ClientCode=" + objParam.ClientCode);
}