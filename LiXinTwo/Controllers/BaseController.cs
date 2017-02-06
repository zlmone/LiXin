using LiXinBLL;
using LiXinBLL.DeptSystemManage;
using LiXinBLL.NewClassManage;
using LiXinBLL.Report_fVedio;
using LiXinBLL.Report_Vedio;
using LiXinBLL.SystemManage;
using LiXinBLL.User;
using LiXinCommon;
using LiXinCommon.Configs;
using LiXinControllers.Filter;
using LixinModels.NewClassManage;
using LiXinModels;
using LiXinModels.CourseManage;
using LiXinModels.NewQueryStatistics;
using LiXinModels.User;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQClient;
using Retech.Cache;
using Retech.Core.Cache;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using LiXinBLL.Report_AllData;
using LiXinBLL.Report_Together;


namespace LiXinControllers
{
    [BaseAccess, HandleError, AjaxSession]
    [ValidateInput(false)]
    public class BaseController : Controller
    {
        protected internal virtual JsonResult Json(object data, JsonRequestBehavior behavior)
        {
            return new ConfigurableJsonResult { Data = data, JsonRequestBehavior = behavior };
        }

        #region == 知识库管理相关配置 ==
        //<!--上传图片的路径 -->
        public static string ReResourceImg = ConfigurationManager.AppSettings["ReResourceImg"];
        //<!--上传文件的路径 -->
        public static string ReResourceOldFile = ConfigurationManager.AppSettings["ReResourceOldFile"];
        //<!--上传文件转换的路径 -->
        public static string ReResourceConvertFile = ConfigurationManager.AppSettings["ReResourceConvertFile"];

        #region == 文件转换消息队列变量(知识管理中心文件转换使用 add by yxt 2013-07-31) ==

        //RabbitMQ所在的服务器路径 -->
        public static string Resource_serverAddress = ConfigurationManager.AppSettings["Resource_serverAddress"];
        //用户 -->
        public static string Resource_userName = ConfigurationManager.AppSettings["Resource_userName"];
        //密码-->
        public static string Resource_password = ConfigurationManager.AppSettings["Resource_password"];
        //路由 -->
        public static string Resource_exchange = ConfigurationManager.AppSettings["Resource_exchange"];
        //交换模式 -->
        public static string Resource_exchangeType = ConfigurationManager.AppSettings["Resource_exchangeType"];
        //路由关键字 -->
        public static string Resource_routingKey = ConfigurationManager.AppSettings["Resource_routingKey"];
        //队列名称 -->
        public static string Resource_queue = ConfigurationManager.AppSettings["Resource_queue"];
        //虚拟机名 -->
        public static string Resource_virtualHost = ConfigurationManager.AppSettings["Resource_virtualHost"];

        //免修的虚拟机名 -->
        public static string Free_virtualHost = ConfigurationManager.AppSettings["Free_virtualHost"];

        #endregion
        #endregion


        public static string BaseCompanyUrl = "";

        // 部门分所 开课上传资源文件夹
        public static string UFCODepResource = ConfigurationManager.AppSettings["UFCODepResource"];

        //    <!--集中授课上传时 保存的资源文件及附件路径  UFCO 意为 Upload Files Course -->
        public static string UFCOResource = ConfigurationManager.AppSettings["UFCOResource"];
        //     <!--视频课程上传时 保存的SCORM资源文件路径  UFCO 意为 Upload Files Course -->
        public static string UFCOScorm = ConfigurationManager.AppSettings["UFCOScorm"];

        public static string NewCourseFile = ConfigurationManager.AppSettings["NewCourseFile"];

        public static string NewClassroomResource = ConfigurationManager.AppSettings["ClassRoomResource"];
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

        /// <summary>
        /// 部门分所附件
        /// </summary>
        public static string DepTranFileUrl = ConfigurationManager.AppSettings["DepTranFileUrl"].ToString();

        //同步结果发送至邮箱
        public static string SyncResultEmail = ConfigurationManager.AppSettings["SyncResultEmail"].ToString();

