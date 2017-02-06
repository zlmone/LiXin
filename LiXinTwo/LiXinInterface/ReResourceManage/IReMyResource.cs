using System;
using System.Collections.Generic;
using LixinModels.ReResourceManage;

namespace LiXinInterface.ReResourceManage 

{
    public interface IReMyResource
    {
        /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         Re_MyResource GetModel(int id);

        /// <summary>
        /// 增加一条数据
        /// </summary>
         /// <param name="model">要添加的实体对象</param>
         int AddModel(Re_MyResource model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
         bool UpdateModel(Re_MyResource model);

         
         /// <summary>
         /// 批量删除数据
         /// </summary>
         /// <param name="ids">ID集合格式为: 1,2,3,4</param>
         /// <returns>成功返回True，失败返回False</returns>
         bool DeleteBatchModel(string ids);

         /// <summary>
         /// 获取所有资源
         /// </summary>
         /// <returns></returns>
         Re_Resource GetAllResource();

         /// <summary>
         /// 获取所有资源列表
         /// </summary>
         /// <returns></returns>
        List<Re_Resource> GetResourceList(out int totalCount, string where, string orderBy, int startIndex = 1,
                                          int startLength = int.MaxValue);

        /// <summary>
        /// 根据ID获得资源
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
         Re_Resource GetResource(int Id);

         /// <summary>
         /// 获得用户的该资源下的信息
         /// </summary>
         /// <param name="userId"></param>
         /// <param name="courseId"></param>
         /// <returns></returns>
        Re_MyResource GetMyResource(int userId, int courseId);

        /// <summary>
        /// 插入该用户在该资源下的默认信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <param name="timeNow"></param>
        void insertResource(int userId, int courseId, DateTime timeNow);

        /// <summary>
        /// 判断该用户该资源下是否有信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        List<Re_MyResource> IsExist(int userId, int courseId);

        /// <summary>
        /// 评分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <param name="type"></param>
        /// <param name="man"></param>
        /// <param name="score"></param>
        /// <param name="manL"></param>
        /// <param name="scoreL"></param>
        /// <returns></returns>
        bool MyTotal(int userId, int courseId, int type, int man, double score, int manL, double scoreL);

        /// <summary>
        /// 评分—更新个人信息表信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <param name="type"></param>
        /// <param name="man"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        bool MyScoreTo(int userId, int courseId, int type, int man, double score);

        /// <summary>
        /// 获得阅读人数
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        List<Re_MyResource> GetNum(int courseId);

        /// <summary>
        /// 更新阅读人数
        /// </summary>
        /// <param name="num"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        bool ReaderNum(int num, int courseId);
    }
}