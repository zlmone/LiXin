using LiXinCommon;
using LiXinModels;
using LiXinModels.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LiXinInterface;
using LiXinInterface.User;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using LiXinControllers.Filter;

namespace LiXinControllers
{
    public class NewUserManageController : BaseController
    {
        public INewUser nuserBL;
        public IUser userBL;
        public ISys_TrainGrade gradeBL;
        public NewUserManageController(INewUser _nuserBL, IUser _userBL, ISys_TrainGrade _gradeBL)
        {
            nuserBL = _nuserBL;
            userBL = _userBL;
            gradeBL = _gradeBL;
        }

        #region 呈现
        public ActionResult NewUserManage()
        {
            var yearList = nuserBL.GetNewYear();
            yearList.Add(yearList.Last() + 1);
            ViewBag.yearList = yearList;
            ViewBag.nowyear = DateTime.Now.Year;
            return View();
        }

        public ActionResult NewUserEdit(int userID)
        {
            ViewBag.userID = userID;
            var model = userBL.Get(userID);
            ViewBag.trainGrade = AllTrainGrade;
            return View(model);
        }

        public ActionResult inputScore(int userID)
        {
            ViewBag.userID = userID;
            var model = nuserBL.GetModel(" UserID=" + userID);
            ViewBag.model = model;
            return View(model);
        }

        public ActionResult NewInputScore()
        {
            var yearList = nuserBL.GetNewYear();
            yearList.Add(yearList.Last() + 1);
            ViewBag.yearList = yearList;
            ViewBag.nowyear = DateTime.Now.Year;
            return View();
        }


        public ActionResult UpdateGrade()
        {
            ViewBag.Grade = AllTrainGrade;
            return View();
        }

        public ActionResult UserDetail(int id)
        {
            ViewBag.User = userBL.Get(id);
            return View();
        }


        public ActionResult ImportScore()
        {
            return View();
        }

        public ActionResult ImportUser()
        {
            return View();
        }
        #endregion

