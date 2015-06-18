/**
* EasyUI DataGrid根据字段动态合并单元格
* 参数 tableID 要合并table的id
* 参数 colList 要合并的列,用逗号分隔(例如："name,department,office");
*/
function mergeCellsByField(tableID, colList) {
    var ColArray = colList.split(",");
    var tTable = $("#" + tableID);
    var TableRowCnts = tTable.datagrid("getRows").length;
    var tmpA;
    var tmpB;
    var PerTxt = "";
    var CurTxt = "";
    var alertStr = "";
    for (j = ColArray.length - 1; j >= 0; j--) {
        PerTxt = "";
        tmpA = 1;
        tmpB = 0;

        for (i = 0; i <= TableRowCnts; i++) {
            if (i == TableRowCnts) {
                CurTxt = "";
            }
            else {
                CurTxt = tTable.datagrid("getRows")[i][ColArray[j]];
            }
            if (PerTxt == CurTxt) {
                tmpA += 1;
            }
            else {
                tmpB += tmpA;

                tTable.datagrid("mergeCells", {
                    index: i - tmpA,
                    field: ColArray[j],　　//合并字段
                    rowspan: tmpA,
                    colspan: null
                });
                tTable.datagrid("mergeCells", { //根据ColArray[j]进行合并
                    index: i - tmpA,
                    field: "Ideparture",
                    rowspan: tmpA,
                    colspan: null
                });

                tmpA = 1;
            }
            PerTxt = CurTxt;
        }
    }
}



/** 
* 在iframe中调用，在父窗口中出提示框(herf方式不用调父窗口)
*/
$.extend({ show_warning: function (strTitle, strMsg) {
    $.messager.show({
        title: strTitle,
        width: 300,
        height: 100,
        msg: strMsg,
        closable: true,
        showType: 'slide',
        style: {
            right: '',
            top: document.body.scrollTop + document.documentElement.scrollTop,
            bottom: ''
        }
    });
}
});

/** 
* 弹框
*/
$.extend({ show_alert: function (strTitle, strMsg) {
    $.messager.alert(strTitle, strMsg);
}
});

/**
* @author 孙宇
* 
* @requires jQuery,EasyUI
* 
* 扩展validatebox，添加验证两次密码功能
*/
$.extend($.fn.validatebox.defaults.rules, {
    eqPwd: {
        validator: function (value, param) {
            return value == $(param[0]).val();
        },
        message: '密码不一致！'
    }
});

/**
* @author 风骑士
* 
* @requires jQuery,EasyUI
* 
* 初始化datagrid toolbar
*/
getToolBar = function (data) {
    if (data.toolbar != undefined && data.toolbar != '') {
        var toolbar = [];
        $.each(data.toolbar, function (index, row) {
            var handler = row.handler;
            row.handler = function () { eval(handler); };
            toolbar.push(row);
        });
        return toolbar;
    } else {
        return [];
    }
}

/**
* @author 孙宇
*
* 接收一个以逗号分割的字符串，返回List，list里每一项都是一个字符串（做编辑功能的时候 传入id 然后自动勾选combo系列组件）
*
* @returns list
*/
stringToList = function (value) {
    if (value != undefined && value != '') {
        var values = [];
        var t = value.split(',');
        for (var i = 0; i < t.length; i++) {
            values.push('' + t[i]); /* 避免将ID当成数字 */
        }
        return values;
    } else {
        return [];
    }
};


/**
* @author ss
*
* 获取当前日期
*
* @returns list
*/
function CurentTime() {
    var now = new Date();

    var year = now.getFullYear();       //年
    var month = now.getMonth()+1;     //月
    var day = now.getDate();            //日

    var hh = now.getHours();            //时
    var mm = now.getMinutes();          //分
    var ss = now.getSeconds();          //秒

    var clock = year + "-";

    if (month < 10)
        clock += "0";

    clock += month + "-";

    if (day < 10)
        clock += "0";

    clock += day + " ";

    if (hh < 10)
        clock += "0";

    clock += hh + ":";
    if (mm < 10) clock += '0';
    clock += mm+":";
    if (ss < 10) clock += '0';
    clock += ss;
    
        return clock;
}

//转yyyy-mm-dd hh：mm：ss 为yyyymmdd
function CheckDateTime(str) {
    var reg = /^(\d+)-(\d{ 1,2 })-(\d{ 1,2 }) (\d{ 1,2 }):(\d{ 1,2 }):(\d{ 1,2 })$/;
    var r = str.match(reg);
    if (r == null) return null;
    var date = r[1] + r[2] + r[3];
    return date;
}

