using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseLearn;

namespace LiXinInterface.CourseLearn
{
    public interface ICl_CpaLearnStatus
    {
        #region CPA 成绩导入方法

        /// <summary>
        /// 某门课程某个学员的CPA 课程成绩是否已经导入  
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="getLength"></param>
        /// <returns></returns>
        bool IsImport(int courseId, int userId, int getLength);

        /// <summary>
        ///  某门课程的CPA 课程成绩是否已经导入  完毕
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="getLength"></param>
        /// <returns></returns>
        bool IsImportOver(int courseId);

        /// <summary>
        /// 更新 学员获得的CPA学时
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="getLength"></param>
        /// <returns></returns>
        bool UpdateCpaLearnStatus(int courseId, int userId, int getLength);

        bool UpdateCPALearnStatusByModel(Cl_CpaLearnStatus model);

        /// <summary>
        /// 根据课程编号 获取 该课程下学员所获得的成绩详情
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        List<Cl_CpaLearnStatus> GetCPACourseScoreList(int courseId, string orderStr = "GetLength desc");
        #endregion


        void SubscribeCPALearnStatus(Cl_CpaLearnStatus model);


        /// <summary>
        /// 根据课程和用户查找状态
        /// </summary>
        /// <param name="CourseId">课程ID</param>
        /// <param name="UserId">用户ID</param>
        /// <param name="CpaFlag">0：视频课程；1：CPA课程；2：转换的CPA课程</param> 
        /// <returns></returns>
        Cl_CpaLearnStatus GetCl_CpaLearnStatusByCourseId(int CourseId, int UserId, string whereStr = " 1 = 1 ");


        /// <summary>
        /// 修改学时和状态
        /// </summary>
        /// <param name="CourseId">课程ID</param>
        /// <param name="UserId">用户ID</param>
        /// <param name="GetLength">学时</param>
        /// <param name="IsPass">0：未通过；1：通过</param>
        /// <returns></returns>
        bool UpdateGetCl_CpaLearnStatusByCourseId(int CourseId, int UserId, decimal GetLength,
                                                               int IsPass);

        /// <summary>
        /// 根据主键ID和人员ID 查找
        /// </summary>
        /// <param name="LearnId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Cl_CpaLearnStatus GetCl_CpaLearnStatusByLearnId(int LearnId, int UserId);

        /// <summary>
        /// 根据CPA课程ID 找出所预定的人员
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        List<Cl_CpaLearnStatus> GetCpaCourseByCourseId(int courseid);



        /// <summary>
        /// 根据课程ID和学员ID 修改是否在记录学时 0：可以改   1不能改
        /// </summary>
        /// <param name="num"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        bool UpdateProgress(int num, int userid);

        /// <summary>
        /// 获取查询集合
        /// </summary>
        /// <param name="wherestr">查询条件</param>
        List<Cl_CpaLearnStatus> GetCpaCourse(string wherestr = "1=1");

        /// <summary>
        /// 视频考试不过 清除学习
        /// </summary>
        /// <param name="learnId"></param>
        /// <returns></returns>
        bool DeleteLearn(int learnId);

        /// <summary>
        /// 根据课程ID和课程类型删除学习记录 0：视频课程；1：CPA课程；2：转换的CPA课程
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="cpaflag"></param>
        /// <returns></returns>
        bool DeleteByCourseIdAndCpaFlag(int courseid, int cpaflag);

        void FInsertNewListCpa();

        /// <summary>
        /// 修改视频进度
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="VedioTime"></param>
        /// <returns></returns>
        bool UpdateVedioTime(int learnid, decimal VedioTime);

        /// <summary>
        /// 记录观看视频总时长累加
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="VedioTime"></param>
        /// <returns></returns>
        bool UpdateVedioScormTime(int learnid, decimal VedioTime);
    }
}
