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
    
    #line 1 "..\..\Views\Exampaper\ExampaperRule.cshtml"
    using LiXinLanguage;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Views\Exampaper\ExampaperRule.cshtml"
    using LiXinModels.Examination.DBModel;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.3.2.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Exampaper/ExampaperRule.cshtml")]
    public class ExampaperRule : System.Web.Mvc.WebViewPage<dynamic>
    {
        public ExampaperRule()
        {
        }
        public override void Execute()
        {



            
            #line 3 "..\..\Views\Exampaper\ExampaperRule.cshtml"
  
    Layout = null;
    var model = ViewData["sort"] as List<tbQuestionSort>;


            
            #line default
            #line hidden
WriteLiteral(@"
<input type=""hidden"" id=""maxid"" name=""maxid"" value=""1""/>
<input type=""hidden"" id=""maxrandom"" name=""maxrandom"" />
<div id=""mdiv"">
    <div class=""exam-sel"">
        <input type=""hidden"" name=""random"" value=""1""/>
        <table name=""subdiv"" class=""tab-Form"">
            <tr>
                <td class=""Tit""><span class=""must"">*</span>");


            
            #line 15 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                      Write(Exampaper.qType);

            
            #line default
            #line hidden
WriteLiteral("：</td>\r\n                <td>\r\n                    <select id=\"selQuestionType1\" c" +
"lass=\"span8\" onchange=\"statusChange(1);\">\r\n                        <option value" +
"=\"1\">");


            
            #line 18 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                     Write(Exampaper.Subject);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"2\">");


            
            #line 19 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                     Write(Exampaper.Single);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"3\">");


            
            #line 20 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                     Write(Exampaper.Multi);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"4\">");


            
            #line 21 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                     Write(Exampaper.Judge);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"5\">");


            
            #line 22 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                     Write(Exampaper.Fill);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"6\">");


            
            #line 23 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                     Write(Exampaper.Scene);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                    </select>\r\n                </td>\r\n            </tr" +
">\r\n            <tr>\r\n                <td class=\"Tit\"><span class=\"must\">*</span>" +
"");


            
            #line 28 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                      Write(Exampaper.qSort);

            
            #line default
            #line hidden
WriteLiteral(@"：</td>
                <td>
                    <input id=""selQuestionSort1"" name=""selQuestionSort1"" type=""hidden"" />
                    <input id=""sortName1"" type=""text"" disabled=""disabled"" readonly=""readonly"" />
                    <input type=""button"" class=""btn btn-co"" value=""选择题库"" onclick="" selectQuestionSort(1);"" />
                </td>
            </tr>
            <tr>
                <td class=""Tit""><span class=""must"">*</span>分值：</td>
                <td>
                    <input type=""text"" id=""txtQuestionsum1"" onkeyup="" verifyInput(this); "" maxlength=""3"" class=""span4"" />
                </td>
            </tr>
            <tr>
                <td class=""Tit""><span class=""must"">*</span>");


            
            #line 42 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                      Write(Exampaper.qLevel);

            
            #line default
            #line hidden
WriteLiteral("：</td>\r\n                <td>\r\n                    <span>");


            
            #line 44 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                     Write(Question.Question_QuestionManage_EasyLevel);

            
            #line default
            #line hidden
WriteLiteral(@"：</span>
                    <input type=""text"" class=""num"" id=""easyc1"" onkeyup="" verifyInput1(this) "" value=""0"" disabled=""disabled"" />
                    /<input id=""Easy1""  type=""text"" readonly=""readonly"" value=""0"" class=""num total"" />
                    <span class=""ml10"">");


            
            #line 47 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                  Write(Question.Question_QuestionManage_CommonLevel);

            
            #line default
            #line hidden
WriteLiteral(@"：</span>
                    <input type=""text"" class=""num"" id=""commonc1"" onkeyup="" verifyInput1(this) "" value=""0"" disabled=""disabled"" />
                    /<input id=""Common1""  type=""text"" readonly=""readonly"" value=""0"" class=""num total"" />
                    <span class=""ml10"">");


            
            #line 50 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                  Write(Question.Question_QuestionManage_HardLevel);

            
            #line default
            #line hidden
WriteLiteral(@"：</span>
                    <input type=""text"" class=""num"" id=""hardc1"" onkeyup="" verifyInput1(this) "" value=""0"" disabled=""disabled"" />
                    /<input id=""Hard1""  type=""text"" readonly=""readonly"" value=""0"" class=""num total"" />
                </td>
            </tr>
        </table>
        <a id=""delDefault1"" name=""rmlink"" title=""");


            
            #line 56 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                            Write(CommonLanguage.Common_Delete);

            
            #line default
            #line hidden
WriteLiteral("\" style=\"display: none;\" class=\"icon idel\" title=\"删除\"></a>\r\n    </div>\r\n</div>\r\n<" +
"center class=\"mt10\">\r\n    <input type=\"button\" id=\"addDefault\" value=\"");


            
            #line 60 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                           Write(Exampaper.Add);

            
            #line default
            #line hidden
WriteLiteral("\" class=\"btn btn-co\" />\r\n    <input type=\"button\" id=\"btnRuleAdd\" class=\"btn\" val" +
"ue=\"");


            
            #line 61 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                       Write(Exampaper.Submit);

            
            #line default
            #line hidden
WriteLiteral(@""" />
</center>
<script type=""text/javascript"">
    //选择题库
    function selectQuestionSort(idi) {
        var openurl = ""/Question/GetQSortTree?divid="" + idi;
        $.getJSON(openurl + ""&t="" + new Date(), function(data) {
            art.dialog({
                title: """);


            
            #line 69 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                   Write(Question.Question_QuestionManage_SelectSort);

            
            #line default
            #line hidden
WriteLiteral(@""",
                id: ""QueSort"" + idi,
                lock: true,
                fixed: true,
                //width: 400,
                //height: 300,
                time: false,
                content: ""<div class='treeview-box span30'><div class='tree-list'>"" + data + ""</div></div>"",
                close: function() {
                    $('#popIframe').hide();
                    //$("".aui_content"").css({ ""height"": ""auto"", ""width"": ""auto"", ""overflow"": ""auto"" });
                }
            });
            $(""#navigation"").treeview({
                persist: ""location"",
                collapsed: false,
                unique: false
            });
            $(""#navigation>li>ul>li>div"").each(function () {
                $(this).click();
            });
            //$("".aui_content"").css({ ""height"": ""300px"", ""overflow"": ""auto"", ""width"": ""100%"", ""padding"": ""0px"" });
        });
    }

    function selectSort(sortID, obj, did) {
        if (parseInt(sortID) > 0) {
            $(""#selQuestionSort"" + did).val(sortID);
            $(""#sortName"" + did).val($(obj).text());
            art.dialog.list[""QueSort"" + did].close();
            statusChange(did);
        } else {
            art.dialog({
                title: '");


            
            #line 102 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                   Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\',\r\n                content: \'");


            
            #line 103 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                     Write(Question.Question_QuestionManage_Tip_SelectSort);

            
            #line default
            #line hidden
WriteLiteral("\',\r\n                width: 200,\r\n                height: 50,\r\n                fix" +
"ed: true,\r\n                lock: true,\r\n                time: 3\r\n            });" +
"\r\n        }\r\n    }\r\n\r\n    function statusChange(iq) {\r\n        $.ajax({\r\n       " +
"     type: \"post\",\r\n            url: \'/Exampaper/CheckRandomQuestion?did=\' + iq " +
"+ \'&qType=\' + $(\"#selQuestionType\" + iq).val() + \'&qSort=\' + $(\"#selQuestionSort" +
"\" + iq).val() + \'&t=\' + new Date(),\r\n            dataType: \"json\",\r\n            " +
"success: function(data) {\r\n                if (data.Easy > 0) {\r\n               " +
"     $(\"#easyc\" + data.DivId).removeAttr(\"disabled\");\r\n                    $(\"#E" +
"asy\" + data.DivId).val(data.Easy);\r\n                } else {\r\n                  " +
"  $(\"#easyc\" + data.DivId).val(\"0\");\r\n                    $(\"#easyc\" + data.DivI" +
"d).attr(\"disabled\", \"disabled\");\r\n                    $(\"#Easy\" + data.DivId).va" +
"l(0);\r\n                }\r\n                if (data.Common > 0) {\r\n              " +
"      $(\"#commonc\" + data.DivId).removeAttr(\"disabled\");\r\n                    $(" +
"\"#Common\" + data.DivId).val(data.Common);\r\n                } else {\r\n           " +
"         $(\"#commonc\" + data.DivId).val(\"0\");\r\n                    $(\"#commonc\" " +
"+ data.DivId).attr(\"disabled\", \"disabled\");\r\n                    $(\"#Common\" + d" +
"ata.DivId).val(0);\r\n                }\r\n                if (data.Hard > 0) {\r\n   " +
"                 $(\"#hardc\" + data.DivId).removeAttr(\"disabled\");\r\n             " +
"       $(\"#Hard\" + data.DivId).val(data.Hard);\r\n                } else {\r\n      " +
"              $(\"#hardc\" + data.DivId).val(\"0\");\r\n                    $(\"#hardc\"" +
" + data.DivId).attr(\"disabled\", \"disabled\");\r\n                    $(\"#Hard\" + da" +
"ta.DivId).val(0);\r\n                }\r\n            }\r\n        });\r\n    }\r\n\r\n//验证重" +
"复条件\r\n\r\n    function countInstances(mainStr, subStr) {\r\n        var count = 0;\r\n " +
"       var offset = 0;\r\n        do {\r\n            offset = mainStr.indexOf(subSt" +
"r, offset);\r\n            if (offset != -1) {\r\n                count++;\r\n        " +
"        offset += subStr.length;\r\n            }\r\n        } while (offset != -1);" +
"\r\n        return count;\r\n    }\r\n\r\n    function ckque(str) {\r\n        var tmp = $" +
"(\'#mdiv\').find(\'input[name=random]\');\r\n        var eqidss = \"\";\r\n        var fla" +
"g = true;\r\n        for (var g = 0; g < tmp.length; g++) {\r\n            if (tmp[g" +
"].value != \"\" && tmp[g].value != null) {\r\n                var QuestionType = \"#s" +
"elQuestionType\" + tmp[g].value;\r\n                var QuestionSort = \"#selQuestio" +
"nSort\" + tmp[g].value;\r\n                var question = $(QuestionType).val() + \"" +
"|\" + $(QuestionSort).val();\r\n                eqidss = eqidss + question + \",\";\r\n" +
"            }\r\n        }\r\n        if (parseInt(countInstances(eqidss, str)) > 1)" +
" {\r\n            flag = false;\r\n        }\r\n        return flag;\r\n    }\r\n\r\n    fun" +
"ction ckque1(str) {\r\n        var tmp = $(\'#divExamQuestions\').find(\'input[name=q" +
"ru]\');\r\n        var eqidss = \"\";\r\n        var flag = true;\r\n        for (var g =" +
" 0; g < tmp.length; g++) {\r\n            if (tmp[g].value != \"\" && tmp[g].value !" +
"= null) {\r\n                eqidss = eqidss + tmp[g].value + \",\";\r\n            }\r" +
"\n        }\r\n        if (parseInt(countInstances(eqidss, str)) > 0) {\r\n          " +
"  flag = false;\r\n        }\r\n        return flag;\r\n    }\r\n\r\n//FROM验证\r\n\r\n    funct" +
"ion CheckLevel(did) {\r\n        var easyc = $(\"#easyc\" + did).val();\r\n        var" +
" Easy = $(\"#Easy\" + did).val();\r\n        var commonc = $(\"#commonc\" + did).val()" +
";\r\n        var Common = $(\"#Common\" + did).val();\r\n        var hardc = $(\"#hardc" +
"\" + did).val();\r\n        var Hard = $(\"#Hard\" + did).val();\r\n        var r = /^[" +
"0-9]*[1-9][0-9]*$/;\r\n        var frombool = true;\r\n        var sosun = 0;\r\n     " +
"   var zsun = 0;\r\n        if (parseInt(Easy) > 0 || parseInt(Common) > 0 || pars" +
"eInt(Hard) > 0) {\r\n            if (parseInt(Easy) > 0) {\r\n                if (ea" +
"syc == null || easyc == \"\" || easyc == 0) {\r\n                    sosun = sosun +" +
" 1;\r\n                } else {\r\n                    if (!(r.test(easyc)) || parse" +
"Int(easyc) > parseInt(Easy)) {\r\n                        frombool = false; \r\n    " +
"                }\r\n                }\r\n                zsun = zsun+1;\r\n          " +
"  }\r\n            if (parseInt(Common) > 0) {\r\n                if (commonc == nul" +
"l || commonc == \"\" || commonc == 0) {\r\n                    sosun = sosun + 1;\r\n " +
"               } else {\r\n                    if (!(r.test(commonc)) || parseInt(" +
"commonc) > parseInt(Common)) {\r\n                        frombool = false;\r\n     " +
"                   \r\n                    }\r\n                }\r\n                z" +
"sun = zsun + 1;\r\n            }\r\n            if (parseInt(Hard) > 0) {\r\n         " +
"       if (hardc == null || hardc == \"\" || hardc == 0) {\r\n                    so" +
"sun = sosun + 1;\r\n                } else {\r\n                    if (!(r.test(har" +
"dc)) || parseInt(hardc) > parseInt(Hard)) {\r\n                        frombool = " +
"false;\r\n                    }\r\n                }\r\n                zsun = zsun + " +
"1;\r\n            }\r\n            if (parseInt(sosun) >= parseInt(zsun)) {\r\n       " +
"         frombool = false;\r\n            }\r\n        } else {\r\n            fromboo" +
"l = false;\r\n        }\r\n        return frombool;\r\n    }\r\n\r\n    function ckfrom() " +
"{\r\n        var tmp = $(\'#mdiv\').find(\'input[name=random]\');\r\n        var eroor =" +
" \"\";\r\n        for (var g = 0; g < tmp.length; g++) {\r\n            if (tmp[g].val" +
"ue != \"\" && tmp[g].value != null) {\r\n                var QuestionType = \"#selQue" +
"stionType\" + tmp[g].value;\r\n                var QuestionSort = \"#selQuestionSort" +
"\" + tmp[g].value;\r\n                var txtQuestionsum = \"#txtQuestionsum\" + tmp[" +
"g].value;\r\n\r\n                if ($(QuestionType).val() != \"\" && $(QuestionSort)." +
"val() != \"\" && $(txtQuestionsum).val() != \"\" && $(QuestionType).val() != null &&" +
" $(QuestionSort).val() != null && $(txtQuestionsum).val() != null) {\r\n          " +
"          var r = /^[0-9]*[1-9][0-9]*$/;\r\n                    var va = $(txtQues" +
"tionsum).val();\r\n                    if (!(r.test(va)) || parseInt(va) < 1 || pa" +
"rseInt(va) > 100) {\r\n                        eroor = \"分值请输入1-100的正整数\";\r\n        " +
"            } else {\r\n                        var que = $(QuestionType).val() + " +
"\"|\" + $(QuestionSort).val();\r\n                        if (!ckque(que)) {\r\n      " +
"                      eroor = \"请不要选择重复的组卷条件\";\r\n                        } else {\r" +
"\n                            if (!ckque1(que)) {\r\n                              " +
"  eroor = \"组卷规则已存在，请重新选择\";\r\n                            } else {\r\n              " +
"                  if (!CheckLevel(tmp[g].value)) {\r\n                            " +
"        eroor = \"难度填写有误\";\r\n                                }\r\n                  " +
"          }\r\n                        }\r\n                    }\r\n                }" +
" else {\r\n                    eroor = \"请填写组卷条件\";\r\n                }\r\n            " +
"}\r\n        }\r\n        return eroor;\r\n    }\r\n\r\n//添加\r\n\r\n    function insert(numid)" +
" {\r\n        var QuestionType = \"#selQuestionType\" + numid;\r\n        var Question" +
"Sort = \"#selQuestionSort\" + numid;\r\n        if ($(\"#sortName\" + numid).val() != " +
"\"\" && $(\"#sortName\" + numid).val() != null) {\r\n            if ($(\"#txtQuestionsu" +
"m\" + numid).val() != \"\") {\r\n                var que = $(QuestionType).val() + \"|" +
"\" + $(QuestionSort).val();\r\n                if (ckque(que)) {\r\n                 " +
"   if (ckque1(que)) {\r\n                        if (CheckLevel(numid)) {\r\n       " +
"                     $(\"#delDefault\" + numid).show();\r\n                         " +
"   var mid = parseInt(numid) + 1;\r\n                            var liHTML = \'<di" +
"v class=\"exam-sel\"><input type=\"hidden\" name=\"random\" value=\"\' + mid + \'\"/><tabl" +
"e name=\"subdiv\" class=\"tab-Form\">\';\r\n                            liHTML += \'<tr>" +
"\';\r\n                            liHTML += \'<td class=\"Tit\"><span class=\"must\">*<" +
"/span>");


            
            #line 300 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                                             Write(Exampaper.qType);

            
            #line default
            #line hidden
WriteLiteral("：</td><td>\';\r\n                            liHTML += \'<select id=\"selQuestionType\'" +
" + mid + \'\" class=\"span8\" onchange=\"statusChange(\' + mid + \');\">\';\r\n            " +
"                liHTML += \'<option value=\"1\">");


            
            #line 302 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                    Write(Exampaper.Subject);

            
            #line default
            #line hidden
WriteLiteral("</option>\';\r\n                            liHTML += \'<option value=\"2\">");


            
            #line 303 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                    Write(Exampaper.Single);

            
            #line default
            #line hidden
WriteLiteral("</option>\';\r\n                            liHTML += \'<option value=\"3\">");


            
            #line 304 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                    Write(Exampaper.Multi);

            
            #line default
            #line hidden
WriteLiteral("</option>\';\r\n                            liHTML += \'<option value=\"4\">");


            
            #line 305 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                    Write(Exampaper.Judge);

            
            #line default
            #line hidden
WriteLiteral("</option>\';\r\n                            liHTML += \'<option value=\"5\">");


            
            #line 306 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                    Write(Exampaper.Fill);

            
            #line default
            #line hidden
WriteLiteral("</option>\';\r\n                            liHTML += \'<option value=\"6\">");


            
            #line 307 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                    Write(Exampaper.Scene);

            
            #line default
            #line hidden
WriteLiteral("</option>\';\r\n                            liHTML += \'</select></td></tr><tr>\';\r\n  " +
"                          liHTML += \'<td class=\"Tit\"><span class=\"must\">*</span>" +
"");


            
            #line 309 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                                             Write(Exampaper.qSort);

            
            #line default
            #line hidden
WriteLiteral(@"：</td><td>';
                            liHTML += '<input id=""selQuestionSort' + mid + '"" name=""selQuestionSort' + mid + '"" type=""hidden"" />';
                            liHTML += '<input id=""sortName' + mid + '"" type=""text"" readonly=""readonly"" value="""" disabled=""disabled"" />';
                            liHTML += ' <input type=""button"" class=""btn btn-co"" value=""选择题库"" onclick=""selectQuestionSort(' + mid + ');""/>';
                            liHTML += '</td></tr><tr>';
                            liHTML += '<td class=""Tit""><span class=""must"">*</span>分值：</td><td><input type=""text"" id=""txtQuestionsum' + mid + '"" maxlength=""3"" class=""span4"" /></td>';
                            liHTML += '</tr><tr>';
                            liHTML += '<td class=""Tit""><span class=""must"">*</span>");


            
            #line 316 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                                             Write(Exampaper.qLevel);

            
            #line default
            #line hidden
WriteLiteral("：</td><td>\';\r\n                            liHTML += \'<span>");


            
            #line 317 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                        Write(Question.Question_QuestionManage_EasyLevel);

            
            #line default
            #line hidden
WriteLiteral(@"：</span>';
                            liHTML += '<input type=""text"" class=""num"" id=""easyc' + mid + '"" onkeyup=""verifyInput1(this)"" value=""0"" disabled=""disabled"" />';
                            liHTML += '/<input class=""num total"" id=""Easy' + mid + '""  type=""text"" readonly=""readonly"" value=""0"" />';
                            liHTML += '<span class=""ml10"">");


            
            #line 320 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                     Write(Question.Question_QuestionManage_CommonLevel);

            
            #line default
            #line hidden
WriteLiteral(@"：</span>';
                            liHTML += '<input type=""text"" class=""num"" id=""commonc' + mid + '"" onkeyup=""verifyInput1(this)"" value=""0"" disabled=""disabled"" />';
                            liHTML += '/<input class=""num total"" id=""Common' + mid + '""  type=""text"" readonly=""readonly"" value=""0"" />';
                            liHTML += '<span class=""ml10"">");


            
            #line 323 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                     Write(Question.Question_QuestionManage_HardLevel);

            
            #line default
            #line hidden
WriteLiteral(@"：</span>';
                            liHTML += '<input type=""text"" class=""num"" id=""hardc' + mid + '"" onkeyup=""verifyInput1(this)"" value=""0"" disabled=""disabled"" />';
                            liHTML += '/<input class=""num total"" id=""Hard' + mid + '""  type=""text"" readonly=""readonly"" value=""0"" />';
                            liHTML += '</td></tr></table>';
                            liHTML += '<a id=""delDefault' + mid + '"" name=""rmlink"" title=""");


            
            #line 327 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                                                     Write(CommonLanguage.Common_Delete);

            
            #line default
            #line hidden
WriteLiteral(@""" class=""icon idel"" title=""删除""></a>';
                            liHTML += '</div>';
                            $(""#mdiv"").append(liHTML);
                            var max = 0;
                            $('#mdiv').find('input[name=random]').each(function() {
                                var id = parseInt($(this).val());
                                if (id > max) {
                                    max = id;
                                }
                            });
                            $(""#maxid"").val(max);
                            // 为新元素节点添加事件侦听器
                            bindListener();
                            document.getElementById(""mdiv"").scrollTop = document.getElementById(""mdiv"").scrollHeight;
                        } else {
                            art.dialog.tips(""难度填写有误"", 1.5);
                            //art.dialog({ title: '");


            
            #line 343 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                              Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\', content: \"难度填写有误\", width: 400, height: 100, fixed: true, lock: true, time: 3 }" +
");\r\n                        }\r\n                    } else {\r\n                   " +
"     art.dialog.tips(\"组卷规则已存在，请重新选择\", 1.5);\r\n                        //art.dialo" +
"g({ title: \'");


            
            #line 347 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                          Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\', content: \"组卷规则已存在，请重新选择\", width: 400, height: 100, fixed: true, lock: true, ti" +
"me: 3 });\r\n                    }\r\n                } else {\r\n                    " +
"art.dialog.tips(\"请不要选择重复的组卷条件\", 1.5);\r\n                    //art.dialog({ title:" +
" \'");


            
            #line 351 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                      Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\', content: \"请不要选择重复的组卷条件\", width: 400, height: 100, fixed: true, lock: true, tim" +
"e: 3 });\r\n                }\r\n            } else {\r\n                art.dialog.ti" +
"ps(\"请输入分值\", 1.5);\r\n                //art.dialog({ title: \'");


            
            #line 355 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                  Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\', content: \"请输入分值\", width: 400, height: 100, fixed: true, lock: true, time: 3 })" +
";\r\n            }\r\n        } else {\r\n            art.dialog.tips(\"请选择题库\", 1.5);\r\n" +
"            //art.dialog({ title: \'");


            
            #line 359 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                              Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral(@"', content: ""请选择题库"", width: 400, height: 100, fixed: true, lock: true, time: 3 });
        }
    }

    //删除

    function bindListener() {
        $(""a[name=rmlink]"").unbind().click(function() {
            var tmpid = $('#mdiv').find('input[name=random]');
            if (tmpid.length > 1) {
                $(this).parent().remove();
                var max = 0;
                $('#mdiv').find('input[name=random]').each(function() {
                    var id = parseInt($(this).val());
                    if (id > max) {
                        max = id;
                    }
                });
                $(""#maxid"").val(max);
            } else {
                art.dialog.tips(""必须保留一项"", 1.5);
                //art.dialog({ title: '");


            
            #line 380 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                  Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\', content: \"必须保留一项\", width: 200, height: 100, fixed: true, lock: true, time: 3 }" +
");\r\n            }\r\n        });\r\n    }\r\n\r\n    function maxrandom() {\r\n        var" +
" tmp = $(\'#divExamQuestions\').find(\'input[name=Rule]\');\r\n        var eqids = \"\";" +
"\r\n        var i = 0;\r\n        if (tmp.length > 0) {\r\n            for (var i = 0;" +
" i < tmp.length; i++) {\r\n                if (tmp[i].value != \"\") {\r\n            " +
"        eqids += tmp[i].value + \",\";\r\n                }\r\n            }\r\n        " +
"    var myarray = eqids.split(\",\");\r\n            i = parseInt(myarray[0]);\r\n    " +
"        for (n = 0; n < myarray.length; n++) {\r\n                if (i < parseInt" +
"(myarray[n])) {\r\n                    i = parseInt(myarray[n]);\r\n                " +
"}\r\n            }\r\n        }\r\n        $(\"#maxrandom\").val(i);\r\n    }\r\n\r\n    $(doc" +
"ument).ready(function() {\r\n        bindListener();\r\n        maxrandom();\r\n      " +
"  $(\"#addDefault\").bind(\"click\", function() {\r\n            var maxid = $(\"#maxid" +
"\").val();\r\n            insert(maxid);\r\n        });\r\n\r\n        $(\"#btnRuleAdd\").b" +
"ind(\"click\", function() {\r\n            if (ckfrom() == \"\") {\r\n                va" +
"r tmp = $(\"#mdiv\").find(\"input[name=random]\");\r\n                var strhtml = \"\"" +
";\r\n                var eqids = \"\";\r\n                var random = 0;\r\n           " +
"     if ($(\"#maxrandom\").val() != null || $(\"#maxrandom\").val() != \"\") {\r\n      " +
"              random = parseInt($(\"#maxrandom\").val());\r\n                }\r\n    " +
"            for (var g = 0; g < tmp.length; g++) {\r\n                    if (tmp[" +
"g].value != \"\") {\r\n                        var rsum = random + g + 1;\r\n         " +
"               var QuestionType = \"#selQuestionType\" + tmp[g].value;\r\n          " +
"              var QuestionSort = \"#selQuestionSort\" + tmp[g].value;\r\n           " +
"             var txtQuestionsum = \"#txtQuestionsum\" + tmp[g].value;\r\n           " +
"             if ($(QuestionType).val() != \"\" && $(QuestionSort).val() != \"\" && $" +
"(txtQuestionsum).val() != \"\") {\r\n                            var qru = $(Questio" +
"nType).val() + \"|\" + $(QuestionSort).val();\r\n                            var eas" +
"y = $(\"#easyc\" + tmp[g].value).val() == \"\" ? \"0\" : $(\"#easyc\" + tmp[g].value).va" +
"l();\r\n                            var common = $(\"#commonc\" + tmp[g].value).val(" +
") == \"\" ? \"0\" : $(\"#commonc\" + tmp[g].value).val();\r\n                           " +
" var hard = $(\"#hardc\" + tmp[g].value).val() == \"\" ? \"0\" : $(\"#hardc\" + tmp[g].v" +
"alue).val();\r\n                            var eqids = $(QuestionType).val() + \"|" +
"\" + $(QuestionSort).val() + \"|\" + $(txtQuestionsum).val() + \"|\" + easy + \"|\" + c" +
"ommon + \"|\" + hard;\r\n                            strhtml += \'<div class=\"exam-ru" +
"le\"><input type=\"hidden\" name=\"Rule\" value=\"\' + rsum + \'\"/>\';\r\n                 " +
"           strhtml += \'<input type=\"hidden\" id=\"qru\' + rsum + \'\" name=\"qru\" valu" +
"e=\"\' + qru + \'\"/>\';\r\n                            strhtml += \'<input type=\"hidden" +
"\" id=\"eqid\' + rsum + \'\" name=\"eqid\" value=\"\' + eqids + \'\"/>\';\r\n                 " +
"           strhtml += \'<p class=\"tit\">");


            
            #line 438 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                  Write(Exampaper.qType);

            
            #line default
            #line hidden
WriteLiteral("：</p>\';\r\n                            strhtml += \'<p class=\"span8\">\' + $(\"#selQues" +
"tionType\" + tmp[g].value).find(\"option:selected\").text() + \'</p>\';\r\n            " +
"                strhtml += \'<p class=\"tit\">");


            
            #line 440 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                  Write(Exampaper.qSort);

            
            #line default
            #line hidden
WriteLiteral(@"：</p>';
                            strhtml += '<p class=""span40 ovh"" title=""' + $(""#sortName"" + tmp[g].value).val() + '"">' + $(""#sortName"" + tmp[g].value).val() + '</p>';
                            strhtml += '<p class=""tit"">每题分值：</p><p><span>' + $(txtQuestionsum).val() + '</span></p>';
                            strhtml += '<p class=""tit"">");


            
            #line 443 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                  Write(Exampaper.qLevel);

            
            #line default
            #line hidden
WriteLiteral(@"：</p>';
                            strhtml += '<p>易<span>' + easy + '</span><p>';
                            strhtml += '<p>中<span>' + common + '</span><p>';
                            strhtml += '<p>难<span>' + hard + '</span></p>';
                            strhtml += '<a name=""rmRule"" title=""");


            
            #line 447 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                                           Write(CommonLanguage.Common_Delete);

            
            #line default
            #line hidden
WriteLiteral(@""" class=""icon idel""></a>';
                            strhtml += '</div>';
                        }
                    }
                }
                $(""#divExamQuestions"").append(strhtml);
                bindRule();
                qtype();
                Score();
                art.dialog.list['ExamRule'].close();
            } else {
                var error = ckfrom();
                art.dialog.tips(error, 1.5);
                //art.dialog({ title: '");


            
            #line 460 "..\..\Views\Exampaper\ExampaperRule.cshtml"
                                  Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\', content: error, width: 400, height: 100, fixed: true, lock: true, time: 3 });\r" +
"\n            }\r\n        });\r\n\r\n    });\r\n</script>");


        }
    }
}
#pragma warning restore 1591
