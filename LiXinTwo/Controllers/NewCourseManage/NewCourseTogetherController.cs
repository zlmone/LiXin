using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinModels;
using LiXinModels.NewClassManage;
using LiXinModels.NewCourseManage;
using LiXinModels.Survey;
using LiXinModels.User;
using LixinModels.NewClassManage;
using System.Text.RegularExpressions;
using LiXinControllers.Filter;

namespace LiXinControllers
{
    public partial class NewCourseManageController : BaseController
    {
        #region 页面呈现

        /// <summary>
        /// 批量修改排座信息（临时快捷修改用）
        /// </summary>
        /// <returns></returns>
        public ActionResult SeatHelp()
        {
            return View();
        }

        /// <summary>
        /// 设置分组带教的设置
        /// </summary>
        /// <returns></returns>
        public ActionResult SetNowGroup()
        {
            return View();
        }
        /// <summary>
        /// 选择教室
        /// </summary>
        /// <returns></returns>
        public ActionResult SelClassRoom()
        {
            return View();
        }
        
        /// <summary>
        /// 集中授课页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseSeatSistribute(int courseID,int rtype=0)
        {
            //课程信息
            var course = _courseBL.GetSingleCourse(courseID);
            var list = _courseRoomRule.GetNew_CourseRoomRuleListByCourseId(courseID, string.Format(" Type={0}", rtype));
            //时间段集合
            ViewBag.DateList = new List<New_CourseRoomRule>();
            if (list.Count>0)
            {
                ViewBag.DateList = list.GroupBy(p => p.RoomId).ToList()[0].ToList();
            }
            //课程已选班级
            //ViewBag.ClassList = _newclass.GetClassList(string.Format(" Id in ({0}) ", course.Classes));

            //评估问卷
            //评估问卷
            ViewBag.SurveyPaperList = new List<Survey_Exampaper>();
            ViewBag.SurveyPaperID = 0;
            var ptempList = list.Where(p => p.Type == 0).ToList();
            if (ptempList.Count > 0)
            {
                ViewBag.SurveyPaperID = ptempList[0].PingId;
                ViewBag.SurveyPaperList = _surveyExampaperBL.GetServeyExamPaperList(string.Format(" ExampaperID in ('" + ptempList[0].PingId + "') "));
            }

            ViewBag.Course = course;
            return View();
        }

        /// <summary>
        /// 分组受教页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult GroupCourseSeatSistribute(int courseID)
        {
            //课程信息
            var course = _courseBL.GetSingleCourse(courseID);
            var list = _courseRoomRule.GetNew_CourseRoomRuleListByCourseId(courseID, string.Format(" Type=1"));
            //时间段集合
            ViewBag.DateList = new List<New_CourseRoomRule>();
            if (list.Count > 0)
            {
                ViewBag.DateList = list.GroupBy(p => p.RoomId).ToList()[0].ToList();
            }
            //课程已选班级
            //ViewBag.ClassList = _newclass.GetClassList(string.Format(" Id in ({0}) ", course.Classes));

            //评估问卷
            ViewBag.SurveyPaperList = new List<Survey_Exampaper>();
            ViewBag.SurveyPaperID = 0;
            var ptempList = list.Where(p => p.Type == 0).ToList();
            if (ptempList.Count > 0)
            {
                ViewBag.SurveyPaperID = ptempList[0].PingId;
                ViewBag.SurveyPaperList = _surveyExampaperBL.GetServeyExamPaperList(string.Format(" ExampaperID in ('" + ptempList[0].PingId + "') "));
            }

            //统计人数
            //var groupUser = _newGroupUser.GetList(string.Format(" ClassId in (select id from dbo.F_SplitIDs('{0}'))", course.Classes));
            //ViewBag.ClassCount = course.Classes.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries).Count();
            //ViewBag.GroupCount = groupUser.GroupBy(p => p.GroupId).Count();
            //ViewBag.PersonCount = groupUser.Count;

            ViewBag.Course = course;
            return View();
        }

        /// <summary>
        /// 选择班级
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectClass()
        {
            return View();
        }

        /// <summary>
        /// 新员工集中课程
        /// </summary>
        /// <returns></returns>
        public ActionResult EditNewCourseTogether(int courseID=0)
        {
            var co = new New_Course();
            //已选讲师集合
            ViewBag.TeachList = new List<Sys_User>();
            ViewBag.ClassList = new List<New_Class>();
            ViewBag.SurveyPaperList = new List<Survey_Exampaper>();
            ViewBag.PaperName = "";
            ViewBag.PaperTotalScore = 0;
            ViewBag.CourseResourceList = new List<New_CourseFiles>();
            ViewBag.CourseDateList = new List<New_CourseRoomRule>();
            var coursePaper = new New_CoursePaper
                                  {
                                      Hour = Convert.ToInt32(AllSystemConfigs.Find(s => s.ConfigType == 26).ConfigValue.Split(';')[0]),//课程结束后 多久时间内 允许考试
                                      Length = Convert.ToInt32(AllSystemConfigs.Find(s => s.ConfigType == 26).ConfigValue.Split(';')[1]),//考试时长
                                      TestTimes = Convert.ToInt32(AllSystemConfigs.Find(s => s.ConfigType == 27).ConfigValue) //测试次数
                                  };

            if(courseID>0)
            {
                co = _courseBL.GetSingleCourse(courseID);

                //讲师
                if (!string.IsNullOrEmpty(co.Teachers))
                {
                    ViewBag.TeachList = _userBL.GetList(" UserId in (select id from dbo.F_SplitIDs('" + co.Teachers + "'))");
                }

                //班级
                if (!string.IsNullOrEmpty(co.Classes))
                {
                    ViewBag.ClassList = _newclass.GetClassList(string.Format(" Id in (select id from dbo.F_SplitIDs('" + co.Classes + "'))"));
                }

                //评估问卷
                if (!string.IsNullOrEmpty(co.IsPingTeacher))
                {
                    ViewBag.SurveyPaperList = _surveyExampaperBL.GetServeyExamPaperList(string.Format(" ExampaperID in (select id from dbo.F_SplitIDs('" + co.IsPingTeacher + "'))"));
                }

                //课程资源
                var listResource = _courseResourceBL.GetCourseResourceList(courseID);
                ViewBag.CourseResourceList = listResource.Where(c => c.Type == 0);//资源

                //课后测试试卷
                var coPaperList = _newCoursePaper.GetCoursePaper(courseID);
                if(coPaperList.Count>0)
                {
                    coursePaper = coPaperList.FirstOrDefault();
                    var exampaper = _exampaperBL.GetExampaper(coursePaper.PaperId);
                    //TODO: 保存 试卷名称 以及试卷总分
                    if (exampaper != null && exampaper.Status != 1)
                    {
                        ViewBag.PaperName = exampaper.ExampaperTitle;
                        ViewBag.PaperTotalScore = exampaper.ExampaperScore;
                    }
                }
            }
            ViewBag.CoursePaper = coursePaper;

            return View(co);
        }

