using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.CourseLearn;
using LiXinModels.PlanManage;
using LiXinInterface.PlanManage;

namespace LiXinControllers
{
    public partial class MyAttendceController : BaseController
    {
        protected ICl_Attendce IAttendce;
        protected ITr_YearPlan Iyear;


        public MyAttendceController(ICl_Attendce _IAttendce, ITr_YearPlan _Iyear)
        {
            IAttendce = _IAttendce;
            Iyear = _Iyear;
        }

        public ActionResult MyAttendceList()
        {
            List<Tr_YearPlan> itemArray = Iyear.GetAllYear("IsDelete=0 and PublishFlag=1");
            ViewData["StrYear"] = itemArray;
            return View();
        }


        public JsonResult AttendceList(string CourseName, string year, string status, string Way, string Sort, int pageSize = 10, int pageIndex = 1
            , string teachername = "", int isMaster = -1, string start = "", string end = "")
        {

            string sql = " 1=1";
            #region 查询条件

            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and Co_Course.CourseName like '%" + CourseName.ReplaceSql() + "%'";
            }
            if (!string.IsNullOrEmpty(year))
            {
                sql += " and Co_Course.YearPlan=" + year;
            }

            //<option value="0">正常</option>
            //<option value="1">缺勤</option>
            //<option value="2">迟到</option>
            //<option value="3">早退</option>

            if (!string.IsNullOrEmpty(status))
            {
                if (status == "0")
                {
                      sql += "  and";
			          sql += "  (";
				      sql += "      Co_Course.AttFlag=0";
				      sql += "       or ";
				      sql += "       ( ";
					  sql += "          Co_Course.AttFlag=1 ";
					  sql += "          and ";
					  sql += "          Cl_Attendce.StartTime<=Co_Course.StartTime";
				      sql += "       )";
				      sql += "       or";
				      sql += "      (";
					  sql += "          Co_Course.AttFlag=2";
					  sql += "          and ";
					  sql += "          Cl_Attendce.EndTime>=Co_Course.EndTime";
				      sql += "       )";
				      sql += "       or";
				      sql += "       (";
					  sql += "          Co_Course.AttFlag=3";
					  sql += "          and";
					  sql += "          Cl_Attendce.StartTime<=Co_Course.StartTime and Cl_Attendce.EndTime>=Co_Course.EndTime ";
				      sql += "       )";
                      sql += "  )";
                }
                else if (status == "1")
                {
                   sql += " and";
			       sql += " (	";			
				   sql += "      ( ";
					sql += "        Co_Course.AttFlag=1 ";
					sql += "        and ";
                    sql += "       Co_Course.StartTime<'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and Co_Course.EndTime<'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and (Cl_Attendce.StartTime is null or Cl_Attendce.StartTime='2050-1-1')";
				    sql += "     )";
				    sql += "     or";
				    sql += "     (";
					sql += "        Co_Course.AttFlag=2";
					sql += "        and ";
                    sql += "       Co_Course.StartTime<'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and Co_Course.EndTime<'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and (Cl_Attendce.EndTime is null or Cl_Attendce.EndTime='2000-1-1')";
				    sql += "     )";
				    sql += "     or";
				    sql += "     (";
					sql += "        Co_Course.AttFlag=3";
					sql += "        and";
                    sql += "       Co_Course.StartTime<'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and Co_Course.EndTime<'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and ((Cl_Attendce.StartTime is null or Cl_Attendce.StartTime='2050-1-1') and (Cl_Attendce.EndTime is null or Cl_Attendce.EndTime='2000-1-1'))";
				    sql += "     )";
			        sql += ")";
                }
                else if (status == "2")
                {
                   sql += " and";
			       sql += " (				";
				   sql += "      ( ";
				   sql += "        Co_Course.AttFlag=1 ";
				   sql += "        and ";
                   sql += "        Co_Course.StartTime<'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and Cl_Attendce.StartTime is not null";
                   sql += "               and Cl_Attendce.StartTime <>'2050-1-1'";
					sql += "              and Cl_Attendce.StartTime>Co_Course.StartTime";                     
				    sql += "     )";	
				    sql += "     or";
				    sql += "     (";
					sql += "        Co_Course.AttFlag=3";
					sql += "        and";
                    sql += "        Co_Course.StartTime<'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and Cl_Attendce.StartTime is not null";
                    sql += "               and Cl_Attendce.StartTime <>'2050-1-1'";
                    sql += "               and Cl_Attendce.StartTime>Co_Course.StartTime  and Cl_Attendce.EndTime<>'2000-1-1'";
				    sql += "     )";
                    sql += ")";
                }
                else if (status == "3")
                {
                    sql += " and";
                    sql += " (		";             
                    sql += "     (";
                    sql += "        Co_Course.AttFlag=2";
                    sql += "        and ";
                    sql += "        Co_Course.StartTime<'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and Cl_Attendce.EndTime is not null";
                    sql += "               and Cl_Attendce.EndTime <>'2000-1-1'";
                    sql += "               and Cl_Attendce.EndTime<Co_Course.EndTime";
                    sql += "     )";
                    sql += "     or";
                    sql += "     (";
                    sql += "        Co_Course.AttFlag=3";
                    sql += "              and Co_Course.StartTime<'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and Cl_Attendce.EndTime is not null";
                    sql += "               and Cl_Attendce.EndTime <>'2000-1-1'";
                    sql += "              and Cl_Attendce.EndTime<Co_Course.EndTime ";
                    sql += "     )";
                    sql += ")";
                }
                else if (status == "4")
                {
                    sql += " and";
                    sql += " (		"; 
                    sql += "        Co_Course.AttFlag=3";
                    sql += "        and Co_Course.StartTime<'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' and Cl_Attendce.StartTime is not null and Cl_Attendce.StartTime<>'2050-1-1' and Cl_Attendce.EndTime is not null ";//and Cl_Attendce.EndTime<>'2000-1-1'
                    sql += " and Cl_Attendce.EndTime<Co_Course.EndTime and  Cl_Attendce.StartTime>Co_Course.StartTime";
                    sql += ")";
                }
                else if (status == "5")
                {
                    sql += " and Co_Course.StartTime>'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' ";
                }
                else if (status == "6")
                {
                    sql += " and";
                    sql += " (	";                  
                    sql += "       Co_Course.StartTime<='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and Co_Course.EndTime>'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                     sql += ")";



                }

            }
            if (!string.IsNullOrEmpty(Way) && Way != "0")
            {
                if (Way == "1")
                {
                    sql += " and Way=1";
                }
                else if (Way == "2")
                {
                    sql += " and Way=2";
                }
                else if (Way == "3")
                {
                    sql += " and Way=3";
                }
            }

