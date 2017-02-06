using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;
using LiXinDataAccess.CourseManage;
using LiXinModels.CourseManage;
using LiXinInterface.CourseManage;
using LiXinDataAccess.Examination;

namespace LiXinBLL.CourseManage
{
    public class Co_CourseBL : ICo_Course
    {
        private Co_CourseDB courseDb;
        private readonly ExaminationDB _ExamDB ;
        public Co_CourseBL()
        {
            courseDb = new Co_CourseDB();
            _ExamDB = new ExaminationDB();
        }

        private Co_CoursePaperDB _coursePaperDB = new Co_CoursePaperDB();

        public Co_Course GetCo_Course(int Id)
        {
            return courseDb.GetCo_Course(Id);
        }

        public Co_Course GetCo_Course(int Id, int userid)
        {
            return courseDb.GetCo_Course(Id, userid);
        }

        /// <summary>
        /// 验证课程编号是否重名
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="courseFrom">课程来源 0：年度计划；1：月度计划；2：课程管理</param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool IsExistCourseCode(string courseCode, int courseFrom = 2, int Id = 0, int way = 1)
        {
            return courseDb.IsExistCourseCode(courseCode, courseFrom, Id,way);
        }

        /// <summary>
        /// 验证课程是否重名
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="courseFrom">课程来源 0：年度计划；1：月度计划；2：课程管理</param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool IsExistCourseName(string courseName, int courseFrom = 2, int Id = 0)
        {
            return courseDb.IsExistCourseName(courseName, courseFrom, Id);
        }

        public bool UpdateCourse(Co_Course model)
        {
            return courseDb.UpdateCourse(model);
        }

        /// <summary>
        /// 根据课程编号更新  同一课次中的课程的课次数 2013-3-21 9:33:42
        /// </summary>
        /// <param name="code"></param>
        /// <param name="courseTimes"></param>
        /// <returns></returns>
        public bool UpdateCourseTimesByCode(string code, int courseTimes)
        {
            return courseDb.UpdateCourseTimesByCode(code, courseTimes);
        }

        /// <summary>
        /// 修改课程单一状态
        /// </summary>
        /// <param name="flag">0:删除 1:发布</param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public bool ModifySingleCourse(int flag, int courseId)
        {
            return courseDb.ModifySingleCourse(flag, courseId);
        }

        /// <summary>
        /// 新增课程
        /// </summary>
        /// <param name="model"></param>
        public void AddCourse(Co_Course model)
        {
            courseDb.AddCourse(model);
        }

        /// <summary>
        /// 新增课程--部门开放至全所使用
        /// </summary>
        /// <param name="model"></param>
        public int AddOpenCourse(Co_Course model)
        {
            courseDb.AddCourse(model);
            return model.Id;
        }


