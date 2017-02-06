﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.296
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LiXinExam.Views.ExamTest
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
    
    #line 1 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
    using LiXinLanguage;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
    using LiXinModels.Examination.DBModel;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.3.2.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ExamTest/ExamTestDetail.cshtml")]
    public class ExamTestDetail : System.Web.Mvc.WebViewPage<dynamic>
    {
        public ExamTestDetail()
        {
        }
        public override void Execute()
        {



            
            #line 3 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
  
    ViewBag.Title = NavigateMenuLanguage.ExamTestDetail;
    var questionList = ViewBag.QuestionList as List<tbQuestion>;
    var examUser = ViewBag.ExanUser as tbExamSendStudent;
    var backurl = Request.QueryString["backurl"] ?? "";


            
            #line default
            #line hidden
WriteLiteral("<script src=\"");


            
            #line 9 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
        Write(Url.Content("~/Scripts/ckplayer5.8/ckplayer.js"));

            
            #line default
            #line hidden
WriteLiteral("\" type=\"text/javascript\"> </script>\r\n<div class=\"main-c\">\r\n    ");


            
            #line 11 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
Write(Html.Action("SiteMapLink", "Common", new
    {
        linkName = "ExamTestDetail"
    }));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <div class=\"TitClass_line\">\r\n");


            
            #line 16 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
          
            if (ViewBag.ExamScoreStatus == 1)
            {

            
            #line default
            #line hidden
WriteLiteral("            <span>得分: <strong class=\"c_blue f16\">");


            
            #line 19 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                             Write(ViewBag.PercentFlag == 0 ? (examUser.StudentAnswerList.Sum(p => p.GetScore) * 100 / examUser.StudentAnswerList.Sum(p => p.Score)) : (examUser.StudentAnswerList.Sum(p => p.GetScore)));

            
            #line default
            #line hidden
WriteLiteral("</strong>\r\n                分</span>\r\n");


            
            #line 21 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
            }
        

            
            #line default
            #line hidden
WriteLiteral("        <h2>");


            
            #line 23 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
        Write(ViewBag.ExamName);

            
            #line default
            #line hidden
WriteLiteral("</h2>\r\n    </div>\r\n    <div class=\"qView\">\r\n        <span class=\"ml10\">答错：");


            
            #line 26 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                          Write(examUser.StudentAnswerList.Count(p => p.GetScore == 0 && p.Answer != ""));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n        <span class=\"ml10\">未答：");


            
            #line 27 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                          Write(examUser.StudentAnswerList.Count(p => p.Answer == ""));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n        <span class=\"ml10\">正确：");


            
            #line 28 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                          Write(examUser.StudentAnswerList.Count(p => p.GetScore > 0));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n    </div>\r\n    <div class=\"exam-list mt10\">\r\n");


            
            #line 31 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
          
            foreach (ReStudentExamAnswer q in examUser.StudentAnswerList.OrderBy(p => p.Order))
            {
                string rightAnswer = "";
                tbQuestion qu = questionList.FirstOrDefault(p => p._id == q.Qid);

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"QSingle\">\r\n                <div class=\"title\">\r\n         " +
"           <div class=\"info\">\r\n                        <span>");


            
            #line 39 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                          Write(q.Order);

            
            #line default
            #line hidden
WriteLiteral("</span> . [ <span class=\"fen\">");


            
            #line 39 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                                  Write(q.Score);

            
            #line default
            #line hidden
WriteLiteral("</span>分 ]\r\n                    </div>\r\n                    <h5>\r\n               " +
"         ");


            
            #line 42 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                    Write(Html.Raw(qu.QuestionContent));

            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 43 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                          
                if (qu.QuestionType == 6)
                {
                    string name = qu.FileUpload[0].FileName;
                    switch (qu.FileUpload[0].FileType)
                    {
                        case 0:
                            {

            
            #line default
            #line hidden
WriteLiteral(@"                            <center class=""mt10"">
                                <div id=""imageCollection"">
                                    <a id=""k-prev"" class=""pl"" onclick="" turnToNext(this, 'left'); ""></a>
                                    <input type=""hidden"" value=""1"" />
");


            
            #line 55 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                      
                                for (int i = 0; i < qu.FileUpload.Count; i++)
                                {

            
            #line default
            #line hidden
WriteLiteral("                                        <img src=\"../../ClientBin/UploadFile/");


            
            #line 58 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                                         Write(qu.FileUpload[i].FileName);

            
            #line default
            #line hidden
WriteLiteral("\" style=\"");


            
            #line 58 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                                                                              Write(i == 0 ? " display:block; " : " display:none; ");

            
            #line default
            #line hidden
WriteLiteral("\" />\r\n");


            
            #line 59 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                }
                                    

            
            #line default
            #line hidden
WriteLiteral("                                    <a id=\"k-next\" class=\"pr\" onclick=\" turnToNex" +
"t(this, \'right\'); \"></a>\r\n                                </div>\r\n              " +
"              </center>\r\n");


            
            #line 64 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                            }
                            break;
                        case 1:

            
            #line default
            #line hidden
WriteLiteral("                            <div class=\"mt10\">\r\n                                <" +
"embed type=\"application/x-mplayer2\" src=\"../../ClientBin/UploadFile/");


            
            #line 68 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                                                                Write(name);

            
            #line default
            #line hidden
WriteLiteral("\" name=\"mediaplayer\" width=\"500\" height=\"250\" showcontrols=\"1\" showstatusbar=\"0\" " +
"showdisplay=\"0\" autostart=\"0\"></embed>\r\n                            </div>\r\n");


            
            #line 70 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                                                                break;
                        case 2:
                                                                                                {
                                                                                                    var id = name.Substring(0, name.Length - 4);   

            
            #line default
            #line hidden
WriteLiteral("                            <input name=\"FlvName\" value=\"");


            
            #line 74 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                    Write(name);

            
            #line default
            #line hidden
WriteLiteral("\" type=\"hidden\" />\r\n");



WriteLiteral("                            <div class=\"mt10\">\r\n                                <" +
"div id=\'");


            
            #line 76 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                    Write(id);

            
            #line default
            #line hidden
WriteLiteral("\'>\r\n                                </div>\r\n                            </div> \r\n" +
"");


            
            #line 79 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                                                                }
                                                                                                break;
                    }
                }
                        

            
            #line default
            #line hidden
WriteLiteral("                    </h5>\r\n                </div>\r\n                <div class=\"co" +
"ntent\">\r\n");


            
            #line 87 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                      
                if (qu.QuestionType == 1)
                {
                    rightAnswer = qu.QuestionAnswer[0].Answer;

            
            #line default
            #line hidden
WriteLiteral("                        <!-- 问答题 -->\r\n");



WriteLiteral("                        <div class=\"ans\">\r\n                            <div class" +
"=\"ans-stu\">\r\n                                <span class=\"tit\"><i></i>学员答案</span" +
">\r\n                                <p>");


            
            #line 95 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                               Write(q.Answer);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                            </div>\r\n                        </div>\r\n");


            
            #line 98 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                }
                else if (qu.QuestionType == 2 || qu.QuestionType == 3)
                {

            
            #line default
            #line hidden
WriteLiteral("                        <!-- 单选题 -->\r\n");


            
            #line 102 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                    foreach (QuestionAnswer an in qu.QuestionAnswer.OrderBy(p => p.Order))
                    {
                        rightAnswer += an.AnswerFlag == 1 ? (rightAnswer == "" ? ((char)(an.Order + 64)).ToString() : ("、" + ((char)(an.Order + 64)))) : "";

            
            #line default
            #line hidden
WriteLiteral("                        <div>\r\n                            <input disabled=\"disab" +
"led\" type=\"");


            
            #line 106 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                         Write(qu.QuestionType == 2 ? "radio" : "checkbox");

            
            #line default
            #line hidden
WriteLiteral("\" ");


            
            #line 106 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                                                                         Write(("," + q.Answer + ",").IndexOf("," + an.Order + ",") >= 0 ? "checked=checked" : "");

            
            #line default
            #line hidden
WriteLiteral(" name=\"answer_");


            
            #line 106 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                                                                                                                                                                            Write(qu._id);

            
            #line default
            #line hidden
WriteLiteral("\" />");


            
            #line 106 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                                                                                                                                                                                         Write((char)(an.Order + 64));

            
            #line default
            #line hidden
WriteLiteral(".\r\n                            ");


            
            #line 107 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                        Write(an.Answer);

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n");


            
            #line 108 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                    }
                }
                else if (qu.QuestionType == 4)
                {
                    rightAnswer = qu.QuestionAnswer[0].Answer == "0" ? "A、正确" : "B、错误";

            
            #line default
            #line hidden
WriteLiteral("                        <!-- 判断题 -->\r\n");



WriteLiteral("                        <div class=\"jud\">\r\n                            <span>\r\n  " +
"                              <input disabled=\"disabled\" type=\"radio\" name=\"answ" +
"er_");


            
            #line 116 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                                                 Write(qu._id);

            
            #line default
            #line hidden
WriteLiteral("\" ");


            
            #line 116 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                                                            Write(q.Answer == "0" ? "checked=checked" : "");

            
            #line default
            #line hidden
WriteLiteral(" /><label>A.\r\n                                    正确</label></span> <span>\r\n     " +
"                                   <input disabled=\"disabled\" type=\"radio\" name=" +
"\"answer_");


            
            #line 118 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                                                         Write(qu._id);

            
            #line default
            #line hidden
WriteLiteral("\" ");


            
            #line 118 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                                                                    Write(q.Answer == "1" ? "checked=checked" : "");

            
            #line default
            #line hidden
WriteLiteral(" /><label>B.\r\n                                            错误</label></span>\r\n    " +
"                    </div>\r\n");


            
            #line 121 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                }
                else if (qu.QuestionType == 5)
                {
                    rightAnswer = qu.QuestionAnswer[0].Answer.Replace("!!%%!!", " ");

            
            #line default
            #line hidden
WriteLiteral("                        <!-- 填空题 -->\r\n");



WriteLiteral("                        <div class=\"ans\">\r\n                            <div class" +
"=\"ans-stu\">\r\n                                <span class=\"tit\"><i></i>学员答案</span" +
">\r\n                                <p>");


            
            #line 129 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                               Write(q.Answer.Replace("##**##", " "));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                            </div>\r\n                        </div>\r\n");


            
            #line 132 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                }
                else if (qu.QuestionType == 6)
                {
                    int type = qu.QuestionAnswer[0].AnswerType;
                    if (type == 0)
                    {
                        rightAnswer = qu.QuestionAnswer[0].Answer;

            
            #line default
            #line hidden
WriteLiteral("                        <!-- 问答题 -->\r\n");



WriteLiteral("                        <div class=\"ans\">\r\n                            <div class" +
"=\"ans-stu\">\r\n                                <span class=\"tit\"><i></i>学员答案</span" +
">\r\n                                <p>");


            
            #line 143 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                               Write(q.Answer);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                            </div>\r\n                        </div>\r\n");


            
            #line 146 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                    }
                    else
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <!-- 单选题 -->\r\n");


            
            #line 150 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                        foreach (QuestionAnswer an in qu.QuestionAnswer.OrderBy(p => p.Order))
                        {
                            rightAnswer += an.AnswerFlag == 1 ? (rightAnswer == "" ? ((char)(an.Order + 64)).ToString() : ("、" + ((char)(an.Order + 64)))) : "";

            
            #line default
            #line hidden
WriteLiteral("                        <div>\r\n                            <input disabled=\"disab" +
"led\" type=\"");


            
            #line 154 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                         Write(type == 1 ? "radio" : "checkbox");

            
            #line default
            #line hidden
WriteLiteral("\" ");


            
            #line 154 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                                                              Write(("," + q.Answer + ",").IndexOf("," + an.Order + ",") >= 0 ? "checked=checked" : "");

            
            #line default
            #line hidden
WriteLiteral(" name=\"answer_");


            
            #line 154 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                                                                                                                                                                 Write(qu._id);

            
            #line default
            #line hidden
WriteLiteral("\" />");


            
            #line 154 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                                                                                                                                                                                                              Write((char)(an.Order + 64));

            
            #line default
            #line hidden
WriteLiteral("、\r\n                            ");


            
            #line 155 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                        Write(an.Answer);

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n");


            
            #line 156 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                        }
                    }
                }
                    

            
            #line default
            #line hidden
WriteLiteral("                    <div class=\"ans\">\r\n                        <div class=\"ans-ok" +
"\">\r\n                            <span class=\"tit\"><i></i>正确答案</span>\r\n          " +
"                  <p>");


            
            #line 163 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                           Write(rightAnswer);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                        </div>\r\n                    </div>\r\n");


            
            #line 166 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                      
                if (qu.QuestionAnalysis == "")
                {
                }
                else
                {

            
            #line default
            #line hidden
WriteLiteral("                        <div class=\"ans\">\r\n                            <div class" +
"=\"ans-res\">\r\n                                <span class=\"tit\"><i></i>试题解析</span" +
">\r\n                                <p>");


            
            #line 175 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                               Write(qu.QuestionAnalysis);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                            </div>\r\n                        </div>\r\n");


            
            #line 178 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                }
                    

            
            #line default
            #line hidden
WriteLiteral("                </div>\r\n            </div>\r\n");


            
            #line 182 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
            }
        

            
            #line default
            #line hidden
WriteLiteral(@"    </div>
    <center class=""mt10"">
        <input type=""button"" class=""btn btn-cancel"" value=""返回"" onclick=""backurl();"" /></center>
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


            
            #line 201 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
              Write(Url.Content("~/ClientBin/UploadFile/"));

            
            #line default
            #line hidden
WriteLiteral("\' + name;\r\n        var id = name.substring(0, name.length - 4);\r\n        var s1 =" +
" new swfupload();\r\n        s1.ckplayer_url = \'");


            
            #line 204 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
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


    function backurl()
    {
        if(""");


            
            #line 230 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
       Write(backurl);

            
            #line default
            #line hidden
WriteLiteral("\"==\"\")\r\n        {\r\n           window.location.href = \"/ExamTest/MyExaminationList" +
"\"; \r\n        }\r\n        else\r\n        {\r\n           window.location.href=\"");


            
            #line 236 "..\..\Views\ExamTest\ExamTestDetail.cshtml"
                            Write(backurl);

            
            #line default
            #line hidden
WriteLiteral("\";\r\n        }\r\n    }\r\n</script>\r\n");


        }
    }
}
#pragma warning restore 1591