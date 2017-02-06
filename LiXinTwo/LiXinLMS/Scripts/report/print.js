//print
//function reportPrint1(arg) {
//    openPopWindow({ content: "<div class='ma w800 ovy fs14' id='pindiv'></div>", title: "<span class='reportPrint' title='打印' onclick='pintreport(\"pindiv\");'></span>", width: 850, height: 500 });
//    if (arg.pTitle != null && arg.pTitle != undefined && arg.pTitle != "")
//        $("#pindiv").append('<p class="line_h24 tac mt10 pTitle">' + arg.pTitle + '</p>');
//    if (arg.pContainer != null && arg.pContainer != undefined && arg.pContainer != "")
//        $("#pindiv").append($("#" + arg.pContainer).html());
//    if (arg.dataTitle != null && arg.dataTitle != undefined && arg.dataTitle != "")
//        $("#pindiv").append('<p class="line_h24 tac mt20 dataTitle">' + arg.dataTitle + '</p>');
//    if (arg.dataContainer != null && arg.dataContainer != undefined && arg.dataContainer != "")
//        $("#pindiv").append($("#" + arg.dataContainer).html());
//    $("#pindiv .ovh,#pindiv .tc").removeClass("ovh").removeClass("tc");
//    $("#pindiv td").css({"padding":"13px 0px"});
//}

function reportPrint(arg) {
    if ($("#pindiv").length == 0) {
        $("body").append('<div class="fs14" id="pindiv"></div>');
    } else {
        $("#pindiv").html("");
    }
    $("#pindiv").css({ "height": "auto", "margin": "0 auto", "overflow": "hidden", "width": "1002px" });
    $("#top,#header,#main,#mainFoot,#btnBrowse,.mtaba,#informain").hide();
    $("#pindiv").append("<div id='operation' class='curp'>&nbsp;<div id='closediv' style='padding:10px 0px;'>" +
        "<dl class='fll cc70'>" +
        "<dt>温馨提示：</dt>" +
        "<dd>1、为了获得更好的打印效果，打印时关闭此处提示；</dd>" +
        "<dd>2、使用浏览器打印预览，预览效果没有问题，即可打印；</dd>" +
        "<dd>3、关闭此处提示后，若要返回，请将鼠标放在此处。</dd>" +
        "</dl><a class='flr curp' onclick='printback();'>返回</a><a class='flr curp mr10' onclick='closemessage(this);'>关闭</a><div class='cb'></div></div></div>");
    if (arg.pTitle != null && arg.pTitle != undefined && arg.pTitle != "")
        $("#pindiv").append('<p class="line_h24 tac mt10 pTitle">' + arg.pTitle + '</p>');
    if (arg.pContainer != null && arg.pContainer != undefined && arg.pContainer != "")
        $("#pindiv").append($("#" + arg.pContainer).html());
    if (arg.dataTitle != null && arg.dataTitle != undefined && arg.dataTitle != "")
        $("#pindiv").append('<p class="line_h24 tac mt20 dataTitle">' + arg.dataTitle + '</p>');
    if (arg.dataContainer != null && arg.dataContainer != undefined && arg.dataContainer != "")
        $("#pindiv").append($("#" + arg.dataContainer).html());
    $("#pindiv .ovh,#pindiv .tc").removeClass("ovh").removeClass("tc");
    $("#pindiv td").css({ "padding": "13px 0px" });

    $(".highcharts-container").addClass("ma");

    //remove the colum Of operations
    if ($("#pindiv table  .icon").length > 0) {
        $("#pindiv table thead tr th:last").remove();
        $("#pindiv table tbody tr").each(function() {
            $(this).find("td:last").remove();
        });
        $("#pindiv table thead tr th").css({ "width": ((1002 / $("#pindiv table thead tr th").length) + "px") });
    }
}

//关闭提示

function closemessage(obj) {
    $(obj).hide();
    $("#operation").bind("mouseover", function() {
        $(this).find("#closediv").show();
    }).bind("mouseout", function() {
        $(this).find("#closediv").hide();
    });
    $(this).find("#closediv").hide();
}

//返回

function printback() {
    $("#top,#header,#main,#mainFoot,#btnBrowse,.mtaba,#informain").show();
    $("#pindiv").html("");
}

//打印

function pintreport(objID) {
    try {
        $("#operation").hide();
        $("#" + objID).printArea();
    } catch(e) {
        $("#operation").show();
    }
}

///
//向悬浮框添加筛选条件
///
//containerID:筛选的对象容器的ID
//selectType:筛选的对象的类型（单选：single；多选：multiple）
//objType:筛选的对象的类型包括（'condition_exam':考试；'condition_course':课程；'condition_domain':区域；'condition_store':店铺；'condition_person':人员；"condition_train":培训）
///