        protected ICacheManager cacheManager = new MemoryCacheManager();

        #region 消息队列定义变量

        //RabbitMQ所在的服务器路径 短信 -->
        public static string Rabbit_serverAddress = ConfigurationManager.AppSettings["Rabbit_serverAddress"];
        //RabbitMQ所在的服务器路径 邮件 -->
        public static string Rabbit_EmailAddress = ConfigurationManager.AppSettings["Rabbit_EmailAddress"];
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

        /// <summary>
        /// 获取当前部门及其子部门集合
        /// ids=""时，获取当前人所在部门及其子部门；ids!="时，获取相关部门及其子部门
        /// </summary>
        /// <returns></returns>
        protected List<Sys_Department> GetAllSubDepartments(string ids = "")
        {
            if (ids == "")
            {
                if (CurrentUser.TrainMaster == 0)
                    ids = CurrentUser.DeptId.ToString();
                else
                {
                    return AllDepartments.Where(p => CurrentUser.ManageDeparts.Split(',').ToList().Contains(p.DepartmentId.ToString())).ToList();
                }
            }
            var deptList = new List<Sys_Department>();
            foreach (var id in ids.Split(',').Where(id => id.Trim() != ""))
            {
                GetSubDepartment(Convert.ToInt32(id), deptList);
            }
            return deptList.Distinct().ToList();
        }

        /// <summary>
        /// 获取当前部门及其子部门集合
        /// </summary>
        protected void GetSubDepartment(int deptID, List<Sys_Department> deptList)
        {
            deptList.Add(AllDepartments.FirstOrDefault(p => p.DepartmentId == deptID));
            foreach (var dept in AllDepartments.Where(p => p.ParentDeptId == deptID))
            {
                GetSubDepartment(dept.DepartmentId, deptList);
            }
        }


        public string DeptName = "业务一部,北京分所";

