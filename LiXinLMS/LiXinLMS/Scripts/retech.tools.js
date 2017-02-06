//jQuery扩展
$.extend({
    //改写$.get方法，对Url添加时间戳，确保每次请求，数据都是最新的
    //添加没有权限的处理
    get: function (url, data, callback, type) {
        // shift arguments if data argument was omited
        if (jQuery.isFunction(data)) {
            type = type || callback;
            callback = data;
            data = null;
        }
        if (url.indexOf("?") > 0) {

            url += "&Timestamp=" + this.getTime();
        } else {
            url += "?Timestamp=" + this.getTime();
        }
        return jQuery.ajax({
            type: "GET",
            url: url.replace(/\+/g, "%2B"),
            data: data,
            success: function (rdata) {
                if (rdata.result == 404) {
                    art.dialog({
                        id: 'SB',
                        content: rdata.content,
                        close: function () {
                            window.location.href = "/Login/Index?backUrl=" + encodeURIComponent(window.location.href);
                        },
                        ok: function () {
                            window.location.href = "/Login/Index?backUrl=" + encodeURIComponent(window.location.href);
                        }
                    });
                } else {
                    try {
                        callback(rdata);
                    } catch (e) {

                    }

                }
            },
            dataType: type
        });
    },
    //改写$.post方法，添加没有权限的处理
    post: function (url, data, callback, type) {
        // shift arguments if data argument was omited
        if (jQuery.isFunction(data)) {
            type = type || callback;
            callback = data;
            data = {};
        }

        return jQuery.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (rdata) {
                if (rdata.result == 404) {
                    art.dialog({
                        id: 'SB',
                        content: rdata.content,
                        close: function () {
                            window.location.href = "/Login/Index?backUrl=" + encodeURIComponent(window.location.href);
                        },
                        ok: function () {
                            window.location.href = "/Login/Index?backUrl=" + encodeURIComponent(window.location.href);
                        }
                    });
                } else {
                    try {
                        callback(rdata);
                    } catch (e) {

                    }
                }
            },
            dataType: type
        });
    },

    //jQuery.dataTable插件的回调函数
    //对内容过长的列添加addtitle样式，自动设置td的Title属性为内容
    completeTdTitle: function (tabid) {
        var tab = $("#" + tabid);
        $(tab).find("td.addtitle").each(function () {
            // $(this).html("<div class='curd' title='"+$(this).text()+"'>"+$(this).html()+"</div>");
            $(this).attr("title", $(this).text());
        });
    },
    //字符串的与C# String.Format类似的format方法
    format: function (source, params) {
        /// <summary>
        ///     格式化字符串
        ///     <param name="source">要格式化的字符串</param>
        ///     <param name="params">参数列表</param>
        /// </summary>
        if (arguments.length == 1) {
            return function () {
                var args = $.makeArray(arguments);
                args.unshift(source);
                return $.format.apply(this, args);
            };
        }
        if (arguments.length > 2 && params.constructor != Array) {
            params = $.makeArray(arguments).slice(1);
        }
        if (params.constructor != Array) {
            params = [params];
        }
        $.each(params, function (i, n) {
            source = source.replace(new RegExp("\\{" + i + "}", "g"), n);
        });
        return source;
    },
    //返回时间的数值表示形式
    getTime: function () {
        /// <summary>
        ///     获取时间戳
        /// </summary>
        return (new Date()).valueOf();
    },
    //Array的操作
    arrayRemove: function (array, obj) {
        var index = -1;
        for (var i = 0; i < array.length; i++) {
            if (array[i] == obj) {
                index = i;
                break;
            }
        }
        if (index >= 0) {
            $.arrayRemoveAt(array, index);
        }
    },
    arrayClear: function (array) {
        array.length = 0;
    },
    arrayRemoveAt: function (array, index) {
        array.splice(index, 1);
    },
    arrayInsertAt: function (array, index, obj) {
        array.splice(index, 0, obj);
    },
    //入队（将元素添加到数组）
    Array$enqueue: function (array, item) {
        /// <summary>入队（将元素添加到数组）</summary>
        /// <param name="array" type="Array" elementMayBeNull="true"></param>
        /// <param name="item" mayBeNull="true"></param>
        var e = Function._validateParams(arguments, [
            { name: "array", type: Array, elementMayBeNull: true },
            { name: "item", mayBeNull: true }
        ]);
        if (e) throw e;
        array[array.length] = item;
    },
    //添加一组数据
    Array$addRange: function (array, items) {
        /// <summary locid="M:J#Array.addRange" />
        /// <param name="array" type="Array" elementMayBeNull="true"></param>
        /// <param name="items" type="Array" elementMayBeNull="true"></param>
        var e = Function._validateParams(arguments, [
            { name: "array", type: Array, elementMayBeNull: true },
            { name: "items", type: Array, elementMayBeNull: true }
        ]);
        if (e) throw e;
        array.push.apply(array, items);
    },
    Array$clear: function (array) {
        /// <summary locid="M:J#Array.clear" />
        /// <param name="array" type="Array" elementMayBeNull="true"></param>
        var e = Function._validateParams(arguments, [
            { name: "array", type: Array, elementMayBeNull: true }
        ]);
        if (e) throw e;
        array.length = 0;
    },
    Array$clone: function (array) {
        /// <summary locid="M:J#Array.clone" />
        /// <param name="array" type="Array" elementMayBeNull="true"></param>
        /// <returns type="Array" elementMayBeNull="true"></returns>
        var e = Function._validateParams(arguments, [
            { name: "array", type: Array, elementMayBeNull: true }
        ]);
        if (e) throw e;
        if (array.length === 1) {
            return [array[0]];
        } else {
            return Array.apply(null, array);
        }
    },
    Array$contains: function (array, item) {
        /// <summary locid="M:J#Array.contains" />
        /// <param name="array" type="Array" elementMayBeNull="true"></param>
        /// <param name="item" mayBeNull="true"></param>
        /// <returns type="Boolean"></returns>
        var e = Function._validateParams(arguments, [
            { name: "array", type: Array, elementMayBeNull: true },
            { name: "item", mayBeNull: true }
        ]);
        if (e) throw e;
        return (Sys._indexOf(array, item) >= 0);
    },
    Array$dequeue: function (array) {
        /// <summary locid="M:J#Array.dequeue" />
        /// <param name="array" type="Array" elementMayBeNull="true"></param>
        /// <returns mayBeNull="true"></returns>
        var e = Function._validateParams(arguments, [
            { name: "array", type: Array, elementMayBeNull: true }
        ]);
        if (e) throw e;
        return array.shift();
    },
    Array$forEach: function (array, method, instance) {
        /// <summary locid="M:J#Array.forEach" />
        /// <param name="array" type="Array" elementMayBeNull="true"></param>
        /// <param name="method" type="Function"></param>
        /// <param name="instance" optional="true" mayBeNull="true"></param>
        var e = Function._validateParams(arguments, [
            { name: "array", type: Array, elementMayBeNull: true },
            { name: "method", type: Function },
            { name: "instance", mayBeNull: true, optional: true }
        ]);
        if (e) throw e;
        for (var i = 0, l = array.length; i < l; i++) {
            var elt = array[i];
            if (typeof (elt) !== 'undefined') method.call(instance, elt, i, array);
        }
    },
    Array$indexOf: function (array, item, start) {
        /// <summary locid="M:J#Array.indexOf" />
        /// <param name="array" type="Array" elementMayBeNull="true"></param>
        /// <param name="item" optional="true" mayBeNull="true"></param>
        /// <param name="start" optional="true" mayBeNull="true"></param>
        /// <returns type="Number"></returns>
        var e = Function._validateParams(arguments, [
            { name: "array", type: Array, elementMayBeNull: true },
            { name: "item", mayBeNull: true, optional: true },
            { name: "start", mayBeNull: true, optional: true }
        ]);
        if (e) throw e;
        return Sys._indexOf(array, item, start);
    },
    Array$insert: function (array, index, item) {
        /// <summary locid="M:J#Array.insert" />
        /// <param name="array" type="Array" elementMayBeNull="true"></param>
        /// <param name="index" mayBeNull="true"></param>
        /// <param name="item" mayBeNull="true"></param>
        var e = Function._validateParams(arguments, [
            { name: "array", type: Array, elementMayBeNull: true },
            { name: "index", mayBeNull: true },
            { name: "item", mayBeNull: true }
        ]);
        if (e) throw e;
        array.splice(index, 0, item);
    },
    Array$parse: function (value) {
        /// <summary locid="M:J#Array.parse" />
        /// <param name="value" type="String" mayBeNull="true"></param>
        /// <returns type="Array" elementMayBeNull="true"></returns>
        var e = Function._validateParams(arguments, [
            { name: "value", type: String, mayBeNull: true }
        ]);
        if (e) throw e;
        if (!value) return [];
        var v = eval(value);
        if (!Array.isInstanceOfType(v)) throw Error.argument('value', Sys.Res.arrayParseBadFormat);
        return v;
    },
    Array$remove: function (array, item) {
        /// <summary locid="M:J#Array.remove" />
        /// <param name="array" type="Array" elementMayBeNull="true"></param>
        /// <param name="item" mayBeNull="true"></param>
        /// <returns type="Boolean"></returns>
        var e = Function._validateParams(arguments, [
            { name: "array", type: Array, elementMayBeNull: true },
            { name: "item", mayBeNull: true }
        ]);
        if (e) throw e;
        var index = Sys._indexOf(array, item);
        if (index >= 0) {
            array.splice(index, 1);
        }
        return (index >= 0);
    },
    Array$removeAt: function (array, index) {
        /// <summary locid="M:J#Array.removeAt" />
        /// <param name="array" type="Array" elementMayBeNull="true"></param>
        /// <param name="index" mayBeNull="true"></param>
        var e = Function._validateParams(arguments, [
            { name: "array", type: Array, elementMayBeNull: true },
            { name: "index", mayBeNull: true }
        ]);
        if (e) throw e;
        array.splice(index, 1);
    }
});


