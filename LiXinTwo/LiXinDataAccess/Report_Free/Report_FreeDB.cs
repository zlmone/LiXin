using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using LiXinModels;
using System.Data;
using LiXinModels.SystemManage;

namespace LiXinDataAccess
{
    public class Report_FreeDB : BaseRepository
    {

        /// <summary>
        /// 授课人的自动折算
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ReportFreeDetails> GetTeacherScoreList(string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = @"select * from (
SELECT syu.UserId,Realname,Username,DeptName,TrainGrade,CPA,CPANo,DeptId,
cc.CourseName ClassName, ConvertScore,GettScore,GetCPAScore,typeForm,fuo.Year,IsCPA
FROM Free_UserOtherApply fuo
LEFT JOIN Co_Course cc ON fuo.ApplyID=cc.Id
LEFT JOIN Sys_User syu ON syu.UserId=fuo.ApplyUserID
WHERE fuo.typeForm=3 AND fuo.IsDelete=0) t where  " + where;
                return conn.Query<ReportFreeDetails>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取授课人
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ReportFreeDetails> GetCPATeacherList(string where="1=1")
        {
            using (var conn=OpenConnection())
            {
                var sql = string.Format(@"SELECT  syu.UserId,syu.Realname,syu.DeptName,syu.TrainGrade,syu.CPA,syu.CPANo,fuat.ClassName,fuat.ConvertScore,syu.DeptId
,typeForm,fuo.tScore GettScore,fuo.CPAScore GetCPAScore,syu.Username
FROM   Free_UserOtherApply fuo
LEFT JOIN Free_UserApplyTeacherDetails fuat ON fuo.ID=fuat.userApplyID OR fuo.fromID=fuat.userApplyID
LEFT JOIN Sys_User syu ON syu.UserId=fuo.ApplyUserID 
WHERE fuo.IsDelete=0 and  fuat.IsDelete=0 and   ApproveStatus=1 AND  DepApproveStatus=1 and  ApplyType=3  AND (typeForm=0 OR   (typeForm=1 AND CreateUserID=0)) and {0}", where);
                return conn.Query<ReportFreeDetails>(sql).ToList();

            }
        }

        /// <summary>
        /// 授课人的信息
        /// </summary>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetTeacherCourse(string where)
        {
            using (var conn=OpenConnection())
            {
                var sql = @"SELECT distinct cc.Id,cc.CourseName ApplyName_New, CourseLength ConvertScore,EndTime ApplyTime,Teacher,3 typeForm 
,syu.CPA,syu.Realname,syu.DeptName,syu.TrainGrade,syu.UserId,syu.IsTeacher,syu.CPANo,syu.Username,syu.DeptId
FROM Co_Course cc
LEFT JOIN Sys_User syu ON syu.UserId=cc.Teacher
WHERE EndTime<getdate() AND Publishflag=1  AND Way=1  AND cc.IsDelete=0 AND CourseFrom=2
AND syu.IsTeacher=1 and " + where;

                return conn.Query<Free_UserOtherApply>(sql).ToList();
            }

        }

        /// <summary>
        /// 获取其他形式
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ReportFreeDetails> GetOtherList(int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " GetCPAScore asc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(
SELECT row_number()OVER(ORDER BY {0}) number,count(*)OVER(PARTITION BY null) totalcount,* FROM(
SELECT distinct syu.UserId,syu.Realname,syu.DeptName,syu.TrainGrade,syu.CPA,syu.CPANo,
CASE WHEN typeForm=2 THEN (SELECT CourseName FROM Co_Course WHERE Id=fuo.ApplyID)  ELSE foc.ApplyContent END ClassName
,typeForm ,fuo.GettScore,fuo.GetCPAScore,ConvertType=CASE WHEN typeForm=2 THEN 1 ELSE ConvertType end,ConvertScore,syu.Username,syu.DeptId
,ApplyID,fuo.Year
FROM   Free_UserOtherApply fuo
LEFT JOIN Free_OtherApplyConfig foc ON fuo.ApplyID=foc.ID
LEFT JOIN Sys_User syu ON syu.UserId=fuo.ApplyUserID 
WHERE fuo.IsDelete=0 and ApproveStatus=1 AND  DepApproveStatus=1 and fuo.ApplyType=1
  AND (typeForm=2 or (typeForm=0 OR   (typeForm=1 AND CreateUserID=0))) 
 
) result 
WHERE  {1}
) t WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", jsRenderSortField, where);
                 var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                 return conn.Query<ReportFreeDetails>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取符合时间的人员
        /// </summary>
        /// <param name="JoinDate"></param>
        /// <param name="CPACreateDate"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetUserByDate(string JoinDate, string CPACreateDate,string where="1=1")
        {
            using (var conn=OpenConnection())
            {
                var sql = string.Format(@"SELECT UserId ApplyUserID, Username,Realname, DeptId, DeptName, TrainGrade, CPANo, TrainGrade, CPA,JoinDate,CPACreateDate  FROM Sys_User
WHERE (JoinDate>='{0}' OR CPACreateDate>='{1}')
AND IsDelete=0 AND Status=0 AND IsTeacher<2 and {2}", JoinDate, CPACreateDate,where);
                return conn.Query<Free_UserOtherApply>(sql).ToList();
            }
        }

        /// <summary>
        /// 所有免修的人员ID
        /// </summary>
        /// <returns></returns>
        public List<int> GetFreeUserList(string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT DISTINCT  ApplyUserID  FROM Free_UserOtherApply
WHERE Status=1 AND ApproveStatus=1 AND DepApproveStatus=1 AND ApplyType=2
AND CreateUserID=0 and {0}", where);
                return conn.Query<int>(sql).ToList();
            }
        }
    }
}
