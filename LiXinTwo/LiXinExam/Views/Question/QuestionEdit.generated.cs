﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.269
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
    
    #line 1 "..\..\Views\Question\QuestionEdit.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 2 "..\..\Views\Question\QuestionEdit.cshtml"
    using LiXinLanguage;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Views\Question\QuestionEdit.cshtml"
    using LiXinModels.Examination.DBModel;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.4.1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Question/QuestionEdit.cshtml")]
    public class QuestionEdit : System.Web.Mvc.WebViewPage<dynamic>
    {
        public QuestionEdit()
        {
        }
        public override void Execute()
        {




            
            #line 4 "..\..\Views\Question\QuestionEdit.cshtml"
  
    var id = Request.QueryString["id"] ?? "0";
    var baseInfor = ViewData["BaseInfor"] as tbQuestion;
    var sortID = Request.QueryString["sortID"] ?? baseInfor.QuestionSortID.ToString();
    var backSortID = Request.QueryString["sortID"] ?? "0";
    var timestr = DateTime.Now.ToString("yyyyMMddHHmmss");
    ViewBag.Title = NavigateMenuLanguage.QuestionEdit;
    var deptId = Request.QueryString["deptId"] ?? "0";


            
            #line default
            #line hidden
WriteLiteral("<link href=\"");


            
            #line 13 "..\..\Views\Question\QuestionEdit.cshtml"
       Write(Url.Content("~/Scripts/ueditor/themes/default/ueditor.css"));

            
            #line default
            #line hidden
WriteLiteral("\" rel=\"stylesheet\" type=\"text/css\" />\r\n<div class=\"main-c\">\r\n    ");


            
            #line 15 "..\..\Views\Question\QuestionEdit.cshtml"
Write(Html.Action("SiteMapLink", "Common", new
    {
        linkName = ViewBag.Flag == 0 ? "QuestionEdit" : "DepQuestionEdit"
    }));

            
            #line default
            #line hidden
WriteLiteral(@"
    <div>
        <form method=""post"" action="""" id=""submitQuestionForm"">
        <!-- 隐藏域 enctype=""multipart/form-data""-->
        <input type=""hidden"" id=""hiddenSelQuestionKey"" name=""hiddenSelQuestionKey"" value=""0"" /><!-- 选择知识点 -->
        <input type=""hidden"" id=""hiddenSelQuestionSort"" name=""hiddenSelQuestionSort"" /><!-- 选择题库 -->
        <input type=""hidden"" id=""hiddenSelQuestionType"" name=""hiddenSelQuestionType"" /><!-- 试题类型 -->
        <input type=""hidden"" id=""hiddenSelQuestionLevel"" name=""hiddenSelQuestionLevel"" /><!-- 试题难度 -->
        <input type=""hidden"" id=""hiddenQuestionAnswer"" name=""hiddenQuestionAnswer"" /><!-- 主观、单选、多选试题的选项或答案 -->
        <input type=""hidden"" id=""hiddenQuestionContent"" name=""hiddenQuestionContent"" /><!-- 题干 -->
        <h3 class=""tit-h3"">");


            
            #line 28 "..\..\Views\Question\QuestionEdit.cshtml"
                      Write(Question.Question_QuestionManage_QuestionBaseInformation);

            
            #line default
            #line hidden
WriteLiteral(@"</h3>
        <table class=""tab-Form mt10"">
            <tr>
                <td class=""Tit all15"">
                    <span title=""必填项"" class=""must"">*</span>知识点：
                </td>
                <td>
                    <select id=""keylist""></select>
                </td>
            </tr>
            <tr>
                <td class=""Tit"">
                    <span title=""必填项"" class=""must"">*</span>");


            
            #line 40 "..\..\Views\Question\QuestionEdit.cshtml"
                                                      Write(Question.Question_QuestionManage_Sort);

            
            #line default
            #line hidden
WriteLiteral("：\r\n                </td>\r\n                <td>\r\n                    <input id=\"se" +
"lQuestionSort\" name=\"selQuestionSort\" type=\"hidden\" value=\"");


            
            #line 43 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                                       Write(sortID);

            
            #line default
            #line hidden
WriteLiteral("\"/>\r\n                    <input id=\"sortName\" type=\"text\" readonly=\"readonly\" dis" +
"abled=\"disabled\" value=\"");


            
            #line 44 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                                               Write(ViewData["fatherModel"]);

            
            #line default
            #line hidden
WriteLiteral(@""" class=""Box Raster_20""/>
                    <input type=""button"" class=""btn btn-co"" value=""选择题库""
                        onclick="" selectQuestionSort(); "" />
                </td>
            </tr>
            <tr>
                <td class=""Tit"">
                    <span title=""必填项"" class=""must"">*</span>");


            
            #line 51 "..\..\Views\Question\QuestionEdit.cshtml"
                                                      Write(Question.Question_QuestionManage_Type);

            
            #line default
            #line hidden
WriteLiteral("：\r\n                </td>\r\n                <td>\r\n                    <select id=\"s" +
"elQuestionType\">\r\n                        <option value=\"1\">");


            
            #line 55 "..\..\Views\Question\QuestionEdit.cshtml"
                                     Write(Question.Question_QuestionManage_Subject);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"2\">");


            
            #line 56 "..\..\Views\Question\QuestionEdit.cshtml"
                                     Write(Question.Question_QuestionManage_Single);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"3\">");


            
            #line 57 "..\..\Views\Question\QuestionEdit.cshtml"
                                     Write(Question.Question_QuestionManage_Mulitple);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"5\">");


            
            #line 58 "..\..\Views\Question\QuestionEdit.cshtml"
                                     Write(Question.Question_QuestionManage_FillBlank);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"4\">");


            
            #line 59 "..\..\Views\Question\QuestionEdit.cshtml"
                                     Write(Question.Question_QuestionManage_Judge);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"6\">");


            
            #line 60 "..\..\Views\Question\QuestionEdit.cshtml"
                                     Write(Question.Question_QuestionManage_Mediea);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                    </select>\r\n                </td>\r\n            </tr" +
