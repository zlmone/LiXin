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
    
    #line 1 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
    using LiXinLanguage;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
    using LiXinModels.Examination.DBModel;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
    using LiXinModels.Examination.ShowModel;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.4.1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Exampaper/ExampaperDetail.cshtml")]
    public class ExampaperDetail : System.Web.Mvc.WebViewPage<dynamic>
    {
        public ExampaperDetail()
        {
        }
        public override void Execute()
        {




            
            #line 4 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
  
    Layout = null;
    var expape = ViewData["expape"] as tbExampaper;
    var expapeQuestion = ViewData["expapeQuestion"] as List<tbQuestion>;
    var expapeRule = ViewData["expapeRule"] as List<MExamRuleShow>;
    string type = Request.QueryString["type"]; //为0隐藏返回按钮，1不隐藏
    var url = Url.Content("~/ClientBin/UploadFile/");


            
            #line default
            #line hidden


WriteLiteral("\r\n<script src=\"");


            
            #line 13 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
        Write(Url.Content("~/Scripts/ckplayer5.8/ckplayer.js"));

            
            #line default
            #line hidden
WriteLiteral("\" type=\"text/javascript\"> </script>\r\n<div style=\"width:850px;height:450px;overflo" +
"w-y: scroll;\">\r\n<h3 class=\"tit-h3\">");


            
            #line 15 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
               Write(expape.ExampaperTitle);

            
            #line default
            #line hidden
WriteLiteral("</h3>\r\n<div class=\"c80\">\r\n    ");


            
            #line 17 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
Write(Exampaper.ExampaperSortName);

            
            #line default
            #line hidden
WriteLiteral("：");


            
            #line 17 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                            Write(ViewData["fatherModel"]);

            
            #line default
            #line hidden
WriteLiteral("\r\n    <span class=\"ml10\">创建于 ");


            
            #line 18 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                       Write(expape.LastUpdateTime.ToString("yyyy-MM-dd"));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n</div>\r\n<div id=\"Questions\" class=\"exam-list mt10\">\r\n");


            
            #line 21 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
      
        if (expapeQuestion.Count > 0)
        {
            int qq = 0;
            foreach (tbQuestion Question in expapeQuestion)
            {
                        

            
            #line default
            #line hidden
WriteLiteral("                <div id=\"QuestionID");


            
            #line 28 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                               Write(Question._id);

            
            #line default
            #line hidden
WriteLiteral("\" class=\"QSingle\">\r\n                    <input type=\"hidden\" name=\"");


            
            #line 29 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                           Write(Question.QuestionType);

            
            #line default
            #line hidden
WriteLiteral("\" value=\"");


            
            #line 29 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                            Write(Question._id);

            
            #line default
            #line hidden
WriteLiteral("\"/>\r\n                    <div class=\"title\"><div class=\"info\"><span>");


            
            #line 30 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                           Write(expape.QuestionList[qq].QOrder);

            
            #line default
            #line hidden
WriteLiteral("</span>. [ <span class=\"fen\">");


            
            #line 30 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                                                                         Write(expape.QuestionList[qq].QScore);

            
            #line default
            #line hidden
WriteLiteral("</span> 分 ]</div><h5>");


            
            #line 30 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                                                                                                                               Write(Html.Raw(Question.QuestionContent));

            
            #line default
            #line hidden
WriteLiteral("</h5></div>\r\n                    <div class=\"content\">\r\n");


            
            #line 32 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                          
                switch (Question.QuestionType)
                {
                    case 1:

            
            #line default
            #line hidden
WriteLiteral("                                    <div><textarea readonly=\"readonly\"></textarea" +
"></div>\r\n");



WriteLiteral("                                    <div class=\"ans\"><div class=\"ans-ok\">\r\n      " +
"                                      <span class=\"tit\"><i></i>正确答案</span>\r\n    " +
"                                        <p>");


            
            #line 39 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                           Write(Question.QuestionAnswer.Count > 0 ? Question.QuestionAnswer[0].Answer : "");

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                    </div></div>\r\n");


            
            #line 41 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                    break;
                                case 2:
                                    {
                                        int count = 0;
                                        var an="";
                                        foreach (QuestionAnswer s in Question.QuestionAnswer)
                                        {
                                            if (s.AnswerFlag == 1){
                                                an = ((char)(65 + count)).ToString();
                                            }

            
            #line default
            #line hidden
WriteLiteral("                                            <div><input type=\"radio\" disabled=\"di" +
"sabled\" /><label>");


            
            #line 51 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                                              Write((char)(65 + count));

            
            #line default
            #line hidden
WriteLiteral(". ");


            
            #line 51 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                                                                    Write(s.Answer);

            
            #line default
            #line hidden
WriteLiteral("</label></div>\r\n");


            
            #line 52 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                            count++;
                                        }

            
            #line default
            #line hidden
WriteLiteral("                                        <div class=\"ans\"><div class=\"ans-ok\">\r\n  " +
"                                          <span class=\"tit\"><i></i>正确答案</span>\r\n" +
"                                            <p>");


            
            #line 56 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                          Write(an);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                        </div></div>\r\n");


            
            #line 58 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                    }
                                    break;
                                case 3:
                                    {
                                        int count = 0;
                                        var an = "";
                                        foreach (QuestionAnswer s in Question.QuestionAnswer)
                                        {
                                            if (s.AnswerFlag == 1)
                                            {
                                                an += an == "" ? ("<span>" + ((char)(65 + count)).ToString() + "</span>") : ("<span>" + ((char)(65 + count)).ToString() + "</span>");
                                            }

            
            #line default
            #line hidden
WriteLiteral("                                            <div><input type=\"checkbox\" disabled=" +
"\"disabled\" /><label>");


            
            #line 70 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                                                 Write((char)(65 + count));

            
            #line default
            #line hidden
WriteLiteral(". ");


            
            #line 70 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                                                                       Write(s.Answer);

            
            #line default
            #line hidden
WriteLiteral("</label></div>\r\n");


            
            #line 71 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                            count++;
                                        }

            
            #line default
            #line hidden
WriteLiteral("                                        <div class=\"ans\"><div class=\"ans-ok\">\r\n  " +
"                                          <span class=\"tit\"><i></i>正确答案</span>\r\n" +
"                                            <p>");


            
            #line 75 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                          Write(Html.Raw(an));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                        </div></div>\r\n");


            
            #line 77 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                    }
                                    break;
                                case 4:
                                    {
                                        var an = Exampaper.eTrue;
                                        if (Question.QuestionAnswer.Count > 0){
                                            an = Question.QuestionAnswer[0].Answer == "0" ? Exampaper.eTrue : Exampaper.eFalse;
                                        }
                                        

            
            #line default
            #line hidden
WriteLiteral("                                        <div class=\"jud\">\r\n                      " +
"                  <input type=\"radio\" disabled=\"disabled\"/>\r\n                   " +
"                     <label>");


            
            #line 88 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                          Write(Exampaper.eTrue);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                                        <input type=\"radio\" disabled=\"d" +
"isabled\"/>\r\n                                        <label>");


            
            #line 90 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                          Write(Exampaper.eFalse);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                                        </div>\r\n");



WriteLiteral("                                        <div class=\"ans\"><div class=\"ans-ok\">\r\n  " +
"                                          <span class=\"tit\"><i></i>正确答案</span>\r\n" +
"                                            <p>");


            
            #line 94 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                          Write(an);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                        </div></div>\r\n");


            
            #line 96 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                    }
                                    break;
                                case 5:
                                    {

            
            #line default
            #line hidden
WriteLiteral("                                        <div class=\"ans\"><div class=\"ans-ok\">\r\n  " +
"                                          <span class=\"tit\"><i></i>正确答案</span>\r\n" +
"                                            <p>\r\n");


            
            #line 103 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                  
                                        if (Question.QuestionAnswer.Count > 0)
                                        {
                                            foreach (string s in Question.QuestionAnswer[0].Answer.Split(new[] { "!!%%!!" }, StringSplitOptions.None))
                                            {

            
            #line default
            #line hidden
WriteLiteral("                                                            <span>");


            
            #line 108 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                              Write(s);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n");


            
            #line 109 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                            }
                                        }
                                                

            
            #line default
            #line hidden
WriteLiteral("                                            </p>\r\n                               " +
"         </div></div>\r\n");


            
            #line 114 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                    }
                                    break;
                                case 6:
                                    {
                                        string name = Question.FileUpload[0].FileName;
                                        switch (Question.FileUpload[0].FileType)
                                        {
                                            case 0:

            
            #line default
            #line hidden
WriteLiteral(@"                                                <center class=""all70 pos_r"">
                                                        <a id=""k-prev"" onclick="" turnToNext(this, 'left'); "" class=""pl""></a>
                                                        <span id=""imageCollection"">
                                                            <input type=""hidden"" value=""1"" />
");


            
            #line 126 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                              
                                                for (int i = 0; i < Question.FileUpload.Count; i++)
                                                {

            
            #line default
            #line hidden
WriteLiteral("                                                                    <img src=\"../" +
"../ClientBin/UploadFile/");


            
            #line 129 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                                                    Write(Question.FileUpload[i].FileName);

            
            #line default
            #line hidden
WriteLiteral("\" style=\"");


            
            #line 129 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                                                                                              Write(i == 0 ? " display:block; " : " display:none; ");

            
            #line default
            #line hidden
WriteLiteral("\" />\r\n");


            
            #line 130 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                }
                                                            

            
            #line default
            #line hidden
WriteLiteral("                                                        </span>\r\n                " +
"                                        <a id=\"k-next\" onclick=\" turnToNext(this" +
", \'right\'); \" class=\"pr\"></a>\r\n                                                <" +
"/center>\r\n");


            
            #line 135 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                break;
                                            case 1:

            
            #line default
            #line hidden
WriteLiteral("                        <embed class=\"mLeft_1\" src=\"../../Scripts/mp3player/playe" +
"r.swf?url=../../ClientBin/UploadFile/");


            
            #line 137 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                                                                 Write(name);

            
            #line default
            #line hidden
WriteLiteral("&amp;autoplay=0;autostart=0\" \r\n                                                  " +
"  type=\"application/x-shockwave-flash\" wmode=\"transparent\"  allowscriptaccess=\"a" +
"lways\" height=\"25\" width=\"400\"></embed> \r\n");


            
            #line 139 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                                                                 break;
                                            case 2:
                                                                                                                 var id = name.Substring(0, name.Length - 4);   

            
            #line default
            #line hidden
WriteLiteral("                                                <input name=\"FlvName\" value=\"");


            
            #line 142 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                        Write(name);

            
            #line default
            #line hidden
WriteLiteral("\" type=\"hidden\" />\r\n");



WriteLiteral("                                                <div id=\'");


            
            #line 143 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                    Write(id);

            
            #line default
            #line hidden
WriteLiteral("\' class=\"mLeft_1\">\r\n                                                </div> \r\n");


            
            #line 145 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                    break;
                                        }
                                        if (Question.QuestionAnswer[0].AnswerType == 0)
                                        {

            
            #line default
            #line hidden
WriteLiteral("                                            <div><textarea class=\"Boxarea all50 \"" +
" readonly=\"readonly\"></textarea></div>\r\n");



WriteLiteral("                            <div class=\"ans\"><div class=\"ans-ok\">\r\n              " +
"                              <span class=\"tit\"><i></i>正确答案</span>\r\n            " +
"                                <p>");


            
            #line 152 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                           Write(Question.QuestionAnswer.Count > 0 ? Question.QuestionAnswer[0].Answer : "");

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                    </div></div>\r\n");


            
            #line 154 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                        }
                                        else if (Question.QuestionAnswer[0].AnswerType == 1)
                                        {
                                            int count = 0;
                                            var an="";
                                            foreach (QuestionAnswer s in Question.QuestionAnswer)
                                            {
                                                if (s.AnswerFlag == 1)
                                                {
                                                    an = ((char)(65 + count)).ToString();
                                                }

            
            #line default
            #line hidden
WriteLiteral("                                                <div><input type=\"radio\" disabled" +
"=\"disabled\" /><label>");


            
            #line 165 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                                                  Write((char)(65 + count));

            
            #line default
            #line hidden
WriteLiteral(". ");


            
            #line 165 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                                                                        Write(s.Answer);

            
            #line default
            #line hidden
WriteLiteral("</label></div>\r\n");


            
            #line 166 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                count++;
                                            }

            
            #line default
            #line hidden
WriteLiteral("                                            <div class=\"ans\"><div class=\"ans-ok\">" +
"\r\n                                                <span class=\"tit\"><i></i>正确答案<" +
"/span>\r\n                                                <p>");


            
            #line 170 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                              Write(an);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                            </div></div>\r\n");


            
            #line 172 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                        }
                                        else
                                        {
                                            int count = 0;
                                            var an = "";
                                            foreach (QuestionAnswer s in Question.QuestionAnswer)
                                            {
                                                if (s.AnswerFlag == 1)
                                                {
                                                    an += an == "" ? ("<span>" + ((char)(65 + count)).ToString() + "</span>") : ("<span>" + ((char)(65 + count)).ToString() + "</span>");
                                                }

            
            #line default
            #line hidden
WriteLiteral("                                                <div><input type=\"checkbox\" disab" +
"led=\"disabled\" ");


            
            #line 183 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                                            Write(s.AnswerFlag == 1 ? "checked=checked" : "");

            
            #line default
            #line hidden
WriteLiteral("/><label>");


            
            #line 183 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                                                                                                  Write((char)(65 + count));

            
            #line default
            #line hidden
WriteLiteral(". ");


            
            #line 183 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                                                                                                                                        Write(s.Answer);

            
            #line default
            #line hidden
WriteLiteral("</label></div>\r\n");


            
            #line 184 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                count++;
                                            }

            
            #line default
            #line hidden
WriteLiteral("                                            <div class=\"ans\"><div class=\"ans-ok\">" +
"\r\n                                                <span class=\"tit\"><i></i>正确答案<" +
"/span>\r\n                                                <p>");


            
            #line 188 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                              Write(Html.Raw(an));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                            </div></div>\r\n");


            
            #line 190 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                        }
                                    }
                                    break;
                }
                        

            
            #line default
            #line hidden
WriteLiteral("                    </div>\r\n                </div>\r\n");


            
            #line 197 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                qq++;
            }
        }
        if (expapeRule != null && expapeRule.Count > 0)
        {
            foreach (MExamRuleShow Rule in expapeRule)
            {

            
            #line default
            #line hidden
WriteLiteral("                <div class=\"mt10\"><table class=\"tab-Form\">\r\n                    <" +
"tr><td class=\"Tit\">");


            
            #line 205 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                   Write(Exampaper.qType);

            
            #line default
            #line hidden
WriteLiteral(": </td>\r\n                    <td class=\"all30\">");


            
            #line 206 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                  Write(Rule.QuestionType);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                    <td class=\"Tit\">");


            
            #line 207 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                               Write(Exampaper.qSort);

            
            #line default
            #line hidden
WriteLiteral(": </td>\r\n                    <td class=\"all40\" title=\"");


            
            #line 208 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                         Write(Rule.QuestionSort);

            
            #line default
            #line hidden
WriteLiteral("\">");


            
            #line 208 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                                               Write(Rule.QuestionSort);

            
            #line default
            #line hidden
WriteLiteral("</td></tr>\r\n                    <tr><td class=\"Tit\">分值: </td>\r\n                  " +
"  <td>");


            
            #line 210 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                    Write(Rule.QuestingScore);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                    <td class=\"Tit\">");


            
            #line 211 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                               Write(Exampaper.qLevel);

            
            #line default
            #line hidden
WriteLiteral(": </td>\r\n                    <td>\r\n                    易<strong class=\"c_orange\">" +
"");


            
            #line 213 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                          Write(Rule.Leveleasy);

            
            #line default
            #line hidden
WriteLiteral("</strong>\r\n                    中<strong class=\"c_orange\">");


            
            #line 214 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                          Write(Rule.Levelcommon);

            
            #line default
            #line hidden
WriteLiteral("</strong>\r\n                    难<strong class=\"c_orange\">");


            
            #line 215 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                                          Write(Rule.Levelhard);

            
            #line default
            #line hidden
WriteLiteral("</strong></td>\r\n                    </tr>\r\n                </table></div>\r\n");


            
            #line 218 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
            }
        }
    

            
            #line default
            #line hidden
WriteLiteral(@"</div>
</div>
<script type=""text/javascript"">
    $(document).ready(function ()
    {
        $(""input[name='FlvName']"").each(function ()
        {
            ckplay($(this).val());
        });
        //ckplay();
    });
    function ckplay(name)
    {

        var url = '");


            
            #line 235 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
              Write(Url.Content("~/ClientBin/UploadFile/"));

            
            #line default
            #line hidden
WriteLiteral("\' + name;\r\n        var id = name.substring(0, name.length - 4);\r\n        var s1 =" +
" new swfupload();\r\n        s1.ckplayer_url = \'");


            
            #line 238 "..\..\Views\Exampaper\ExampaperDetail.cshtml"
                      Write(Url.Content("~/Scripts/ckplayer5.8/ckplayer.swf"));

            
            #line default
            #line hidden
WriteLiteral(@"'; //播放器文件名
        s1.ckplayer_flv = url; //视频地址,这里也可以填写一个网址或xml地址或flash文件,具体的设置请到网站了解
        s1.ckplayer_pat = '1'; //传递的参数,这里的具体参数设置方式到网站了解
        s1.ckplayer_style = 0; //传递的方式,0是普通方式，1是网址传送，2是xml传送，3是flash传送
        s1.ckplayer_default = 0; //读取文本配置，此参数具有非常强大的功能，（比如外站引用视频只需一个参数即可）说来话长，请到网站了解详情
        s1.ckplayer_xml = ''; //风格配置xml文件，如果为空的话将使用js文件配置
        s1.ckplayer_loadimg = ''; //初始图片地址
        s1.ckplayer_pauseflash = ''; //暂停时播放的广告，只支持flash和图片
        s1.ckplayer_pauseurl = ''; //暂停时播放图片时需要加一个链接
        s1.ckplayer_loadadv = ''; //视频开始前播放的广告，可以是flash,也可是视频格式
        s1.ckplayer_loadurl = ''; //视频开始前广告的链接地址，主要针对视频广告，如果是flash可以不填写
        s1.ckplayer_loadtime = 0; //视频开始前广告播放的秒数,只针对flash或图片有效
        s1.ckplayer_endstatus = 2; //视频结束后的动作，0停止播放并发送js,1是不发送js且重新循环播放,2停止播放
        s1.ckplayer_volume = 40; //视频默认音量0-100之间
        s1.ckplayer_play = 0; //视频默认播放还是暂停，0是暂停，1是播放
        s1.ckplayer_width = 300; //播放器宽度
        s1.ckplayer_height = 150; //播放器高度
        s1.ckplayer_bgcolor = '#000000'; //播放器背景颜色
        s1.ckplayer_allowFullScreen = true; //是否支持全屏，true支持，false不支持，默认支持
        s1.swfwrite(id); //div的id

    }

</script>
");


        }
    }
}
#pragma warning restore 1591
