using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using LiXinModels.User;
using LiXinCommon;
using System.Web;
using System.IO;
using LiXinModels.CourseManage;
using LiXinModels.PlanManage;
using LiXinModels;
using System.Text.RegularExpressions;
using LiXinInterface.SystemManage;

namespace LiXinControllers.User
{
    public partial class UserManageController : BaseController
    {

        #region 呈现页面

        public ActionResult UserInfoComm(int nowyear = -1)
        {

            int uid = CurrentUser == null ? 0 : CurrentUser.UserId;
            Sys_User usermodel = InitSuper;
            int year = nowyear == -1 ? DateTime.Now.Year : nowyear;
            //所内目标学时
            string result = "0";
            //CPA目标学时
            string CPAresult = "0";
            //个人总学时
            decimal userscore = 0;
            //全年补预定次数
            int ordercount = 0;

            decimal CPAscore = 0;

            decimal FreetScore = 0;
            decimal FreeCPAScore = 0;
            if (uid != 0)
            {
                
                //CPA目标学时
                Sys_ParamConfig cpazq1 = AllSystemConfigs.Where(p => p.ConfigType == 16).FirstOrDefault();
                CPAresult = cpazq1.ConfigValue == "" ? "0" : cpazq1.ConfigValue;
                //计算个人学时
                Sys_ParamConfig cpazq2 = AllSystemConfigs.Where(p => p.ConfigType == 14).FirstOrDefault();
                userscore = userBL.GetUserScore(uid, year, cpazq2, CurrentUser.SubordinateSubstation.Contains("上海"));


                //所内目标学时
                usermodel = userBL.GetmodelCAP(uid, year);

                #region 计算CPA有效学时
                //判断 0:部门or 1:分所
                int Isdep = CurrentUser == null ? 0 : (CurrentUser.SubordinateSubstation.Contains("上海") ? 0 : 1);
                List<Co_CourseShow> MyCPACourse = userBL.GetMyCPACourse(uid, year, "DESC");

                //var sumLength = MyCPACourse.Sum(p => p.GetLength);//一二期全部CPA学时
                //一期CPA有效学时（即一期全部CPA学时）
                var coLength = MyCPACourse.Where(p => p.CPAForm == 0).Sum(pp => pp.GetLength);
                //二期计算有效学时
                string confValue = AllSystemConfigs.Where(p => p.ConfigType == 14).FirstOrDefault().ConfigValue;
                var deplength = MyCPACourse.Where(p => p.CPAForm == 1).Sum(pp => pp.GetLength);
                //部门学时上限
                int depValue = Convert.ToInt32(confValue.Split(';')[2].Split(',')[0]);
                //分所学时上限
                int deptValue = Convert.ToInt32(confValue.Split(';')[3].Split(',')[0]);
                if (Isdep == 1 && deptValue != -1 && deplength > deptValue)
                {
                    deplength = deptValue;
                }
                if (Isdep == 0 && depValue != -1 && deplength > depValue)
                {
                    deplength = depValue;
                }
                usermodel.CPAScore = coLength + deplength;
                #endregion

                Sys_ParamConfig cpazq = AllSystemConfigs.Where(p => p.ConfigType == 13).FirstOrDefault();
                string mianstr = cpazq.ConfigValue + ";";
                string substr = "(?<=" + usermodel.TrainGrade + "-).*?(?=;)";
                //string substr = "(?<=B-).*?(?=;)";
                result = Regex.Match(mianstr, substr).Value == "" ? "0" : Regex.Match(mianstr, substr).Value;


                //全年补预定次数
                Sys_ParamConfig cpazq3 = AllSystemConfigs.Where(p => p.ConfigType == 20).FirstOrDefault();
                ordercount = Convert.ToInt32(cpazq3.ConfigValue);
                if (usermodel.ordertimes > ordercount)
                {
                    usermodel.ordertimes = ordercount;
                }

                #region 其他形式、免修
                var userFreeList = userApplyBL.GetAllMyScore(year.ToString(), CurrentUser.UserId);
                userscore = userscore + userFreeList.Sum(p => p.GettScore);
                FreetScore = userFreeList.Where(p => p.ApplyType==2).Sum(p => p.GettScore);
                usermodel.CPAScore = usermodel.CPAScore + userFreeList.Sum(p => p.GetCPAScore);
                FreeCPAScore = userFreeList.Where(p => p.ApplyType == 2).Sum(p => p.GetCPAScore);
              
                var freeConfig = AllSystemConfigs.Where(p => p.ConfigType == 63 && p.LastUpdateTime.Year == year).FirstOrDefault();
                if (!(freeConfig == null || freeConfig.ConfigValue == ""))
                {
                    var configvalue = freeConfig.ConfigValue.Split(';');
                    var tDate = year + "-" + configvalue[0].Split(',')[0];
                    var tScore = configvalue[0].Split(',')[1];
                    var CPADate = year + "-" + configvalue[1].Split(',')[0];
                    var CPAScore = configvalue[1].Split(',')[1];

                    if (CurrentUser.JoinDate != null)
                    {
                        if (CurrentUser.JoinDate >= Convert.ToDateTime(tDate))
                        {
                            userscore = userscore + Convert.ToDecimal(tScore);
                            FreetScore = FreetScore + Convert.ToDecimal(tScore);
                        }
                    }
                    if (CurrentUser.CPACreateDate != null)
                    {
                        if (CurrentUser.CPACreateDate >= Convert.ToDateTime(CPADate))
                        {
                            usermodel.CPAScore = usermodel.CPAScore + Convert.ToDecimal(CPAScore);
                            FreeCPAScore = FreeCPAScore + Convert.ToDecimal(CPAScore);
                        }
                    }
                }
                #endregion
           
            }
            ViewData["model"] = usermodel;
            var Sys_ParamConfig = AllSystemConfigs.Find(p => p.ConfigType == 33);
            ViewBag.ALLOnLineTestNum = Sys_ParamConfig.ConfigValue;

            ViewBag.PassOnLineTestNum = iattendce.GetMyExamListPassCount(CurrentUser.UserId, CurrentUser.TrainGrade, CurrentUser.DeptId, " ((way=1 and PassStatus=1) or (way=2 and IsPass=1)) and YearPlan=" + year);

            ViewBag.CourseLe = result;
            ViewBag.UserScore = userscore;
            ViewBag.CPACourseLe = CPAresult;
            ViewBag.OrderCount = ordercount;

            ViewBag.FreetScore = FreetScore;
            ViewBag.FreeCPAScore = FreeCPAScore;
            return View();
        }
        /// <summary>
        /// 个人主页
        /// </summary>
        /// <returns></returns>
        public ActionResult UserInfoIndex()
        {
            return View(CurrentUser);
        }

