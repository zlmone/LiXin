using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace LixinModels.ReResourceManage
{
	 	//Re_MyResource
    public class Re_MyResource
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// ResourceId
        /// </summary>
        public int ResourceId { get; set; }

        /// <summary>
        /// ThumbnailURL
        /// </summary>
        public string ThumbnailURL { get; set; }

        /// <summary>
        /// ResourceTypeID
        /// </summary>
        public int ResourceTypeID { get; set; }

        /// <summary>
        /// ResourceName
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// OpenTime
        /// </summary>
        public DateTime OpenTime { get; set; }

        /// <summary>
        /// Score
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Satisfaction
        /// </summary>
        public int Satisfaction { get; set; }

        /// <summary>
        /// Practical
        /// </summary>
        public int Practical { get; set; }

        /// <summary>
        /// Attention
        /// </summary>
        public int Attention { get; set; }

        /// <summary>
        /// IsCollection
        /// </summary>
        public int IsCollection { get; set; }

        /// <summary>
        /// CollectionTime
        /// </summary>
        public DateTime CollectionTime { get; set; }

        /// <summary>
        /// IsDownload
        /// </summary>
        public int IsDownload { get; set; }

        /// <summary>
        /// DownloadTime
        /// </summary>
        public DateTime DownloadTime { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// FavoriteId
        /// </summary>
        public int FavoriteId { get; set; }

        /// <summary>
        /// Format
        /// </summary>
        public int Format { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// OpenNum
        /// </summary>
        public int OpenNum { get; set; }

        /// <summary>
        /// DownNum
        /// </summary>
        public int DownNum { get; set; }

        /// <summary>
        /// Tag
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// TypeName
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// SatisfactionNum
        /// </summary>
        public int SatisfactionNum { get; set; }

        /// <summary>
        /// ScoreNum
        /// </summary>
        public int ScoreNum { get; set; }

        /// <summary>
        /// PracticalNum
        /// </summary>
        public int PracticalNum { get; set; }

        /// <summary>
        /// AttentionNum
        /// </summary>
        public int AttentionNum { get; set; }

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

        /// <summary>
        /// 总数
        /// </summary>
        public int totalCount
        {
            get;
            set;
        }
    }
}