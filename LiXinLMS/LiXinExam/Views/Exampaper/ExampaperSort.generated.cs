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
    
    #line 1 "..\..\Views\Exampaper\ExampaperSort.cshtml"
    using LiXinLanguage;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.3.2.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Exampaper/ExampaperSort.cshtml")]
    public class ExampaperSort : System.Web.Mvc.WebViewPage<dynamic>
    {
        public ExampaperSort()
        {
        }
        public override void Execute()
        {


            
            #line 2 "..\..\Views\Exampaper\ExampaperSort.cshtml"
  
    Layout = null;


            
            #line default
            #line hidden
WriteLiteral("<script src=\"");


            
            #line 5 "..\..\Views\Exampaper\ExampaperSort.cshtml"
        Write(Url.Content("~/Scripts/jquery.contextmenu.r2.js"));

            
            #line default
            #line hidden
WriteLiteral(@""" type=""text/javascript""> </script>
<div class=""treeview-box span30"">
    <div id=""sortList"" class=""tree-list""></div>
</div>
<input type=""hidden"" id=""ExampaperSortid"" />
<input type=""hidden"" id=""ExampaperSortname"" />
<script type=""text/javascript"">
    function initExampaperSort() {
        $.getJSON(""/Exampaper/GetAllExampaperSort?t="" + new Date(), function(data) {
            $(""#sortList"").html(data);
//            $(""#navigation"").css({ ""width"": ""600px"" }).addClass(""ovy"");
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

//问题分类树点击事件

    function selectSort(id, obj) {
        $(""#ExampaperSortid"").val(id);
        $(""#ExampaperSortname"").val($(obj).text());
        if ($(""#ExampaperSortid"").val() != null && $(""#ExampaperSortid"").val() != """" && $(""#ExampaperSortid"").val() != ""0"") {
            $(""#Sortid"").val($(""#ExampaperSortid"").val());
            $(""#fatherid"").val($(""#ExampaperSortname"").val());
            art.dialog.list['ExamSort'].close();
        } else {
            art.dialog.tips('");


            
            #line 38 "..\..\Views\Exampaper\ExampaperSort.cshtml"
                        Write(Exampaper.Prompt_One);

            
            #line default
            #line hidden
WriteLiteral("\', 1.5);\r\n            //art.dialog({ title: \'");


            
            #line 39 "..\..\Views\Exampaper\ExampaperSort.cshtml"
                              Write(CommonLanguage.Common_Tip);

            
            #line default
            #line hidden
WriteLiteral("\', content: \'");


            
            #line 39 "..\..\Views\Exampaper\ExampaperSort.cshtml"
                                                                     Write(Exampaper.Prompt_One);

            
            #line default
            #line hidden
WriteLiteral("\', width: 200, height: 50, fixed: true, lock: true, time: 3 });\r\n        }\r\n    }" +
"\r\n\r\n    $(document).ready(function() {\r\n        //初始化试卷库\r\n        initExampaperS" +
"ort();\r\n    });\r\n</script>");


        }
    }
}
#pragma warning restore 1591