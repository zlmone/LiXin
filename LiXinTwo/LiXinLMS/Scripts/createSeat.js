//生成座位
/**obj:教室容器**/
/**total:教室总人数**/
/**row:教室行数**/
/**column:教室列数**/
/**disseat:教室的座位人员分配：如0,0:1,1**/
/**direction:教室座位讲台位置：如1:讲台在下；2：讲台在上**/
/**printf:1时，当前用户为：dengzi2**/
/**userid:当前用户ID**/
function exportcreateSeat(obj, total, row, column, disseat, direction,printf,userid) {
    $(obj).find("#zhuoziMain").attr('style', 'position:relative;');
    var totalcount = parseInt(total); //教室实际座位数
    var rowcount = parseInt(row); //教室行数
    var colcount = parseInt(column); //教室列数
    totalcount = rowcount * colcount > totalcount ? totalcount : rowcount * colcount;
    var deskwh = 70; //桌子宽度
    var desthe = 80; //桌子高度
    var margin = 20; //留白
    disseat = disseat || '';
    direction = direction || 1;

    if (direction == 2) {
        $(obj).parent().parent().find(".bg").addClass("bg-up").removeClass("bg");
    }

    var totalWidth = deskwh * colcount + margin * (colcount + 1); //总的宽度
    var totalHeight = desthe * rowcount + margin * (rowcount + 1); //总的高度

    $(obj).find("#zhuoziMain").css({ "width": (totalWidth + "px"), "height": (totalHeight + "px") });

    var x = margin;
    var y = margin;
    if (direction == 1) {
        x = totalWidth - (margin + deskwh); //区域起点（横坐标）
        y = totalHeight - (margin + desthe); //纵排起点（纵左标）
    }

    //添加座位
    for (var i = 0; i < totalcount; i++) {
        var numrow = parseInt((i / colcount)); //第几行
        var numcol = i % colcount; //第几列
        var newx = x + numcol * (deskwh + margin);
        var newy = y + numrow * (desthe + margin);
        if (direction == 1) {
            newx = x - numcol * (deskwh + margin);
            newy = y - numrow * (desthe + margin);
        }
        var zuobiao = numrow + ',' + numcol;
        //var index = (';' + disseat + ';').indexOf(';' + zuobiao + ';');
        $(obj).find("#zhuoziMain").append($("#singleSeatTemplate").render({ order: i, left: newx, top: newy, width: deskwh, row: numrow, col: numcol, freeze: 0, divclass:  'dengzi1' }));
    }
    //将人放置在座位上
    if (disseat != '') {
        $(obj).find("#zhuoziMain div[type='zuowei']").each(function() {
            var zuobiao = $(this).attr("data-position");//当前座位坐标
            //查找此座位上的人员信息
            var startp = (":" + disseat).indexOf(':' + zuobiao+',');
            if (startp >= 0) {
                var sub = disseat.substring(startp, disseat.length);
                var ssub = '';
                if (sub.indexOf(':') >= 0) {
                    ssub = sub.substring(0, sub.indexOf(':'));
                } else {
                    ssub = sub;
                }
                var uarr = ssub.split(',');
                if (uarr[2] != "" && parseInt(uarr[2]) < 0) {
                    $(this).attr('freeze', 1).attr('userID', '-1').removeClass('dengzi1').addClass('dengzi0');
                } else {
                    $(this).find(".name").html(uarr[3]);
                    $(this).find(".gender").html(uarr[4]);
                    $(this).attr("userID", uarr[2]==""?"0":uarr[2]);
                    if (userid == parseInt(uarr[2]) && printf==1)
                        $(this).attr("class", 'dengzi2');
                }
            }
        });
    }

    var len1 = parseInt($(obj).find("#room").width());
    var len2 = parseInt($(obj).find("#zhuoziMain").width());
    var hei1 = parseInt($(obj).find("#room").height());
    var hei2 = parseInt($(obj).find("#zhuoziMain").height());
    var zongleft = len1 > len2 ? parseInt((len1 - len2) / 2) : 0;
    var zongtop = parseInt((hei1 - hei2));

    if (len1 > len2) {
        $(obj).find("#zhuoziMain").css({ "left": (zongleft + "px") });
    }
    if (direction == 1) {
        if (hei1 > hei2) {
            $(obj).find("#zhuoziMain").css({ "top": (zongtop + 'px') });
        }
    }
}

