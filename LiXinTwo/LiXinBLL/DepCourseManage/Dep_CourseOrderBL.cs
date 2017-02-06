using LiXinDataAccess.DepCourseManage;
using LiXinDataAccess.Examination;
using LiXinInterface.DepCourseManage;
using LiXinModels.DepCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.Examination.DBModel;

using LiXinModels.User;

using LiXinModels.CourseLearn;
using LiXinDataAccess.User;


namespace LiXinBLL.DepCourseManage
{
    public partial class Dep_CourseOrderBL : IDep_CourseOrder
    {
        private Dep_CourseOrderDB _dep_courseorder;
        private readonly ExaminationDB _ExamDB = new ExaminationDB();
        private readonly ExampaperDB _paperDB = new ExampaperDB();
        private UserDB _UserDb = new UserDB();

        public Dep_CourseOrderBL(Dep_CourseOrderDB dep_courseorder)
        {
            _dep_courseorder = dep_courseorder;
        }

        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertDep_CourseOrder(Dep_CourseOrder model)
        {
            _dep_courseorder.InsertDep_CourseOrder(model);
        }

        /// <summary>
        /// 我的课程(讲师)详细下学员列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="CourseId">课程ID</param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Dep_CourseOrder> GetTeacherCourseUserList(out int totalCount, int CourseId, int startIndex = 0, int pageLength = int.MaxValue)
        {
            return _dep_courseorder.GetTeacherCourseUserList(out totalCount, CourseId, startIndex, pageLength);
        }
        /// <summary>
        /// 我的课程(讲师)考勤学员列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="CourseId"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Dep_CourseOrder> GetTeacherCourseAttendceList(out int totalCount, int CourseId, int startIndex = 0,
                                                                 int pageLength = int.MaxValue)
        {
            return _dep_courseorder.GetTeacherCourseAttendceList(out totalCount, CourseId, startIndex, pageLength);
        }