function insertQueryCondition(containerID, selectType, objType) {
    if ($("#browseSelectCondition")) {
        var title = '';
        switch (objType) {
        case 'condition_course':
            title = '所选课程';
            break;
        case 'condition_domain':
            title = '所选区域';
            break;
        case 'condition_store':
            title = '所选店铺';
            break;
        case 'condition_person':
            title = '所选人员';
            break;
        case 'condition_train':
            title = '所选培训';
            break;
        case 'condition_exam':
            title = '所选考试';
            break;
        }

        if ($("#browseSelectCondition #" + objType).length == 0) {
            $("#browseSelectCondition").append('<div id="' + objType + '" class="' + objType + '">' +
                '<div class="conditionTitle pd10 bgcf06 cff"><div class="w100 fll">' + title + '</div><a class="curp flr icon shousuo" href="#" onclick="conditionUpAndDown(this);"></a><a class="curp flr cff" href="#" onclick="deleteAllQueryCondition(\'' + objType + '\',\'' + containerID + '\',\'' + selectType + '\');">撤销全部</a><div class="cb"></div></div>' +
                '<ul class="conditionul ovy transparent" style="display:block; height:250px;">' +
                '</ul>' +
                '</div>');
        }

        //培训的情况
        if (objType == "condition_exam") {
            if (selectType == "single") {
                var strHtml = '';
                $("#" + containerID + " input[type='radio']").each(function() {
                    if ($(this).attr("checked")) {
                        strHtml += '<li id="' + $(this).attr("id") + '"><div class="ovh w180 fll" title="' + $(this).attr("value") + '">' + $(this).attr("value") + '</div><a class="flr red curp" onclick="deleteConditionbyID(\'' + $(this).attr("id") + '\',\'' + objType + '\',\'' + containerID + '\',\'' + selectType + '\');">x</a><div class="cb"></div></li>';
                    }
                });
                $("#browseSelectCondition #condition_exam ul").eq(0).html("").append(strHtml);
            } else {
                var strHtml = '';
                $("#" + containerID + " input[type='checkbox']").each(function() {
                    if ($(this).attr("checked")) {
                        strHtml += '<li id="' + $(this).attr("id") + '"><div class="ovh w180 fll" title="' + $(this).attr("value") + '">' + $(this).attr("value") + '</div><a class="flr red curp" onclick="deleteConditionbyID(\'' + $(this).attr("id") + '\',\'' + objType + '\',\'' + containerID + '\',\'' + selectType + '\');">x</a><div class="cb"></div></li>';
                    }
                });
                $("#browseSelectCondition #condition_exam ul").eq(0).html("").append(strHtml);
            }
        }

        //培训的情况
        if (objType == "condition_train") {
            if (selectType == "single") {
                var strHtml = '';
                $("#" + containerID + " input[type='radio']").each(function() {
                    if ($(this).attr("checked")) {
                        strHtml += '<li id="' + $(this).attr("id") + '"><div class="ovh w180 fll" title="' + $(this).attr("value") + '">' + $(this).attr("value") + '</div><a class="flr red curp" onclick="deleteConditionbyID(\'' + $(this).attr("id") + '\',\'' + objType + '\',\'' + containerID + '\',\'' + selectType + '\');">x</a><div class="cb"></div></li>';
                    }
                });
                $("#browseSelectCondition #condition_train ul").eq(0).html("").append(strHtml);
            } else {
                var strHtml = '';
                $("#" + containerID + " input[type='checkbox']").each(function() {
                    if ($(this).attr("checked")) {
                        strHtml += '<li id="' + $(this).attr("id") + '"><div class="ovh w180 fll" title="' + $(this).attr("value") + '">' + $(this).attr("value") + '</div><a class="flr red curp" onclick="deleteConditionbyID(\'' + $(this).attr("id") + '\',\'' + objType + '\',\'' + containerID + '\',\'' + selectType + '\');">x</a><div class="cb"></div></li>';
                    }
                });
                $("#browseSelectCondition #condition_train ul").eq(0).html("").append(strHtml);
            }
        }

        //人员的情况
        if (objType == "condition_person") {
            if (selectType == "single") {
                var strHtml = '';
                $("#userList tbody tr").each(function() {
                    var o = $(this).find("input[type='radio']").eq(0);
                    if ($(o).attr("checked") && !$(o).attr("disabled")) {
                        strHtml += '<li id="' + $(o).attr("id") + '"><div class="ovh w180 fll" title="' + $(o).attr("username") + '">' + $(o).attr("username") + '</div><a class="flr red curp" onclick="deleteConditionbyID(\'' + $(o).attr("id") + '\',\'' + objType + '\',\'' + containerID + '\',\'' + selectType + '\');">x</a><div class="cb"></div></li>';
                    }
                });
                $("#browseSelectCondition #condition_person ul").eq(0).html("").append(strHtml);
            } else {
                var strHtml = '';
                $("#userList tbody tr").each(function() {
                    var o = $(this).find("input[type='checkbox']").eq(0);
                    if ($(o).attr("checked") && !$(o).attr("disabled")) {
                        strHtml += '<li id="' + $(o).attr("id") + '"><div class="ovh w180 fll" title="' + $(o).attr("username") + '">' + $(o).attr("username") + '</div><a class="flr red curp" onclick="deleteConditionbyID(\'' + $(o).attr("id") + '\',\'' + objType + '\',\'' + containerID + '\',\'' + selectType + '\');">x</a><div class="cb"></div></li>';
                    }
                });
                $("#browseSelectCondition #condition_person ul").eq(0).append(strHtml);
            }
        }

        //店铺的情况
        if (objType == "condition_store") {
            var strHtml = '';
            $("#" + containerID + " a").each(function() {
                if ($(this).attr("class").indexOf("commonSelect") >= 0) {
                    strHtml += '<li id="' + $(this).attr("id") + '"><div class="ovh w180 fll" title="' + $(this).html() + '">' + $(this).html() + '</div><a class="flr red curp" onclick="deleteConditionbyID(\'' + $(this).attr("id") + '\',\'' + objType + '\',\'' + containerID + '\',\'' + selectType + '\');">x</a><div class="cb"></div></li>';
                }
            });
            $("#browseSelectCondition #condition_store ul").eq(0).html("").append(strHtml);
        }

        //区域的情况
        if (objType == "condition_domain") {
            var strHtml = '';
            $("#" + containerID + " a").each(function() {
                if ($(this).attr("class").indexOf("commonSelect") >= 0) {
                    strHtml += '<li id="' + $(this).attr("id") + '"><div class="ovh w180 fll" title="' + $(this).html() + '">' + $(this).html() + '</div><a class="flr red curp" onclick="deleteConditionbyID(\'' + $(this).attr("id") + '\',\'' + objType + '\',\'' + containerID + '\',\'' + selectType + '\');">x</a><div class="cb"></div></li>';
                }
            });
            $("#browseSelectCondition #condition_domain ul").eq(0).html("").append(strHtml);
        }

        //课程的情况
        if (objType == "condition_course") {
            if (selectType == "single") {
                var strHtml = '';
                $("#CourseContent tbody tr").each(function() {
                    var o = $(this).find("input[type='radio']").eq(0);
                    if ($(o).attr("checked") && !$(o).attr("disabled")) {
                        strHtml += '<li id="' + $(o).attr("courseID") + '"><div class="ovh w180 fll" title="' + $(o).attr("value") + '">' + $(o).attr("value") + '</div><a class="flr red curp" onclick="deleteConditionbyID(\'' + $(o).attr("courseID") + '\',\'' + objType + '\',\'' + containerID + '\',\'' + selectType + '\');">x</a><div class="cb"></div></li>';
                    }
                });
                $("#browseSelectCondition #condition_course ul").eq(0).html("").append(strHtml);
            } else {
                var strHtml = '';
                //获取已选择的课程
                var selectedCourses = GetStoreQueryCondition("condition_course");
                $("#CourseContent tbody tr").each(function() {
                    var o = $(this).find("input[type='checkbox']").eq(0);
                    if ($(o).attr("checked") && !$(o).attr("disabled") && (("," + selectedCourses + ",").indexOf("," + $(o).attr("courseID") + ",") < 0)) {
                        strHtml += '<li id="' + $(o).attr("courseID") + '"><div class="ovh w180 fll" title="' + $(o).attr("value") + '">' + $(o).attr("value") + '</div><a class="flr red curp" onclick="deleteConditionbyID(\'' + $(o).attr("courseID") + '\',\'' + objType + '\',\'' + containerID + '\',\'' + selectType + '\');">x</a><div class="cb"></div></li>';
                        selectedCourses += selectedCourses == "" ? ($(o).attr("courseID")) : ("," + $(o).attr("courseID"));
                    }
                });
                $("#browseSelectCondition #condition_course ul").eq(0).append(strHtml);
            }
        }

        if ($("#" + objType).length > 0) {
            if ($("#" + objType + " li").length == 0) {
                $("#" + objType).eq(0).remove();
            }
        }
    }
}

