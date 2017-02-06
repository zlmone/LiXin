using System;
using System.Collections.Generic;
using LiXinModels.User;
using LiXinModels.CourseManage;
using LiXinModels;

namespace LiXinInterface.User
{
    public interface IUser
    {
        /// <summary>
        ///     创建用户
        /// </summary>
        /// <param name="user"></param>
        int Add(Sys_User user);

        /// <summary>
        ///     更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool Update(Sys_User model);

        /// <summary>
        ///     删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool Delete(string userIds);

        /// <summary>
        ///     设置用户状态
        /// </summary>
        /// <returns></returns>
        bool SetUserStatus(string userIds, int userStatus, DateTime? FreezeTime);

        /// <summary>
        ///     获取用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetUserStatus(int userId);

        /// <summary>
        ///     获取用户冻结终止时间
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>返回是否是永久冻结 or 冻结时间</returns>
        string GetUserFreezeTime(int userId);

        /// <summary>
        ///     添加用户到角色
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        void AddRoleToUser(string userIds, string roleIds);

        /// <summary>
        ///     取消角色
        /// </summary>
        /// <returns></returns>
        void DeleteUserRole(int roleId, int userId);

        /// <summary>
        ///     根据用户ID获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Sys_User Get(int userId);

        /// <summary>
        ///     根据用户名获取用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Sys_User GetUserByName(string userName);

        /// <summary>
        ///     根据LoginId获取用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Sys_User GetUserByLoginId(string loginId);

        /// <summary>
        ///     判断 用户信息 是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool Exists(string jobNum, string userName, int userId);

        /// <summary>
        ///     获得数据列表
        /// </summary>
        /// <param name="strWhere">获取数据的Sql Where条件（不带WHERE）</param>
        List<Sys_User> GetList(string strWhere = " 1 = 1 ");

        /// <summary>
        ///     获取所有用户
        /// </summary>
        /// <returns></returns>
        List<Sys_User> GetList(out int totalCount, string where, int iDisplayStart = default(int),
                               int iDisplayLength = default(int), string orderBy = "ORDER BY Sys_User.UserId DESC");

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="totalCount">总数量</param>
        /// <param name="deptIds">部门Id的集合  【0：不查询】</param>
        /// <param name="postIds">岗位Id的集合  【0：不查询】</param>
        /// <param name="deptName">部门名称</param>
        /// <param name="postName">岗位名称</param>
        /// <param name="email">邮箱</param>
        /// <param name="jobNum">工号</param>
        /// <param name="realName">姓名</param>
        /// <param name="sex">性别  【99：全部；0：男；1：女】</param>
        /// <param name="status">状态  【99：全部；0：正常；1：冻结】</param>
        /// <param name="NotShowHasPost">是否显示有岗位的  【0：显示；1：不显示；默认为0】</param>
        /// <param name="NotShowHasDept">是否显示有部门的  【0：显示；1：不显示；默认为0】</param>
        /// <param name="iDisplayStart">分页【不传代表不分页，显示全部】</param>
        /// <param name="iDisplayLength">分页【不传代表不分页，显示全部】</param>
        /// <returns></returns>
        List<Sys_User> GetList(out int totalCount, string deptIds, string postIds, string deptName, string postName,
                                      string email, string jobNum, string realName, int sex, int status,
                                      int NotShowHasPost = 0, int NotShowHasDept = 0, int iDisplayStart = default(int),
                                      int iDisplayLength = default(int));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetUserState(int userId);

        bool UpdateUserPwdStatus(string userIds, int count, DateTime? time);

        /// <summary>
        /// 获取讲师列表
        /// </summary>
        /// <returns></returns>
        List<Sys_User> GetAllTeacher(out int totalCount, string RealName = "", int isTeacher = 0, string PayGrade = "", int pageSize = int.MaxValue, int pageIndex = 1, string isDelete = "  Sys_User.IsDelete = 0 ");

        /// <summary>
        /// 添加内部讲师
        /// </summary>
        /// <param name="userID"></param>
        void InsertInnerTeacher(string userID);

        /// <summary>
        /// 添加外聘讲师
        /// </summary>
        /// <param name="model"></param>
        void InsertOuterTeacher(Sys_User model);

        /// <summary>
        /// 删除讲师
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="teacher"></param>
        void DeleteTeacher(int userID, int teacher);

        /// <summary>
        /// 找出人员表的所有人员分类
        /// </summary>
        /// <returns></returns>
        List<string> GetUserType();

        #region 指纹库同步时使用

        /// <summary>
        /// 根据HRID获取用户
        /// </summary>
        /// <param name="hrID"></param>
        /// <returns></returns>
        Sys_User GetUserByHrID(string hrID);

        /// <summary>
        /// 判断 用户信息 是否存在
        /// </summary>
        /// <param name="userId">用户名</param>
        /// <returns></returns>
        bool ExistUserFinger(int userId);

        /// <summary>
        ///     增加一条数据
        /// </summary>
        /// <param name="model"></param>
        void AddUserFinger(Sys_UserFinger model);

        /// <summary>
        ///     更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <param name="flag">0:HR同步；1：原系统同步 </param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateUserFinger(Sys_UserFinger model, int flag);

        #endregion

        #region 个人中心
        /// <summary>
        /// 个人算学时
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        decimal GetUserScore(int uid, Sys_ParamConfig cpazq);

        /// <summary>
        /// 获取个人信息和CPA学时
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Sys_User GetmodelCAP(int uid);

        /// <summary>
        /// 学分前5名
        /// </summary>
        /// <returns></returns>
        List<Sys_User> GetTop5Credit();

        /// <summary>
        /// 更新头像
        /// </summary>
        bool UpdatePhoto(string photourl, int userid);

        /// <summary>
        /// 我的年度培训目标
        /// </summary>
        List<Co_CourseShow> GetMyCourse(int userid, int year, int way,out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string Order = "asc", string where = "1=1");

        /// <summary>
        /// 我的年度培训目标(CPA)
        /// </summary>
        List<Co_CourseShow> GetMyCPACoursebyWay(int userid, int year, out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string Order = "asc", string where = "1=1");

        /// <summary>
        /// CPA年度课程
        /// </summary>
        List<Co_CourseShow> GetMyCPACourse(int userid, int year, out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string Order = "asc", string where = "1=1");

        /// <summary>
        /// CPA周期课程
        /// </summary>
        List<Co_CourseShow> GetMyCPACourse(int userid, string bentime, string endtime, out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string Order = "asc", string where = "1=1");
        #endregion
    }
}