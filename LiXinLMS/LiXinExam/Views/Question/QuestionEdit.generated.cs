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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.3.2.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Question/QuestionEdit.cshtml")]
    public class QuestionEdit : System.Web.Mvc.WebViewPage<dynamic>
    {
        public QuestionEdit()
        {
        }
        public override void Execute()
        {




            
            #line 4 "..\..\Views\Question\QuestionEdit.cshtml"
  
    string id = Request.QueryString["id"] ?? "0";
    var baseInfor = ViewData["BaseInfor"] as tbQuestion;
    string sortID = Request.QueryString["sortID"] ?? baseInfor.QuestionSortID.ToString();
    string backSortID = Request.QueryString["sortID"] ?? "0";
    string timestr = DateTime.Now.ToString("yyyyMMddHHmmss");
    ViewBag.Title = NavigateMenuLanguage.QuestionEdit;


            
            #line default
            #line hidden
WriteLiteral("<link href=\"");


            
            #line 12 "..\..\Views\Question\QuestionEdit.cshtml"
       Write(Url.Content("~/Scripts/ueditor/themes/default/ueditor.css"));

            
            #line default
            #line hidden
WriteLiteral("\" rel=\"stylesheet\" type=\"text/css\" />\r\n<div class=\"main-c\">\r\n    ");


            
            #line 14 "..\..\Views\Question\QuestionEdit.cshtml"
Write(Html.Action("SiteMapLink", "Common", new
    {
        linkName = "QuestionEdit"
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


            
            #line 27 "..\..\Views\Question\QuestionEdit.cshtml"
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


            
            #line 39 "..\..\Views\Question\QuestionEdit.cshtml"
                                                      Write(Question.Question_QuestionManage_Sort);

            
            #line default
            #line hidden
WriteLiteral("：\r\n                </td>\r\n                <td>\r\n                    <input id=\"se" +
"lQuestionSort\" name=\"selQuestionSort\" type=\"hidden\" value=\"");


            
            #line 42 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                                       Write(sortID);

            
            #line default
            #line hidden
WriteLiteral("\"/>\r\n                    <input id=\"sortName\" type=\"text\" readonly=\"readonly\" dis" +
"abled=\"disabled\" value=\"");


            
            #line 43 "..\..\Views\Question\QuestionEdit.cshtml"
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


            
            #line 50 "..\..\Views\Question\QuestionEdit.cshtml"
                                                      Write(Question.Question_QuestionManage_Type);

            
            #line default
            #line hidden
WriteLiteral("：\r\n                </td>\r\n                <td>\r\n                    <select id=\"s" +
"elQuestionType\">\r\n                        <option value=\"1\">");


            
            #line 54 "..\..\Views\Question\QuestionEdit.cshtml"
                                     Write(Question.Question_QuestionManage_Subject);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"2\">");


            
            #line 55 "..\..\Views\Question\QuestionEdit.cshtml"
                                     Write(Question.Question_QuestionManage_Single);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"3\">");


            
            #line 56 "..\..\Views\Question\QuestionEdit.cshtml"
                                     Write(Question.Question_QuestionManage_Mulitple);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"5\">");


            
            #line 57 "..\..\Views\Question\QuestionEdit.cshtml"
                                     Write(Question.Question_QuestionManage_FillBlank);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"4\">");


            
            #line 58 "..\..\Views\Question\QuestionEdit.cshtml"
                                     Write(Question.Question_QuestionManage_Judge);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"6\">");


            
            #line 59 "..\..\Views\Question\QuestionEdit.cshtml"
                                     Write(Question.Question_QuestionManage_Mediea);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                    </select>\r\n                </td>\r\n            </tr" +
">\r\n            <tr>\r\n                <td class=\"Tit\">\r\n                    <span" +
" title=\"必填项\" class=\"must\">*</span>");


            
            #line 65 "..\..\Views\Question\QuestionEdit.cshtml"
                                                      Write(Question.Question_QuestionManage_Level);

            
            #line default
            #line hidden
WriteLiteral("：\r\n                </td>\r\n                <td>\r\n                    <select id=\"s" +
"elQuestionLevel\">\r\n                        <option value=\"1\">");


            
            #line 69 "..\..\Views\Question\QuestionEdit.cshtml"
                                     Write(Question.Question_QuestionManage_EasyLevel);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"2\" selected=\"selected\">");


            
            #line 70 "..\..\Views\Question\QuestionEdit.cshtml"
                                                         Write(Question.Question_QuestionManage_CommonLevel);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n                        <option value=\"3\">");


            
            #line 71 "..\..\Views\Question\QuestionEdit.cshtml"
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


            
            #line 79 "..\..\Views\Question\QuestionEdit.cshtml"
                                   Write(CommonLanguage.Common_Save);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n            <a class=\"btn\" id=\"btnSaveAndAdd\">");


            
            #line 80 "..\..\Views\Question\QuestionEdit.cshtml"
                                         Write(CommonLanguage.Common_SaveAndAdd);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n            <a id=\"btnBack\" class=\"btn btn-cancel\">");


            
            #line 81 "..\..\Views\Question\QuestionEdit.cshtml"
                                              Write(CommonLanguage.Common_Back);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n        </center>\r\n        </form>\r\n    </div>\r\n    <div style=\"display: no" +
