﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.225
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
    
    #line 1 "..\..\Views\Question\QuestionJudgeEdit.cshtml"
    using LiXinLanguage;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Views\Question\QuestionJudgeEdit.cshtml"
    using LiXinModels.Examination.DBModel;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.3.2.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Question/QuestionJudgeEdit.cshtml")]
    public class QuestionJudgeEdit : System.Web.Mvc.WebViewPage<dynamic>
    {
        public QuestionJudgeEdit()
        {
        }
        public override void Execute()
        {



            
            #line 3 "..\..\Views\Question\QuestionJudgeEdit.cshtml"
  
    Layout = null;
    Response.Expires = 0;
    string id = Request.QueryString["id"];
    var questonInfor = ViewData["QuestionInfor"] as List<QuestionAnswer>;
    var baseInfor = ViewData["BaseInfor"] as tbQuestion;


            
            #line default
            #line hidden


WriteLiteral("\r\n<table class=\"tab-Form\">\r\n    <tr>\r\n        <td class=\"Tit all15\">\r\n           " +
" <span title=\"必填项\" class=\"must\">*</span>");


            
            #line 14 "..\..\Views\Question\QuestionJudgeEdit.cshtml"
                                              Write(Question.Question_QuestionManage_QuestionContent);

            
            #line default
            #line hidden
WriteLiteral("：\r\n        </td>\r\n        <td>\r\n            <textarea type=\"text\" id=\"txtQuestion" +
"Content\"  class=\"all80\" name=\"txtQuestionContent\" isType=\"text\" isnull=\"0\" messa" +
"ge=\"");


            
            #line 17 "..\..\Views\Question\QuestionJudgeEdit.cshtml"
                                                                                                                                Write(Question.Question_QuestionManage_QueationContentNotNull);

            
            #line default
            #line hidden
WriteLiteral("\">");


            
            #line 17 "..\..\Views\Question\QuestionJudgeEdit.cshtml"
                                                                                                                                                                                           Write(baseInfor.QuestionContent);

            
            #line default
            #line hidden
WriteLiteral("</textarea>\r\n        </td>\r\n    </tr>\r\n    <tr>\r\n        <td class=\"Tit\">\r\n      " +
"      <span title=\"必填项\" class=\"must\">*</span>");


            
            #line 22 "..\..\Views\Question\QuestionJudgeEdit.cshtml"
                                              Write(Question.Question_QuestionManage_RightAnswer);

            
            #line default
            #line hidden
WriteLiteral("：\r\n        </td>\r\n        <td>\r\n            <div class=\"sel\">\r\n                <i" +
"nput type=\"hidden\" id=\"txtQuestionAnswer\" value=\"");


            
            #line 26 "..\..\Views\Question\QuestionJudgeEdit.cshtml"
                                                               Write(questonInfor.Count > 0 ? (questonInfor[0].Answer ?? "0") : "0");

            
            #line default
            #line hidden
WriteLiteral("\" name=\"txtQuestionAnswer\" />\r\n                <input type=\"radio\" id=\"rightAnswe" +
"r\" value=\"0\" name=\"judgeAnswer\" ");


            
            #line 27 "..\..\Views\Question\QuestionJudgeEdit.cshtml"
                                                                              Write(id == "0" ? "checked=checked" : (questonInfor[0].Answer=="0" ? "checked=checked" : ""));

            
            #line default
            #line hidden
WriteLiteral(" />\r\n                <label>");


            
            #line 28 "..\..\Views\Question\QuestionJudgeEdit.cshtml"
                  Write(Question.Question_QuestionManage_Right);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                <input type=\"radio\" id=\"wrongAnswer\" value=\"1\" name=\"ju" +
"dgeAnswer\" ");


            
            #line 29 "..\..\Views\Question\QuestionJudgeEdit.cshtml"
                                                                              Write(id == "0" ? "" : (questonInfor[0].Answer == "1" ? "checked=checked" : ""));

            
            #line default
            #line hidden
WriteLiteral(" />\r\n                <label>");


            
            #line 30 "..\..\Views\Question\QuestionJudgeEdit.cshtml"
                  Write(Question.Question_QuestionManage_Wrong);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n            </div>\r\n        </td>\r\n    </tr>\r\n    <tr>\r\n        <td cla" +
"ss=\"Tit\">试题解析：</td>\r\n        <td>\r\n            <textarea id=\"txtQuestionAnalysis" +
"\" class=\"all80\" name=\"txtQuestionAnalysis\"\r\n                message=\"试题解析不可以为空\">" +
"");


            
            #line 38 "..\..\Views\Question\QuestionJudgeEdit.cshtml"
                                Write(id == "0" ? "" : baseInfor.QuestionAnalysis);

            
            #line default
            #line hidden
WriteLiteral(@"</textarea>
        </td>
    </tr>
</table>
<script type=""text/javascript"">
    //判断信息是否完整
    function isCheck()
    {
        return true;
    }

    $(document).ready(function ()
    {
        $(""#submitQuestionForm"").PreCheckForm();
        $(""#rightAnswer,#wrongAnswer"").bind(""change"", function ()
        {
            $(""#txtQuestionAnswer"").val($(this).attr(""value""));
        });
    });
</script>
");


        }
    }
}
#pragma warning restore 1591