        protected List<Sys_Department> GetDepartments
        {
            get
            {

                ;
                var departments = AllDepartments;
                //AllDepartments  部门表里所有内容

                string[] arr = DeptName.Split(',');
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
        /// <para>33 全年在线测试完成次数 </para>
        /// <para>34 综合评分—总分和奖励</para>
        /// <para>35 综合评分—考勤</para>
        /// <para>36 综合评分—课后评估</para>
        /// <para>38 综合评分—考勤每次扣分多少</para>
        /// <para>39 是否联合学习配置</para>
        /// <para>40 综合评分—评估每次给多少</para>
        /// <para>41 视频转播预程退订次数</para>
        /// <para>42 学时折算分布</para>
        /// <para>43 年度计划与课程关联配置</para>
        /// <para>44 参加课后评估获取奖励学时上限</para>
        /// <para>45 是否需要月度大纲</para>
        /// <para>46 部门/分所考勤上报截止时间</para>
        /// <para>47 部门/分所课程退订次数</para>
        /// <para>48 暂不用</para>
        /// <para>49 年度计划上报时间设定</para>
        /// <para>50 过时课程—学时折算分布</para>
        /// <para>51 过时课程—考勤上报截止时间</para>
        /// <para>54 纳入考核范围的部门	</para>
        /// <para>60 其他形式、免修申请开放时间配置	</para>
        /// <para>61 其他形式、免修审批开放时间配置</para>
        /// <para>62 其他形式考核级别配置	</para>
        /// <para>64 其他形式申请项目--课后评估</para> 
        /// <para>63 免修配置--自动折算</para>
        /// <para>65 其他有组织形式折算学时限制</para>
        /// </summary>
        protected List<Sys_ParamConfig> AllSystemConfigs
        {
            get
            {
                return cacheManager.Get("all_systemconfigs", 1440, () =>
                {
                    return new Sys_ParamConfigBL().GetList();
                });
            }
        }


        /// <summary>
        /// 获取班级信息集合
        /// </summary>
        protected List<New_Class> AllClass
        {
            get
            {
                return cacheManager.Get("all_Class", () =>
                {
                    return new NewClassBL().GetClassList();
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

        protected void RefreshClass()
        {
            cacheManager.Remove("all_Class");
            cacheManager.Get("all_Class", () =>
            {
                return new NewClassBL().GetClassList();
            });
        }





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
        /// <param name="type">0正常发送 1截取逗号后面的（坑爹的日志）</param>
        /// <param name="islog">1 记录日志 0 不记录日志</param>
        public void SendMessage(List<KeyValuePair<string, string>> list, int type = 0, int islog = 1)
        {
            try
            {

                SmsMessage message = new SmsMessage();

                if (list != null && list.Count > 0)
                {
                    var newList = new List<KeyValuePair<string, string>>();
                    if (type == 1)
                    {
                        foreach (var item in list)
                        {
                            var index = item.Value.IndexOf("，");
                            newList.Add(new KeyValuePair<string, string>(item.Key, item.Value.Substring(index + 1, item.Value.Length - index - 1)));
                        }
                    }
                    else
                    {
                        newList = list;
                    }

                    message.list = newList;
                    message.config =
                        (new Sys_ParamConfigBL()).GetSmsConfing(
                            AllSystemConfigs.Where(p => p.ConfigType == 2).FirstOrDefault());
                    if (islog == 1)
                    {
                        LogHelper.WriteLog(list, this.ControllerContext, 5, 1, type);
                    }
                    new RabbitMQHelper(Rabbit_serverAddress, Rabbit_userName, Rabbit_password, Rabbit_fileTrans,
                                       Rabbit_queue).SendMessage(message);


                }
            }
            catch
            {
                if (islog == 1)
                {
                    LogHelper.WriteLog(list, this.ControllerContext, 5, 1, type);
                }
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
        /// <param name="type">0正常发送 1截取逗号后面的（坑爹的日志）</param>
        /// <param name="islog">1 记录日志 0 不记录日志</param>
        public void SendEmail(List<KeyValuePair<string, string>> listEmailAndContent, string strTitle, string mailName = "", int type = 0, int islog = 1)
        {
            try
            {
                // string listName = String.Join(",", listEmailAndContent.Select(p => p.Key));
                EmailInforMation emailinf = new EmailInforMation();
                var newList = new List<KeyValuePair<string, string>>();
                if (type == 1)
                {
                    foreach (var item in listEmailAndContent)
                    {
                        var index = item.Value.IndexOf("，");
                        newList.Add(new KeyValuePair<string, string>(item.Key, item.Value.Substring(index + 1, item.Value.Length - index - 1)));
                    }
                }
                else
                {
                    newList = listEmailAndContent;
                }

                emailinf.listEmailAndContent = newList;
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
                if (islog == 1)
                {
                    LogHelper.WriteLog(listEmailAndContent, this.ControllerContext, 6, 1, type);
                }
                new RabbitMQHelper(Rabbit_EmailAddress, Rabbit_userName, Rabbit_password, Rabbit_SendEmailHost,
                                   email_queue).SendMessage(emailinf);
                //   RabbitMQManager rabbitmq =
                //new RabbitMQManager("host=192.168.100.180:5672;virtualHost=newsmtp_mq;username=retech;password=123456");
                //   rabbitmq.Publish(emailinf);

            }
            catch
            {
                if (islog == 1)
                {
                    LogHelper.WriteLog(listEmailAndContent, this.ControllerContext, 6, 0, type);
                }
            }
        }

        #endregion

        public bool SaveCommonFile(HttpFileCollectionBase postedFile, string filepath, string saveName)
        {
            bool result = false;
            if (!Directory.Exists(HttpContext.Server.MapPath(filepath)))
            {
                Directory.CreateDirectory(filepath);
            }
            try
            {
                string a = HttpContext.Server.MapPath(filepath);
                postedFile[0].SaveAs(a + "\\" + saveName);
                result = true;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
            return result;
        }


        /// <summary>
        /// 返回List
        /// </summary>
        /// <returns></returns>
        public List<ShowScore> GetShowScoreList(string realname = "", string numberID = "", int Year = -1)
        {
            Year = Year == -1 ? DateTime.Now.Year : Year;
            //将配置中的分数取出来
            var course = AllSystemConfigs.Find(p => p.ConfigType == 34).ConfigValue.Split(';');

            //考勤范围扣分
            var Attend = AllSystemConfigs.Find(p => p.ConfigType == 35);
            //考勤累加扣分
            var AttendSingle = AllSystemConfigs.Find(p => p.ConfigType == 38);

            //评估范围奖励
            var Survey = AllSystemConfigs.Find(p => p.ConfigType == 36);

            //评估累加奖励
            var SurveySingle = AllSystemConfigs.Find(p => p.ConfigType == 40);

            var statistics = new string[4] { "0", "0", "0", "0" };
            if (course[1].Count() > 0)
            {
                statistics = course[1].Split(',');
            }

            //评分（集中+分组）
            var ScoreList = new UserStatisticsBL().GetScoreList(statistics[1].StringToInt32(), statistics[2].StringToInt32(), StartTime: Year + "-01-01");

            //考试
            var exam = course[2] == "" ? new string[3] { "0", "0", "0" } : course[2].Split(',');
            var examList = new UserStatisticsBL().GetExamCourseScore(exam[0].ToString().StringToInt32Zero(), (exam.Length >= 2 ? exam[1].ToString().StringToInt32Zero() : 0), (exam.Length >= 3 ? exam[2].ToString().StringToInt32Zero() : 0), statistics[3].StringToInt32());

            //奖励
            var surveyList = new UserStatisticsBL().GetSurveyList(Survey.ConfigValue, SurveySingle.ConfigValue, course[0].Split(',')[1].StringToInt32(), Survey.userType);

            //考勤
            var AttendceList = new UserStatisticsBL().GetAttendceList(Attend.ConfigValue, AttendSingle.ConfigValue, Attend.userType, statistics[0].StringToInt32());

            int totalcount = 0;
            var where = " 1=1";
            if (!string.IsNullOrEmpty(realname))
            {
                where += string.Format(" and Realname like '%{0}%'", realname.ReplaceSql());
            }
            if (!string.IsNullOrEmpty(numberID))
            {
                where += string.Format(" and LOWER(NumberID) like '%{0}%'", numberID.ReplaceSql().ToLower());
            }
            where += string.Format(" and NewYear={0}", Year);
            var userList = new UserStatisticsBL().GetAllStudyUser(where);

            var ShowList = new List<ShowScore>();
            foreach (var item in userList)
            {
                var user = new ShowScore();

                var singleScore = ScoreList.Find(p => p.UserID == item.UserId);
                var singleExam = examList.Find(p => p.UserID == item.UserId);
                var singleSurvey = surveyList.Find(p => p.UserID == item.UserId);
                var singleAttend = AttendceList.Find(p => p.UserID == item.UserId);

                user.UserID = item.UserId;
                user.NumberID = item.NumberID;
                user.Realname = item.Realname;
                user.InternDeptStr = item.InternDeptStr;
                user.DeptName = item.DeptName;
                user.SAttendScore = singleAttend == null ? 0 : singleAttend.SAttendScore;
                user.StogetherSumScore = singleScore == null ? 0 : singleScore.StogetherSumScore;
                user.SgroupSumScore = singleScore == null ? 0 : singleScore.SgroupSumScore;
                user.SExamScore = singleExam == null ? 0 : singleExam.SExamScore;
                user.cSumScore = singleExam == null ? 0 : singleExam.cSumScore;
                user.vSumScore = singleExam == null ? 0 : singleExam.vSumScore;
                user.eSumScore = singleExam == null ? 0 : singleExam.eSumScore;

                user.SRewardScore = singleSurvey == null ? 0 : singleSurvey.SRewardScore;
                user.SSumScore = user.SAttendScore + user.SgroupSumScore + user.StogetherSumScore + user.SExamScore + user.SRewardScore;
                user.SSumScore = Math.Round(user.SSumScore, 2, MidpointRounding.AwayFromZero);
                ShowList.Add(user);
            }
            var num = 0;
            var sum = 0;
            ShowList = ShowList.OrderByDescending(p => p.SSumScore).ToList();
            //   ShowList.ForEach(p => p.number = num++);
            double sSumScore = 0;
            ShowList.ForEach(p =>
            {
                var flag = false;
                sum++;
                if (num == 0 || (num > 0 && !p.SSumScore.Equals(sSumScore)))
                {
                    flag = true;
                    num = sum;
                }

                sSumScore = p.SSumScore;
                p.number = flag ? sum : num;

            });
            return ShowList;
        }

        /// <summary>                  
        /// 获取中文首字母
        /// <param name="chineseStr">中文字符串</param>
        /// <returns>首字母</returns>
        public string GB2Spell(string chineseStr)
        {
            var capstr = string.Empty;
            var chinaStr = "";
            for (var i = 0; i <= chineseStr.Length - 1; i++)
            {
                var charStr = chineseStr.Substring(i, 1).ToString(CultureInfo.InvariantCulture);
                var zw = System.Text.Encoding.Default.GetBytes(charStr);
                if (zw.Length == 2)
                {
                    int i1 = (short)(zw[0]);
                    int i2 = (short)(zw[1]);
                    long chineseStrInt = i1 * 256 + i2;
                    if ((chineseStrInt >= 45217) && (chineseStrInt <= 45252)) { chinaStr = "a"; }
                    else if ((chineseStrInt >= 45253) && (chineseStrInt <= 45760)) { chinaStr = "b"; }
                    else if ((chineseStrInt >= 45761) && (chineseStrInt <= 46317)) { chinaStr = "c"; }
                    else if ((chineseStrInt >= 46318) && (chineseStrInt <= 46825)) { chinaStr = "d"; }
                    else if ((chineseStrInt >= 46826) && (chineseStrInt <= 47009)) { chinaStr = "e"; }
                    else if ((chineseStrInt >= 47010) && (chineseStrInt <= 47296)) { chinaStr = "f"; }
                    else if ((chineseStrInt >= 47297) && (chineseStrInt <= 47613)) { chinaStr = "g"; }
                    else if ((chineseStrInt >= 47614) && (chineseStrInt <= 48118)) { chinaStr = "h"; }
                    else if ((chineseStrInt >= 48119) && (chineseStrInt <= 49061)) { chinaStr = "j"; }
                    else if ((chineseStrInt >= 49062) && (chineseStrInt <= 49323)) { chinaStr = "k"; }
                    else if ((chineseStrInt >= 49324) && (chineseStrInt <= 49895)) { chinaStr = "l"; }
                    else if ((chineseStrInt >= 49896) && (chineseStrInt <= 50370)) { chinaStr = "m"; }
                    else if ((chineseStrInt >= 50371) && (chineseStrInt <= 50613)) { chinaStr = "n"; }
                    else if ((chineseStrInt >= 50614) && (chineseStrInt <= 50621)) { chinaStr = "o"; }
                    else if ((chineseStrInt >= 50622) && (chineseStrInt <= 50905)) { chinaStr = "p"; }
                    else if ((chineseStrInt >= 50906) && (chineseStrInt <= 51386)) { chinaStr = "q"; }
                    else if ((chineseStrInt >= 51387) && (chineseStrInt <= 51445)) { chinaStr = "r"; }
                    else if ((chineseStrInt >= 51446) && (chineseStrInt <= 52217)) { chinaStr = "s"; }
                    else if ((chineseStrInt >= 52218) && (chineseStrInt <= 52697)) { chinaStr = "t"; }
                    else if ((chineseStrInt >= 52698) && (chineseStrInt <= 52979)) { chinaStr = "w"; }
                    else if ((chineseStrInt >= 52980) && (chineseStrInt <= 53640)) { chinaStr = "x"; }
                    else if ((chineseStrInt >= 53689) && (chineseStrInt <= 54480)) { chinaStr = "y"; }
                    else if ((chineseStrInt >= 54481) && (chineseStrInt <= 55289)) { chinaStr = "z"; }
                }
                else
                    capstr += charStr;

                capstr += chinaStr;

            }
            return capstr.ToUpper();
        }

        /// <summary>
        /// 部门分所获得配置
        /// configtyp=1 培训需求上报时间设定
        /// configtyp=2 课前建议、课后评估的开放时间
        /// configtyp=3 在线测试允许答题时间
        /// configtyp=4 在线测试允许最大次数
        /// configtyp=5 全年在线测试完成次数
        /// configtyp=6 全年课程退订次数
        /// configtyp=7 请假审核时限
        /// configtyp=8 过时课程课后评估的开放时间
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string DepConfig(int id, int type)
        {
            var list = new Sys_ParamConfig();

            list = new ParamConfigBl().GetConfig(id, type);

            if (list != null)
            {
                if (list.ConfigType == 1)
                {
                    var temp = list.ConfigValue.Split(';');
                    int yearbegin = Convert.ToInt32(temp[0]) + 2012;
                    int yearend = Convert.ToInt32(temp[3]) + 2012;
                    string begin = Convert.ToString(yearbegin) + "-" + temp[1] + "-" + temp[2];
                    string end = Convert.ToString(yearend) + "-" + temp[4] + "-" + temp[5];
                    string returnValue = begin + ";" + end;
                    return returnValue;

                }
                return list.ConfigValue;
            }
            return "";

        }

        #region  报表缓存刷新
        /// <summary>
        ///缓存课程应学习的总人数
        /// </summary>
        public void RefreshVedioSumNumber()
        {
            cacheManager.Remove("Vedio_Number");
            cacheManager.Get("Vedio_Number", () => { return (new Report_VedioBL()).GetCacheVedioSumNumber(); });
        }

        /// <summary>
        ///获取视频课程的应该参与人员对应的部门
        /// </summary>
        public void RefreshfVedioJoinNumber()
        {
            cacheManager.Remove("fVedio_JoinNumber");
            cacheManager.Get("fVedio_JoinNumber", () => { return (new Report_VedioBL()).GetfVedioJoinNumber(); });
        }

        /// <summary>
        ///获取视频课程的应该参与人员评分
        /// </summary>
        public void RefreshfVedioSurvey()
        {
            cacheManager.Remove("Vedio_Survey");
            cacheManager.Get("Vedio_Survey", 1440, () => { return (new Report_VedioBL()).GetUserSurvey(); });
        }


        /// <summary>
        ///考核范围内的人
        /// </summary>
        public void RefreshCheckUser()
        {
            cacheManager.Remove("Check_User");
            cacheManager.Get("Check_User", 1440, () => { return (new Report_VedioBL()).GetCheckUserList(); });
        }

        /// <summary>
        ///课程考试缓存
        /// </summary>
        public void RefreshGetExamList()
        {
            cacheManager.Remove("Vedio_Exam");
            cacheManager.Get("Vedio_Exam", 1440, () => { return (new Report_VedioBL()).GetExamList(); });
        }
        /// <summary>
        ///每个部门分所应参加的人数
        /// </summary>
        public void RefreshGetTogetherUser(int year = -1)
        {
            year = year == -1 ? DateTime.Now.Year : year;
            cacheManager.Remove("together_number");
            cacheManager.Get("together_number" + year, 1440, () => { return (new ReportTogetherBL()).GetTogetherUser(year); });
        }

        public void RefreshGetDepCourseUser()
        {
            cacheManager.Remove("DepCourse_number");
            cacheManager.Get("DepCourse_number", 1440, () => { return (new Report_DepCourseBL()).GetDepCourseUser(); });
        }

        /// <summary>
        /// 总表报数据处理（去除了不纳入考核范围内的人呢）
        /// </summary>
        public void RefreshGetAllCourseScore(int year = -1)
        {
            Sys_ParamConfig configType14 = AllSystemConfigs.Where(p => p.ConfigType == 14).FirstOrDefault();
            Sys_ParamConfig configType13 = AllSystemConfigs.Where(p => p.ConfigType == 13).FirstOrDefault();
            Sys_ParamConfig configType16 = AllSystemConfigs.Where(p => p.ConfigType == 16).FirstOrDefault();
            Sys_ParamConfig configType27 = AllSystemConfigs.Where(p => p.ConfigType == 27).FirstOrDefault();
            int yearplan = year == -1 ? DateTime.Now.Year : year;

            cacheManager.Remove("courseReport" + yearplan);

            cacheManager.Get(("courseReport" + yearplan), 1440, () =>
            {
                return new Report_AllData().GetAllCourseReport(configType14, configType13, configType16, configType27, yearplan);
            });

        }
        /// <summary>
        /// 集中授课个部门参与考试的人数
        /// </summary>
        /// <returns></returns>
        public void RefreshGetCourseNumber()
        {
            cacheManager.Remove("together_examnumber");
            cacheManager.Get("together_examnumber", 1440, () => { return (new ReportTogetherBL()).GetCourseNumber(); });
        }

        /// <summary>
        /// 缓存 课程的基本信息 实际参与人数人全部的信息
        /// </summary>
        /// <returns></returns>
        public void RefreshGetCacheCourseInfoList(string freeWhere = " 1=1")
        {
            cacheManager.Remove("together_CourseInfoList");
            cacheManager.Get("together_CourseInfoList", 1440, () => { return (new ReportTogetherBL()).GetCacheCourseInfoList(freeWhere); });
        }
        #endregion

        /// <summary>
        /// 获取当前人员负责的部门或者他为其他人的上级或者他本部门
        /// </summary>
        /// <returns></returns>
        protected List<int> GetAllSubLeardDepartments()
        {
            var ids = CurrentUser.DeptId;

            if (CurrentUser.TrainMaster == 0)
            {
                var listID = (new UserBL()).GetLeardForUserID(CurrentUser.UserId);
                if (listID.Count > 0)
                {
                    return listID;
                }
                else
                {
                    return new List<int>() { CurrentUser.DeptId };
                }

            }
            else
            {
                return AllDepartments.Where(p => CurrentUser.ManageDeparts.Split(',').ToList().Contains(p.DepartmentId.ToString())).Select(p => p.DepartmentId).ToList();
            }

        }


        #region 免修公共方法
        /// <summary>
        /// 当前时间是否在时间之内
        /// </summary>
        /// <param name="ConfigType">60 其他形式、免修申请开放时间配置 61 其他形式、免修审批开放时间配置</param>
        /// <param name="type">0 其他形式 1 免修 2其他有组织形式</param>
        /// <returns>1 在 0 不在</returns>
        public int FreeIsTimeIn(int ConfigType, int type)
        {
            var model = AllSystemConfigs.Where(p => p.ConfigType == ConfigType).FirstOrDefault();
            var flag = 0;
            var now = DateTime.Now.ToString("g");
            var startTime = (model != null && model.ConfigValue != "") ? model.ConfigValue.Split(';')[type].Split(',')[0] : "";
            var endTime = (model != null && model.ConfigValue != "") ? model.ConfigValue.Split(';')[type].Split(',')[1] : "";
            if (startTime == "")//为空没有配置，则无法进行申请
            {
                flag = 1;
            }
            else
            {

                if (now.StringToDate(2) >= startTime.StringToDate(0) && now.StringToDate(2) <= endTime.StringToDate(0))
                {
                    flag = 1;
                }
            }
            return flag;
        }

        /// <summary>
        /// 当前级别是否在考核级别之内
        /// </summary>
        /// <param name="type">0 其他形式 1 免修 2其他有组织形式</param>
        /// <returns>1 在 0 不在</returns>
        public int FreeIsInTrainGrade(int type)
        {
            var flag = 0;

            var model = AllSystemConfigs.Where(p => p.ConfigType == 62).FirstOrDefault();

            if (model != null && model.ConfigValue != "")
            {
                var TrainGrade = model.ConfigValue.Split(';')[type].Split(',').ToList();
                if (TrainGrade.Contains(CurrentUser.TrainGrade))
                {
                    flag = 1;
                }
            }
            else
            {
                flag = 1;
            }

            return flag;
        }
        #endregion

    }
}
