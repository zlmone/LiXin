using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using LiXinModels.CourseManage;
using Retech.Cache;
using Retech.Core.Cache;
using LiXinBLL.User;
using LiXinControllers.Filter;
using LiXinModels;
using LiXinModels.User;
using System.Xml.Linq;
using LiXinCommon.Configs;
using LiXinBLL.SystemManage;
using LiXinBLL;
using System;
using LiXinCommon;
using Newtonsoft.Json;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using RabbitMQClient;
using System.Xml;


namespace LiXinControllers
{
    [BaseAccess, HandleError, AjaxSession]
    [ValidateInput(false)]
    public class BaseController : Controller
    {
        public static string BaseCompanyUrl = "";

        //    <!--集中授课上传时 保存的资源文件及附件路径  UFCO 意为 Upload Files Course -->
        public static string UFCOResource = ConfigurationManager.AppSettings["UFCOResource"];
        //     <!--视频课程上传时 保存的SCORM资源文件路径  UFCO 意为 Upload Files Course -->
        public static string UFCOScorm = ConfigurationManager.AppSettings["UFCOScorm"];


        //<!--考勤 签到表的文件路径 -->
        public static string AttendceUrl = ConfigurationManager.AppSettings["AttendceUrl"];

        //<!--问卷的文件路径 -->
        public static string SurveyUrl = ConfigurationManager.AppSettings["SurveyUrl"];

        //<!--头像原文件的文件路径 -->
        public static string OLDphotoUrl = ConfigurationManager.AppSettings["OLDphotoUrl"];

        //<!--头像新文件的文件路径 -->
        public static string NEWphotoUrl = ConfigurationManager.AppSettings["NEWphotoUrl"];

        public static string pathurl = ConfigurationManager.AppSettings["PrinUrl"].ToString();

        public static string UFCOVideo = ConfigurationManager.AppSettings["UFCOVideo"].ToString();

        /// <summary>
        /// 培训FQA附件
        /// </summary>
        public static string UFCONoteResource = ConfigurationManager.AppSettings["UFCONoteResource"].ToString();

        public static string UFCOVideoZIP = ConfigurationManager.AppSettings["UFCOVideoZIP"].ToString();

        public static string UFCOVideoADDR = ConfigurationManager.AppSettings["UFCOVideoADDR"].ToString();
        public static string UFCOVideoUSR = ConfigurationManager.AppSettings["UFCOVideoUSR"].ToString();
        public static string UFCOVideoPwd = ConfigurationManager.AppSettings["UFCOVideoPwd"].ToString();

        //同步结果发送至邮箱
        public static string SyncResultEmail = ConfigurationManager.AppSettings["SyncResultEmail"].ToString();

        protected ICacheManager cacheManager = new MemoryCacheManager();

        #region 消息队列定义变量

        //RabbitMQ所在的服务器路径 -->
        public static string Rabbit_serverAddress = ConfigurationManager.AppSettings["Rabbit_serverAddress"];
        //用户 -->
        public static string Rabbit_userName = ConfigurationManager.AppSettings["Rabbit_userName"];
        //密码-->
        public static string Rabbit_password = ConfigurationManager.AppSettings["Rabbit_password"];
        // 知识分享 虚拟机名
        public static string Rabbit_ShareUser = ConfigurationManager.AppSettings["Rabbit_ShareUser"];
        //交换模式 -->
        public static string Rabbit_exchange = ConfigurationManager.AppSettings["Rabbit_exchange"];
        //路由关键字 -->
        public static string Rabbit_routingKey = ConfigurationManager.AppSettings["Rabbit_routingKey"];
        //队列名称-->
        public static string Rabbit_queue = ConfigurationManager.AppSettings["Rabbit_queue"];
        //队列名称-->
        public static string email_queue = ConfigurationManager.AppSettings["email_queue"];
        //发送短信 虚拟机名
        public static string Rabbit_fileTrans = ConfigurationManager.AppSettings["Rabbit_SendMessageTrans"];

        //发送邮件 虚拟机名
        public static string Rabbit_SendEmailHost = ConfigurationManager.AppSettings["Rabbit_SendEmail"];

        #endregion