//撤销全部
//objType:筛选的对象的类型包括（'condition_exam':考试；'condition_course':课程；'condition_domain':区域；'condition_store':店铺；'condition_person':人员；"condition_train":培训）
//containerID:需要同步更新的容器的ID
//selectType:筛选的对象的类型（单选：single；多选：multiple）

function deleteAllQueryCondition(objType, containerID, selectType) {
    if ($("#browseSelectCondition #" + objType).length > 0) {
        $("#browseSelectCondition #" + objType).eq(0).remove();
    }
    //培训
    if (objType == "condition_train" || objType == "condition_exam") {
        $("#" + containerID + " input").each(function() {
            $(this).attr("checked", false);
        });
    }
    //区域、店铺
    if (objType == "condition_domain" || objType == "condition_store") {
        $("#" + containerID).find("a").removeClass("commonSelect");
    }
    if ($("#browseSelectCondition").text().length <= 0) {
        $("#browseSelectCondition").hide();
    }
}

//收起或伸缩

function conditionUpAndDown(obj) {
    if ($(obj).parent().parent().find("ul").eq(0).css("display") == "none") {
        $(obj).parent().parent().find("ul").eq(0).css("display", "block");
        $(obj).removeClass("icon zhankai").addClass("icon shousuo");
    } else {
        $(obj).parent().parent().find("ul").eq(0).css("display", "none");
        $(obj).removeClass("icon shousuo").addClass("icon zhankai");
    }
}

