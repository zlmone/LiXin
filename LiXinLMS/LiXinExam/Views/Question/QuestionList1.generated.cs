﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.544
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LiXinExam.Views.Question
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 1 "..\..\Views\Question\QuestionList.cshtml"
    using LiXinLanguage;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.3.2.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Question/QuestionList.cshtml")]
    public class QuestionList : System.Web.Mvc.WebViewPage<dynamic>
    {
        public QuestionList()
        {
        }
        public override void Execute()
        {


            
            #line 2 "..\..\Views\Question\QuestionList.cshtml"
  
    ViewBag.Title = NavigateMenuLanguage.QuestionManage;
    Layout = "~/Views/Shared/_Layout.cshtml";
    var sortID = Request.QueryString["sortID"] ?? "0";


            
            #line default
            #line hidden
WriteLiteral("<div id=\"contentInfor\" class=\"main-c\">\r\n    ");


            
            #line 8 "..\..\Views\Question\QuestionList.cshtml"
Write(Html.Action("SiteMapLink", "Common", new
{
    linkName = "QuestionManage"
}));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <div class=\"clb\">\r\n        <div class=\"fl span29\">\r\n            <div class=" +
"\"menu-list\">\r\n                <h3><i class=\"il\"></i><i class=\"ir\"></i>题库分类</h3>\r" +
"\n                <div class=\"list-con\"><div class=\"list-do\" style=\"padding-botto" +
"m: 5px;\">\r\n                    <input type=\"button\" id=\"addSort\" value=\"新增\" oncl" +
"ick=\"addSort();\" class=\"btn btn-co\"/>\r\n                    <input type=\"button\" " +
"id=\"modifySort\" value=\"修改\" onclick=\"modifySort();\" class=\"btn btn-co\"/>\r\n       " +
"             <input type=\"button\" id=\"deleteSort\" value=\"删除\" onclick=\"deleteSort" +
"();\" class=\"btn btn-cancel\"/>\r\n                </div></div>\r\n            </div>\r" +
"\n            <div class=\"treeview-box span29\" style=\"margin-top:-5px;\"><div id=\"" +
"sortList\" class=\"tree-list\"></div></div>\r\n        </div>\r\n        <div class=\"fr" +
" span70\">\r\n            <!--搜索-->\r\n            <div id=\"Search\" class=\"so-set\">\r\n" +
"                <table class=\"tab-Form\">\r\n                    <tr>\r\n            " +
"            <td class=\"Tit span5\">知识点：</td>\r\n                        <td class=\"" +
"span15\"><input id=\"txtQuestionKey\" type=\"text\" class=\"span14 searchclass\" value=" +
"\"请输入搜索内容\" info=\"\" /></td>\r\n                        <td class=\"Tit span6\">试题题干：</" +
"td>\r\n                        <td><input id=\"txtQuestionContent\" type=\"text\" clas" +
"s=\"span20 searchclass\" value=\"请输入搜索内容\" info=\"\" /></td>\r\n                        " +
"<td class=\"so-do\">\r\n                            <input id=\"btnSearch\" class=\"btn" +
"\" type=\"button\" value=\"搜索\" name=\"搜索\" onclick=\" InitlQUestion(); \" />\r\n          " +
"                  <a id=\"btnAddQuestion\" class=\"btn btn-co\">");


            
            #line 35 "..\..\Views\Question\QuestionList.cshtml"
                                                                 Write(Question.Question_QuestionManage_AddQuestion);

            
            #line default
            #line hidden
WriteLiteral(@"</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div class=""so-seq"">
                <select id=""selQuestionType"" onchange="" InitlQUestion(); "">
                    <option value=""0"">所有题型</option>
                    <option value=""1"">");


            
            #line 43 "..\..\Views\Question\QuestionList.cshtml"
                                 Write(Question.Question_QuestionManage_Subject);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                    <option value=\"2\">");


            
            #line 44 "..\..\Views\Question\QuestionList.cshtml"
                                 Write(Question.Question_QuestionManage_Single);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                    <option value=\"3\">");


            
            #line 45 "..\..\Views\Question\QuestionList.cshtml"
                                 Write(Question.Question_QuestionManage_Mulitple);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                    <option value=\"4\">");


            
            #line 46 "..\..\Views\Question\QuestionList.cshtml"
                                 Write(Question.Question_QuestionManage_Judge);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                    <option value=\"5\">");


            
            #line 47 "..\..\Views\Question\QuestionList.cshtml"
                                 Write(Question.Question_QuestionManage_FillBlank);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                    <option value=\"6\">");


            
            #line 48 "..\..\Views\Question\QuestionList.cshtml"
                                 Write(Question.Question_QuestionManage_Mediea);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                </select>\r\n                <select id=\"selQuestionLeve" +