        #region 新进员工
        /// <summary>
        /// 新进员工列表
        /// </summary>
        /// <param name="realName">姓名</param>
        /// <param name="starttime">入职开始时间</param>
        /// <param name="endtime">入职结束时间</param>
        /// <param name="graduateSchool">毕业院校</param>
        /// <param name="Major">所学专业</param>
        /// <param name="InternDept">实习部门/param>
        /// <param name="IsInternExp">是否有实习经验</param>
        /// <param name="IsOurIntern">是否在我所实习</param>
        /// <param name="sex">性别</param>
        public JsonResult GetUserList(int Year = -1, string realName = "", string graduateSchool = "", string Major = "", string InternDept = "",
            int IsInternExp = -1, int IsOurIntern = -1, int sex = -1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " UserId desc")
        {
            try
            {
                int totalcount = 0;
                string where = " 1=1";

                #region 查询条件
                if (!string.IsNullOrEmpty(realName))
                {
                    where += string.Format(@" and Realname LIKE '%{0}%'", realName.ReplaceSql());
                }

                if (!string.IsNullOrEmpty(graduateSchool))
                {
                    where += string.Format(@" and GraduateSchool LIKE '%{0}%'", graduateSchool.ReplaceSql());
                }
                if (!string.IsNullOrEmpty(Major))
                {
                    where += string.Format(@" and Major LIKE '%{0}%'", Major.ReplaceSql());
                }
                if (!string.IsNullOrEmpty(InternDept))
                {
                    where += string.Format(@" and InternDept LIKE '%{0}%'", InternDept.ReplaceSql());
                }
                if (IsInternExp != -1)
                {
                    where += " AND IsInternExp=" + IsInternExp;
                }
                if (IsOurIntern != -1)
                {
                    where += " AND IsOurIntern=" + IsOurIntern;
                }
                if (sex != -1)
                {
                    where += " AND sex=" + sex;
                }
                #endregion


                var list = nuserBL.GetNewUserList(out totalcount, pageIndex, pageSize, jsRenderSortField, where, Year);
                foreach (var item in list)
                {
                    item.Realname = item.Realname.HtmlXssEncode();

                }
                return Json(new
                {
                    dataList = list,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<Sys_User>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 更新人员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Filter.SystemLog("新进员工维护", LogLevel.Info)]
        public JsonResult Update(int IsInternExp, int IsOurIntern, string userID, string InternDept, string TrainGrade = "")
        {
            try
            {
                //var user = userBL.Get(userID);
                var model = new Sys_User()
                {
                    IsOurIntern = IsOurIntern,
                    IsInternExp = IsInternExp,
                    InternDept = InternDept,
                    TrainGrade = TrainGrade
                };
                if (TrainGrade == "")
                {
                    model.TrainGrade = "T";

                    nuserBL.UpdateNewUserNoTrain(model, userID);
                }
                else
                {
                    nuserBL.UpdateNewUser(model, userID);
                }

                return Json(new
                {
                    result = 0,
                    Content = "成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    Content = "失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入学员
        /// </summary>
        /// <returns></returns>
        [Filter.SystemLog("导入学员", LogLevel.Info)]
        public JsonResult ImportUserList()
        {
            string folder = ConfigurationManager.AppSettings["NewUserUrl"];
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

                var userList = userBL.GetList();
                var count = 0;
                if (IsTrueTemplate(dtList[0]))
                {
                    foreach (DataRow row in dtList[0].Rows)
                    {
                        rowCount++;
                        if (row[3].ToString() != "")
                        {

                            var LoginId = row[3];
                            var model = userList.Find(p => string.Equals(p.LoginId, LoginId));
                            if (model != null)
                            {
                                model.IsNew = 1;
                                model.IsHistoryNew = 1;
                                model.NewYear = DateTime.Now.Year;
                                model.IsInternExp = row[10].ToString() == "" ? 0 : (int)((Enums.IsNot)Enum.Parse(typeof(Enums.IsNot), row[10].ToString()));
                                model.IsOurIntern = row[11].ToString() == "" ? 0 : (int)((Enums.IsNot)Enum.Parse(typeof(Enums.IsNot), row[11].ToString()));
                                model.InternDept = row[12].ToString();
                                // model.TrainGrade = model.TrainGrade == null ? (row[4].ToString() == "" ? "T" : row[4].ToString()) : model.TrainGrade;
                                //model.TrainGrade = "T";
                                model.OldTrainGrade = "T";
                                model.Sex = (int)((Enums.SexImport)Enum.Parse(typeof(Enums.SexImport), row[5].ToString()));
                                model.GraduateSchool = string.IsNullOrEmpty(model.GraduateSchool) ? row[8].ToString() : model.GraduateSchool;
                                model.Major = string.IsNullOrEmpty(model.Major) ? row[9].ToString() : model.Major;
                                model.Email = string.IsNullOrEmpty(model.Email) ? row[7].ToString() : model.Email;
                                model.MobileNum = string.IsNullOrEmpty(model.MobileNum) ? row[6].ToString() : model.MobileNum;
                                userBL.Update(model);
                                count++;
                            }
                        }
                        else
                        {
                            flag = false;
                            error = error + "," + rowCount;
                        }

                        //var user = userBL.GetList(" And LoginId=" + row[0]).FirstOrDefault();
                        //if(user!=null)

                    }
                    error = error == "" ? "" : error.TrimStart(',');
                    message = "导入成功，有" + count + "名学员新进员工" + (flag ? "" : ",第" + error + "行有错误");
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
            catch (Exception e)
            {
                return Json(new
                {
                    result = 0,
                    content = e.Message + "---" + e.Source//"导入失败"
                }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }



        //判断模板是否正确
        private bool IsTrueTemplate(DataTable dt)
        {
            if (dt.Columns.Count >= 14)
            {
                #region 列名
                if (dt.Columns[0].ColumnName.Trim() != "序号")
                    return false;
                if (dt.Columns[0].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[1].ColumnName.Trim() != "分配部门")
                    return false;
                if (dt.Columns[1].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[2].ColumnName.Trim() != "姓名")
                    return false;
                if (dt.Columns[2].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[3].ColumnName.Trim() != "登录名")
                    return false;
                if (dt.Columns[3].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[4].ColumnName.Trim() != "培训级别")
                    return false;
                if (dt.Columns[4].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[5].ColumnName.Trim() != "性别")
                    return false;
                if (dt.Columns[5].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[6].ColumnName.Trim() != "手机号")
                    return false;
                if (dt.Columns[6].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[7].ColumnName.Trim() != "e-mail")
                    return false;
                if (dt.Columns[7].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[8].ColumnName.Trim() != "毕业院校")
                    return false;
                if (dt.Columns[8].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[9].ColumnName.Trim() != "所学专业")
                    return false;
                if (dt.Columns[9].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[10].ColumnName.Trim().Replace("\n", "") != "是否有事务所实习经验")
                    return false;
                if (dt.Columns[10].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[11].ColumnName.Trim().Replace("\n", "") != "是否在我所进行过实习")
                    return false;
                if (dt.Columns[11].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[12].ColumnName.Trim() != "实习部门")
                    return false;
                if (dt.Columns[12].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[13].ColumnName.Trim() != "备注")
                    return false;
                if (dt.Columns[13].DataType.FullName != "System.String")
                    return false;
                #endregion

            }
            else
                return false;
            return true;
        }

        /// <summary>
        /// 删除人，就是把新员工变为老员工
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Filter.SystemLog("新员工删除", LogLevel.Info)]
        public JsonResult UpdateIsNew(int userID)
        {
            try
            {
                nuserBL.Update(userID, " IsNew=0,IsHistoryNew=2,NewYear=0");
                return Json(new
                {
                    result = 1,
                    Content = "成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    Content = "失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 独立考试录入
        /// <summary>
        /// 独立考试录入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Filter.SystemLog("培训成绩录入 独立考试录入", LogLevel.Info)]
        public JsonResult SaveScore(New_UserExamScore model)
        {
            try
            {
                nuserBL.InsertNew_UserExamScore(model);
                return Json(new
                {
                    result = 1,
                    content = "成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 导入成绩
        /// </summary>
        /// <returns></returns>

        [Filter.SystemLog("培训成绩录入 导入成绩", LogLevel.Info)]
        public JsonResult ImportScoreList()
        {
            string folder = ConfigurationManager.AppSettings["NewScoreUrl"];
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

                var userList = userBL.GetList();
                var count = 0;
                if (IsTrueExamTemplate(dtList[0]))
                {
                    foreach (DataRow row in dtList[0].Rows)
                    {
                        rowCount++;
                        if (row[0].ToString() != "")
                        {
                            if (IsScore(row[2].ToString(), row[3].ToString()))
                            {
                                count++;
                                var score1 = Convert.ToDouble(Math.Round(Convert.ToDouble(row[2]), 2, MidpointRounding.AwayFromZero));
                                var score2 = Convert.ToDouble(Math.Round(Convert.ToDouble(row[3]), 2, MidpointRounding.AwayFromZero));
                                nuserBL.UpdateOrInsert(row[0].ToString(), score1, score2);
                            }
                            else
                            {
                                flag = false;
                                error = error + "," + rowCount;
                            }

                        }
                        else
                        {
                            flag = false;
                            error = error + "," + rowCount;
                        }

                        //var user = userBL.GetList(" And LoginId=" + row[0]).FirstOrDefault();
                        //if(user!=null)

                    }
                    error = error == "" ? "" : error.TrimStart(',');
                    message = "导入成功，有" + count + "名学员新进员工有分数" + (error == "" ? "" : ",第" + error + "行有错误");
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
        private bool IsTrueExamTemplate(DataTable dt)
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
                if (dt.Columns[2].ColumnName.Trim() != "考试得分(0-999)")
                    return false;
                if (dt.Columns[2].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[3].ColumnName.Trim() != "考试总分(1-999)")
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
                    var score1 = Convert.ToDouble(Math.Round(Convert.ToDouble(score), 2, MidpointRounding.AwayFromZero));
                    var score2 = Convert.ToDouble(Math.Round(Convert.ToDouble(sumScore), 2, MidpointRounding.AwayFromZero));
                    if (score1 > score2)
                    {
                        return false;
                    }
                    else
                    {
                        //var reg = new Regex(@"^\d+[\.]?\d{0,2}$");
                        //if (reg.IsMatch(score) && reg.IsMatch(sumScore))
                        //{
                        return true;
                        //}
                        //return false;
                    }
                }
            }
            return true;
        }
        #endregion

        #region 批量修改培训级别
        /// <summary>
        /// 批量修改培训级别
        /// </summary>
        /// <returns></returns>
        [Filter.SystemLog("新进员工维护 批量修改培训级别", LogLevel.Info)]
        public JsonResult SaveUpdateGrade(string userIDs, string grade)
        {
            try
            {
                gradeBL.UpdateTrainGradeByUserID(userIDs, grade);
                return Json(new
                {
                    result = 1,
                    Content = "成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    Content = "失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}
