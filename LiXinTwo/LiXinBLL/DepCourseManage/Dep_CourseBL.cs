using LiXinCommon;
using LiXinDataAccess.DepCourseManage;
using LiXinDataAccess.Examination;
using LiXinInterface.DepCourseManage;
using LiXinModels.CourseManage;
using LiXinModels.DepCourseManage;
using LiXinModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL.DepCourseManage
{
    public class Dep_CourseBL : IDep_Course
    {
        private Dep_CourseDB dep_coursedb;
        private Dep_CoursepaperDB _coursePaperDB;
        private ExaminationDB _examinationDb;
        public Dep_CourseBL(Dep_CourseDB _dep_coursedb, Dep_CoursepaperDB coursePaperDB, ExaminationDB examinationDb)
        {
            dep_coursedb = _dep_coursedb;
            _coursePaperDB = coursePaperDB;
            _examinationDb = examinationDb;
        }


        public Dep_Course GetCo_Course(int Id)
        {
            return dep_coursedb.GetCo_Course(Id);
        }

        public Dep_Course GetCo_Course(int Id, int userid)
        {
            return dep_coursedb.GetCo_Course(Id, userid);
        }

        /// <summary>
        /// 新增课程
        /// </summary>
        /// <param name="model"></param>
        public void AddCourse(Dep_Course model)
        {
            dep_coursedb.AddCourse(model);
        }

        public bool UpdateCourseTimesByCode(string code, int courseTimes)
        {
            return dep_coursedb.UpdateCourseTimesByCode(code, courseTimes);
        }


        public bool InsertOrUpdateCourseMainPaper(Dep_Coursepaper coursePaper)
        {
            if (_coursePaperDB.IsExistCourseMainPaper(coursePaper.CourseId))
            {
                return _coursePaperDB.UpdateCourseMainPaper(coursePaper);
            }
            else
            {
                return _coursePaperDB.AddCoursePaper(coursePaper);
            }
        }

        public bool UpdateCourse(Dep_Course model)
        {
            return dep_coursedb.UpdateCourse(model);
        }

        /// <summary>
        /// 指定课程负责人
        /// </summary>
        /// <param name="id">课程ID</param>
        /// <param name="uid">负责人ID</param>
        /// <returns></returns>
        public bool ModifyCourseMaster(int id, string uid)
        {
            return dep_coursedb.ModifyCourseMaster(id, uid);
        }


        public List<Dep_Course> GetCourseTogetherList(out int totalCount, int way = 1, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Id DESC")
        {
            return dep_coursedb.GetCourseTogetherList(out totalCount, way, where, startIndex, pageLength, orderBy);
        }


        /// <summary>
        /// 修改课程单一状态
        /// </summary>
        /// <param name="flag">0:删除 1:发布</param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public bool ModifySingleCourse(int flag, int courseId)
        {
            return dep_coursedb.ModifySingleCourse(flag, courseId);
        }

        public bool IsExistCourseCode(string courseCode, int courseFrom = 2, int Id = 0, int way = 1)
        {
            return dep_coursedb.IsExistCourseCode(courseCode, courseFrom, Id, way);
        }



        /// <summary>
        /// （教育培训部）获得部门开放至全所审批列表
        /// </summary>
        /// <param name="totalCount">记录总数</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="teacherName">讲师名称</param>
        /// <param name="openUserName">提交人</param>
        /// <param name="isOpenOthers">审批状态0:未开放；1：待审批；2：审批通过；3：审批拒绝</param>
        /// <param name="startTime">开课时间-开始</param>
        /// <param name="endTime">开课时间-结束</param>
        /// <param name="where">条件语句</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序：默认按审批状态排序</param>
        /// <returns></returns>
        public List<Dep_Course> DepCourseOpenOthersCheck(out int totalCount, string courseName, string teacherName, string openUserName,
                                                         int isOpenOthers, string startTime, string endTime, string where = "",
                                                         int startIndex = 1, int pageLength = int.MaxValue, string orderBy = "")
        {
            if (!string.IsNullOrWhiteSpace(courseName))
            {
                where += string.Format(" and cc.CourseName like '%{0}%' ", courseName.Trim().ReplaceSql());
            }
            if (!string.IsNullOrWhiteSpace(teacherName))
            {
                where += string.Format(" and u.Realname like '%{0}%' ", teacherName.Trim().ReplaceSql());
            }
            if (!string.IsNullOrWhiteSpace(openUserName))
            {
                where += string.Format(" and u1.Realname like '%{0}%' ", openUserName.Trim().ReplaceSql());
            }
            if (isOpenOthers != -1)
            {
                where += string.Format(" and cc.IsOpenOthers={0} ", isOpenOthers);
            }
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                where += " and cc.StartTime>='" + startTime + "' ";
            }
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                where += " and cc.StartTime<='" + endTime + "' ";
            }
            return dep_coursedb.DepCourseOpenOthersCheck(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 根据ID更新审批信息
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateApproval(Dep_Course model)
        {
            return dep_coursedb.UpdateApproval(model);
        }

        public Dep_Course GetVideoCo_CourseById(int Id)
        {
            return dep_coursedb.GetVideoCo_CourseById(Id);
        }


        #region == 部门开课跟踪 ==
        /// <summary>
        /// （教育培训部）获得部门开课跟踪列表
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <param name="deptFlag">是否部门 0:部门；1：分所 默认为0</param>
        /// <param name="totalCount">记录总数</param>
        /// <param name="where">条件语句"  and ... "</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序：默认按Id倒序排序</param>
        /// <returns></returns>
        public List<Dep_Course> DepOpenCourseFollowingList(out int totalCount, string deptName, int deptFlag = 0, string where = "",
                                                   int startIndex = 1, int pageLength = int.MaxValue,
                                                    string orderBy = "")
        {
            if (deptFlag == 0)
            {
                where += string.Format(
                        @" and ( select count(*) 
                                  from (SELECT ID,deptName FROM dbo.f_GetDeptParentByDeptID(Dep_Course_B.DeptId) pardept 
                                         where pardept.deptName like '%上海%' ) as parcount 
                                 )> 0  --部门 
                            ");
            }
            else
            {
                where += string.Format(
                        @" and ( select count(*) 
                                  from (SELECT ID,deptName FROM dbo.f_GetDeptParentByDeptID(Dep_Course_B.DeptId) pardept 
                                         where pardept.deptName like '%上海%' ) as parcount 
                                 )<=0  --分所
                            ");
            }
            if (!string.IsNullOrWhiteSpace(deptName))
            {
                where += string.Format(" and DeptName like '%{0}%' ", deptName.Trim().ReplaceSql());
            }
            return dep_coursedb.DepOpenCourseFollowingList(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 实际开课详情-（教育培训部）获得部门开放至全所列表
        /// </summary>
        /// <param name="totalCount">记录总数</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="isMust">选修\必修 0:必修；1：选修 ；-1：全部(复选框全不选)； -2：全部（复选框全选）</param>
        /// <param name="isYearPlan">计划内\外 0：否；1：是 ； -1：全部(复选框全不选) ；-2：全部（复选框全选）</param>
        /// <param name="isOpenOthers">审批状态1：待审批；2：审批通过；3：审批拒绝 ；-1：待审批+通过+拒绝</param>
        /// <param name="startTime">开课时间-开始</param>
        /// <param name="endTime">开课时间-结束</param>
        /// <param name="yearPlan">计划年度</param>
        /// <param name="deptId">创建部门</param>
        /// <param name="where">条件语句</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序：默认按审批状态排序</param>
        /// <returns></returns>
        public List<Dep_Course> DepCourseOpenOthersList(out int totalCount, string courseName, int isMust, int isYearPlan,
                                                         int isOpenOthers, string startTime, string endTime, int yearPlan, string deptId, string where = "",
                                                         int startIndex = 1, int pageLength = int.MaxValue, string orderBy = "")
        {
            where += string.Format(" and cc.YearPlan={0} and cc.DeptId in ({1}) ", yearPlan, deptId);
            if (!string.IsNullOrWhiteSpace(courseName))
            {
                where += string.Format(" and cc.CourseName like '%{0}%' ", courseName.Trim().ReplaceSql());
            }
            if (isMust != -1 && isMust != -2)
            {
                where += string.Format(" and cc.IsMust={0} ", isMust);
            }
            if (isYearPlan != -1 && isYearPlan != -2)
            {
                where += string.Format(" and cc.IsYearPlan={0} ", isYearPlan);
            }
            if (isOpenOthers != -1)
            {
                where += string.Format(" and cc.IsOpenOthers={0} ", isOpenOthers);
            }
            else
            {
                where += string.Format(" and cc.IsOpenOthers in (1,2,3) ");
            }
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                where += " and cc.StartTime>='" + startTime + "' ";
            }
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                where += " and cc.StartTime<='" + endTime + "' ";
            }
            return dep_coursedb.DepCourseOpenOthersCheck(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 实际开课详情-获得部门自学列表
        /// </summary>
        /// <param name="totalCount">记录总数</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="isYearPlan">计划内\外 0：否；1：是 ； -1：全部(复选框全不选) ；-2：全部（复选框全选）</param>
        /// <param name="yearPlan">计划年度</param>
        /// <param name="deptId">创建部门</param>
        /// <param name="where">条件语句</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public List<Dep_Course> DepCourseLearnSelfList(out int totalCount, string courseName, int isYearPlan,
                                                          int yearPlan, string deptId, string where = "",
                                                         int startIndex = 1, int pageLength = int.MaxValue, string orderBy = "", int type = 0, string dwhere = " 1=1", int isYearStatus = 0)
        {

            if (isYearStatus == 0)
            {
                where += string.Format(" and cc.YearPlan={0} and cc.DeptId in ({1}) and {2}", yearPlan, deptId, dwhere);
            }
            else
            {
                where += string.Format(@" and  cc.YearPlan={0} and (cc.DeptId in ({1}) OR cc.DeptId IN (SELECT DeptId FROM Dep_YearPlan
                                       WHERE Id in (SELECT YearId FROM Dep_LinkDepart
                                          WHERE DeptId={1}))) and {2}", yearPlan, deptId, dwhere);

            }


            if (!string.IsNullOrWhiteSpace(courseName))
            {
                where += string.Format(" and cc.CourseName like '%{0}%'", courseName.Trim().ReplaceSql());
            }
            if (isYearPlan != -1 && isYearPlan != -2)
            {
                where += string.Format(" and cc.IsYearPlan={0} ", isYearPlan);
            }
            var list = dep_coursedb.DepCourseLearnSelfList(out totalCount, where, startIndex, pageLength, orderBy);
            list.ForEach(p =>
            {

                p.OnlineTestUserCount = p.OnlineExampaperId == 0
                                            ? 0
                                            : _examinationDb.GetExamSendStudentByDeptSelfList(p.Id, p.OnlineExampaperId)
                                                  .Select(p1 => p1.UserID).Distinct().Count();
            });//在线测试人数
            return list;
        }
        #endregion

        /// <summary>
        /// 获取课程列表 普通获取List方式
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Dep_Course> GetCourseCommonList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY dep_Course.Code,dep_Course.Id DESC")
        {
            return dep_coursedb.GetCourseCommonList(out totalCount, where, startIndex, pageLength, orderBy);

        }

        public Dep_Course GetCo_CourseAllList(int Id)
        {
            return dep_coursedb.GetCo_CourseAll(Id);
        }



        /// <summary>
        /// 有评价的课程
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Dep_Course> GetCourseIndexList(out int totalcount, string where = " 1 = 1 ", int startIndex = 1, int startLength = int.MaxValue, string order = "Id asc")
        {
            var list = dep_coursedb.GetNoCPACourse(startIndex, startLength, where, order);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }



        public List<Dep_Course> GetCourseAvg(out int totalcount, int CourseID, int startIndex = 1, int startLength = int.MaxValue, string order = "tab.UserID asc")
        {
            int CoexmaID = 0;
            int TeexmaID = 0;
            Dep_Course comodel = dep_coursedb.GetCo_Course(CourseID);
            var ArrayValue = comodel.SurveyPaperId.Split(';');
            if (!string.IsNullOrEmpty(ArrayValue[0]))
            {
                var ArrValue = ArrayValue[0].Split(',');
                CoexmaID = Convert.ToInt32(ArrValue[1]);
            }
            if (!string.IsNullOrEmpty(ArrayValue[1]))
            {
                var ArrValue = ArrayValue[1].Split(',');
                TeexmaID = Convert.ToInt32(ArrValue[1]);
            }
            List<Dep_Course> list = dep_coursedb.GetCourseAvg(CourseID, CoexmaID, TeexmaID, startIndex, startLength, order);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            foreach (var item in list)
            {
                item.SurveyPaperId = comodel.SurveyPaperId;
                item.Realname = item.Realname.HtmlXssEncode();
                item.DeptName = item.DeptName.HtmlXssEncode();
                item.TeacherName = comodel.TeacherName.HtmlXssEncode();
                item.CourseId = CourseID;
                if (item.CoAvg == null)
                {
                    item.CoAvg = 0;
                }
                if (item.TeAvg == null)
                {
                    item.TeAvg = 0;
                }
            }
            return list;
        }



        #region == 部门指定查询 ==
        /// <summary>
        /// （部门负责人）获得部门指定查询列表
        /// </summary>
        /// <param name="totalCount">记录总数</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="teacherName">讲师名称</param>
        /// <param name="isOrder">课程性质2-指定 3-兼有 -1:指定+兼有</param>
        /// <param name="courseStatus">课程状态0-未开始 1-进行中 2-已结束 -1:全部</param>
        ///  <param name="deptId">创建部门ID</param>
        /// <param name="where">条件语句‘and ....’</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序：默认按Id倒序排序</param>
        /// <returns></returns>
        public List<Dep_Course> DepSelfCourseAppointSearch(out int totalCount, string courseName, string teacherName, int isOrder = -1
                                                         , int courseStatus = -1, int deptId = 0, string where = "",
                                                         int startIndex = 1, int pageLength = int.MaxValue, string orderBy = "")
        {
            where += string.Format(" and cc.DeptId={0} ", deptId);
            if (!string.IsNullOrWhiteSpace(courseName))
            {
                where += string.Format(" and cc.CourseName like '%{0}%' ", courseName.Trim().ReplaceSql());
            }
            if (!string.IsNullOrWhiteSpace(teacherName))
            {
                where += string.Format(" and u.Realname like '%{0}%' ", teacherName.Trim().ReplaceSql());
            }
            if (isOrder != -1)
            {
                where += string.Format(" and cc.IsOrder={0} ", isOrder);
            }
            if (courseStatus != -1)
            {
                if (courseStatus == 0)
                {
                    where += " and cc.StartTime>getDate() ";
                }
                else if (courseStatus == 1)
                {
                    where += " and (cc.StartTime<=getDate() and cc.EndTime>=getDate()) ";
                }
                else
                {
                    where += " and cc.EndTime<getDate() ";
                }
            }
            return dep_coursedb.DepSelfCourseAppointSearch(out totalCount, where, startIndex, pageLength, orderBy);
        }
        #endregion

        /// <summary>
        /// 我开放的课程指定查询
        /// </summary>
        /// <returns></returns>
        public List<Co_Course> GetMyOpenCourse(out int totalcount, int deptID, string where = " 1 = 1 ", int startIndex = 1, int pageLength = int.MaxValue, string jsRenderSortField = "Co_Course.Id desc")
        {
            var list = dep_coursedb.GetMyOpenCourse(deptID, where, startIndex, pageLength, jsRenderSortField);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }

        /// <summary>
        /// 查询课程指定的人员
        /// </summary>
        /// <param name="courseID">开放后的课程ID</param>
        /// <param name="deptCourseID">开放前的课程ID</param>
        /// <returns></returns>
        public List<Sys_User> GetCanOrderList(out int totalcount, int courseID, int deptCourseID, string where = "1=1", int startIndex = 1, int pageLength = int.MaxValue, string jsRenderSortField = " DeptName desc")
        {
            var list = dep_coursedb.GetCanOrderList(courseID, deptCourseID, where, startIndex, pageLength, jsRenderSortField);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }

        /// <summary>
        /// 用来取消指定
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetDropList(out int totalcount, int courseID, int deptCourseID, string where = "1=1", int startIndex = 1, int pageLength = int.MaxValue, string jsRenderSortField = "")
        {
            var list = dep_coursedb.GetDropList(courseID, deptCourseID, where, startIndex, pageLength, jsRenderSortField);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }

        /// <summary>
        /// 查处纳入考核范围的所有课程
        /// </summary>
        /// <returns></returns>
        public List<Dep_Course> GetDep_CourseList(int year)
        {
            return dep_coursedb.GetDep_CourseList(year);
        }
    }
}