">\r\n            <tr>\r\n                <td class=\"Tit\">\r\n                    <span" +
" title=\"必填项\" class=\"must\">*</span>");


            
            #line 66 "..\..\Views\Question\QuestionEdit.cshtml"
                                                      Write(Question.Question_QuestionManage_Level);

            
            #line default
            #line hidden
WriteLiteral("：\r\n                </td>\r\n                <td>\r\n                    <select id=\"s" +
"elQuestionLevel\">\r\n                        <option value=\"1\">");


            
            #line 70 "..\..\Views\Question\QuestionEdit.cshtml"
                                     Write(Question.Question_QuestionManage_EasyLevel);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"2\" selected=\"selected\">");


            
            #line 71 "..\..\Views\Question\QuestionEdit.cshtml"
                                                         Write(Question.Question_QuestionManage_CommonLevel);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"3\">");


            
            #line 72 "..\..\Views\Question\QuestionEdit.cshtml"
                                     Write(Question.Question_QuestionManage_HardLevel);

            
            #line default
            #line hidden
WriteLiteral(@"</option>
                    </select>
                </td>
            </tr>
        </table>
        <h3 class=""tit-h3 mt20"">试题信息</h3>
        <div id=""questionInfor"" class=""mt10""></div>
        <center class=""mt10"">
            <a class=""btn"" id=""btnSave"">");


            
            #line 80 "..\..\Views\Question\QuestionEdit.cshtml"
                                   Write(CommonLanguage.Common_Save);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n            <a class=\"btn\" id=\"btnSaveAndAdd\">");


            
            #line 81 "..\..\Views\Question\QuestionEdit.cshtml"
                                         Write(CommonLanguage.Common_SaveAndAdd);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n            <a id=\"btnBack\" class=\"btn btn-cancel\">");


            
            #line 82 "..\..\Views\Question\QuestionEdit.cshtml"
                                              Write(CommonLanguage.Common_Back);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n        </center>\r\n        </form>\r\n    </div>\r\n    <div style=\"display: no" +
"ne;\" id=\"tempContent\"></div>\r\n</div>\r\n<script src=\"");


            
            #line 88 "..\..\Views\Question\QuestionEdit.cshtml"
        Write(Url.Content("~/Scripts/ueditor/editor_all.js"));

            
            #line default
            #line hidden