//日期格式化  
    function formatDate(date){  
        var month = date.getMonth()+1;  
        if( "" != date ){  
            if( date.getMonth() +1 < 10 ){  
                month = '0' + (date.getMonth() +1);  
            }  
            var day = date.getDate();  
            if( date.getDate() < 10 ){  
                day = '0' + date.getDate();  
            }  
           // return date.getFullYear()+'-'+month+'-'+day+" "+date.getHours()+":00:00";//将2011-01-01  17：20：08 这种格式转换为2011-01-01  17：00：00  
            return date.getFullYear() + month + day;
        } else {
            return "";  
        }  
    }  
    

//导出excel
    function ChangeToTable(printDatagrid) {
        var tableString = '<table cellspacing="0" class="pb">';
        var frozenColumns = printDatagrid.datagrid("options").frozenColumns;  // 得到frozenColumns对象
        var columns = printDatagrid.datagrid("options").columns;    // 得到columns对象
        var nameList = new Array();

        // 载入title
        if (typeof columns != 'undefined' && columns != '') {
            $(columns).each(function (index) {
                tableString += '\n<tr>';
                if (typeof frozenColumns != 'undefined' && typeof frozenColumns[index] != 'undefined') {
                    for (var i = 0; i < frozenColumns[index].length; ++i) {
                        if (!frozenColumns[index][i].hidden) {
                            tableString += '\n<th width="' + frozenColumns[index][i].width + '"';
                            if (typeof frozenColumns[index][i].rowspan != 'undefined' && frozenColumns[index][i].rowspan > 1) {
                                tableString += ' rowspan="' + frozenColumns[index][i].rowspan + '"';
                            }
                            if (typeof frozenColumns[index][i].colspan != 'undefined' && frozenColumns[index][i].colspan > 1) {
                                tableString += ' colspan="' + frozenColumns[index][i].colspan + '"';
                            }
                            if (typeof frozenColumns[index][i].field != 'undefined' && frozenColumns[index][i].field != '') {
                                nameList.push(frozenColumns[index][i]);
                            }
                            tableString += '>' + frozenColumns[0][i].title + '</th>';
                        }
                    }
                }
                for (var i = 0; i < columns[index].length; ++i) {
                    if (!columns[index][i].hidden) {
                        tableString += '\n<th width="' + columns[index][i].width + '"';
                        if (typeof columns[index][i].rowspan != 'undefined' && columns[index][i].rowspan > 1) {
                            tableString += ' rowspan="' + columns[index][i].rowspan + '"';
                        }
                        if (typeof columns[index][i].colspan != 'undefined' && columns[index][i].colspan > 1) {
                            tableString += ' colspan="' + columns[index][i].colspan + '"';
                        }
                        if (typeof columns[index][i].field != 'undefined' && columns[index][i].field != '') {
                            nameList.push(columns[index][i]);
                        }
                        tableString += '>' + columns[index][i].title + '</th>';
                    }
                }
                tableString += '\n</tr>';
            });
        }
        // 载入内容
        var rows = printDatagrid.datagrid("getRows"); // 这段代码是获取当前页的所有行
        for (var i = 0; i < rows.length; ++i) {
            tableString += '\n<tr>';
            for (var j = 0; j < nameList.length; ++j) {
                var e = nameList[j].field.lastIndexOf('_0');

                tableString += '\n<td';
                if (nameList[j].align != 'undefined' && nameList[j].align != '') {
                    tableString += ' style="text-align:' + nameList[j].align + ';"';
                }
                tableString += '>';
                if (e + 2 == nameList[j].field.length) {
                    tableString += rows[i][nameList[j].field.substring(0, e)];
                }
                else
                    tableString += rows[i][nameList[j].field];
                tableString += '</td>';
            }
            tableString += '\n</tr>';
        }
        tableString += '\n</table>';
        return tableString;
    }

    function Export(strXlsName, exportGrid) {
        var f = $('<form action="html/export_excel.aspx" method="post" id="fm1"></form>');
        var i = $('<input type="hidden" id="txtContent" name="txtContent" />');
        var l = $('<input type="hidden" id="txtName" name="txtName" />');
        i.val(ChangeToTable(exportGrid));
        i.appendTo(f);
        l.val(strXlsName);
        l.appendTo(f);
        f.appendTo(document.body).submit();
        document.body.removeChild(f);
    }
    