

//弹出知识分类弹窗
function ShowType() {
    art.dialog.load("/ReResourceManage/ResourceTypeTreeView",
            {
                title: '选择知识类别',
                id: 'art_ResourceTypeTreeView',
                width: 400
            });
}
//关闭知识分类弹窗
function NodeClick() {
    $("#typeid").val($("#hidNodeID").val());
    $("#Rtype").val($("#hidNodeIDName").val());
    closeDialog("art_ResourceTypeTreeView");
}
//返回
function Back() {
    location.href = "/ReResourceManage/ReResourceManage";
}

//开放人群方法

//选择群组或组织结构
function fnChooseOpen(obj, flag) {

    if (flag == 1)//群组
    {
        if ($(obj).is(":checked")) {
            art.dialog.load("/CourseManage/CourseChooseGroup?ids=" + $("#txtOpenGroupObject").val() + "&TokenPublishflag=1" + "&tempGroupIds=" + $("#txtTempOpenGroupObject").val(), { title: '选择群组', id: "pop_chooseGroup",
                close: function () {
                    if ($("#txtOpenGroupObject").val() == "") {
                        $(obj).attr("checked", false); $("#sp_choosegroup").hide();
                    } else {
                        $(obj).attr("checked", true); $("#sp_choosegroup").show();
                    }
                }
            }, false);
        }
        else {
            $("#div_showgroup").html("");
            $("#txtOpenGroupObject").val("");
        }
    }
    if (flag == 2)//组织结构
    {
        if ($(obj).is(":checked")) {
            art.dialog.load("/UserCommon/DeptTree?type=checkbox&buttonShow=1", { title: '选择组织结构', id: "pop_chooseDept",
                close: function () {
                    if ($("#txtOpenDepartObject").val() == "") {
                        $(obj).attr("checked", false)
                    }
                }
            }, false);
        }
        else {
            $("#div_showDept").html("");
            $("#txtOpenDepartObject").val("");
        }

    }
}

function InitDeptTree() {
    if (($("#txtOpenDepartObject").val() != "") && ($("#txtOpenDepartObject").val() != 0)) {
        var checkboxArray3 = $('#pop_deptTree input[type=checkbox]');
        var IsOrderList = "," + $("#txtOpenDepartObject").val() + ",";
        var IsOrderList2 = "," + $("#txtTempOpenDepartObject").val() + ",";
        $.each(checkboxArray3, function (index, value) {
            if (IsOrderList.indexOf("," + $(value).val().split("_")[0] + ",") >= 0) {
                $(this).attr('checked', true);
                $(this).attr('disabled', "disabled");
            }
        });
    }
}
function SelectDept() {
    var ids = $("#txtOpenDepartObject").val();
    var names = "";
    var html = "";
    var flag = 0;
    if ($("#sp_chooseDept").length == 0) {
        html += "<div class='seled-list'><h4 id='sp_chooseDept'>已选组织结构：</h4><ul>";
        flag = 1;
    }
    $("#pop_deptTree input:checked").each(function () {
        if ($(this).attr("disabled") == "disabled") {
            return;
        }
        var s = $(this).attr("value").split("_");
        ids = ids == "" ? s[0] : ids + "," + s[0];
        names = names + $(this).next().text() + "♣";
        html += "<li id='div_DeptItem_" + s[0] + "'><span title='" + $(this).next().text() + "'>" + $(this).next().text() + "</span>";
        if ($(this).attr("oldchoose") != "oldchoose") {
            html += "<input class='btn btn-cancel' type='button' name='btn' value='X' title='移除'  onclick=fnRemoveDeptItem(\'div_DeptItem_" + s[0] + "\'," + s[0] + ") /></li>";
        }


    });
    if (ids.length > 0) {
        names = names.substring(0, names.length - 1);
        if (flag == 1) {
            html += "</ul><div class='mt10'><input type='button' class='btn btn-co' id='btnGoOnAddDept' onclick='fnGoOnAddDept()' value='继续添加组织结构' /></div></div>";
            $("#div_showDept").append(html);
        }
        else {
            $("#div_showDept").find("ul").eq(0).append(html);
        }
    }
    else {
        html = "";
    }
    $("#txtOpenDepartObject").val(ids);
    art.dialog.list["pop_chooseDept"].close();
}


//继续添加群组
function fnGoOnAddGroups() {
    art.dialog.load("/CourseManage/CourseChooseGroup?ids=" + $("#txtOpenGroupObject").val() + "&TokenPublishflag=1" + "&tempGroupIds=" + $("#txtTempOpenGroupObject").val(), { title: '选择群组', id: "pop_chooseGroup",
        close: function () {
            if ($("#txtOpenGroupObject").val() == "") {
                $("#chbOpenFlag1").attr("checked", false);
                $("#sp_choosegroup").hide();
            } else {
                $("#chbOpenFlag1").attr("checked", true); $("#sp_choosegroup").show();
            }
        }
    }, false);

}
//继续添加部门
function fnGoOnAddDept() {
    art.dialog.load("/UserCommon/DeptTree?type=checkbox&buttonShow=1", { title: '选择组织结构', id: "pop_chooseDept",
        close: function () {
            if ($("#txtOpenDepartObject").val() == "") {
                $("#chbOpenFlag2").attr("checked", false);
            } else {
                $("#chbOpenFlag2").attr("checked", true);
            }
        }
    }, false);

}

//从隐藏域中移除相关的群组编号
function fnRemoveGroupItem(id, groupId) {
    $("#" + id).remove();
    var oldGroupIds = $("#txtOpenGroupObject").val();

    oldGroupIds = "," + oldGroupIds + ",";
    var str = "," + groupId + ",";
    if (oldGroupIds == str) {
        oldGroupIds = oldGroupIds.replace(str, "");
    } else {
        oldGroupIds = oldGroupIds.replace(str, ",");
    }
    oldGroupIds = oldGroupIds.substring(1, oldGroupIds.length - 1);
    var splitObj = oldGroupIds.split(",");
    var objflag = true;
    if (splitObj.length > 0) {
        for (var i = 0; i < splitObj.length; i++) {
            if (splitObj[i] != "") {
                objflag = false;
            }
        }
    }
    if (objflag == true) {
        oldGroupIds = "";
    }
    $("#txtOpenGroupObject").val(oldGroupIds);
    if (oldGroupIds == "") {
        $("#chbOpenFlag1").attr("checked", false);
        $("#div_showgroup").html("");
    }
}

//
function fnRemoveDeptItem(id, groupId) {
    $("#" + id).remove();
    var oldGroupIds = $("#txtOpenDepartObject").val();

    oldGroupIds = "," + oldGroupIds + ",";
    var str = "," + groupId + ",";
    if (oldGroupIds == str) {
        oldGroupIds = oldGroupIds.replace(str, "");
    } else {
        oldGroupIds = oldGroupIds.replace(str, ",");
    }
    oldGroupIds = oldGroupIds.substring(1, oldGroupIds.length - 1);
    var splitObj = oldGroupIds.split(",");
    var objflag = true;
    if (splitObj.length > 0) {
        for (var i = 0; i < splitObj.length; i++) {
            if (splitObj[i] != "") {
                objflag = false;
            }
        }
    }
    if (objflag == true) {
        oldGroupIds = "";
    }
    $("#txtOpenDepartObject").val(oldGroupIds);
    if (oldGroupIds == "") {
        $("#chbOpenFlag2").attr("checked", false);
        $("#div_showDept").html("");
    }
}