//jsrender 扩展
(function ($)
{
    $.fn.JsRenderData = function (args)
    {

        var isPage = true; //是否分页
        var pageSize = 10000; //每页的个数
        var pageIndex = 1; //请求的页数
        var templateId; //自定义模板
        var jsRenderSortField; //默认排序字段
        var jsRenderASC; //默认排序方式
        var ajaxtype = "get";//传值方式 默认为get
        var ajaxParam;//如果是post方式的时候传过来的参数
        var datatype;//类型 mongodb的时候 加载一个参数
        var wait;//等待框
        me = this;

        if (me.length == 0)
        {
            return me;
        }
        if ($(me).parent().find("thead tr th ").eq(0).find("input[type='checkbox']").length > 0)
            $(me).parent().find("thead tr th ").eq(0).find("input[type='checkbox']").removeAttr("checked");


        var recordCount; //数据的总数
        var sourceUrl; //请求的数据源连接
        var funCallback; //回调函数
        var dataSource; //定义数据源
        if (arguments.length > 0)
        { //绑定对象的ID
            pageSize = arguments[0].pageSize == undefined ? pageSize : arguments[0].pageSize; //每页的个数
            pageIndex = arguments[0].pageIndex == undefined ? pageIndex : arguments[0].pageIndex; //请求的页数
            recordCount = arguments[0].recordCount == undefined ? null : arguments[0].recordCount; //数据的总数
            sourceUrl = arguments[0].sourceUrl == undefined ? null : arguments[0].sourceUrl; //请求的数据源连接
            funCallback = arguments[0].funCallback == undefined ? null : arguments[0].funCallback; //回调函数
            dataSource = arguments[0].dataSource == undefined ? null : arguments[0].dataSource; //定义数据源
            isPage = arguments[0].isPage == undefined ? null : arguments[0].isPage; //是否分页
            templateId = arguments[0].templateID == undefined ? null : arguments[0].templateID; //自定义模板
            jsRenderSortField = arguments[0].jsRenderSortField == undefined ? null : arguments[0].jsRenderSortField; //默认排序字段
            jsRenderASC = arguments[0].jsRenderASC == undefined ? " ASC" : arguments[0].jsRenderASC; //默认排序方式
            ajaxtype = arguments[0].ajaxtype == undefined ? "get" : arguments[0].ajaxtype;
            ajaxParam = arguments[0].ajaxParam == undefined ? null : arguments[0].ajaxParam;
            datatype = arguments[0].datatype == undefined ? null : arguments[0].datatype;
            wait = arguments[0].wait == undefined ? 0 : arguments[0].wait;

        }
        if (sourceUrl != null)
        {
            //url请求
            if (sourceUrl.indexOf("?") == -1)
            {
                sourceUrl += "?";
                sourceUrl += "pageIndex=" + pageIndex + "&pageSize=" + pageSize;
            } else
            {
                sourceUrl = sourceUrl.replace(/&?(pageIndex|pageSize|_)=([^&]*)/gi, ""); //过滤url的翻页参数
                sourceUrl += "&pageIndex=" + pageIndex + "&pageSize=" + pageSize;
            }
            if (jsRenderSortField != null && jsRenderSortField != undefined && jsRenderSortField != "")
            {
                sourceUrl += "&jsRenderSortField=" + jsRenderSortField + (jsRenderASC == " ASC" ? " ASC " : " DESC ");
            }
            if (datatype == "mongodb")
            {
                sourceUrl = sourceUrl.replace(/&?(mongodb_lastID)=([^&]*)/gi, "")
                sourceUrl += "&mongodb_lastID=" + $("#lastID").val();

            }
            //为以后要用到的Ajax请求设置默认的值
            if (ajaxtype == "get")
            {
               
                $.ajax({
                    // processData:false,
                    type: "get",
                    url: sourceUrl.indexOf("?") >= 0 ? (sourceUrl + "&signt=" + new Date()).replace(/\+/g, "%2B") : (sourceUrl + "?signt=" + new Date()).replace(/\+/g, "%2B"),
                    async: false,
                    success: function (data)
                    {
                      
                        fnRenderData(me, data, isPage, pageSize, pageIndex, sourceUrl, templateId, funCallback, ajaxtype, ajaxParam, datatype);
                        if (funCallback != null && funCallback != undefined)
                            funCallback(data);
                        fnTableBindSort(me, data, isPage, pageSize, pageIndex, sourceUrl, templateId, funCallback, jsRenderASC, ajaxtype, ajaxParam, datatype);
                        //奇偶行效果
                        ChangeNum(pageIndex, pageSize);
                        var rowc = 1;
                        $(me).find('tr').each(function ()
                        {
                            $(this).addClass(rowc++ % 2 == 0 ? "even" : "odd");
                        });
                        //排序标志
                        $(me).parent().find("thead th").each(function ()
                        {
                            var sort = $(this).attr("jsRenderSortField");
                            if (sort != null && sort != undefined)
                            {
                                var str = $(this).text();
                                
                                $(this).css("cursor", "pointer").html('<div class="sort" >' + str + '<div class="sort-do"><i class="s-up"></i><i class="s-down"></i></div></div>');
                            }
                        });
                    }
                });
            }
            else
            {
                if (wait == 1)
                {
                    zhezaowait(); //等待提示
                }
               
                $.post(sourceUrl.indexOf("?") >= 0 ? (sourceUrl + "&signt=" + new Date()).replace(/\+/g, "%2B") : (sourceUrl + "?signt=" + new Date()).replace(/\+/g, "%2B"),
                    ajaxParam,function (data)
                    {
                        if (wait == 1)
                        {
                            closewait();
                        }
                        fnRenderData(me, data, isPage, pageSize, pageIndex, sourceUrl, templateId, funCallback, ajaxtype, ajaxParam, datatype);
                        if (funCallback != null && funCallback != undefined)
                            funCallback(data);
                        fnTableBindSort(me, data, isPage, pageSize, pageIndex, sourceUrl, templateId, funCallback, jsRenderASC, ajaxtype, ajaxParam, datatype);
                        //奇偶行效果
                        ChangeNum(pageIndex, pageSize);
                        var rowc = 1;
                        $(me).find('tr').each(function ()
                        {
                            $(this).addClass(rowc++ % 2 == 0 ? "even" : "odd");
                        });
                        //排序标志
                        $(me).parent().find("thead th").each(function ()
                        {
                            var sort = $(this).attr("jsRenderSortField");
                            if (sort != null && sort != undefined)
                            {
                                var str = $(this).text();
                                $(this).css("cursor", "pointer").html('<div class="sort"  style="text-align:left">' + str + '<div class="sort-do"><i class="s-up"></i><i class="s-down"></i></div></div>');
                            }
                        });
                    });

            }



        } else
        {
            if (dataSource != null && dataSource != undefined)
            {
                if ($(me).parent().find("tbody").length == 0)
                    $(me).html("<div id='listbody'></div><div id='listPage' class='listPage'></div>");
                me.JsRenderBody(me, dataSource, templateId);
                if (isPage)
                {
                    me.JsRenderPaging(me, dataSource, isPage, pageSize, pageIndex, sourceUrl, templateId, funCallback, ajaxtype, ajaxParam, datatype);
                }
                if (funCallback != null && funCallback != undefined)
                    funCallback(data);
                fnTableBindSort(me, data, isPage, pageSize, pageIndex, sourceUrl, templateId, funCallback, jsRenderASC, ajaxtype, ajaxParam, datatype);
                var rowc = 1;
                $(me).find('tr').each(function ()
                {
                    $(this).addClass(rowc++ % 2 == 0 ? "even" : "odd");
                });
            }
        }
    };

    function fnRenderData(me, dataSource, isPage, pageSize, pageIndex, sourceUrl, templateId, funCallback, ajaxtype, ajaxParam, datatype)
    {
        if ($(me).parent().find("tbody").length == 0)
        {
            if ($(me).find("#listbody").length == 0)
            {
                $(me).append("<div id='listbody'></div>");
            }
            if ($(me).find("#listPage").length == 0)
            {
                $(me).append("<div id='listPage' class='listPage'></div>");
            }
        } else
        {
            $(me).parent().find("tfoot").html("<tr><td colspan='" + $(me).parent().find("thead tr th").length + "'><div id='listPage' class='listPage'></div></td></tr>");
        }
        me.JsRenderBody(me, dataSource, templateId);
        if (isPage)
        {
            me.JsRenderPaging(me, dataSource, isPage, pageSize, pageIndex, sourceUrl, templateId, funCallback, ajaxtype, ajaxParam, datatype);
        }
    }

    function fnTableBindSort(me, data, isPage, pageSize, pageIndex, sourceUrl, templateId, funCallback, jsRenderASC, ajaxtype, ajaxParam, datatype)
    {
        $(me).parent().find('th').each(function ()
        {
            var sort = $(this).attr("jsRenderSortField");
            if (sort != null && sort != undefined)
            {
                $(this).attr("sort", $(this).attr("sort") == "asc" ? "desc" : "asc");

                $(this).unbind("click");
                $(this).bind("click", function ()
                {
                    sourceUrl = sourceUrl.replace(/&?(jsRenderSortField|jsRenderASC)=([^&]*)/gi, ""); //过滤url的排序参数
                    sourceUrl += "&jsRenderSortField=" + sort + ' ' + $(this).attr("sort");
                    me.JsRenderData({
                        sourceUrl: sourceUrl, pageIndex: pageIndex, pageSize: pageSize, isPage: isPage, templateID: templateId, funCallback: funCallback, ajaxtype: ajaxtype, ajaxParam: ajaxParam, datatype: datatype
                    });
                    var removestr = $(this).attr("sort") == "desc" ? "do-down" : "do-up";
                    var addstr = $(this).attr("sort") == "desc" ? "do-up" : "do-down";
                    $(this).find(".sort-do").eq(0).removeClass(removestr).addClass(addstr);
                });
            }
        });
    }


    //生成主体部分
    $.fn.JsRenderBody = function (obj, dataS, dataTemplateID)
    {
        if (dataS.dataList.length > 0)
        {
            if ($(obj).parent().find("tbody").length == 0)
            {
                $(obj).find("#listbody").eq(0).html($("#" + dataTemplateID).render(dataS.dataList));
                //鼠标悬浮事件
                $(obj).find("div#listbody").eq(0).append('<div class="clear"></div>');
                //$(obj).find("ul#listbody").eq(0).append('<li class="clear"></li>');
                $(obj).find("#listbody dl").bind("mouseover", function ()
                {
                    $(this).find(".Doing").show();
                }).bind("mouseleave", function ()
                {
                    $(this).find(".Doing").hide();
                });
            } else
            {
                $(obj).html($("#" + dataTemplateID).render(dataS.dataList));
            }
        } else
        {
            if ($(obj).parent().find("tbody").length == 0)
            {
                $(obj).find("#listbody").eq(0).html("<div class='tc c38 line_h30'>暂无数据</div>");
            } else
            {
                $(obj).parent().find("tbody").eq(0).html("<tr><td colspan='" + $(obj).parent().find("thead tr th").length + "'><div class='tc c38 line_h30'>暂无数据</div></td></tr>");
            }
        }
    };

    //公共的分页部分
    $.fn.PageHtml = function (recordCount, pageCount, pageSize, pageIndex)
    {
        var html = '';
        pageCount = pageCount == 0 ? 1 : pageCount;
        if (parseInt(pageIndex) > pageCount)
            pageIndex = pageCount;

        html += '<span class="c38">第' + pageIndex + '页/共' + pageCount + '页(共' + recordCount + '条)</span>';
        if (parseInt(pageIndex) == 1)
        {
            html += "<span class='first status_disabled'>首页</span>";
            html += "<span class='previous status_disabled'>&nbsp;</span>";
        } else
        {
            html += "<a index='first' class='first status-default' href=\"#1\">首页</a>";
            html += "<a index='previous' class='previous status-default' href=\"#" + (pageIndex - 1) + "\">&nbsp;</a>";
        }
        if (true)
        {
            var maxIndex = 1; //当前要显示的最大索引
            if (pageCount <= 5)
                maxIndex = pageCount;
            else if (parseInt(pageIndex) + 2 <= 5)
                maxIndex = 5;
            else if (parseInt(pageIndex) + 2 >= pageCount)
                maxIndex = pageCount;
            else
                maxIndex = parseInt(pageIndex) + 2;

            for (var i = 4; i >= 0; i--)
            {
                if (maxIndex - i == parseInt(pageIndex))
                    html += "<span class='status_disabled status-on'>&nbsp;" + (maxIndex - i) + "&nbsp;</span>";
                else if (maxIndex - i > 0)
                    html += "<a index='" + (maxIndex - i) + "' class='status-default' href='#" + (maxIndex - i) + "'>&nbsp;" + (maxIndex - i) + "&nbsp;</a>";
            }
        }
        if (pageCount == parseInt(pageIndex))
        {
            html += "<span class='next status_disabled'>&nbsp;</span>";
            html += "<span class='last status_disabled'>末页</span>";
        } else
        {
            html += "<a index='next' class='next status-default' href=\"#" + (parseInt(pageIndex, 10) + 1) + "\">&nbsp;</a>";
            html += "<a index='last' class='last status-default' href=\"#" + pageCount + "\">末页</a>";
        }
        html += '<span class="c38">转到&nbsp;<a class="curp c2934f3" id="btnpagegoto">GO</a><input type="text" id="txtpagegoto" class="w24 h16 line_h16" maxlength="3" /></span>';
        return html;
    };
    //生成分页部分
    $.fn.JsRenderPaging = function (obj, dataS, isPage, pageSize, pageIndex, sourceUrl, dataTemplateId, funCallback, ajaxtype, ajaxParam, datatype)
    {
        var recordCount = parseInt(dataS.recordCount || 0);
        var pageCount = Math.ceil(recordCount / pageSize);
        var html = this.PageHtml(recordCount, pageCount, pageSize, pageIndex);
        if ($(obj).parent().find("tfoot").length == 0)
        {
            $(obj).find("#listPage").eq(0).show().html(html);
        } else
        {
            $(obj).parent().find("#listPage").eq(0).show().html(html);
        }

        //翻页需要存储数据到table的属性中
        this.setListPageLink(sourceUrl, funCallback, isPage, pageSize, dataTemplateId, ajaxtype, ajaxParam, datatype); //设置分页的链接

        this.setListGotoPageLink(sourceUrl, funCallback, isPage, pageSize, dataTemplateId, pageCount, ajaxtype, ajaxParam, datatype); //设置'转向'分页的事件

        return this;
    };
    //设置分页连接
    $.fn.setListPageLink = function (sourceUrl, funCallback, isPage, pageSize, templateId, ajaxtype, ajaxParam, datatype)
    {
        var me = this;
        $(me).parent().find("#listPage a").unbind("click").click(function ()
        {
            var thisPageIndex = $(this).attr("href");
            thisPageIndex = thisPageIndex.substring(thisPageIndex.lastIndexOf("#") + 1);
            me.JsRenderData({ sourceUrl: sourceUrl, pageIndex: thisPageIndex, pageSize: pageSize, isPage: isPage, templateID: templateId, funCallback: funCallback, ajaxtype: ajaxtype, ajaxParam: ajaxParam, datatype: datatype });
        });
        return me;
    };

    //设置转向连接
    $.fn.setListGotoPageLink = function (sourceUrl, funCallback, isPage, pageSize, templateId, pageCount, ajaxtype, ajaxParam, datatype)
    {
        var me = this;
        $(me).parent().find("#listPage #btnpagegoto").unbind("click").click(function ()
        {
            var o = $(me).parent().find("#listPage #txtpagegoto").eq(0);
            if (o.val() != "")
            {
                var pattern = /^[0-9]+$/;
                if (pattern.test(o.val()))
                {
                    var thisGotoIndex = parseInt(o.val());
                    if (thisGotoIndex <= pageCount && thisGotoIndex > 0)
                    {
                        me.JsRenderData({ sourceUrl: sourceUrl, pageIndex: thisGotoIndex, pageSize: pageSize, isPage: isPage, templateID: templateId, funCallback: funCallback, ajaxtype: ajaxtype, ajaxParam: ajaxParam, datatype: datatype });
                    }
                }
            }
        });
        return me;
    };

    //更改Number
    function ChangeNum(pageIndex, pageSize)
    {
        var eqindex = 0;
        if ($(me).parent().find("thead tr th ").eq(0).find(":checkbox").length > 0 || $(me).parent().find("tbody tr td").find(":radio").length > 0)
            eqindex = 1;
        $(me).parent().find("tbody tr").each(function ()
        {
            var obj = $(this).find("td").eq(eqindex);
            if (!$(obj).attr("data-type"))
            {
                if (!isNaN(obj.text()))
                {
                    obj.text((pageIndex - 1) * pageSize + parseInt(obj.text()));
                } else if ($(this).find("td").eq(0).find("div span").length > 0)
                {
                    var divobj = $(this).find("td").eq(0).find("div span");
                    if (!isNaN(divobj.html()))
                    {
                        divobj.text((pageIndex - 1) * pageSize + parseInt(divobj.html()));
                    }
                }
            }
        });
    }


})(jQuery);

