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

namespace LiXinControllers.User
{
    public partial class UserManageController : BaseController
    {
        #region 呈现页面

        public ActionResult UserInfoComm()
        {
            int uid = CurrentUser == null ? 0 : CurrentUser.UserId;
            Sys_User usermodel = InitSuper;
            //所内目标学时
            string result = "0";
            //CPA目标学时
            string CPAresult = "0";
            //个人总学时（除CPA）
            decimal userscore = 0;
            //全年补预定次数
            int ordercount = 0;

            if (uid != 0)
            {
                //CPA目标学时
                Sys_ParamConfig cpazq1 = AllSystemConfigs.Where(p => p.ConfigType == 16).FirstOrDefault();
                CPAresult = cpazq1.ConfigValue;
                //计算个人学时
                Sys_ParamConfig cpazq2 = AllSystemConfigs.Where(p => p.ConfigType == 14).FirstOrDefault();
                userscore = userBL.GetUserScore(uid, cpazq2);

                
                //所内目标学时
                usermodel = userBL.GetmodelCAP(uid);
                Sys_ParamConfig cpazq = AllSystemConfigs.Where(p => p.ConfigType == 13).FirstOrDefault();
                string mianstr = cpazq.ConfigValue + ";";
                string substr = "(?<=" + usermodel.TrainGrade + "-).*?(?=;)";
                //string substr = "(?<=B-).*?(?=;)";
                result = Regex.Match(mianstr, substr).Value;
                
                
                //全年补预定次数
                Sys_ParamConfig cpazq3 = AllSystemConfigs.Where(p => p.ConfigType == 20).FirstOrDefault();
                ordercount = Convert.ToInt32(cpazq3.ConfigValue);
                if (usermodel.ordertimes > ordercount)
                {
                    usermodel.ordertimes = ordercount;
                }
            }
            ViewData["model"] = usermodel;
            var Sys_ParamConfig = AllSystemConfigs.Find(p => p.ConfigType == 33);
            ViewBag.ALLOnLineTestNum = Sys_ParamConfig.ConfigValue;

            ViewBag.PassOnLineTestNum = iattendce.GetMyExamListPassCount(CurrentUser.UserId, CurrentUser.TrainGrade, CurrentUser.DeptId, " ((way=1 and PassStatus=1) or (way=2 and IsPass=1))");

            ViewBag.CourseLe = result;
            ViewBag.UserScore = userscore;
            ViewBag.CPACourseLe = CPAresult;
            ViewBag.OrderCount = ordercount;

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
            List<Tr_YearPlan> itemArray = Iyear.GetAllYear("IsDelete=0 and PublishFlag=1");
            ViewData["StrYear"] = itemArray;
            Sys_ParamConfig cpazq = AllSystemConfigs.Where(p => p.ConfigType == 13).FirstOrDefault();
            string mianstr = cpazq.ConfigValue + ";";
            string substr = "(?<=" + TrGrade + "-).*?(?=;)";
            //string substr = "(?<=B-).*?(?=;)";
            string result = Regex.Match(mianstr, substr).Value;
            ViewBag.IsCpa = CurrentUser.CPA;
            ViewData["allsum"] = result;
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
        public JsonResult GetMyCourse(int year, string name, int way, string allsum,string coLength,int pageSize = 20, int pageIndex = 1)
        {
            string where = "1=1";
            //DateTime startTime = DateTime.Now;
            //DateTime endTime = DateTime.Now;
            try
            {
                int totalCount = 0;
                int uid = CurrentUser == null ? 0 : CurrentUser.UserId;
                if (way!=3)
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
                    MyCourse = userBL.GetMyCPACoursebyWay(uid, year,out totalCount, pageIndex, pageSize, "asc", where);
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
        public JsonResult GetMyCPAYearCourse(int year, string name, string allsum, int pageSize = 20, int pageIndex = 1)
        {
            string where = "1=1";
            //DateTime startTime = DateTime.Now;
            //DateTime endTime = DateTime.Now;
            try
            {
                int totalCount = 0;
                int uid = CurrentUser == null ? 0 : CurrentUser.UserId;
                if (!string.IsNullOrEmpty(name))
                {
                    where += string.Format(" and cc.CourseName LIKE '%{0}%'", name.ReplaceSql());
                }
                List<Co_CourseShow> MyCPACourse = userBL.GetMyCPACourse(uid, year, out totalCount, pageIndex, pageSize, "asc", where);
                foreach (var item in MyCPACourse)
                {
                    item.CourseName = item.CourseName.HtmlXssEncode();
                    item.allsum = allsum == "" ? 0 : Convert.ToDecimal(allsum);
                }
                return Json(new
                {
                    dataList = MyCPACourse,
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
        /// CPA周期目标
        /// </summary>
        /// <param name="year"></param>
        /// <param name="name"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetMyCPACourse(string year, string name, string allsum, int pageSize = 20, int pageIndex = 1)
        {
            string where = "1=1";
            var ArrayValue = year.Split('—');
            DateTime startTime = DateTime.Parse(ArrayValue[0].Replace("年", "-").Replace("月", "-") + "01 0:00:00");
            DateTime endTime = DateTime.Parse(ArrayValue[1].Replace("年", "-").Replace("月", "-") + "01 0:00:00").AddMonths(1).AddSeconds(-1);
            try
            {
                int totalCount = 0;
                int uid = CurrentUser == null ? 0 : CurrentUser.UserId;
                if (!string.IsNullOrEmpty(name))
                {
                    where += string.Format(" and cc.CourseName LIKE '%{0}%'", name.ReplaceSql());
                }
                List<Co_CourseShow> MyCPACourse = userBL.GetMyCPACourse(uid, startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"), out totalCount, pageIndex, pageSize, "asc", where);
                foreach (var item in MyCPACourse)
                {
                    item.CourseName = item.CourseName.HtmlXssEncode();
                    item.allsum = allsum == "" ? 0 : Convert.ToDecimal(allsum);
                }
                return Json(new
                {
                    dataList = MyCPACourse,
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