//验证中华人民共和国居民身份证号码的正确性

function checkID(num) {
    var len = num.length, re;
    if (len == 15)
        re = new RegExp(/^(\d{6})()?(\d{2})(\d{2})(\d{2})(\d{3})$/);
    else if (len == 18)
        re = new RegExp(/^(\d{6})()?(\d{4})(\d{2})(\d{2})(\d{3})(\d)$/);
    else { return false; }
    var a = num.match(re);
    if (a != null) {
        if (len == 15) {
            var D = new Date("19" + a[3] + "/" + a[4] + "/" + a[5]);
            var B = D.getYear() == a[3] && (D.getMonth() + 1) == a[4] && D.getDate() == a[5];
        }
        else {
            var D = new Date(a[3] + "/" + a[4] + "/" + a[5]);
            var B = D.getFullYear() == a[3] && (D.getMonth() + 1) == a[4] && D.getDate() == a[5];
        }
        if (!B) { return false; }
    }
    return true;
    ;
}

(function ($) {
    $.fn.bgIframe = $.fn.bgiframe = function (s) {
        if ($.browser.msie && /6.0/.test(navigator.userAgent)) {
            s = $.extend({
                top: 'auto',
                left: 'auto',
                width: 'auto',
                height: 'auto',
                src: 'javascript:false;'
            }, s || {});
            var prop = function (n) {
                return n && n.constructor == Number ? n + 'px' : n;
            };
            return this.each(function () {
                if ($('> iframe.bgiframe', this).length == 0) {
                    var iframe = $('<iframe frameborder="0" tabindex="-1"></iframe>')
                        .addClass("bgiframe")
                        .css({
                            display: 'block',
                            position: 'absolute',
                            zIndex: '-1',
                            opacity: 0,
                            top: s.top === 'auto' ?
                                ((this.clientTop || 0) * -1 + 'px') : prop(s.top),
                            left: s.left === 'auto' ?
                                ((this.clientLeft || 0) * -1 + 'px') : prop(s.left),
                            width: s.width === 'auto' ?
                                (this.offsetWidth + 'px') : prop(s.width),
                            height: s.height === 'auto' ?
                                (this.offsetHeight + 'px') : prop(s.height)
                        })
                        .insertBefore(this.firstChild);
                }
            });
        }
        return this;
    };
})(jQuery);

