using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseLearn;
using LiXinModels.CourseManage;

namespace LiXinInterface.CourseLearn
{
    public interface ICl_Attendce
    {
        /// <summary>
        /// 获取单个考勤实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Cl_Attendce GetCl_Attendce(int CourseId, int UserId);

        /// <summary>
        /// 获取所有考勤
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<Cl_Attendce> GetList(string where = "1=1");


        /// <summary>
        /// 我的课程(讲师) 下考勤学员列表--暂时没用
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        List<Cl_Attendce> GetListByTeacher(out int totalCount, string courseId, int startIndex = 0,
                                           int pageLength = int.MaxValue);




        /// <summary>
        /// 我的考试列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="UserId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        List<Co_CourseShow> GetMyExamList(out int totalCount, int UserId = 0, string TrainGrade = "", int DeptId = 0, string where = " 1 = 1 ",
                                          int startIndex = 0,
                                          int pageLength = int.MaxValue);

        int GetMyExamListPassCount(int UserId = 0, string TrainGrade = "", int DeptId = 0, string where = " 1 = 1 ");


        /// <summary>
        /// 我的考勤列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        List<Co_CourseShow> GetMyAttendcsList(out int totalCount,int UserId, string where = " 1 = 1 ",
                                              int startIndex = 0,
                                              int pageLength = int.MaxValue, int falg = 0);

        /// <summary>
        /// 我的课前建议列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        List<Co_CourseShow> GetMyproposeList(out int totalCount,int UserId, string where = " 1 = 1 ",string num="1=1", int startIndex = 0,
                                             int pageLength = int.MaxValue);



        List<Co_CourseShow> GetMyAfterCourseList(out int totalCount, int UserId, string TrainGrade = "", int DeptId = 0,
                                                 string where = " 1 = 1 ", string ALLnum = "", int startIndex = 0,
                                                 int pageLength = int.MaxValue);


  

    }
}
