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
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 1 "..\..\Views\Question\QuestionSingleEdit.cshtml"
    using LiXinLanguage;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Views\Question\QuestionSingleEdit.cshtml"
    using LiXinModels.Examination.DBModel;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.4.1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Question/QuestionSingleEdit.cshtml")]
    public class QuestionSingleEdit : System.Web.Mvc.WebViewPage<dynamic>
    {
        public QuestionSingleEdit()
        {
        }
        public override void Execute()
        {



            
            #line 3 "..\..\Views\Question\QuestionSingleEdit.cshtml"
  
    Layout = null;
    Response.Expires = 0;
    string id = Request.QueryString["id"];
    var questonInfor = ViewData["QuestionInfor"] as List<QuestionAnswer>;
    var baseInfor = ViewData["BaseInfor"] as tbQuestion;


            
            #line default
            #line hidden


WriteLiteral("\r\n<table class=\"tab-Form\">\r\n    <tr>\r\n        <td class=\"Tit all15\"><span title=\"" +
"必填项\" class=\"must\">*</span>");


            
            #line 13 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                                                Write(Question.Question_QuestionManage_QuestionContent);

            
            #line default
            #line hidden
WriteLiteral("：</td>\r\n        <td><textarea id=\"txtQuestionContent\" class=\"all80\" name=\"txtQues" +
"tionContent\" isType=\"text\" isnull=\"0\" message=\"");


            
            #line 14 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                                                                                                   Write(Question.Question_QuestionManage_QueationContentNotNull);

            
            #line default
            #line hidden
WriteLiteral("\">");


            
            #line 14 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                                                                                                                                                              Write(baseInfor.QuestionContent);

            
            #line default
            #line hidden
WriteLiteral("</textarea></td>\r\n    </tr>\r\n    <tr>\r\n        <td class=\"Tit\"><span title=\"必填项\" " +
"class=\"must\">*</span>");


            
            #line 17 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                                          Write(Question.Question_QuestionManage_ItemAnswer);

            
            #line default
            #line hidden
WriteLiteral("：</td>\r\n        <td><input type=\"button\" class=\"btn btn-co\" value=\"");


            
            #line 18 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                                      Write(Question.Question_QuestionManage_AddItemAnswer);

            
            #line default
            #line hidden
WriteLiteral("\" id=\"btnAddAnswer\" /></td>\r\n    </tr>\r\n    <tr>\r\n        <td class=\"Tit\"></td>\r\n" +
"        <td>\r\n        <table class=\"tab-List fl\" style=\"width:80%;\">\r\n          " +
"  <thead>\r\n                <tr>\r\n                    <th>");


            
            #line 26 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                   Write(Question.Question_QuestionManage_ItemContent);

            
            #line default
            #line hidden
WriteLiteral("</th>\r\n                    <th class=\"all15\">");


            
            #line 27 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                 Write(Question.Question_QuestionManage_RightAnswer);

            
            #line default
            #line hidden
WriteLiteral("</th>\r\n                    <th class=\"all10\">操作</th>\r\n                </tr>\r\n    " +
"        </thead>\r\n            <tbody id=\"singleAnswer\">\r\n");


            
            #line 32 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                  
                    if (questonInfor.Count > 0)
                    {
                        foreach (var anser in questonInfor.OrderBy(p=>p.Order))
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <tr>\r\n                                <td>\r\n         " +
"                           <div class=\"tl\"><span title=\"必填项\" class=\"must\">*</spa" +
"n><input class=\"all70\" type=\"text\" isType=\"text\" isnull=\"0\" maxlength=\"200\" mess" +
"age=\"");


            
            #line 39 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                                                                                                                                                         Write(Question.Question_QuestionManage_ItemContentNotNull);

            
            #line default
            #line hidden
WriteLiteral("\" value=\"");


            
            #line 39 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                                                                                                                                                                                                                       Write(anser.Answer);

            
            #line default
            #line hidden
WriteLiteral("\"/></div>\r\n                                </td>\r\n                               " +
" <td class=\"tc\">\r\n                                    <input style=\"margin-top:6" +
"px;\" type=\"radio\" name=\"single\" ");


            
            #line 42 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                                                                          Write(anser.AnswerFlag == 1 ? " value=1 " : " value=0 ");

            
            #line default
            #line hidden
WriteLiteral(" ");


            
            #line 42 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                                                                                                                               Write(anser.AnswerFlag == 1 ? "checked='checked'" : "");

            
            #line default
            #line hidden
WriteLiteral(" onclick=\" checkRadio(this); \" />\r\n                                </td>\r\n       " +
"                         <td class=\"tc\">\r\n                                    <a" +
" title=\"");


            
            #line 45 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                         Write(CommonLanguage.Common_Delete);

            
            #line default
            #line hidden
WriteLiteral("\" onclick=\" delAnswer(this); \" class=\"icon idel\" style=\"margin-top:6px;\"></a>\r\n  " +
"                              </td>\r\n                            </tr>\r\n");


            
            #line 48 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                        }
                    }
                    else
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <tr>\r\n                            <td>\r\n                 " +
"               <div class=\"tl\"><span title=\"必填项\" class=\"must\">*</span><input cla" +
"ss=\"all70\" type=\"text\" isType=\"text\" isnull=\"0\" maxlength=\"200\" message=\"");


            
            #line 54 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                                                                                                                                                     Write(Question.Question_QuestionManage_ItemContentNotNull);

            
            #line default
            #line hidden
