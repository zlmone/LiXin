using LiXinDataAccess.NewCourseManage;
using LiXinInterface.NewCourseManage;
using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL.NewCourseManage
{
    public class New_CourseAdviceBL : INew_CourseAdvice
    {
          private New_CourseAdviceDB newCourseAdviceDb;
          public New_CourseAdviceBL()
        {
            newCourseAdviceDb = new New_CourseAdviceDB();
        }


          public New_CourseAdvice Get(int Id)
          {
              return newCourseAdviceDb.Get(Id);
          }

          public void SubmitCl_CourseAdvice(New_CourseAdvice model)
          {
              newCourseAdviceDb.SubmitCl_CourseAdvice(model);
          }

          public bool ReplyCl_CourseAdvice(New_CourseAdvice model)
          {
              return newCourseAdviceDb.ReplyCl_CourseAdvice(model);
          }

          public bool Delete(int id)
          {
              return newCourseAdviceDb.Delete(id);
          }


          public List<New_CourseAdvice> GetList(string strWhere = "1=1")
          {
              return newCourseAdviceDb.GetList(strWhere);
          }

          public int GetCl_CourseAdviceCount(int CourseId, int UserId)
          {
              return newCourseAdviceDb.GetNew_CourseAdviceCount(CourseId, UserId);
          }

    }
}
