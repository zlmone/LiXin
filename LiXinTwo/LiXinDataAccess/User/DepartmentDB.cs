using System.Collections.Generic;
using System.Data;
using System.Linq;
using Retech.Core;
using Retech.Data;
using LiXinModels.User;

namespace LiXinDataAccess.User
{
    /// <summary>
    ///     Department部门信息(区域信息)
    /// </summary>
    public class DepartmentDB : BaseRepository
    {
        /// <summary>
        ///     判断 部门信息(区域信息) 是否存在
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public bool Exists(string DeptCode, string DeptName, int DeptId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere =
                    "select count(*) from Sys_Department WHERE IsDelete = 0 and (DeptCode=@DeptCode or DeptName=@DeptName)";
                if (DeptId != 0)
                    sqlwhere += " and DepartmentId <> " + DeptId;
                var param = new
                    {
                        DeptCode,
                        DeptName
                    };
                int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count > 0;
            }
        }

        /// <summary>
        ///     增加一条数据
        /// </summary>
        /// <param name="model"></param>
        public void Add(Sys_Department model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into Sys_Department(DeptCode,DeptName,ParentDeptId,Remark,TypeNum,DeptGrade,OldDeptId) values(@DeptCode,@DeptName,@ParentDeptId,@Remark,@TypeNum,@DeptGrade,@OldDeptId)
                SELECT @@Identity AS ID
";
                var param = new
                    {
                        model.DeptCode,
                        model.DeptName,
                        model.ParentDeptId,
                        model.Remark,
                        model.TypeNum,
                        model.DeptGrade,
                        model.OldDeptId
                    };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.DepartmentId = decimal.ToInt32(id);
            }
        }

        /// <summary>
        ///     更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool Update(Sys_Department model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere =
                    @"update Sys_Department set DeptCode=@DeptCode,DeptName=@DeptName,ParentDeptId=@ParentDeptId,Remark=@Remark,TypeNum=@TypeNum,DeptGrade=@DeptGrade,OldDeptId=@OldDeptId where DepartmentId=@DepartmentId
                
                    UPDATE Sys_User SET DeptCode=@DeptCode,DeptName=@DeptName WHERE DeptId=@DepartmentId
                ";
                var param = new
                    {
                        model.DepartmentId,
                        model.DeptCode,
                        model.DeptName,
                        model.ParentDeptId,
                        model.Remark,
                        model.TypeNum,
                        model.DeptGrade,
                        model.OldDeptId
                    };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        ///     删除一条数据
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public bool Delete(int departmentId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"update Sys_Department set IsDelete = 1 WHERE DepartmentId=@DepartmentId
                    update Sys_User set DeptId = -1 ,DeptCode = '',DeptName='' where DeptId = @DepartmentId ";
                var param = new
                    {
                        DepartmentId = departmentId
                    };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        ///     删除父级关系
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public bool DeleteParentDeptId(int parentId)
        {
            const string sqlwhere = "UPDATE Sys_Department SET ParentDeptId=-1 WHERE ParentDeptId=@parentId";
            using (IDbConnection connection = OpenConnection())
            {
                var param = new { parentId };
                return connection.Execute(sqlwhere, param) > 0;
            }
        }

        /// <summary>
        ///     得到一个对象实体
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public Sys_Department Get(int departmentId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = string.Format("select * from Sys_Department where IsDelete = 0 and DepartmentId={0}",
                                                departmentId);
                return conn.Query<Sys_Department>(sqlwhere).FirstOrDefault();
            }
        }

        /// <summary>
        ///     获得数据列表
        /// </summary>
        public List<Sys_Department> GetList(string strWhere = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "select *,dbo.GetDeptPathByDeptID(DepartmentId) DeptPath from Sys_Department where " + strWhere + " and Sys_Department.IsDelete = 0 ";
                return conn.Query<Sys_Department>(sqlwhere).ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Sys_Department> GetList(out int totalCount, string where = " 1 = 1", int startIndex = 0,
                                            int pageLength = int.MaxValue, string orderBy = "ORDER BY DepartmentId DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>("select count(1) from Sys_Department where " + where +
                                          " and Sys_Department.IsDelete = 0 ").First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,* from Sys_Department
    where " + where + @" and Sys_Department.IsDelete = 0 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<Sys_Department>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="deptId"></param>
        public void AddUserToDept(string userIds, int deptId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql =
                    @"UPDATE a SET a.DeptId=@DeptId,a.DeptCode=b.DeptCode,a.DeptName=b.DeptName FROM Sys_User a,Sys_Department b WHERE a.UserId in (" +
                    userIds + ") AND b.DepartmentId=@DeptId";
                var param = new { DeptId = deptId };
                connection.Execute(sql, param);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="deptId"></param>
        public void DeleteUserDept(string userIds)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "UPDATE Sys_User SET DeptId=-1,DeptCode='',DeptName='' WHERE  UserId in (" + userIds + ")";
                connection.Execute(sql);
            }
        }

        /// <summary>
        /// 根据当前ID找出所有子集部门
        /// </summary>
        /// <param name="deptIds"></param>
        /// <returns></returns>
        public List<Sys_Department> GetTreeDeptDown(string deptIds)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "  SELECT ID,deptName FROM dbo.f_GetDeptChildByDeptID(" + deptIds + ")";
                return connection.Query<Sys_Department>(sql).ToList();
            }
        }

        /// <summary>
        /// 根据当前ID找出所有父及部门
        /// </summary>
        /// <param name="deptIds"></param>
        /// <returns></returns>
        public List<Sys_Department> GetTreeDeptParent(string deptIds)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "  SELECT ID,deptName FROM dbo.f_GetDeptParentByDeptID(" + deptIds + ")";
                return connection.Query<Sys_Department>(sql).ToList();
            }
        }
    }
}