//用于jQuery.dataTable fnDrawCallback回调函数内，传入Table的Id
//实现集合CheckBox选定，达到全选的效果

function SetCheckBox(allId, listId, fun) {
    $("#" + listId + " input[type='checkbox']").each(function () {
        $(this).bind("change", function () {
            var flag = true;
            $("#" + listId + " input[type='checkbox']").each(function () {
                if (!$(this).attr("checked") && flag && !$(this).attr("disabled"))
                    flag = false;
                if ($("#commonSelectedIdStr").length>0) {
                    var haved = $("#commonSelectedIdStr").val();
                    if ($(this).attr("checked")) {
                        if ((',' + haved + ',').indexOf(',' + $(this).attr('id') + ',') < 0) {
                            $("#commonSelectedIdStr").val(haved == "" ? $(this).attr('id') : (haved + (',' + $(this).attr('id'))));
                        }
                    } else {
                        if ((',' + haved + ',').indexOf(',' + $(this).attr('id') + ',') >= 0) {
                            haved = (',' + haved + ',').replace((',' + $(this).attr('id') + ','), ',');
                            if (haved.length <= 1) {
                                haved = "";
                            } else {
                                haved = haved.substring(0, haved.length - 1).substring(1, haved.length - 1);
                            }
                            $("#commonSelectedIdStr").val(haved);
                        }
                    }
                }
                if ($("#commonSelectedNameStr").length > 0) {
                    var haved = $("#commonSelectedNameStr").val();
                    if ($(this).attr("checked")) {
                        if ((';' + haved + ';').indexOf(';' + $(this).attr('id') + ',' + $(this).attr('username') + ';') < 0) {
                            $("#commonSelectedNameStr").val(haved == "" ? ($(this).attr('id') + ',' + $(this).attr('username')) : (haved+(';' + $(this).attr('id') + ',' + $(this).attr('username'))));
                        }
                    } else {
                        if ((';' + haved + ';').indexOf(';' + $(this).attr('id') + ',' + $(this).attr('username') + ';') >= 0) {
                            haved = (';' + haved + ';').replace((';' + $(this).attr('id') + ',' + $(this).attr('username') + ';'), ';');
                            if (haved.length <= 1) {
                                haved = "";
                            } else {
                                haved = haved.substring(0, haved.length - 1).substring(1, haved.length - 1);
                            }
                            $("#commonSelectedNameStr").val(haved);
                        }
                    }
                }
            });
            if (flag)
                $("#" + allId).attr("checked", true);
            else
                $("#" + allId).attr("checked", false);
        });
    });
    $("#" + allId).bind("change", function () {
        $("#" + listId + " input[type='checkbox']").each(function () {
            if (!$(this).attr("disabled")) {
                $(this).attr("checked", $("#" + allId).attr("checked") || false);
                if ($("#commonSelectedIdStr").length > 0) {
                    var haved = $("#commonSelectedIdStr").val();
                    if ($(this).attr("checked")) {
                        if ((',' + haved + ',').indexOf(',' + $(this).attr('id') + ',') < 0) {
                            $("#commonSelectedIdStr").val(haved == "" ? $(this).attr('id') : (haved+(',' + $(this).attr('id'))));
                        }
                    } else {
                        if ((',' + haved + ',').indexOf(',' + $(this).attr('id') + ',') >= 0) {
                            haved = (',' + haved + ',').replace((',' + $(this).attr('id') + ','), ',');
                            if (haved.length <= 1) {
                                haved = "";
                            } else {
                                haved = haved.substring(0, haved.length - 1).substring(1, haved.length - 1);
                            }
                            $("#commonSelectedIdStr").val(haved);
                        }
                    }
                }
                if ($("#commonSelectedNameStr").length > 0) {
                    var haved = $("#commonSelectedNameStr").val();
                    if ($(this).attr("checked")) {
                        if ((';' + haved + ';').indexOf(';' + $(this).attr('id') +','+ $(this).attr('username') + ';') < 0) {
                            $("#commonSelectedNameStr").val(haved == "" ? ($(this).attr('id') + ',' + $(this).attr('username')) : (haved+(';' + $(this).attr('id') + ',' + $(this).attr('username'))));
                        }
                    } else {
                        if ((';' + haved + ';').indexOf(';' + $(this).attr('id') + ',' + $(this).attr('username') + ';') >= 0) {
                            haved = (';' + haved + ';').replace((';' + $(this).attr('id') + ',' + $(this).attr('username') + ';'), ';');
                            if (haved.length <= 1) {
                                haved = "";
                            } else {
                                haved = haved.substring(0, haved.length - 1).substring(1, haved.length - 1);
                            }
                            $("#commonSelectedNameStr").val(haved);
                        }
                    }
                }
            }
        });
        if (fun)
            fun();
    });

}