        /// <summary>
        /// 排行榜
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCourseTop()
        {
            return View(CurrentUser);
        }

        /// <summary>
        /// 头像
        /// </summary>
        /// <returns></returns>
        public ActionResult Avatar()
        {
            return View();
        }

        /// <summary>
        /// 基本信息
        /// </summary>
        /// <returns></returns>
        public ActionResult BaseInfo()
        {
            int uid = CurrentUser == null ? 0 : CurrentUser.UserId;
            Sys_User usermodel = new Sys_User
            {
                UserId = 0,
                Username = "superadmin",
                Realname = "superadmin"
            };
            if (uid != 0)
                usermodel = userBL.Get(uid);
            ViewData["modelinfo"] = usermodel;
            return View();
        }

        /// <summary>
        ///  年度培训课程列表呈现
        /// </summary>
        public ViewResult MyCourse(string TrGrade)
        {
            ViewBag.nowyear = DateTime.Now.Year;
            var  itemArray = Iyear.GetALLDepYear();
            ViewData["StrYear"] = itemArray;
            Sys_ParamConfig cpazq = AllSystemConfigs.Where(p => p.ConfigType == 13).FirstOrDefault();
            string mianstr = cpazq.ConfigValue + ";";
            string substr = "(?<=" + ((string.IsNullOrEmpty(TrGrade) || TrGrade == "undefined") ? CurrentUser.TrainGrade : TrGrade) + "-).*?(?=;)";
            //string substr = "(?<=B-).*?(?=;)";
            string result = Regex.Match(mianstr, substr).Value == "" ? "0" : Regex.Match(mianstr, substr).Value;
            ViewData["allsum"] = result;
            ViewBag.IsCpa = CurrentUser.CPA;

            //根据人员SubordinateSubstation字段区分是 开设人员是分所还是部门  有上海是属于部门  没有上海则是分所
            ViewBag.DepOrBranch = CurrentUser.SubordinateSubstation.Contains("上海");
            ViewBag.year = DateTime.Now.Year;
            //  ViewBag.isMan = CurrentUser.SubordinateSubstation.Contains("上海") ? 1 : 0;
            return View();
        }

