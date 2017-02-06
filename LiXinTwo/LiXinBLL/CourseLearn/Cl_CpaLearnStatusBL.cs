using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinInterface.CourseLearn;
using LiXinModels.CourseLearn;
using LiXinDataAccess.CourseLearn;

namespace LiXinBLL.CourseLearn
{
    public class Cl_CpaLearnStatusBL : ICl_CpaLearnStatus
    {
        #region CPA 成绩导入方法

        private Cl_CpaLearnStatusDB _cpaLearnStatusDb = new Cl_CpaLearnStatusDB();
        /// <summary>
        /// 某门课程某个学员的CPA 课程成绩是否已经导入  
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="getLength"></param>
        /// <returns></returns>
        public bool IsImport(int courseId, int userId, int getLength)
        {
            return _cpaLearnStatusDb.IsImport(courseId, userId, getLength);
        }

        /// <summary>
        ///  某门课程的CPA 课程成绩是否已经导入  完毕
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="getLength"></param>
        /// <returns></returns>
        public bool IsImportOver(int courseId)
        {
            return _cpaLearnStatusDb.IsImportOver(courseId);
        }

        /// <summary>
        /// 更新 学员获得的CPA学时
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="getLength"></param>
        /// <returns></returns>
        public bool UpdateCpaLearnStatus(int courseId, int userId, int getLength)
        {
            return _cpaLearnStatusDb.UpdateCpaLearnStatus(courseId, userId, getLength);

        }

        public bool UpdateCPALearnStatusByModel(Cl_CpaLearnStatus model)
        {
            return _cpaLearnStatusDb.UpdateCPALearnStatus(model);
        }


        /// <summary>
        /// 根据课程编号 获取 该课程下学员所获得的成绩详情
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<Cl_CpaLearnStatus> GetCPACourseScoreList(int courseId, string orderStr = "GetLength desc")
        {
            return _cpaLearnStatusDb.GetCPACourseScoreList(courseId, orderStr);
        }
        #endregion

        public void  SubscribeCPALearnStatus(Cl_CpaLearnStatus model)
        {
            _cpaLearnStatusDb.SubscribeCPALearnStatus(model);
        }


        public Cl_CpaLearnStatus GetCl_CpaLearnStatusByCourseId(int CourseId, int UserId, string whereStr = " 1 = 1 ")
        {
            return _cpaLearnStatusDb.GetCl_CpaLearnStatusByCourseId(CourseId, UserId, whereStr);
        }

        public Cl_CpaLearnStatus GetCl_CpaLearnStatusByLearnId(int LearnId, int UserId)
        {
            return _cpaLearnStatusDb.GetCl_CpaLearnStatusByLearnId(LearnId, UserId);
        }

        public bool UpdateGetCl_CpaLearnStatusByCourseId(int CourseId, int UserId, decimal GetLength,
                                                                      int IsPass)
        {
            return _cpaLearnStatusDb.UpdateGetCl_CpaLearnStatusByCourseId(CourseId, UserId, GetLength, IsPass);
        }

        /// <summary>
        /// 根据课程ID和学员ID 修改是否在记录学时 0：可以改   1不能改
        /// </summary>
        /// <param name="num"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool UpdateProgress(int num, int userid)
        {
            return _cpaLearnStatusDb.UpdateProgress(num, userid);
        }

        
        /// <summary>
        /// 获取查询集合
        /// </summary>
        /// <param name="wherestr">查询条件</param>
        public List<Cl_CpaLearnStatus> GetCpaCourse(string wherestr="1=1")
        {
            return _cpaLearnStatusDb.GetCpaCourse(wherestr);
        }

        public bool DeleteLearn(int learnId)
        {
            return _cpaLearnStatusDb.DeleteLearn(learnId);
        }


        /// <summary>
        /// 修复数据
        /// </summary>
        public void FInsertNewListCpa()
        {
            var list = _cpaLearnStatusDb.NewGetList(@"
                    select 
		Cl_CpaLearnStatus.Id 
		,Cl_CpaLearnStatus.CourseID
		,Cl_CpaLearnStatus.UserID
		,Cl_CpaLearnStatus.IsAttFlag
		,Cl_CpaLearnStatus.IsPass
		,Cl_CpaLearnStatus.Progress
		,Cl_CpaLearnStatus.LearnTimes
		,Cl_CpaLearnStatus.GetLength
		,Cl_CpaLearnStatus.LastUpdateTime
		,Cl_CpaLearnStatus.CpaFlag
		,Cl_CpaLearnStatus.GradeStatus
    from Cl_CpaLearnStatus 
              left join dbo.Co_Course on Cl_CpaLearnStatus.CourseId=Co_Course.Id
               where Co_Course.Way=2 and Co_Course.IsCPA=1 and Cl_CpaLearnStatus.[CpaFlag] in(0,2) and Co_Course.CourseFrom=2	   
                order by Cl_CpaLearnStatus.UserId 
                
            ");

            var iscpa = list.Where(p => p.CpaFlag == 2).ToList();

            foreach (var item in list)
            {
                Cl_CpaLearnStatus t = null;

                var a = list.Where(p => p.CourseID == item.CourseID && p.UserID == item.UserID).ToList();
                if (a.Count < 2)
                {
                    if(a.Any(p=>p.CpaFlag==2))
                    {
                        t = new Cl_CpaLearnStatus();                   
                        item.CpaFlag = 0;         
                        _cpaLearnStatusDb.UpdateCPALearnStatus(item);
                
                
                            Cl_CpaLearnStatus model  = item;
                            model.CpaFlag = 2;
                            _cpaLearnStatusDb.SubscribeCPALearnStatus(model);
                 
                      }
                }
                
            }
        }
        public List<Cl_CpaLearnStatus> GetCpaCourseByCourseId(int courseid)
        {
            return _cpaLearnStatusDb.GetCpaCourseByCourseId(courseid);
        }

        /// <summary>
        /// 根据课程ID和课程类型 删除学习记录
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="cpaflag"></param>
        /// <returns></returns>
        public bool DeleteByCourseIdAndCpaFlag(int courseid, int cpaflag)
        {
            return _cpaLearnStatusDb.DeleteByCourseIdAndCpaFlag(courseid,cpaflag);
        }

        /// <summary>
        /// 记录观看视频总时长
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="VedioTime"></param>
        /// <returns></returns>
        public bool UpdateVedioTime(int learnid, decimal VedioTime)
        {
            return _cpaLearnStatusDb.UpdateVedioTime(learnid, VedioTime);
        }

          /// <summary>
        /// 记录观看视频总时长累加
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="VedioTime"></param>
        /// <returns></returns>
        public bool UpdateVedioScormTime(int learnid, decimal VedioTime)
        {
            return _cpaLearnStatusDb.UpdateVedioScormTime(learnid, VedioTime);
        }
    }
}
