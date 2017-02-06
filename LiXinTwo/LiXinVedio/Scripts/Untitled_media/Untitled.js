
var tt = 0;
var allduration = 0;
var chartarr = [];
var learnLength = 0;//在线观看时间（秒）
function OnLoad() {
    timerInit();
    //初始化章节
    try {
        initChart();
    }
    catch(e) {}
}

//初始化章节
function initChart() {
    for(var i=0;i<$("#indexlinks p").length;i++) {
        var index =$.trim($("#indexlinks p").eq(i).attr('id').replace('Index', ''));
        var start = $.trim($("#indexlinks p").eq(i).find("a").attr("href").replace('javascript:SeekTime', '').replace('(', '').replace(')', '').replace(';', ''));
        var end = 0;
        if ((i + 1) == $("#indexlinks p").length) {
            end = 10000000.000000;
        } else {
            end = $.trim($("#indexlinks p").eq(i + 1).find("a").attr("href").replace('javascript:SeekTime', '').replace('(', '').replace(')', '').replace(';', ''));
        }
        chartarr.push(index + ',' + start + ',' + end);
    }
}

function SeekTime(Time) {
    if (document.mediaPlayer == undefined || document.mediaPlayer == null) {
        return;
    }

    if (navigator.appName == "Netscape" && !window.GeckoActiveXObject) {
        document.mediaPlayer.getControls().setCurrentPosition(Time);
        document.mediaPlayer.getControls().Play();
    }
    else {
        document.mediaPlayer.controls.currentPosition = Time;
        document.mediaPlayer.controls.play();
    }
}


function ScrollToIndex(DivID, destIndex) {
    var nDestYCoord = destIndex.offsetTop;
    var thisNode = destIndex;

    while (thisNode.offsetParent && (thisNode.offsetParent != document.body)) {
        thisNode = thisNode.offsetParent;
        nDestYCoord += thisNode.offsetTop;
    }

    nCurWindowYPos = GetCurrentScrollYPos(DivID.id);

    //Only scroll if it's needed..
    if (destIndex.offsetTop + destIndex.offsetHeight > DivID.clientHeight) {
        nDestYCoord -= DivID.clientHeight;

        if (nDestYCoord > DivID.scrollTop) {
            DivID.scrollTop = nDestYCoord;
        }
    }
}

function GetCurrentScrollYPos(DivID) {
    var aDivs = document.body.getElementsByTagName("DIV");

    for (var i = 0; i < aDivs.length; i++) {
        var Div = aDivs[i];

        if (Div.id == DivID) {
            return Div.scrollTop;
        }
    }

    return 0;
}


function timerInit() {
    window.setInterval("timer()", 100);
}

function timer()
{
    learnLength++;
    if (document.mediaPlayer == undefined || document.mediaPlayer == null) {
        return;
    }
    if (document.mediaPlayer.PlayState == 3) {
        var destIndex = null;
        var curTime = mediaPlayer.controls.currentPosition;
        allduration = mediaPlayer.currentMedia.duration;
        tt = curTime;
        try {
            for (var i = 0; i < chartarr.length;i++ ) {
                if (curTime >= $.trim(chartarr[i].split(',')[1]) && curTime < $.trim(chartarr[i].split(',')[2])) {
                    document.getElementById('Index' + chartarr[i].split(',')[0]).style.backgroundColor = '#ffffcc';
                    destIndex = document.getElementById('Index' + chartarr[i].split(',')[0]);
                } else {
                    document.getElementById('Index' + chartarr[i].split(',')[0]).style.backgroundColor = '';
                }
            }
            if (destIndex != null) {
                ScrollToIndex(document.getElementById('indexlinks'), destIndex);
            }
        } catch(e) {
        }
    }
}



