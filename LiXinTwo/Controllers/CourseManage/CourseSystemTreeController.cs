using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinInterface;
using LiXinModels;
using System.Web.Mvc;
using LiXinInterface.CourseManage;
using LiXinInterface.User;
using LiXinModels.CourseManage;
using LiXinCommon;
using LiXinControllers.Filter;
namespace LiXinControllers
{
    public partial class CourseManageController : BaseController
    {
        /// <summary>
        /// 获取课程体系(左右结构)
        /// </summary>
        /// <param name="checkBoxFlag">0：木有复选框，1：前置复选框</param>
        ///<param name="showButton">0：木有确定按钮，1：显示确定按钮</param>
        ///<param name="showManage">0：不显示 新增 修改 删除，1：显示新增修改删除</param>
        ///<param name="radioFlag">单选按钮 0木有 1有</param>
        /// <returns></returns>
        public ActionResult CourseSystemTree(int showManage = 0, int checkBoxFlag = 0, int showButton = 0, int radioFlag = 0)
        {

            ViewBag.checkBoxFlag = checkBoxFlag;
            ViewBag.radioFlag = radioFlag;
            ViewBag.showButton = showButton;
            ViewBag.showManage = showManage;
            return View();
        }

        [Filter.SystemLog("编辑胜任力课程体系", LogLevel.Info)]
        public ActionResult EditCourseSystem(int flag, int Id)
        {

            Co_CourseSystem courseSys = new Co_CourseSystem();
            if (Id != 0)
            {
                courseSys = _courseSysBL.GetCourseSystem(Id);
            }
            if (flag == 1)//编辑  选中的接点
            {
                if (courseSys != null)
                {
                    var list = _sysPayGradeBL.GetSys_SortLinkGradeList(" SortId=" + Id);
                    if (courseSys.ParentID == 0)
                    {
                        ViewBag.PartentName = TopCourseSystemName;
                    }
                    else
                    {
                        var temp = _courseSysBL.GetCourseSystem(courseSys.ParentID);
                        if (temp == null)
                        {
                            ViewBag.PartentName = TopCourseSystemName;
                        }
                        else
                        {
                            ViewBag.PartentName = temp.CourseSystemName;
                        }
                    }
                }
                return View(courseSys);
            }
            else//新增
            {
                var tempAdd = new Co_CourseSystem();
                tempAdd.ParentID = Id;

                var tempParent = _courseSysBL.GetCourseSystem(courseSys.Id);
                if (tempParent == null)
                {
                    ViewBag.PartentName = TopCourseSystemName;
                }
                else
                {
                    ViewBag.PartentName = tempParent.CourseSystemName;
                }
                return View(tempAdd);
            }

        }



        public ActionResult EditCompetencyCourse(int? Id)
        {
            ViewBag.trainGrade = _sys_TrainBL.GetAllTrainGrade();
            ViewBag.SysPayGradeList = _sysPayGradeBL.GetSys_PayGradeList();
            if (Id.HasValue)
            {
                var list = _sysPayGradeBL.GetSys_SortLinkGradeList(" CourseId=" + Id);
                ViewBag.SysLinkSortIds = string.Join(",", list.Select(i => i.SortID));
                ViewBag.SysLinkGradeIds = string.Join(",", list.Select(i => i.GradeID));
                ViewBag.haveID = "";
                if (list != null)
                {
                    var sortlist = _courseSysBL.GetCo_CourseSystemList().Where(c => list.Select(i => i.SortID).Contains(c.Id));
                    ViewBag.SysLinkSort = sortlist;
                    ViewBag.haveID = string.Join(",", sortlist.Select(p => p.Id));
                }

                var sortGradeCourse = _sysPayGradeBL.GetSys_SortGradeCourseList(" Sys_SortGradeCourse.Id=" + Id.Value).FirstOrDefault();
                if (sortGradeCourse == null)
                {
                    return View(new Sys_SortGradeCourse());
                }
                else
                {
                    return View(sortGradeCourse);
                }
            }

            return View(new Sys_SortGradeCourse());
        }

