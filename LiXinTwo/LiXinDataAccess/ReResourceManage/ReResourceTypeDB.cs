using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using LixinModels.ReResourceManage;
using Retech.Core;
using Retech.Data;
namespace LiXinDataAccess.ReResourceManage
{
	public class ReResourceTypeDB : BaseRepository
	{
	    /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Re_ResourceType GetModel(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select * from Re_ResourceType where  ResourceTypeID=@ResourceTypeID ";
                return conn.Query<Re_ResourceType>(sqlstr, new { ResourceTypeID = id }).FirstOrDefault();
            }
        }
		
		/// <summary>
        /// 增加一条数据
        /// </summary>
         /// <param name="model">要添加的实体对象</param>
       public void AddModel(Re_ResourceType model)
        {
            using (IDbConnection conn = OpenConnection())
            {

                const string sqlwhere = @" INSERT INTO dbo.Re_ResourceType (TypeName, TypeDec, ParentID, IsDelete)
VALUES (@TypeName, @TypeDec, @ParentID, @IsDelete)
SELECT @@IDENTITY AS ID ";
                var param = new
                                {
                                    model.TypeName,
                                    model.TypeDec,
                                    model.ParentID,
                                    model.IsDelete
                                };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.ResourceTypeID = decimal.ToInt32(id);
            }
        }
        
         /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateModel(Re_ResourceType model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @" UPDATE dbo.Re_ResourceType
SET TypeName = @TypeName,
	TypeDec = @TypeDec,
	ParentID = @ParentID,
	IsDelete = @IsDelete
WHERE ResourceTypeID = @ResourceTypeID ";
                var param = new
                                {
                                    model.ResourceTypeID,
                                    model.TypeName,
                                    model.TypeDec,
                                    model.ParentID,
                                    model.IsDelete
                                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }
        
        
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteModel(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere =
                    @" UPDATE dbo.Re_ResourceType set IsDelete = 1 where ResourceTypeID=@ResourceTypeID ";
                var param = new { ResourceTypeID =id};
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 判断类别名称是否存在
        /// </summary>
        /// <param name="sortName">类别名称</param>
        /// <param name="id">分类ID</param>
        /// <param name="parentId">分类父级ID</param>
        /// <returns></returns>
        public bool IsExist(string sortName,  int id = 0, int parentId = 0)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere =
                    "select count(*) from Re_ResourceType WHERE TypeName=@TypeName and ParentID=@ParentID and IsDelete = 0 ";
                if (id > 0)
                    sqlwhere += " and ResourceTypeID <> " + id + " ";
                var param = new
                {
                    TypeName=sortName,
                    ParentID = parentId
                };
                int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count > 0;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <param name="strWhere">条件语句</param>
        /// <returns></returns>
        public List<Re_ResourceType> GetResourceTypeList(string strWhere = "1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string selectSql = "select * from Re_ResourceType where " + strWhere + " and IsDelete = 0 ";
                return connection.Query<Re_ResourceType>(selectSql).ToList();
            }
        }

        /// <summary>
        /// 判断类别下是否存在子节点或资源
        /// </summary>
        /// <param name="typeId">类别ID</param>
        /// <returns>true:存在</returns>
        public bool IsExistsChildNodeOrResource(int typeId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string selectSql = string.Format("select count(*) from Re_ResourceType where IsDelete = 0 and ParentID={0} ",typeId);
                int childFlag=connection.Query<int>(selectSql).FirstOrDefault();

                string selectSql1 = string.Format("select count(*) from Re_Resource where Status = 0 and ResourceTypeID={0} ",typeId);
                int resourceFlag=connection.Query<int>(selectSql1).FirstOrDefault();
                return (childFlag > 0 || resourceFlag > 0);
            }
        }
	}
}