        /// <summary>
        /// 我的课程讲师端学员端列表(加入考试成绩)
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="CourseId"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Dep_CourseOrder> GetTeacherOnLineTest(out int totalCount, int CourseId, int startIndex = 0,
                                                                 int pageLength = int.MaxValue)
        {
            var list = _dep_courseorder.GetTeacherOnLineTest(out totalCount, CourseId, startIndex, pageLength);
            for (int i = 0; i < list.Count; i++)
            {
                tbExamSendStudent exam = _ExamDB.GetExamSendStudentWithByCourseIdAndUserId(list[i].CourseId, list[i].UserId, 5);//5表示部门分所课程

                var exampaper = _paperDB.GetExampaper(list[i].PaperId);
                list[i].LevelScoreStr = Convert.ToInt32(list[i].LevelScore * 0.01 * (exampaper == null ? 0 : exampaper.ExampaperScore));

                if (exam != null)
                {
                    list[i].tbExamstudentId = exam._id;
                    list[i].DoExamStatus = exam.DoExamStatus;
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

        public LiXinModels.DepCourseManage.Dep_Course GetCourseById(int courseId, int userId = 0)
        {
            return _dep_courseorder.GetCourseById(courseId, userId);
        }


        public Dep_CourseOrder Get(int Id)
        {
            return _dep_courseorder.Get(Id);
        }



        /// <summary>
        /// 我的可预定
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userid"></param>
        /// <param name="DepartId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Dep_Course> GetDepCourseList(out int totalCount, int userid, int DepartId, string where = " 1=1", int startIndex = 0,
                                                                            int pageLength = int.MaxValue, string orderBy = "ORDER BY  aa.CourseId", string orderWhere = " (tt.courseid is null or tt.OrderStatus!=1)")
        {
            return _dep_courseorder.GetDepCourseList(out totalCount, userid, DepartId, where, startIndex, pageLength, orderBy, orderWhere);
        }

        public int GetYuDing(int courseid)
        {
            return _dep_courseorder.GetYuDing(courseid);
        }


        /// <summary>
        /// 根据课程ID和用户ID找出信息
        /// </summary>
        /// <param name="CourseId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public Dep_CourseOrder GetDep_CourseOrderByCourseIdAndUserId(int CourseId, int UserId)
        {
            return _dep_courseorder.GetDep_CourseOrderByCourseIdAndUserId(CourseId, UserId);
        }

        public bool UpdateDep_CourseOrder(Dep_CourseOrder model)
        {
            return _dep_courseorder.UpdateDep_CourseOrder(model);
        }


        public List<Dep_Course> GetMyCourseOrderList(out int totalCount, int userId,
                                                                              string where = " 1 = 1 ",
                                                                              int startIndex = 0,
                                                                              int pageLength = int.MaxValue,
                                                                              string orderBy =
                                                                                  "ORDER BY Dep_Course.id DESC", int apptime = 0)
        {
            return _dep_courseorder.GetMyCourseOrderList(out totalCount, userId, where, startIndex, pageLength, orderBy, apptime);
        }

        /// <summary>
        /// 查看个人总退订次数
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetAllCourseOrderTimes(int userid, int year = -1)
        {
            return _dep_courseorder.GetAllCourseOrderTimes(userid, year);
        }


        public bool UpdateOrderStatus(int courseid, int userid, int OrderStatus)
        {
            return _dep_courseorder.UpdateOrderStatus(courseid, userid, OrderStatus);
        }

        /// <summary>
        /// 请假
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateLeave(int id, string Reason, int courseid, int userid, Sys_User leader, double configHour)
        {
            return _dep_courseorder.UpdateLeave(id, Reason, courseid, userid,leader,configHour);
        }

        /// <summary>
        /// 课程预定查询列表
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <param name="jsRenderSortField"></param>
        /// <returns></returns>
        public List<Dep_Course> GetCourseSubscribeList(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " Id")
        {
            var list = _dep_courseorder.GetCourseSubscribeList(startIndex, startLength, where, jsRenderSortField);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
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
        public void AddSpecialCrowdUserToCourse(int courseId, int isAppoint, string userIds)
        {
            var ids = userIds.Split(',');
            int num = 0;
            for (int i = 0; i < ids.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(ids[i]))
                    //if (string.IsNullOrWhiteSpace(ids[i]))
                    continue;
                var user = _UserDb.Get(Convert.ToInt32(ids[i]));


                Dep_CourseOrder model = _dep_courseorder.GetCo_CourseMainPaperByCourseIdAndUserId(courseId, Convert.ToInt32(ids[i]));
                if (model != null)
                    return;

                //try
                //{
                    //int flag = _courseOrderDB.GetCanSignupSpecialCrowdUserToCourse(out num, courseId, timespan);
                    //if (flag == 1 || flag == 2)
                    //{
                    InsertDep_CourseOrder(new Dep_CourseOrder
                    {
                        CourseId = courseId,
                        UserId = Convert.ToInt32(ids[i]),
                        OrderTime = DateTime.Now,
                        OrderStatus = 1,
                        DepartSetId=user.DeptId,
                        //OrderEndTime = courseStartTime.AddHours(timespan * -1),
                        IsAppoint = isAppoint,
                        //CourseStartTime = courseStartTime,
                        //CourseEndTime = courseEndTime,
                        //CourseName = courseName,
                        PassStatus = "2",
                        //FtriggerFlag = 0
                    });
                    //}
                //}
                //catch
                // {
                //}
            }
        }




        /// <summary>
        /// 修改是否允许报名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status">1关闭  0开启</param>
        /// <returns></returns>
        public bool UpdateOrderFlag(int id, int status)
        {
            return _dep_courseorder.UpdateOrderFlag(id, status);
        }

        /// <summary>
        /// 获取报名成功的人员，根据培训级别分
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<NameSubscribeCount> GetSuccessTrainGradeSubscribe(int deptID, int courseID)
        {
            return _dep_courseorder.GetSuccessTrainGradeSubscribe(deptID, courseID);
        }

        /// <summary>
        /// 课程预定查询中能管理的所有人员
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetCourseSubscribeUserList(out int totalcount, int courseID, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " UserId")
        {
            var list = _dep_courseorder.GetCourseSubscribeUserList(courseID, startIndex, startLength, where, jsRenderSortField);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }

        /// <summary>
        /// 判断是否存在该科目该学员的考勤记录
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool ExistDeptTran_courseOrder(int courseId, int userId)
        {
            return _dep_courseorder.ExistDeptTran_courseOrder(courseId, userId);
        }

        /// <summary>
        /// 插入考勤分数(带部门)
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="departId"></param>
        /// <param name="attScore"></param>
        /// <returns></returns>
        public bool InsertDeptTran_courseOrder(int courseId, int userId, int departId, decimal attScore, decimal EvlutionScore = 0, decimal ExamScore = 0)
        {
            return _dep_courseorder.InsertDeptTran_courseOrder(courseId, userId, departId, attScore);
        }

        /// <summary>
        /// 更新考勤分数
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="attScore"></param>
        /// <returns></returns>
        public bool DepTran_CourseOrderUsers(int courseId, int userId, decimal attScore, int type, decimal EvlutionScore = 0, decimal ExamScore = 0)
        {
            return _dep_courseorder.DepTran_CourseOrderUsers(courseId, userId, attScore, type, EvlutionScore, ExamScore);
        }
        public Dep_CourseOrder GetCo_CourseMainPaperByCourseIdAndUserId(int CourseId, int UserId)
        {
            return _dep_courseorder.GetCo_CourseMainPaperByCourseIdAndUserId(CourseId, UserId);
        }


        #region == 部门课程指定查询 ==
        /// <summary>
        /// 查询课程中学员的预订情况
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="courseId">课程ID</param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Sys_User> GetDepCourseSubscribeUserList(out int totalCount, int courseId, string where = " 1 = 1 ",
                          int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " ORDER BY u.userId DESC ")
        {
            return _dep_courseorder.GetDepCourseSubscribeUserList(out totalCount, courseId, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 课程指定查询-添加课程【指定人员】
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <param name="isAppoint">0：自主报名 1：指定</param>
        /// <param name="userIds">学员Id</param>
        /// <param name="newAddUserIdList">新添加成功的人员id列表</param>
        public void AddDepSpecialCrowdUserToCourse(int courseId, int isAppoint, string userIds, out List<string> newAddUserIdList)
        {
            newAddUserIdList = new List<string>();
            var ids = userIds.Trim(',').Split(',');
            for (int i = 0; i < ids.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(ids[i]))
                    continue;
                try
                {
                    int flag = _dep_courseorder.GetDepCanSignupSpecialCrowdUserToCourse(courseId);
                    var user = _UserDb.Get(Convert.ToInt32(ids[i]));
                    if (flag == 1)
                    {
                        Add(new Dep_CourseOrder
                        {
                            CourseId = courseId,
                            UserId = Convert.ToInt32(ids[i]),
                            OrderTime = DateTime.Now,
                            OrderStatus = flag,
                            IsAppoint = isAppoint,
                            DepartSetId = user.DeptId,
                            LearnStatus = 0,
                            GetScore = 0,
                            PassStatus = "2",
                            AttScore = 0,
                            EvlutionScore = 0,
                            ExamScore = 0,
                            IsLeave = 0,
                            OrderTimes = 0
                        });
                        newAddUserIdList.Add(ids[i]);
                    }
                }
                catch
                {

                }
            }

        }
        /// <summary>
        /// 指定查询添加方法
        /// </summary>
        /// <param name="model"></param>
        public void Add(Dep_CourseOrder model)
        {
            var tmp = _dep_courseorder.GetCo_CourseMainPaperByCourseIdAndUserId(model.CourseId, model.UserId);
            if (tmp == null)
                _dep_courseorder.InsertDep_CourseOrder(model);
            else
            {
                tmp.PassStatus = model.PassStatus;
                tmp.OrderStatus = model.OrderStatus;
                tmp.LeaveTime = model.LeaveTime;
                tmp.OrderTime = model.OrderTime;
                tmp.DepartSetId = model.DepartSetId;
                tmp.IsAppoint = model.IsAppoint;
                tmp.LearnStatus = model.LearnStatus;
                tmp.GetScore = model.GetScore;
                tmp.AttScore = model.AttScore;
                tmp.EvlutionScore = model.EvlutionScore;
                tmp.ExamScore = model.ExamScore;
                tmp.IsLeave = 0;
                tmp.ApprovalFlag = 0;
                tmp.DropType = 0;
                tmp.DropReason = "";
                _dep_courseorder.UpdateDep_CourseOrder(tmp);
            }
        }

        /// <summary>
        /// 撤销部门指定人员
        /// </summary>
        /// <param name="sqlstr"></param>
        public void DepDropAssignUser(string sqlstr)
        {
            _dep_courseorder.DepDropAssignUser(sqlstr);
        }
        #endregion



        /// <summary>
        /// 修改 条件自己加
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool UpdateWhere(string where)
        {
            return _dep_courseorder.UpdateWhere(where);
        }


        /// <summary>
        /// 部门报名明细
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<Sys_Department> GetDeptSubscribe(int courseId)
        {
            return _dep_courseorder.GetDeptSubscribe(courseId);
        }


        /// <summary>
        /// 培训级别报名明细
        /// </summary>
        /// <param name="courseId"></param>
        public List<NameSubscribeCount> GetTrainGradeSubscribe(int courseId)
        {
            return _dep_courseorder.GetTrainGradeSubscribe(courseId);
        }


        /// <summary>
        /// 学员我的考勤列表及学时
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Dep_CourseOrder> GetMyAttendceList(out int totalCount, int userId,
                                                                             string where = " 1 = 1 ",
                                                                             int startIndex = 0,
                                                                             int pageLength = int.MaxValue,
                                                                             string orderBy =
                                                                                 "ORDER BY Dep_Course.id DESC")
        {
            return _dep_courseorder.GetMyAttendceList(out totalCount, userId, where, startIndex, pageLength, orderBy);
        }


        public List<Dep_CourseOrder> GetMyExamList(out int totalCount, int userId,
                                                                              string where = " 1 = 1 ",
                                                                              int startIndex = 0,
                                                                              int pageLength = int.MaxValue,
                                                                              string orderBy =
                                                                                  "ORDER BY Dep_Course.id DESC")
        {
            var list = _dep_courseorder.GetMyExamList(out totalCount, userId, where, startIndex, pageLength, orderBy);


            for (int i = 0; i < list.Count; i++)
            {
                tbExamSendStudent exam = _ExamDB.GetExamSendStudentBySQL2005(list[i].Id, userId, list[i].CoPaperID, list[i].PaperId);

                var exampaper = _paperDB.GetExampaper(list[i].PaperId);
                list[i].LevelScoreStr = Convert.ToInt32(list[i].LevelScore * 0.01 * (exampaper == null ? 0 : exampaper.ExampaperScore));
                list[i].ExampaperScore = exampaper == null ? 0 : exampaper.ExampaperScore;
                //var CoCoursePaper = ICoCoursePaperBL.GetCo_CourseMainPaper(

                //((double)examUser.StudentAnswerList.Sum(p => p.GetScore) / (double)exampaper.ExampaperScore) * 100
                if (exam != null)
                {
                    list[i].GetexamScore = exam.StudentAnswerList.Sum(pa => pa.GetScore);
                    list[i].ExamTestTimes = exam.TestTimes;  //记录考试次数,考了几次                   
                }
                else
                {
                    list[i].GetexamScore = 0;
                }

            }
            return list;
        }


        /// <summary>
        /// 获取集中授课中报名的人员
        /// </summary>
        public List<Sys_User> GetDepCourseOrderName(out int totalCount, int courseID, string where = "1=1", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " su.UserId asc")
        {
            var list = _dep_courseorder.GetDepCourseOrderName(courseID, where, pageSize, pageIndex, jsRenderSortField);
            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }

        #region 我的授课课程
        /// <summary>
        /// 查询我的授课的课程下报名成功的人员信息
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetCourseUserListByTeacher(out int totalcount, int courseID, int startIndex = 1, int startLength = int.MaxValue)
        {
            var list = _dep_courseorder.GetCourseUserListByTeacher(courseID, startIndex, startLength);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }

        /// <summary>
        /// 我的课程讲师端学员端列表(加入考试成绩)
        /// </summary>
        public List<Dep_CourseOrder> GetCourseTeacherOnLineTest(out int totalCount, int CourseId, int startIndex = 1, int pageLength = int.MaxValue)
        {
            List<Dep_CourseOrder> clarr = new List<Dep_CourseOrder>();


            var list = _dep_courseorder.GetCourseTeacherOnLineTest(CourseId, startIndex, pageLength);

            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;

            if (list.Count() == 0)
                return clarr;

            if (list[0].PaperId == 0)
            {
                totalCount = 0;
                return clarr;
            }
            else
            {


                for (int i = 0; i < list.Count; i++)
                {
                    tbExamSendStudent exam = null;
                    //2013-9-23 方法最后一个参数改为5，5是部门分所课程的考试
                    exam = _ExamDB.GetExamSendStudentBySQL2008(list[i].CourseId, list[i].UserId, list[i].CoPaperID, list[i].PaperId, 5);

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



                totalCount = clarr.Count();

                return clarr;
            }
        }
        #endregion


        /// <summary>
        /// 取消课程发布
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public bool CancelCoursePub(int courseid)
        {
            return _dep_courseorder.CancelCoursePub(courseid);
        }

        /// <summary>
        /// 删除发布全所后预定的人员
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public void DeleteZhiDingUser(int courseid)
        {
             _dep_courseorder.DeleteZhiDingUser(courseid);
        }
    }
}