WriteLiteral("\" type=\"text/javascript\"> </script>\r\n<script src=\"");


            
            #line 89 "..\..\Views\Question\QuestionEdit.cshtml"
        Write(Url.Content("~/Scripts/ueditor/editor_config.js"));

            
            #line default
            #line hidden
WriteLiteral(@""" type=""text/javascript""> </script>
<script type=""text/javascript"">
    var editor;

    //选择题库

    function selectQuestionSort() {
        $.getJSON(""/Question/GetAllQuestionSortTree?t="" + new Date(), function(data) {
            art.dialog({ content: ""<div class='treeview-box span30'><div class='tree-list'>"" + data + ""</div></div>"", title: """);


            
            #line 97 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                                                                         Write(Question.Question_QuestionManage_SelectSort);

            
            #line default
            #line hidden
WriteLiteral(@""", id:""SortTree""});
            //树的显示
            $(""#navigation"").treeview({
                persist: ""location"",
                collapsed: false,
                unique: false
            });
            $(""#navigation>li>ul>li>div"").each(function () {
                $(this).click();
            });
        });
    }

    function selectSort(sortID, obj) {
        if (parseInt(sortID) > 0) {
            $(""#selQuestionSort"").val(sortID);
            $(""#sortName"").val($(obj).text());
            art.dialog.list['SortTree'].close();
        } else {
            art.dialog({
                title: '");


            
            #line 117 "..\..\Views\Question\QuestionEdit.cshtml"
                   Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\',\r\n                content: \'");


            
            #line 118 "..\..\Views\Question\QuestionEdit.cshtml"
                     Write(Question.Question_QuestionManage_Tip_SelectSort);

            
            #line default
            #line hidden
WriteLiteral(@"',
                width: 200,
                height: 50,
                fixed: true,
                lock: true,
                time: 3
            });
        }
    }

    //选择哪种题型信息

    function selectQuestionType() {
        switch ($(""#selQuestionType"").val()) {
        case ""1"":
//问答题
            $(""#questionInfor"").load(""/Question/QuestionSubjectEdit?id=");


            
            #line 134 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                  Write(id);

            
            #line default
            #line hidden
WriteLiteral("&t=");


            
            #line 134 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                        Write(timestr);

            
            #line default
            #line hidden
WriteLiteral(@""", function() {
                editor = new baidu.editor.ui.Editor({
                    toolbars: [['Bold', 'ForeColor', 'BackColor', 'UnderLine', 'StrikeThrough', 'JustifyLeft', 'JustifyRight',
                        'JustifyCenter']],
                    focus: true,
                    elementPathEnabled: false
                });
                editor.render(""txtQuestionContent"");
            });
            break;
        case ""2"":
