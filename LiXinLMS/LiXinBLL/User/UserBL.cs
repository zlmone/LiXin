using System;
using System.Collections.Generic;
using LiXinDataAccess.User;
using System.Linq;
using LiXinInterface.User;
using LiXinModels.User;
using LiXinModels.CourseManage;
using LiXinDataAccess.Examination;
using LiXinModels.Examination.DBModel;
using LiXinModels.CourseLearn;
using LiXinModels;
using System.Text.RegularExpressions;

namespace LiXinBLL.User
{
    public class UserBL : IUser
    {
        private readonly UserDB _userDB = new UserDB();
        private readonly ExaminationDB _ExamDB = new ExaminationDB();
        private readonly ExampaperDB _paperDB = new ExampaperDB();


        /// <summary>
        ///     创建用户
        /// </summary>
        /// <param name="user"></param>
        public int Add(Sys_User user)
        {
            _userDB.Add(user);
            return user.UserId;
        }

        /// <summary>
        ///     更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool Update(Sys_User model)
        {
            return _userDB.Update(model);
        }

        /// <summary>
        ///     删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool Delete(string userIds)
        {
            return _userDB.Delete(userIds);
        }

        /// <summary>
        ///     设置用户状态
        /// </summary>
        /// <returns></returns>
        public bool SetUserStatus(string userIds, int userStatus, DateTime? FreezeTime)
        {
            return _userDB.UpdateUserStatus(userIds, userStatus, FreezeTime);
        }

        /// <summary>
        ///     设置用户状态
        /// </summary>
        /// <returns></returns>
        public bool UpdateUserPwdStatus(string userIds, int count, DateTime? time)
        {
            return _userDB.UpdateUserPwdStatus(userIds, count, time);
        }

        /// <summary>
        ///     获取用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserStatus(int userId)
        {
            return _userDB.GetUserStatus(userId);
        }

        /// <summary>
        ///     获取用户冻结终止时间
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>返回是否是永久冻结 or 冻结时间</returns>
        public string GetUserFreezeTime(int userId)
        {
            return _userDB.GetUserFreezeTime(userId);
        }

        /// <summary>
        ///     添加用户到角色
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public void AddRoleToUser(string userIds, string roleIds)
        {
            string[] id = userIds.Split(',');
            for (int i = 0; i < id.Length; i++)
            {
                _userDB.AddRoleToUser(Convert.ToInt32(id[i]), roleIds);
            }
        }

        /// <summary>
        ///     取消角色
        /// </summary>
        /// <returns></returns>
        public void DeleteUserRole(int roleId, int userId)
        {
            _userDB.DeleteUserRole(userId, roleId);
        }

        /// <summary>
        ///     根据用户ID获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Sys_User Get(int userId)
        {
            return _userDB.Get(userId);
        }

        /// <summary>
        ///     根据用户名获取用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sys_User GetUserByName(string userName)
        {
            return _userDB.GetUserByName(userName);
        }

        /// <summary>
        ///     根据LoginId获取用户信息
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public Sys_User GetUserByLoginId(string loginId)
        {
            return _userDB.GetUserByLoginId(loginId);
        }

        /// <summary>
        ///     判断 用户信息 是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool Exists(string jobNum, string userName, int userId)
        {
            return _userDB.Exists(jobNum, userName, userId);
        }

        /// <summary>
        ///     获得数据列表
        /// </summary>
        /// <param name="strWhere">获取数据的Sql Where条件（不带WHERE）</param>
        public List<Sys_User> GetList(string strWhere = " 1 = 1 ")
        {
            return _userDB.GetList(strWhere);
        }

