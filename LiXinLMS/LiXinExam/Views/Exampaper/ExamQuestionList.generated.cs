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

namespace LiXinExam.Views.Exampaper
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
    
    #line 1 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
    using LiXinLanguage;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.3.2.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Exampaper/ExamQuestionList.cshtml")]
    public class ExamQuestionList : System.Web.Mvc.WebViewPage<dynamic>
    {
        public ExamQuestionList()
        {
        }
        public override void Execute()
        {


            
            #line 2 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
  
    Layout = null;


            
            #line default
            #line hidden


WriteLiteral("\r\n<style type=\"text/css\">\r\n    .subTreeChild {\r\n        height: 20px;\r\n        li" +
"ne-height: 20px;\r\n    }\r\n\r\n    .treeBanch { margin-left: 24px; }\r\n</style>\r\n<scr" +
"ipt src=\"");


            
            #line 14 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
        Write(Url.Content("~/Scripts/jquery.contextmenu.r2.js"));

            
            #line default
            #line hidden
WriteLiteral(@""" type=""text/javascript""> </script>
<div class=""clb"">
    <input id=""selQuestionSort"" type=""hidden"" value=""0"" />
    <div class=""fl span20 treeview-box"">
        <div id=""sortList"" class=""tree-list""></div>
    </div>     
    <div class=""fr span65 ml10"">
        <div id=""Search"" class=""so-set"">
            <table class=""tab-Form"">
                <tr>
                    <td class=""Tit span4"">");


            
            #line 24 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                                     Write(Exampaper.QuestionContent);

            
            #line default
            #line hidden
WriteLiteral("：</td>\r\n                    <td><input type=\"text\" class=\"span30\" id=\"txtQuestion" +
"Content\" /></td>\r\n                    <td class=\"so-do\"><input type=\"button\" id=" +
"\"btnSearch\" value=\"");


            
            #line 26 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                                                                            Write(Exampaper.Search);

            
            #line default
            #line hidden
WriteLiteral("\" class=\"btn\" /></td>\r\n                </tr>\r\n            </table>\r\n        </div" +
">\r\n        <div class=\"so-seq\">\r\n            <select id=\"selQuestionType\">\r\n    " +
"            <option value=\"0\">");


            
            #line 32 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                             Write(Exampaper.All);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                <option value=\"1\">");


            
            #line 33 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                             Write(Exampaper.Subject);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                <option value=\"2\">");


            
            #line 34 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                             Write(Exampaper.Single);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                <option value=\"3\">");


            
            #line 35 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                             Write(Exampaper.Multi);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                <option value=\"4\">");


            
            #line 36 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                             Write(Exampaper.Judge);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                <option value=\"5\">");


            
            #line 37 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                             Write(Exampaper.Fill);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                <option value=\"6\">");


            
            #line 38 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                             Write(Exampaper.Scene);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n            </select>\r\n            <select id=\"selQuestionLevel\">\r\n   " +
"             <option value=\"0\">");


            
            #line 41 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                             Write(CommonLanguage.Common_All);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                <option value=\"1\">");


            
            #line 42 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                             Write(Question.Question_QuestionManage_EasyLevel);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                <option value=\"2\">");


            
            #line 43 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                             Write(Question.Question_QuestionManage_CommonLevel);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                <option value=\"3\">");


            
            #line 44 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                             Write(Question.Question_QuestionManage_HardLevel);

            
            #line default
            #line hidden
WriteLiteral(@"</option>
            </select>
        </div>
        <div id=""question"">
            <table class=""tab-List mt10"">
                <thead>
                    <tr>
                        <th class=""span4"">
                            <input id=""selectQueall"" type=""checkbox"" />
                        </th>
                        <th>
                            ");


            
            #line 55 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                       Write(Question.Question_QuestionManage_QuestionContent);

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </th>\r\n                        <th>\r\n                  " +
"          ");


            
            #line 58 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                       Write(Question.Question_QuestionManage_Type);

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </th>\r\n                        <th>\r\n                  " +
"          ");


            
            #line 61 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                       Write(Question.Question_QuestionManage_Level);

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </th>\r\n                        <th>\r\n                  " +
"          ");


            
            #line 64 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                       Write(Question.Question_QuestionManage_VoidTimes);

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </th>\r\n                        <th>\r\n                  " +
"          ");


            
            #line 67 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                       Write(CommonLanguage.Common_Do);

            
            #line default
            #line hidden
WriteLiteral(@"
                        </th>
                    </tr>
                </thead>
                <tbody id=""questionList"">
                </tbody>
                <tfoot>
                </tfoot>
            </table>
        </div>
    </div>
</div>
<center class=""mt10""><input type=""button"" id=""btnAdd"" class=""btn"" value=""");


            
            #line 79 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                                                                    Write(Exampaper.Add);

            
            #line default
            #line hidden
WriteLiteral("\" /></center>\r\n<script id=\"QuestionKeyTemplate\" type=\"text/x-jsrender\">\r\n    {{fo" +
"r #data}}\r\n    <tr>\r\n        <td><input name=\"checkquestion\" type=\"checkbox\" val" +
"ue=\"{{:id}}\" /></td>\r\n        <td><div class=\"tl\" title=\"{{:QuestionContent}}\">{" +
"{:QuestionContent.length>20?(QuestionContent.substring(0,20)+\"…\"):QuestionConten" +
"t}}</div></td>\r\n        <td>{{:QuestionTypeStr}}</td>\r\n        <td>{{:QuestionLe" +
"velStr}}</td>\r\n        <td><strong>{{:VoidTimes}}</strong> 次</td>\r\n        <td><" +
"a target=\"_blank\" onclick=\"window.open(\'/Question/BrowseQuestion?ids=[{{:id}}]&b" +
"ack=noback\');\" title=\"查看\" class=\"icon iview\"></a></td>\r\n    </tr>\r\n    {{/for}}\r" +
"\n</script>\r\n<script type=\"text/javascript\">\r\n    $.ajaxSetup({\r\n        cache: f" +
"alse //关闭AJAX相应的缓存\r\n    });\r\n\r\n    function QuestionIDs() {\r\n        var tmp = $" +
"(\'#divExamQuestions\').find(\'input[type=hidden]\');\r\n        var eqids = \"\";\r\n    " +
"    for (var i = 0; i < tmp.length; i++) {\r\n            if (tmp[i].value != \"\") " +
"{\r\n                var msg = tmp[i].value.split(\"|\");\r\n                eqids += " +
"msg[0] + \",\";\r\n            }\r\n        }\r\n        var qidhtml = \'<input type=\"hid" +
"den\" id=\"QuestionIDs\" value=\"\' + eqids + \'\"/>\';\r\n        $(\"#hiddExamQuestions\")" +
".html(qidhtml);\r\n    }\r\n\r\n    $(document).ready(function() {\r\n        QuestionID" +
"s();\r\n        //初始化题库\r\n        initQuestionSort();\r\n        //初始化列表\r\n        Ini" +
"tializeTargetList(getUrlParms());\r\n\r\n        //****下拉框\r\n        //问题类别\r\n        " +
"$(\"#selQuestionType\").change(function() {\r\n            InitializeTargetList(getU" +
"rlParms());\r\n\r\n        });\r\n\r\n        //问题难度\r\n        $(\"#selQuestionLevel\").cha" +
"nge(function() {\r\n            InitializeTargetList(getUrlParms());\r\n        });\r" +
"\n\r\n        //****按钮事件\r\n        //查询\r\n        $(\"#btnSearch\").bind(\"click\", funct" +
"ion() {\r\n            InitializeTargetList(getUrlParms());\r\n        });\r\n\r\n      " +
"  //添加\r\n        $(\"#btnAdd\").bind(\"click\", function() {\r\n            var flag = " +
"false;\r\n            var check = document.getElementsByName(\"checkquestion\");\r\n  " +
"          var checkList = \"\";\r\n            for (var i = 0; i < check.length; i++" +
") {\r\n                if (check[i].checked) {\r\n                    checkList = ch" +
"eckList + check[i].value + \",\";\r\n                    flag = true;\r\n             " +
"   }\r\n            }\r\n            if (!flag) {\r\n                art.dialog.tips(\'" +
"");


            
            #line 147 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                            Write(Question.Question_QuestionManage_Tip_SelectQuestion);

            
            #line default
            #line hidden
WriteLiteral("\', 1.5);\r\n                //art.dialog({ title: \'");


            
            #line 148 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                                  Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\', content: \'");


            
            #line 148 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                                                                         Write(Question.Question_QuestionManage_Tip_SelectQuestion);

            
            #line default
            #line hidden
WriteLiteral(@"', width: 200, height: 50, fixed: true, lock: true, time: 1000 });
                return false;
            } else {
                if (checkList != """") {
                    $.ajax({
                        type: ""post"",
                        url: ""/Exampaper/GetQuestion?id="" + checkList,
                        dataType: ""json"",
                        success: function(data) {
                            var strhtml = '';
                            for (var i = 1; i <= data.length; i++) {
                                var Juid = ""QuestionID"" + data[i - 1]._id;
                                //判断题库是否存在
                                if (document.getElementById(Juid) == null) {
                                    strhtml += '<div class=""QSingle"" id=""QuestionID' + data[i - 1]._id + '"">';
                                    strhtml += '<input type=""hidden"" name=""' + data[i - 1].QuestionType + '"" value=""' + data[i - 1]._id + '|' + data[i - 1].QuestionType + '""/>';
                                    strhtml += '<div class=""title""><div class=""info""><input id=""Order' + data[i - 1]._id + '"" readonly=""readonly"" type=""text"" name=""Order' + data[i - 1]._id + '""  value=""' + i + '"" maxlength=""3"" />.';
                                    strhtml += '[ <input id=""Score' + data[i - 1]._id + '"" type=""text"" name=""Score' + data[i - 1]._id + '"" readonly=""readonly"" value=""1"" onkeyup=""verifyInput(this)"" maxlength=""3"" /> ");


            
            #line 165 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                                                                                                                                                                                                                 Write(Exampaper.Fen);

            
            #line default
            #line hidden
WriteLiteral(" ]</div>\';\r\n                                    strhtml += \'<h5>\' + data[i - 1].Q" +
"uestionContent + \'<div class=\"do\"><a href=\"#");


WriteLiteral("@\" onclick=\"Delete(\' + data[i - 1]._id + \')\" title=\"");


            
            #line 166 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                                                                                                                                                                 Write(CommonLanguage.Common_Delete);

            
            #line default
            #line hidden
WriteLiteral(@""" class=""icon idel""></a></div></h5></div>';
                                    strhtml += '<div class=""content"">';
                                    switch (data[i - 1].QuestionType) {
                                    case 1:
                                        strhtml += '<textarea class=""Boxarea"" disabled=""disabled"" ></textarea>';
                                        break;
                                    case 2:
                                        for (var j = 0; j < data[i - 1].QuestionAnswer.length; j++) {
                                            strhtml += '<div><input type=""radio""  disabled=""disabled"" />' + String.fromCharCode((65 + j)) + '.' + data[i - 1].QuestionAnswer[j].Answer + '</div>';
                                        }
                                        break;
                                    case 3:
                                        for (var k = 0; k < data[i - 1].QuestionAnswer.length; k++) {
                                            strhtml += '<div><input type=""checkbox"" disabled=""disabled"" />' + String.fromCharCode((65 + k)) + '.' + data[i - 1].QuestionAnswer[k].Answer + '</div>';
                                        }
                                        break;
                                    case 4:
                                        strhtml += '<div><input type=""radio"" disabled=""disabled"" /><label>");


            
            #line 183 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                                                                                                     Write(Exampaper.eTrue);

            
            #line default
            #line hidden
WriteLiteral("</label></div>\';\r\n                                        strhtml += \'<div><input" +
" type=\"radio\" disabled=\"disabled\" /><label>");


            
            #line 184 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                                                                                                     Write(Exampaper.eFalse);

            
            #line default
            #line hidden
WriteLiteral("</label></div>\';\r\n                                        break;\r\n               " +
"                     case 6:\r\n                                        if (data[i" +
" - 1].QuestionAnswer[0].AnswerType == 0) {\r\n                                    " +
"        strhtml += \'<textarea class=\"Boxarea\" disabled=\"disabled\" ></textarea>\';" +
"\r\n                                        } else if (data[i - 1].QuestionAnswer[" +
"0].AnswerType == 1) {\r\n                                            for (var q = " +
"0; q < data[i - 1].QuestionAnswer.length; q++) {\r\n                              " +
"                  strhtml += \'<div><input type=\"radio\" disabled=\"disabled\" />\' +" +
" String.fromCharCode((65 + q)) + \'.\' + data[i - 1].QuestionAnswer[q].Answer + \'<" +
"/div>\';\r\n                                            }\r\n                        " +
"                } else {\r\n                                            for (var y" +
" = 0; y < data[i - 1].QuestionAnswer.length; y++) {\r\n                           " +
"                     strhtml += \'<div><input type=\"checkbox\" disabled=\"disabled\"" +
" />\' + String.fromCharCode((65 + y)) + \'.\' + data[i - 1].QuestionAnswer[y].Answe" +
"r + \'</div>\';\r\n                                            }\r\n                  " +
"                      }\r\n                                        break;\r\n       " +
"                             }\r\n                                    strhtml += \'" +
"</div></div>\';\r\n                                }\r\n                            }" +
"\r\n                            $(\"#divExamQuestions\").append(strhtml);\r\n         " +
"                   order();\r\n                            StatType();\r\n          " +
"                  Score();\r\n                            statID();\r\n             " +
"               qtype();\r\n                            upScore();\r\n               " +
"             art.dialog.list[\'QueList\'].close();\r\n                        },\r\n  " +
"                      error: function(XMLHttpRequest, textStatus, errorThrown) {" +
"\r\n                            art.dialog.tips(errorThrown, 1.5);\r\n              " +
"              //art.dialog({ title: \'");


            
            #line 214 "..\..\Views\Exampaper\ExamQuestionList.cshtml"
                                              Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\', content: errorThrown, width: 200, height: 50, fixed: true, lock: true, time: 1" +
"000 });\r\n                        }\r\n                    });\r\n                }\r\n" +
"            }\r\n        });\r\n    });\r\n\r\n    //初始化题库\r\n\r\n    function initQuestionS" +
"ort() {\r\n        $.getJSON(\"/Question/GetAllQuestionSortTree?t=\" + new Date(), f" +
"unction(data) {\r\n            $(\"#sortList\").html(data);\r\n//            $(\"#navig" +
"ation\").css({ \"width\": \"600px\" }).addClass(\"ovy\");\r\n            //树的显示\r\n        " +
"    $(\"#navigation\").treeview({\r\n                persist: \"location\",\r\n         " +
"       collapsed: false,\r\n                unique: false\r\n            });\r\n      " +
"      threeBackground(\'0\');\r\n            $(\"#navigation>li>ul>li>div\").each(func" +
"tion () {\r\n                $(this).click();\r\n            });\r\n        });\r\n    }" +
"\r\n\r\n//显示所有试题\r\n\r\n    function InitializeTargetList(url) {\r\n        $(\"#questionLi" +
"st\").JsRenderData({\r\n            sourceUrl: url,\r\n            isPage: true,\r\n   " +
"         pageSize: 10,\r\n            pageIndex: 1,\r\n            templateID: \'Ques" +
"tionKeyTemplate\',\r\n            funCallback: function() {\r\n                SetChe" +
"ckBox(\"selectQueall\", \"questionList\");\r\n                HiddenSelect();\r\n       " +
"     }\r\n        });\r\n\r\n\r\n    }\r\n\r\n    function HiddenSelect() {\r\n        if ($(\"" +
"#QuestionIDs\") != undefined && $(\"#QuestionIDs\") != null && $(\"#QuestionIDs\") !=" +
" \"\") {\r\n            $(\"#questionList td input[type=\'checkbox\']\").each(function()" +
" {\r\n                if ((\',\' + $(\"#QuestionIDs\").val() + \',\').indexOf(\',\' + $(th" +
"is).attr(\'value\') + \',\') >= 0) {\r\n                    $(this).attr(\'disabled\', \'" +
"disabled\');\r\n                }\r\n            });\r\n        }\r\n    }\r\n\r\n//问题分类树点击事件" +
"\r\n\r\n    function selectSort(id, obj) {\r\n        $(\"#selQuestionSort\").val(id);\r\n" +
"        $(\"#selQuestionType\").val(\"0\");\r\n        $(\"#selQuestionLevel\").val(\"0\")" +
";\r\n        $(\"#txtQuestionContent\").val(\"\");\r\n        InitializeTargetList(getUr" +
"lParms());\r\n\r\n    }\r\n\r\n    //生成url参数字符串\r\n\r\n    function getUrlParms() {\r\n       " +
" return \'/Question/GetAllQuestion?sortID=\' + $(\"#selQuestionSort\").val() +\r\n    " +
"        \'&type=\' + $(\"#selQuestionType\").val() +\r\n            \'&level=\' + $(\"#se" +
"lQuestionLevel\").val() +\r\n            \'&content=\' + escape($(\"#txtQuestionConten" +
"t\").val()) + \'&t=\' + new Date();\r\n    }\r\n</script>");


        }
    }
}
#pragma warning restore 1591