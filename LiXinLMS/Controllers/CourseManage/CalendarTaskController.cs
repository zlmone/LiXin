using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseManage;
using System.Web.Mvc;
using LiXinCommon;
namespace LiXinControllers
{
    public partial class CourseManageController : BaseController
    {

        public ActionResult CourseTaskMonthSchedule(int year = 0, int month = 0)
        {
            if (year == 0)
            {
                ViewBag.Year = DateTime.Now.Year;
                ViewBag.Month = DateTime.Now.Month;
            }
            return View();
        }


        /// <summary>
        ///     某天的工作Attention
        /// </summary>Attention
        /// <param name="tID">课程ID</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <returns></returns>
        public JsonResult GetCourseDayTask(int year = 0, int month = 0, int day = 0)
        {
            DateTime date = Convert.ToDateTime(year + "-" + month + "-" + day);
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" Publishflag=1 AND CourseFrom=1 AND  PreCourseTime >='{0}' AND PreCourseTime <'{1}' AND  CHARINDEX('{2}',OpenLevel)>0  ", date, date.AddDays(1), CurrentUser.TrainGrade);
            int total = 0;
            List<Co_Course> listCourse = _courseBL.GetCourseCommonList(out total, strWhere.ToString(), 0, int.MaxValue);
            int n = 0;
            var itemArray = new object[listCourse.Count()];
            foreach (var item in listCourse)
            {
                var temp = new
                {
                    Id = item.Id,
                    CourseName = item.CourseName.HtmlXssEncode(),
                    TeacherName = item.Way == 3 ? item.CpaTeacher.HtmlXssEncode() : item.TeacherName,
                    OpenLevel = item.OpenLevel,
                    CourseAddress = item.Way == 3 ? item.CourseAddress.HtmlXssEncode() : item.RoomName.HtmlXssEncode(),
                    Memo = item.Memo,
                    Way = typeof(LiXinModels.Enums.Way).GetEnumName(item.Way)
                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = total
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        ///     获取某月的任务安排
        /// </summary>
        /// <param name="tID">课程ID</param>
        /// <param name="month">月</param>
        /// <param name="addMonth">所加的月数</param>
        /// <param name="year">年</param>
        /// <returns></returns>
        public JsonResult GetCalendarTask(string year = "", string month = "", int addMonth = 0)
        {
            year = year == "" ? DateTime.Now.Year.ToString() : year;
            month = month == "" ? DateTime.Now.Month.ToString() : month;

            int total = 0;
            DateTime quMonth = Convert.ToDateTime(year + "-" + month + "-1").AddMonths(addMonth);
            string _year = quMonth.Year.ToString();
            string _month = quMonth.Month.ToString();
            if (_month.Length == 1)
            {
                _month = "0" + _month;
            }
            string strwhere = string.Format(" Publishflag=1 AND  CourseFrom=1 AND Month='{0}' AND  CHARINDEX('{1}',OpenLevel)>0 ", _year.Trim() + "-" + _month.Trim(), CurrentUser.TrainGrade);
            List<Co_Course> listCourse = _courseBL.GetCourseCommonList(out total, strwhere, 0, int.MaxValue);
            //所查询的月的1号



            //初始化日历集合
            var calendarTask = new List<CalendarTask>();
            InitCalendarTaskCollection(ref calendarTask, quMonth);
            //加载当前月
            for (int i = 0; ; i++)
            {
                DateTime newdate = quMonth.AddDays(i);
                if (newdate.Month != quMonth.Month)
                    break;
                calendarTask.Add(new CalendarTask
                {
                    Year = newdate.Year,
                    Month = newdate.Month,
                    Day = newdate.Day,
                    Bg = (newdate.Day == DateTime.Now.Day && newdate.Month == DateTime.Now.Month) ? 3 : 1,
                    TaskTotal = listCourse.Where(c => c.PreCourseTime.Day == newdate.Day).Count()
                });
            }
            //加载后续的几个日子
            AddNextMonthDays(ref calendarTask, quMonth);

            return Json(new
            {
                dataList = calendarTask,
                year = quMonth.Year,
                month = quMonth.Month
            },
                        JsonRequestBehavior.AllowGet);
        }
    }
}
