using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Configuration;

namespace LiXinControllers
{
    public class GlobalThreading : BaseController
    {
        private static readonly TimeSpan waitTime = new TimeSpan(0, 20, 0);
        private static GlobalThreading Singleton;
        private Thread thread = null;
        private bool m_Flag;
        private LiXinBLL.SystemManage.Sys_LogBL logBL = new LiXinBLL.SystemManage.Sys_LogBL();

        private GlobalThreading()
        {
            Start();
        }

        private void Processes()
        {
            while (m_Flag)
            {
                var dt = DateTime.Now;

                #region 用户同步

                //执行同步用户模块
                if (DateTime.Now.Hour == 0 && DateTime.Now.Minute < 20)
                {
                    try
                    {
                        new LiXinSync.SyncInfo().Sync();
                        SyncLog("同步用户", "同步用户成功！");
                    }
                    catch (Exception e)
                    {
                        var mess = "同步用户失败！" + e.Message;
                        SyncLog("同步用户", mess);
                        var list = new List<string>();
                        list.Add(SyncResultEmail);
                        SendEmail(list, "同步用户失败", mess);
                    }
                    finally
                    {
                        RefreshDepartmentCache();
                        RefreshPostCache();
                        RefreshTrainGrade();
                    }
                }
                #endregion

                #region 暂不同步

                //执行同步用户指纹模块
                //if (DateTime.Now.Hour == 1 && DateTime.Now.Minute < 20)
                //{
                //    try
                //    {
                //        new LiXinSync.SyncInfo().SyncUserFinger();
                //        SyncLog("同步用户指纹", "同步用户指纹成功！");
                //    }
                //    catch (Exception e)
                //    {
                //        var mess = "同步用户指纹失败！" + e.Message;
                //        SyncLog("同步用户指纹", mess);
                //        var list = new List<string>();
                //        list.Add(SyncResultEmail);
                //        SendEmail(list, "同步用户指纹失败", mess);
                //    }
                //}
                

                //执行补预定审批结果处理
                //try
                //{
                //    if (DateTime.Now.Hour == 2 && DateTime.Now.Minute < 20)
                //    {
                //        new LiXinBLL.MyApproval.MyApprovalBL().DealMakeUpCourseOrder(Convert.ToInt32(AllSystemConfigs.FirstOrDefault(p => p.ConfigType == 20).ConfigValue));
                //    }
                //}
                //catch { }

                #endregion

                #region 课程报名，排队中的人员递补
                //课程报名，排队中的人员递补
                if (dt.Minute < 20)
                {
                    try
                    {
                        var list = new LiXinBLL.CourseLearn.CourseOrderBL().UpdateCourseOrderStatus(dt.AddHours(-1));
                        var content = GetFormworkContent(5);
                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            var messList = new List<KeyValuePair<string, string>>();
                            var emailList = new List<KeyValuePair<string, string>>();
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (!string.IsNullOrWhiteSpace(list[i].realname))
                                {
                                    var c = string.Format(content,
                                                        list[i].realname,
                                                        list[i].OrderTime.Value.ToString("yyyy-MM-dd HH:mm"),
                                                        list[i].Course_Name);
                                    if (!string.IsNullOrWhiteSpace(list[i].userMobileNum))
                                        messList.Add(new KeyValuePair<string, string>(list[i].userMobileNum, c));
                                    if (!string.IsNullOrWhiteSpace(list[i].userEmail))
                                        emailList.Add(new KeyValuePair<string, string>(list[i].userEmail, c));
                                }
                            }
                            if (messList.Count > 0)
                                SendMessage(messList);
                            if (emailList.Count > 0)
                                SendEmail(emailList, "排队中的课程，报名成功了！");
                        }
                    }
                    catch
                    {
                    }
                }
                #endregion

                #region 集中授课，提醒讲师上课，20分钟执行一次

                try
                {
                    var con = GetFormworkContent(6);
                    if (!string.IsNullOrWhiteSpace(con))
                    {
                        var remindspan = 1;
                        int totalCount = 0;
                        //保证每20分钟执行一次，时间上不冲突、不重叠
                        //0-19分：0；20-39：20；40-59：40；
                        var start =
                            DateTime.Parse(dt.ToString("yyyy-MM-dd HH") + ":" + (dt.Minute/20)*20 + ":00").AddHours(
                                remindspan);
                        var end = start.AddMinutes(20);
                        var courseList = new LiXinBLL.CourseManage.Co_CourseBL().GetCourseCommonList(out totalCount,
                                                                                                     string.Format(
                                                                                                         "Co_Course.Way = 1 and Co_Course.CourseFrom = 2 and Co_Course.starttime >= '{0}' and Co_Course.starttime < '{1}' and Co_Course.publishflag = 1 and Co_Course.Isdelete = 0",
                                                                                                         start, end));
                        var messList = new List<KeyValuePair<string, string>>();
                        var emailList = new List<KeyValuePair<string, string>>();
                        for (int i = 0; i < courseList.Count; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(courseList[i].TeacherName))
                            {
                                string c = string.Format(con,
                                                         courseList[i].TeacherName,
                                                         courseList[i].CourseName,
                                                         courseList[i].StartTime.ToString("yyyy-MM-dd HH:mm"),
                                                         courseList[i].RoomName);
                                if (!string.IsNullOrWhiteSpace(courseList[i].TeacherMobileNum))
                                    messList.Add(new KeyValuePair<string, string>(courseList[i].TeacherMobileNum, c));
                                if (!string.IsNullOrWhiteSpace(courseList[i].TeacherEmail))
                                    emailList.Add(new KeyValuePair<string, string>(courseList[i].TeacherEmail, c));
                            }
                        }
                        if (messList.Count > 0)
                            SendMessage(messList);
                        if (emailList.Count > 0)
                            SendEmail(emailList, "快要开课了哦，赶紧准备吧！");
                    }
                }
                catch (Exception e)
                {
                }

                #endregion

                Thread.Sleep(waitTime);
            }
        }

        public static GlobalThreading Instance()
        {
            return Singleton ?? (Singleton = new GlobalThreading());
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

        /// <summary>
        /// 记同步日志
        /// </summary>
        /// <param name="logModule">模块名称</param>
        /// <param name="logContent">日志内容</param>
        private void SyncLog(string logModule, string logContent)
        {
            var log = new LiXinModels.SystemManage.Sys_Log()
            {
                ClientIP = "",
                LogTime = DateTime.Now,
                LogType = 3,
                LogContent = logContent,
                ModuleName = logModule,
                RequestUrl = "",
                CurrentUserId = -1//同步的用户Id定为-1
            };
            logBL.Add(log);
        }
    }
}