        /// <summary>
        /// 集中授课详情页面呈现
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult NewCourseTogetherDetial(int Id)
        {
            var course = _courseBL.GetSingleCourse(Id);
            if (course == null || course.IsDelete == 1)
            {
                course = new New_Course();
                ViewBag.MsgTips = "该课程不存在或已被删除！";
            }
            else
            {
                List<New_CourseFiles> listResource = _courseResourceBL.GetCourseResourceList(Id);
                ViewBag.CourseResourceList = listResource.Where(c => c.Type == 0);//资源
                //在线测试
                if (course.IsTest == 1)
                {
                    var coPaperList = _newCoursePaper.GetCoursePaper(Id);
                    ViewBag.CoursePaper = coPaperList.FirstOrDefault();
                    if (ViewBag.CoursePaper == null)
                    {
                        ViewBag.CoursePaper = new New_CoursePaper();
                        course.IsTest = 0;
                    }
                    else
                    {
                        LiXinModels.Examination.DBModel.tbExampaper exampaper = _exampaperBL.GetExampaper(ViewBag.CoursePaper.PaperId);
                        //TODO: 保存 试卷名称 以及试卷总分
                        if (exampaper != null && exampaper.Status != 1)
                        {
                            ViewBag.PaperName = exampaper.ExampaperTitle;
                            ViewBag.PaperTotalScore = exampaper.ExampaperScore;
                        }
                        else
                        {
                            course.IsTest = 0;
                        }
                    }
                }
            }
            return View(course);
        }

        /// <summary>
        /// 集中授课详情
        /// </summary>
        /// <returns></returns>
        public ActionResult NewCourseRoom()
        {
            return View();
        }

        /// <summary>
        /// 问卷
        /// </summary>
        /// <returns></returns>
        public ActionResult NewCourseSurvey()
        {
            return View();
        }

        /// <summary>
        /// 课程详情人员调整页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseSubCourseList(int courseID=0)
        {
            //课程信息
            ViewBag.Course = _courseBL.GetSingleCourse(courseID);
            ViewBag.RoomList = _courseRoomRule.GetNew_CourseRoomRuleListByCourseId(courseID);
            return View();
        }

        /// <summary>
        /// 问卷详情
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        public ActionResult SurveyDetial(int surveyId)
        {
            var afterCourseAssess = _surveyExampaperBL.GetSurveyExampaper(surveyId);
            return View(afterCourseAssess);
        }

        /// <summary>
        /// 显示作为详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowUserSeatDetail(int id)
        {
            var rule = _courseRoomRule.GetNew_CourseRoomRule(id);
            ViewBag.roomRule = "";
            ViewBag.room = new New_ClassRoom();
            if (rule != null)
            {
                ViewBag.roomRule = rule.SeatDetail;
                ViewBag.room = _newClassRoom.GetRoom(rule.RoomId);
            }
            ViewBag.UserID = CurrentUser.UserId;
            return View();
        }

        #region 座位导出

        /// <summary>
        /// 导出座位
        /// </summary>
        /// <param name="id"></param>
        /// <param name="flag">2：讲台在下导出；3：讲台在上导出</param>
        public void ExportSeat(int id,int flag=2)
        {
            var rule = _courseRoomRule.GetNew_CourseRoomRule(id);
            var userIDs = rule.SeatDetail.Split(':').Where(str => !string.IsNullOrEmpty(str)).Select(str => str.Split(',')[2].StringToInt32()).Where(uid => uid > 0).ToList();
            //课程信息
            var course = _courseBL.GetSingleCourse(rule.CourseId);

            //教室安排的人员
            var userList = _courseRoomRule.GetUserSeatDetail(string.Join(",",userIDs));
            //教室
            var room = _newClassRoom.GetRoom(rule.RoomId);
            var row = room.RowNumber;//行
            var col = room.ColumnNumber;//列

            //组装table
            var outTable = new DataTable();
            //添加第一行：课程信息
            outTable.Columns.Add(" ");
            outTable.Columns.Add("课程名称");
            outTable.Columns.Add(course.CourseName);
            outTable.Columns.Add("  ");
            outTable.Columns.Add("上课时间");
            outTable.Columns.Add(rule.StartTime.ToString("yyyy-MM-dd HH:mm"));
            outTable.Columns.Add("    ");
            outTable.Columns.Add("下课时间");
            outTable.Columns.Add(rule.EndTime.ToString("yyyy-MM-dd HH:mm"));
            outTable.Columns.Add("     ");
            outTable.Columns.Add("教室");
            outTable.Columns.Add(room.RoomName);
            for (var i = 0; i < col - 10; i++)
            {
                var str = "";
                for (var j = 0; j < 6 + i; j++)
                {
                    str += " ";
                }
                outTable.Columns.Add(str);
            }
            var maxCol = (col + 2) > 12 ? (col + 2) : 12;
            //讲台在上导出
            if (flag==3)
            {
                var jiangtai = new List<string>();
                for (var i = 0; i < maxCol; i++)
                {
                    jiangtai.Add(i == maxCol/2 ? "讲台" : "");
                }
                outTable.Rows.Add(jiangtai.ToArray());
            }

            #region 座位信息

            var seatdic = GetSeatUserID(rule.SeatDetail);
            if (flag == 2)
            {
                //讲台在下
                for(var i=row-1;i>=0;i--)
                {
                    var rowseat = new List<string>();
                    for(var j=maxCol-1;j>=0;j--)
                    {
                        if(j==maxCol-1)
                            rowseat.Add((i + 1) + "行");
                        else if (j == 0)
                            rowseat.Add((i + 1) + "行");
                        else
                        {
                            var po = i + "," + (j-1);
                            rowseat.Add(seatdic.Keys.Contains(po) ? UserInformation(userList, seatdic[po]) : " ");
                        }
                    }
                    outTable.Rows.Add(rowseat.ToArray());
                }
            }
            else
            {
                //讲台在上
                for (var i = 0; i < row; i++)
                {
                    var rowseat = new List<string>();
                    for (var j = 0; j < maxCol; j++)
                    {
                        if (j == maxCol - 1)
                            rowseat.Add((i+1) + "行");
                        else if (j == 0)
                            rowseat.Add((i + 1) + "行");
                        else
                        {
                            var po = i + "," + (j-1);
                            rowseat.Add(seatdic.Keys.Contains(po) ? UserInformation(userList, seatdic[po]) : " ");
                        }
                    }
                    outTable.Rows.Add(rowseat.ToArray());
                }
            }

            #endregion


            //讲台在下导出
            if (flag == 2)
            {
                var jiangtai = new List<string>();
                for (var i = 0; i < maxCol; i++)
                {
                    jiangtai.Add(i == maxCol / 2 ? "讲台" : "");
                }
                outTable.Rows.Add(jiangtai.ToArray());
            }
            new Spreadsheet().Template("座位安排", null, outTable, HttpContext, "座位安排预览", "座位安排预览");
        }