        /// <summary>
        /// 胜任力课程列表
        /// </summary>
        /// <param name="showGrade">0：木有薪酬体系，1：显示薪酬体系</param>
        /// <param name="CCcourseIds">已经选择的courseIds </param>
        /// <param name="showCCManage">0 不显示 管理操作 1  显示 </param>
        /// <param name="showCCCheckbox">0 不显示 1  显示Checkbox 2 显示 </param>
        /// <param name="showCCSureButton">0 不显示  1  显示 </param>
        /// <param name="showAddCCButton">0 不显示  1  显示 </param>
        /// <param name="showtype">0默认按照当前的薪酬级别进行查询  1没有薪酬级别</param>
        /// <returns></returns>
        public ActionResult CompetencyCourseList(int showGrade = 0, string CCcourseIds = "", int showCCManage = 0, int showCCCheckbox = 0, int showCCSureButton = 0, int showAddCCButton = 0, int showtype = 0)
        {
            if (showGrade == 1 || showtype == 1)
            {
                var list = _sysPayGradeBL.GetSys_PayGradeList();
                string html = "";
                foreach (var item in list)
                {
                    html += item.Id + ",";
                }
                ViewBag.SysPayGradeList = html.Substring(0, html.LastIndexOf(','));
                //ViewBag.SysPayGradeList = _sysPayGradeBL.GetSys_PayGradeList();


            }
            ViewBag.showGrade = showGrade;
            ViewBag.CCcourseIds = CCcourseIds;
            ViewBag.showCCManage = showCCManage;
            ViewBag.showCCCheckbox = showCCCheckbox;
            ViewBag.showCCSureButton = showCCSureButton;
            ViewBag.showAddCCButton = showAddCCButton;
            ViewBag.showtype = showtype;
            return View();
        }


        /// <summary>
        /// 胜任力课程列表
        /// </summary>
        /// <param name="showGrade">0：木有薪酬体系，1：显示薪酬体系</param>
        /// <param name="CCcourseIds">已经选择的courseIds </param>
        /// <param name="showCCManage">0 不显示 管理操作 1  显示 </param>
        /// <param name="showCCCheckbox">0 不显示 1  显示Checkbox 2 显示 </param>
        /// <param name="showCCSureButton">0 不显示  1  显示 </param>
        /// <param name="showAddCCButton">0 不显示  1  显示 </param>
        /// <returns></returns>
        public ActionResult CompetencyCourseManager(int showGrade = 0, string CCcourseIds = "", int showCCManage = 0, int showCCCheckbox = 0, int showCCSureButton = 0, int showAddCCButton = 0)
        {
            if (showGrade == 1)
            {
                var list = _sysPayGradeBL.GetSys_PayGradeList();
                string html = "";
                foreach (var item in list)
                {
                    html += item.Id + ",";
                }
                ViewBag.SysPayGradeStr = html.Substring(0, html.LastIndexOf(','));
                ViewBag.SysPayGradeList = _sysPayGradeBL.GetSys_PayGradeList();
            }
            ViewBag.showGrade = showGrade;
            ViewBag.CCcourseIds = CCcourseIds;
            ViewBag.showCCManage = showCCManage;
            ViewBag.showCCCheckbox = showCCCheckbox;
            ViewBag.showCCSureButton = showCCSureButton;
            ViewBag.showAddCCButton = showAddCCButton;
            return View();
        }

        public ActionResult CompetencyCourseManageList()
        {
            return View();
        }

        public ActionResult CompetencyCourseShowList(int ShowLayout = 0)
        {
            ViewBag.ShowLayout = ShowLayout;
            return View();
        }

        public ActionResult PopCompetencyCourseShowList()
        {
            return View();
        }

        public ActionResult PopCourseTreeForSurvey()
        {
            ViewBag.SysPayGradeList = _sysPayGradeBL.GetSys_PayGradeList();
            return View();
        }

        public ActionResult PopCourseOfSurvey(int sysIds, string gradeIds)
        {
            ViewBag.gradeIds = gradeIds;
            ViewBag.sysIds = sysIds;
            return View();
        }
        #region 分类方法