"ne;\" id=\"tempContent\"></div>\r\n</div>\r\n<script src=\"");


            
            #line 87 "..\..\Views\Question\QuestionEdit.cshtml"
        Write(Url.Content("~/Scripts/ueditor/editor_all.js"));

            
            #line default
            #line hidden
WriteLiteral("\" type=\"text/javascript\"> </script>\r\n<script src=\"");


            
            #line 88 "..\..\Views\Question\QuestionEdit.cshtml"
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


            
            #line 96 "..\..\Views\Question\QuestionEdit.cshtml"
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


            
            #line 116 "..\..\Views\Question\QuestionEdit.cshtml"
                   Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\',\r\n                content: \'");


            
            #line 117 "..\..\Views\Question\QuestionEdit.cshtml"
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


            
            #line 133 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                  Write(id);

            
            #line default
            #line hidden
WriteLiteral("&t=");


            
            #line 133 "..\..\Views\Question\QuestionEdit.cshtml"
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


            
            #line 145 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                 Write(id);

            
            #line default
            #line hidden
WriteLiteral("&t=");


            
            #line 145 "..\..\Views\Question\QuestionEdit.cshtml"
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


            
            #line 157 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                   Write(id);

            
            #line default
            #line hidden
WriteLiteral("&t=");


            
            #line 157 "..\..\Views\Question\QuestionEdit.cshtml"
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


            
            #line 169 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                    Write(id);

            
            #line default
            #line hidden
WriteLiteral("&t=");


            
            #line 169 "..\..\Views\Question\QuestionEdit.cshtml"
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


            
            #line 181 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                Write(id);

            
            #line default
            #line hidden
WriteLiteral("&t=");


            
            #line 181 "..\..\Views\Question\QuestionEdit.cshtml"
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


            
            #line 193 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                     Write(id);

            
            #line default
            #line hidden