//获取Table内被选定的Checkbox的所有Id，返回格式为{id1},{id2},{id3}....

function GetChecked(objId) {
    var str = '';
    $("#" + objId + " input[type='checkbox'],#" + objId + " input[type='radio']").each(function () {
        if ($(this).attr("checked")) {
            if (this.type == "radio") {
                str = $(this).attr("id") + "," + $(this).attr("username");
            } else {
                if (("," + str + ",").indexOf("," + $(this).attr("id") + ",") < 0) {
                    str += str.length > 0 ? (',' + $(this).attr("id")) : $(this).attr("id");
                }
            }
        }
    });
    return str;
}

//获取Table内被选定的Checkbox的所有Value，返回格式为{Value1},{Value2},{Value3}....

function GetCheckedValue(objId) {
    var str = '';
    $("#" + objId + " input[type='checkbox'],#" + objId + " input[type='radio']").each(function () {
        if ($(this).attr("checked")) {
            if (this.type == "radio") {
                str = $(this).val();
            } else {
                if (("," + str + ",").indexOf("," + $(this).val() + ",") < 0) {
                    str += str.length > 0 ? (',' + $(this).val()) : $(this).val();
                }
            }
        }
    });
    return str;
}


//获取当前页面Url中的参数

function getUrlParam(name) {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars[name];
}

