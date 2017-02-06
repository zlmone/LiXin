using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinInterface.CourseLearn;
using LiXinModels.CourseLearn;
using LiXinDataAccess.CourseLearn;

namespace LiXinBLL.CourseLearn
{
    public class Cl_CourseAdviceBL : ICl_CourseAdvice
    {
        private Cl_CourseAdviceDB clCourseAdviceDb;
        public Cl_CourseAdviceBL()
        {
            clCourseAdviceDb = new Cl_CourseAdviceDB();
        }

        public Cl_CourseAdvice Get(int Id)
        {
            return clCourseAdviceDb.Get(Id);
        }

        public void SubmitCl_CourseAdvice(Cl_CourseAdvice model)
        {
             clCourseAdviceDb.SubmitCl_CourseAdvice(model);
        }

        public bool ReplyCl_CourseAdvice(Cl_CourseAdvice model)
        {
            return clCourseAdviceDb.ReplyCl_CourseAdvice(model);
        }

        public bool Delete(int id)
        {
            return clCourseAdviceDb.Delete(id);
        }


        public List<Cl_CourseAdvice> GetList(string strWhere = "1=1")
        {
            return clCourseAdviceDb.GetList(strWhere);
        }

        public int GetCl_CourseAdviceCount(int CourseId, int UserId)
        {
            return clCourseAdviceDb.GetCl_CourseAdviceCount(CourseId, UserId);
        }
    }
}
