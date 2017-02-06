//obj1为加载分页控件的div，obj2为需要分页的div包含的div，totalcount为总的项数,pageIndex为每页显示多少项
(function ($) {
    $.extend({
        divpager: function (obj1, totalcount, obj2, pageindex, id) {
            obj1.empty();
            var me = $(id);
            var pagecount = Math.ceil(totalcount / pageindex);
            var curentpage = 1;

            var loadpage = function () {
                $.each(obj2, function (i, n) {

                    $(n).hide();
                    $(id).parent().find("input[type='checkbox']").each(function () {
                        $(this).attr("checked", false);
                    });
                    i = i + 1;
                    var max_pagecount = pageindex * curentpage;
                    var min__pagecount = max_pagecount - pageindex + 1;
                    if (i <= max_pagecount && i >= min__pagecount)
                    {
                        $(n).show();
                    }
                    fenyeHtml();
                });

                //奇偶行效果
                var rowc = 1;
                $(me).find('tr').each(function () {
                    $(this).addClass(rowc++ % 2 == 0 ? "even" : "odd");
                });
            }

            var init = function () {
                fenyeHtml();
                if (totalcount > pageindex)
                {
                    loadpage();

                }
                else
                {
                    $.each(obj2, function (i, n) {
                        $(n).show();
                    });
                }


            }


            var fenyeHtml = function () {
                var html = "";
                if (totalcount < 1)
                {
                    pagecount = 1;
                    me.eq(0).html("<tr><td colspan='" + me.parent().find("thead tr th").length + "'><div id='null' class='tc c38 line_h30'>暂无数据</div></td></tr>");
                }
                html += '<span id="paper_count" class="c38">第' + curentpage + '页/共' + pagecount + '页(共' + totalcount + '条)</span>';
                if (curentpage == 1)
                {
                    html += "<span class='first status_disabled'>首页</span>&nbsp;";
                    html += "<span class='previous status_disabled '>&nbsp;</span>&nbsp;";
                }
                else
                {
                    html += "<a index='first' class='first status-default albumsfirst' href='#page'>首页</a>&nbsp;";
                    html += "<a index='previous' class='previous status-default albumspre' href='#page'>&nbsp;</a>&nbsp;";
                }
                if (pagecount > 0)
                {
                    var maxIndex = 1; //当前要显示的最大索引
                    if (pagecount <= 5)
                        maxIndex = pagecount;
                    else if (parseInt(curentpage) + 2 <= 5)
                        maxIndex = 5;
                    else if (parseInt(curentpage) + 2 >= pagecount)
                        maxIndex = pagecount;
                    else
                        maxIndex = parseInt(curentpage) + 2;

                    for (var i = 4; i >= 0; i--)
                    {
                        if (maxIndex - i == parseInt(curentpage))
                            html += "<span class='status_disabled status-on'>&nbsp;" + (maxIndex - i) + "&nbsp;</span>";
                        else if (maxIndex - i > 0)
                            html += "<a index='" + (maxIndex - i) + "' class='status-default albumsSize' href='#'>&nbsp;" + (maxIndex - i) + "&nbsp;</a>";
                    }
                }

                if (curentpage == pagecount)
                {
                    html += "<span class='next status_disabled'>&nbsp;</span>";
                    html += "<span class='last status_disabled'>末页</span>";
                } else
                {
                    html += "<a index='next' class='next status-default albumsnext' href='#page'>&nbsp;</a>";
                    html += "<a index='last' class='last status-default albumslast' href='#page'>末页</a>";
                }
                html += "<span class='c38'>转到<a class='curp c2934f3' id='btnpagegoto'>GO</a><input type='text' id='txtpagegoto'  class='w24 h16 line_h16' onkeyup='this.value=this.value.replace(/[^0-9]/ig,\"\")' maxlength='3'>";
                //            if ($("#pager_null").length > 0)
                //            {
                //                me.eq(0).html("");
                //            }
                obj1.html(html);
                BingGoto();
            }

            var changeyeci = function () {
                if (parseInt(totalcount) > 0)
                {
                    $("#paper_count").html("第" + curentpage + "页/共" + pagecount + "页(共" + totalcount + "条)");
                } else
                {
                    $("#paper_count").html("第1页/共1页(共0条)");
                }
            }

            var BingGoto = function () {
                $("#btnpagegoto").bind("click", function () {
                    var num = 1;
                    var txtnum = $("#txtpagegoto").val();
                    if (txtnum != "")
                    {
                        num = parseInt(txtnum);
                        if (num == 0)
                        {
                            num = 1;
                        }
                    }
                    if (num > pagecount)
                    {
                        num = pagecount;
                    }

                    curentpage = num;
                    changeyeci();
                    loadpage();
                    // fenyeHtml();
                    $("#txtpagegoto").val("");
                });
            }

            obj1.delegate("a.albumsSize", "click", function () {
                var num = $(this).attr("index")

                if (num > pagecount)
                {
                    num = pagecount;
                }

                curentpage = num;
                changeyeci();
                loadpage();
                // fenyeHtml();
                $("#txtpagegoto").val("");
            });

            obj1.delegate("a.albumsfirst", "click", function () {
                curentpage = 1;
                changeyeci();
                loadpage();
                // fenyeHtml();
            });
            obj1.delegate("a.albumspre", "click", function () {
                curentpage = curentpage - 1;
                if (parseInt(curentpage) < 1)
                {
                    curentpage = 1;
                }
                changeyeci();
                loadpage();



            });
            obj1.delegate("a.albumsnext", "click", function () {
                curentpage = curentpage + 1;
                if (parseInt(curentpage) > parseInt(pagecount))
                {
                    curentpage = pagecount;
                }
                changeyeci();
                loadpage();

                //fenyeHtml();

            });
            obj1.delegate("a.albumslast", "click", function () {
                curentpage = pagecount;
                changeyeci();
                loadpage();
                // fenyeHtml();
            });


            //        obj1.delegate("#btnpagegoto", "click", function ()
            //        {

            //            var num = 1;
            //            var txtnum = $("#txtpagegoto").val();
            //            if (txtnum != "")
            //            {
            //                num = parseInt(txtnum);
            //                if (num == 0)
            //                {
            //                    num = 1;
            //                }
            //            }
            //            if (num > pagecount)
            //            {
            //                num = pagecount;
            //            }

            //            curentpage = num;
            //            changeyeci();
            //            loadpage();
            //            fenyeHtml();
            //            $("#txtpagegoto").val(num);

            //        });
            init();

        }
    });
})(jQuery)