//长度限制

function AutoIndent(objID) {
    var count = 0;
    $("#" + objID + " thead tr th").each(function () {
        if ($(this).attr("class").indexOf('autoIndent') >= 0) {
            var width = $(this).css("width").replace("px", "");
            var wordNumber = parseInt(parseInt(width) / 12) - 2; //要显示的字的个数
            $("#" + objID + " tbody tr").each(function () {
                var str = $(this).find("td").eq(count).html();
                $(this).find("td").eq(count).attr("title", str);
                $(this).find("td").eq(count).html(str.length > wordNumber ? (str.substring(0, wordNumber) + '…') : str);
            });
        }
        count++;
    });
}

function AutocompleteSearch(url, objId) {
    var availableTags = "";
    $.getJSON(url, function (data) {
        availableTags = data;
    });

    function split(val) {
        return val.split(/,\s*/);
    }

    function extractLast(term) {
        return split(term).pop();
    }

    $("#" + objId).bind("keydown", function (event) {
        if (event.keyCode === $.ui.keyCode.TAB && $(this).data("autocomplete").menu.active) {
            event.preventDefault();
        }
    }).autocomplete({
        minLength: 0,
        source: function (request, response) {
            response($.ui.autocomplete.filter(availableTags, extractLast(request.term)));
        },
        focus: function () {
            return false;
        },
        select: function (event, ui) {
            var terms = split(this.value);
            terms.pop();
            terms.push(ui.item.value);
            this.value = terms;
            return false;
        }
    });
}

function ShowPages(objId, index, pagesize) {
    var pagesize = pagesize == undefined ? 10 : pagesize;
    var obj = "#" + objId;
    var currentpage = index; //当前页号
    var totalCount = 0; //总条数
    if ($(obj).find("tbody tr").length > 1)
        totalCount = $(obj).find("tbody tr").length;
    else {
        if ($(obj).find("tbody tr td").length == 1)
            totalCount = 0;
        else
            totalCount = 1;
    }

    //显示数据
    for (var i = 1; i <= $(obj).find("tbody tr").length; i++) {
        if (i < ((index - 1) * pagesize + 1) || i > index * pagesize) {
            $(obj).find("tbody tr").eq(i - 1).hide();
        } else {
            $(obj).find("tbody tr").eq(i - 1).show();
        }
    }

    $(obj).find("span[id='singledatacount']").eq(0).html(totalCount); //总条数

    //分页
    var pagecount = totalCount % pagesize == 0 ? (parseInt(totalCount / pagesize)) : (parseInt(totalCount / pagesize) + 1);
    var pageStr = '';
    for (var i = 1; i <= pagecount; i++) {
        if (i == 1 && index > 2) {
            pageStr += '…';
        }
        if ((i >= parseInt(index) - 1 && i <= parseInt(index) + 1 && parseInt(index) >= 2 && parseInt(index) <= pagecount - 1) || (parseInt(index) == 1 && i <= 3) || (parseInt(index) == pagecount && i >= pagecount - 2)) {
            pageStr += '<a ';
            if (i == parseInt(index)) {
                pageStr += ' style="margin:0px 5px; font-size:12px; background-color:#FFF; color:Gray; text-decoration:none;" ';
            } else {
                pageStr += ' style="margin:0px 5px; font-size:12px; background-color:#FFF; color:#000;" onclick="ShowPages(\'' + objId + '\',' + i + ');"';
            }
            pageStr += '>' + i;
            pageStr += '</a>';
        }
        if (i == pagecount && index < pagecount - 1) {
            pageStr += '…';
        }
    }

    if (parseInt(index) == 1 || totalCount == 0) {
        $(obj + " #singlefirst," + obj + " #singleprevious").addClass("ui-state-disabled");
    } else {
        $(obj + " #singlefirst," + obj + " #singleprevious").removeClass("ui-state-disabled");
    }
    if (parseInt(index) == pagecount || totalCount == 0) {
        $(obj + " #singlenext," + obj + " #singlelast").addClass("ui-state-disabled");
    } else {
        $(obj + " #singlenext," + obj + " #singlelast").removeClass("ui-state-disabled");
    }
    $(obj + " #fromnumber").html(totalCount > 0 ? ((index - 1) * pagesize + 1) : 0);
    $(obj + " #tonumber").html(totalCount > 0 ? (totalCount - index * pagesize > 0 ? index * pagesize : totalCount) : 0);
    $(obj + " #singleprevious").attr("value", index == 1 ? 1 : index - 1);
    $(obj + " #singlenext").attr("value", index == pagecount ? pagecount : (parseInt(index) + 1));
    $(obj + " #singlelast").attr("value", pagecount);

    $(obj + " #singlepages").html('').append(pageStr);
    $(obj + " #singlefirst," + obj + " #singleprevious," + obj + " #singlenext," + obj + " #singlelast").unbind("click").bind("click", function () {
        ShowPages(objId, $(this).attr("value"));
    });
}

