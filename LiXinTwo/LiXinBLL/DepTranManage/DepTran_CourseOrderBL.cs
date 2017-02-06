using LiXinDataAccess.DepTranManage;
using LiXinDataAccess.Examination;
using LiXinInterface.DepTranManage;
using LiXinModels.CourseManage;
using LiXinModels.DepTranManage;
using LiXinModels.Examination.DBModel;
using LiXinModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL.DepTranManage
{
    public class DepTran_CourseOrderBL : IDepTran_CourseOrder
    {
        private DepTran_CourseOrderDB deptrancourseorderdb;
        private readonly ExaminationDB _ExamDB = new ExaminationDB();
        private readonly ExampaperDB _paperDB = new ExampaperDB();

        public DepTran_CourseOrderBL()
        {
            deptrancourseorderdb = new DepTran_CourseOrderDB();
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddDepTran_CourseOrder(DepTran_CourseOrder model)
        {
            return deptrancourseorderdb.AddDepTran_CourseOrder(model);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateDepTran_CourseOrder(DepTran_CourseOrder model)
        {
            return deptrancourseorderdb.UpdateDepTran_CourseOrder(model);
        }

        public DepTran_CourseOrder GetCo_CourseMainPaperByCourseIdAndUserId(int CourseId, int UserId)
        {
            return deptrancourseorderdb.GetCo_CourseMainPaperByCourseIdAndUserId(CourseId, UserId);
        }
        /// <summary>
        /// 查看个人所有课程总退订次数
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetAllCourseOrderTimes(int userid)
        {
            return deptrancourseorderdb.GetAllCourseOrderTimes(userid);
        }

        public DepTran_CourseOrder Get(int Id)
        {
            return deptrancourseorderdb.Get(Id);
        }

        public bool Delete(int id)
        {
            return deptrancourseorderdb.Delete(id);
        }


        public List<Co_Course> GetCourseList(int userid, string where = " 1=1")
        {
            return deptrancourseorderdb.GetCourseList(userid, where);
        }


        public int GetYuDing(int courseid,int deptid)
        {
            return deptrancourseorderdb.GetYuDing(courseid, deptid);
        }

        public bool UpdateOrderStatus(int courseid, int userid, int OrderStatus)
        {
            return deptrancourseorderdb.UpdateOrderStatus(courseid, userid, OrderStatus);
        }



        public List<Co_Course> GetMyCourseOrderList(out int totalCount, int userId, int deptid = 0,
                                                                             string where = " 1 = 1 ",
                                                                             int startIndex = 0,
                                                                             int pageLength = int.MaxValue,
                                                                             string orderBy =
                                                                                 "ORDER BY Co_Course.id DESC")
        {
            return deptrancourseorderdb.GetMyCourseOrderList(out totalCount, userId, deptid, where, startIndex, pageLength, orderBy);
        }

        public Co_Course GetCourseById(int courseId, int userId = 0)
        {
            return deptrancourseorderdb.GetCourseById(courseId, userId);
        }

        public List<Co_Course> GetNewCourseList(out int totalCount, int userid, int DepartId, string where = " 1=1", int startIndex = 0,
                                                                              int pageLength = int.MaxValue)
        {
            return deptrancourseorderdb.GetNewCourseList(out totalCount, userid, DepartId, where, startIndex, pageLength);
        }
        public bool UpdateDepTran_CourseOrderEvlutionScore(int courseid, int userid, int EvlutionScore)
        {
            return deptrancourseorderdb.UpdateDepTran_CourseOrderEvlutionScore(courseid, userid, EvlutionScore);
        }

        public bool UpdateDepTran_CourseOrderExamScore(int courseid, int userid, int ExamScore)
        {
            return deptrancourseorderdb.UpdateDepTran_CourseOrderExamScore(courseid, userid, ExamScore);
        }

        public bool UpdateDepTran_CourseOrderAttScore(int courseid, int userid, int AttScore)
        {
            return deptrancourseorderdb.UpdateDepTran_CourseOrderAttScore(courseid, userid, AttScore);
        }



        /// <summary>
        /// 修改 条件自己加
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool UpdateWhere(string where)
        {
            return deptrancourseorderdb.UpdateWhere(where);
        }

        /// <summary>
        /// 更新考勤分数
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="attScore"></param>
        /// <returns></returns>
        public bool DepTran_CourseOrderUsers(int courseId, int userId, decimal attScore)
        {
            return deptrancourseorderdb.DepTran_CourseOrderUsers(courseId, userId, attScore);
        }

        /// <summary>
        /// 判断是否存在该科目该学员的考勤记录
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool ExistDeptTran_courseOrder(int courseId, int userId)
        {
            return deptrancourseorderdb.ExistDeptTran_courseOrder(courseId, userId);
        }

        /// <summary>
        /// 插入考勤分数(带部门)
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="departId"></param>
        /// <param name="attScore"></param>
        /// <returns></returns>
        public bool InsertDeptTran_courseOrder(int courseId, int userId, int departId, decimal attScore)
        {
            return deptrancourseorderdb.InsertDeptTran_courseOrder(courseId, userId, departId, attScore);
        }

        public List<DepTran_CourseOrder> SetUserOrder(out int totalCount, int courseId, int departId)
        {
            return deptrancourseorderdb.SetUserOrder(out totalCount, courseId, departId);
        }

        public bool SetOrder(int courseId, int departId, int userId)
        {
            return deptrancourseorderdb.SetOrder(courseId, departId, userId);
        }

        public List<DepTran_CourseOrder> GetAllOrder(int courseId, int departId)
        {
            return deptrancourseorderdb.GetAllOrder(courseId, departId);
        }

        /// <summary>
        /// 查询报名人员
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetCourseUserList(out int totalcount, int id, int startIndex = 1, int startLength = int.MaxValue, string where = " 1=1")
        {
            var list = deptrancourseorderdb.GetCourseUserList(id, startIndex, startLength, where);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }

        /// <summary>
        /// 获取相关考试人员的信息
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="CourseId"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<DepTran_CourseOrder> GetOnLineTest(out int totalCount, int CourseId, int startIndex = 0,
                                                               int pageLength = int.MaxValue)
        {
            List<DepTran_CourseOrder> clarr = new List<DepTran_CourseOrder>();

          //  List<DepTran_CourseOrder> cllist = new List<DepTran_CourseOrder>();

            //return _courseOrderDB.GetTeacherOnLineTest(out totalCount, CourseId, TeacherId, startIndex, pageLength);

            var list = deptrancourseorderdb.GetOnLineTest(CourseId, startIndex, pageLength);
            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;


            for (int i = 0; i < list.Count; i++)
            {
                tbExamSendStudent exam = null;

                exam = _ExamDB.GetExamSendStudentBySQL2008(list[i].CourseId, list[i].UserId, list[i].CoPaperID, list[i].PaperId, 4);

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

            
            return clarr;
        }


        /// <summary>
        /// 该方法找出视频转播的开课人数
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public Co_Course GetCo_CourseLimitNumber(int courseid, int deptid)
        {
            return deptrancourseorderdb.GetCo_CourseLimitNumber(courseid,deptid);
        }

    }

}