        /// <summary>
        /// 年度培训课程列表呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult CPAYearCourse()
        {
            List<Tr_YearPlan> itemArray = Iyear.GetAllYear("IsDelete=0 and PublishFlag=1");
            ViewData["StrYear"] = itemArray;

            Sys_ParamConfig cpazq = AllSystemConfigs.Where(p => p.ConfigType == 16).FirstOrDefault();
            string result = cpazq.ConfigValue;
            ViewData["allsum"] = result;
            //判断 0:部门or 1:分所
            var Isdep = CurrentUser == null ? 0 : (CurrentUser.SubordinateSubstation.Contains("上海") ? 0 : 1);
            ViewBag.titlename = Isdep == 0 ? "部门课程学时" : "分所课程学时";
            return View();
        }
        /// <summary>
        ///  CPA周期课程列表呈现
        /// </summary>
        public ViewResult CPACourse()
        {

            List<string> itemArray = Imonth.CPAStartAndEnd(AllSystemConfigs.Where(p => p.ConfigType == 8).FirstOrDefault());
            ViewData["StrCPAYear"] = itemArray;
            Sys_ParamConfig cpazq = AllSystemConfigs.Where(p => p.ConfigType == 17).FirstOrDefault();
            string result = cpazq.ConfigValue;
            ViewData["allsum"] = result;
            var Isdep = CurrentUser == null ? 0 : (CurrentUser.SubordinateSubstation.Contains("上海") ? 0 : 1);
            ViewBag.titlename = Isdep == 0 ? "部门课程学时" : "分所课程学时";
            return View();
        }

        #endregion

