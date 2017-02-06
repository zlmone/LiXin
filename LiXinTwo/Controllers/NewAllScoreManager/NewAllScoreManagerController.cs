using LiXinInterface.ClassRoom;
using LiXinInterface.NewClassManage;
using LiXinInterface.NewCourseManage;
using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using System.Configuration;
using System.Web;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using LiXinInterface;
using LiXinControllers.Filter;

namespace LiXinControllers
{
    public partial class NewAllScoreManagerController : BaseController
    {
        protected INewClassRoom _classRoomBL;
        protected INew_Course _newcourseBL;
        protected INew_CourseOrder orderBL;
        protected IUserStatistics nuserBL;
        public NewAllScoreManagerController(INewClassRoom classRoomBL, INew_Course newcourseBL, INew_CourseOrder _orderBL, IUserStatistics _nuserBL)
        {
            _classRoomBL = classRoomBL;
            _newcourseBL = newcourseBL;
            orderBL = _orderBL;
            nuserBL = _nuserBL;
        }

        #region 页面呈现
        public ActionResult NewAllScore(int courseid, int IsGroupTeach)
        {
            ViewBag.AllClass = AllClass;
            ViewBag.courseid = courseid;
            ViewBag.IsGroupTeach = IsGroupTeach;
            return View();
        }

        public ActionResult NewAllScoreManagerList()
        {
            ViewBag.nowTime = DateTime.Now.Year + "-01-01 00:00";

            ViewBag.ClassRoomList = _classRoomBL.GetRoomList();

            return View();
        }

        public ActionResult AllInput(int IsGroupTeach)
        {
            ViewBag.IsGroupTeach = IsGroupTeach;
            return View();
        }

        public ActionResult SingleInput(int id, int IsGroupTeach)
        {
            var model = orderBL.Get(" Id=" + id);
            ViewBag.ids = id;
            ViewBag.IsGroupTeach = IsGroupTeach;
            return View(model.FirstOrDefault());
        }

        public ActionResult ImportScore()
        {
            return View();
        }
        #endregion

        #region 综合评分管理
        /// <summary>
        /// 评分管理列表
        /// </summary>
        /// <param name="CourseName"></param>
        /// <param name="ClassRoom"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetNewAllScoreManagerList(string CourseName, string ClassRoom, string starttime, string endtime, int pageSize = 10, int pageIndex = 1)
        {
            int litme = 0;

            string sql = " 1=1 and Way=1 ";

            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and CourseName like '%" + CourseName.ReplaceSql() + "%'";
            }
            if (!string.IsNullOrEmpty(ClassRoom))
            {
                if (ClassRoom != "0")
                {
                    sql += string.Format(@" and Id IN (SELECT CourseId FROM    New_CourseRoomRule where
   RoomId={0})", ClassRoom);
                }
            }
            if (!string.IsNullOrEmpty(starttime))
            {
                sql += " and StartTime>='" + starttime + "'";
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                sql += " and EndTime<='" + endtime + "'";
            }

            var list = _newcourseBL.GetNewAllScoreManager(out litme, sql, pageIndex, pageSize);

