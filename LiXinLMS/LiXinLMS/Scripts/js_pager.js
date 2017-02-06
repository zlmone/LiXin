//obj1为加载分页控件的div，obj2为需要分页的div包含的div，totalcount为总的项数,pageIndex为每页显示多少项
(function ($) {
    $.extend({ divpager: function (obj1, totalcount, obj2, pageindex) {
        obj1.empty();
        var pagecount = Math.ceil(totalcount / pageindex);
        var curentpage = 1;
        var a_disabled = function () {
            var albumsfirst = $("#first");
            var albumspre = $("#pre");
            var albumsnext = $("#next");
            var albumslast = $("#last");
            if (curentpage == 1) {
                albumsfirst.attr("class", "first status_disabled");
                albumsfirst.attr("disabled", "true");
                albumsfirst.removeAttr("href");
                albumspre.attr("class", "previous status_disabled");
                albumspre.attr("disabled", "true");
                albumspre.removeAttr("href");
                if (parseInt(pagecount) <= 1) {
                    albumsnext.attr("class", "next status_disabled");
                    albumsnext.attr("disabled", "true");
                    albumsnext.removeAttr("href");
                    albumslast.attr("class", "last status_disabled");
                    albumslast.attr("disabled", "true");
                    albumslast.removeAttr("href");
                } else {
                    albumsnext.attr("class", "next status-default");
                    albumsnext.attr("href", "#");
                    albumsnext.removeAttr("disabled");
                    albumslast.attr("class", "last status-default");
                    albumslast.attr("href", "#");
                    albumslast.removeAttr("disabled");
                }
            }
            else if (parseInt(curentpage) < 1) {
                albumsfirst.attr("class", "first status_disabled");
                albumsfirst.attr("disabled", "true");
                albumsfirst.removeAttr("href");
                albumspre.attr("class", "previous status_disabled");
                albumspre.attr("disabled", "true");
                albumspre.removeAttr("href");
                if (parseInt(pagecount) <= 1) {
                    albumsnext.attr("class", "next status_disabled");
                    albumsnext.attr("disabled", "true");
                    albumsnext.removeAttr("href");
                    albumslast.attr("class", "last status_disabled");
                    albumslast.attr("disabled", "true");
                    albumslast.removeAttr("href");
                } else {
                    albumsnext.attr("class", "next status-default");
                    albumsnext.attr("href", "#");
                    albumsnext.removeAttr("disabled");
                    albumslast.attr("class", "last status-default");
                    albumslast.attr("href", "#");
                    albumslast.removeAttr("disabled");
                }
                curentpage = 1;
            }
            else if (curentpage == pagecount) {
                albumsnext.attr("class", "next status_disabled");
                albumsnext.attr("disabled", "true");
                albumsnext.removeAttr("href");
                albumslast.attr("class", "last status_disabled");
                albumslast.attr("disabled", "true");
                albumslast.removeAttr("href");
                if (parseInt(curentpage) <= 1) {
                    albumsfirst.attr("class", "first status_disabled");
                    albumsfirst.attr("disabled", "true");
                    albumsfirst.removeAttr("href");
                    albumspre.attr("class", "previous status_disabled");
                    albumspre.attr("disabled", "true");
                    albumspre.removeAttr("href");
                } else {
                    albumsfirst.attr("class", "first status-default");
                    albumsfirst.attr("href", "#");
                    albumsfirst.removeAttr("disabled");
                    albumspre.attr("class", "previous status-default");
                    albumspre.attr("href", "#");
                    albumspre.removeAttr("disabled");
                }
            }
            else if (parseInt(curentpage) > parseInt(pagecount)) {
                albumsnext.attr("class", "next status_disabled");
                albumsnext.attr("disabled", "true");
                albumsnext.removeAttr("href");
                albumslast.attr("class", "last status_disabled");
                albumslast.attr("disabled", "true");
                albumslast.removeAttr("href");
                if (parseInt(curentpage) <= 1) {
                    albumsfirst.attr("class", "first status_disabled");
                    albumsfirst.attr("disabled", "true");
                    albumsfirst.removeAttr("href");
                    albumspre.attr("class", "previous status_disabled");
                    albumspre.attr("disabled", "true");
                    albumspre.removeAttr("href");
                } else {
                    albumsfirst.attr("class", "first status-default");
                    albumsfirst.attr("href", "#");
                    albumsfirst.removeAttr("disabled");
                    albumspre.attr("class", "previous status-default");
                    albumspre.attr("href", "#");
                    albumspre.removeAttr("disabled");
                }
                curentpage = pagecount;
            }
            else {
                albumsfirst.attr("class", "first status-default");
                albumsfirst.attr("href", "#");
                albumsfirst.removeAttr("disabled");
                albumspre.attr("class", "previous status-default");
                albumspre.attr("href", "#");
                albumspre.removeAttr("disabled");
                albumsnext.attr("class", "next status-default");
                albumsnext.attr("href", "#");
                albumsnext.removeAttr("disabled");
                albumslast.attr("class", "last status-default");
                albumslast.attr("href", "#");
                albumslast.removeAttr("disabled");
            }

        }
        var loadpage = function () {
            $.each(obj2, function (i, n) {
                $(n).hide();
                i = i + 1;
                var max_pagecount = pageindex * curentpage;
                var min__pagecount = max_pagecount - pageindex + 1;
                if (i <= max_pagecount && i >= min__pagecount) {
                    $(n).show();
                }
                a_disabled();
            });
        }
        var init = function () {
            var div_page = "";
            if (parseInt(totalcount) > 0) {
                div_page = "<span id='txtpage' class='c38'>第" + curentpage + "页/共" + pagecount + "页(共" + totalcount + "条)</span><input id='pagecount' type='hidden' /><a class='first status-disabled' id='first' >首页</a><a class='previous status-disabled' id='pre' >上一页</a>"
                for (var i = 0; i < pagecount; i++) {
                    if (i == 0){
                        div_page += "<a class='pager status_disabled status-on' href='#'>" + (i + 1) + "</a>"
                    } else {
                        div_page += "<a class='pager status-default' href='#'>" + (i + 1) + "</a>"
                    }
                }
                div_page += "<a class='next status-disabled' id='next' >下一页</a><a class='last status-disabled' id='last' >末页</a><span class='c38'>转到<a id='btnpagegoto' class='curp c2934f3'>GO</a><input class='w24 h16 line_h16' id='txtpagegoto' type='text' onkeyup='this.value=this.value.replace(/[^0-9]/ig,\"\")' /></span>";
            } else {
                div_page = "<span id='txtpage' class='c38'>第1页/共1页(共0条)</span><input id='pagecount' type='hidden' /><a class='first status_disabled' id='first'>首页</a><a class='previous status_disabled' id='pre' >上一页</a><a class='status_disabled status-on'>1</a><a class='next status_disabled' id='next' >下一页</a><a class='last status_disabled' id='last' >末页</a><span class='c38'>转到<a id='btnpagegoto' class='curp c2934f3'>GO</a><input class='w24 h16 line_h16' id='txtpagegoto' type='text' onkeyup='this.value=this.value.replace(/[^0-9]/ig,\"\")' /></span>";
            }
            obj1.html(div_page);
            if (totalcount > pageindex) {
                loadpage();
                a_disabled();
            }
            else {
                $.each(obj2, function (i, n) {
                    $(n).show();
                });
                a_disabled();
            }
            $("#pagecount").val(pagecount);
        }
        var changeyeci = function () {
            if (parseInt(totalcount) > 0) {
                $("#txtpage").html("第" + curentpage + "页/共" + pagecount + "页(共" + totalcount + "条)");
                var temp = $("#listPage").find("a.pager");
                for (var i = 0; i < temp.length; i++) {
                    if ((i+1) == curentpage) {
                        $(temp[i]).removeClass("status-default");
                        $(temp[i]).addClass("status_disabled").addClass("status-on");
                    } else {
                        $(temp[i]).removeClass("status_disabled").removeClass("status-on");
                        $(temp[i]).addClass("status-default");
                    }
                }
            } else {
                $("#txtpage").html("第1页/共1页(共0条)");
            }
        }
        init();
        obj1.delegate("#first", "click", function () {
            curentpage = 1;
            changeyeci();
            loadpage();
        });
        obj1.delegate("#pre", "click", function () {
            curentpage = curentpage - 1;
            if (parseInt(curentpage) < 1) {
                curentpage = 1;
            }
            changeyeci();
            loadpage();
        });
        obj1.delegate("#next", "click", function () {
            curentpage = curentpage + 1;
            if (parseInt(curentpage) > parseInt(pagecount)) {
                curentpage = pagecount;
            }
            changeyeci();
            loadpage();
        });
        obj1.delegate("#last", "click", function () {
            curentpage = pagecount;
            changeyeci();
            loadpage();
        });
        obj1.delegate("#btnpagegoto", "click", function () {
            var txtnum = $("#txtpagegoto").val();
            var pcount = $("#pagecount").val();
            if (txtnum != "") {
                if (parseInt(txtnum) > parseInt(pcount)) {
                    curentpage = pcount;
                } else if (parseInt(txtnum) < 1) {
                    curentpage = 1;
                } else {
                    curentpage = txtnum;
                }
            }
            $("#txtpagegoto").val(curentpage);
            changeyeci();
            loadpage();
        });
        obj1.delegate("a.pager", "click", function () {
            curentpage = $.trim(this.innerHTML);
            changeyeci();
            loadpage();
        });
    }
    });
})(jQuery)