WriteLiteral("&t=");


            
            #line 193 "..\..\Views\Question\QuestionEdit.cshtml"
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
        $.getJSON(""/KnowledgeKey/GetKnowledgeKey?t="" + new Date(), function(data) {
            for (var i = 0; i < data.length; i++) {
                $(""#keylist"").append('<option value=""' + data[i].id + '"">' + data[i].name + '</option>');
            }
            if (");


            
            #line 222 "..\..\Views\Question\QuestionEdit.cshtml"
            Write(id);

            
            #line default
            #line hidden
WriteLiteral(" > 0) {\r\n                $(\"#keylist\").val(\'");


            
            #line 223 "..\..\Views\Question\QuestionEdit.cshtml"
                               Write(baseInfor.QuestionKey);

            
            #line default
            #line hidden
WriteLiteral("\');\r\n                $(\"#hiddenSelQuestionKey\").val(\'");


            
            #line 224 "..\..\Views\Question\QuestionEdit.cshtml"
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


            
            #line 238 "..\..\Views\Question\QuestionEdit.cshtml"
        Write(id);

            
            #line default
            #line hidden
WriteLiteral(" > 0) {\r\n            $(\"#selQuestionSort\").val(\'");


            
            #line 239 "..\..\Views\Question\QuestionEdit.cshtml"
                                   Write(baseInfor.QuestionSortID);

            
            #line default
            #line hidden
WriteLiteral("\');\r\n            $(\"#selQuestionType\").val(\'");


            
            #line 240 "..\..\Views\Question\QuestionEdit.cshtml"
                                   Write(baseInfor.QuestionType);

            
            #line default
            #line hidden
WriteLiteral("\');\r\n            $(\"#selQuestionLevel\").val(\'");


            
            #line 241 "..\..\Views\Question\QuestionEdit.cshtml"
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
            window.location.href = ""/Question/QuestionList?sortID=");


            
            #line 254 "..\..\Views\Question\QuestionEdit.cshtml"
                                                              Write(backSortID);

            
            #line default
            #line hidden
WriteLiteral("\";\r\n        });\r\n\r\n        //保存、保存并新增事件\r\n        $(\"#btnSave,#btnSaveAndAdd\").bin" +
"d(\"click\", function() {\r\n            var obj = this;\r\n            var type = -1;" +
"\r\n            if ($(\"#keylist option\").length == 0) {\r\n                //art.dia" +
"log({ title: \'");


            
            #line 262 "..\..\Views\Question\QuestionEdit.cshtml"
                                  Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral(@"', content: '还没有知识点，请先添加知识点', width: 200, height: 50, fixed: true, lock: true, time: 3 });
                art.dialog.tips(""还没有知识点，请先添加知识点"", 1.5);
                return;
            }

            if ($(""#selQuestionSort"").val() == """" || $(""#selQuestionSort"").val() == ""0"") {
                //art.dialog({ title: '");


            
            #line 268 "..\..\Views\Question\QuestionEdit.cshtml"
                                  Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral(@"', content: '请选择题库', width: 200, height: 50, fixed: true, lock: true, time: 3 });
                art.dialog.tips(""请选择题库"", 1.5);
                return;
            }

            $(""#tempContent"").html(editor.getContent());
            if ($(""#tempContent"").text().trim().length == 0) {
                //art.dialog({ title: '");


            
            #line 275 "..\..\Views\Question\QuestionEdit.cshtml"
                                  Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral(@"', content: '请输入试题题干', width: 200, height: 50, fixed: true, lock: true, time: 3 });
                art.dialog.tips(""请输入试题题干"", 1.5);
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


            
            #line 287 "..\..\Views\Question\QuestionEdit.cshtml"
                                                      Write(id);

            
            #line default
            #line hidden
WriteLiteral(@"&type=' + type,
                        callback: function(data) {
                            if (data.result == 1) {
                                switch ($(obj).attr(""id"")) {
                                case ""btnSave"":
                                    window.location.href = ""/Question/QuestionList?sortID=");


            
            #line 292 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                                      Write(backSortID);

            
            #line default
            #line hidden
WriteLiteral("\";\r\n                                    break;\r\n                                c" +
"ase \"btnSaveAndAdd\":\r\n                                    window.location.href =" +
" \"/Question/QuestionEdit?id=0&sortID=");


            
            #line 295 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                                           Write(sortID);

            
            #line default
            #line hidden
WriteLiteral(@"&it="" + new Date();
                                    break;
                                }
                            } else {
                                art.dialog.tips(data.content, 1.5);
                                //art.dialog({ title: '");


            
            #line 300 "..\..\Views\Question\QuestionEdit.cshtml"
                                                  Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral(@"', content: data.content, width: 200, height: 50, fixed: true, lock: true, time: 3 });
                            }
                        }
                    }).submit();
                }
            }
        });
    });

    //检查文件类型及其大小

    function FileCheck() {
        var flag = true;
        if ($(""#selQuestionType"").val() == ""6"") {
            if ($(""input[name='fileName']"").length > 0) {
                $(""input[name='fileName']"").each(function() {
                    var fileName = $(this).val();
                    var extStart = fileName.lastIndexOf(""."");
                    var ext = fileName.substring(extStart, fileName.length).toLowerCase();
                    switch ($(""#fileType"").val()) {
                    case ""0"":
                        if (ext != "".jpg"" && ext != "".png"") {
                            flag = false;
                        }
                        break;
                    case ""1"":
                        if (ext != "".mp3"") {
                            flag = false;
                        }
                        break;
                    case ""2"":
                        if (!(ext == "".wmv""||ext == "".rmvb""||ext == "".avi""||ext == "".flv"")) {
                            flag = false;
                        }
                        break;
                    }
                });
                if (!flag)
                    art.dialog.tips('");


            
            #line 338 "..\..\Views\Question\QuestionEdit.cshtml"
                                Write(Question.Question_QuestionManage_Tip_FileFormat);

            
            #line default
            #line hidden
WriteLiteral("\', 1.5);\r\n                    //art.dialog({ title: \'");


            
            #line 339 "..\..\Views\Question\QuestionEdit.cshtml"
                                      Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\', content: \'");


            
            #line 339 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                             Write(Question.Question_QuestionManage_Tip_FileFormat);

            
            #line default
            #line hidden
WriteLiteral("\', width: 200, height: 50, fixed: true, lock: true, time: 3 });\r\n                " +
"return flag;\r\n            } else {\r\n                if ($(\"#fileCollection div\")" +
".length == 0) {\r\n                    art.dialog.tips(\'");


            
            #line 343 "..\..\Views\Question\QuestionEdit.cshtml"
                                Write(Question.Question_QuestionManage_MediaFileLimit);

            
            #line default
            #line hidden
WriteLiteral("\', 1.5);\r\n                    //art.dialog({ title: \'");


            
            #line 344 "..\..\Views\Question\QuestionEdit.cshtml"
                                      Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\', content: \'");


            
            #line 344 "..\..\Views\Question\QuestionEdit.cshtml"
                                                                             Write(Question.Question_QuestionManage_MediaFileLimit);

            
            #line default
            #line hidden
WriteLiteral("\', width: 200, height: 50, fixed: true, lock: true, time: 3 });\r\n                " +
"    return false;\r\n                } else\r\n                    return true;\r\n   " +
"         }\r\n        } else {\r\n            return flag;\r\n        }\r\n    }\r\n</scri" +
"pt>\r\n");


        }
    }
}
#pragma warning restore 1591