        /// <summary>
        ///     当前登录系统的用户
        /// </summary>
        protected Sys_User CurrentUser
        {
            get
            {
                var user = Session["currentUser"] as Sys_User;
                if (user == null)
                    HttpContext.Response.Redirect("/Login");
                return user;
            }
            set
            {
                Session["userID"] = value.UserId;
                Session["userName"] = value.Username;
                Session["realName"] = value.Realname;
                Session["currentUser"] = value;
                Session["moduleName"] = "perCenter";
            }
        }

        protected Sys_User InitSuper
        {
            get
            {
                return new Sys_User
                {
                    UserId = 0,
                    Username = "superadmin",
                    Realname = "superadmin"
                };
            }
        }

        /// <summary>
        ///     用户所拥有的权限
        /// </summary>
        protected List<Sys_Right> UserRights
        {
            get
            {
                object myrights = Session["myRights"];
                return myrights == null ? new List<Sys_Right>() : myrights as List<Sys_Right>;
            }
            set
            {
                Session["myRights"] = value;
            }
        }

        /// <summary>
        ///     系统中所有的权限
        /// </summary>
        protected List<Sys_Right> AllRights
        {
            get
            {
                return cacheManager.Get("all_rights", () =>
                {
                    return new RightBL().GetAllRights();
                });
            }
        }

        /// <summary>
        ///     系统中所有的岗位
        /// </summary>
        protected List<Sys_Post> AllPosts
        {
            get
            {
                return cacheManager.Get("all_posts", () =>
                {
                    return new PostBL().GetAllPost();
                });
            }
        }

        /// <summary>
        ///     记录系统中所有的区域和店铺信息
        /// </summary>
        protected List<Sys_Department> AllDepartments
        {
            get
            {
                return cacheManager.Get("all_depts", () =>
                {
                    return new DepartmentBL().GetAllList();
                });
            }
        }


        public string DeptName = "业务一部,北京分所";

        protected List<Sys_Department> GetDepartments
        {
            get {

;                var departments=AllDepartments;
                //AllDepartments  部门表里所有内容

                string[] arr=DeptName.Split(',');
                List<Sys_Department> t = new List<Sys_Department>();
                List<Sys_Department> list = null;
                for (int i = 0; i < arr.Length; i++)
                {
                    var id = AllDepartments.Where(p => p.DeptName == arr[i]).FirstOrDefault();
                    if (id != null)
                    {
                        if (id.ParentDeptId == 1) //直接是父及
                        {
                            t.Add(AllDepartments.Where(p => p.DeptName == arr[i]).FirstOrDefault());
                            list.Add(AllDepartments.Where(p => p.DepartmentId == id.ParentDeptId).FirstOrDefault());
                            t.AddRange(list);
                        }
                        else
                        {
                            list = AllDepartments.Where(p => p.ParentDeptId == id.ParentDeptId).ToList();
                            t.AddRange(list);
                            list.Add(AllDepartments.Where(p => p.DepartmentId == id.ParentDeptId).FirstOrDefault());
                        }     
                    }
                               
                }
                return t;
                
            }
        }




        /// <summary>
        ///     记录系统中所有的培训级别
        /// </summary>
        protected List<string> AllTrainGrade
        {
            get
            {
                return cacheManager.Get("all_traingrade", () =>
                {
                    return new Sys_TrainGradeBL().GetAllTrainGrade();
                });
            }
        }


        /// <summary>
        ///    记录系统中所有的邮件/短信模板
        /// </summary>
        protected XmlNodeList AllFormwork
        {
            get
            {
                return cacheManager.Get("all_formwork", () =>
                {
                    try
                    {
                        var xmlDoc = new XmlDocument();
                        var path = (HttpRuntime.AppDomainAppPath + "/Configs/Formwork.xml");
                        xmlDoc.Load(path);
                        return xmlDoc.SelectSingleNode("FormworkList").ChildNodes;
                    }
                    catch
                    {
                        return null;
                    }
                });
            }
        }