//生成圆桌座位
/**obj:教室容器**/
/**total:教室总人数**/
/**row:教室行数**/
/**column:教室列数**/
/**preperson:每个桌子的人数**/
/**disseat:教室的禁用座位：如0,0:1,1**/
/**direction:教室座位讲台位置：如1:讲台在下；2：讲台在上**/
/**printf:1时，当前用户为：dengzi2**/
/**userid:当前用户ID**/
function exportcreateRoundSeat(obj, total, row, column, preperson, disseat, direction, printf, userid) {
    $(obj).find("#zhuoziMain").attr('style', 'position:relative;');
    var totalcount = parseInt(total); //教室实际座位数
    var rowcount = parseInt(row); //每行显示的桌子数
    var colcount = parseInt(column); //每张桌子的人数
    totalcount = rowcount * colcount * preperson > totalcount ? totalcount : rowcount * colcount * preperson;
    var deskwh = 230; //桌子宽度
    var desthe = 230; //桌子高度
    var margin = 20; //留白
    disseat = disseat || '';
    direction = direction || 1;

    if (direction == 2) {
        $(obj).parent().parent().find(".bg").addClass("bg-up").removeClass("bg");
    }

    var totalWidth = deskwh * colcount + margin * (colcount + 1); //总的宽度
    var totalHeight = desthe * rowcount + margin * (rowcount + 1); //总的高度

    $(obj).find("#zhuoziMain").css({ "width": (totalWidth + "px"), "height": (totalHeight + "px") });
    var x = margin;
    var y = margin;
    if (direction == 1) {
        x = totalWidth - (margin + deskwh); //区域起点（横坐标）
        y = totalHeight - (margin + desthe); //纵排起点（纵左标）
    }
    //循环添加桌子
    var deskcount = 1;
    for (var i = 0; i < row; i++) {
        for (var j = 0; j < column; j++) {
            var newx = x + j * (deskwh + margin);
            var newy = y + i * (desthe + margin);
            if (direction == 1) {
                newx = x - j * (deskwh + margin);
                newy = y - i * (desthe + margin);
            }
            var arrseat = [];
            for (var k = 0; k < preperson; k++) {
                arrseat.push({ order: k, row: i, col: j,freeze:0  });
            }
            $(obj).find("#zhuoziMain").append($("#singleDeskSeatTemplate").render({ number: deskcount, orderlist: arrseat,left:newx,top:newy}));
            deskcount++;
        }
    }
    $("#zhuoziMain .circle").each(function () {
        var obj = this;
        //半径
        var radius = $(this).width() / 3;
        //每一个BOX对应的角度;
        var avd = 360 / $(obj).find("div[type='zuowei']").length;
        //每一个BOX对应的弧度;
        var ahd = avd * Math.PI / 180;
        //座位的大小
        var width = 50; //radius * 2 / $(obj).find("div[type='zuowei']").length;
        //中心点横坐标
        var dotLeft = parseInt($(this).width() / 2) - width / 2;
        //中心点纵坐标
        var dotTop = parseInt($(this).height() / 2) - width / 2;
        $(obj).find("div[type='zuowei']").each(function (index, element) {
            $(this).css({ "left": Math.sin((ahd * index)) * radius + dotLeft, "top": Math.cos((ahd * index)) * radius + dotTop, "width": width, "height": width });
        });

    });
    //将人放置在座位上
    if (disseat != '') {
        $(obj).find("#zhuoziMain div[type='zuowei']").each(function () {
            var zuobiao = $(this).attr("data-position");//当前座位坐标
            //查找此座位上的人员信息
            var startp = (":" + disseat).indexOf(':' + zuobiao + ',');
            if (startp >= 0) {
                var sub = disseat.substring(startp, disseat.length);
                var ssub = '';
                if (sub.indexOf(':') >= 0) {
                    ssub = sub.substring(0, sub.indexOf(':'));
                } else {
                    ssub = sub;
                }
                var uarr = ssub.split(',');
                if (uarr[3]!="" && parseInt(uarr[3]) < 0) {
                    $(this).attr('freeze', 1).attr('userID', '-1').removeClass('yizi1').addClass('yizi0');
                } else {
                    $(this).find(".name").html(uarr[4]);
                    $(this).find(".gender").html(uarr[5]);
                    $(this).attr("userID", uarr[3]==""?"0":uarr[3]);
                    if (userid == parseInt(uarr[3]) && printf == 1)
                        $(this).attr("class", 'yizi2');
                }
            }
        });
    }

    var len1 = parseInt($(obj).find("#room").width());
    var len2 = parseInt($(obj).find("#zhuoziMain").width());
    var hei1 = parseInt($(obj).find("#room").height());
    var hei2 = parseInt($(obj).find("#zhuoziMain").height());
    var zongleft = len1 > len2 ? parseInt((len1 - len2) / 2) : 0;
    var zongtop = parseInt((hei1 - hei2));

    if (len1 > len2) {
        $(obj).find("#zhuoziMain").css({ "left": (zongleft + "px") });
    }
    if (direction == 1) {
        if (hei1 > hei2) {
            $(obj).find("#zhuoziMain").css({ "top": (zongtop + 'px') });
        }
    }
}