        /// <summary>
        /// 获取课程列表 CPA课程 并读取是否已经录入成绩完毕
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Co_Course> GetCourseCPAListImportScore(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                              int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Code,Co_Course.Id DESC")
        {
            return courseDb.GetCourseCPAListImportScore(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 获取课程列表 CPA课程
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Co_Course> GetCourseCPAList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                              int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Code,Co_Course.Id DESC")
        {
            return courseDb.GetCourseCPAList(out totalCount, where, startIndex, pageLength, orderBy);

        }

        /// <summary>
        /// 获取课程列表 视频课程
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Co_Course> GetCourseVideoList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                              int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Id DESC")
        {
            return courseDb.GetCourseVideoList(out totalCount, where, startIndex, pageLength, orderBy);
        }

        public List<Co_Course> GetCourseTogetherList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Id DESC")
        {
            return courseDb.GetCourseTogetherList(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 获取课程列表 普通获取List方式
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Co_Course> GetCourseCommonList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Code,Co_Course.Id DESC")
        {
            return courseDb.GetCourseCommonList(out totalCount, where, startIndex, pageLength, orderBy);

        }
        public List<Co_Course> GetCourseCommonListByJiZhong(out int totalCount, int userid, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Code,Co_Course.Id DESC")
        {
            return courseDb.GetCourseCommonListByJiZhong(out totalCount, userid,where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 课程关联试卷 如果存在那么更新 否则 新增
        /// </summary>
        /// <param name="coursePaper"></param>
        /// <returns></returns>
        public bool InsertOrUpdateCourseMainPaper(Co_CoursePaper coursePaper)
        {
            if (_coursePaperDB.IsExistCourseMainPaper(coursePaper.CourseId.Value))
            {
                return _coursePaperDB.UpdateCourseMainPaper(coursePaper);
            }
            else
            {
                return _coursePaperDB.AddCoursePaper(coursePaper);
            }
        }


        public List<Co_Course> GetCPACourseList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int userid = 0,
                                                int pageLength = int.MaxValue,
                                                string orderBy = "")
        {
            return courseDb.GetCPACourseList(out totalCount, where, userid, startIndex, pageLength, orderBy);
        }


        public List<Co_Course> GetVideoCourseList(out int totalCount, string where = " 1 = 1 ", string TrainGrade = "",int Userid=0,int DeptId=0,string IsOpenSub="", int startIndex = 0,
                                                  int pageLength = int.MaxValue,
                                                  string orderBy = "ORDER BY Co_Course.Code,Co_Course.Id DESC")
        {
            return courseDb.GetVideoCourseList(out totalCount, where, TrainGrade,Userid,DeptId,IsOpenSub, startIndex, pageLength, orderBy);
        }

        public Co_Course GetVideoCo_CourseById(int Id)
        {
            return courseDb.GetVideoCo_CourseById(Id);
        }


        public List<Co_Course> GetCourseIndexList(out int totalcount, string where = " 1 = 1 ", int startIndex = 1, int startLength = int.MaxValue, string order = "Id asc")
        {
            var list = courseDb.GetNoCPACourse(startIndex, startLength, where, order);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        public List<Co_CourseShow> GetCourseAvg(out int totalcount, int CourseID, int startIndex = 1, int startLength = int.MaxValue, string order = "tab.UserID asc")
        {
            int CoexmaID = 0;
            int TeexmaID = 0;
            Co_Course comodel = courseDb.GetCo_Course(CourseID);
            var ArrayValue = comodel.SurveyPaperId.Split(';');
            if (!string.IsNullOrEmpty(ArrayValue[0]))
            {
                var ArrValue = ArrayValue[0].Split(',');
                CoexmaID=Convert.ToInt32(ArrValue[1]);
            }
            if (!string.IsNullOrEmpty(ArrayValue[1]))
            {
                var ArrValue = ArrayValue[1].Split(',');
                TeexmaID = Convert.ToInt32(ArrValue[1]);
            }
            List<Co_CourseShow> list = courseDb.GetCourseAvg(CourseID, CoexmaID, TeexmaID, startIndex, startLength, order);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            foreach (var item in list)
            {
                item.SurveyPaperId = comodel.SurveyPaperId;
                item.Realname = item.Realname.HtmlXssEncode();
                item.DeptName = item.DeptName.HtmlXssEncode();
                item.TeacherName = comodel.TeacherName.HtmlXssEncode();
                item.CourseId = CourseID;
                if (item.CoAvg == null)
                {
                    item.CoAvg = 0;
                }
                if (item.TeAvg == null)
                {
                    item.TeAvg = 0;
                }
            }
            return list;
        }

        public List<Co_Course> GetMyReportList( string where = " 1 = 1 ", int Userid = 0)                                                   
        {
            var list = courseDb.GetMyReportList(where, Userid);
            foreach (var item in list)
            {
                
                if (item.IsTest == 1)
                {
                    var exam = _ExamDB.GetExamSendStudentWithByCourseIdAndUserId(item.Id, Userid, 2);
                    if (exam != null && exam.DoExamStatus!=0)
                    {

                        item.MyReport_ExamScore = exam.StudentAnswerList.Sum(p => p.GetScore);
                        item.MyReport_ExamNum = exam.TestTimes;
                        item.MyReport_ExamStatus = exam.DoExamStatus.ToString();
                        //item.MyReport_IsPass = exam.IsPass;

                    }
                    else
                    {
                        item.MyReport_ExamScore = -1;
                        item.MyReport_ExamNum = -1;
                        item.MyReport_ExamStatus = "0";                        
                        //item.MyReport_IsPass = 0;

                    }
                }
                else
                {
                    item.MyReport_ExamScore = -1;
                    item.MyReport_ExamNum = -1;
                    item.MyReport_ExamStatus = "N/A";
                    //item.MyReport_IsPass = 0;
                }

            }

            return list;
        }

    }
}
