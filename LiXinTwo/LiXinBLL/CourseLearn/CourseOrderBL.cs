using System.Collections.Generic;
using LiXinCommon;
using LiXinDataAccess;
using LiXinDataAccess.Examination;
using LiXinInterface.CourseLearn;
using LiXinModels.CourseLearn;
using System;
using System.Linq;
using LiXinDataAccess.CourseManage;
using LiXinDataAccess.CourseLearn;
using System.Diagnostics;
using LiXinModels.CourseManage;
using LiXinModels.Examination.DBModel;
using LiXinModels.User;
using LiXinBLL.DepTranManage;
using LiXinDataAccess.DepTranManage;
using System.Text.RegularExpressions;
using LiXinModels;
using LiXinDataAccess.User;
using LiXinDataAccess.DepCourseManage;
using LiXinModels.Report_zVedio;

namespace LiXinBLL.CourseLearn
{
    public class CourseOrderBL : ICourseOrder
    {
        private readonly UserDB _userDB = new UserDB();
        private static DepMyScoreDB scoreDB = new DepMyScoreDB();
        private readonly CourseOrderDB _courseOrderDB = new CourseOrderDB();
        private readonly Co_CourseDB _courseDB = new Co_CourseDB();
        private readonly Cl_CpaLearnStatusDB _cpaLearnStatusDB = new Cl_CpaLearnStatusDB();

        private readonly ExaminationDB _ExamDB = new ExaminationDB();
        private readonly ExampaperDB _paperDB = new ExampaperDB();

        private readonly DepTran_CourseOrderDB _DepTran_CourseOrderBL = new DepTran_CourseOrderDB();



        public void Add(Cl_CourseOrder model)
        {
            var tmp = _courseOrderDB.GetList(string.Format("Cl_CourseOrder.UserId = {0} and Cl_CourseOrder.CourseId = {1}", model.UserId, model.CourseId)).FirstOrDefault();
            if (tmp == null)
                _courseOrderDB.Add(model);
            else
            {
                tmp.PassStatus = 2;
                tmp.OrderStatus = model.OrderStatus;
                tmp.LeaveTime = model.LeaveTime;
                tmp.OrderTime = model.OrderTime;
                tmp.OrderEndTime = model.OrderEndTime;
                tmp.IsAppoint = model.IsAppoint;
                tmp.IsLeave = 0;
                tmp.Reson = "";
                tmp.ApprovalFlag = 0;
                tmp.ApprovalMemo = "";
                tmp.ApprovalUser = "";
                tmp.CourseName = model.CourseName;
                tmp.CourseStartTime = model.CourseStartTime;
                tmp.CourseEndTime = model.CourseEndTime;
                tmp.FtriggerFlag = model.FtriggerFlag;
                tmp.DropType = 0;
                tmp.DropReason = "";
                _courseOrderDB.Update(tmp);
            }
        }

        public bool Update(Cl_CourseOrder model)
        {
            return _courseOrderDB.Update(model);
        }

        public bool Delete(int id)
        {
            return _courseOrderDB.Delete(id);
        }

        public Cl_CourseOrder Get(int Id)
        {
            return _courseOrderDB.Get(Id);
        }

        public Cl_TimeOutOrder GetOneTimeOutOrder(int id)
        {
            return _courseOrderDB.GetOneTimeOutOrder(id);
        }

        public void UpdateTimeOutOrderStatus(Cl_TimeOutOrder model)
        {
            _courseOrderDB.UpdateTimeOutOrderStatus(model);
        }

        public List<Cl_CourseOrder> GetList(string strWhere = " 1 = 1 ")
        {
            return _courseOrderDB.GetList(strWhere);
        }

        public List<Cl_CourseOrder> GetList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = "ORDER BY id DESC")
        {
            return _courseOrderDB.GetList(out totalCount, where, startIndex, pageLength, orderBy);
        }

        public bool UpdateLeave(int id, string memo, string leaderID, double timespan, string hrId, string name)
        {
            return _courseOrderDB.UpdateLeave(id, memo, leaderID, timespan, hrId, name);
        }

        public int UpdateStatus(int id, int userId, DateTime start, DateTime end, int unsubscribeCount)
        {
            return _courseOrderDB.UpdateStatus(id, userId, start, end, unsubscribeCount);
        }

        public int UpdateStatus(out int num, int id, int userId, int year, int unsubscribeCount)
        {
            return _courseOrderDB.UpdateStatus(out num, id, userId, year, unsubscribeCount);
        }

        /// <summary>
        /// 0:不可报名
        /// 1:可直接报名
        /// 2:可报名，需排队 
        /// 3:报名失败
        /// 4:不可报名，排队已关闭
        /// 5:不可报名，报名已关闭
        /// </summary>
        public int GetCanSignup(out int num, int courseId, int userId, double timespan)
        {
            var flag = _courseOrderDB.GetCanSignup(out num, courseId, timespan);
            if (flag == 1)
            {
                try
                {
                    var course = _courseDB.GetCo_Course(courseId);
                    Add(new Cl_CourseOrder
                    {
                        CourseId = courseId,
                        UserId = userId,
                        OrderTime = DateTime.Now,
                        OrderStatus = 1,
                        OrderEndTime = course.StartTime.AddHours(timespan * -1),
                        IsAppoint = 0,
                        CourseStartTime = course.StartTime,
                        CourseEndTime = course.EndTime,
                        CourseName = course.CourseName,
                        PassStatus = 2,
                        FtriggerFlag = 0,
                        DropType = 0,
                        DropReason = ""
                    });
                    return 1;
                }
                catch
                {
                    return 3;
                }
            }
            return flag;
        }