        public XmlNodeList GetMessageTemplate()
        {
            var xmlDoc = new XmlDocument();
            var path = (HttpRuntime.AppDomainAppPath + "Configs\\Formwork.xml");
            xmlDoc.Load(path);
            return xmlDoc.SelectSingleNode("FormworkList").ChildNodes;

        }
        //protected  List<string> AllTrainGrade
        //{
        //    get
        //    {
        //        return cacheManager.Get("all_trainGrade", () =>
        //        {
        //            return new Sys_TrainGradeBL().GetAllPayGrade();
        //        });
        //    }
        //}

        /// <summary>
        ///     记录系统中所有的系统配置信息
        /// <para>configType/configValue</para>
        /// <para>1/邮箱配置</para>
        /// <para>2/短信配置</para>
        /// <para>3/培训年度设定</para>
        /// <para>7/培训年度设定</para>
        /// <para>8/CPA培训周期设定</para>
        /// <para>9/培训级别统一调整时间设定</para>
        /// <para>10/各部门/分所培训负责人设定时限</para>
        /// <para>11/培训需求上报时间设定</para>
        /// <para>13/各级别当年度内部培训的目标学时</para>
        /// <para>14/各培训形式当年度内部培训的获取学时上限</para>
        /// <para>16/CPA年度考核目标学时</para>
        /// <para>17/CPA培训周期考核目标学时</para>
        /// <para>18/必修/选修课程学时获取上限</para>
        /// <para>19/全年课程退订次数</para>
        /// <para>20/全年课程补预定次数</para>
        /// <para>21/排队状态更改为正常预定状态的时间点</para>
        /// <para>22/请假审核时限</para>
        /// <para>23/补预定审核时限</para>
        /// <para>24/集中授课学时折算分布</para>
        /// <para>25/视频课程课后可参加在线测试条件</para>
        /// <para>26/在线测试允许答题时间</para>
        /// <para>27/在线测试允许最大次数</para>
        /// <para>29/课前建议、课后评估的开放时间</para>
        /// <para>30/参加课后评估获取奖励学时上限</para>
        /// <para>31/请假条模板</para>
        /// <para>32/违纪情况的学时扣除</para>
        /// <para>5/违纪学时 早退/迟到   1;2;3-2;3;4|1;2;3-2;3;4</para>
        /// <para>33/全年在线测试完成次数 </para>
        /// </summary>
        protected List<Sys_ParamConfig> AllSystemConfigs
        {
            get
            {
                return cacheManager.Get("all_systemconfigs", () =>
                {
                    return new Sys_ParamConfigBL().GetList();
                });
            }
        }

        public LoginConfigurationSection LoginConfig
        {
            get
            {
                return (LoginConfigurationSection)ConfigurationManager.GetSection("loginConfigs");
            }
        }

        protected void RefreshRightCache()
        {
            cacheManager.Remove("all_rights");
            cacheManager.Get("all_rights", () =>
            {
                return new RightBL().GetAllRights();
            });
        }

        protected void RefreshDepartmentCache()
        {
            cacheManager.Remove("all_depts");
            cacheManager.Get("all_depts", () =>
            {
                return new DepartmentBL().GetAllList();
            });
        }

        protected void RefreshSystemConfigsCache()
        {
            cacheManager.Remove("all_systemconfigs");
            cacheManager.Get("all_systemconfigs", () =>
            {
                return new Sys_ParamConfigBL().GetList();
            });
        }

        protected void RefreshPostCache()
        {
            cacheManager.Remove("all_posts");
            cacheManager.Get("all_posts", () =>
            {
                return new PostBL().GetAllPost();
            });
        }

        protected void RefreshTrainGrade()
        {
            cacheManager.Remove("all_traingrade");
            cacheManager.Get("all_traingrade", () =>
            {
                return new Sys_TrainGradeBL().GetAllTrainGrade();
            });
        }



        //protected void RefreshTrainGrade()
        //{
        //    cacheManager.Remove("all_trainGrade");
        //    cacheManager.Get("all_trainGrade", () =>{ return new Sys_TrainGradeBL().GetAllPayGrade();});
        //}

        public bool ExistsCache(string cacheKey)
        {
            return HttpContext.Cache[cacheKey] != null;
        }

