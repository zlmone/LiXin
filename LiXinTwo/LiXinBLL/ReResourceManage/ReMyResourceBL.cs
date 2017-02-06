using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.ReResourceManage;
using LiXinInterface.ReResourceManage;
using LixinModels.ReResourceManage;

namespace LiXinBLL.ReResourceManage

{
    public class ReMyResourceBL : IReMyResource
    {
        private readonly ReMyResourceDB _reMyResourceDB = new ReMyResourceDB();

        /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Re_MyResource GetModel(int id)
        {
            return _reMyResourceDB.GetModel(id);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">要添加的实体对象</param>
        public int AddModel(Re_MyResource model)
        {
            _reMyResourceDB.AddModel(model);
            return model.Id;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateModel(Re_MyResource model)
        {
            return _reMyResourceDB.UpdateModel(model);
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="ids">ID集合格式为: 1,2,3,4</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteBatchModel(string ids)
        {
            return _reMyResourceDB.DeleteBatchModel(ids);
        }

        /// <summary>
        /// 获取所有资源
        /// </summary>
        /// <returns></returns>
        public Re_Resource GetAllResource()
        {
            return _reMyResourceDB.GetAllResource();
        }

        /// <summary>
        /// 获取所有资源列表
        /// </summary>
        /// <returns></returns>
        public List<Re_Resource> GetResourceList(out int totalCount, string where, string orderBy, int startIndex = 1, int startLength = int.MaxValue)
        {
            return _reMyResourceDB.GetResourceList(out totalCount, where, orderBy, startIndex, startLength);
        }

        /// <summary>
        /// 根据ID获得资源
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Re_Resource GetResource(int Id)
        {
            return _reMyResourceDB.GetResource(Id);
        }

        /// <summary>
        /// 获得用户的该资源下的信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public Re_MyResource GetMyResource(int userId, int courseId)
        {
            return _reMyResourceDB.GetMyResource(userId, courseId);
        }

        /// <summary>
        /// 插入该用户在该资源下的默认信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <param name="timeNow"></param>
        public void insertResource(int userId, int courseId, DateTime timeNow)
        {
            _reMyResourceDB.insertResource(userId,courseId,timeNow);
        }

        /// <summary>
        /// 判断该用户该资源下是否有信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<Re_MyResource> IsExist(int userId, int courseId)
        {
            return _reMyResourceDB.IsExist(userId, courseId);
        }

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
        public bool MyTotal(int userId, int courseId, int type, int man, double score, int manL, double scoreL)
        {
            return _reMyResourceDB.MyTotal(userId, courseId, type, man, score, manL, scoreL);
        }

        /// <summary>
        /// 评分—更新个人信息表信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <param name="type"></param>
        /// <param name="man"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool MyScoreTo(int userId, int courseId, int type, int man, double score)
        {
            return _reMyResourceDB.MyScoreTo(userId, courseId, type, man, score);
        }

        /// <summary>
        /// 获得阅读人数
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<Re_MyResource> GetNum(int courseId)
        {
            return _reMyResourceDB.GetNum(courseId);
        }

        /// <summary>
        /// 更新阅读人数
        /// </summary>
        /// <param name="num"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public bool ReaderNum(int num, int courseId)
        {
            return _reMyResourceDB.ReaderNum(num, courseId);
        }
    }
}