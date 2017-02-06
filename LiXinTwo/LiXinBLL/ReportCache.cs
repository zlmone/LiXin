using LiXinBLL.Report_fVedio;
using LiXinBLL.Report_Vedio;
using LiXinModels.Examination.DBModel;
using LiXinModels.Report_Vedio;
using LiXinModels.Report_zVedio;
using Retech.Cache;
using Retech.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL.Cache
{
    public class ReportCache
    {
        protected ICacheManager cacheManager = new MemoryCacheManager();

        /// <summary>
        /// 视频课程 应参加人数
        /// </summary>
        public List<VedioNumber> Vedio_NumberList
        {
            get
            {
                return cacheManager.Get("Vedio_Number", 1440, () => { return (new Report_VedioBL()).GetCacheVedioSumNumber(); });
            }
        }


        /// <summary>
        /// 视频课程每个部门应参加的人数 
        /// </summary>
        public List<fVedioJoinNumber> FVedio_NumberList
        {
            get
            {
                var list = cacheManager.Get("fVedio_JoinNumber", 1440, () => { return (new Report_VedioBL()).GetfVedioJoinNumber(); });
                if (list != null)
                {
                    return list;
                }
                return new List<fVedioJoinNumber>();
            }
        }


        /// <summary>
        ///获取视频课程的应该参与人员的评分
        /// </summary>
        public List<UserSurvey> UserSurveyList
        {
            get
            {
                return cacheManager.Get("Vedio_Survey", 1440, () => { return (new Report_VedioBL()).GetUserSurvey(); });
            }
        }

        /// <summary>
        /// 考核范围内的部门 人员
        /// </summary>
        public List<dynamic> GetCheckList
        {
            get
            {
                // return (new Report_VedioBL()).GetCheckUserList();
                return cacheManager.Get("Check_User", 1440, () => { return (new Report_VedioBL()).GetCheckUserList(); });
            }
        }

        public List<tbExamSendStudent> VedioExamList
        {
            get
            {
                // return (new Report_VedioBL()).GetCheckUserList();
                return cacheManager.Get("Vedio_Exam", 1440, () => { return (new Report_VedioBL()).GetExamList(); });
            }
        }


    }
}
