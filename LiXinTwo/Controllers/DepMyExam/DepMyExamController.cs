using LiXinInterface.DepCourseManage;
using LiXinInterface.DeptSurvey;
using LiXinInterface.Examination;
using LiXinModels.Examination.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;

namespace LiXinControllers.DepMyExam
{
    public class DepMyExamController : BaseController
    {
        private IDep_Course _IDep_course;
        private IDep_CoursePaper _IDep_CoursePaper;
        private IDep_CourseOrder _IDep_CourseOrder;
        private IDep_Attendce _Dep_Attendce;

        protected IDepSurveyReplyAnswer _IDepSurveyReplyAnswerBL;
        protected IExamination IExaminationrBL;

        public DepMyExamController(IDep_Course IDep_course, IDep_CoursePaper IDep_CoursePaper, IDepSurveyReplyAnswer IDepSurveyReplyAnswerBL,
            IExamination _IExaminationrBL, IDep_CourseOrder IDep_CourseOrder, IDep_Attendce Dep_Attendce)
        {
            _IDep_course = IDep_course;
            _IDep_CoursePaper = IDep_CoursePaper;
            _IDepSurveyReplyAnswerBL = IDepSurveyReplyAnswerBL;
            IExaminationrBL = _IExaminationrBL;
            _IDep_CourseOrder = IDep_CourseOrder;
            _Dep_Attendce = Dep_Attendce;
        }

        public ActionResult AllExamList()
        {
            return View();
        }


        public ActionResult MyExamList()
        {
            return View();
        }

        public ActionResult NotPassExamList()
        {
            return View();
        }

        public ActionResult PassExamList()
        {
            return View();
        }


        #region  我的全部考试