        /// <summary>
        /// 根据配置文件和当前时间，获取本年度的开始时间和结束时间
        /// 如（startTime = 2012-10-01 0:00:00 ; endTime = 2012-12-31 23:59:59 ）
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        protected void GetYearStartAndEnd(out DateTime startTime, out DateTime endTime, int year)
        {
            Sys_ParamConfig yearConfig = AllSystemConfigs.Find(p => p.ConfigType == 7);
            new Tr_MonthPlanBL().GetYearStartAndEnd(out  startTime, out  endTime, yearConfig, year);
        }


        #region 日期方法

        /// <summary>
        ///     初始化日历集合
        /// </summary>
        /// <param name="calendarList">数据集合</param>
        /// <param name="date">日期</param>
        public static void InitCalendarTaskCollection(ref List<CalendarTask> calendarList, DateTime date)
        {
            for (int i = GetNumberWeek(date); i > 0; i--)
            {
                calendarList.Add(new CalendarTask
                {
                    Year = date.Year,
                    Month = date.AddMonths(-1).Month,
                    Day = date.AddDays(-i).Day,
                    TaskTotal = 0,
                    Bg = 0,
                    MoringStr = "",
                    AfterStr = ""
                });
            }
        }

        /// <summary>
        ///     添加后月的天数
        /// </summary>
        /// <param name="calendarTask">数据集合</param>
        /// <param name="quMonth">当前月的第一天</param>
        public static void AddNextMonthDays(ref List<CalendarTask> calendarTask, DateTime quMonth)
        {
            DateTime lastdate = quMonth.AddMonths(1);
            int nocount = 42 - calendarTask.Count;
            for (int i = 0; i < nocount; i++)
            {
                DateTime tempdate = lastdate.AddDays(i);
                calendarTask.Add(new CalendarTask
                                     {
                                         Year = tempdate.Year,
                                         Month = tempdate.Month,
                                         Day = tempdate.Day,
                                         TaskTotal = 0,
                                         Bg = 0,
                                         MoringStr = "",
                                         AfterStr = ""
                                     });
            }
        }

        /// <summary>
        ///     获取一周的第几天
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static int GetNumberWeek(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Saturday:
                    return 5;
                case DayOfWeek.Sunday:
                    return 6;
                default:
                    return 0;
            }
        }

        #endregion

        /// <summary>
        /// 获取模板内容
        /// <para>0：总所指定【参数：0，学员姓名；1，报名时间；2：课程名称；3：课程开课时间；】</para>
        /// <para>1：部门负责人指定【参数：0，学员姓名；1：报名人姓名；2，报名时间；3：课程名称；4：课程开课时间；】</para>
        /// <para>2：学员收到的信息提醒【参数：0，学员姓名；1，讲师名称；2，课程名称；3：课程开课时间；】</para>
        /// <para>3：请假审核【参数：0，审核人姓名；1，请假人姓名；2：请假原因；3：请假时间；4：课程名称；5：最后审批时间；】</para>
        /// <para>4：请假审核通过/未通过【参数：0，学员姓名；1，请假时间；2：课程名称；3：审批人姓名；4：审批时间；5：审批状态(审批通过/未通过)】</para>
        /// <para>5：集中授课由排队状态更改为正常预定状态，需发送提醒学员本人【参数：0，学员姓名；1，报名时间；2：课程名称；】</para>
        /// <para>6：集中授课开课前的一个时点，需发送提醒信息至授课人【参数：0，讲师姓名；1，课程名称；2：课程开课时间；3：课程开课地点；】</para>
        ///<para>7：考勤管理-补预订申请【参数：0，审核人姓名；1，申请人；2：课程名称】</para>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetFormworkContent(int id)
        {
            if (AllFormwork != null)
            {
                foreach (var item in AllFormwork)
                {
                    XmlElement xe = (XmlElement)item;
                    if (id == xe.GetAttribute("id").StringToInt32())
                    {
                        return xe.ChildNodes.Item(1).InnerText;
                    }
                }
            }
            return "";
        }