//GO TO TOP
(function ($)
{
    var top = 0;
    var bottom = 0;
    var tophrefpoint = "";
    var bottomhrefpoint = "";
    var pingshow = true;
    var topshow = true;
    $.fn.createTop = function ()
    {
        if (arguments.length > 0)
        {
            top = arguments[0].top == undefined ? 0 : arguments[0].top;
            bottom = arguments[0].bottom == undefined ? 0 : arguments[0].bottom;
            tophrefpoint = arguments[0].tophrefpoint == undefined ? "" : arguments[0].tophrefpoint;
            bottomhrefpoint = arguments[0].bottomhrefpoint == undefined ? "" : arguments[0].bottomhrefpoint;
            pingshow = arguments[0].pingshow == undefined ? pingshow : arguments[0].pingshow;
            topshow = arguments[0].topshow == undefined ? topshow : arguments[0].topshow;
        }
        $("body").append('<div class="positiontop">' + (topshow ? '<a href="#' + (tophrefpoint == "" ? "@" : tophrefpoint) + '" class="gototop">&nbsp;</a>' : '') + (pingshow ? '<a href="#' + (tophrefpoint == "" ? "@" : tophrefpoint) + '" class="evlution">&nbsp;</a>' : '') + '</div>');
        $(".positiontop").css({ "left": ((document.body.clientWidth + 950) / 2) + "px" });
        $(this).bindClick();
    };

    $.fn.bindClick = function ()
    {
        if (tophrefpoint == "")
        {
            $(".gototop").bind("click", function ()
            {
                $('body,html').animate({ scrollTop: top }, 1000);
            });
        }
        if (bottomhrefpoint == "")
        {
            $(".evlution").bind("click", function ()
            {
                $('body,html').animate({ scrollTop: document.body.scrollHeight }, 1000);
            });
        }
    };

    $.fn.preInput = function ()
    {
        var o = this;
        if ($(o).attr("tip") == $(o).val())
            $(o).css("color", "#999");
        $(o).bind("blur", function ()
        {
            if ($(o).val() == "" || $(o).attr("tip") == $(o).val())
            {
                $(o).val($(o).attr("tip"));
                $(o).css("color", "#999");
            }
        }).bind("focus", function ()
        {
            if ($(o).attr("tip") == $(o).val())
            {
                $(o).val("");
                $(o).css("color", "#000");
            }
            $(o).css("color", "#000");
        });
    };
})(jQuery);

