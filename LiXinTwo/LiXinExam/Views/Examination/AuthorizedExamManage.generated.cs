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

namespace LiXinExam.Views.Examination
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
    
    #line 1 "..\..\Views\Examination\AuthorizedExamManage.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 2 "..\..\Views\Examination\AuthorizedExamManage.cshtml"
    using LiXinLanguage;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.4.1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Examination/AuthorizedExamManage.cshtml")]
    public class AuthorizedExamManage : System.Web.Mvc.WebViewPage<dynamic>
    {
        public AuthorizedExamManage()
        {
        }
        public override void Execute()
        {



            
            #line 3 "..\..\Views\Examination\AuthorizedExamManage.cshtml"
  
    ViewBag.Title = NavigateMenuLanguage.AuthorizedExamManage;


            
            #line default
            #line hidden
WriteLiteral("\r\n<div id=\"contentInfor\">\r\n    ");


            
            #line 8 "..\..\Views\Examination\AuthorizedExamManage.cshtml"
Write(Html.Action("SiteMapLink", "Common", new {linkName = "AuthorizedExamManage"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <div>\r\n        ");


            
            #line 10 "..\..\Views\Examination\AuthorizedExamManage.cshtml"
   Write(Html.Action("AuthorizedExam", "Examination", new
            {
                ViewBag.examinationId
            }
             ));

            
            #line default
            #line hidden
WriteLiteral(@"
    </div>
    <div class=""mTop_1 tc"">
        <a title=""保存"" id=""Save"" class=""Btn Btn_blue"">保存</a>
        <a class=""Btn Btn_white"" id=""Back"" title=""返回"">返回</a>
    </div>
</div>
<script type=""text/javascript"">
    $(document).ready(function() {
        $(""#Save"").bind(""click"", function() {
            ChangtbExamSendStudent(""");


            
            #line 24 "..\..\Views\Examination\AuthorizedExamManage.cshtml"
                               Write(ViewBag.examinationId);

            
            #line default
            #line hidden
WriteLiteral("\");\r\n        });\r\n\r\n        $(\"#Back\").bind(\"click\", function() {\r\n            wi" +
"ndow.location.href = \"/Examination/ExaminationList\";\r\n        });\r\n    });\r\n\r\n</" +
"script>");


        }
    }
}
#pragma warning restore 1591