        #region 发送短信/邮件

        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="telephone">号码</param>
        /// <param name="content">内容</param>
        public void SendMessage(List<string> telephone, string content)
        {
           
            SmsMessage message = new SmsMessage();
            message.telePhoneList = telephone;
            message.message = content;
            message.config = (new Sys_ParamConfigBL()).GetSmsConfing(AllSystemConfigs.Where(p => p.ConfigType == 2).FirstOrDefault());
            new RabbitMQHelper(Rabbit_serverAddress, Rabbit_userName, Rabbit_password, Rabbit_fileTrans, Rabbit_queue).SendMessage(message);
            //RabbitMQHelper.Instance.SendMessage(message);
        }

        /// <summary>
        /// 短信发送（每人的短信内容不一样时，发送）
        /// </summary>
        /// <param name="list">手机号码、短信内容</param>
        public void SendMessage(List<KeyValuePair<string, string>> list)
        {
            try
            {
                SmsMessage message = new SmsMessage();
                if (list != null && list.Count > 0)
                {
                    message.list = list;
                    message.config =
                        (new Sys_ParamConfigBL()).GetSmsConfing(
                            AllSystemConfigs.Where(p => p.ConfigType == 2).FirstOrDefault());
                    new RabbitMQHelper(Rabbit_serverAddress, Rabbit_userName, Rabbit_password, Rabbit_fileTrans,
                                       Rabbit_queue).SendMessage(message);
                }
            }
            catch
            {

            }
        }


        /// <summary>
        ///  邮件发送
        /// </summary>
        /// <param name="listEmailAddress">收件人地址</param>
        /// <param name="strTitle">邮件主题</param>
        /// <param name="strContent">邮件内容</param>
        /// <param name="mailName">邮件名称（考试、培训等）</param>
        public void SendEmail(List<string> listEmailAddress, string strTitle, string strContent, string mailName = "")
        {
            try
            {
                EmailInforMation emailinf = new EmailInforMation();
                emailinf.ToList = listEmailAddress;
                emailinf.SendFlag = 1;
                emailinf.Subject = strTitle;
                if (!string.IsNullOrEmpty(mailName))
                {
                    emailinf.mailName = mailName;
                }
                emailinf.Context = strContent;
                string[] emailStr =
                    AllSystemConfigs.Where(p => p.ConfigType == 1).FirstOrDefault().ConfigValue.Split(';');
                if (emailStr.Length > 0)
                {
                    emailinf.UserName = emailStr[0];
                    emailinf.PassWord = emailStr[1];

                    //接收邮件服务器
                    emailinf.From = emailStr[2];

                    //发送邮件服务器
                    emailinf.Host = emailStr[4];

                    //端口
                    emailinf.IPort = Convert.ToInt32(emailStr[3]);
                }
                new RabbitMQHelper(Rabbit_serverAddress, Rabbit_userName, Rabbit_password, Rabbit_SendEmailHost,
                                   email_queue).SendMessage(emailinf);
            }
            catch { }
        }


        /// <summary>
        ///  邮件发送
        /// </summary>
        /// <param name="listEmailAddress">收件人地址和内容</param>
        /// <param name="strTitle">邮件主题</param>
        /// <param name="mailName">邮件名称（考试、培训等）</param>
        public void SendEmail(List<KeyValuePair<string, string>> listEmailAndContent, string strTitle, string mailName = "")
        {
            try
            {
                EmailInforMation emailinf = new EmailInforMation();
                emailinf.listEmailAndContent = listEmailAndContent;
                emailinf.SendFlag = 1;
                emailinf.Subject = strTitle;
                if (!string.IsNullOrEmpty(mailName))
                {
                    emailinf.mailName = mailName;
                }
                string[] emailStr =
                    AllSystemConfigs.Where(p => p.ConfigType == 1).FirstOrDefault().ConfigValue.Split(';');
                if (emailStr.Length > 0)
                {
                    emailinf.UserName = emailStr[0];
                    emailinf.PassWord = emailStr[1];
                    //接收邮件服务器
                    emailinf.From = emailStr[2];
                    //发送邮件服务器
                    emailinf.Host = emailStr[4];
                    //端口
                    emailinf.IPort = Convert.ToInt32(emailStr[3]);
                }
                new RabbitMQHelper(Rabbit_serverAddress, Rabbit_userName, Rabbit_password, Rabbit_SendEmailHost,
                                   email_queue).SendMessage(emailinf);
            }
            catch { }
        }

        #endregion



    }
}