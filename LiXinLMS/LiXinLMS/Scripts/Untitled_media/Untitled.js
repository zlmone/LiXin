
var tt = 0;
var allduration = 0;

function OnLoad() {
    timerInit();
    SeekTime(2000.000);
   
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
            return Div.scrollTop
        }
    }

    return 0;
}


function timerInit() {
    //document.mediaPlayer.CanSeek = 50;
    //Sets the timer to check every .1 second
    window.setInterval("timer()", 100);

}

function timer() {
    if (document.mediaPlayer == undefined || document.mediaPlayer == null) {
        return;
    }
    if (document.mediaPlayer.PlayState == 3) {
        
        var destIndex = null;
        
        var curTime = mediaPlayer.controls.currentPosition;

        //var MovieDuration = this.axWindowsMediaPlayer1.newMedia(mediaFileName).duration;
        //currentMedia.duration
        //alert(mediaPlayer.currentMedia.duration); 7246.700000
        allduration = mediaPlayer.currentMedia.duration;
        tt = curTime;
        //alert(curTime);
        if (curTime >= 0.000000 && curTime < 7246.700000) {
            
            document.getElementById('Index0').style.backgroundColor = '#ffffcc';
            destIndex = document.getElementById('Index0');
        }
        else {
            document.getElementById('Index0').style.backgroundColor = '';
        }
        //SeekTime(50);

        if (destIndex != null) {
            ScrollToIndex(document.getElementById('indexlinks'), destIndex);
        }
    }
}



