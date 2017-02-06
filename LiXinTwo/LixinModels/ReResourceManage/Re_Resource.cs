using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Collections.Generic; 
using System.Data;

namespace LixinModels.ReResourceManage
{
	 	//Re_Resource
    public class Re_Resource
    {
        #region == model ==

        /// <summary>
        /// ResourceID
        /// </summary>
        public int ResourceID { get; set; }

        /// <summary>
        /// ResourceName
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// ResourceDesc
        /// </summary>
        public string ResourceDesc { get; set; }

        /// <summary>
        /// ResourceTypeID
        /// </summary>
        public int ResourceTypeID { get; set; }

        /// <summary>
        /// Format
        /// </summary>
        public int Format { get; set; }

        /// <summary>
        /// Suffix
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// Tag
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// ThumbnailURL
        /// </summary>
        public string ThumbnailURL { get; set; }

        /// <summary>
        /// ResultURL
        /// </summary>
        public string ResultURL { get; set; }

        /// <summary>
        /// isOpen
        /// </summary>
        public int isOpen { get; set; }

        /// <summary>
        /// isDownload
        /// </summary>
        public int isDownload { get; set; }

        /// <summary>
        /// UpTime
        /// </summary>
        public DateTime UpTime { get; set; }

        /// <summary>
        /// UpUserID
        /// </summary>
        public int UpUserID { get; set; }

        /// <summary>
        /// LastTime
        /// </summary>
        public DateTime LastTime { get; set; }

        /// <summary>
        /// OpenNum
        /// </summary>
        public int OpenNum { get; set; }

        /// <summary>
        /// DownNum
        /// </summary>
        public int DownNum { get; set; }

        /// <summary>
        /// CollectionNum
        /// </summary>
        public int CollectionNum { get; set; }

        /// <summary>
        /// Score
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// ScoreNum
        /// </summary>
        public int ScoreNum { get; set; }

        /// <summary>
        /// Satisfaction
        /// </summary>
        public double Satisfaction { get; set; }

        /// <summary>
        /// SatisfactionNum
        /// </summary>
        public int SatisfactionNum { get; set; }

        /// <summary>
        /// Practical
        /// </summary>
        public double Practical { get; set; }

        /// <summary>
        /// PracticalNum
        /// </summary>
        public int PracticalNum { get; set; }

        /// <summary>
        /// Attention
        /// </summary>
        public double Attention { get; set; }

        /// <summary>
        /// AttentionNum
        /// </summary>
        public int AttentionNum { get; set; }

        /// <summary>
        /// IsRecommend
        /// </summary>
        public int IsRecommend { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// OpenFlag
        /// </summary>
        public int OpenFlag { get; set; }

        /// <summary>
        /// OpenGroupObject
        /// </summary>
        public string OpenGroupObject { get; set; }

        /// <summary>
        /// OpenDepartObject
        /// </summary>
        public string OpenDepartObject { get; set; }

        public string TypeName { get; set; }

        #endregion

        /// <summary>
        /// 上传者
        /// </summary>
        public string UpUserName { get; set; }

        /// <summary>
        /// 最后更新时间显示
        /// </summary>
        public string LastTimeStr
        {
            get { return LastTime.ToString("yyyy-MM-dd"); }
        }

        /// <summary>
        /// 封面路径显示
        /// </summary>
        public string ThumbnailURLStr
        {
            get
            {
                string picUrlDir = ConfigurationManager.AppSettings["ReResourceImg"].TrimStart('~');
                string picUrl = "/Images/photo/default.jpg";
                if (!string.IsNullOrWhiteSpace(ThumbnailURL))
                {
                    string picUrl1 = string.Format("{0}{1}", picUrlDir, ThumbnailURL);
                    if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(picUrl1)))
                    {
                        picUrl = picUrl1;
                    }
                }
                return picUrl;
            }
        }

        public int totalcount
        {
            get;
            set;
        }
    }
}