        /// <summary>
        ///     获取所有用户
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetList(out int totalCount, string where, int iDisplayStart = default(int),
                                      int iDisplayLength = default(int), string orderBy = "ORDER BY Sys_User.UserId DESC")
        {
            if (iDisplayLength != default(int))
                return _userDB.GetList(out totalCount, where, iDisplayStart, iDisplayLength, orderBy);
            return _userDB.GetList(out totalCount, where);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="totalCount">总数量</param>
        /// <param name="deptIds">部门Id的集合  【0 或者 空：不查询】</param>
        /// <param name="postIds">岗位Id的集合  【0 或者 空：不查询】</param>
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
        public List<Sys_User> GetList(out int totalCount, string deptIds, string postIds, string deptName, string postName,
                                      string email, string jobNum, string realName, int sex, int status,
                                      int NotShowHasPost = 0, int NotShowHasDept = 0, int iDisplayStart = default(int),
                                      int iDisplayLength = default(int))
        {
            string where = " Sys_User.IsDelete = 0 and Sys_User.IsTeacher < 2";
            if (deptIds != "0" && deptIds != "")
                where += string.Format(" and Sys_User.DeptId in ({0})", deptIds);
            if (postIds != "0" && postIds != "")
                where += string.Format(" and Sys_User.PostId in ({0})", postIds);
            if (!string.IsNullOrWhiteSpace(deptName))
                where += string.Format(" and Sys_User.DeptName like '%{0}%' ", deptName);
            if (!string.IsNullOrWhiteSpace(postName))
                where += string.Format(" and Sys_User.PostName like '%{0}%' ", postName);
            if (!string.IsNullOrWhiteSpace(email))
                where += string.Format(" and Sys_User.Email like '%{0}%' ", email);
            if (!string.IsNullOrWhiteSpace(jobNum))
                where += string.Format(" and Sys_User.JobNum like '%{0}%' ", jobNum);
            if (!string.IsNullOrWhiteSpace(realName))
                where += string.Format(" and Sys_User.Realname like '%{0}%' ", realName);
            if (sex != 99)
                where += " and Sys_User.Sex = " + sex;
            if (status != 99)
            {
                if (status == 1)
                    where += string.Format(" and ((Sys_User.status = 1 and Sys_User.FreezeTime is  null) or (Sys_User.status = 1 and Sys_User.FreezeTime is not null and Sys_User.FreezeTime > '{0}')) ", DateTime.Now);
                else if (status == 0)
                    where += string.Format(" and ((Sys_User.Status = 0) or (Sys_User.Status = 1 and Sys_User.FreezeTime is not null and Sys_User.FreezeTime < '{0}')) ", DateTime.Now);
            }
            if (NotShowHasDept == 1)
                where += " and Sys_User.DeptId = -1";
            if (NotShowHasPost == 1)
                where += " and Sys_User.PostId = -1";
            if (iDisplayLength != default(int) && iDisplayStart != default(int))
                return _userDB.GetList(out totalCount, where, iDisplayStart, iDisplayLength);
            return _userDB.GetList(out totalCount, where);
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserState(int userId)
        {
            return _userDB.GetUserStatus(userId);
        }

        public List<string> GetUserType()
        {
            return _userDB.GetUserType();
        }

        #region 讲师的相关操作
        /// <summary>
        /// 获取讲师列表
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetAllTeacher(out int totalCount, string RealName = "", int isTeacher = 0, string PayGrade = "", int pageSize = int.MaxValue, int pageIndex = 1, string isDelete = "  Sys_User.IsDelete = 0 ")
        {
            string where = isDelete;
            if (!string.IsNullOrEmpty(RealName))
            {
                where += "  AND Realname LIKE '%" + RealName + "%'";
            }
            if (isTeacher != 0)
            {
                where += "  AND IsTeacher=" + isTeacher;
            }
            else
            {
                where += "  AND IsTeacher>0";
            }
            if (!string.IsNullOrEmpty(PayGrade))
            {
                where += "  AND PayGrade LIKE '%" + PayGrade + "%'";
            }

            return _userDB.GetList(out totalCount, where, pageIndex, pageSize);
        }

        /// <summary>
        /// 添加内部讲师
        /// </summary>
        /// <param name="userID"></param>
        public void InsertInnerTeacher(string userID)
        {
            _userDB.InsertInnerTeacher(userID);
        }

        /// <summary>
        /// 添加外聘讲师
        /// </summary>
        /// <param name="model"></param>
        public void InsertOuterTeacher(Sys_User model)
        {
            _userDB.InsertOuterTeacher(model);
        }

        /// <summary>
        /// 删除讲师
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="teacher"></param>
        public void DeleteTeacher(int userID, int teacher)
        {
            _userDB.DeleteTeacher(userID, teacher);
        }
        #endregion

        #region 指纹库同步时使用

        /// <summary>
        /// 根据HRID获取用户
        /// </summary>
        /// <param name="hrID"></param>
        /// <returns></returns>
        public Sys_User GetUserByHrID(string hrID)
        {
            return _userDB.GetUserByHrID(hrID);
        }

        /// <summary>
        /// 判断 用户信息 是否存在
        /// </summary>
        /// <param name="userId">用户名</param>
        /// <returns></returns>
        public bool ExistUserFinger(int userId)
        {
            return _userDB.ExistUserFinger(userId);
        }

        /// <summary>
        ///     增加一条数据
        /// </summary>
        /// <param name="model"></param>
        public void AddUserFinger(Sys_UserFinger model)
        {
            _userDB.AddUserFinger(model);
        }

        /// <summary>
        ///     更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <param name="flag">0:HR同步；1：原系统同步</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateUserFinger(Sys_UserFinger model, int flag)
        {
            return _userDB.UpdateUserFinger(model, flag);
        }
        #endregion

        #region 个人中心

        public static decimal TogetherSum = 0;
        public static decimal VideoSum =0;
        
        /// <summary>
        /// 个人算学时
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        public decimal GetUserScore(int uid, Sys_ParamConfig cpazq)
        {
            
            decimal sumT1 = 0;
            decimal sumT2 = 0;
            decimal sumT = 0;
            //集中
            List<Cl_CourseOrder> Tolist=_userDB.GetTogetherList(uid);
            List<Cl_CourseOrder> tempT1 = Tolist.Where(p => p.IsMust == 0).ToList();//必修
            sumT1 = Convert.ToDecimal(tempT1.Sum(p => p.tmpScore));
            List<Cl_CourseOrder> tempT2 = Tolist.Where(p => p.IsMust == 1).ToList();//选修
            sumT2 = Convert.ToDecimal(tempT2.Sum(p => p.tmpScore));
            

            decimal sumV1 = 0;
            decimal sumV2 = 0;
            decimal sumV = 0;
            //视频
            List<Cl_CourseOrder> Volist = _userDB.GetVideoList(uid);
            List<Cl_CourseOrder> tempV1 = Volist.Where(p => p.IsMust == 0).ToList();//必修
            sumV1=Convert.ToDecimal(tempV1.Sum(p => p.GetLength));
            List<Cl_CourseOrder> tempV2 = Volist.Where(p => p.IsMust == 1).ToList();//选修
            sumV2=Convert.ToDecimal(tempV2.Sum(p => p.GetLength));
            

            string mianstr = cpazq.ConfigValue;
            string[] CoType = Regex.Split(mianstr, ";", RegexOptions.IgnoreCase);
            if (!string.IsNullOrEmpty(CoType[0]))
            {
                string[] ToType = Regex.Split(CoType[0], ",", RegexOptions.IgnoreCase);
                if (sumT1 > Convert.ToDecimal(ToType[1]))
                {
                    sumT1 = Convert.ToDecimal(ToType[1]);
                }
                if (sumT2 > Convert.ToDecimal(ToType[2]))
                {
                    sumT2 = Convert.ToDecimal(ToType[2]);
                }
                sumT = sumT1 + sumT2;
                if (sumT > Convert.ToDecimal(ToType[0]))
                {
                    sumT = Convert.ToDecimal(ToType[0]);
                }
            }
            else
            {
                sumT = sumT1 + sumT2;
            }
            if (!string.IsNullOrEmpty(CoType[1]))
            {
                string[] VoType = Regex.Split(CoType[1], ",", RegexOptions.IgnoreCase);
                if (sumV1 > Convert.ToDecimal(VoType[1]))
                {
                    sumV1 = Convert.ToDecimal(VoType[1]);
                }
                if (sumV2 > Convert.ToDecimal(VoType[2]))
                {
                    sumV2 = Convert.ToDecimal(VoType[2]);
                }
                sumV = sumV1 + sumV2;
                if (sumV > Convert.ToDecimal(VoType[0]))
                {
                    sumV = Convert.ToDecimal(VoType[0]);
                }
            }
            else
            {
                sumV = sumV1 + sumV2;
            }
            TogetherSum = sumT;
            VideoSum = sumV;
            return sumV + sumT;
        }

        /// <summary>
        /// 获取个人信息和CPA学时
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Sys_User GetmodelCAP(int uid)
        {
            var usermodel=_userDB.GetmodelCAP(uid);
            usermodel.CoScore=TogetherSum;
            usermodel.voScore = VideoSum;
            return usermodel;
        }
        /// <summary>
        /// 学分前5名
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetTop5Credit()
        {
            return _userDB.GetTop5Credit();
        }
        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="photourl"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool UpdatePhoto(string photourl, int userid)
        {
            return _userDB.UpdatePhoto(photourl, userid);
        }

        /// <summary>
        /// 我的年度培训目标
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="bentime"></param>
        /// <param name="endtime"></param>
        /// <param name="totalcount"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="Order"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Co_CourseShow> GetMyCourse(int userid, int year, int way,out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            var list = new List<Co_CourseShow>();
            list = way == 1 ? _userDB.GetMyCourse(userid, year, startIndex, startLength, Order, where) : _userDB.GetMyVidioCourse(userid, year, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].CoPaperID > 0 && list[i].PaperId > 0)
                {
                    tbExamSendStudent exam = new tbExamSendStudent();
                    if (list[i].Way == 2)
                    {
                        exam = _ExamDB.GetExamSendStudentBySQL2008(list[i].Id, userid, list[i].CoPaperID, list[i].PaperId, 2);
                    }
                    else
                    {
                        exam = _ExamDB.GetExamSendStudentBySQL2008(list[i].Id, userid, list[i].CoPaperID, list[i].PaperId, 1);
                    }
                    list[i].GetexamScore = exam==null?0:exam.StudentAnswerList.Sum(pa => pa.GetScore);
                    tbExampaper paper = _paperDB.GetExampaper(list[i].PaperId);
                    list[i].ExamScore = paper.ExampaperScore;
                }
                else
                {
                    list[i].GetexamScore = 0;
                    list[i].ExamScore = 0;
                }
            }
            return list;
        }

        /// <summary>
        /// 我的年度培训目标(CPA)
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="year"></param>
        /// <param name="totalcount"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="Order"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Co_CourseShow> GetMyCPACoursebyWay(int userid, int year, out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            var list = _userDB.GetMyCPACoursebyway(userid, year, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// CPA年度课程
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="year"></param>
        /// <param name="totalcount"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="Order"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Co_CourseShow> GetMyCPACourse(int userid, int year, out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            var list = _userDB.GetMyCPACourse(userid,year, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// CPA周期课程
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="bentime"></param>
        /// <param name="endtime"></param>
        /// <param name="totalcount"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="Order"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Co_CourseShow> GetMyCPACourse(int userid, string bentime, string endtime, out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            var list = _userDB.GetMyCPACourse(userid, bentime,endtime, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }
        #endregion
    }
}