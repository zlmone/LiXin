using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseLearn;

namespace LiXinInterface.CourseLearn
{
    public interface ICl_CourseAdvice
    {
        /// <summary>
        /// 获取单个课前建议表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Cl_CourseAdvice Get(int Id);


        /// <summary>
        /// 添加课前建议
        /// </summary>
        /// <param name="model"></param>
        void SubmitCl_CourseAdvice(Cl_CourseAdvice model);

        /// <summary>
        /// 回复课前建议
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool ReplyCl_CourseAdvice(Cl_CourseAdvice model);

        /// <summary>
        /// 删除课前建议
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);

        /// <summary>
        /// 获取多个建议
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        List<Cl_CourseAdvice> GetList(string strWhere = "1=1");

        int GetCl_CourseAdviceCount(int CourseId, int UserId);
    }
}