        /// <summary>
        /// 根据坐标获取人员ID
        /// </summary>
        /// <param name="seatDetail"></param>
        /// <returns></returns>
        private Dictionary<string,int> GetSeatUserID(string seatDetail)
        {
            return seatDetail.Split(':').Select(str => str.Split(',')).Where(strarr => Convert.ToInt32(strarr[2]) > 0).ToDictionary(strarr => strarr[0] + "," + strarr[1], strarr => Convert.ToInt32(strarr[2]));
        }

        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <param name="userList"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        private string UserInformation(IEnumerable<UserSeatModel> userList,int userID )
        {
            var user= userList.FirstOrDefault(p => p.UserId == userID);
            return user!=null ? string.Format("{0}-{1}-{2}\n{3}\n{4},{5}",user.ClassNo??"",user.GroupNo??"",user.NumberID??"",user.DeptName??"",user.Sex==0?"男":"女",user.UserName) : "";
        }

        #endregion


        #endregion

        #region 方法

        /// <summary>
        /// 获取教室组人员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetRoomRuleUser(int id, string name = "", int status = -1, int pageSize = int.MaxValue, int pageIndex = 1, int pflag = 0)
        {
            var newList = new List<object>();
            var listcount = 0;
            var userStr = "";
            var havedcount = 0;
            if (pflag == 1)
            {
                var totalCount = 0;
                var roomUser = _courseOrderDetail.GetCourseOrderDetailSeatUserDetail(out totalCount,string.Format(" a.SubCourseID={0} ", id));
                userStr = roomUser.Count > 0 ? string.Join(",", roomUser.Select(p => p.UserId)) : "";
                var searchResult =roomUser.Where(p =>(string.IsNullOrEmpty(name) || (p.Realname.Contains(name))) &&(status == -1 || p.IsNew == status));
                var newRoomUser = searchResult.Skip((pageIndex - 1)*pageSize).Take(pageSize).ToList();
                newRoomUser.ForEach(p => newList.Add(new
                                                         {
                                                             Id = id,
                                                             UserID = p.UserId,
                                                             RealName = p.Realname,
                                                             p.Sex,
                                                             //JoinDate=p.JoinDate.ToString("yyyy-MM-dd HH:mm"),
                                                             School = p.GraduateSchool,
                                                             IsLeave = p.IsNew
                                                         }));
                havedcount = roomUser.Count(p => p.IsNew == 0);
                listcount = searchResult.Count();
            }
            else
            {
                var roomrule = _courseRoomRule.GetNew_CourseRoomRule(id);
                var newids = roomrule.SeatDetail.Split(':').Where(str => str != "" && str.Split(',')[2].StringToInt32() > 0).Aggregate("", (current, str) => current + (current == "" ? (str.Split(',')[2]) : ("," + str.Split(',')[2])));
                var userlist= _userBL.GetList(string.Format(" UserId in ({0})",newids));
                havedcount = userlist.Count;
                var searchResult =userlist.Where(p =>(string.IsNullOrEmpty(name) || (p.Realname.Contains(name))) &&(status == -1 || status==0));
                listcount = searchResult.Count();
                var newRoomUser = searchResult.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                newRoomUser.ForEach(p => newList.Add(new
                {
                    Id = id,
                    UserID = p.UserId,
                    RealName = p.Realname,
                    p.Sex,
                    //JoinDate=p.JoinDate.ToString("yyyy-MM-dd HH:mm"),
                    School = p.GraduateSchool,
                    IsLeave = 0
                }));
            }
            return Json(new
            {
                dataList = newList,
                recordCount = listcount,
                haveduser = userStr,
                realCount = havedcount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取班级集合
        /// </summary>
        /// <param name="realName">班级名称</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页</param>
        /// <returns></returns>
        public JsonResult GetClassList(string realName = "", int pageSize = int.MaxValue, int pageIndex = 1)
        {
            var totalCount = 0;
            var list = _newclass.GetClassList(out totalCount,
                                               string.Format(" ClassName like '%{0}%' and Year={1} and IsDoDelete=0 ", realName.ReplaceSql(), DateTime.Now.Year),
                                               (pageIndex - 1) * pageSize, pageSize);
            return Json(new
            {
                dataList = list,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 判断时间有效性
        /// </summary>
        /// <param name="starttime"></param>
        /// <returns></returns>
        public JsonResult JudgeTimeRight(string starttime)
        {
            try
            {
                var time = Convert.ToDateTime(starttime);
                return Json(time<DateTime.Now ? new { result = 0, message = "开始时间要大于当前时间" } : new { result = 1, message = "" }, JsonRequestBehavior.AllowGet);
            }catch
            {
                return Json(new {result = 0, message = "时间格式错误"}, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 提交课程
        /// </summary>
        /// <returns></returns>

        public JsonResult SubmitCourse(int id, string name, string code, int groupteach, string starttime, string endtime, string teacher, string classes, string survey, string test, string memo, int way,string delFile)
        {
            try
            {
                var course = new New_Course();
                var testarr = test.Split(',');
                if (id > 0)
                {
                    course = _courseBL.GetSingleCourse(id);
                }else
                {
                    course.EndTime = Convert.ToDateTime(endtime);
                    course.StartTime = Convert.ToDateTime(starttime);
                }
                course.IsTest = (!string.IsNullOrEmpty(testarr[0]) && Convert.ToInt32(testarr[0])>0) ? 1 : 0;
                course.Classes = classes;
                course.Code = code;
                course.CourseName = name;
                course.IsGroupTeach = groupteach;
                course.Teachers = teacher;
                course.Memo = memo;
                if (way == 1)
                {
                    course.IsPingTeacher = survey;
                    course.IsPingCourse = 0;
                }
                else
                {
                    course.IsPingTeacher ="";
                    course.IsPingCourse = Convert.ToInt32(survey);
                }
                
                if (id > 0)
                {
                    _courseBL.UpdateCourse(course);
                }
                else
                {
                    course.GGroupNumber = 0;
                    course.GGroupPersonCount = 0;
                    course.GGroupRule = "";
                    course.GStartTime = "";
                    course.GType = 0;
                    course.Way = way;
                    course.LastUpdateTime = DateTime.Now;
                    course.PublicFlag = 0;
                    course.IsDelete = 0;
                    course.ScoreDistribute = "";
                    _courseBL.AddCourse(course);
                }

                //删除课程附件
                if (!string.IsNullOrEmpty(delFile))
                    _courseResourceBL.DeleteCourseFiles(delFile);

                //修改测试
                _newCoursePaper.DeleteCoursePaper(course.Id);
                
                var coursepaper = new New_CoursePaper
                                      {
                                          CourseId = course.Id,
                                          PaperId = Convert.ToInt32(testarr.Length > 0 ? testarr[0] : "0"),
                                          Hour = Convert.ToInt32(testarr.Length > 1 ? testarr[1] : "0"),
                                          Length = Convert.ToInt32(testarr.Length > 2 ? testarr[2] : "0"),
                                          LevelScore = Convert.ToInt32(testarr.Length > 3 ? testarr[3] : "0"),
                                          TestTimes = Convert.ToInt32(testarr.Length > 4 ? testarr[4] : "0")
                                      };
                _newCoursePaper.AddCoursePaper(coursepaper);

                //修改课程类型
                if (id>0 &&(course.IsGroupTeach == 0 || course.IsGroupTeach == 2))
                {
                    //删除集中授课
                    _courseRoomRule.DeleteCourseRoomRule(id, 0);
                }
                if (id > 0 && (course.IsGroupTeach == 0 || course.IsGroupTeach == 1))
                {
                    //删除分组带教
                    _courseRoomRule.DeleteCourseRoomRule(id, 1);
                }
                return Json(new {result = 1, id = course.Id}, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new {result = 0, id = 0}, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存课程资源
        /// </summary>
        /// <returns></returns>
        public ContentResult SubmitCourseResource(New_CourseFiles model)
        {
            model.CreateDate = DateTime.Now;
            model.IsDelete = 0;
            model.LoadTimes = 0;
            model.PackId = 0;
            //model.ResourceSize = 0;
            _newCourseFiles.AddCourseFiles(model);
            return Content(model.Id > 0 ? "1" : "0");
        }

        /// <summary>
        /// 获取课程中集中授课的教室安排的集合
        /// </summary>
        /// courseID：课程ID
        /// type：0：集中；1：分组带教
        /// <returns></returns>
        public JsonResult GetRoomSeatList(int courseID,int type)
        {
            var co = _courseBL.GetSingleCourse(courseID);
            var teachList = new List<object>();
            //课程的讲师
            if (!string.IsNullOrEmpty(co.Teachers))
            {
                var totalCount = 0;
                var list = _userBL.GetAllTeacher(out totalCount); ;
                list.ForEach(p=> teachList.Add(new{id=p.UserId,name=p.Realname}));
            }
            //教室的集合
            var roomList = new List<object>();
            var tempRoomList = _newClassRoom.GetRoomList();
            tempRoomList.ForEach(p => roomList.Add(new { id = p.Id,disSeats=p.DisSeat, name = p.RoomName, row = p.RowNumber, col = p.ColumnNumber, number = p.RoomNumber > (p.RowNumber * p.ColumnNumber) ? p.RowNumber * p.ColumnNumber : p.RoomNumber }));
            //集中授课集合
            var roomSeatList = new List<object>();
            var itemlist = _courseRoomRule.GetNew_CourseRoomRuleListByCourseId(courseID, string.Format(" Type={0}", type));
            var tRoomList = new List<int>();
            itemlist.OrderBy(p => p.Id).ToList().ForEach(p =>
                                       {
                                           if (!tRoomList.Contains(p.RoomId))
                                           {
                                               tRoomList.Add(p.RoomId);
                                               var tempdate = new List<object>();
                                               var temproom = tempRoomList.FirstOrDefault(pr => pr.Id == p.RoomId);
                                               itemlist.Where(pa => pa.RoomId == p.RoomId).ToList().ForEach(pa => tempdate.Add(new
                                                                                                                                    {
                                                                                                                                        id = pa.Id,
                                                                                                                                        pflag=co.PublicFlag,
                                                                                                                                        starttime = pa.StartTime.ToString("yyyy-MM-dd HH:mm"),
                                                                                                                                        endtime = pa.EndTime.ToString("yyyy-MM-dd HH:mm"),
                                                                                                                                        teacher = pa.TeacherId,
                                                                                                                                        realName=pa.teachername,
                                                                                                                                        surveyID=pa.PingId,
                                                                                                                                        attFlag=pa.IsAttFlag,
                                                                                                                                        AfterEvlutionConfigTime = pa.AfterEvlutionConfigTime
                                                                                                                                    }));
                                               roomSeatList.Add(new
                                                                   {
                                                                       id=p.Id,
                                                                       pflag = co.PublicFlag,
                                                                       roomID=p.RoomId,
                                                                       roomName=p.RoomName,
                                                                       row=temproom.RowNumber,
                                                                       col=temproom.ColumnNumber,
                                                                       perCount=temproom.PrePersonCount,
                                                                       seatType=temproom.SeatType,
                                                                       rule=p.Rules,
                                                                       seatDetail=p.SeatDetail,
                                                                       classorder=p.ClassIDs, 
                                                                       tempdate,
                                                                       percount=p.PersonCount,
                                                                       name=p.GroupName
                                                                   });
                                           }
                                       });
            //总人数
            var personCount = 0;// _newGroupUser.GetList(string.Format(" ClassId in (select id from dbo.F_SplitIDs('{0}'))", co.Classes)).Count;

            return Json(new { courseID, teachList, roomList, roomSeatList, personCount }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 判断时间是否冲突
        /// </summary>
        /// <param name="dstr">时间字符串</param>
        /// <param name="flag">0:新增时间验证；1：修改时间验证</param>
        /// <returns></returns>
        public JsonResult JudgeDateScopeRight(string dstr,int flag=0)
        {
            var datearr = dstr.Split(';');
            var mstart = flag == 0 ? DateTime.Now.Date : Convert.ToDateTime("2000-1-1");
            var mend = Convert.ToDateTime("2050-1-1");
            var falg = true;
            var dateList = new List<DateCompare>();
            for(var i=1;i<datearr.Length;i++)
            {
                if (datearr[i].Trim() != "")
                {
                    var tempstart = Convert.ToDateTime(datearr[i].Split(',')[0]);
                    var tempend = Convert.ToDateTime(datearr[i].Split(',')[1]);
                    //比较是否开始时间大于结束时间
                    if(tempstart>=tempend)
                    {
                        falg = false;
                        break;
                    }
                    //比较是否在总的时间范围内
                    if(!(tempstart>=mstart && tempend<=mend))
                    {
                        falg = false;
                        break;
                    }
                    //比较是否与其他时间冲突
                    if (dateList.Count > 0)
                    {
                        var subFlag = dateList.All(date => (tempstart > date.EndTime || tempend < date.StartTime));
                        if(!subFlag)
                        {
                            falg = false;
                            break;
                        }
                        dateList.Add(new DateCompare{EndTime=tempend,StartTime=tempstart});
                    }
                    else
                    {
                        dateList.Add(new DateCompare { EndTime = tempend, StartTime = tempstart });
                    }
                }
            }
            return falg ? Json(new{result=1,message=""}, JsonRequestBehavior.AllowGet) : Json(new{result=0,message="时间设置错误！"}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取新进员工的问卷
        /// </summary>
        /// <returns></returns>
        public JsonResult GetNewSurveyPaper()
        {
            var list = _surveyExampaperBL.GetServeyExamPaperList(string.Format(" Survey_Exampaper.IsDelete=0 and Survey_Exampaper.ExamType=3 "));
            var newList = new List<object>();
            list.ForEach(p=> newList.Add(new{id=p.ExampaperID,name=p.ExamTitle}));
            return Json(newList, JsonRequestBehavior.AllowGet);
        }

        #region 分组带教分组方法

        /// <summary>
        /// 分组带教自由重组人员
        /// </summary>
        /// <param name="classIDs">课程中班级ID集合</param>
        /// <param name="type">0：分组数；1：每组人数</param>
        /// <param name="count">分组数/每组人数</param>
        /// <param name="rule">规则()</param>
        /// <returns></returns>
        public JsonResult GetAutoGroupPerson(string classIDs, int type, int count, string rule="0,0,0,0")
        {
            if(string.IsNullOrEmpty(classIDs))
            {
                return Json(new{result=0,message="分组失败",dataList=""}, JsonRequestBehavior.AllowGet);
            }
            #region 排序条件
            var ordersql = "";
            if(string.IsNullOrEmpty(rule))
            {
                var rulearr = rule.Split(',');
                for(var i=0;i<rulearr.Length;i++)
                {
                    if(rule[i]=='1')
                    {
                        switch(i)
                        {
                            case 0://性别
                                ordersql += ordersql == "" ? (" u.Sex ") : (" ,u.Sex ");
                                break;
                            case 1://实习经验
                                ordersql += ordersql == "" ? (" u.IsInternExp ") : (" ,u.IsInternExp ");
                                break;
                            case 2://院校
                                ordersql += ordersql == "" ? (" u.GraduateSchool ") : (" ,u.GraduateSchool ");
                                break;
                            case 3://专业
                                ordersql += ordersql == "" ? (" u.Major ") : (" ,u.Major ");
                                break;
                        }
                    }
                }
            }
            #endregion
            //获取人员
            var classUser =
                _newGroupUser.GetClassUserList(
                    string.Format(" and a.ClassId in (select id from dbo.F_SplitIDs('{0}'))", classIDs),
                    string.Format(" order by  {0} ", ordersql == "" ? " u.UserId " : ordersql));
            if (classUser.Count <= 0)
            {
                return Json(new {result = 0, message = "所选班级没有人", dataList = ""}, JsonRequestBehavior.AllowGet);
            }
            var totalList = new List<List<int>>();
            var newgroupcount = count;
            if (type == 1)
            {
                //按每组人数
                newgroupcount = (int) Math.Ceiling((double) classUser.Count()/count); //计算要分几个组
            }
            for (var i = 0; i < newgroupcount; i++)
            {
                totalList.Add(new List<int>());
            }
            for (var i = 0; i < classUser.Count; i++)
            {
                totalList[i % newgroupcount].Add(classUser[i].UserId);
            }
            return Json(new {result = 1, message = "", dataList = totalList}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取课程中班级及其小组
        /// </summary>
        /// <param name="classlist">班级集合</param>
        /// <returns></returns>
        public JsonResult GetClassGroupList(string classlist)
        {
            if(string.IsNullOrEmpty(classlist))
            {
                return Json(new {result = 0, message = "课程内没有班级！", dataList = ""},JsonRequestBehavior.AllowGet);
            }

            //获取班级人员
            var classUser = _newGroupUser.GetList(string.Format(" ClassId in (select id from dbo.F_SplitIDs('{0}'))", classlist));
            //获取班级
            var classList = _newclass.GetList(string.Format("  and c.Id in (select id from dbo.F_SplitIDs('{0}')) ", classlist));
            //获取班级里的小组
            var groupList = _newGroup.GetList(string.Format(" and g.ClassId in (select id from dbo.F_SplitIDs('{0}')) ", classlist));

            var dataList = new List<object>();
            foreach (var cl in classList.OrderBy(p => p.ClassNo))
            {
                var className = cl.ClassName;
                var classID = cl.Id;
                var tgList = new List<object>();

                foreach(var gr in groupList.Where(p=>p.ClassId==cl.Id).OrderBy(p=>p.GroupIndex))
                {
                    var groupName = gr.GroupName;
                    var groupID = gr.Id;
                    var perCount = classUser.Count(p => p.GroupId == gr.Id && p.ClassId == cl.Id);
                    var sign = cl.Id + "-" + gr.Id;
                    tgList.Add(new { groupName, groupID, perCount, sign });
                }

                dataList.Add(new{classID,className,tgList});
            }
            return Json(new { result = 1, message = "", dataList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分组带教现有班级分组人员
        /// </summary>
        /// <param name="groupIDs">班级小组ID集合</param>
        /// <param name="classlist">班级集合</param>
        /// <returns></returns>
        public JsonResult GetGroupTeachPerson(string groupIDs, string classlist)
        {
            if (string.IsNullOrEmpty(groupIDs))
                return Json(new { result = 0, message = "没有可分配的小组！", dataList = "" }, JsonRequestBehavior.AllowGet);
            if (string.IsNullOrEmpty(classlist))
                return Json(new { result = 0, message = "没有可分配的班级！", dataList = "" }, JsonRequestBehavior.AllowGet);
            //获取人员
            var classUser = _newGroupUser.GetClassUserList(string.Format(" and a.ClassId in (select id from dbo.F_SplitIDs('{0}'))", classlist));
            if (classUser.Count <= 0)
                return Json(new { result = 0, message = "所选班级没有人！", dataList = "" }, JsonRequestBehavior.AllowGet);
            var totalList = new List<List<int>>();
            var carr = groupIDs.Split(new string[] {";"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string t in carr)
            {
                var temp = new List<int>();
                var gr = t.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                classUser.Where(p=>gr.Contains(p.GroupId.ToString())).ToList().ForEach(p=> temp.Add(p.UserId));
                totalList.Add(temp);
            }
            return Json(new { result = 1, message = "", dataList = totalList }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        /// <summary>
        /// 保存集中授课和分组带教
        /// </summary>
        /// <param name="courseID">课程ID</param>
        /// <param name="type">0:集中授课；1：分组带教</param>
        /// <param name="publicFlag">是否发布：0：未发布；1：已发布</param>
        /// <param name="detail">设置的详细信息</param>
        /// <param name="rtype">设置的详细信息</param>
        /// <returns></returns>
         [Filter.SystemLog("新员工 编辑课程", LogLevel.Info)]
        public JsonResult SaveSubCourse(int courseID,int type,int publicFlag,string detail)
        {
            var compareDate = new List<DateCompare>();
            var co = _courseBL.GetSingleCourse(courseID);
            var roomArr = detail.Split(new string[] {"!"}, StringSplitOptions.RemoveEmptyEntries);
            if(roomArr.Length>0)
            {
                //删除原来的
                if (publicFlag==1 || (publicFlag==0 && _courseRoomRule.DeleteCourseRoomRule(courseID, type)))
                {
                    for (var i = 0; i < roomArr.Length; i++)
                    {
                        var singleArr = roomArr[i].Split(';');
                        var roomID = Convert.ToInt32(singleArr[0]); //教室ID
                        var rule = singleArr[2]; //规则
                        var seats = singleArr[3]; //排座详情
                        var personCount = Convert.ToInt32(singleArr[4]); //人数
                        var dateArr = singleArr[1].Split(new string[] {"|"}, StringSplitOptions.RemoveEmptyEntries);
                        var groupName = ((char) (65 + i) + "组");
                        foreach (var t in dateArr)
                        {
                            var datearr = t.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
                            var sdate = Convert.ToDateTime(datearr[0]); //开始时间
                            var edate = Convert.ToDateTime(datearr[1]); //结束时间
                            var teacherID = Convert.ToInt32(datearr[2]); //讲师ID
                            var surveyID = Convert.ToInt32(datearr[3]); //问卷ID
                            var attFlag = Convert.ToInt32(datearr[4]); //考勤方式
                            var afterTime = Convert.ToDecimal(datearr[5]); //评估时间
                            var roomruleID = Convert.ToInt32(datearr[6]); //教室组ID
                            compareDate.Add(new DateCompare {StartTime = sdate, EndTime = edate});//添加到集合里面
                            var classRoom = new New_CourseRoomRule
                                                {
                                                    Id=roomruleID,
                                                    ClassIDs = "",
                                                    RoomId=roomID,
                                                    CourseId = courseID,
                                                    EndTime = edate,
                                                    PingId = surveyID,
                                                    Rules = rule,
                                                    SeatDetail = seats,
                                                    StartTime = sdate,
                                                    TeacherId = teacherID,
                                                    Type = type,
                                                    PersonCount = personCount,
                                                    GroupName=groupName,
                                                    IsAttFlag = attFlag,
                                                    AfterEvlutionConfigTime=afterTime
                                                };
                            if (roomruleID == 0 || publicFlag == 0)
                            {
                                _courseRoomRule.AddNew_CourseRoomRule(classRoom);
                            }else
                            {
                                _courseRoomRule.UpdateCourseRoomRule(classRoom);
                            }
                        }
                    }
                    //修改课程的全局时间
                    var newCourseDate = GetCourseDate(co.Id);
                    if (co.StartTime != newCourseDate.StartTime || co.EndTime != newCourseDate.EndTime)
                    {
                        co.StartTime = newCourseDate.StartTime;
                        co.EndTime = newCourseDate.EndTime;
                        _courseBL.UpdateCourse(co);
                    }
                }else
                {
                    return Json(new {result = 0, message = "保存失败！"}, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = 1, message = "保存成功！" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取课程的全局时间
        /// </summary>
        /// <param name="courseID">课程ID</param>
        /// <returns></returns>
        protected DateCompare GetCourseDate(int courseID)
        {
            var coRoomlist = _courseRoomRule.GetNew_CourseRoomRuleListByCourseId(courseID);
            var stime = Convert.ToDateTime("2000-1-1");
            var etime = Convert.ToDateTime("2050-1-1");
            foreach (var d in coRoomlist)
            {
                if (stime == Convert.ToDateTime("2000-1-1") || d.StartTime < stime)
                    stime = d.StartTime;
                if (etime == Convert.ToDateTime("2050-1-1") || d.EndTime > etime)
                    etime = d.EndTime;
            }
            return new DateCompare {StartTime = stime, EndTime = etime};
        }

        /// <summary>
        /// 获取课程中教室安排的集合
        /// </summary>
        /// courseID：课程ID
        /// type：0：集中；1：分组带教
        /// <returns></returns>
        public JsonResult GetCoRoomRuleList(int courseID, int type)
        {
            //教室分配规则集合
            var roomSeatList = new List<object>();
            var CoRoomlist = _courseRoomRule.GetNew_CourseRoomRuleListByCourseId(courseID, string.Format(" Type={0}", type));
            var tRoomList = new List<int>();
            CoRoomlist.OrderBy(p => p.Id).ToList().ForEach(p =>
            {
                if (!tRoomList.Contains(p.RoomId))
                {
                    tRoomList.Add(p.RoomId);
                    var tempdate = new List<object>();
                    CoRoomlist.Where(pa => pa.RoomId == p.RoomId).ToList().ForEach(pa => tempdate.Add(new
                    {
                        starttime = pa.StartTime.ToString("yyyy-MM-dd HH:mm"),
                        endtime = pa.EndTime.ToString("yyyy-MM-dd HH:mm"),
                        teacher = pa.TeacherId,
                        surveyID = pa.PingId,
                        teachername=pa.teachername
                    }));
                    roomSeatList.Add(new
                    {
                        id = p.Id,
                        roomID = p.RoomId,
                        rule = p.Rules,
                        seatDetail = p.SeatDetail,
                        classorder = p.ClassIDs,
                        Roomname=p.RoomName,
                        userCount=p.PersonCount,
                        tempdate
                    });
                }
            });

            return Json(new {roomSeatList}, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 添加新的学员
        /// </summary>
        /// <param name="id">教室课程ID</param>
        /// <param name="user">新增人员</param>
        /// <returns></returns>
        public JsonResult AddNewPerson(int id,string user="")
        {
            try
            {
                var roomRule = _courseRoomRule.GetNew_CourseRoomRule(id);
                var userlist = _userBL.GetList(string.Format(" UserId in ({0})", user));
                var newseatstr = "";
                foreach (var str in roomRule.SeatDetail.Split(':'))
                {
                    if (userlist.Count > 0)
                    {
                        var uid = 0;
                        if (roomRule.Type == 0)
                        {
                            uid = Convert.ToInt32(str.Split(',')[2]);
                        }
                        else
                        {
                            uid = Convert.ToInt32(str.Split(',')[3]);
                        }
                        
                        if (uid == 0)
                        {
                            var obj = userlist[0];
                            //新的字符串
                            var newstr = "";

                            //区分集中和分组带教 格式
                            if (roomRule.Type == 0)
                            {
                                newstr = str.Split(',')[0] + "," + str.Split(',')[1] + "," + obj.UserId + "," +
                                            obj.Realname + "," + (obj.Sex == 0 ? "男" : "女");
                            }
                            else
                            {
                                newstr = str.Split(',')[0] + "," + str.Split(',')[1] + "," + str.Split(',')[2] +","+ obj.UserId + "," +
                                                                           obj.Realname + "," + (obj.Sex == 0 ? "男" : "女");
                            }
                            newseatstr += newseatstr == "" ? newstr : (":" + newstr);
                            //判断此人是否有学习记录
                            if (
                                _newCourseOrder.Get(string.Format(" CourseId={0} and UserId={1} ", roomRule.CourseId,
                                                                  obj.UserId)).Count == 0)
                            {
                                var groupuser = _newGroupUser.GetModelByUserID(obj.UserId);
                                _newCourseOrder.InsertNew_CourseOrder(new New_CourseOrder
                                                                          {
                                                                              CourseId = roomRule.CourseId,
                                                                              ClassId = groupuser == null? 0 : groupuser.ClassId,
                                                                              UserId = obj.UserId,
                                                                              OrderTime = DateTime.Now,
                                                                              LearnStatus = 0,
                                                                              TogetherScore = 0,
                                                                              GroupScore = 0,
                                                                              CourseExamScore = 0,
                                                                              ExamScore = 0,
                                                                              TogetherMemo = "",
                                                                              GroupMemo = "",
                                                                              Type = 0,
                                                                              CourseExamSumScore = 0,
                                                                              ExamSumScore = 0
                                                                          });
                            }
                            //判断添加此人的学习记录
                            if (
                                _courseOrderDetail.GetSearchResult(string.Format(" SubCourseID={0} and UserId={1} ",
                                                                                 roomRule.Id, obj.UserId)).Count == 0)
                            {
                                _courseOrderDetail.InsertNew_CourseOrderDetail(new New_CourseOrderDetail
                                                                                   {
                                                                                       CourseId = roomRule.CourseId,
                                                                                       UserId = obj.UserId,
                                                                                       SubCourseID = roomRule.Id,
                                                                                       LearnTime = DateTime.Now,
                                                                                       LearnStatus = 0,
                                                                                       Type = roomRule.Type,
                                                                                       IsLeave = 0
                                                                                   });
                            }
                            userlist.Remove(obj);
                        }
                        else
                        {
                            newseatstr += newseatstr == "" ? str : (":" + str);
                        }
                    }
                    else
                    {
                        newseatstr += newseatstr == "" ? str : (":" + str);
                    }
                }
                //修改教室课程信息
                var personcount=0;
                if (roomRule.Type == 0)
                {
                    personcount = newseatstr.Split(':').Count(s => s.Split(',')[2] != "0" && s.Split(',')[2] != "-1");
                }
                else
                {
                    personcount = newseatstr.Split(':').Count(s => s.Split(',')[3] != "0" && s.Split(',')[3] != "-1");
                }
                roomRule.SeatDetail = newseatstr;
                roomRule.PersonCount = personcount;
                _courseRoomRule.UpdateCourseRoomRule(roomRule);
                return Json(new {result = 1, message = ""}, JsonRequestBehavior.AllowGet);
            }catch
            {
                return Json(new { result = 0, message = "添加失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 提交请假信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <param name="name"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public JsonResult SaveLeaveInfor(int id,int userID,string name,string reason)
        {
            try
            {
                var roomRule = _courseRoomRule.GetNew_CourseRoomRule(id);
                var detail = _courseOrderDetail.GetSearchResult(string.Format(" SubCourseID={0} and UserId={1} ", id, userID)).FirstOrDefault();
                detail.IsLeave = 1;
                detail.ApprovalName = name;
                detail.LeaveReason = reason;
                _courseOrderDetail.UpdateNew_CourseOrderDetail(detail);
                //把位子空出来
                var newseatstr = "";
                foreach (var str in roomRule.SeatDetail.Split(':'))
                {
                    if(str.Split(',')[2]==userID.ToString())
                    {
                        var newstr = str.Split(',')[0] + "," + str.Split(',')[1] + ",0,,";
                        newseatstr += newseatstr == "" ? newstr : (":" + newstr);
                    }else
                    {
                        newseatstr += newseatstr == "" ? str : (":" + str);
                    }
                }
                roomRule.PersonCount = roomRule.PersonCount - 1;
                roomRule.SeatDetail = newseatstr;
                _courseRoomRule.UpdateCourseRoomRule(roomRule);
                return Json(new {result = 1, message = ""}, JsonRequestBehavior.AllowGet);
            }catch
            {
                return Json(new { result = 0, message = "保存失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取教室信息
        /// </summary>
        /// <returns></returns>
        public JsonResult Getroom(int id)
        {
            //教室
            New_ClassRoom roomList = _newClassRoom.GetRoom(id);

            return Json(new { roomList}, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 客户提出的XX要求

        /// <summary>
        /// 批量修改排座信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        public JsonResult SubmitUpdateSeat(int id,string tid="")
        {
            var ruleList = _courseRoomRule.GetNew_CourseRoomRuleListByCourseId(id);
            if (ruleList.Count == 0)
                return Json(new {result = 0, message = "此课程没有排座信息，请确认！"}, JsonRequestBehavior.AllowGet);

            var temjizhong = ruleList.Where(p=>p.Type==0).OrderBy(p => p.Id).ToList();//集中
            var temgroup = ruleList.Where(p => p.Type == 1).OrderBy(p => p.Id).ToList();//分组

            var totalCount = 0;
            var courseList = _courseBL.GetNew_CourseList(out totalCount,
                                                         string.Format(
                                                             " IsDelete=0 and PublicFlag=0 and IsGroupTeach>0 {0}",
                                                             tid.Trim() == "" ? "" : (" and Id in (" + tid + ")")));
            foreach(var coure in courseList)
            {
                var ruList = _courseRoomRule.GetNew_CourseRoomRuleListByCourseId(coure.Id);
                //集中
                var jizhong = ruList.Where(p => p.Type == 0).OrderBy(p => p.Id).ToList();
                if(jizhong.Any())
                {
                    for(var i=0;i< jizhong.Count();i++)
                    {
                        if(i<temjizhong.Count())
                        {
                            var rule = jizhong[i] ;
                            rule.SeatDetail = temjizhong[i].SeatDetail;
                            _courseRoomRule.UpdateCourseRoomRule(rule);
                        }
                    }
                }
                //分组
                var fenzu = ruList.Where(p => p.Type == 1).OrderBy(p => p.Id).ToList();
                if (fenzu.Any())
                {
                    for (var i = 0; i < fenzu.Count(); i++)
                    {
                        if (i < temgroup.Count())
                        {
                            var rule = fenzu[i];
                            rule.SeatDetail = temgroup[i].SeatDetail;
                            _courseRoomRule.UpdateCourseRoomRule(rule);
                        }
                    }
                }
            }
            return Json(new { result = 1, message = "操作成功！" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        /// <summary>
        /// 定义时间类
        /// </summary>
        protected class DateCompare
        {
            /// <summary>
            /// 开始时间
            /// </summary>
            public DateTime StartTime { set; get; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public DateTime EndTime { set; get; }
        }
    }
}