        public JsonResult GetAllExamList(string ExamName,  int pageSize = 10, int pageIndex = 1
            , string start = "", string end = "")
        {
            string sql = "  1=1";
            string strsql = " 1=1";
            #region 查询条件
            int count = 0;
            if (!string.IsNullOrEmpty(ExamName))
            {
                if (ExamName != "请输入你要找的课程名称")
                {
                    sql += " and Dep_Course.CourseName like '%" + ExamName.ReplaceSql() + "%'";
                }
            }
            

            if (!string.IsNullOrEmpty(start))
            {
                sql += string.Format(" AND StartTime>='{0}'", start);
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql += string.Format(" AND EndTime<='{0}'", Convert.ToDateTime(end).AddDays(1).AddSeconds(-1));
            }
            #endregion


            var list = _IDep_CourseOrder.GetMyExamList(out count, CurrentUser.UserId, sql, (pageIndex - 1) * pageSize + 1, pageSize);

            return Json(new
            {
                result = 1,
                dataList = list,
                recordCount = count

            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 我的未通过考试
        public JsonResult GetNotPassExamList(string ExamName, int pageSize = 10, int pageIndex = 1)
        {
            string sql = " ((way=1 or way=3) and (PassStatus=2 or PassStatus=0))";
            int count = 0;
            if (!string.IsNullOrEmpty(ExamName))
            {
                if (ExamName != "请输入你要找的课程名称")
                {
                    sql += " and Dep_Course.CourseName like '%" + ExamName.ReplaceSql() + "%'";
                }
            }

            var list = _IDep_CourseOrder.GetMyExamList(out count, CurrentUser.UserId, sql, (pageIndex - 1) * pageSize + 1, pageSize);

            return Json(new
            {
                result = 1,
                dataList = list,
                recordCount = count
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 我通过的考试
        public JsonResult GetPassExamList(string ExamName, int pageSize = 10, int pageIndex = 1)
        {
            string sql = " ((way=1 or way=3) and PassStatus=1)";
            int count = 0;
            if (!string.IsNullOrEmpty(ExamName))
            {
                if (ExamName != "请输入你要找的课程名称")
                {
                    sql += " and Dep_Course.CourseName like '%" + ExamName.ReplaceSql() + "%'";
                }
            }
            var list = _IDep_CourseOrder.GetMyExamList(out count, CurrentUser.UserId, sql, (pageIndex - 1) * pageSize + 1, pageSize);
            return Json(new
            {
                result = 1,
                dataList = list,
                recordCount = count

            }, JsonRequestBehavior.AllowGet);
        }
        #endregion



        public JsonResult FGetInto(int courseid, int ExamPaperId, int SourceType)
        {
            var course = _IDep_course.GetCo_Course(courseid);
            var coursepaper = _IDep_CoursePaper.GetCo_CourseMainPaper(courseid);//查找关联试卷表


            var att = _Dep_Attendce.GetDepAttendce(courseid, CurrentUser.UserId);

            if (att == null || att.Status == 0 || att.Status == 2)
            {
                return Json(new { result = 0, message = "请先考勤后才能进行考试" }, JsonRequestBehavior.AllowGet);
            }

            //如果课程有课后评估则判断 没有则直接进入考试
            if (course.IsPing == 1)
            {
                if (course.SurveyPaperId != "")
                {
                    var arr = course.SurveyPaperId.Split(';');
                    if (arr[0] != "")
                    {
                        if (!_IDepSurveyReplyAnswerBL.GetStatus(courseid, CurrentUser.UserId, 1, Convert.ToInt32(arr[0].Split(',')[1])))
                        {
                            return Json(new { result = 0, message = "请先提交课后评估，再进行考试" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (arr[1] != "")
                    {
                        if (!_IDepSurveyReplyAnswerBL.GetStatus(courseid, CurrentUser.UserId, 1, Convert.ToInt32(arr[1].Split(',')[1])))
                        {
                            return Json(new { result = 0, message = "请先提交讲师评估，再进行考试" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            DateTime dateTime = DateTime.Now;
            DateTime endtime = course.EndTime.AddHours(Convert.ToDouble(coursepaper.Hour));//根据参数配置的 单位:分钟

            if (dateTime > endtime)
            {
                //return Json(new { result = 0, message = "课程结束后" + coursepaper.Hour + "小时后,才能进行考试" }, JsonRequestBehavior.AllowGet);
                return Json(new { result = 0, message = "考试已经过期" }, JsonRequestBehavior.AllowGet);
            }
            if (SourceType == 5)
            {
                if (dateTime < course.EndTime)
                {
                    return Json(new { result = 0, message = "课程还未结束，无法进行考试" }, JsonRequestBehavior.AllowGet);
                }
            }

            var Student = IExaminationrBL.GetSingletbExamSendStudentByCourseIdAndUserId(courseid, CurrentUser.UserId, SourceType);

            if (Student == null)
            {
                tbExamSendStudent model = new tbExamSendStudent();
                model.RelationID = courseid; //存放课程ID
                model.SourceType = SourceType; //区分0;1
                model.ExamPaperID = ExamPaperId;
                model.UserID = CurrentUser.UserId;
                model.DoExamStatus = 0;
                model.LastUpdateTime = DateTime.Now;
                model.SubmitTime = DateTime.Now;
                model.TestTimes = 0;
                model.IsPass = 0;
                model.Status = 0;
                model.CoursePaperId = coursepaper.Id;//添加
                model.StudentAnswerList = new List<ReStudentExamAnswer>();

                int t = IExaminationrBL.InserttbExamSendStudent(model);

                return Json(new { result = 1, euId = model._id }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Student.RelationID != courseid)
                {

                    IExaminationrBL.DeleteExamSendStudentWithByCourseIdAndUserId(courseid, CurrentUser.UserId, SourceType);

                    tbExamSendStudent model = new tbExamSendStudent();
                    model.RelationID = courseid; //存放课程ID
                    model.SourceType = SourceType; //区分0;1
                    model.ExamPaperID = ExamPaperId;
                    model.UserID = CurrentUser.UserId;
                    model.DoExamStatus = 0;
                    model.LastUpdateTime = DateTime.Now;
                    model.SubmitTime = DateTime.Now;
                    model.TestTimes = 0;
                    model.IsPass = 0;
                    model.Status = 0;
                    model.CoursePaperId = coursepaper.Id;
                    model.StudentAnswerList = new List<ReStudentExamAnswer>();

                    int t = IExaminationrBL.InserttbExamSendStudent(model);

                    return Json(new { result = 1, euId = model._id }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { result = 1, euId = Student._id }, JsonRequestBehavior.AllowGet);
        }
    }
}