//打印座位
/**obj:教室容器**/
/**total:教室总人数**/
/**row:教室行数**/
/**column:教室列数**/
/**disseat:教室的禁用座位：如0,0:1,1**/
/**direction:教室座位讲台位置：如1:讲台在下；2：讲台在上**/
function exportPrintSeat(total, row, column, direction) {
    var totalcount = parseInt(total); //教室实际座位数
    var rowcount = parseInt(row); //教室行数
    var colcount = parseInt(column); //教室列数
    var deskwh = 70; //桌子宽度
    var desthe = 80; //桌子高度
    var margin = 20; //留白
    direction = direction || 1;
    totalcount = totalcount > rowcount * colcount ? rowcount * colcount : totalcount;
    //添加座位
    $("#seatShowList").find("thead td,tfoot td").attr("colspan", colcount + 1);
    if (direction == 1) {
        var currcount = rowcount * colcount;
        for (var i = rowcount - 1; i >= 0; i--) {
            var str = '<tr>';
            for (var j = colcount - 1; j >= 0; j--) {
                currcount--;
                var pos = i + ',' + j;
                str += ('<td userid="0" data-position="' + pos + '">' + (currcount <= totalcount ? '<div class="name">&nbsp;</div><div class="sex">&nbsp;</div>' : '') + '</td>');
                if (j == 0) {
                    str += ('<td class="xuhao" style="width:10px;">' + (i + 1) + '</td>');
                }
            }
            $("#seatShowList tbody").append(str + '</tr>');
        }
        var strhtml = '<tr class="zuobiao">';
        for (var i = colcount; i >= 0; i--) {
            strhtml += ('<td class="tc xuhao" style="height:10px; ' + (i == 0 ? 'width:10px;' : '') + '">' + (i == 0 ? '' : i) + '</td>');
        }
        $("#seatShowList tbody").append(strhtml + '</tr>');
    } else {
        var strhtml = '<tr class="zuobiao">';
        for (var i = 0; i <= colcount; i++) {
            strhtml += ('<td class="tc xuhao" style="height:10px; ' + (i == 0 ? 'width:10px;' : '') + '">' + (i == 0 ? '' : i) + '</td>');
        }
        $("#seatShowList tbody").append(strhtml + '</tr>');
        var currcount = 0;
        for (var i = 0; i < rowcount; i++) {
            var str = '<tr>';
            for (var j = 0; j < colcount; j++) {
                currcount++;
                var pos = i + ',' + j;
                if (j == 0) {
                    str += ('<td class="xuhao" style="width:10px;">' + (i + 1) + '</td>');
                }
                str += ('<td userid="0" data-position="' + pos + '">' + (currcount <= totalcount ? '<div class="name">&nbsp;</div><div class="sex">&nbsp;</div>' : '') + '</td>');
            }
            $("#seatShowList tbody").append(str + '</tr>');
        }
    }
}



