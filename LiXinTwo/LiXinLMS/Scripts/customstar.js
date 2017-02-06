
/*
版本更新说明：parseInt(score)参数取整，即当传入分数为小数时，取整显示星星★
ex:   <div id='WScoreXXXXXXXXXXXX' name='{{:TotalScore}}' class='showstar flr'>{{:PingCount}}人评.</div>
保证id的唯一性  name 中存放 分数，可以为浮点型   by Richter
*/

function showStar() {
    var a = $(".showstar");
    var b = $("div[id^='WScore']");
    var idArray = new Array();
    idArray = $("div[id^='WScore']");
    for (var i = 0; i < idArray.length; i++) {
        var score = $("#" + idArray[i].id).attr('name');
        resultStar(idArray[i].id, score);
    }
}

function DisPlayStar() {
    var a = $(".DisPlaystar");
    var b = $("div[id^='CScore']");
    var idArray = new Array();
    idArray = $("div[id^='CScore']");
    for (var i = 0; i < idArray.length; i++) {
        var score = $("#" + idArray[i].id).attr('name');
        resultStar(idArray[i].id, score);
    }
}

function resultStar(id, score) {
    var htmlstar = "";
    var count = 0;
    for (var i = 0; i < parseInt(score); i++) {
        htmlstar += "<img alt=" + score + " title=" + score + "分" + " src='../../Images/star.gif' class='Oldstar' style='position: relative; top: 2px;' width='15px' />";
        count++;
    }
    for (var i = 0; i < 5 - count; i++) {
        htmlstar += "<img  alt=" + score + " title=" + score + "分" + " src='../../Images/distar.gif' class='Oldstar' style='position: relative; top: 2px;' width='15px' />";
    }
    $("#" + id).prepend(htmlstar);
}

function showStarS(str) {
    var a = $(".showstar");
    var b = $("div[id^='WScore']");
    var idArray = new Array();
    idArray = $("#" + str + " div[id^='WScore']");
    for (var i = 0; i < idArray.length; i++) {
        var score = $("#" + idArray[i].id).attr('name');
        resultStarS(idArray[i].id, score);
    }
}

function resultStarS(id, score) {
    var htmlstar = "";
    var count = 0;
    for (var i = 0; i < parseInt(score); i++) {
        htmlstar += "<img alt=" + score + " title=" + score + "分" + " src='../../Images/star.gif' class='Oldstar' style='position: relative; top: 2px;' width='15px' />";
        count++;
    }
    for (var i = 0; i < 5 - count; i++) {
        htmlstar += "<img  alt=" + score + " title=" + score + "分" + " src='../../Images/distar.gif' class='Oldstar' style='position: relative; top: 2px;' width='15px' />";
    }
    $("#" + id).prepend(htmlstar);
}


/*
星星评分功能实现
*/

function ShowScore(id, score) {
    var html = "";
    for (var i = 0; i < 5 - count; i++) {
        htmlstar += "<img  alt=" + score + " title=" + score + "分" + " src='../../Images/distar.gif' class='Oldstar' style='position: relative; top: 2px;' width='15px' />";
    } //获得5颗灰色星星
}


/*
jquery extend star  2012-10-25 10:32:23  from internet 

by Richter
*/
var starcount = 0;
$.fn.studyplay_star = function(options, callback) {
    //默认设置
    starcount++;
    var settings = {
        MaxStar: 5,
        StarWidth: 16,
        CurrentStar: 0,
        Enabled: true,
        Cursor: 'default'
    };
    if (options) {
        jQuery.extend(settings, options);
    }
    ;
    var container = jQuery(this);
    container.css({ "position": "relative" })
        .html('<ul class="studyplay_starBg" style="cursor:' + settings.Cursor + ';"></ul>')
        .find('.studyplay_starBg').width(settings.MaxStar * settings.StarWidth)
        .html('<li class="studyplay_starovering" style="width:' + settings.CurrentStar * settings.StarWidth + 'px; z-index:0;" id="studyplay_current' + starcount + '"></li>');
    if (settings.Enabled) {
        var ListArray = "";
        for (k = 1; k < settings.MaxStar + 1; k++) {
            ListArray += '<li title="' + k + '分" class="studyplay_starON"  style="width:' + settings.StarWidth * k + 'px;z-index:' + (settings.MaxStar - k + 1) + ';"></li>';
        }
        container.find('.studyplay_starBg').append(ListArray);
        container.find('.studyplay_starON').hover(function() {
            $(this).removeClass('studyplay_starON').addClass("studyplay_starovering");
            $(this).parent().children("li").eq(0).hide();
        },
            function() {
                $(this).removeClass('studyplay_starovering').addClass("studyplay_starON");
                $(this).parent().children("li").eq(0).show();
            })
            .click(function() {
                var studyplay_count = settings.MaxStar - $(this).css("z-index") + 1;

                $(this).parent().children("li").eq(0).width(studyplay_count * settings.StarWidth); //回调函数
                if (typeof callback == 'function') {
                    callback(studyplay_count);
                    return;
                }
            });
    }
};