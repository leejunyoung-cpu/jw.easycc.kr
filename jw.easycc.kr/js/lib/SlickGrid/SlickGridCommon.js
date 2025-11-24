function fnGridExcelDownload(strtitle, objGridData, objColumns, footerData) {
    // 새 워크북 생성
    var workbook = new ExcelJS.Workbook();
    var worksheet = workbook.addWorksheet('Sheet1');

    // ▶ 보이는 컬럼만 사용
    var visibleCols = (objColumns || []).filter(col => col && col.visible !== false);

    // 헤더/키 매핑
    worksheet.columns = visibleCols.map(col => ({
        header: col.name,
        key: col.field || col.id,   // field 우선
        width: 20
    }));

    // HTML 제거 util (링크/태그 formatter 대비)
    function stripHtml(s) {
        if (s == null) return '';
        if (typeof s !== 'string') return s;
        return s
            .replace(/<br\s*\/?>/gi, '\n')
            .replace(/<[^>]*>/g, '')
            .replace(/\u00A0/g, ' ')
            .trim();
    }

    // ▶ 데이터 추가 (formatter 우선 적용)
    objGridData.forEach(function (item, rowIdx) {
        var rowOut = {};
        visibleCols.forEach(function (col, colIdx) {
            var val = item[col.field];

            if (typeof col.exporter === 'function') {
                // (선택) 별도 exporter 제공 시 그 값 사용
                val = col.exporter(item, rowIdx, col);
            } else if (typeof col.formatter === 'function') {
                // Slick formatter 시그니처: (row, cell, value, columnDef, dataContext)
                try {
                    var formatted = col.formatter(rowIdx, colIdx, val, col, item);
                    if (formatted != null) {
                        if (typeof formatted === 'string') {
                            val = stripHtml(formatted);
                        } else if (typeof formatted === 'object') {
                            // 혹시 객체 형태 반환 시 텍스트 추출 시도
                            if (formatted.text) val = stripHtml(formatted.text);
                            else if (formatted.html) val = stripHtml(formatted.html);
                            else val = stripHtml(String(formatted));
                        }
                    }
                } catch (e) {
                    // formatter 오류 시 원본 값 사용
                }
            }

            rowOut[col.field || col.id] = val;
        });
        worksheet.addRow(rowOut);
    });

    // ▶ 푸터 추가 (visibleCols 기준으로만 매핑)
    if (footerData) {
        const footerRowData = {};
        visibleCols.forEach((col, idx) => {
            if (Array.isArray(footerData)) {
                footerRowData[col.field || col.id] = footerData[idx] || '';
            } else {
                footerRowData[col.field || col.id] =
                    (footerData[col.field] != null ? footerData[col.field] :
                        footerData[col.id] != null ? footerData[col.id] : '');
            }
        });

        const footerRow = worksheet.addRow(footerRowData);
        footerRow.eachCell((cell) => {
            cell.font = { bold: true, color: { argb: 'FF0000FF' }, size: 11 };
            cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFE7E6E6' } };
            cell.alignment = { vertical: 'middle', horizontal: 'center' };
        });
    }

    // 헤더 스타일
    worksheet.getRow(1).eachCell((cell) => {
        cell.font = { bold: true, size: 10 };
        cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFFFCC00' } };
        cell.alignment = { vertical: 'middle', horizontal: 'center' };
    });

    // 데이터 셀 기본 정렬
    worksheet.eachRow({ includeEmpty: false }, function (row) {
        row.eachCell({ includeEmpty: false }, function (cell) {
            cell.font = { size: 10 };
            cell.alignment = { vertical: 'middle', horizontal: 'center' };
        });
    });

    // 다운로드
    workbook.xlsx.writeBuffer().then(function (buffer) {
        var blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        var blobUrl = URL.createObjectURL(blob);
        var a = document.createElement('a');
        a.href = blobUrl;
        a.download = strtitle + '.xlsx';
        document.body.appendChild(a);
        a.click();
        setTimeout(function () {
            URL.revokeObjectURL(blobUrl);
            document.body.removeChild(a);
        }, 0);
    });
}

/*function fnGridExcelDownload(strtitle, objGridData, objColumns) {
    // 새 워크북 생성
    var workbook = new ExcelJS.Workbook();
    var worksheet = workbook.addWorksheet('Sheet1');

    

    worksheet.columns = objColumns.filter(col => col.visible).map(col => ({
        header: col.name,
        key: col.field,
        width: 20
    }));

    // 데이터 추가
    objGridData.forEach(item => {
        worksheet.addRow(item);
    });

    // 헤더 스타일 설정
    worksheet.getRow(1).eachCell((cell) => {
        cell.font = { bold: true, size : 10 };
        cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFFFCC00' }
        };
        cell.alignment = { vertical: 'middle', horizontal: 'center' }; // 헤더 셀 가운데 정렬
    });

    // 데이터 셀 가운데 정렬
    worksheet.eachRow({ includeEmpty: false }, function (row, rowNumber) {
        row.eachCell({ includeEmpty: false }, function (cell, colNumber) {
            cell.font = {size: 10 };
            cell.alignment = { vertical: 'middle', horizontal: 'center' };
        });
    });

    // 엑셀 파일 Blob 생성
    workbook.xlsx.writeBuffer().then(function (buffer) {
        var blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

        // Blob URL 생성
        var blobUrl = URL.createObjectURL(blob);

        // 다운로드 링크 생성
        var a = document.createElement('a');
        a.href = blobUrl;
        a.download = strtitle + '.xlsx';
        document.body.appendChild(a);
        a.click();

        // 다운로드 후 URL 해제
        setTimeout(function () {
            URL.revokeObjectURL(blobUrl);
            document.body.removeChild(a);
        }, 0);
    });
}
*/
