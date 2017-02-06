using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.CourseLearn;

namespace LiXinControllers
{
    public partial class MyExamController : BaseController
    {
        protected ICl_Attendce IAttendce;

        public MyExamController(ICl_Attendce _IAttendce)
        {
            IAttendce = _IAttendce;
        }

        #region 页面呈现
        public ActionResult MyExamList()
        {
            return View();
        }

        /// <summary>
        /// 所有考试
        /// </summary>
        /// <returns></returns>
        public ActionResult AllExamList()
        {
            return View();
        }
        /// <summary>
        /// 未通过考试
        /// </summary>
        /// <returns></returns>
        public ActionResult NotPassExamList()
        {
            return View();
        }
        /// <summary>
        /// 通过的考试
        /// </summary>
        /// <returns></returns>
        public ActionResult PassExamList()
        {
            return View();
        }
        #endregion

        #region  我的全部考试

        public JsonResult GetAllExamList(string ExamName, string PassStatus, int pageSize = 10, int pageIndex = 1
            , int way = -1, int sort = -1, string start = "", string end = "")
        {
            string sql = "  1=1";
            string strsql = " 1=1";
            #region 查询条件
            int count = 0;
            if (!string.IsNullOrEmpty(ExamName))
            {
                if (ExamName != "请输入你要找的课程名称")
                {
                    sql += " and Co_Course.CourseName like '%" + ExamName.ReplaceSql() + "%'";
                }
            }
            if (!string.IsNullOrEmpty(PassStatus))
            {
                if (PassStatus != "2")
                {
                    if (PassStatus == "0")
                    {
                        sql += " and((way=1 and PassStatus=1) or (way=2 and IsPass=1))";
                    }
                    else
                    {
                        sql += " and((way=1 and PassStatus=2) or (way=2 and (IsPass=0 or IsPass is null)))";
                    }
                }
            }
            if (way != -1)
            {
                sql += " and way=" + way;
            }
            if (sort != -1)
            {
                sql += " and (SELECT  count(*)  FROM dbo.F_SplitIDs(sort) WHERE ID=" + sort + ")>0";
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


            var list = IAttendce.GetMyExamList(out count, CurrentUser.UserId, CurrentUser.TrainGrade, CurrentUser.DeptId, sql, (pageIndex - 1) * pageSize + 1, pageSize);

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
            string sql = " ((way=1 and PassStatus=2 or PassStatus=0) or (way=2 and (IsPass=0 or IsPass is null)))";
            int count = 0;
            if (!string.IsNullOrEmpty(ExamName))
            {
                if (ExamName != "请输入你要找的课程名称")
                {
                    sql += " and Co_Course.CourseName like '%" + ExamName.ReplaceSql() + "%'";
                }
            }

            var list = IAttendce.GetMyExamList(out count, CurrentUser.UserId, CurrentUser.TrainGrade, CurrentUser.DeptId, sql, (pageIndex - 1) * pageSize + 1, pageSize);

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
            string sql = " ((way=1 and PassStatus=1) or (way=2 and IsPass=1))";
            int count = 0;
            if (!string.IsNullOrEmpty(ExamName))
            {
                if (ExamName != "请输入你要找的课程名称")
                {
                    sql += " and Co_Course.CourseName like '%" + ExamName.ReplaceSql() + "%'";
                }
            }
            var list = IAttendce.GetMyExamList(out count, CurrentUser.UserId, CurrentUser.TrainGrade, CurrentUser.DeptId, sql, (pageIndex - 1) * pageSize + 1, pageSize);
            return Json(new
            {
                result = 1,
                dataList = list,
                recordCount = count

            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
