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

namespace LiXinExam.Views.KnowledgeKey
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.4.1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/KnowledgeKey/EditKnowledgeKey.cshtml")]
    public class EditKnowledgeKey : System.Web.Mvc.WebViewPage<dynamic>
    {
        public EditKnowledgeKey()
        {
        }
        public override void Execute()
        {

            
            #line 1 "..\..\Views\KnowledgeKey\EditKnowledgeKey.cshtml"
  
    Layout = null;


            
            #line default
            #line hidden
WriteLiteral(@"<div class=""centerbody"">
    <form method=""post"" action="""" id=""keyForm"">
        <table class=""tab-Form"">
            <tr>
                <td class=""Tit span10""><span class=""must"">*</span>知识点名称：</td>
                <td><input type=""text"" id=""keyName"" maxlength=""30"" name=""keyName"" value=""");


            
            #line 9 "..\..\Views\KnowledgeKey\EditKnowledgeKey.cshtml"
                                                                                    Write(Html.Raw(Model.KeyName));

            
            #line default
            #line hidden
WriteLiteral("\" class=\"span20\"/></td>\r\n            </tr>\r\n            <tr>\r\n                <td" +
" class=\"Tit\">知识点描述：</td>\r\n                <td><textarea id=\"keyDescription\" name" +
"=\"keyDescription\" class=\"span30\">");


            
            #line 13 "..\..\Views\KnowledgeKey\EditKnowledgeKey.cshtml"
                                                                                  Write(Html.Raw(Model.KeyDescription));

            
            #line default
            #line hidden
WriteLiteral(@"</textarea></td>
            </tr>
            <tr>
                <td colspan=""2"" class=""tc"">
                    <a id=""btnAdd"" class=""btn"">提交</a>
                </td>
            </tr>
        </table>
    </form>
</div>
<script type=""text/javascript"">
    $(document).ready(function() {
        $(""#keyForm"").validate({
            debug: true,
            submitHandler: submitform,
            event: ""blur"",
            rules: {
                keyName: {
                    required: true,
                    remote: { type: ""post"", dataType: ""json"", url: ""/KnowledgeKey/IsExsitSortName"", data: { keyName: function() { return $.trim($(""#keyName"").val()); }, id: ");


            
            #line 32 "..\..\Views\KnowledgeKey\EditKnowledgeKey.cshtml"
                                                                                                                                                                         Write(Model._id);

            
            #line default
            #line hidden
WriteLiteral(@",deptId:$(""#selectDeptID"").val() } }
                }
            },
            messages: {
                keyName: {
                    required: ""请输入知识点名称"",
                    remote: ""已存在此知识点名称""
                }
            }
        });
        $(""#btnAdd"").bind(""click"", function() {
            $(""#keyForm"").submit();
        });
        $(""#keyDescription"").textareaCount({maxCharacterSize:200});
    });

    function submitform(form) {
        $.post(""/KnowledgeKey/SubmitKnowledgeKey?id=");


            
            #line 49 "..\..\Views\KnowledgeKey\EditKnowledgeKey.cshtml"
                                                Write(Model._id);

            
            #line default
            #line hidden
WriteLiteral("&deptId=\"+$(\"#selectDeptID\").val(), $(\"#keyForm\").formSerialize(), function(data)" +
" {\r\n            if (data.result == 1) {\r\n                InitData();\r\n          " +
"      art.dialog.list[\'EditKnow\'].close();\r\n            }\r\n        });\r\n    }\r\n<" +
"/script>");


        }
    }
}
#pragma warning restore 1591