(function ($)
{
    $.fn.textareaCount = function (options, fn)
    {
        var defaults = {
            maxCharacterSize: -1,
            originalStyle: 'originalTextareaInfo',
            warningStyle: 'warningTextareaInfo',
            warningNumber: 20,
            displayFormat: '已输入 #input 字符',
            messageShow: options.messageShow || ''
        };
        var options = $.extend(defaults, options);

        var container = $(this);

        if (options.messageShow == "")
            $("<div class='charleft'>&nbsp;</div>").insertAfter(container);

        //create charleft css
        var charLeftCss = {
            'width': container.width()
        };

        var charLeftInfo = getNextCharLeftInformation(container);
        charLeftInfo.addClass(options.originalStyle);
        charLeftInfo.css(charLeftCss);

        var numInput = 0;
        var maxCharacters = options.maxCharacterSize;
        var numLeft = 0;
        var numWords = 0;

        container.bind('keyup', function (event) { limitTextAreaByCharacterCount(); })
            .bind('mouseover', function (event) { setTimeout(function () { limitTextAreaByCharacterCount(); }, 10); })
            .bind('paste', function (event) { setTimeout(function () { limitTextAreaByCharacterCount(); }, 10); });


        function limitTextAreaByCharacterCount()
        {
            charLeftInfo.html(countByCharacters());
            //function call back
            if (typeof fn != 'undefined')
            {
                fn.call(this, getInfo());
            }
            return true;
        }

        function countByCharacters()
        {
            var content = container.val();
            var contentLength = content.length;

            //Start Cut
            if (options.maxCharacterSize > 0)
            {
                //If copied content is already more than maxCharacterSize, chop it to maxCharacterSize.
                if (contentLength >= options.maxCharacterSize)
                {
                    content = content.substring(0, options.maxCharacterSize);
                }

                var newlineCount = getNewlineCount(content);

                // newlineCount new line character. For windows, it occupies 2 characters
                var systemmaxCharacterSize = options.maxCharacterSize - newlineCount;
                if (!isWin())
                {
                    systemmaxCharacterSize = options.maxCharacterSize;
                }
                if (contentLength > systemmaxCharacterSize)
                {
                    //avoid scroll bar moving
                    var originalScrollTopPosition = this.scrollTop;
                    container.val(content.substring(0, systemmaxCharacterSize));
                    this.scrollTop = originalScrollTopPosition;
                }
                charLeftInfo.removeClass(options.warningStyle);
                if (systemmaxCharacterSize - contentLength <= options.warningNumber)
                {
                    charLeftInfo.addClass(options.warningStyle);
                }

                numInput = container.val().length + newlineCount;
                if (!isWin())
                {
                    numInput = container.val().length;
                }

                numWords = countWord(getCleanedWordString(container.val()));

                numLeft = maxCharacters - numInput;
            } else
            {
                var newlineCount = getNewlineCount(content);
                numInput = container.val().length + newlineCount;
                if (!isWin())
                {
                    numInput = container.val().length;
                }
                numWords = countWord(getCleanedWordString(container.val()));
            }

            return formatDisplayInfo();
        }

        function formatDisplayInfo()
        {
            var format = options.displayFormat;
            format = format.replace('#input', numInput);
            format = format.replace('#words', numWords);
            if (maxCharacters > 0)
            {
                format = format.replace('#max', maxCharacters);
                format = format.replace('#left', numLeft);
            }
            return format;
        }

        function getInfo()
        {
            var info = {
                input: numInput,
                max: maxCharacters,
                left: numLeft,
                words: numWords
            };
            return info;
        }

        function getNextCharLeftInformation(container)
        {
            if (options.messageShow == "")
                return container.next('.charleft');
            else
                return $("#" + options.messageShow);
        }

        function isWin()
        {
            var strOS = navigator.appVersion;
            if (strOS.toLowerCase().indexOf('win') != -1)
            {
                return true;
            }
            return false;
        }

        function getNewlineCount(content)
        {
            var newlineCount = 0;
            for (var i = 0; i < content.length; i++)
            {
                if (content.charAt(i) == '\n')
                {
                    newlineCount++;
                }
            }
            return newlineCount;
        }

        function getCleanedWordString(content)
        {
            var fullStr = content + " ";
            var initial_whitespace_rExp = /^[^A-Za-z0-9]+/gi;
            var left_trimmedStr = fullStr.replace(initial_whitespace_rExp, "");
            var non_alphanumerics_rExp = rExp = /[^A-Za-z0-9]+/gi;
            var cleanedStr = left_trimmedStr.replace(non_alphanumerics_rExp, " ");
            var splitString = cleanedStr.split(" ");
            return splitString;
        }

        function countWord(cleanedWordString)
        {
            var word_count = cleanedWordString.length - 1;
            return word_count;
        }
    };
})(jQuery);

//扩展 自定义的排序