        #region 操作
        /// <summary>
        /// 上传头像
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadPhoto()
        {
            string filename = "";
            string resultName = "";
            string folder = ConfigurationManager.AppSettings["OLDphotoUrl"];
            HttpPostedFileBase imgfiles = Request.Files[0];
            if (null != imgfiles)
            {
                try
                {
                    if (imgfiles.ContentLength > 2621440)
                    {
                        return Json(new
                        {
                            result = 0,
                            content = "图片需小于2.5M"
                        }, "text/html", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        filename = Path.GetFileName(imgfiles.FileName); //获得文件名
                        string fullPathname = Path.Combine(folder, filename);//文件后缀名
                        string suffix = imgfiles.FileName.Substring(imgfiles.FileName.LastIndexOf(".") + 1).ToLower();
                        resultName = Guid.NewGuid() + "." + suffix;
                        saveFile(imgfiles, folder, resultName);
                        return Json(new
                        {
                            result = 1,
                            content = resultName
                        }, "text/html", JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        result = 0,
                        content = "上传失败"
                    }, "text/html", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    result = 0,
                    content = "上传失败"
                }, "text/html", JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 保存头像
        /// </summary>
        /// <param name="picname"></param>
        /// <param name="txt_width"></param>
        /// <param name="txt_height"></param>
        /// <param name="txt_top"></param>
        /// <param name="txt_left"></param>
        /// <param name="txt_DropWidth"></param>
        /// <param name="txt_DropHeight"></param>
        /// <returns></returns>
        public JsonResult SavePhoto(string picname, float z, int t, int l, int w, int h)
        {
            float zoomLevel = Convert.ToSingle(z);
            int top = Convert.ToInt32(t);
            int left = Convert.ToInt32(l);
            int width = Convert.ToInt32(w);
            int height = Convert.ToInt32(h);

            string picurl = ConfigurationManager.AppSettings["OLDphotoUrl"] + picname;
            string folderNew = ConfigurationManager.AppSettings["NEWphotoUrl"];
            try
            {
                string filename = CutPhotoHelp.Crop(Server.MapPath(picurl), Server.MapPath(folderNew), zoomLevel, top, left, width, height);
                int uid = CurrentUser == null ? 0 : CurrentUser.UserId;
                string PhotoUrl = folderNew + filename;
                bool re = userBL.UpdatePhoto(PhotoUrl, uid);
                if (re)
                {
                    return Json(new
                    {
                        result = 1,
                        content = filename
                    }, "text/html", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        result = 0,
                        content = "保存失败"
                    }, "text/html", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = 0,
                    content = "保存失败"
                }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }

        public bool saveFile(HttpPostedFileBase postedFile, string filepath, string saveName)
        {
            bool result = false;
            if (!Directory.Exists(HttpContext.Server.MapPath(filepath)))
            {
                Directory.CreateDirectory(filepath);
            }
            try
            {
                string a = HttpContext.Server.MapPath(filepath);
                postedFile.SaveAs(a + "\\" + saveName);
                result = true;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
            return result;
        }


        /// <summary>
        /// 年度培训目标(所有课程)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="name"></param>
        /// <param name="way"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetMyCourse(int year, string name, int way, string allsum, string coLength, int pageSize = 20, int pageIndex = 1)
        {
            string where = "1=1";
            //DateTime startTime = DateTime.Now;
            //DateTime endTime = DateTime.Now;
            try
            {
                int totalCount = 0;
                int uid = CurrentUser == null ? 0 : CurrentUser.UserId;
                if (way != 3)
                {
                    where += string.Format(" and cc.Way={0}", way);
                }
                if (!string.IsNullOrEmpty(name))
                {
                    where += string.Format(" and cc.CourseName LIKE '%{0}%'", name.ReplaceSql());
                }
                List<Co_CourseShow> MyCourse = new List<Co_CourseShow>();
                if (way == 3)
                {
                    MyCourse = userBL.GetMyCPACoursebyWay(uid, year, out totalCount, pageIndex, pageSize, "asc", where);
                }
                else
                {
                    MyCourse = userBL.GetMyCourse(uid, year, way, out totalCount, pageIndex, pageSize, "asc", where);
                }

                foreach (var item in MyCourse)
                {
                    item.CourseName = item.CourseName.HtmlXssEncode();
                    item.RoomName = item.RoomName.HtmlXssEncode();
                    item.allsum = allsum == "" ? 0 : Convert.ToDecimal(allsum);
                    item.CoLength = coLength == "" ? 0 : Convert.ToDecimal(coLength);
                }
                return Json(new
                {
                    dataList = MyCourse,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// CPA年度培训目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="name"></param> 
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetMyCPAYearCourse(int year, string name, string allsum, int scoreway = -1, int pageSize = 20, int pageIndex = 1)
        {
            string where = "1=1";
            //DateTime startTime = DateTime.Now;
            //DateTime endTime = DateTime.Now;
            try
            {
                int uid = CurrentUser == null ? 0 : CurrentUser.UserId;
                //判断 0:部门or 1:分所
                int Isdep = CurrentUser == null ? 0 : (CurrentUser.SubordinateSubstation.Contains("上海") ? 0 : 1);

                var tilte = Isdep == 0 ? "部门" : "分所";
                if (!string.IsNullOrEmpty(name))
                {
                    where += string.Format(" and temp.CourseName LIKE '%{0}%'", name.ReplaceSql());
                }
                if (scoreway != -1)
                {
                    where += " and way=" + scoreway;
                }
                List<Co_CourseShow> MyCPACourse = userBL.GetMyCPACourse(uid, year, "DESC", where);

                foreach (var item in MyCPACourse)
                {
                    switch (item.Way)
                    {
                        case 0:
                            item.CPAFromStr = tilte + "课程学时";
                            break;
                        case 1:
                            item.CPAFromStr = "集中授课学时";
                            break;
                        case 2:
                            item.CPAFromStr = "视频课程学时";
                            break;
                        case 3:
                            item.CPAFromStr = "注协课程学时";
                            break;
                    }
                }

                #region 计算有效学时
                //var sumLength = MyCPACourse.Sum(p => p.GetLength);//一二期全部CPA学时
                //一期CPA有效学时（即一期全部CPA学时）
                var coLength = MyCPACourse.Where(p => p.CPAForm == 0).Sum(pp => pp.GetLength);
                //二期计算有效学时
                string confValue = AllSystemConfigs.Where(p => p.ConfigType == 14).FirstOrDefault().ConfigValue;
                var deplength = MyCPACourse.Where(p => p.CPAForm == 1).Sum(pp => pp.GetLength);
                //部门学时上限
                int depValue = Convert.ToInt32(confValue.Split(';')[2].Split(',')[0]);
                //分所学时上限
                int deptValue = Convert.ToInt32(confValue.Split(';')[3].Split(',')[0]);
                if (Isdep == 1 && deptValue != -1 && deplength > deptValue)
                {
                    deplength = deptValue;
                }
                if (Isdep == 0 && depValue != -1 && deplength > depValue)
                {
                    deplength = depValue;
                }

                #endregion

                MyCPACourse = MyCPACourse.Where(p => p.GetLength > 0).ToList();
                List<Co_CourseShow> list = MyCPACourse.Skip(((pageIndex - 1) * pageSize)).Take(pageSize).ToList();
                foreach (var item in list)
                {
                    item.CourseName = item.CourseName.HtmlXssEncode();
                    item.allsum = Convert.ToDecimal(allsum);//目标学时
                    item.CoLength = coLength + deplength;//总的有效学时
                }

                var daode = list.Where(p => p.Way == 3).Sum(p => p.daode);
                return Json(new
                {
                    dataList = list,
                    recordCount = MyCPACourse.Count(),
                    daode = daode
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// CPA周期目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="name"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetMyCPACourse(string year, string name, string allsum, int scoreway = -1, int pageSize = 20, int pageIndex = 1)
        {
            string where = "1=1";
            var ArrayValue = year.Split('—');
            DateTime startTime = DateTime.Parse(ArrayValue[0].Replace("年", "-").Replace("月", "-") + "01 0:00:00");
            DateTime endTime = DateTime.Parse(ArrayValue[1].Replace("年", "-").Replace("月", "-") + "01 0:00:00").AddMonths(1).AddSeconds(-1);

            //间隔的年份
            //var listYear = new List<int>();
            //for (int i = 1; i <=endTime.Year-startTime.Year; i++)
            //{
                
            //}
            
            try
            {
                int uid = CurrentUser == null ? 0 : CurrentUser.UserId;
                //判断 0:部门or 1:分所
                int Isdep = CurrentUser == null ? 0 : (CurrentUser.SubordinateSubstation.Contains("上海") ? 0 : 1);


                if (!string.IsNullOrEmpty(name))
                {
                    where += string.Format(" and temp.CourseName LIKE '%{0}%'", name.ReplaceSql());
                }
                if (scoreway != -1)
                {
                    where += " and way=" + scoreway;
                }
                List<Co_CourseShow> MyCPACourse = userBL.GetMyCPACourse(uid, startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"), "DESC", where);

                var tilte = Isdep == 0 ? "部门" : "分所";
                foreach (var item in MyCPACourse)
                {
                    switch (item.Way)
                    {
                        case 0:
                            item.CPAFromStr = tilte + "课程学时";
                            break;
                        case 1:
                            item.CPAFromStr = "集中授课学时";
                            break;
                        case 2:
                            item.CPAFromStr = "视频课程学时";
                            break;
                        case 3:
                            item.CPAFromStr = "注协课程学时";
                            break;
                    }
                }
                #region 计算有效学时
                //var sumLength = MyCPACourse.Sum(p => p.GetLength);//一二期全部CPA学时
                //一期CPA有效学时（即一期全部CPA学时）
                var coLength = MyCPACourse.Where(p => p.CPAForm == 0).Sum(pp => pp.GetLength);
                //二期计算有效学时
                string confValue = AllSystemConfigs.Where(p => p.ConfigType == 14).FirstOrDefault().ConfigValue;
                decimal deplength = 0;
                foreach (var item in MyCPACourse.Select(p=>p.YearPlan).Distinct())
                {
                    
                   var  sdeplength = MyCPACourse.Where(p => p.CPAForm == 1 && p.YearPlan ==item).Sum(pp => pp.GetLength);//二期全部CPA学时
                    int depValue = Convert.ToInt32(confValue.Split(';')[2].Split(',')[0]);//部门学时上限
                    int deptValue = Convert.ToInt32(confValue.Split(';')[3].Split(',')[0]);//分所学时上限
                    if (Isdep == 1 && deptValue != -1 && sdeplength > deptValue)
                    {
                        sdeplength = deptValue;
                    }
                    if (Isdep == 0 && depValue != -1 && sdeplength > depValue)
                    {
                        sdeplength = depValue;
                    }
                    deplength = deplength + sdeplength;
                }
              

                #endregion
                var daode = MyCPACourse.Where(p => p.Way == 3).Sum(p => p.daode);
                List<Co_CourseShow> list = MyCPACourse.Where(p=>p.GetLength>0).Skip(((pageIndex - 1) * pageSize)).Take(pageSize).ToList();
                foreach (var item in list)
                {
                    item.CourseName = item.CourseName.HtmlXssEncode();
                    item.allsum = Convert.ToDecimal(allsum);//目标学时
                    item.CoLength = coLength + deplength;//总的有效学时
                    item.daode = daode;
                }
                return Json(new
                {
                    dataList = list,
                    recordCount = MyCPACourse.Where(p => p.GetLength > 0).Count()
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 积分前5名
        /// </summary>
        /// <returns></returns>
        public JsonResult GetVantages()
        {
            return Json(new
            {
                dataList = new object[0],
                recordCount = 0
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 学分前5名
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCredit()
        {
            List<Sys_User> Creditlist = userBL.GetTop5Credit();
            foreach (var item in Creditlist)
            {
                item.Realname = item.Realname.HtmlXssEncode();
                item.DeptName = item.DeptName.HtmlXssEncode();
            }
            return Json(new
            {
                dataList = Creditlist,
                recordCount = Creditlist.Count
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}