            if (!string.IsNullOrEmpty(Sort) && Sort != "0")
            {
                if (Sort == "1")
                {
                    sql += " and charindex('1',Sort)>0";
                }
                else if (Sort == "2")
                {
                    sql += " and charindex('2',Sort)>0";
                }
                else if (Sort == "3")
                {
                    sql += " and charindex('3',Sort)>0";
                }
                else if (Sort == "4")
                {
                    sql += " and charindex('4',Sort)>0";
                }
            }


            if (!string.IsNullOrEmpty(start))
            {
                sql += string.Format(" AND Co_Course.StartTime>='{0}'", start);
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql += string.Format(" AND Co_Course.EndTime<='{0}'", Convert.ToDateTime(end).AddDays(1).AddSeconds(-1));
            }
            if (!string.IsNullOrEmpty(teachername))
            {
                sql += " and Realname like '%" + teachername.ReplaceSql() + "%'";
            }
            if (isMaster != -1)
            {
                sql += " and  IsMust=" + isMaster;
            }
            #endregion
            int count = 0;
              var list = IAttendce.GetMyAttendcsList(out count, CurrentUser.UserId, sql, (pageIndex - 1) * pageSize + 1, pageSize);


            return Json(new
            {
                result = 1,
                dataList = list,
                recordCount = count
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AttendceListIndex()
        {

            const string sql = " 1=1";
            
            var count = 0;
            var list = IAttendce.GetMyAttendcsList(out count, CurrentUser.UserId, sql, 1, 4,1);
            var newlist = new List<object>();
            list.ForEach(p=> newlist.Add(new
                                             {
                                                 name=p.CourseName,
                                                 date="",
                                                 status=p.Status,
                                                 flag=1
                                             }));
            return Json(newlist, JsonRequestBehavior.AllowGet);
        }
    }
}