//单选题
            $(""#questionInfor"").load(""/Question/QuestionSingleEdit?id=");


            
            #line 146 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                 Write(id);

            
            #line default
            #line hidden
WriteLiteral("&t=");


            
            #line 146 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                       Write(timestr);

            
            #line default
            #line hidden
WriteLiteral(@""", function() {
                editor = new baidu.editor.ui.Editor({
                    toolbars: [['Bold', 'ForeColor', 'BackColor', 'UnderLine', 'StrikeThrough', 'JustifyLeft', 'JustifyRight',
                        'JustifyCenter']],
                    focus: true,
                    elementPathEnabled: false
                });
                editor.render(""txtQuestionContent"");
            });
            break;
        case ""3"":
//多选题
            $(""#questionInfor"").load(""/Question/QuestionMultipleEdit?id=");


            
            #line 158 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                   Write(id);

            
            #line default
            #line hidden
WriteLiteral("&t=");


            
            #line 158 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                         Write(timestr);

            
            #line default
            #line hidden
WriteLiteral(@""", function() {
                editor = new baidu.editor.ui.Editor({
                    toolbars: [['Bold', 'ForeColor', 'BackColor', 'UnderLine', 'StrikeThrough', 'JustifyLeft', 'JustifyRight',
                        'JustifyCenter']],
                    focus: true,
                    elementPathEnabled: false
                });
                editor.render(""txtQuestionContent"");
            });
            break;
        case ""5"":
//填空题
            $(""#questionInfor"").load(""/Question/QuestionFillblankEdit?id=");


            
            #line 170 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                    Write(id);

            
            #line default
            #line hidden
WriteLiteral("&t=");


            
            #line 170 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                          Write(timestr);

            
            #line default
            #line hidden
WriteLiteral(@""", function() {
                editor = new baidu.editor.ui.Editor({
                    toolbars: [['Bold', 'ForeColor', 'BackColor', 'UnderLine', 'StrikeThrough', 'JustifyLeft', 'JustifyRight',
                        'JustifyCenter']],
                    focus: true,
                    elementPathEnabled: false
                });
                editor.render(""txtQuestionContent"");
            });
            break;
        case ""4"":
//判断题
            $(""#questionInfor"").load(""/Question/QuestionJudgeEdit?id=");


            
            #line 182 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                Write(id);

            
            #line default
            #line hidden
WriteLiteral("&t=");


            
            #line 182 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                      Write(timestr);

            
            #line default
            #line hidden
WriteLiteral(@""", function() {
                editor = new baidu.editor.ui.Editor({
                    toolbars: [['Bold', 'ForeColor', 'BackColor', 'UnderLine', 'StrikeThrough', 'JustifyLeft', 'JustifyRight',
                        'JustifyCenter']],
                    focus: true,
                    elementPathEnabled: false
                });
                editor.render(""txtQuestionContent"");
            });
            break;
        case ""6"":
//情景题
            $(""#questionInfor"").load(""/Question/QuestionMultimediaEdit?id=");


            
            #line 194 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                     Write(id);

            
            #line default
            #line hidden
WriteLiteral("&t=");


            
            #line 194 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                           Write(timestr);

            
            #line default
            #line hidden
WriteLiteral(@""", function() {
                editor = new baidu.editor.ui.Editor({
                    toolbars: [['Bold', 'ForeColor', 'BackColor', 'UnderLine', 'StrikeThrough', 'JustifyLeft', 'JustifyRight',
                        'JustifyCenter']],
                    focus: true,
                    elementPathEnabled: false
                });
                editor.render(""txtQuestionContent"");
            });
            break;
        }
    }


    //提交时的隐藏域保存

    function hiddenSave() {
        $(""#hiddenSelQuestionSort"").val($(""#selQuestionSort"").val());
        $(""#hiddenSelQuestionType"").val($(""#selQuestionType"").val());
        $(""#hiddenSelQuestionLevel"").val($(""#selQuestionLevel"").val());
        $(""#hiddenQuestionScope"").val(1);
        return true;
    }

    $(document).ready(function() {
        $.getJSON(""/KnowledgeKey/GetKnowledgeKey?deptId=");


            
            #line 219 "..\..\Views\Question\QuestionEdit.cshtml"
                                                    Write(deptId);

            
            #line default
            #line hidden
WriteLiteral("&t=\" + new Date(), function(data) {\r\n            for (var i = 0; i < data.length;" +
" i++) {\r\n                $(\"#keylist\").append(\'<option value=\"\' + data[i].id + \'" +
"\">\' + data[i].name + \'</option>\');\r\n            }\r\n            if (");


            
            #line 223 "..\..\Views\Question\QuestionEdit.cshtml"
            Write(id);

            
            #line default
            #line hidden
WriteLiteral(" > 0) {\r\n                $(\"#keylist\").val(\'");


            
            #line 224 "..\..\Views\Question\QuestionEdit.cshtml"
                               Write(baseInfor.QuestionKey);

            
            #line default
            #line hidden
WriteLiteral("\');\r\n                $(\"#hiddenSelQuestionKey\").val(\'");


            
            #line 225 "..\..\Views\Question\QuestionEdit.cshtml"
                                            Write(baseInfor.QuestionKey);

            
            #line default
            #line hidden
WriteLiteral(@"');
            } else {
                $(""#hiddenSelQuestionKey"").val($(""#keylist"").val());
            }
            $(""#keylist"").bind(""change"", function() {
                $(""#hiddenSelQuestionKey"").val($(""#keylist"").val());
            });
        });

        //题型切换事件
        $(""#selQuestionType"").bind(""change"", function() {
            selectQuestionType();
        });

        if (");


            
            #line 239 "..\..\Views\Question\QuestionEdit.cshtml"
        Write(id);

            
            #line default
            #line hidden
WriteLiteral(" > 0) {\r\n            $(\"#selQuestionSort\").val(\'");


            
            #line 240 "..\..\Views\Question\QuestionEdit.cshtml"
                                   Write(baseInfor.QuestionSortID);

            
            #line default
            #line hidden
WriteLiteral("\');\r\n            $(\"#selQuestionType\").val(\'");


            
            #line 241 "..\..\Views\Question\QuestionEdit.cshtml"
                                   Write(baseInfor.QuestionType);

            
            #line default
            #line hidden
WriteLiteral("\');\r\n            $(\"#selQuestionLevel\").val(\'");


            
            #line 242 "..\..\Views\Question\QuestionEdit.cshtml"
                                    Write(baseInfor.QuestionLevel);

            
            #line default
            #line hidden
WriteLiteral(@"');

            $(""#txtQuestionKeys"").val('');
            $(""#selQuestionType"").change();
            $(""#selQuestionType"").attr(""disabled"", ""disabled"");
            $(""#btnSaveAndAdd"").hide();
        } else {
            //初始化时默认题型信息
            selectQuestionType();
        }

        //返回
        $(""#btnBack"").bind(""click"", function() {
            var redirturl = ""QuestionList"";
            if ('");


            
            #line 256 "..\..\Views\Question\QuestionEdit.cshtml"
             Write(ViewBag.Flag);

            
            #line default
            #line hidden
WriteLiteral("\' == \'1\') {\r\n                redirturl = \"DepQuestionList\";\r\n            }\r\n     " +
"       window.location.href = \"/Question/\"+redirturl+\"?sortID=");


            
            #line 259 "..\..\Views\Question\QuestionEdit.cshtml"
                                                               Write(backSortID);

            
            #line default
            #line hidden
WriteLiteral("&deptId=");


            
            #line 259 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                                    Write(deptId);

            
            #line default
            #line hidden
WriteLiteral("\";\r\n        });\r\n\r\n        //保存、保存并新增事件\r\n        $(\"#btnSave,#btnSaveAndAdd\").bin" +
"d(\"click\", function() {\r\n            var obj = this;\r\n            var type = -1;" +
"\r\n            if ($(\"#keylist option\").length == 0) {\r\n                //art.dia" +
"log({ title: \'");


            
            #line 267 "..\..\Views\Question\QuestionEdit.cshtml"
                                  Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral(@"', content: '还没有知识点，请先添加知识点', width: 200, height: 50, fixed: true, lock: true, time: 3 });
                art.dialog.tips(""还没有知识点，请先添加知识点"", 3);
                return;
            }

            if ($(""#selQuestionSort"").val() == """" || $(""#selQuestionSort"").val() == ""0"") {
                //art.dialog({ title: '");


            
            #line 273 "..\..\Views\Question\QuestionEdit.cshtml"
                                  Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral(@"', content: '请选择题库', width: 200, height: 50, fixed: true, lock: true, time: 3 });
                art.dialog.tips(""请选择题库"", 3);
                return;
            }

            $(""#tempContent"").html(editor.getContent());
            if ($(""#tempContent"").text().trim().length == 0) {
                //art.dialog({ title: '");


            
            #line 280 "..\..\Views\Question\QuestionEdit.cshtml"
                                  Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral(@"', content: '请输入试题题干', width: 200, height: 50, fixed: true, lock: true, time: 3 });
                art.dialog.tips(""请输入试题题干"", 3);
                return;
            }

            if ($(""#selQuestionType"").val() == ""6"") {
                type = $(""#fileType"").val();
            }
            if ($(""#submitQuestionForm"").CheckForm() && isCheck() && hiddenSave()) {
                if (FileCheck()) {
                    $(""#hiddenQuestionContent"").val(editor.getContent());
                    $(""#submitQuestionForm"").submitForm({
                        url: '/Question/SubmitQuestion?id=");


            
            #line 292 "..\..\Views\Question\QuestionEdit.cshtml"
                                                      Write(id);

            
            #line default
            #line hidden
WriteLiteral("&deptId=");


            
            #line 292 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                   Write(deptId);

            
            #line default
            #line hidden
WriteLiteral(@"&type=' + type,
                        callback: function(data) {
                            if (data.result == 1) {
                                switch ($(obj).attr(""id"")) {
                                case ""btnSave"":
                                    {
                                        var redirturl = ""QuestionList"";
                                        if('");


            
            #line 299 "..\..\Views\Question\QuestionEdit.cshtml"
                                        Write(ViewBag.Flag);

            
            #line default
            #line hidden
WriteLiteral("\'==\'1\') {\r\n                                            redirturl = \"DepQuestionLi" +
"st\";\r\n                                        }\r\n                               " +
"         window.location.href = \"/Question/\"+redirturl+\"?sortID=");


            
            #line 302 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                                           Write(backSortID);

            
            #line default
            #line hidden
WriteLiteral("&deptId=");


            
            #line 302 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                                                                Write(deptId);

            
            #line default
            #line hidden
WriteLiteral(@""";
                                    }
                                    break;
                                case ""btnSaveAndAdd"":
                                    {
                                        var redirturl = ""QuestionEdit"";
                                        if ('");


            
            #line 308 "..\..\Views\Question\QuestionEdit.cshtml"
                                         Write(ViewBag.Flag);

            
            #line default
            #line hidden
WriteLiteral("\' == \'1\') {\r\n                                            redirturl = \"DepQuestion" +
"Edit\";\r\n                                        }\r\n                             " +
"           window.location.href = \"/Question/\"+redirturl+\"?id=0&sortID=");


            
            #line 311 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                                                Write(sortID);

            
            #line default
            #line hidden
WriteLiteral("&deptId=");


            
            #line 311 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                                                                 Write(deptId);

            
            #line default
            #line hidden
WriteLiteral("&it=\" + new Date();\r\n                                    }\r\n                     " +
"               break;\r\n                                }\r\n                      " +
"      } else {\r\n                                art.dialog.tips(data.content, 3)" +
";\r\n                            }\r\n                        }\r\n                   " +
" }).submit();\r\n                }\r\n            }\r\n        });\r\n    });\r\n\r\n    //检" +
"查文件类型及其大小\r\n\r\n    function FileCheck() {\r\n        var flag = true;\r\n        if ($" +
"(\"#selQuestionType\").val() == \"6\") {\r\n            if ($(\"input[name=\'fileName\']\"" +
").length > 0) {\r\n                $(\"input[name=\'fileName\']\").each(function() {\r\n" +
"                    var fileName = $(this).val();\r\n                    var extSt" +
"art = fileName.lastIndexOf(\".\");\r\n                    var ext = fileName.substri" +
"ng(extStart, fileName.length).toLowerCase();\r\n                    switch ($(\"#fi" +
"leType\").val()) {\r\n                    case \"0\":\r\n                        if (ex" +
"t != \".jpg\" && ext != \".png\") {\r\n                            flag = false;\r\n    " +
"                    }\r\n                        break;\r\n                    case " +
"\"1\":\r\n                        if (ext != \".mp3\") {\r\n                            " +
"flag = false;\r\n                        }\r\n                        break;\r\n      " +
"              case \"2\":\r\n                        if (!(ext == \".wmv\"||ext == \".r" +
"mvb\"||ext == \".avi\"||ext == \".flv\")) {\r\n                            flag = false" +
";\r\n                        }\r\n                        break;\r\n                  " +
"  }\r\n                });\r\n                if (!flag)\r\n                    art.di" +
"alog.tips(\'");


            
            #line 354 "..\..\Views\Question\QuestionEdit.cshtml"
                                Write(Question.Question_QuestionManage_Tip_FileFormat);

            
            #line default
            #line hidden
WriteLiteral("\', 3);\r\n                return flag;\r\n            } else {\r\n                if ($" +
"(\"#fileCollection div\").length == 0) {\r\n                    art.dialog.tips(\'");


            
            #line 358 "..\..\Views\Question\QuestionEdit.cshtml"
                                Write(Question.Question_QuestionManage_MediaFileLimit);

            
            #line default
            #line hidden
WriteLiteral("\', 3);\r\n                    return false;\r\n                } else\r\n              " +
"      return true;\r\n            }\r\n        } else {\r\n            return flag;\r\n " +
"       }\r\n    }\r\n</script>\r\n");


        }
    }
}
#pragma warning restore 1591