//获取时间戳

function getTimestamp() {
    var date = new Date();
    return date.valueOf();
}

function threeBackground(id, fun) {

    if (id != '0') {
        $("#navigation a[id='" + id + "']").eq(0).addClass("On");
    }
    //选择后定位
    $("#navigation a").unbind("click").bind("click", function () {
        $("#navigation a").removeClass("On");
        $(this).addClass("On");
        try {
            fun();
        } catch (e) {

        }
    });
}

/*获取浏览器类型*/

function identifyApp() {
    var appName;
    if (navigator.userAgent.indexOf("MSIE 7.0") >= 0) {
        appName = "IE7";
    } else if (navigator.userAgent.indexOf("MSIE 8.0") >= 0) {
        appName = "IE8";
    } else if (navigator.userAgent.indexOf("MSIE 9.0") >= 0) {
        appName = "IE9";
    } else if (navigator.userAgent.indexOf("MSIE 6.0") >= 0) {
        appName = "IE6";
    } else if (navigator.userAgent.indexOf("Firefox") >= 0) {
        appName = "FF";
    } else if (navigator.userAgent.indexOf("Chrome") >= 0) {
        appName = "Chrome";
    } else if (navigator.userAgent.indexOf("Opera") >= 0) {
        appName = "Opera";
    } else if (navigator.userAgent.indexOf("Safari") >= 0) {
        appName = "Safari";
    } else {
        appName = "Other"; //Safari, Chrome, ...
    }
    return appName;
}

//获取页面高度

function GetScreenHeight() {
    if (navigator.userAgent.indexOf("MSIE 7.0") >= 0) {
        return window.screen.height;
    } else if (navigator.userAgent.indexOf("MSIE 8.0") >= 0) {
        return window.screen.height;
    } else if (navigator.userAgent.indexOf("MSIE 9.0") >= 0) {
        return window.screen.height;
    } else if (navigator.userAgent.indexOf("MSIE 6.0") >= 0) {
        return window.screen.height;
    } else if (navigator.userAgent.indexOf("Firefox") >= 0) {
        return window.screen.height;
    } else if (navigator.userAgent.indexOf("Chrome") >= 0) {
        return window.screen.height;
    } else if (navigator.userAgent.indexOf("Opera") >= 0) {
        return window.screen.height;
    } else if (navigator.userAgent.indexOf("Safari") >= 0) {
        return window.screen.height;
    } else {
        return window.screen.height;
    }
}

//打开模式窗口

