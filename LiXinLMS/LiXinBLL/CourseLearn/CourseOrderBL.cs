using System.Collections.Generic;
using LiXinDataAccess;
using LiXinDataAccess.Examination;
using LiXinInterface.CourseLearn;
using LiXinModels.CourseLearn;
using System;
using System.Linq;
using LiXinDataAccess.CourseManage;
using LiXinDataAccess.CourseLearn;
using System.Diagnostics;
using LiXinModels.Examination.DBModel;
using LiXinModels.User;

namespace LiXinBLL.CourseLearn
{
    public class CourseOrderBL : ICourseOrder
    {
        private readonly CourseOrderDB _courseOrderDB = new CourseOrderDB();
        private readonly Co_CourseDB _courseDB = new Co_CourseDB();
        private readonly Cl_CpaLearnStatusDB _cpaLearnStatusDB = new Cl_CpaLearnStatusDB();

        private readonly ExaminationDB _ExamDB = new ExaminationDB();
        private readonly ExampaperDB _paperDB = new ExampaperDB();

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
            for (int i = 0; i < ids.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(ids[i]))
                    continue;
                try
                {
                    int flag = _courseOrderDB.GetCanSignupSpecialCrowdUserToCourse(out num, courseId, timespan);
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
                                                                              "ORDER BY Sys_User.userId DESC")
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

        public List<LiXinModels.CourseManage.Co_Course> GetMyCourseCPAList(out int totalCount, int userId, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.id DESC")
        {
            return _courseOrderDB.GetMyCourseCPAList(out totalCount, userId, where, startIndex, pageLength, orderBy);
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
            //return _courseOrderDB.GetTeacherOnLineTest(out totalCount, CourseId, TeacherId, startIndex, pageLength);
            var list = _courseOrderDB.GetTeacherOnLineTest(out totalCount, CourseId, TeacherId, startIndex, pageLength);
            for (int i = 0; i < list.Count; i++)
            {
                tbExamSendStudent exam = _ExamDB.GetExamSendStudentBySQL2005(list[i].CourseId.Value, list[i].UserId, list[i].CoPaperID, list[i].PaperId);

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
            }
            return list;
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
        public List<Sys_User> GetCourseOrderName(out int totalCount, int courseID, string where = "1=1", int pageSize = 10, int pageIndex = 1)
        {
            var list = _courseOrderDB.GetCourseOrderName(courseID,where, pageSize, pageIndex);
            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }
    }
}