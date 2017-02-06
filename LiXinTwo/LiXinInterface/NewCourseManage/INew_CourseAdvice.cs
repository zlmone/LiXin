using LiXinModels.CourseLearn;
using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.NewCourseManage
{
    public interface INew_CourseAdvice
    {

        New_CourseAdvice Get(int Id);

        void SubmitCl_CourseAdvice(New_CourseAdvice model);

        bool ReplyCl_CourseAdvice(New_CourseAdvice model);

        bool Delete(int id);

        List<New_CourseAdvice> GetList(string strWhere = "1=1");

        int GetCl_CourseAdviceCount(int CourseId, int UserId);
    }
}
