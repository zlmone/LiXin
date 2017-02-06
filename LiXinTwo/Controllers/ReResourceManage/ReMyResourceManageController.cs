using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinControllers.Filter;
using LixinModels.ReResourceManage;
using LixinModels;


namespace LiXinControllers
{
    public partial class ReResourceManageController
    {

        #region 页面呈现

        //知识点总显示页面
        public ActionResult ReMyResourceManage()
        {
            //ReResourceBl
            Re_Resource ReMyResource = new Re_Resource();
            ReMyResource = ReMyResourceBl.GetAllResource();
            return View(ReMyResource);
        }

        //知识点详情显示
        public ActionResult ReMyResourceListShow()
        {
            int userId = CurrentUser.UserId;
            int courseId = Request.QueryString["id"].StringToInt32();//获得资源课程的编号
            int where = Request.QueryString["where"].StringToInt32();//获得页面的来源
            DateTime timeNow = System.DateTime.Now;
            List<Re_MyResource> exist = new List<Re_MyResource>();
            Re_Resource ReResource = new Re_Resource();
            Re_MyResource ReMyResource = new Re_MyResource();
            exist = ReMyResourceBl.IsExist(userId, courseId);
            if (exist.Count() == 0)
            {
                ReMyResourceBl.insertResource(userId, courseId, timeNow);
            }
            int num = ReMyResourceBl.GetNum(courseId).Count();
            ReMyResourceBl.ReaderNum(num, courseId);
            ReResource = ReMyResourceBl.GetResource(courseId);
            ReMyResource = ReMyResourceBl.GetMyResource(userId, courseId);
            ViewBag.ReMyResource = ReMyResource;
            ViewBag.where = where;
            return View(ReResource);
        }

        //知识点详情显示
        public ActionResult PreReMyResourceListShow()
        {
            var userId = CurrentUser.UserId;
            var courseId = Request.QueryString["id"].StringToInt32();//获得资源课程的编号
            var where = Request.QueryString["where"].StringToInt32();//获得页面的来源
            var timeNow = System.DateTime.Now;
            var exist = new List<Re_MyResource>();
            var ReResource = new Re_Resource();
            var ReMyResource = new Re_MyResource();
            exist = ReMyResourceBl.IsExist(userId, courseId);
            if (!exist.Any())
            {
                ReMyResourceBl.insertResource(userId, courseId, timeNow);
            }
            var num = ReMyResourceBl.GetNum(courseId).Count();
            ReMyResourceBl.ReaderNum(num, courseId);
            ReResource = ReMyResourceBl.GetResource(courseId);
            ReMyResource = ReMyResourceBl.GetMyResource(userId, courseId);
            ViewBag.ReMyResource = ReMyResource;
            ViewBag.where = where;
            return View("ReMyResourceListShow", ReResource);
        }
        #endregion

        #region 方法

        /// <summary>
        /// 获得查询内容
        /// </summary>
        /// <returns></returns>
        public JsonResult GetList(string courseName, string kind, int way, int resourceTypeId, int pageSize = int.MaxValue, int pageIndex = 1)
        {
            int userDept = CurrentUser.DeptId;
            int userId = CurrentUser.UserId;
            string where = "  1=1 ";
            string orderBy = " order by Re_Resource.ResourceID DESC ";
            if (courseName != "")
            {
                where += " and ResourceName like " + "'%" + courseName + "%'";
            }
            if (kind != "all")
            {
                where += " and Suffix=" + "'" + kind + "'";
            }
            if (resourceTypeId != 0)
            {
                where += "and ResourceTypeID in (SELECT rrt.ResourceTypeID FROM GetChildResourceTypeIds(" +
                         resourceTypeId + ") rrt)";
            }

            where += "  and ( ((select count(*) from Re_Resource where OpenFlag=3)>0 and" +
                     "(select count(*) from Sys_GroupUser where UserId=" + userId + " and groupid in " +
                     "(select id from dbo.F_SplitIDs(Re_Resource.OpenGroupObject)))>0" +
                     "or" +
                     "(select count(*) from Sys_User where DeptId=" + userDept + " and deptid in " +
                     "(select id from dbo.F_SplitIDs(Re_Resource.OpenDepartObject)))>0)" +
                     "or" +
                     "((select count(*) from Re_Resource where OpenFlag=1)>0" +
                     "and" +
                     "(select count(*) from Sys_GroupUser where UserId=" + userId + " and groupid in " +
                     "(select id from dbo.F_SplitIDs(Re_Resource.OpenGroupObject)))>0)" +
                     "or" +
                     "((select count(*) from Re_Resource where OpenFlag=2)>0" +
                     "and" +
                     "(select count(*) from Sys_User where DeptId=" + userDept + " and deptid in " +
                     "(select id from dbo.F_SplitIDs(Re_Resource.OpenDepartObject)))>0) )";

            if (courseName != "")
            {
                where += " and ResourceName like " + "'%" + courseName + "%'";
            }
            if (resourceTypeId != 0)
            {
                where += "and ResourceTypeID in (SELECT rrt.ResourceTypeID FROM GetChildResourceTypeIds(" +
                         resourceTypeId + ") rrt)";
            }

            switch (way)
            {
                case -1:
                    orderBy = "  order by Re_Resource.ResourceID DESC ";
                    break;
                case 0:
                    orderBy = " order by Re_Resource.Score ASC ";
                    break;
                case 1:
                    orderBy = " order by Re_Resource.Score DESC ";
                    break;
                case 2:
                    orderBy = " order by Re_Resource.OpenNum ASC "; 
                    break;
                case 3:
                    orderBy = "order by Re_Resource.OpenNum DESC ";
                    break;
            }
            List<Re_Resource> listAll = new List<Re_Resource>();
            int totalCount = 0;
            listAll = ReMyResourceBl.GetResourceList(out totalCount, where, orderBy, pageIndex, pageSize);
            int n = 0;
            var itemArray = new object[listAll.Count()];
            foreach (var item in listAll)
            {
                var temp = new
                    {
                        ResourceID = item.ResourceID,
                        ResourceName = item.ResourceName,
                        ResourceTypeID = item.ResourceTypeID,
                        ThumbnailURL = item.ThumbnailURL,
                        ThumbnailURLStr=item.ThumbnailURLStr
                    };
                itemArray[n] = temp;
                n++;
            }
            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 评分
        /// </summary>
        /// <returns></returns>
        public JsonResult SetResourceScore(int ResourceID, int Score, int type, double OldScore, int OldMen, int TotalMen, double OldTotalScore, int ScoreTotal)
        {
            int userID = CurrentUser.UserId;
            double totalScore = Convert.ToDouble(((OldTotalScore + Score)/(TotalMen + 1)).ToString("#.00"));//所有总分
            double scoreTo = Convert.ToDouble(((OldScore + Score)/(OldMen + 1)).ToString("#.00"));//分类别的总分
            TotalMen = TotalMen + 1;//总评人数  
            OldMen = OldMen + 1;//分类别评分人数
            bool flag = false;
            flag = ReMyResourceBl.MyTotal(userID, ResourceID, type, ScoreTotal, totalScore, OldMen, scoreTo);
            if (flag == true)
            {
                flag = ReMyResourceBl.MyScoreTo(userID, ResourceID, type, OldMen, scoreTo);
            }

            return Json(new
            {
                result = 1,
                score = scoreTo,
                num = OldMen,
                totalSc = totalScore,
                totalNu = ScoreTotal,
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        }
    }