        /// <summary>
        /// 课程【特殊人群/指定】报名
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="courseStartTime">课程开始时间</param>
        /// <param name="courseEndTime">课程结束时间</param>
        /// <param name="isAppoint">1：部门指定；2：总所指定</param>
        /// <param name="userIds">学员Id</param>
        /// <param name="timespan">排队状态更改为正常预定状态的时间点</param>
        public void AddSpecialCrowdUserToCourse(int courseId, string courseName, DateTime courseStartTime, DateTime courseEndTime, int isAppoint, string userIds, double timespan)
        {
            var ids = userIds.Split(',');
            int num = 0;
            int flag = _courseOrderDB.GetCanSignupSpecialCrowdUserToCourse(out num, courseId, timespan);
            for (int i = 0; i < ids.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(ids[i]))
                    continue;
                try
                {
                    if (flag == 1 || flag == 2)
                    {
                        Add(new Cl_CourseOrder
                        {
                            CourseId = courseId,
                            UserId = Convert.ToInt32(ids[i]),
                            OrderTime = DateTime.Now,
                            OrderStatus = flag,
                            OrderEndTime = courseStartTime.AddHours(timespan * -1),
                            IsAppoint = isAppoint,
                            CourseStartTime = courseStartTime,
                            CourseEndTime = courseEndTime,
                            CourseName = courseName,
                            PassStatus = 2,
                            FtriggerFlag = 0
                        });
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 0:不可报名
        /// 1:可直接报名
        /// 2:不可报名，人数已满
        /// 3:报名失败
        /// </summary>
        public int GetCanSignupCPA(int courseId, int userId)
        {
            var flag = _courseOrderDB.GetCanSignupCPA(courseId);
            if (flag == 1)
            {
                try
                {
                    _cpaLearnStatusDB.SubscribeCPA(new Cl_CpaLearnStatus
                    {
                        CourseID = courseId,
                        UserID = userId
                    });
                    return 1;
                }
                catch
                {
                    return 3;
                }
            }
            return flag;
        }

        public int SubmitTimeOutApply(string ids, string applymemo)
        {
            return _courseOrderDB.SubmitTimeOutApply(ids, applymemo);
        }

        public int SubmitTimeOutApproval(string ids, string approvalmemo, int approval)
        {
            return _courseOrderDB.SubmitTimeOutApproval(ids, approvalmemo, approval);
        }

        public Cl_CourseOrder GetTimeOutApplyDetail(int id)
        {
            return _courseOrderDB.GetTimeOutApplyDetail(id);
        }

        public List<Cl_CourseOrder> GetMyLeaveList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                       int pageLength = int.MaxValue, string orderBy = "ORDER BY Cl_CourseOrder.id DESC")
        {
            return _courseOrderDB.GetMyLeaveList(out totalCount, where, startIndex, pageLength, orderBy);
        }

        public List<Cl_CourseOrder> GetMyComplementResList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Cl_MakeUpOrder.id DESC")
        {
            return _courseOrderDB.GetMyComplementResList(out totalCount, where, startIndex, pageLength, orderBy);
        }

        public List<Cl_CourseOrder> GetMyTimeOutList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Cl_TimeOutOrder.id DESC")
        {
            return _courseOrderDB.GetMyTimeOutList(out totalCount, where, startIndex, pageLength, orderBy);
        }
        public List<Cl_CourseOrder> GetMyTimeOutListLeave(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                     int pageLength = int.MaxValue, string orderBy = "ORDER BY Cl_TimeOutOrder.id DESC")
        {
            return _courseOrderDB.GetMyTimeOutListLeave(out totalCount, where, startIndex, pageLength, orderBy);
        }


        public List<Cl_CourseOrder> GetTimeOutApplyApprovalList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Cl_TimeOutOrder.id DESC")
        {
            return _courseOrderDB.GetTimeOutApplyApprovalList(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 查询CPA课程中学员的预订情况
        /// </summary>
        /// <returns></returns>
        public List<LiXinModels.User.Sys_User> GetCPACourseSubscribeUserList(out int totalCount, int courseId,
                                                                          string where = " 1 = 1 ", int startIndex = 0,
                                                                          int pageLength = int.MaxValue,
                                                                          string orderBy =
                                                                              "ORDER BY su.userId DESC")
        {
            return _courseOrderDB.GetCPACourseSubscribeUserList(out totalCount, courseId, where, startIndex, pageLength,
                                                                orderBy);
        }

        /// <summary>
        /// 逾时审核申请
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetTimeOutApplyList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Cl_MakeUpOrder.id DESC")
        {
            return _courseOrderDB.GetTimeOutApplyList(out totalCount, where, startIndex, pageLength, orderBy);
        }

        public List<Cl_CourseOrder> GetTimeOutApprovalList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Cl_MakeUpOrder.id DESC")
        {
            return _courseOrderDB.GetTimeOutApprovalList(out totalCount, where, startIndex, pageLength, orderBy);
        }



        public List<LiXinModels.CourseManage.Co_Course> GetMyCourseList(out int totalCount, int userId, string where = " 1 = 1 ", string num = " 1=1", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.id DESC")
        {
            return _courseOrderDB.GetMyCourseList(out totalCount, userId, where, num, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 我的可预订注协课程
        /// </summary>
        /// <returns></returns>
        public List<LiXinModels.CourseManage.Co_Course> GetMyCourseCPAList(out int totalCount, int userId, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " id DESC")
        {
            var list = _courseOrderDB.GetMyCourseCPAList(userId, where, startIndex, pageLength, orderBy);
            totalCount = list.Count() == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }

        public List<LiXinModels.CourseManage.Co_Course> GetCourseSubscribeList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.id DESC")
        {
            return _courseOrderDB.GetCourseSubscribeList(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 查询CPA课程的报名状态
        /// </summary>
        /// <returns></returns>
        public List<LiXinModels.CourseManage.Co_Course> GetCpaCourseSubscribeList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.id DESC")
        {
            return _courseOrderDB.GetCpaCourseSubscribeList(out totalCount, where, startIndex, pageLength, orderBy);
        }

        public List<LiXinModels.CourseManage.Co_Course> GetCourseDeptSubscribeList(out int totalCount, string leaderId, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.id DESC")
        {
            return _courseOrderDB.GetCourseDeptSubscribeList(out totalCount, leaderId, where, startIndex, pageLength, orderBy);
        }

        public bool UpdateDucueFlag(int id, int status)
        {
            return _courseOrderDB.UpdateDucueFlag(id, status);
        }

        public bool UpdateOrderFlag(int id, int status)
        {
            return _courseOrderDB.UpdateOrderFlag(id, status);
        }

        public LiXinModels.CourseManage.Co_Course GetCourseById(int courseId, int userId = 0)
        {
            return _courseOrderDB.GetCourseById(courseId, userId);
        }

        public List<LiXinModels.User.Sys_Department> GetDeptSubscribe(int courseId)
        {
            return _courseOrderDB.GetDeptSubscribe(courseId);
        }

        public List<NameSubscribeCount> GetTrainGradeSubscribe(int courseId)
        {
            return _courseOrderDB.GetTrainGradeSubscribe(courseId);
        }

        public List<NameSubscribeCount> GetSuccessTrainGradeSubscribe(int courseId)
        {
            return _courseOrderDB.GetSuccessTrainGradeSubscribe(courseId);
        }
        /// <summary>
        /// CPA课程报名培训级别明细
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<NameSubscribeCount> GetCpaTrainGradeSubscribe(int courseId)
        {
            return _courseOrderDB.GetCpaTrainGradeSubscribe(courseId);
        }

        /// <summary>
        /// 查询培训级别下CPA课程的报名情况
        /// </summary>
        /// <param name="courseID">课程ID</param>
        /// <returns></returns>
        public List<NameSubscribeCount> GetTrainGradeCPASubscribe(int courseID)
        {
            return _courseOrderDB.GetTrainGradeCPASubscribe(courseID);
        }

        public List<LiXinModels.CourseManage.Co_Course> GetMyCourseListCourse(out int totalCount, int userId,
                                                                               string where = " 1 = 1 ",
                                                                               int startIndex = 0,
                                                                               int pageLength = int.MaxValue,
                                                                               string orderBy =
                                                                                   "ORDER BY Co_Course.id DESC")
        {
            return _courseOrderDB.GetMyCourseListCourse(out totalCount, userId, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 我的课程表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<LiXinModels.CourseManage.Co_Course> GetMyCourseShedule(int userId, string where = " 1 = 1 ", string orderBy = " ORDER BY Co_Course.id DESC")
        {
            return _courseOrderDB.GetMyCourseShedule(userId, where, orderBy);
        }

        public List<LiXinModels.User.Sys_User> GetCourseSubscribeUserList(out int totalCount, int courseId, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = "ORDER BY Sys_User.userId DESC")
        {
            return _courseOrderDB.GetCourseSubscribeUserList(out totalCount, courseId, where, startIndex, pageLength, orderBy);
        }


        public List<LiXinModels.CourseManage.Co_Course> GetListByTeacher(out int totalCount, int teacher,
                                                                         string where = " 1 = 1 ",
                                                                         int startIndex = 0,
                                                                         int pageLength = int.MaxValue,
                                                                         string orderBy = "ORDER BY Co_Course.id DESC")
        {
            return _courseOrderDB.GetListByTeacher(out totalCount, teacher, where, startIndex, pageLength, orderBy);
        }


        public List<LiXinModels.CourseManage.Co_Course> GetListByAllTeacher(out int totalCount,
                                                                        string where = " 1 = 1 ", int startIndex = 0,
                                                                        int pageLength = int.MaxValue,
                                                                        string orderBy = "ORDER BY Co_Course.id DESC")
        {
            return _courseOrderDB.GetListByAllTeacher(out totalCount, where, startIndex, pageLength, orderBy);
        }


        public List<Cl_CourseOrder> GetTeacherCourse(out int totalCount, int CourseId, int startIndex = 0, int pageLength = int.MaxValue)
        {
            return _courseOrderDB.GetTeacherCourse(out totalCount, CourseId, startIndex, pageLength);
        }


        public List<Cl_CourseOrder> GetTeacherCourseUserList(out int totalCount, int CourseId, int startIndex = 0, int pageLength = int.MaxValue)
        {
            return _courseOrderDB.GetTeacherCourseUserList(out totalCount, CourseId, startIndex, pageLength);
        }

        /// <summary>
        /// 我的课程讲师端学员端列表(加入考试成绩)
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="CourseId"></param>
        /// <param name="TeacherId"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetTeacherOnLineTest(out int totalCount, int CourseId, int TeacherId, int startIndex = 0,
                                                                 int pageLength = int.MaxValue)
        {
            List<Cl_CourseOrder> clarr = new List<Cl_CourseOrder>();

            List<Cl_CourseOrder> cllist = new List<Cl_CourseOrder>();

            //return _courseOrderDB.GetTeacherOnLineTest(out totalCount, CourseId, TeacherId, startIndex, pageLength);

            var list = _courseOrderDB.GetTeacherOnLineTest(out totalCount, CourseId, TeacherId, startIndex, int.MaxValue);





            //找出课程转播的学员 加到一起 在一期我的授课中显示
            var _DepTran_CourseOrderList = _DepTran_CourseOrderBL.GetTeacherMyCourseOrderList(TeacherId, CourseId);



            for (int i = 0; i < list.Count; i++)
            {
                tbExamSendStudent exam = null;

                exam = _ExamDB.GetExamSendStudentBySQL2008(list[i].CourseId.Value, list[i].UserId, list[i].CoPaperID, list[i].PaperId, 1);

                var exampaper = _paperDB.GetExampaper(list[i].PaperId);
                list[i].LevelScoreStr = Convert.ToInt32(list[i].LevelScore * 0.01 * (exampaper == null ? 0 : exampaper.ExampaperScore));

                if (exam != null)
                {
                    list[i].tbExamstudentId = exam._id;
                    list[i].DoExamStatus = exam.DoExamStatus;
                    //list[i].GetexamScore = (exam.StudentAnswerList.Sum(pa => pa.GetScore) / exampaper.ExampaperScore) * 100;
                    list[i].GetexamScore = exam.StudentAnswerList.Sum(pa => pa.GetScore);
                    list[i].ExamTestTimes = exam.TestTimes;  //记录考试次数,考了几次

                }
                else
                {
                    list[i].GetexamScore = 0;
                    list[i].tbExamstudentId = 0;

                }
                clarr.Add(list[i]);
            }



            if (_DepTran_CourseOrderList != null)
            {
                foreach (var item in _DepTran_CourseOrderList)
                {
                    Cl_CourseOrder cl = new Cl_CourseOrder();
                    cl.CourseId = item.CourseId;
                    cl.UserId = item.UserId;
                    cl.realname = item.Realname;
                    cl.DeptName = item.DeptName;
                    cl.JobNum = item.JobNum;
                    cl.PaperId = item.PaperId;
                    cl.TestTimes = item.TestTimes;
                    cl.LevelScore = item.LevelScore;
                    cl.CoPaperID = item.CoPaperID;
                    cllist.Add(cl);
                }
            }



            for (int i = 0; i < cllist.Count; i++)
            {
                tbExamSendStudent exam = null;

                exam = _ExamDB.GetExamSendStudentBySQL2008(cllist[i].CourseId.Value, cllist[i].UserId, cllist[i].CoPaperID, cllist[i].PaperId, 4);

                var exampaper = _paperDB.GetExampaper(cllist[i].PaperId);
                cllist[i].LevelScoreStr = Convert.ToInt32(cllist[i].LevelScore * 0.01 * (exampaper == null ? 0 : exampaper.ExampaperScore));

                if (exam != null)
                {
                    cllist[i].tbExamstudentId = exam._id;
                    cllist[i].DoExamStatus = exam.DoExamStatus;
                    //list[i].GetexamScore = (exam.StudentAnswerList.Sum(pa => pa.GetScore) / exampaper.ExampaperScore) * 100;
                    cllist[i].GetexamScore = exam.StudentAnswerList.Sum(pa => pa.GetScore);
                    cllist[i].ExamTestTimes = exam.TestTimes;  //记录考试次数,考了几次

                }
                else
                {
                    cllist[i].GetexamScore = 0;
                    cllist[i].tbExamstudentId = 0;

                }
                clarr.Add(cllist[i]);
            }

            totalCount = clarr.Count();

            return clarr;
        }

        public List<Cl_CourseOrder> GetTeacherCourseAttendceList(out int totalCount, int CourseId, int startIndex = 0,
                                                                 int pageLength = int.MaxValue)
        {
            return _courseOrderDB.GetTeacherCourseAttendceList(out totalCount, CourseId, startIndex, pageLength);
        }

        public bool UpdateGetScore(int courseId, int UserId, decimal GetScore, int PassStatus, int LearnStatus)
        {
            return _courseOrderDB.UpdateGetScore(courseId, UserId, GetScore, PassStatus, LearnStatus);
        }

        public bool UpdateLearnStatus(int courseId, int UserId, int LearnStatus = 0)
        {
            return _courseOrderDB.UpdateLearnStatus(courseId, UserId, LearnStatus);
        }

        public bool UpdatePassStatus(int courseid, int userid, int PassStatus)
        {
            return _courseOrderDB.UpdatePassStatus(courseid, userid, PassStatus);
        }




        public List<Cl_CourseOrder> UpdateCourseOrderStatus(DateTime lastTime)
        {
            var result = new List<Cl_CourseOrder>();
            string where = string.Format(@" Co_course.IsDelete = 0 and Co_course.Publishflag = 1 
                and Co_course.Way = 1 and Co_course.CourseFrom = 2
            --((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
	        --    or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
            --    or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
            --        and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
	        --    or Cl_CourseOrder.orderstatus = 2 or Cl_CourseOrder.orderstatus = 3)
            and Co_course.starttime > '{0}'", lastTime);
            string orderby = "order by Cl_CourseOrder.courseid asc,Cl_CourseOrder.orderstatus asc,Cl_CourseOrder.ordertime asc";

            var list = _courseOrderDB.GetUpdateCourseOrderStatus(lastTime, where, orderby);
            if (list.Count > 0)
            {
                int courseId = 0;
                int numberLimitedTmp = 0;
                int userCount = 0;
                string patchIds = "";
                int AvailableInLineStatus = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    if (courseId != list[i].CourseId)
                    {
                        if (!string.IsNullOrEmpty(patchIds) && AvailableInLineStatus == 0)
                        {
                            _courseOrderDB.UpdateOrderStatus(patchIds.TrimEnd(','));
                        }
                        courseId = list[i].CourseId.Value;
                        numberLimitedTmp = list[i].NumberLimited;
                        userCount = 0;
                        patchIds = "";
                        AvailableInLineStatus = 0;
                    }
                    if (list[i].OrderStatus == 3)
                        AvailableInLineStatus = 1;
                    if (list[i].OrderStatus == 0 && list[i].ApprovalDateTime > list[i].OrderEndTime)
                        userCount++;
                    if (list[i].OrderStatus == 1)
                    {
                        if (list[i].IsLeave == 0)
                        {
                            userCount++;
                        }
                        else
                        {
                            if (list[i].ApprovalFlag == 1)
                            {
                                if (list[i].ApprovalDateTime < list[i].ApprovalLimitTime)
                                {
                                    if (list[i].ApprovalDateTime > list[i].OrderEndTime)
                                        userCount++;
                                }
                                else
                                    userCount++;
                            }
                            else
                                userCount++;
                        }
                    }
                    if (list[i].OrderStatus == 2 && userCount < numberLimitedTmp)
                    {
                        if (list[i].OrderTime <= list[i].OrderEndTime)
                        {
                            patchIds += list[i].Id + ",";
                            userCount++;
                            result.Add(list[i]);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(patchIds))
                {
                    _courseOrderDB.UpdateOrderStatus(patchIds.TrimEnd(','));
                }
            }

            return result;
        }

        public List<Cl_CourseOrder> UpdateCourseOrderStatusByCourseId(int id)
        {
            var result = new List<Cl_CourseOrder>();
            var list = _courseOrderDB.GetUpdateCourseOrderStatusByCourseId(id);
            int numberLimitedTmp = 0;
            int userCount = 0;
            string patchIds = "";
            int AvailableInLineStatus = 0;
            if (list.Count > 0)
                numberLimitedTmp = list[0].NumberLimited;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].OrderStatus == 3)
                {
                    AvailableInLineStatus = 1;
                    break;
                }
                if (list[i].OrderStatus == 0 && list[i].ApprovalDateTime > list[i].OrderEndTime)
                    userCount++;
                if (list[i].OrderStatus == 1)
                {
                    if (list[i].IsLeave == 0)
                        userCount++;
                    else
                    {
                        if (list[i].ApprovalFlag == 1)
                        {
                            if (list[i].ApprovalDateTime < list[i].ApprovalLimitTime)
                            {
                                if (list[i].ApprovalDateTime > list[i].OrderEndTime)
                                    userCount++;
                            }
                            else
                                userCount++;
                        }
                        else
                            userCount++;
                    }
                }

                if (list[i].OrderStatus == 2 && userCount < numberLimitedTmp)
                {
                    if (list[i].OrderTime <= list[i].OrderEndTime)
                    {
                        patchIds += list[i].Id + ",";
                        userCount++;
                        result.Add(list[i]);
                    }
                }
            }
            if (!string.IsNullOrEmpty(patchIds) && AvailableInLineStatus == 0)
            {
                _courseOrderDB.UpdateOrderStatus(patchIds.TrimEnd(','));
            }
            return result;
        }

        /// <summary>
        /// 撤销部门指定人员
        /// </summary>
        /// <param name="sqlstr"></param>
        public void DropAssignUser(string sqlstr)
        {
            _courseOrderDB.DropAssignUser(sqlstr);
        }

        /// <summary>
        /// 获取集中授课中报名的人员
        /// </summary>
        public List<Sys_User> GetCourseOrderName(out int totalCount, int courseID, string where = "1=1", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " su.UserId asc")
        {
            var list = _courseOrderDB.GetCourseOrderName(courseID, where, pageSize, pageIndex,jsRenderSortField);
            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }


        public List<Cl_CourseOrder> GetCourseLengthByUserId(int userid)
        {
            return _courseOrderDB.GetCourseLengthByUserId(userid);
        }

        public List<Cl_CourseOrder> GetDepCourseLengthByUserId(int userid)
        {
            return _courseOrderDB.GetDepCourseLengthByUserId(userid);
        }
        #region == 部门/分所视频同步转播-我的课程-转播课程 ==
        /// <summary>
        /// 获得集中课程视频转播列表
        /// </summary>
        /// <param name="totalCount">总记录数</param>
        /// <param name="userId">当前用户ID</param>
        /// <param name="teacherName">讲师</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="isMust">是否必修:0-必修 1-选修</param>
        /// <param name="learnStatus">课程状态0-未开始，1-进行中,2-已结束</param>
        /// <param name="sort">所有课程类型1-内部培训 2-社会招聘 3-新进员工 4-实习生</param>
        /// <param name="subscribeType">预订状态 0-不可预订 1-未预订</param>
        /// <param name="startTime">开课开始时间-begin</param>
        /// <param name="endTime">开课开始时间-end</param>
        /// <param name="where">条件语句</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public List<Co_Course> GetMyBroadcastList(out int totalCount, int userId,
            string teacherName, string courseName, int isMust, int learnStatus, int sort,
                                int subscribeType, string startTime, string endTime,
            string where = "", int startIndex = 1, int pageLength = int.MaxValue,
             string orderBy = "")
        {
            switch (subscribeType)
            {
                case 0://不可预订
                    where +=
                    string.Format(@" AND 
                                   (
                                      cc.Id NOT IN (SELECT dtco.CourseId FROM DepTran_CourseOpen dtco
                                                WHERE dtco.DepartId in (select du.DepartSetId from DepTran_DepartUser du  
join DepTran_DepartLeaderConfig dlc on dlc.Id=du.DepartSetId and dlc.IsDelete=0 
where UserId={0}) AND dtco.OpenFlag=1 
    union 
    SELECT dtcorder.CourseId 
    FROM DepTran_CourseOrder dtcorder
    WHERE dtcorder.UserId={0} AND dtcorder.OrderStatus=1 )  --不可预订(去除可预订的和已预订的)
                                   ) 
                   ", userId);
                    break;
                case 1://未预订
                    where +=
                    string.Format(@" AND 
                                   (
                                      cc.Id IN  (SELECT dtco.CourseId FROM DepTran_CourseOpen dtco
                                        WHERE dtco.DepartId in (select du.DepartSetId from DepTran_DepartUser du  
join DepTran_DepartLeaderConfig dlc on dlc.Id=du.DepartSetId and dlc.IsDelete=0 
where UserId={0}) AND dtco.OpenFlag=1 
                                              AND dtco.CourseId NOT IN (SELECT dtcorder.CourseId 
                                                                        FROM DepTran_CourseOrder dtcorder
                                                                        WHERE dtcorder.UserId={0} AND dtcorder.OrderStatus=1) 
                                         )  --未预定 (即在可预订课程中去除已预订的)
                                       and (getdate()>=cc.StartTime) --未预定只显示进行中和已结束的可预订课程
                                   )  
                   ", userId);
                    break;
                default: //-1 不可预订+未预订
                    where +=
                    string.Format(@" AND 
                                   (
                                      cc.Id NOT IN (SELECT dtco.CourseId FROM DepTran_CourseOpen dtco
                                                WHERE dtco.DepartId in (select du.DepartSetId from DepTran_DepartUser du  
join DepTran_DepartLeaderConfig dlc on dlc.Id=du.DepartSetId and dlc.IsDelete=0 
where UserId={0}) AND dtco.OpenFlag=1 
    union 
    SELECT dtcorder.CourseId 
    FROM DepTran_CourseOrder dtcorder
    WHERE dtcorder.UserId={0} AND dtcorder.OrderStatus=1 )  --不可预订(去除可预订的和已预订的)
                        
                                      OR  
                                          (
                                              cc.Id IN  (SELECT dtco.CourseId FROM DepTran_CourseOpen dtco
                                                WHERE dtco.DepartId in (select du.DepartSetId from DepTran_DepartUser du  
join DepTran_DepartLeaderConfig dlc on dlc.Id=du.DepartSetId and dlc.IsDelete=0 
where UserId={0}) AND dtco.OpenFlag=1 
                                                      AND dtco.CourseId NOT IN (SELECT dtcorder.CourseId 
                                                                                FROM DepTran_CourseOrder dtcorder
                                                                                WHERE dtcorder.UserId={0} AND dtcorder.OrderStatus=1) 
                                                 )  --未预订 (即在可预订课程中去除已预订的)
                                                and (getdate()>=cc.StartTime) --未预订只显示进行中和已结束的可预订课程
                                             )
                                    )  
                                   ", userId);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(teacherName))
            {
                where += string.Format(" and u.Realname like '%{0}%'", teacherName.Trim().ReplaceSql());
            }
            if (!string.IsNullOrWhiteSpace(courseName))
            {
                where += string.Format(" and cc.CourseName like '%{0}%'", courseName.Trim().ReplaceSql());
            }
            if (isMust != -1)
            {
                where += string.Format(" and cc.IsMust={0} ", isMust);

            }
            if (learnStatus != -1)
            {
                if (learnStatus == 0)
                {
                    where += " and (getdate() < cc.StartTime)";
                }
                else if (learnStatus == 1)
                {
                    where += " and (getdate()>=cc.StartTime and getdate()<=cc.EndTime)";
                }
                else
                {
                    where += " and (getdate() >cc.EndTime)";
                }
            }
            if (sort != -1)
            {
                where += string.Format(" and charindex('{0}',cc.Sort)>0 ", sort);
            }

            if (!string.IsNullOrWhiteSpace(startTime))
            {
                where += " and cc.StartTime>='" + startTime + "' ";
            }
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                where += " and cc.StartTime<='" + endTime + "' ";
            }


            string mySubType =
                string.Format(
                    @" ,(case  
         when (SELECT count(dtco.CourseId) FROM DepTran_CourseOpen dtco
                                                WHERE dtco.DepartId in (select du.DepartSetId from DepTran_DepartUser du  
join DepTran_DepartLeaderConfig dlc on dlc.Id=du.DepartSetId and dlc.IsDelete=0 
where UserId={0}) AND dtco.OpenFlag=1 and dtco.CourseId=cc.Id)=0  
         then 0	  
         when (SELECT count(dtco.CourseId) FROM DepTran_CourseOpen dtco
                                                WHERE dtco.DepartId in (select du.DepartSetId from DepTran_DepartUser du  
join DepTran_DepartLeaderConfig dlc on dlc.Id=du.DepartSetId and dlc.IsDelete=0 
where UserId={0}) AND dtco.OpenFlag=1 and  dtco.CourseId=cc.Id 
                                                      AND dtco.CourseId NOT IN (SELECT dtcorder.CourseId 
                                                                                FROM DepTran_CourseOrder dtcorder
                                                                                WHERE dtcorder.UserId={0} AND dtcorder.OrderStatus=1) 
                                                 )>0 
          then 1  
          else 2
          end ) as MySubscribeType ",
                     userId);
            return _courseOrderDB.GetMyBroadcastList(out totalCount, mySubType, where, startIndex, pageLength);
        }
        #endregion



        #region 我的报表培训
        public static decimal TogetherSum = 0;
        public static decimal VideoSum = 0;
        public static decimal CPASum = 0;
        public static decimal DepSum = 0;//部门自学

        /// <summary>
        /// 个人算学时-我的报表
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        public string GetUserScoreToMyReport(int uid, int year, Sys_ParamConfig cpazq, bool flag = true)
        {

            decimal sumT1 = 0;
            decimal sumT2 = 0;
            decimal sumT = 0;
            //集中
            List<Cl_CourseOrder> Tolist = _userDB.GetTogetherList(uid, year);
            List<Cl_CourseOrder> tempT1 = Tolist.Where(p => p.IsMust == 0).ToList();//必修
            sumT1 = Convert.ToDecimal(tempT1.Sum(p => p.tmpScore));
            List<Cl_CourseOrder> tempT2 = Tolist.Where(p => p.IsMust == 1).ToList();//选修
            sumT2 = Convert.ToDecimal(tempT2.Sum(p => p.tmpScore));


            decimal sumV1 = 0;
            decimal sumV2 = 0;
            decimal sumV = 0;
            //视频
            List<Cl_CourseOrder> Volist = _userDB.GetVideoList(uid, year);
            List<Cl_CourseOrder> tempV1 = Volist.Where(p => p.IsMust == 0).ToList();//必修
            sumV1 = Convert.ToDecimal(tempV1.Sum(p => p.GetLength));
            List<Cl_CourseOrder> tempV2 = Volist.Where(p => p.IsMust == 1).ToList();//选修
            sumV2 = Convert.ToDecimal(tempV2.Sum(p => p.GetLength));

            //部门自学
            decimal sumD1 = 0;
            decimal sumD2 = 0;
            decimal sumD = 0;
            int totalcount = 0;
            var DepList = scoreDB.GetCourseShow(uid, "  YearPlan =" + year + " AND (ApprovalFlag =1 or  ApprovalFlag =-1)  and GetSumScore>0 ");
            sumD1 = DepList.Count == 0 ? 0 : DepList.FirstOrDefault().mustScore;//必修
            sumD2 = DepList.Count == 0 ? 0 : DepList.FirstOrDefault().selectScore;//必修
            sumD = DepList.Count == 0 ? 0 : DepList.FirstOrDefault().percenterSumScore;
            string mianstr = cpazq.ConfigValue;
            string[] CoType = Regex.Split(mianstr, ";", RegexOptions.IgnoreCase);
            //集中计算
            if (!string.IsNullOrEmpty(CoType[0]))
            {
                string[] ToType = Regex.Split(CoType[0], ",", RegexOptions.IgnoreCase);
                if (sumT1 > Convert.ToDecimal(ToType[1]))
                {
                    sumT1 = Convert.ToDecimal(ToType[1]);
                }
                if (sumT2 > Convert.ToDecimal(ToType[2]))
                {
                    sumT2 = Convert.ToDecimal(ToType[2]);
                }
                sumT = sumT1 + sumT2;
                if (sumT > Convert.ToDecimal(ToType[0]))
                {
                    sumT = Convert.ToDecimal(ToType[0]);
                }
            }
            else
            {
                sumT = sumT1 + sumT2;
            }
            //视频计算
            if (!string.IsNullOrEmpty(CoType[1]))
            {
                string[] VoType = Regex.Split(CoType[1], ",", RegexOptions.IgnoreCase);
                if (sumV1 > Convert.ToDecimal(VoType[1]))
                {
                    sumV1 = Convert.ToDecimal(VoType[1]);
                }
                if (sumV2 > Convert.ToDecimal(VoType[2]))
                {
                    sumV2 = Convert.ToDecimal(VoType[2]);
                }
                sumV = sumV1 + sumV2;
                if (sumV > Convert.ToDecimal(VoType[0]))
                {
                    sumV = Convert.ToDecimal(VoType[0]);
                }
            }
            else
            {
                sumV = sumV1 + sumV2;
            }
            if (!flag)//分所
            {
                if (CoType.Length >= 4)
                {
                    if (!string.IsNullOrEmpty(CoType[3]))
                    {
                        string[] VoType = Regex.Split(CoType[3], ",", RegexOptions.IgnoreCase);
                        if (sumD1 > Convert.ToDecimal(VoType[1]) && Convert.ToInt32(VoType[1]) != -1)
                        {
                            sumD1 = Convert.ToDecimal(VoType[1]);
                        }
                        if (sumD2 > Convert.ToDecimal(VoType[2]) && Convert.ToInt32(VoType[2]) != -1)
                        {
                            sumD2 = Convert.ToDecimal(VoType[2]);
                        }
                        if (Convert.ToInt32(VoType[0]) != -1)
                        {
                            if (sumD > Convert.ToDecimal(VoType[0]))
                            {
                                sumD = Convert.ToDecimal(VoType[0]);
                            }
                            else
                            {
                                sumD = sumD1 + sumD2;
                            }

                        }

                    }
                    else
                    {
                        sumD = sumD1 + sumD2;
                    }
                }
            }
            else//部门
            {
                if (CoType.Length >= 3)
                {
                    if (!string.IsNullOrEmpty(CoType[2]))
                    {
                        string[] VoType = Regex.Split(CoType[2], ",", RegexOptions.IgnoreCase);
                        if (sumD1 > Convert.ToDecimal(VoType[1]) && Convert.ToInt32(VoType[1]) != -1)
                        {
                            sumD1 = Convert.ToDecimal(VoType[1]);
                        }
                        if (sumD2 > Convert.ToDecimal(VoType[2]) && Convert.ToInt32(VoType[2]) != -1)
                        {
                            sumD2 = Convert.ToDecimal(VoType[2]);
                        }
                        sumD = sumD1 + sumD2;
                        if (sumD > Convert.ToDecimal(VoType[0]) && Convert.ToInt32(VoType[0]) != -1)
                        {
                            sumD = Convert.ToDecimal(VoType[0]);
                        }
                    }
                    else
                    {
                        sumD = sumD1 + sumD2;
                    }
                }
            }

            TogetherSum = sumT;
            VideoSum = sumV;
            decimal sumCPA = 0;
            sumCPA = _userDB.GetCPAScore(uid, year);
            CPASum = sumCPA;
            DepSum = sumD;
            return sumV +","+ sumT +","+ sumCPA +","+ sumD;
        }
        #endregion

        /// <summary>
        /// 查看是否学习了CPA课程的职业道德
        /// </summary>
        public int GetCpaWithIsMust(int userid, string year)
        {
            return _courseOrderDB.GetCpaWithIsMust(userid, year);
        }

        /// <summary>
        /// 第2期-明细表
        /// </summary>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetCompletionDetial(string where = " 1=1", string timewhere = " 1=1")
        {
            return _courseOrderDB.GetCompletionDetial(where, timewhere);
        }

        /// <summary>
        /// 查询部门
        /// </summary>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetDepartment(string name)
        {
            return _courseOrderDB.GetDepartment(name);
        }

  
        public List<Cl_CourseOrder> GetAttendByCompletionDetial()
        {
            return _courseOrderDB.GetAttendByCompletionDetial();
        }
    }
}