"l\" onchange=\" InitlQUestion(); \">\r\n                    <option value=\"0\">所有难度</o" +
"ption>\r\n                    <option value=\"1\">");


            
            #line 52 "..\..\Views\Question\QuestionList.cshtml"
                                 Write(Question.Question_QuestionManage_EasyLevel);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                    <option value=\"2\">");


            
            #line 53 "..\..\Views\Question\QuestionList.cshtml"
                                 Write(Question.Question_QuestionManage_CommonLevel);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                    <option value=\"3\">");


            
            #line 54 "..\..\Views\Question\QuestionList.cshtml"
                                 Write(Question.Question_QuestionManage_HardLevel);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                </select>\r\n                ");



WriteLiteral("\r\n                <div class=\"list-do\">\r\n                    <a id=\"btnDeleteQues" +
"tion\">");


            
            #line 60 "..\..\Views\Question\QuestionList.cshtml"
                                         Write(Question.Question_QuestionManage_DeleteQuestion);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n                    <a id=\"btnImportQuestion\">");


            
            #line 61 "..\..\Views\Question\QuestionList.cshtml"
                                         Write(Question.Question_QuestionManage_ImportQuestion);

            
            #line default
            #line hidden
WriteLiteral(@"</a>
                    <a id=""btnQuestionView"" onclick="" Browse(); "">预览试题</a>
                </div>
            </div>
            <!-- 列表 -->
            <div id=""Content"">
                <table class=""tab-List mt10"">
                    <thead>
                        <tr>
                            <th class=""span4""><input type=""checkbox"" value=""0"" id=""selectall"" /></th>
                            <th>试题题干</th>
                            <th class=""span8"">题型(难易度)</th>
                            <th class=""span8"">调用次数</th>
                            <th class=""span10"">发布时间</th>
                            <th class=""span8"">操作</th>
                        </tr>
                    </thead>
                    <tbody id=""questionList"">
                    </tbody>
                    <tfoot>
                    </tfoot>
                </table>
                ");



WriteLiteral("\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n<input id=\"selQuestionS" +
"ort\" type=\"hidden\" value=\"");


            
            #line 94 "..\..\Views\Question\QuestionList.cshtml"
                                            Write(sortID);

            
            #line default
            #line hidden
WriteLiteral(@""" />
<script id=""questionListTemplate"" type=""text/x-jsrender"">
    {{for #data}}
      <tr>
        <td><input id=""{{:id}}"" type=""checkbox""/></td>
        <td>
            <div class=""tl"">
                <p title=""{{:QuestionContent}}""><strong>{{:QuestionContent.length>25?QuestionContent.substring(0,25)+""......"":QuestionContent}}</strong></p>
                <p class=""Info""><strong>{{:Creater}}</strong> 发布<i>|</i>知识点: <span title=""{{:QuestionKey}}"">{{:QuestionKey.length>5?QuestionKey.substring(0,5)+""..."":QuestionKey}}</span></p>
            </div>
        </td>
        <td>
            <div class=""tl"">
                <p>{{:QuestionTypeStr}}</p>
                <p>{{:QuestionLevelStr}}</p>
            </div>
        </td>
		<td><strong>{{:VoidTimes}}</strong> 次</td>
        <td><div class=""tl"">{{:CreatLocalTime}}</div></td>
        <td>
		    <a onclick=""EditQuestion({{:id}});"" title=""编辑"" class=""icon iedit""></a>  
 		    <a onclick=""ViewQuestion({{:id}});"" title=""查看详情"" class=""icon iview""></a>
        </td>
    </tr>
    {{/for}}
</script>

<script type=""text/javascript"">
    $(document).ready(function () {
        //初始化查询条件
        initSearch();
        $(document).attr(""title"");
    });

    //新增分类
    function addSort() {
        if ($(""#selQuestionSort"").val() != """") {
            art.dialog.load(""/Question/SortEdit?id=0&fatherID="" + $(""#selQuestionSort"").val() + ""&t="" + new Date(), { title: """);


            
            #line 131 "..\..\Views\Question\QuestionList.cshtml"
                                                                                                                         Write(Question.Question_QuestionManage_AddSort);

            
            #line default
            #line hidden
WriteLiteral("\", id: \"qSortEdit\", height: 250 });\r\n        } else {\r\n            art.dialog.tip" +
"s(\'请选择父题库\', 3);\r\n            //art.dialog({ title: \'");


            
            #line 134 "..\..\Views\Question\QuestionList.cshtml"
                              Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral(@"', content: ""请选择父题库"", width:200, height: 50, fixed: true, lock: true, time: 3 });
        }
    }

    //修改分类
    function modifySort() {
        if ($(""#selQuestionSort"").val() != """" && $(""#selQuestionSort"").val() != ""0"") {
            art.dialog.load(""/Question/SortEdit?id="" + $(""#selQuestionSort"").val() + ""&t="" + new Date(), { title: """);


            
            #line 141 "..\..\Views\Question\QuestionList.cshtml"
                                                                                                              Write(Question.Question_QuestionManage_ModifySort);

            
            #line default
            #line hidden
WriteLiteral("\", id: \"qSortEdit\", height: 250 });\r\n        } else {\r\n            art.dialog.tip" +
"s(\'请选择题库\', 3);\r\n            //art.dialog({ title: \'");


            
            #line 144 "..\..\Views\Question\QuestionList.cshtml"
                              Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral(@"', content: ""请选择题库"", width: 200, height: 50, fixed: true, lock: true, time: 3 });
        }
    }

    //删除分类
    function deleteSort() {
        if ($(""#selQuestionSort"").val() != """" && $(""#selQuestionSort"").val() != ""0"") {
            art.dialog({
                title: """);


            
            #line 152 "..\..\Views\Question\QuestionList.cshtml"
                   Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\",\r\n                content: \"");


            
            #line 153 "..\..\Views\Question\QuestionList.cshtml"
                     Write(CommonLanguage.Common_Confirm_Delete);

            
            #line default
            #line hidden
WriteLiteral("\",\r\n                width: 300,\r\n                okValue: \'");


            
            #line 155 "..\..\Views\Question\QuestionList.cshtml"
                     Write(CommonLanguage.Common_Sure);

            
            #line default
            #line hidden
WriteLiteral("\',\r\n                cancelValue: \'");


            
            #line 156 "..\..\Views\Question\QuestionList.cshtml"
                         Write(CommonLanguage.Common_Cancel);

            
            #line default
            #line hidden
WriteLiteral("\',\r\n                height: 50,\r\n                fixed: true,\r\n                lo" +
"ck: true,\r\n                ok: function () {\r\n                    $.getJSON(\"/Qu" +
"estion/DeleteQuestionSortByID?id=\" + $(\"#selQuestionSort\").val(), function (data" +
") {\r\n                        if (data.result == 1) {\r\n                          " +
"  art.dialog.tips(data.content, 3);\r\n                            $(\"#selQuestion" +
"Sort\").val(\'0\');\r\n                            initQuestionSort();\r\n             " +
"           } else {\r\n                            art.dialog.tips(data.content, 3" +
");\r\n                        }\r\n                    });\r\n                },\r\n    " +
"            cancel: function () {\r\n                }\r\n            });\r\n        }" +
" else\r\n            art.dialog.tips(\"请先选择题库\", 3);\r\n    }\r\n\r\n    //切换显示方式\r\n\r\n    f" +
"unction changeView(str)\r\n    {\r\n        ChangeList({ str: str, fun: function () " +
"{ InitlQUestion(); } });\r\n    }\r\n\r\n    $(document).ready(function ()\r\n    {\r\n   " +
"     initQuestionSort();\r\n        //初始化问题\r\n        InitlQUestion();\r\n\r\n    });\r\n" +
"\r\n\r\n    //生成url参数字符串\r\n\r\n    function getUrlParms()\r\n    {\r\n        return \'/Ques" +
"tion/GetAllQuestion?sortID=\' + $(\"#selQuestionSort\").val() +\r\n            \'&type" +
"=\' + $(\"#selQuestionType\").val() +\r\n            \'&level=\' + $(\"#selQuestionLevel" +
"\").val() +\r\n            \'&key=\' + escape(getSearchWord(\"txtQuestionKey\")) +\r\n   " +
"         \'&content=\' + escape(getSearchWord(\"txtQuestionContent\")) +\r\n          " +
"  \'&t=\' + new Date();\r\n    }\r\n\r\n    //初始化问题\r\n\r\n    function InitlQUestion()\r\n   " +
" {\r\n        var templateId = \'\';\r\n\r\n        $(\"#questionList\").JsRenderData({\r\n " +
"           sourceUrl: getUrlParms(),\r\n            isPage: true,\r\n            pag" +
"eSize: 10,\r\n            pageIndex: 1,\r\n            templateID: \'questionListTemp" +
"late\',\r\n            funCallback: function(data){\r\n                $(\"#selectall\"" +
").removeAttr(\"checked\");\r\n            }\r\n        });\r\n        SetCheckBox(\'selec" +
"tall\', \'questionList\');\r\n       \r\n    }\r\n\r\n    //初始化题库\r\n\r\n    function initQuest" +
"ionSort()\r\n    {\r\n        $.getJSON(\"GetAllQuestionSortTree\", function (data)\r\n " +
"       {\r\n            $(\"#sortList\").html(data);\r\n\r\n            //树的显示\r\n        " +
"    $(\"#navigation\").treeview({\r\n                persist: \"location\",\r\n         " +
"       collapsed: false,\r\n                unique: false\r\n            });\r\n      " +
"      $(\"#navigation>li>ul>li>div\").each(function () {\r\n                $(this)." +
"click();\r\n            });\r\n        });\r\n    }\r\n\r\n    //问题分类树点击事件\r\n\r\n    function" +
" selectSort(id, obj)\r\n    {\r\n        $(\"#selQuestionSort\").val(id);\r\n        pos" +
"tNodeClickSelect(obj);\r\n        //初始化问题\r\n        InitlQUestion();\r\n    }\r\n\r\n    " +
"function postNodeClickSelect(obj)\r\n    {\r\n        $(\"#sortList\").find(\"a\").each(" +
"function ()\r\n        {\r\n            $(this).removeClass(\"On\");\r\n        });\r\n\r\n " +
"       $(obj).addClass(\"On\");\r\n    }\r\n\r\n\r\n    //新增试题\r\n    $(\"#btnAddQuestion\").b" +
"ind(\"click\", function ()\r\n    {\r\n        if (parseInt($(\"#selQuestionSort\").val(" +
")) > 0)\r\n            window.location.href = \"/Question/QuestionEdit?id=0&sortID=" +
"\" + $(\"#selQuestionSort\").val();\r\n        else\r\n            art.dialog.tips(\"");


            
            #line 273 "..\..\Views\Question\QuestionList.cshtml"
                        Write(Question.Question_QuestionManage_Tip_SelectSort);

            
            #line default
            #line hidden
WriteLiteral("\", 3);\r\n    });\r\n\r\n    //删除所选的试题\r\n    $(\"#btnDeleteQuestion\").bind(\"click\", funct" +
"ion ()\r\n    {\r\n        var delStr = GetChecked(\"questionList\");\r\n        if (del" +
"Str != \"\")\r\n        {\r\n            art.dialog({\r\n                title: \"");


            
            #line 283 "..\..\Views\Question\QuestionList.cshtml"
                   Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\",\r\n                content: \'");


            
            #line 284 "..\..\Views\Question\QuestionList.cshtml"
                     Write(CommonLanguage.Common_Confirm_Delete);

            
            #line default
            #line hidden
WriteLiteral("\',\r\n                width: 200,\r\n                okValue: \'");


            
            #line 286 "..\..\Views\Question\QuestionList.cshtml"
                     Write(CommonLanguage.Common_Sure);

            
            #line default
            #line hidden
WriteLiteral("\',\r\n                cancelValue: \'");


            
            #line 287 "..\..\Views\Question\QuestionList.cshtml"
                         Write(CommonLanguage.Common_Cancel);

            
            #line default
            #line hidden
WriteLiteral(@"',
                height: 50,
                fixed: true,
                lock: true,
                ok: function ()
                {
                    $.getJSON(""/Question/DeleteSelectQuestions?ids="" + delStr, function (data)
                    {
                        if (data.result == 1)
                        {
                            art.dialog.tips(data.content, 3);
                            //art.dialog({ title: '");


            
            #line 298 "..\..\Views\Question\QuestionList.cshtml"
                                              Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral(@"', content: data.content, width: 200, height: 50, fixed: true, lock: true, time: 3 });
                            //初始化问题
                            InitlQUestion();
                        } else
                            art.dialog.tips(data.content, 3);
                            //art.dialog({ title: '");


            
            #line 303 "..\..\Views\Question\QuestionList.cshtml"
                                              Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral(@"', content: data.content, width: 200, height: 50, fixed: true, lock: true, time: 3 });
                    });
                },
                cancel: function ()
                {
                }
            });
        } else
        {
            art.dialog.tips(""");


            
            #line 312 "..\..\Views\Question\QuestionList.cshtml"
                        Write(Question.Question_QuestionManage_Tip_SelectQuestion);

            
            #line default
            #line hidden
WriteLiteral("\", 3);\r\n            //art.dialog({ title: \'");


            
            #line 313 "..\..\Views\Question\QuestionList.cshtml"
                              Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\', content: \'");


            
            #line 313 "..\..\Views\Question\QuestionList.cshtml"
                                                                     Write(Question.Question_QuestionManage_Tip_SelectQuestion);

            
            #line default
            #line hidden
WriteLiteral(@"', width: 200, height: 50, fixed: true, lock: true, time: 3 });
        }
    });


    //批量导入试题
    $(""#btnImportQuestion"").bind(""click"", function ()
    {
        if (parseInt($(""#selQuestionSort"").val()) > 0)
            art.dialog.load(""/Question/ImportQuestions?id="" + $(""#selQuestionSort"").val(), { title: """);


            
            #line 322 "..\..\Views\Question\QuestionList.cshtml"
                                                                                                Write(Question.Question_QuestionManage_ImportQuestion);

            
            #line default
            #line hidden
WriteLiteral("\", id: \"ImportQue\", width: 400 });\r\n        else\r\n        {\r\n            art.dial" +
"og.tips(\"");


            
            #line 325 "..\..\Views\Question\QuestionList.cshtml"
                        Write(Question.Question_QuestionManage_Tip_SelectSort);

            
            #line default
            #line hidden
WriteLiteral(@""", 3);
        }
    });

    //修改试题

    function EditQuestion(id)
    {
        window.location.href = ""/Question/QuestionEdit?id="" + id + ""&sortID="" + $(""#selQuestionSort"").val();
    }

    //预览试题，列表内的操作

    function ViewQuestion(id)
    {
        window.location.href = ""/Question/BrowseQuestion?ids=["" + id + ""]&sortID="" + $(""#selQuestionSort"").val();
    }

    //预览试题，全体操作

    function Browse()
    {
        var viewStr = GetChecked(""questionList"");
        if (viewStr != """")
        {
            ViewQuestion(viewStr);
        } else
        {
            //art.dialog({ title: '");


            
            #line 353 "..\..\Views\Question\QuestionList.cshtml"
                              Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\', content: \'");


            
            #line 353 "..\..\Views\Question\QuestionList.cshtml"
                                                                     Write(Question.Question_QuestionManage_ViewQuestion);

            
            #line default
            #line hidden
WriteLiteral("\', width: 200, height: 50, fixed: true, lock: true, time: 3 });\r\n            art." +
"dialog.tips(\"");


            
            #line 354 "..\..\Views\Question\QuestionList.cshtml"
                        Write(Question.Question_QuestionManage_ViewQuestion);

            
            #line default
            #line hidden
WriteLiteral("\", 3);\r\n        }\r\n    }\r\n</script>\r\n");


        }
    }
}
#pragma warning restore 1591
