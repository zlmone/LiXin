using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using LixinModels.NewClassManage;
using Retech.Core;
using Retech.Data;
namespace LiXinDataAccess.NewClassManage
{
    public class NewGroupDB : BaseRepository
	{
	
	    /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public New_Group GetModel(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select * from New_Group where  Id=@Id ";
                return conn.Query<New_Group>(sqlstr, new { Id = id }).FirstOrDefault();
            }
        }
		
		/// <summary>
        /// 增加一条数据
        /// </summary>
         /// <param name="model">要添加的实体对象</param>
       public void AddModel(New_Group model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO dbo.New_Group (ClassId, GroupName, PersonCount, GroupNo, GroupIndex)
VALUES (@ClassId, @GroupName, @PersonCount, @GroupNo, @GroupIndex)
SELECT @@IDENTITY AS ID ";
                var param = new
                                {
                                    model.ClassId,
                                    model.GroupName,
                                    model.PersonCount,
                                    model.GroupNo,
                                    model.GroupIndex
                                };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }
        
         /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateModel(New_Group model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"UPDATE dbo.New_Group
SET ClassId = @ClassId,
	GroupName = @GroupName,
	PersonCount = @PersonCount,
	GroupNo = @GroupNo,
	GroupIndex = @GroupIndex
WHERE Id = @id ";
                var param = new
                                {
                                    model.Id,
                                    model.ClassId,
                                    model.GroupName,
                                    model.PersonCount,
                                    model.GroupNo,
                                    model.GroupIndex
                                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 根据条件获得组列表
        /// </summary>
        public List<New_Group> GetList(string where = "")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@" 
    select g.* from dbo.New_Group g
where 1=1 {0} ", where);
                return conn.Query<New_Group>(sql).ToList();
            }
        }
        
        
        
   
	}
}