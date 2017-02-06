using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using Retech.Cache;
using Retech.Core.Cache;
using LiXinBLL.Report_Vedio;
using LiXinBLL.Report_fVedio;
using LiXinModels;
using LiXinBLL.Report_AllData;
using LiXinModels.Report_Vedio;
using LiXinBLL;
using LiXinBLL.Report_Together;

namespace LiXinControllers
{
    public class ReportCache : BaseController
    {
        private static readonly TimeSpan waitTime = new TimeSpan(0, 20, 0);
        private Thread thread = null;
        private bool m_Flag;
        protected ICacheManager cacheManager = new MemoryCacheManager();
        private static ReportCache Singleton;
        private ReportCache()
        {
            Start();
        }

        public void Start()
        {
            if (Status == ThreadState.Stopped)
            {
                thread = new Thread(Processes);
                m_Flag = true;
                thread.Start();
            }
        }
        private ThreadState Status
        {
            get
            {
                if (thread == null)
                {
                    return ThreadState.Stopped;
                }
                return thread.ThreadState;
            }
        }

        public static ReportCache Instance()
        {
            return Singleton ?? (Singleton = new ReportCache());
        }

        /// <summary>
        /// 每天12点缓存数据
        /// </summary>
        private void Processes()
        {
            while (m_Flag)
            {
                var dt = DateTime.Now;
                if (DateTime.Now.Hour == 0 && DateTime.Now.Minute < 20)
                {
                    try
                    {

                        //缓存课程应学习的总人数
                        GetCacheVedioSumNumber();
                        GetfVedioJoinNumber();
                        GetfVedioSurvey();
                        GetCheckUser();
                        //  GetExamList();
                        GetAllCourseScore();
                        GetTogetherUser();
                        GetDepCourseUser();
                        GetCacheCourseInfoList();
                        GetCourseNumber();
                        m_Flag = false;
                    }
                    catch (Exception e)
                    {

                    }
                    finally
                    {
                        //RefreshVedioSumNumber();
                    }


                }
            }
        }

        #region 缓存内容
        /// <summary>
        ///缓存课程应学习的总人数
        /// </summary>
        public void GetCacheVedioSumNumber()
        {
            cacheManager.Get("Vedio_Number", 1440, () => { return (new Report_VedioBL()).GetCacheVedioSumNumber(); });
        }

        /// <summary>
        ///获取视频课程的应该参与人员对应的部门
        /// </summary>
        public void GetfVedioJoinNumber()
        {
            cacheManager.Get("fVedio_JoinNumber", 1440, () => { return (new Report_VedioBL()).GetfVedioJoinNumber(); });
        }

        /// <summary>
        ///获取视频课程的应该参与人员的评分
        /// </summary>
        public void GetfVedioSurvey()
        {
            cacheManager.Get("Vedio_Survey", 1440, () => { return (new Report_VedioBL()).GetUserSurvey(); });
        }


        /// <summary>
        ///考核范围内的人
        /// </summary>
        public void GetCheckUser()
        {
            cacheManager.Get("Check_User", 1440, () => { return (new Report_VedioBL()).GetCheckUserList(); });
        }

        /// <summary>
        ///课程考试缓存
        /// </summary>
        public void GetExamList()
        {
            cacheManager.Get("Vedio_Exam", 1440, () => { return (new Report_VedioBL()).GetExamList(); });
        }

        /// <summary>
        /// 总表报数据处理
        /// </summary>
        public void GetAllCourseScore()
        {
            Sys_ParamConfig configType14 = AllSystemConfigs.Where(p => p.ConfigType == 14).FirstOrDefault();
            Sys_ParamConfig configType13 = AllSystemConfigs.Where(p => p.ConfigType == 13).FirstOrDefault();
            Sys_ParamConfig configType16 = AllSystemConfigs.Where(p => p.ConfigType == 16).FirstOrDefault();
            Sys_ParamConfig configType27 = AllSystemConfigs.Where(p => p.ConfigType == 27).FirstOrDefault();
            int yearplan = DateTime.Now.Year;

            //cacheManager.Remove("courseReport" + yearplan);

            cacheManager.Get(("courseReport" + yearplan), 1440, () =>
            {
                return new Report_AllData().GetAllCourseReport(configType14, configType13, configType16, configType27, yearplan);
            });

        }
        /// <summary>
        /// 每个部门分所应参加的人数
        /// </summary>
        /// <returns></returns>
        public void GetTogetherUser()
        {
            cacheManager.Get("together_number", 1440, () => { return (new ReportTogetherBL()).GetTogetherUser(); });
        }


        /// <summary>
        /// 部门自学每个部门分所应参加的人数
        /// </summary>
        /// <returns></returns>
        public void GetDepCourseUser()
        {
            cacheManager.Get("DepCourse_number", 1440, () => { return (new Report_DepCourseBL()).GetDepCourseUser(); });
        }



        /// <summary>
        /// 集中授课个部门参与考试的人数
        /// </summary>
        /// <returns></returns>
        public void GetCourseNumber()
        {
            cacheManager.Get("together_examnumber", 1440, () => { return (new ReportTogetherBL()).GetCourseNumber(); });
        }

        /// <summary>
        /// 缓存 课程的基本信息 实际参与人数人全部的信息
        /// </summary>
        /// <returns></returns>
        public void GetCacheCourseInfoList()
        {
            cacheManager.Get("together_CourseInfoList", 1440, () => { return (new ReportTogetherBL()).GetCacheCourseInfoList(); });
        }
       // 
        #endregion

    }
}
