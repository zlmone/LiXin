using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using LiXinModels;
using System.Data;
namespace LiXinDataAccess.CourseManage
{
   public class Co_SystemLinkCourseDB:BaseRepository
    {
       public List<Co_SystemLinkCourse> GetSysLinkCourseListBySystemId(int SystemId)
       {
           using (IDbConnection connection = OpenConnection())
           {
               string selectSql = @"select * from Co_SystemLinkCourse
                                    LEFT JOIN Co_Course ON   
                                    CourseId=Co_Course.Id AND Co_Course.IsDelete=0
               where    SystemId=" + SystemId;
               return connection.Query<Co_SystemLinkCourse>(selectSql).ToList();
           }

       }

       /// <summary>
       /// 根据SystemId  返回CourseId
       /// </summary>
       /// <param name="SystemId"></param>
       /// <returns></returns>
       public List<int> GetCourseIdListBySystemId(int SystemId)
       {
           using (IDbConnection connection = OpenConnection())
           {
               string selectSql = @"select CourseId from Co_SystemLinkCourse
                                    LEFT JOIN Co_Course ON   
                                    CourseId=Co_Course.Id AND Co_Course.IsDelete=0
               where    SystemId=" + SystemId;
               return connection.Query<int>(selectSql).ToList();
           }

       }
    }
}