function winModalFullScreen(strURL) {
    if (identifyApp() != "Opera") {
        var sheight = window.screen.height;
        var swidth = window.screen.width;

        var winoption = "dialogHeight:" + sheight + "px;dialogWidth:" + swidth + "px;status:yes;scroll:yes;resizable:yes;center:yes;fullscreen=1";
        var tmp = window.showModalDialog(strURL, window, winoption);
        return tmp;
    } else {
        art.dialog({
            title: "小提示",
            lock: true,
            fixed: true,
            width: 350,
            height: 100,
            time: false,
            content: '暂不支持Opera浏览器，请使用其他浏览器',
            time: 3000
        });
    }
}


////去掉数组中重复的值
//Array.prototype.unique = function () {
//    var data = this || [];
//    var a = {}; //声明一个对象，javascript的对象可以当哈希表用  
//    for (var i = 0; i < data.length; i++) {
//        a[data[i]] = true;  //设置标记，把数组的值当下标，这样就可以去掉重复的值  
//    }
//    data.length = 0;
//    for (var i in a) { //遍历对象，把已标记的还原成数组  
//        this[data.length] = i;
//    }
//    return data;
//};

//Array.prototype.clear = function () {
//    this.length = 0;
//};
//Array.prototype.insertAt = function (index, obj) {
//    this.splice(index, 0, obj);
//};
//Array.prototype.removeAt = function (index) {
//    this.splice(index, 1);
//};
//Array.prototype.remove = function (obj) {
//    var index = -1;
//    for (var i = 0; i < this.length; i++) {
//        if (this[i] == obj) {
//            index = i;
//            break;
//        }
//    }
//    if (index >= 0) {
//        this.removeAt(index);
//    }
//};


//去掉左右两边空格
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
};

//去掉左边空格
String.prototype.lTrim = function () {
    return this.replace(/(^\s*)/g, "");
};

//去掉右边空格
String.prototype.rTrim = function () {
    return this.replace(/(\s*$)/g, "");
};

//日期格式
Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};

//获取光标在输入框的位置
function getPosition(obj) {
    var result = 0;
    if (obj.setSelectionRange) { //非IE浏览器
        result = obj.selectionStart;
    } else { //IE
        var rng;
        if (obj.tagName == "TEXTAREA") { //如果是文本域
            rng = event.srcElement.createTextRange();
            rng.moveToPoint(event.x, event.y);
        } else { //输入框
            rng = document.selection.createRange();
        }
        rng.moveStart("character", -event.srcElement.value.length);
        result = rng.text.length;
    }
    return result;
}

//设置光标在输入框的位置

function setLocation(elm, n) {
    if (n > elm.value.length)
        n = elm.value.length;
    if (elm.createTextRange) { // IE 
        var textRange = elm.createTextRange();
        textRange.moveStart('character', n);
        textRange.collapse();
        textRange.select();
    } else if (elm.setSelectionRange) { // Firefox 
        elm.setSelectionRange(n, n);
        elm.focus();
    }
}

//textarea的长度限制

function TextearaMaxlength(obj) {
    var iMaxLen = parseInt($(obj).attr('maxlength'));
    var iCurLen = $(obj).val().length;
    if ($(obj).attr('maxlength') && iCurLen > iMaxLen) {
        $(obj).val($(obj).val().substring(0, iMaxLen));
    }
}

function turnToNext(obj, rediction) {
    //图片的集合
    var pictureCollection = $(obj).parent().parent().find("#imageCollection img");
    //当前显示的第几个
    var index = $(obj).parent().parent().find("#imageCollection input").eq(0).val();
    if (rediction == "left") {
        //向左
        if (parseInt(index) != 1) {
            pictureCollection.css("display", "none");
            pictureCollection.eq(index - 2).animate({ opacity: 'show' }, 50);
            $(obj).parent().parent().find("#imageCollection input").eq(0).val(parseInt(index) - 1);
        } else {
            pictureCollection.css("display", "none");
            pictureCollection.eq(pictureCollection.length - 1).animate({ opacity: 'show' }, 50);
            $(obj).parent().parent().find("#imageCollection input").eq(0).val(pictureCollection.length);
        }
    } else {
        //向左
        if (parseInt(index) != pictureCollection.length) {
            pictureCollection.css("display", "none");
            pictureCollection.eq(index).animate({ opacity: 'show' }, 50);
            $(obj).parent().parent().find("#imageCollection input").eq(0).val(parseInt(index) + 1);
        } else {
            pictureCollection.css("display", "none");
            pictureCollection.eq(0).animate({ opacity: 'show' }, 50);
            $(obj).parent().parent().find("#imageCollection input").eq(0).val(1);
        }
    }
}

