using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LiXinControllers;
using LiXinInterface.Examination;
using LiXinModels.Examination.DBModel;
using System.Web;
using LiXinControllers.Filter;

namespace LiXinExam.Controllers
{
    public class KnowledgeKeyController : BaseController
    {
        protected IKnowledgeKey EKnowledgeKeyBL;
        protected IQuestion EQuestionBL;

        public KnowledgeKeyController(IKnowledgeKey eKnowledgeKeyBL, IQuestion _EQuestionBL)
        {
            EKnowledgeKeyBL = eKnowledgeKeyBL;
            EQuestionBL = _EQuestionBL;
        }

        #region 页面呈现

        /// <summary>
        ///     编辑知识点页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult EditKnowledgeKey(int id=0)
        {
            return View(id == 0 ? new tbKnowledgeKey() : EKnowledgeKeyBL.GetSingleByID(id));
        }

        /// <summary>
        /// 部门分所编辑知识点页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult DepEditKnowledgeKey(int id = 0)
        {
            var key = id == 0 ? new tbKnowledgeKey() : EKnowledgeKeyBL.GetSingleByID(id);
            return View("EditKnowledgeKey",key);
        }

        /// <summary>
        ///     知识点列表呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult KnowledgeKeyList()
        {
            ViewBag.Flag = 0;//事务所层面的管理
            return View();
        }

        /// <summary>
        /// 部门分所知识点列表呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult DepKnowledgeKeyList()
        {
            ViewBag.Flag = 1;//部门分所层面的管理
            ViewBag.Departments = GetAllSubDepartments();
            return View("KnowledgeKeyList");
        }

        #endregion

        #region 方法

        /// <summary>
        ///     获取所有的知识点
        /// </summary>
        /// <param name="keyName">关键字</param>
        /// <param name="pageSize">没有显示条数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        public JsonResult GetAllKnowledgeKeys(string keyName = "", int pageSize = 20, int pageIndex = 1, int deptId=0)
        {
            var totalCount = 0;
            var keyList = EKnowledgeKeyBL.GetAllKnowledgeKeys(ref totalCount, keyName, pageSize, pageIndex, new List<int> { deptId });
            var qulist = EQuestionBL.GetQuestionList();
            keyList.ForEach(p => { p.Number = qulist.Count(pa => pa.QuestionKey == p._id); });
            return Json(new {result = 1, dataList = keyList, recordCount = totalCount}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     保存知识点
        /// </summary>
        /// <param name="form">表单数据</param>
        /// <param name="id">知识点ID</param>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        [LiXinControllers.Filter.SystemLog("保存知识点", LogLevel.Info)]
        public JsonResult SubmitKnowledgeKey(FormCollection form, int id,int deptId=0)
        {
            var key = new tbKnowledgeKey
                {
                    _id = id,
                    KeyDescription = form["keyDescription"],
                    KeyName =HttpUtility.HtmlEncode(form["keyName"]),
                    Number = 0,
                    Status = 0,
                    DeptId=deptId
                };
            var newid = id == 0 ? EKnowledgeKeyBL.InsertKey(key) : EKnowledgeKeyBL.ModifyKey(key);
            return
                Json(newid > 0
                         ? new {result = 1, content = "成功", data = key}
                         : new {result = 0, content = "添加失败", data = key}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 判断是否已经存在此知识点
        /// </summary>
        /// <param name="keyName">知识点名称</param>
        /// <param name="id">知识点ID</param>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        public JsonResult IsExsitSortName(string keyName, int id = 0,int deptId=0)
        {
            return Json(EKnowledgeKeyBL.IsExsitSortName(keyName, id,deptId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     删除知识点
        /// </summary>
        /// <param name="id">知识点ID</param>
        /// <returns></returns>
        [LiXinControllers.Filter.SystemLog("删除知识点", LogLevel.Info)]
        public JsonResult DeleteKey(int id)
        {
            return
                Json(
                    EKnowledgeKeyBL.DeleteKey(id)
                        ? new {result = 1, content = "删除成功"}
                        : new {result = 0, content = "本知识点关联多个题目，不可以删除"}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     获取知识点
        /// </summary>
        /// <returns></returns>
        public JsonResult GetKnowledgeKey(int deptId=0)
        {
            var newList = new List<object>();
            EKnowledgeKeyBL.GetKnowledgeKey(false).Where(p=>p.DeptId==deptId).ToList().ForEach(p => newList.Add(new
                {
                    id = p._id,
                    name = p.KeyName
                }));
            return Json(newList, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}