        public JsonResult GetAllCourseSystem(int type = 0, int checkBoxFlag = 0, int radioFlag = 0, int arrow = 0)
        {
            List<Co_CourseSystem> Co_CourseSystemList = _courseSysBL.GetCo_CourseSystemList();
            var treeStr = new StringBuilder();
            treeStr.AppendLine("<ul id=\"navigation\" class=\"treeview w700\">");
            treeStr.Append("<li id=\"0\" class='pNote'>");
            if (type == 0)
            {
                treeStr.AppendFormat("<a id=\"sys_a_0\" title=\"{0}\" onclick=\"fnSelectCourseSystem(0,this);\">{0}{1}</a>", TopCourseSystemName, arrow == 1 ? "<i></i>" : "");
            }
            else
            {
                treeStr.AppendFormat("<a id=\"sys_a_0\" title=\"{0}\" onclick=\"fnPopSelectCousreSystem(0,this);\">{0}{1}</a>", TopCourseSystemName, arrow == 1 ? "<i></i>" : "");
            }
            AddChild(0, Co_CourseSystemList, treeStr, type, checkBoxFlag, radioFlag);
            treeStr.Append("</li>");
            treeStr.AppendLine("</ul>");
            return Json(new
            {
                popTreeHtml = treeStr.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        private void AddChild(int partentID, IEnumerable<Co_CourseSystem> courseSystemList, StringBuilder treeStr, int type, int checkBoxFlag, int radioFlag = 0, int arrow = 0)
        {
            var sublist = courseSystemList.Where(p => p.ParentID == partentID);
            if (sublist.Any())
            {
                treeStr.Append("<ul>");
                foreach (Co_CourseSystem courseSys in sublist)
                {
                    if (radioFlag == 0)
                    {
                        if (checkBoxFlag == 1)
                        {
                            treeStr.AppendFormat(
                                "<li id='{0}' class='pNote'><input name='chbCourseSystem' type='checkbox'  id='chb_{0}'  class='fll' value='{0}'/>",
                                courseSys.Id);
                        }
                        else
                        {
                            treeStr.AppendFormat("<li id='{0}' class='pNote'> ", courseSys.Id);
                        }
                    }
                    else
                    {
                        treeStr.AppendFormat(
                            "<li id='{0}' class='pNote'><input name='chbCourseSystem' type='radio'  id='chb_{0}'  class='fll' value='{0}'/>",
                            courseSys.Id);
                    }

                    if (type == 0)
                    {
                        treeStr.AppendFormat(
                            "<a title=\"{1}\" onclick=\"fnSelectCourseSystem({0},this);\" id=\"sys_a_{0}\">{1}{2}</a>",
                            courseSys.Id, courseSys.CourseSystemName.HtmlXssEncode(), arrow == 1 ? "<i></i>" : "");
                    }
                    else
                    {
                        treeStr.AppendFormat(
                            "<a title=\"{1}\" onclick=\"fnPopSelectCousreSystem({0},this);\" id=\"sys_a_{0}\">{1}{2}</a>",
                            courseSys.Id, courseSys.CourseSystemName.HtmlXssEncode(), arrow == 1 ? "<i></i>" : "");
                    }
                    treeStr.AppendLine();
                    AddChild(courseSys.Id, courseSystemList, treeStr, type, checkBoxFlag, radioFlag);
                    treeStr.AppendLine();
                    treeStr.Append("</li>");
                }
                treeStr.Append("</ul>");
            }
        }

        /// <summary>
        ///     保存分类
        /// </summary>
        /// <returns></returns>
        public JsonResult SubmitCourseSystem(Co_CourseSystem courseSystem, string sys_PayGrade, string hidOldPayGradeIds = "")
        {
            if (courseSystem.Id == 0)
            {
                //添加
                _courseSysBL.AddCourseSystem(courseSystem);
                //if (!string.IsNullOrEmpty(sys_PayGrade))
                //{
                //    string[] payGradeArray = sys_PayGrade.Split(',');
                //    foreach (var item in payGradeArray)
                //    {
                //        if (!string.IsNullOrEmpty(item))
                //        {
                //            _sysPayGradeBL.AddSys_SortLinkGrade(new Sys_SortLinkGrade() { GradeID = int.Parse(item), SortID = courseSystem.Id });
                //        }
                //    }
                //}
                if (courseSystem.Id > 0)
                    return Json(new
                    {
                        result = 1,
                        content = "保存成功！"
                    }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new
                    {
                        result = 0,
                        content = "保存失败"
                    }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //修改
                Co_CourseSystem courseSys = _courseSysBL.GetCourseSystem(courseSystem.Id);
                courseSys.CourseSystemName = courseSystem.CourseSystemName;
                courseSys.Memo = courseSystem.Memo;
                bool newid = _courseSysBL.UpdateCourseSystem(courseSys);
                if (newid)
                {
                    //if (!string.IsNullOrEmpty(hidOldPayGradeIds))
                    //{

                    //    string[] CurrentpayGrade = sys_PayGrade.Split(',');// A B C 
                    //    string[] OldpayGrade = hidOldPayGradeIds.Split(',');// B C
                    //    foreach (var item in OldpayGrade)
                    //    {
                    //        if (!CurrentpayGrade.Contains(item))// 移除
                    //        {
                    //            if (!string.IsNullOrEmpty(item))
                    //            {
                    //                _sysPayGradeBL.DeleteSys_SortLinkGrade(courseSystem.Id, int.Parse(item));
                    //            }
                    //        }
                    //    }
                    //    foreach (var item in CurrentpayGrade)
                    //    {
                    //        if (!OldpayGrade.Contains(item))// 新增
                    //        {
                    //            if (!string.IsNullOrEmpty(item))
                    //            {
                    //                _sysPayGradeBL.AddSys_SortLinkGrade(new Sys_SortLinkGrade() { GradeID = int.Parse(item), SortID = courseSystem.Id });
                    //            }
                    //        }
                    //    }
                    //}
                    return Json(new
                            {
                                result = 1,
                                content = "保存成功"
                            }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new
                    {
                        result = 0,
                        content = "保存失败"
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        ///<summary>
        /// 根据ID删除实体
        ///</summary>
        ///<returns></returns>
        [Filter.SystemLog("删除胜任力课程体系", LogLevel.Info)]
        public JsonResult DeleteCourseSystemByID(int id)
        {
            if (_courseSysBL.GetCo_CourseSystemList(" IsDelete=0 AND ParentId= " + id).Count > 0)
            {
                return Json(new
                {
                    result = 0,
                    content = "该体系下含有子体系不能删除！"
                }, JsonRequestBehavior.AllowGet);
            }

            if (_sysPayGradeBL.CourseSystemContainCourseNum(id) > 0)
            {
                return Json(new
                {
                    result = 0,
                    content = "该体系下含有课程，不能删除！"
                }, JsonRequestBehavior.AllowGet);
            }
            if (_courseSysBL.DeleteCourseSystem(id))
                return Json(new
                {
                    result = 1,
                    content = "删除成功！"
                }, JsonRequestBehavior.AllowGet);
            else
            {
                return Json(new
                {
                    result = 0,
                    content = "删除失败！"
                }, JsonRequestBehavior.AllowGet);
            }
        }





        /// <summary>
        ///     验证分类
        /// </summary>
        public JsonResult CheckCourseSystemName(string CourseSystemName, int Id, int ParentID)
        {
            return Json(!_courseSysBL.IsExist(CourseSystemName.ReplaceSingleSql(), ParentID, Id), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 胜任力课程
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortGradeCourse"></param>
        /// <param name="sys_PayGrade">薪酬级别</param>
        /// <param name="sortIds">体系</param>
        /// <returns></returns>
        [Filter.SystemLog("编辑胜任力课程", LogLevel.Info)]
        public JsonResult SubmitSys_SortGradeCourse(Sys_SortGradeCourse sortGradeCourse, string sys_PayGrade = "", string sortIds = "")
        {
            if (sortGradeCourse.Id == 0)
            {
                //添加
                _sysPayGradeBL.AddSys_SortGradeCourse(sortGradeCourse);//添加课程

                // 
                loopAddSortGradeCourse(sortGradeCourse, sys_PayGrade, sortIds);
                if (sortGradeCourse.Id > 0)
                    return Json(new
                    {
                        result = 1,
                        content = "保存成功！"
                    }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new
                    {
                        result = 0,
                        content = "保存失败"
                    }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //修改
                bool newid = _sysPayGradeBL.UpdateSys_SortGradeCourse(sortGradeCourse);
                if (newid)
                {
                    _sysPayGradeBL.DeleteSys_SortLinkGrade(sortGradeCourse.Id);//删除所有
                    loopAddSortGradeCourse(sortGradeCourse, sys_PayGrade, sortIds);

                    return Json(new
                    {
                        result = 1,
                        content = "保存成功"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new
                    {
                        result = 0,
                        content = "保存失败"
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        private void loopAddSortGradeCourse(Sys_SortGradeCourse sortGradeCourse, string sys_PayGrade, string sortIds)
        {
            string[] payGradeArray = sys_PayGrade.Split(',');
            string[] sortIdsArray = sortIds.Split(',');
            foreach (var item in payGradeArray)
            {
                foreach (var itemSort in sortIdsArray)
                {
                    if (!string.IsNullOrEmpty(item) && !string.IsNullOrEmpty(itemSort))
                    {
                        _sysPayGradeBL.AddSys_SortLinkGrade(new Sys_SortLinkGrade()
                        {
                            GradeID = int.Parse(item),
                            SortID = int.Parse(itemSort),
                            CourseId = sortGradeCourse.Id
                        });
                    }
                }
            }
        }


        /// <summary>
        /// 获取胜任力课程 列表
        /// </summary>
        /// <param name="sortIds"></param>
        /// <param name="gradeIds"></param>
        /// <param name="courseName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="showtype">0默认按照当前的薪酬级别进行查询  1没有薪酬级别</param>
        /// <returns></returns>
        public JsonResult GetCompetencyCourseList(string sortIds, string gradeIds, string courseName, int IsMust = -1, int pageSize = 20, int pageIndex = 1, int showtype = 0)
        {
            int totalCount = 0;
            string strWhere = " 1=1 ";

            //找到当前用户薪酬级别的ID           


            if (!string.IsNullOrEmpty(sortIds))
            {
                strWhere += " AND SortId in (select id from dbo.F_SplitIDs('" + sortIds + "')) ";
            }
            if (!string.IsNullOrEmpty(gradeIds))
            {
                strWhere += " AND gradeId in (select id from dbo.F_SplitIDs('" + gradeIds + "')) ";

            }
            else if (showtype == 0)
            {
                gradeIds = _sysPayGradeBL.GetPayGradeByUserId(CurrentUser.UserId).ToString();
                strWhere += " AND gradeId in (select id from dbo.F_SplitIDs('" + gradeIds + "')) ";
            }


            List<int> listCourseIds = _sysPayGradeBL.GetSys_SortLinkGradeList(strWhere).Select(i => i.CourseId).Distinct().ToList();
            string strWhereCourse = " 1=1 ";
            //TODO Current Fix  In  THis
            if ((!string.IsNullOrEmpty(sortIds)) || (!string.IsNullOrEmpty(gradeIds)))
            {
                strWhereCourse += " AND Id in (select id from dbo.F_SplitIDs('" + string.Join(",", listCourseIds) + "')) ";
            }
            else
            {
                //strWhereCourse += " AND Id in (select id from dbo.F_SplitIDs('" + string.Join(",",'select Sys_PayGrade.Id from [Sys_User] left join Sys_PayGrade on  [Sys_User].[PayGrade]=Sys_PayGrade.GradeName where [Sys_User].UserId='+CurrentUser.UserId) + "'))";
            }


            if (IsMust != -1)
            {
                strWhereCourse += " AND IsMust= " + IsMust;
            }
            if (!string.IsNullOrEmpty(courseName))
            {
                strWhereCourse += " AND Name like '%" + courseName.ReplaceSql() + "%' ";
            }
            var list = _sysPayGradeBL.GetSys_SortGradeCourseList(strWhereCourse);
            var totalcount = list.Count;
            list = list.Skip(((pageIndex - 1) * pageSize)).Take(pageSize).ToList();
            int n = 0;
            var itemArray = new object[list.Count()];
            foreach (var item in list)
            {
                var temp = new
                {
                    Id = item.Id,
                    Name = item.Name.HtmlXssEncode(),
                    OpenLeavel = item.OpenLeavel,
                    IsMust = typeof(Enums.IsMust).GetEnumName(item.IsMust.Value),
                    Type = typeof(Enums.Way).GetEnumName(item.Type.Value)

                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = totalcount
            }, JsonRequestBehavior.AllowGet);
        }


        ////<summary>
        ////    根据ID删除实体
        ////</summary>
        ////<returns></returns>
        public JsonResult DeleteCompetencyCourse(int id)
        {
            if (_sysPayGradeBL.DeleteSys_SortGradeCourse(id))
                return Json(new
                {
                    result = 1,
                    content = "删除成功！"
                }, JsonRequestBehavior.AllowGet);
            else
            {
                return Json(new
                {
                    result = 0,
                    content = "删除失败！"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