//验证时间的正确性

function checkDate(startDateID, endDateID, title, message) {
    var startDate = $("#" + startDateID).val();
    var endDate = $("#" + endDateID).val();
    if (startDate != "" && endDate != "") {
        var startLength = new Date(startDate.replace(/-/g, '/')).getTime();
        var endLength = new Date(endDate.replace(/-/g, '/')).getTime();
        if (startLength > endLength) {
            $("#" + startDateID).val("");
            $("#" + endDateID).val("");
            if (title != "" && title != undefined)
                art.dialog({ title: title, content: message, width: 300, height: 60, fixed: true, lock: true, time: 3000 });
            return false;
        }
    }
    return true;
}


/**/
/************************************************************************ 
| 函数名称： setCookie | 
| 函数功能： 设置cookie函数 | 
| 入口参数： name：cookie名称；value：cookie值 | 
*************************************************************************/

function setCookie(name, value) {
    var argv = setCookie.arguments;
    var argc = setCookie.arguments.length;
    var expires = (argc > 2) ? argv[2] : null;
    if (expires != null) {
        var largeExpDate = new Date();
        largeExpDate.setTime(largeExpDate.getTime() + (expires * 1000 * 3600 * 24));
    }
    document.cookie = name + "=" + escape(value) + ((expires == null) ? "" : ("; expires=" + largeExpDate.toGMTString()));
}

/**/
/************************************************************************ 
| 函数名称： getCookie | 
| 函数功能： 读取cookie函数 | 
| 入口参数： name：cookie名称 | 
*************************************************************************/

function getCookie(name) {
    var search = name + "=";
    if (document.cookie.length > 0) {
        var offset = document.cookie.indexOf(search);
        if (offset != -1) {
            offset += search.length;
            var end = document.cookie.indexOf(";", offset);
            if (end == -1) end = document.cookie.length;
            return unescape(document.cookie.substring(offset, end));
        } else return "";
    }
    return "";
}

/**/
/************************************************************************ 
| 函数名称： deleteCookie | 
| 函数功能： 删除cookie函数 | 
| 入口参数： Name：cookie名称 | 
*************************************************************************/

function deleteCookie(name) {
    var expdate = new Date();
    expdate.setTime(expdate.getTime() - (86400 * 1000 * 1));
    setCookie(name, "", expdate);
}

function htmlEncode(str) {
    var div = document.createElement("div");
    div.appendChild(document.createTextNode(str));
    return div.innerText;
}

function htmlDecode(str) {
    var div = document.createElement("div");
    div.innerHTML = str;
    return div.innerHTML;
}

function html_decode(str) {
    var s = "";
    if (str.length == 0) return "";
    s = str.replace(/&amp;/g, "&");
    s = s.replace(/&lt;/g, "<");
    s = s.replace(/&gt;/g, ">");
    s = s.replace(/&nbsp;/g, " ");
    s = s.replace(/&#39;/g, "\'");
    s = s.replace(/&quot;/g, "\"");
    s = s.replace(/<br>/g, "\n");
    return s;
}

function htmlreplace(str) {
    var s = "";
    if (str.length == 0) return "";
    s = str.replace(/</g, "＜");
    s = str.replace(/>/g, "＞");
}

//删除隐藏域中的一个，“，”隔开的形式
function ReomveArray(hidID, id) {
    var arr = $("#" + hidID).val().split(',');
    var newarr = [];
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] != id) {
            newarr.push(arr[i]);
        }
    }

    var str = "";
    $.each(newarr, function (index, value) {
        str = str == "" ? value : str + "," + value;
    });
    $("#" + hidID).val(str);
}


//#region 尝试解决flash在IE下面的bug 即更改标题成描点(类似于：点击上传，标题变成#2)
var value; //页面原标题
//添加事件函数
function addEvent(obj, fun, eventName, name) {
    value = name;
    if (obj.addEventListener)
        obj.addEventListener(eventName, fun, false);
    else
        obj.attachEvent("on" + eventName, fun);
}
//修复标题
function repair() {
    document.title = value;
}

//在浏览器加载完成事件时执行一次可以使用户体验更好
function init() {
    if (document.all) {
        repair();
        //定时刷新页面标题,主要是针对单击Flash时页面标题自动改变的现象
        setInterval(repair, 100);
    }
}

//#endregion