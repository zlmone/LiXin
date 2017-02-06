using System.Collections.Generic;
using LiXinModels.NewClassManage;
using LiXinModels.User;
using LixinModels.NewClassManage;

namespace LiXinInterface.NewClassManage
{
    public interface INewGroupUser
    {
        /// <summary>
        /// 获得当前未分班人员列表
        /// <param name="where">条件语句格式" and ..."</param>
        /// </summary>
        List<ClassUser> GetClassUserList(string where = "", string ordersql = " order by u.UserId");
        /// <summary>
        /// 查询指定班下的相关人员
        /// <param name="where">条件语句格式" and ..."</param>
        /// </summary>
        List<New_GroupUser> GetList(string where = "");
        /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        New_GroupUser GetModel(int id);

        /// <summary>
        /// 根据ClassID和UserId获取model(主要用于获取人员所在组ID)
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <param name="userId">人员ID</param>
        /// <returns></returns>
        New_GroupUser GetModelByClassAndUser(int classId, int userId);

        /// <summary>
        /// 根据UserId获取model(主要用于获取人员所在班ID)
        /// </summary>
        /// <param name="userId">人员ID</param>
        /// <returns></returns>
        New_GroupUser GetModelByUserID(int userId);

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">要添加的实体对象</param>
        int AddModel(New_GroupUser model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateModel(New_GroupUser model);


        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="ids">ID集合格式为: 1,2,3,4</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteBatchModel(string ids);

        /// <summary>
        /// 根据班级Id和人员Id集合批量删除数据
        /// </summary>
        /// <param name="userIds">学员ID集合格式为: 1,2,3,4</param>
        /// <param name="classId">班级ID</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteBatchByUserAndClass(string userIds, int classId);

        /// <summary>
        /// 根据班级Id和人员Id集合批量调整人员班组
        /// </summary>
        /// <param name="userIds">学员ID集合格式为: 1,2,3,4</param>
        /// <param name="classId">老班级ID</param>
        /// <param name="newClassId">新班级ID</param>
        /// <param name="newGroupId">新组ID</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool BatchChangeClassGroup(string userIds, int classId, int newClassId, int newGroupId);

        /// <summary>
        /// 根据班级Id和人员Id集合批量调整人员班组，并且修改学号
        /// </summary>
        /// <param name="userIds">学员ID集合格式为: 1,2,3,4</param>
        /// <param name="classId">老班级ID</param>
        /// <param name="newClassId">新班级ID</param>
        /// <param name="newGroupId">新组ID</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool BatchChangeClassGroup(string userIds, int classId, int newClassId, int newGroupId, string NumberID);

        /// <summary>
        /// 获得当前未分班人员列表
        /// <param name="totalcount">记录总数</param>
        /// <param name="realName">姓名</param>
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// </summary>
        List<Sys_User> GetList(out int totalcount, string realName, int year = -1, int startIndex = 1,
                                    int startLength = int.MaxValue);

        /// <summary>
        /// 根据条件获取指定班组学员学号信息集合
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        List<string> GetNumberList(string where = "");

        /// <summary>
        /// 批量添加班组人员
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <param name="groupId">组ID</param>
        /// <param name="userIds">人员ID集合：1,2,3</param>
        void BatchAddNewGroupUser(int classId, int groupId, string userIds);

         /// <summary>
        /// 批量添加班组人员
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <param name="groupId">组ID</param>
        /// <param name="userIds">人员ID集合：1,2,3</param>
        void BatchAddNewGroupUser(int classId, int groupId, int userId, string NumberID);

        /// <summary>
        /// （根据分组规则排序，执行自动分班时使用）
        /// <param name="splitSex">性别不同</param>
        /// <param name="splitSchool">毕业院校不同</param>
        /// <param name="splitMajor">毕业专业不同</param>
        /// <param name="splitFirm">事务所实习经验的有无</param>
        ///  <param name="perClassPersonCount">每班人数</param>
        /// <param name="perGroupPersonCount">每组人数</param>
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="startLength">每页记录数</param>
        ///  <param name="orderBy">排序规则</param>
        /// </summary>
        void DoAutoSplitClass(bool splitSex, bool splitSchool, bool splitMajor, bool splitFirm
                              , int perClassPersonCount, int perGroupPersonCount
                              , int startIndex = 1, int startLength = int.MaxValue, string orderBy = "");
        /// <summary>
        /// 获得当前未分班人员总数
        /// </summary>
        int GetCountList();

        /// <summary>
        /// 获得班级名称（字母表示）  十进制转二十六进制
        /// </summary>
        /// <param name="d">班级索引号 如0、1、2代表A班、B班、C班。。。</param>
        /// <param name="uplowType">0：转成大写字母 1：转成小写字母</param>
        string DtoX(int d, int uplowType = 0);

        /// <summary>
        /// 查询人所在的组和班级
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        New_GroupUser GetClassInfoByUserID(int userID);

        /// <summary>
        /// 根据分班的数量，生成班级的名字
        /// </summary>
        /// <param name="SumCount"></param>
        /// <returns></returns>
        List<string> GetClassnoList(int SumCount);

        /// <summary>
        /// （根据分组规则排序，执行自动分班时使用）
        /// <param name="splitSex">性别不同</param>
        /// <param name="splitSchool">毕业院校不同</param>
        /// <param name="splitMajor">毕业专业不同</param>
        /// <param name="splitFirm">事务所实习经验的有无</param>
        ///  <param name="perClassPersonCount">每班人数</param>
        /// <param name="perGroupPersonCount">每组人数</param>
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="startLength">每页记录数</param>
        ///  <param name="orderBy">排序规则</param>
        /// </summary>
        void DoAutoSingleSplitClass(bool splitSex, bool splitSchool, bool splitMajor, bool splitFirm
                                  , int perClassPersonCount, int perGroupPersonCount, string orderBy = "");
    }
}
