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
    
    #line 1 "..\..\Views\Question\SortEdit.cshtml"
    using LiXinLanguage;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Views\Question\SortEdit.cshtml"
    using LiXinModels.Examination.DBModel;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.4.1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Question/SortEdit.cshtml")]
    public class SortEdit : System.Web.Mvc.WebViewPage<dynamic>
    {
        public SortEdit()
        {
        }
        public override void Execute()
        {



            
            #line 3 "..\..\Views\Question\SortEdit.cshtml"
  
    Layout = null;
    Response.Expires = 0;
    var model = ViewData["model"] as tbQuestionSort;
    var fsort = Request.QueryString["fatherID"] ?? "0";
    var deptId = Request.QueryString["deptId"] ?? "0";


            
            #line default
            #line hidden
WriteLiteral("\r\n<div class=\"centerbody\">\r\n    <form id=\"questionSortForm\" method=\"post\" action=" +
"\"\">\r\n        <input id=\"hidfatherID\" name=\"hidfatherID\" type=\"hidden\" value=\"");


            
            #line 13 "..\..\Views\Question\SortEdit.cshtml"
                                                                   Write(ViewData["fatherID"]);

            
            #line default
            #line hidden
WriteLiteral("\" />\r\n        <table class=\"tab-Form tab-now\">\r\n            <tr>\r\n               " +
" <td class=\"Tit\">上级题库：</td>\r\n                <td class=\"span30\">\r\n              " +
"      <input type=\"hidden\" id=\"fatherSort\" name=\"fatherSort\" value=\"");


            
            #line 18 "..\..\Views\Question\SortEdit.cshtml"
                                                                             Write(fsort);

            
            #line default
            #line hidden
WriteLiteral("\" />\r\n                    <div title=\"");


            
            #line 19 "..\..\Views\Question\SortEdit.cshtml"
                           Write(ViewData["fatherModel"]);

            
            #line default
            #line hidden
WriteLiteral("\">");


            
            #line 19 "..\..\Views\Question\SortEdit.cshtml"
                                                     Write(ViewData["fatherModel"]);

            
            #line default
            #line hidden
WriteLiteral(@"</div>
                </td>
            </tr>
            <tr>
                <td class=""Tit""><span class=""must"">*</span>题库名称：</td>
                <td>
                    <input type=""text"" class=""span20"" maxlength=""30"" id=""txtSortName"" name=""txtSortName"" value=""");


            
            #line 25 "..\..\Views\Question\SortEdit.cshtml"
                                                                                                            Write(model.Title);

            
            #line default
            #line hidden
WriteLiteral(@""" isType=""text"" isnull=""0"" message=""请填写名称"" />
                </td>
            </tr>
            <tr>
                <td class=""Tit"">题库描述：</td>
                <td>
                    <textarea class=""span20"" id=""txtMemo"" maxlength=""200"" onkeydown="" TextearaMaxlength(this); "" onkeyup="" TextearaMaxlength(this); "" name=""txtMemo"">");


            
            #line 31 "..\..\Views\Question\SortEdit.cshtml"
                                                                                                                                                                 Write(model.Description);

            
            #line default
            #line hidden
WriteLiteral(@"</textarea>
                </td>
            </tr>
            <tr>
                <td colspan=""2"" class=""tc"">
                    <a class=""btn"" id=""btnSaveSort"">保存</a>
                </td>
            </tr>
        </table>
    </form>
</div>
<script type=""text/javascript"">
    $(document).ready(function() {
        $(""#questionSortForm"").PreCheckForm();
        $(""#btnSaveSort"").bind(""click"", function() {
            if ($(""#questionSortForm"").CheckForm()) {
                $(""#questionSortForm"").submitForm({
                    url: '/Question/SubmitQuestionSort?id=");


            
            #line 48 "..\..\Views\Question\SortEdit.cshtml"
                                                      Write(model._id);

            
            #line default
            #line hidden
WriteLiteral("&deptID=");


            
            #line 48 "..\..\Views\Question\SortEdit.cshtml"
                                                                         Write(deptId);

            
            #line default
            #line hidden
WriteLiteral(@"',
                    callback: function(data) {
                        if (data.result == 1) {
                            art.dialog.tips(data.content, 3);
                            initQuestionSort();
                            art.dialog.list['qSortEdit'].close();
                        } else {
                            art.dialog.tips(data.content, 3);
                        }
                    }
                }).submit();
            }
        });
    });
</script>");


        }
    }
}
#pragma warning restore 1591
