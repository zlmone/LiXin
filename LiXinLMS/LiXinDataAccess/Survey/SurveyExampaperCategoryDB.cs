using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.Survey;
using Retech.Data;

namespace LiXinDataAccess.Survey
{
    public partial class SurveyManageDB
    {
        /// <summary>
        /// 根据ID获取问卷分类
        /// </summary>
        /// <param name="sortID"></param>
        /// <returns></returns>
        public Survey_ExampaperCategory GetPaperSort(int sortID)
        {
            using (var connection = OpenConnection())
            {
                const string sqlstr = "select * from Survey_ExampaperCategory where CategoryId=@sortID";
                return connection.Query<Survey_ExampaperCategory>(sqlstr, new { sortID }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取所有问卷分类
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<Survey_ExampaperCategory> GetPaperSortList(string where = " IsDelete=0 ", string orderBy = " order by CategoryId desc ")
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select Survey_ExampaperCategory.* from Survey_ExampaperCategory where {0} {1} ", where, orderBy);
                return connection.Query<Survey_ExampaperCategory>(query).ToList();
            }
        }

        /// <summary>
        /// 增加问卷分类
        /// </summary>
        /// <param name="model">问卷分类实体</param>
        public bool AddPaperSort(Survey_ExampaperCategory model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into Survey_ExampaperCategory (CategoryName,ParentId,IsDelete) values (@CategoryName,@ParentId,@IsDelete) SELECT @@Identity as Id";
                var param = new
                {
                    model.CategoryName,
                    model.ParentId,
                    model.IsDelete
                };
                var id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.CategoryId = decimal.ToInt32(id);
                return model.CategoryId > 0;
            }
        }

        /// <summary>
        /// 修改问卷分类
        /// </summary>
        /// <param name="model">问卷分类</param>
        public bool EditPaperSort(Survey_ExampaperCategory model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"update Survey_ExampaperCategory set CategoryName=@CategoryName where CategoryId=@CategoryId";
                var param = new
                                {
                                    model.CategoryName,
                                    model.CategoryId
                                };
                var result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 删除问卷分类
        /// </summary>
        /// <param name="id">问卷分类ID</param>
        /// <returns></returns>
        public bool DeletePaperSort(int id)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"update Survey_ExampaperCategory set IsDelete=1 where CategoryId=@id";
                var param = new
                {
                    id
                };
                var result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 判断问卷分类是否存在
        /// </summary>
        /// <param name="sortName">问卷分类名称</param>
        /// <param name="sortID">问卷分类ID</param>
        /// <param name="parentID">父级ID</param>
        public bool IsExsitPaperSort(string sortName, int sortID, int parentID)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"select count(*) from Survey_ExampaperCategory where CategoryName=@sortName and CategoryId<>@sortID and ParentId=@parentID
and IsDelete=0";
                var param = new
                {
                    sortName,
                    sortID,
                    parentID
                };
                var count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count == 0;
            }
        }
    }
}