            return Json(new
            {
                dataList = list,
                recordCount = litme
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 综合评分录入

        /// <summary>
        /// 综合评分学员列表
        /// </summary>
        /// <param name="courseid">课程ID</param>
        /// <param name="NumberID">学号</param>
        /// <param name="Realname">姓名</param>
        /// <param name="ClassName">班级</param>
        /// <returns></returns>
        public JsonResult GetNewAllScoreList(int courseid, int IsGroupTeach, string NumberID = "", string Realname = "", int ClassID = -1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " su.UserId desc")
        {
            try
            {
                int totalCount = 0;
                string where = " 1=1";
                #region 条件
                if (!string.IsNullOrEmpty(NumberID))
                {
                    where += " and NumberID like '%" + NumberID.ReplaceSql() + "%'";
                }
                if (!string.IsNullOrEmpty(Realname))
                {
                    where += " and Realname like '%" + Realname.ReplaceSql() + "%'";
                }
                if (ClassID != -1)
                {
                    where += " AND ClassId=" + ClassID;
                }
                #endregion
                var list = orderBL.GetUserScoreList(out totalCount, courseid, pageIndex, pageSize, where, jsRenderSortField);
                var scorelist = nuserBL.GetListByUserID();

                //独立考试的答案
                foreach (var item in list)
                {
                    var score = scorelist.Find(p => p.CourseId == item.CourseId && p.UserId == item.UserId);
                    item.CourseExamScore = score == null ? 0 : score.CourseExamScore;
                    item.CourseExamSumScore = score == null ? 0 : score.CourseExamSumScore;
                    if (IsGroupTeach == 1)
                    {
                        item.sumScore = Convert.ToDecimal(item.TogetherScore);
                    }
                    else if (IsGroupTeach == 2)
                    {
                        item.sumScore = Convert.ToDecimal(item.GroupScore);
                    }
                    else
                    {
                        item.sumScore = Convert.ToDecimal(item.GroupScore + item.TogetherScore);
                    }
                }
                return Json(new
                {
                    dataList = list,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<New_CourseOrder>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 打分
        /// </summary>
        /// <returns></returns>
        public JsonResult SubmitInputScore(New_CourseOrder model)
        {
            try
            {
                orderBL.UpdateNew_CourseOrder(model);
                return Json(new
                {
                    result = 1,
                    Content = "打分成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    Content = "打分失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 批量打分
        /// </summary>
        /// <returns></returns>
        [Filter.SystemLog("新员工 综合评分管理 批量评分", LogLevel.Info)]
        public JsonResult SubmitAllInputScore(int TogetherScore, int GroupScore, string ids)
        {
            try
            {
                orderBL.UpdateAllScore(TogetherScore, GroupScore, ids);
                return Json(new
                {
                    result = 1,
                    Content = "打分成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    Content = "打分失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 一键评分
        /// </summary>
        /// <returns></returns>
         [Filter.SystemLog("新员工 综合评分管理 一键评分", LogLevel.Info)]
        public JsonResult InputAllScore(int courseid, int IsGroupTeach)
        {
            try
            {
                orderBL.UpdateAllScore(courseid, IsGroupTeach);
                return Json(new
                {
                    result = 1,
                    Content = "打分成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    Content = "打分失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入学员
        /// </summary>
        /// <returns></returns>
         [Filter.SystemLog("培训成绩录入 导入学员成绩", LogLevel.Info)]
        public JsonResult ImportScoreList(int courseid)
        {
            string folder = ConfigurationManager.AppSettings["NewSurveyUrl"];
            string filename = "";
            string resultName = "";
            try
            {

                var flag = true;
                int rowCount = 0;
                var message = "";
                var error = "";
                HttpFileCollectionBase FileData = Request.Files;
                filename = Path.GetFileName(FileData[0].FileName); //获得文件名
                string fullPathname = Path.Combine(folder, filename);//文件后缀名
                string suffix = FileData[0].FileName.Substring(FileData[0].FileName.LastIndexOf(".") + 1).ToLower();
                resultName = Guid.NewGuid() + "." + suffix;
                SaveCommonFile(FileData, folder, resultName);
                List<DataTable> dtList = new Spreadsheet().LoadExcel(HttpContext.Server.MapPath(folder) + resultName);
                int totalcount = 0;
                var courseList = orderBL.GetList(" courseid=" + courseid);
                var count = 0;
                if (IsTrueTemplate(dtList[0]))
                {
                    foreach (DataRow row in dtList[0].Rows)
                    {
                        rowCount++;
                        if (row[0].ToString() != "")
                        {

                            var LoginId = row[0].ToString().Trim();
                            var model = courseList.Find(p => string.Equals(p.LoginId, LoginId));
                            if (model != null)
                            {
                                var courseScore = row[2].ToString();
                                var courseSumScore = row[3].ToString();
                                var examScore = row[4].ToString();
                                var examSumScore = row[5].ToString();

                                model.CourseExamScore = 0;
                                model.CourseExamSumScore = 0;
                                model.ExamScore = 0;
                                model.ExamSumScore = 0;
                                if (IsScore(courseScore, courseSumScore))
                                {
                                    model.CourseExamScore = courseScore == "" ? 0 : courseScore.StringToInt32();
                                    model.CourseExamSumScore = courseSumScore == "" ? 0 : courseSumScore.StringToInt32();
                                    orderBL.UpdateNew_CourseOrder(model);
                                    count++;
                                }
                                else
                                {
                                    flag = false;
                                    error = error + "," + rowCount;
                                }

                                //if (IsScore(examScore, examSumScore))
                                //{
                                //    model.ExamScore =examScore==""?0: examScore.StringToInt32();
                                //    model.ExamSumScore = examSumScore == "" ? 0 : examSumScore.StringToInt32();
                                //    orderBL.UpdateNew_CourseOrder(model);
                                //    count++;
                                //}
                                //else
                                //{
                                //    flag = false;
                                //    error = error + "," + rowCount;
                                //}
                            }
                        }
                        else
                        {
                            flag = false;
                            error = error + "," + rowCount;
                        }
                    }
                    error = error == "" ? "" : error.TrimStart(',');
                    message = "导入成功，有" + count + "名学员有了分数" + (flag ? "" : ",第" + error + "行有错误");
                }
                else
                {
                    flag = false;
                    message = "模板头部有错误，请检查";
                }
                return Json(new
                {
                    result = 1,
                    content = message
                }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "导入失败"
                }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }



        //判断模板是否正确
        private bool IsTrueTemplate(DataTable dt)
        {
            if (dt.Columns.Count >= 4)
            {
                if (dt.Columns[0].ColumnName.Trim() != "登录名")
                    return false;
                if (dt.Columns[0].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[1].ColumnName.Trim() != "姓名")
                    return false;
                if (dt.Columns[1].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[2].ColumnName.Trim() != "课程考试得分(0-999)")
                    return false;
                if (dt.Columns[2].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[3].ColumnName.Trim() != "课程考试总分(1-999)")
                    return false;
                if (dt.Columns[3].DataType.FullName != "System.String")
                    return false;

            }
            else
                return false;
            return true;
        }


        public bool IsScore(string score, string sumScore)
        {
            var flag = true;
            if (score != "")
            {
                if (sumScore == "")
                {
                    return false;
                }
                else
                {
                    if (score.StringToInt32() > sumScore.StringToInt32())
                    {
                        return false;
                    }
                    else
                    {
                        var reg = new Regex(@"^\d{1,3}$");
                        if (reg.IsMatch(score) && reg.IsMatch(sumScore))
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion
    }
}