///
//删除悬浮框的筛选条件
///
//objID:删除的对象的ID
//objType:筛选的对象的类型包括（'condition_exam':考试；'condition_course':课程；'condition_domain':区域；'condition_store':店铺；'condition_person':人员；"condition_train":培训）
//containerID:需要同步更新的容器的ID
//selectType:筛选的对象的类型（单选：single；多选：multiple）
///

function deleteConditionbyID(objID, objType, containerID, selectType) {
    if ($("#browseSelectCondition #" + objType).length > 0) {
        if ($("#browseSelectCondition #" + objType + " li").length > 1) {
            $("#browseSelectCondition #" + objType + " li[id='" + objID + "']").eq(0).remove();
        } else {
            $("#browseSelectCondition #" + objType).eq(0).remove();
        }
    }
    //培训
    if (objType == "condition_train" || objType == "condition_exam") {
        $("#" + containerID + " input").each(function() {
            if ($(this).attr("id") == objID.toString()) {
                $(this).attr("checked", false);
            }
        });
    }
    //区域、店铺
    if (objType == "condition_domain" || objType == "condition_store") {
        $("#" + containerID).find("a[id='" + objID + "']").eq(0).removeClass("commonSelect");
    }
    if ($("#browseSelectCondition").text().length <= 0) {
        $("#browseSelectCondition").hide();
    }
}

$(document).ready(function() {
    //如果不存在查看选择条件的悬浮框就添加
    if ($("#browseSelectCondition").length == 0 && $("#btnbrowseSelectCondition").length == 0) {
        $("body").append('<div id="btnBrowseSelectCondition" class="btnBrowseSelectCondition">' +
            '    <a id="btnBrowse" class="btnBrowse">>></a>' +
            '</div>' +
            '<div id="browseSelectCondition" class="browseSelectCondition"></div>');
        $("#btnBrowse").bind("click", function() {
            if ($("#browseSelectCondition").css("display") == "none" && $("#browseSelectCondition").text().length > 0)
                $("#browseSelectCondition").show();
            else
                $("#browseSelectCondition").hide();
        });
    }
});

//清空筛选条件(tab页切换时调用)

function clearQueryCondition() {
    if ($("#browseSelectCondition")) {
        $("#browseSelectCondition").html("");
    }
}

//获取区域树的选择值
//objID:悬浮框的单个分类的容器ID

function GetStoreQueryCondition(objID) {
    var returnvalue = '';
    if ($("#" + objID)) {
        if ($("#" + objID + " li").length > 0) {
            $("#" + objID + " li").each(function() {
                returnvalue += returnvalue == "" ? ($(this).attr("id")) : ("," + $(this).attr("id"));
            });
        }
    }
    return returnvalue;
}

function browseSelectedCondition() {
    //判断lookdiv是否显示了
    if ($("#btnBrowseSelectCondition") && $("#btnBrowseSelectCondition").css("display") != "none" && identifyApp() == "IE6") {
        var h = document.documentElement.scrollTop + 230;
        $("#browseSelectCondition").css("top", h - 60);
        $("#btnBrowseSelectCondition").css("top", h);
    }
}

//滚动条滑动时
window.onscroll = browseSelectedCondition;