//换位
function exportchangeSeat(obj, seatType) {
    seatType = seatType || 0;
    if (seatType == 0) {
        $(obj).find("div[type='zuowei'] div.seatselected").unbind('click').bind("click", function() {
            //如果是第一个
            if ($(obj).find("div[status='1']").length > 0) {
                if ($(this).parent().attr("status") == "1") {
                    $(this).parent().attr("status", "0").css("background-color", "transparent");
                } else {
                    //换位
                    var beiobj = $(obj).find("div[status='1']").eq(0); //被换座位
                    var berobjleft = beiobj.css("left"); //被换座位横坐标
                    var berobjtop = beiobj.css("top"); //被换座位纵坐标
                    var border = beiobj.attr("order"); //被换座位ID
                    var bclass = beiobj.attr("class"); //被换座位ID
                    var bposition = beiobj.attr("data-position"); //被换座位坐标
                    var bposmemo = beiobj.find(".zuoweiposition").text(); //被换座位坐标标签
                    var thisleft = $(this).parent().css("left"); //当前横坐标
                    var thistop = $(this).parent().css("top"); //当前纵坐标
                    var torder = $(this).parent().attr("order"); //当前ID
                    var tclass = $(this).parent().attr("class"); //当前ID
                    var tposition = $(this).parent().attr("data-position"); //当前座位坐标
                    var tposmemo = $(this).parent().find(".zuoweiposition").text(); //当前座位坐标标签
                    var o = $(this).parent();
                    var ishave = parseInt($(this).parent().attr("ishave")) + parseInt(beiobj.attr("ishave")); //是否有空位

                    //方桌实施换位    
                    beiobj.animate({ left: thisleft, top: thistop }, 1000, function() {
                        if (ishave == 2)
                            beiobj.attr("order", torder).attr("class", tclass).attr("data-position", tposition).find(".zuoweiposition").html(tposmemo);
                        else
                            beiobj.attr("order", torder).attr("data-position", tposition).find(".zuoweiposition").html(tposmemo);
                    });
                    o.animate({ left: berobjleft, top: berobjtop }, 1000, function() {
                        if (ishave == 2)
                            o.attr("order", border).attr("class", bclass).attr("data-position", bposition).find(".zuoweiposition").html(bposmemo);
                        else
                            o.attr("order", border).attr("data-position", bposition).find(".zuoweiposition").html(bposmemo);
                    });
                    $(obj).find("div[status='1']").attr("status", "0").css("background-color", "transparent");
                }
            } else {
                $(this).parent().attr("status", "1").css("background-color", "peachpuff");
            }
        });
    } else {
        $(obj).find("div[type='zuowei'] div.seatselected").unbind('click').bind("click", function () {
            //如果是第一个
            if ($(obj).find("div[status='1']").length > 0) {
                if ($(this).parent().attr("status") == "1") {
                    $(this).parent().attr("status", "0").css("background-color", "transparent");
                } else {
                    //换位
                    var beiobj = $("div[status='1']").eq(0); //被换座位
                    var berobjleft = beiobj.css("left"); //被换座位横坐标
                    var berobjtop = beiobj.css("top"); //被换座位纵坐标
                    var border = beiobj.attr("order"); //被换座位ID
                    var bclass = beiobj.attr("class"); //被换座位ID
                    var thisleft = $(this).parent().css("left"); //当前横坐标
                    var thistop = $(this).parent().css("top"); //当前纵坐标
                    var torder = $(this).parent().attr("order"); //当前ID
                    var tclass = $(this).parent().attr("class"); //当前ID
                    var o = $(this).parent();
                    var ishave = parseInt($(this).parent().attr("ishave")) + parseInt(beiobj.attr("ishave")); //是否有空位
                    //圆桌实施换位
                    //生成假的凳子
                    var boffsetleft = beiobj.offset().left;
                    var boffsettop = beiobj.offset().top;
                    var toffsetleft = o.offset().left;
                    var toffsettop = o.offset().top;

                    //var bcontent = beiobj.html();
                    var bname = beiobj.find(".name");
                    var bgender = beiobj.find(".gender");
                    var buserId = beiobj.attr("userid");
                    var bishave = beiobj.attr("ishave");
                    var bfreeze = beiobj.attr("freeze");
                    
                    //var tcontent = o.html();
                    var tname = o.find(".name");
                    var tgender = o.find(".gender");
                    var tuserId = o.attr("userid");
                    var tishave = o.attr("ishave");
                    var tfreeze = o.attr("freeze");

                    $("#bdiv").remove();
                    $("#tdiv").remove();
                    $("body").append('<div id="bdiv" class="yizi0" order="1" status="0" type="zuowei" style="display:none;position:absolute;width:50px;height:50px;z-index:5;"></div><div id="tdiv" class="yizi0" order="1" status="0" type="zuowei" style="display:none;position:absolute;width:50px;height:50px;z-index:5;"></div>');

                    beiobj.hide();
                    o.hide();
                    $("#bdiv").attr("class", beiobj.attr("class")).html(beiobj.html()).css({ "left": boffsetleft + "px", "top": boffsettop + "px" }).show();
                    $("#tdiv").attr("class", o.attr("class")).html(o.html()).css({ "left": toffsetleft + "px", "top": toffsettop + "px" }).show();

                    $("#bdiv").animate({ left: toffsetleft, top: toffsettop }, 1000, function () {
                        if (ishave == 2) {
                            beiobj.prepend(tgender).prepend(tname).attr('userid', tuserId).attr('freeze', tfreeze).show();
                        } else {
                            beiobj.prepend(tgender).prepend(tname).attr('userid', tuserId).attr('freeze', tfreeze).attr("ishave", tishave).attr("class", tclass).show();
                        }
                        $("#bdiv").hide();

                    });
                    $("#tdiv").animate({ left: boffsetleft, top: boffsettop }, 1000, function () {
                        if (ishave == 2) {
                            o.prepend(bgender).prepend(bname).attr('userid', buserId).attr('freeze', bfreeze).show();
                        } else {
                            o.prepend(bgender).prepend(bname).attr('userid', buserId).attr('freeze', bfreeze).attr("ishave", bishave).attr("class", bclass).show();
                        }
                        $("#tdiv").hide();
                    });
                }
                $(obj).find("div[status='1']").attr("status", "0").css("background-color", "transparent");
            } else {
                $(this).parent().attr("status", "1").css("background-color", "peachpuff");
            }
        });
    }
}