using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LiXinControllers
{
    public class GenerateWord 
    {
        public void ExportGenerateWord(HttpContextBase Context, string css, string Body, string downloadfilename)
        {
            var Html = new StringBuilder();
            //头部
            Html.AppendFormat(@"<html xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office'
xmlns:w='urn:schemas-microsoft-com:office:word' xmlns:m='http://schemas.microsoft.com/office/2004/12/omml'
xmlns='http://www.w3.org/TR/REC-html40'><head> <style>{0}</style>", css);
            //打开后默认的视图即为页面视图
            Html.Append(@"<!--[if gte mso 9]><xml><w:WordDocument><w:View>Print</w:View><w:TrackMoves>false</w:TrackMoves><w:TrackFormatting/><w:ValidateAgainstSchemas/><w:SaveIfXMLInvalid>false</w:SaveIfXMLInvalid><w:IgnoreMixedContent>false</w:IgnoreMixedContent><w:AlwaysShowPlaceholderText>false</w:AlwaysShowPlaceholderText><w:DoNotPromoteQF/><w:LidThemeOther>EN-US</w:LidThemeOther><w:LidThemeAsian>ZH-CN</w:LidThemeAsian><w:LidThemeComplexScript>X-NONE</w:LidThemeComplexScript><w:Compatibility><w:BreakWrappedTables/><w:SnapToGridInCell/><w:WrapTextWithPunct/><w:UseAsianBreakRules/><w:DontGrowAutofit/><w:SplitPgBreakAndParaMark/><w:DontVertAlignCellWithSp/><w:DontBreakConstrainedForcedTables/><w:DontVertAlignInTxbx/><w:Word11KerningPairs/><w:CachedColBalance/><w:UseFELayout/></w:Compatibility><w:BrowserLevel>MicrosoftInternetExplorer4</w:BrowserLevel><m:mathPr><m:mathFont m:val='Cambria Math'/><m:brkBin m:val='before'/><m:brkBinSub m:val='--'/><m:smallFrac m:val='off'/><m:dispDef/><m:lMargin m:val='0'/> <m:rMargin m:val='0'/><m:defJc m:val='centerGroup'/><m:wrapIndent m:val='1440'/><m:intLim m:val='subSup'/><m:naryLim m:val='undOvr'/></m:mathPr></w:WordDocument></xml><![endif]--></head>");
            Html.AppendFormat(@"<body>{0}</body></html>", Body);

            Context.Response.Clear();
            Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");

            if (Context.Request.UserAgent.ToLower().IndexOf("msie") > -1)
            {
                downloadfilename = HttpUtility.UrlPathEncode(downloadfilename);
                Context.Response.AddHeader("content-disposition", "attachment;filename=" + downloadfilename);
            }

            if (Context.Request.UserAgent.ToLower().IndexOf("firefox") > -1)
            {
                Context.Response.AddHeader("content-disposition", "attachment;filename=\"" + downloadfilename + "\"");
            }

            else
            {
                Context.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(downloadfilename, Encoding.UTF8));
            }

            Context.Response.ContentType = "application/ms-word";

            Context.Response.Write(Html.ToString());//获取table标签
            Context.Response.End();

        }
    }
}