WriteLiteral(@"""/></div>
                            </td>
                            <td>
                                <input type=""radio"" style=""margin-top:6px;"" name=""single"" value=""0"" onclick="" checkRadio(this); "" />
                            </td>
                            <td>
                                <a title=""");


            
            #line 60 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                     Write(CommonLanguage.Common_Delete);

            
            #line default
            #line hidden
WriteLiteral("\" onclick=\" delAnswer(this); \" class=\"icon idel\" style=\"margin-top:6px;\"></a>\r\n  " +
"                          </td>\r\n                        </tr>\r\n");


            
            #line 63 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                    }
                

            
            #line default
            #line hidden
WriteLiteral("            </tbody>\r\n        </table></td>\r\n    </tr>\r\n    <tr>\r\n        <td cla" +
"ss=\"Tit\">试题解析：</td>\r\n        <td><textarea id=\"txtQuestionAnalysis\" class=\"all80" +
"\" name=\"txtQuestionAnalysis\"  message=\"试题解析不可以为空\">");


            
            #line 70 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                                                                                         Write(id == "0" ? "" : baseInfor.QuestionAnalysis);

            
            #line default
            #line hidden
WriteLiteral(@"</textarea></td>
    </tr>
</table>
<script type=""text/javascript"">
    $(document).ready(function() {
        $(""#submitQuestionForm"").PreCheckForm();
        //添加新的选项
        $(""#btnAddAnswer"").bind(""click"", function() {
            if ($(""#singleAnswer tr"").length < 26) {
                $(""#singleAnswer"").append('<tr>' +
                    '     <td>' +
                    '         <div class=""tl""><span title=""必填项"" class=""must"">*</span><input type=""text"" class=""Box all70"" isType=""text"" isnull=""0"" maxlength=""200"" message=""");


            
            #line 81 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                                                                                                                                                       Write(Question.Question_QuestionManage_ItemContentNotNull);

            
            #line default
            #line hidden
WriteLiteral(@"""/></div>' +
                    '     </td>' +
                    '     <td>' +
                    '         <input type=""radio"" name=""single"" value=""0"" onclick=""checkRadio(this);"" style=""margin-top:6px;"" />' +
                    '     </td>' +
                    '     <td>' +
                    '         <a title=""");


            
            #line 87 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                   Write(CommonLanguage.Common_Delete);

            
            #line default
            #line hidden
WriteLiteral(@""" onclick=""delAnswer(this);"" class=""icon idel"" style=""margin-top:6px;""></a>' +
                    '     </td>' +
                    ' </tr>');
                $(""#submitQuestionForm"").PreCheckForm();
            } else {
                //art.dialog({ title: '");


            
            #line 92 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                  Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\', content: \'");


            
            #line 92 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                                                         Write(Question.Question_QuestionManage_ItemCountLimit);

            
            #line default
            #line hidden
WriteLiteral("\', width: 200, height: 50, fixed: true, lock: true, time: 3 });\r\n                " +
"art.dialog.tips(\"");


            
            #line 93 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                            Write(Question.Question_QuestionManage_ItemCountLimit);

            
            #line default
            #line hidden
WriteLiteral(@""", 3);
            }
        });
    });
    //判断信息是否完整

    function isCheck() {
        var answerStr = '';
        var answers = $(""#singleAnswer tr"");
        var flag = true;
        var rightAnswerCount = 0;
        if (answers.length > 1) {
            for (var i = 0; i < answers.length; i++) {
                var str = $(answers[i]).find(""input[type='text']"").eq(0).val().trim() + '***!!***' + $(answers[i]).find(""input[type='radio']"").eq(0).attr(""value"");
                answerStr += answerStr == """" ? str : (""!!%!%!%!!"" + str);
                if ($(answers[i]).find(""input[type='text']"").eq(0).val().trim() == """")
                    flag = false;
                if ($(answers[i]).find(""input[type='radio']"").eq(0).attr(""value"") == ""1"")
                    rightAnswerCount++;
            }
            if (flag && rightAnswerCount > 0) {
                $(""#hiddenQuestionAnswer"").val(answerStr);
                return true;
            } else {
                if (rightAnswerCount == 0)
                //art.dialog({ title: '");


            
            #line 118 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                  Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\', content: \'");


            
            #line 118 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                                                         Write(Question.Question_QuestionManage_MustOneRightItem);

            
            #line default
            #line hidden
WriteLiteral("\', width: 200, height: 50, fixed: true, lock: true, time: 3 });\r\n                " +
"art.dialog.tips(\"");


            
            #line 119 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                            Write(Question.Question_QuestionManage_MustOneRightItem);

            
            #line default
            #line hidden
WriteLiteral("\", 3);\r\n                return false;\r\n            }\r\n        } else {\r\n         " +
"   //art.dialog({ title: \'");


            
            #line 123 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                              Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\', content: \'");


            
            #line 123 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                                                                     Write(Question.Question_QuestionManage_MustOneItem);

            
            #line default
            #line hidden
WriteLiteral("\', width: 200, height: 50, fixed: true, lock: true, time: 3 });\r\n            art." +
"dialog.tips(\"");


            
            #line 124 "..\..\Views\Question\QuestionSingleEdit.cshtml"
                        Write(Question.Question_QuestionManage_MustOneItem);

            
            #line default
            #line hidden
WriteLiteral(@""", 3);
            return false;
        }
    }

    //删除选项

    function delAnswer(obj) {
        $(obj).parent().parent().remove();
    }

    //设置正确答案

    function checkRadio(obj) {
        $(""#singleAnswer input[name='single']"").attr(""value"", 0);
        $(obj).attr(""value"", 1);
    }
</script>");


        }
    }
}
#pragma warning restore 1591