using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.NewCourseManage;
using LiXinModels.NewCourseManage;
using LiXinInterface.NewCourseManage;
using LiXinDataAccess.Examination;
using LiXinModels.Examination.DBModel;

namespace LiXinBLL.NewCourseManage
{
    public class New_CourseOrderBL : INew_CourseOrder
    {
        protected static New_CourseOrderDB orderDB;

        private readonly ExaminationDB _ExamDB = new ExaminationDB();
        private readonly ExampaperDB _paperDB = new ExampaperDB();

        public New_CourseOrderBL()
        {
            orderDB = new New_CourseOrderDB();
        }

        #region 增改查(基本信息)

        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertNew_CourseOrder(New_CourseOrder model)
        {
            orderDB.InsertNew_CourseOrder(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateNew_CourseOrder(New_CourseOrder model)
        {
            orderDB.UpdateNew_CourseOrder(model);
        }

        /// <summary>
        /// 已存在的课程学习记录
        /// </summary>
        /// <param name="sqlStr">删除条件</param>
        public void DeleteCourseOrder(string sqlStr = "1=2")
        {
            orderDB.DeleteCourseOrder(sqlStr);
        }

        /// <summary>
        /// 获取此课程下面学员的评价详情
        /// </summary>
        /// <returns></returns>
        public List<New_CourseOrder> GetUserScoreList(out int totalcount, int courseID, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = "  su.UserId desc")
        {
            var list = orderDB.GetUserScoreList(courseID, startIndex, startLength, where, jsRenderSortField);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }

        /// <summary>
        /// 批量打分
        /// </summary>
        public void UpdateAllScore(int TogetherScore, int GroupScore, string ids)
        {
            orderDB.UpdateAllScore(TogetherScore, GroupScore, ids);
        }

        /// <summary>
        ///一键打分
        /// </summary>
        public void UpdateAllScore(int courseID, int IsGroupTeach)
        {
            orderDB.UpdateAllScore(courseID, IsGroupTeach);
        }
        #endregion


        /// <summary>
        /// 新进学员课程列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userid"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<New_CourseOrder> GetNew_CourseOrderListByStudent(int userid, string where = " 1 = 1 ", string strwhere = " 1=1", int startIndex = 0,
                             int pageLength = int.MaxValue, string orderBy = "order by New_Course.StartTime")
        {
            return orderDB.GetNew_CourseOrderListByStudent(userid, where, strwhere, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 讲师课程列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userid"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        //public List<New_CourseOrder> GetNew_CourseOrderListByTeacher(out int totalCount, int userid, string where = " 1 = 1 ", int startIndex = 0,
        //                     int pageLength = int.MaxValue, string orderBy = "ORDER BY New_CourseOrder.Id DESC")
        //{
        //    return orderDB.GetNew_CourseOrderListByTeacher(out totalCount, userid, where, startIndex, pageLength, orderBy);
        //}
        public List<New_CourseOrder> GetNew_CourseOrderListByTeacher(int userid, string where = " 1 = 1 ", string strwhere = " 1=1", int startIndex = 0,
                             int pageLength = int.MaxValue, string orderBy = "ORDER BY New_CourseOrder.Id DESC")
        {
            return orderDB.GetNew_CourseOrderListByTeacher(userid, where, strwhere, startIndex, pageLength, orderBy);
        }

        public List<New_CourseOrder> GetCourseIdAndType(int courseid, int userid)
        {
            return orderDB.GetCourseIdAndType(courseid, userid);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<New_CourseOrder> Get(string where = "1=1")
        {
            return orderDB.Get(where);
        }

        /// <summary>
        /// 根据课程ID和用户ID 修改学习状态
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="LearnStatus">0：未开始；1：进行中；2：已完成</param>
        public void UpdateLearnStatus(int courseid, int userid, int LearnStatus)
        {
            orderDB.UpdateLearnStatus(courseid, userid, LearnStatus);
        }


        public List<New_CourseOrder> GetTeacherOnLineTest(out int totalCount, int CourseId, int TeacherId, int startIndex = 0,
                                                                int pageLength = int.MaxValue)
        {
            //return _courseOrderDB.GetTeacherOnLineTest(out totalCount, CourseId, TeacherId, startIndex, pageLength);
            var list = orderDB.GetTeacherOnLineTest(out totalCount, CourseId, TeacherId, startIndex, pageLength);
            for (int i = 0; i < list.Count; i++)
            {
                tbExamSendStudent exam = _ExamDB.GetExamSendStudentBySQL2005(list[i].CourseId, list[i].UserId, list[i].CoPaperID, list[i].PaperId);

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



        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<New_CourseOrder> GetList(string where = " 1=1")
        {
            return orderDB.GetList(where);
        }


        /// <summary>
        /// 点击日期获取讲师当日所有课程
        /// </summary>
        /// <param name="teacherid"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<New_CourseOrder> GetTeachertoLearnDetal(int teacherid, string where = " 1=1")
        {
            return orderDB.GetTeachertoLearnDetal(teacherid, where);
        }

        /// <summary>
        /// 学员点击日期获取当日所有课程
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<New_CourseOrder> GetStudentLearnDetal(int userid, string where = " 1=1")
        {
            return orderDB.GetStudentLearnDetal(userid, where);
        }

        public List<New_CourseOrder> GetGetMyproposeList(out int totalCount, int userid, string where = " 1=1", string strwhere = " 1=1", string jsRenderSortField = "", int startIndex = 0, int pageLength = int.MaxValue)
        {
            return orderDB.GetGetMyproposeList(out totalCount, userid, where, strwhere, jsRenderSortField, startIndex, pageLength);
        }

        public List<New_CourseOrder> GetAfterCourseList(out int totalCount, int userid, string where, string strwhere, string jsRenderSortField = "", int startIndex = 0, int pageLength = int.MaxValue)
        {
            return orderDB.GetAfterCourseList(out totalCount, userid, where, strwhere, jsRenderSortField, startIndex, pageLength);
        }

    }
}
