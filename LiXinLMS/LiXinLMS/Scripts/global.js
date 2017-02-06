//初始化搜索内容
function initSearch(containerID) {

    containerID = containerID == undefined ? "" : "#" + containerID;
    

    $(containerID + " .searchclass").each(function () {
        if ($(this).attr("info") != undefined)
            $(this).attr('info', $(this).val());
    });
    $(containerID + " .searchclass").bind("blur", function () {
        if ($(this).val() == '')
            $(this).val($(this).attr("info"));
    }).bind("focus", function () {
        if ($(this).val() == $(this).attr("info"))
            $(this).val("");
    });
   
}

//获取搜索关键词
function getSearchWord(tid) {
    if ($("#" + tid).val() == $("#" + tid).attr('info'))
        return '';
    return $.trim($("#" + tid).val());
}

//遮罩等待
function zhezaowait(content) {
    $("body").append('<div id="popwindow" class="popwindow" style="display: none;"><div id="popcontent" style="text-align:center; padding:10px 0px;"><img src="../../Images/loading.gif" /><p id="contentinfor">数据处理中，请稍等……</p></div></div><div id="popbackground" class="popmask" style="display: none;"></div><iframe id="popIframe" class="popIframe" frameborder="0"></iframe>');
    if (content != undefined)
        $("#contentinfor").text(content);
    $("#popwindow,#popbackground,#popIframe").show();
    $("#popwindow").css({ "top": "150px", "left": (document.body.scrollWidth - 300) / 2 + "px" });
}

function closewait() {
    $("#popwindow,#popbackground,#popIframe").remove();
}