using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;
using LiXinDataAccess.NewClassManage;
using LiXinDataAccess.User;
using LiXinInterface.NewClassManage;
using LiXinModels;
using LiXinModels.NewClassManage;
using LiXinModels.User;
using LixinModels.NewClassManage;

namespace LiXinBLL.NewClassManage
{
    public class NewGroupUserBL : INewGroupUser
    {
        private readonly static NewGroupUserDB newGroupUserDB = new NewGroupUserDB();
        private readonly static NewClassDB newClassDB = new NewClassDB();
        private readonly static NewGroupDB newGroupDB = new NewGroupDB();
        private readonly UserDB userDB = new UserDB();


        /// <summary>
        /// 获得当前未分班人员列表
        /// <param name="where">条件语句格式" and ..."</param>
        /// </summary>
        public List<ClassUser> GetClassUserList(string where = "", string ordersql = " order by u.UserId")
        {
            return newGroupUserDB.GetClassUserList(where);
        }

        /// <summary>
        /// 查询指定班下的相关人员
        /// <param name="where">条件语句格式" and ..."</param>
        /// </summary>
        public List<New_GroupUser> GetList(string where = "")
        {
            return newGroupUserDB.GetList(where);
        }
        /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public New_GroupUser GetModel(int id)
        {
            return newGroupUserDB.GetModel(id);
        }

        /// <summary>
        /// 根据ClassID和UserId获取model(主要用于获取人员所在组ID)
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <param name="userId">人员ID</param>
        /// <returns></returns>
        public New_GroupUser GetModelByClassAndUser(int classId, int userId)
        {
            return newGroupUserDB.GetModelByClassAndUser(classId, userId);
        }

        /// <summary>
        /// 根据UserId获取model(主要用于获取人员所在班ID)
        /// </summary>
        /// <param name="userId">人员ID</param>
        /// <returns></returns>
        public New_GroupUser GetModelByUserID(int userId)
        {
            return newGroupUserDB.GetModelByUserID(userId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">要添加的实体对象</param>
        public int AddModel(New_GroupUser model)
        {
            newGroupUserDB.AddModel(model);
            return model.Id;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateModel(New_GroupUser model)
        {
            return newGroupUserDB.UpdateModel(model);
        }


        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="ids">ID集合格式为: 1,2,3,4</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteBatchModel(string ids)
        {
            return newGroupUserDB.DeleteBatchModel(ids);
        }
        /// <summary>
        /// 根据班级Id和人员Id集合批量删除数据
        /// </summary>
        /// <param name="userIds">学员ID集合格式为: 1,2,3,4</param>
        /// <param name="classId">班级ID</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteBatchByUserAndClass(string userIds, int classId)
        {
            return newGroupUserDB.DeleteBatchByUserAndClass(userIds, classId);
        }

        /// <summary>
        /// 根据班级Id和人员Id集合批量调整人员班组
        /// </summary>
        /// <param name="userIds">学员ID集合格式为: 1,2,3,4</param>
        /// <param name="classId">老班级ID</param>
        /// <param name="newClassId">新班级ID</param>
        /// <param name="newGroupId">新组ID</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool BatchChangeClassGroup(string userIds, int classId, int newClassId, int newGroupId)
        {
            return newGroupUserDB.BatchChangeClassGroup(userIds, classId, newClassId, newGroupId);
        }


        /// <summary>
        /// 根据班级Id和人员Id集合批量调整人员班组，并且修改学号
        /// </summary>
        /// <param name="userIds">学员ID集合格式为: 1,2,3,4</param>
        /// <param name="classId">老班级ID</param>
        /// <param name="newClassId">新班级ID</param>
        /// <param name="newGroupId">新组ID</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool BatchChangeClassGroup(string userIds, int classId, int newClassId, int newGroupId, string NumberID)
        {
            return newGroupUserDB.BatchChangeClassGroup(userIds, classId, newClassId, newGroupId, NumberID);
        }
        /// <summary>
        /// 获得当前未分班人员列表(分班管理页面使用)
        /// <param name="totalcount">记录总数</param>
        /// <param name="realName">姓名</param>
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// </summary>
        public List<Sys_User> GetList(out int totalcount, string realName, int year = -1, int startIndex = 1, int startLength = int.MaxValue)
        {
            year = year == -1 ? DateTime.Now.Year : year;
            string where = "";
            if (!string.IsNullOrWhiteSpace(realName))
            {
                where += string.Format(" and u.Realname like '%{0}%' ", realName.Trim().ReplaceSql());
            }
            if (year == DateTime.Now.Year)
            {
                where += " and u.IsNew=1";
            }
            else
            {
                where += " and  u.IsHistoryNew=1  AND u.NewYear=" + year;
            }
            var list = newGroupUserDB.GetList(where, startIndex, startLength);
            totalcount = list.Count > 0 ? list[0].totalCount : 0;
            return list;
        }

        /// <summary>
        /// 根据条件获取指定班组学员学号信息集合
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public List<string> GetNumberList(string where = "")
        {
            return newGroupUserDB.GetNumberList(where);
        }
        /// <summary>
        /// 批量添加班组人员
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <param name="groupId">组ID</param>
        /// <param name="userIds">人员ID集合：1,2,3</param>
        public void BatchAddNewGroupUser(int classId, int groupId, string userIds)
        {
            newGroupUserDB.BatchAddNewGroupUser(classId, groupId, userIds);
        }

        /// <summary>
        /// 批量添加班组人员
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <param name="groupId">组ID</param>
        /// <param name="userIds">人员ID集合：1,2,3</param>
        public void BatchAddNewGroupUser(int classId, int groupId, int userId, string NumberID)
        {
            newGroupUserDB.BatchAddNewGroupUser(classId, groupId, userId, NumberID);
        }

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
        public void DoAutoSplitClass(bool splitSex, bool splitSchool, bool splitMajor, bool splitFirm
                                  , int perClassPersonCount, int perGroupPersonCount
                                  , int startIndex = 1, int startLength = int.MaxValue, string orderBy = "")
        {
            if (splitSex)
            {
                orderBy += "u.Sex,";
            }
            if (splitSchool)
            {
                orderBy += "u.GraduateSchool,";
            }
            if (splitMajor)
            {
                orderBy += "u.Major,";
            }
            if (splitFirm)
            {
                orderBy += "u.IsInternExp";
            }
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = " order by " + orderBy.Trim(',');
            }
            var list = newGroupUserDB.GetList("", startIndex, startLength, orderBy).Select(p => p.UserId).ToList();
            int totalcount = list.Count; //未分班总人数
            int classCount = 0; //班数
            int groupCount = 0; //每班组数
            int lastClassPersonCount = 0;//最后一班人数（当人数小于设置的每班人数时使用）
            int lastClassGroupCount = 0;//最后一班组数

            int classYushu = totalcount % perClassPersonCount;
            classCount = (classYushu == 0)
                             ? (totalcount / perClassPersonCount)
                             : ((totalcount - classYushu) / perClassPersonCount + 1);

            int groupYushu = perClassPersonCount % perGroupPersonCount;
            groupCount = (groupYushu == 0)
                             ? (perClassPersonCount / perGroupPersonCount)
                             : ((perClassPersonCount - groupYushu) / perGroupPersonCount + 1);

            if (classYushu != 0)//最后一班人数小于设置的每班人数时使用）
            {
                lastClassPersonCount = classYushu;
                int lastClassGroupYushu = lastClassPersonCount % perGroupPersonCount;
                lastClassGroupCount = (lastClassGroupYushu == 0)
                                 ? (lastClassPersonCount / perGroupPersonCount)
                                 : ((lastClassPersonCount - lastClassGroupYushu) / perGroupPersonCount + 1);
            }

            var personArrayList = new List<List<List<int>>>(); //班组人员列表
            var classPersonList = new List<List<int>>(); //班级人员列表
            //初始化班级人员列表和班组人员列表
            for (int i = 0; i < classCount; i++)
            {
                classPersonList.Add(new List<int>());
                personArrayList.Add(new List<List<int>>());
                for (int j = 0; j < groupCount; j++)
                {
                    if (classYushu != 0 && (i == classCount - 1) && j >= lastClassGroupCount)//最后一班人数小于设置的每班人数时使用）
                    {
                        break;//当最后一班组数达到时，退出当前循环
                    }
                    personArrayList[i].Add(new List<int>());
                }
            }
            for (int i = 0; i < totalcount; i++) //1.遍历所有未分班人员将人员填入班级
            {
                if (classYushu != 0)//最后一班人数小于设置的每班人数时使用
                {
                    if (classPersonList[classCount - 1].Count == lastClassPersonCount)//当最后一班人数满时
                    {
                        classPersonList[i % (classCount - 1)].Add(list[i]);
                        continue;
                    }
                }
                classPersonList[i % classCount].Add(list[i]);
            }

            for (int i = 0; i < classCount; i++) //2.遍历所有班级
            {
                if (classYushu != 0 && (i == classCount - 1))//最后一班
                {
                    for (int j = 0; j < classPersonList[i].Count; j++) //3.遍历班级人员将人员填入组
                    {
                        personArrayList[i][j % lastClassGroupCount].Add(classPersonList[i][j]);
                    }
                    break;
                }
                for (int j = 0; j < classPersonList[i].Count; j++) //3.遍历班级人员将人员填入组
                {
                    personArrayList[i][j % groupCount].Add(classPersonList[i][j]);
                }
            }

            //获得班级最大索引+1
            int classIndex = newClassDB.GetCurrentIndex(System.DateTime.Now.Year);

            //将人员插入数据库
            for (int i = 0; i < classCount; i++) //1.遍历所有班级
            {
                string classno = DtoX(classIndex + i);
                New_Class classModel = new New_Class();
                classModel.ClassName = classno + "班";
                classModel.PersonCount = perClassPersonCount;
                classModel.Year = System.DateTime.Now.Year;
                classModel.ClassNo = classno;
                classModel.ClassIndex = classIndex + i;
                classModel.IsDoDelete = 1;
                newClassDB.AddModel(classModel);
                if (classModel.Id > 0)
                {
                    for (int j = 0; j < personArrayList[i].Count; j++) //2.遍历班中组
                    {
                        string groupno = DtoX(j, 1);
                        New_Group groupModel = new New_Group();
                        groupModel.ClassId = classModel.Id;
                        groupModel.GroupName = groupno + "组";
                        groupModel.PersonCount = perGroupPersonCount;
                        groupModel.GroupNo = groupno;
                        groupModel.GroupIndex = j;
                        newGroupDB.AddModel(groupModel);
                        if (groupModel.Id > 0)
                        {
                            int currentNo = 0;
                            string yearClassGroup = System.DateTime.Now.Year + classno + groupno;//要生成的学号前缀
                            var numberList =
                                newGroupUserDB.GetNumberList(string.Format(" and u.NumberID like '%{0}%' ", yearClassGroup));
                            foreach (var v in numberList)
                            {
                                if (v.Replace(yearClassGroup, "").Length == 4)
                                {
                                    currentNo = int.Parse(v.Replace(yearClassGroup, "")) + 1;
                                    break;
                                }
                            }
                            //if (numberList.Count>0 && numberList[0].Length > 4)
                            //{
                            //    currentNo = int.Parse(numberList[0].Substring(numberList[0].Length - 4)) + 1;
                            //}
                            for (int k = 0; k < personArrayList[i][j].Count; k++) //3.遍历组中人
                            {
                                int no = currentNo + k;
                                New_GroupUser groupUserModel = new New_GroupUser();
                                groupUserModel.ClassId = classModel.Id;
                                groupUserModel.GroupId = groupModel.Id;
                                groupUserModel.UserId = personArrayList[i][j][k];
                                groupUserModel.NumberID = classModel.Year + classModel.ClassNo + groupModel.GroupNo +
                                                          no.ToString("0000");
                                newGroupUserDB.AddModel(groupUserModel);
                                if (groupUserModel.Id > 0)
                                {
                                    Sys_User userModel = userDB.Get(groupUserModel.UserId);
                                    if (userModel.IsChangeNumberId != 1)//当固定标识为可更改时
                                    {
                                        userModel.NumberID = groupUserModel.NumberID;
                                        userDB.Update(userModel);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获得当前未分班人员总数
        /// </summary>
        public int GetCountList()
        {
            return newGroupUserDB.GetCountList();
        }

        #region == 暂时不用的代码 ==
        ///// <summary>
        ///// 获得班级名称（大写字母表示）
        ///// </summary>
        ///// <param name="index">班级索引号 如0、1、2代表A班、B班、C班。。。</param>
        ///// <param name="uplowType">0：转成大写字母 1：转成小写字母</param>
        //public string GetUpperClassName(int index,int uplowType=0)
        //{
        //    index = index + 1;
        //    if (index <= 26)
        //    {
        //        return uplowType == 0
        //                   ? ((Enums.UppercaseLettersNumber) index).ToString()
        //                   : ((Enums.LowercaseLettersNumber) index).ToString();
        //    }

        //    //循环取模倒序替换成字母
        //    var list = new List<int>();
        //    while (index>0 && index/26 >= 0) //商不为0，则继续
        //    {
        //        list.Add(index%26);
        //        index = index/26;
        //    }

        //    string returnStr = "";
        //    for (int i = list.Count-1; i >=0; i--)
        //    {
        //        returnStr += (uplowType == 0
        //                         ? ((Enums.UppercaseLettersNumber) list[i]).ToString()
        //                         : ((Enums.LowercaseLettersNumber) list[i]).ToString());
        //    }
        //    return returnStr;

        //}
        #endregion


        /// <summary>
        /// 获得班级或组名称（字母表示）  十进制转二十六进制
        /// </summary>
        /// <param name="d">班级/组索引号 如0、1、2代表A班/a组、B班/b组、C班/c组。。。</param>
        /// <param name="uplowType">0：转成大写字母 1：转成小写字母</param>
        public string DtoX(int d, int uplowType = 0)
        {
            string x = "";
            if (d < 26)
            {
                x = (uplowType == 0
                                 ? ((Enums.UppercaseLettersNumber)d).ToString()
                                 : ((Enums.LowercaseLettersNumber)d).ToString());
            }
            else
            {
                int c;
                int s = 0;
                int n = d;
                //int temp = d;
                while (n >= 26)
                {
                    s++;
                    n = n / 16;
                }
                string[] m = new string[s];
                int i = 0;
                do
                {
                    c = d / 26;
                    m[i++] = (uplowType == 0
                                 ? ((Enums.UppercaseLettersNumber)(d % 26)).ToString()
                                 : ((Enums.LowercaseLettersNumber)(d % 26)).ToString());
                    d = c;
                } while (c >= 26);
                d = d - 1 >= 0 ? d - 1 : d;//为适应实际班级组的规则而添加，实际算法不存在此行代码
                x = (uplowType == 0
                                 ? ((Enums.UppercaseLettersNumber)d).ToString()
                                 : ((Enums.LowercaseLettersNumber)d).ToString());
                for (int j = m.Length - 1; j >= 0; j--)
                {
                    x += m[j];
                }
            }
            return x;
        }

        /// <summary>
        /// 查询人所在的组和班级
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public New_GroupUser GetClassInfoByUserID(int userID)
        {
            return newGroupUserDB.GetClassInfoByUserID(userID);
        }

        /// <summary>
        /// 根据分班的数量，生成班级的名字
        /// </summary>
        /// <param name="SumCount"></param>
        /// <returns></returns>
        public List<string> GetClassnoList(int SumCount)
        {

            var liststr = new List<string>();
            //获得班级最大索引+1
            int classIndex = newClassDB.GetCurrentIndex(System.DateTime.Now.Year);

            //将人员插入数据库
            for (int i = 0; i < SumCount; i++)
            {
                string classno = DtoX(classIndex + i);
                liststr.Add(classno);
            }
            return liststr;
        }


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
        public void DoAutoSingleSplitClass(bool splitSex, bool splitSchool, bool splitMajor, bool splitFirm
                                  , int perClassPersonCount, int perGroupPersonCount, string orderBy = "")
        {
            if (splitSex)
            {
                orderBy += "u.Sex,";
            }
            if (splitSchool)
            {
                orderBy += "u.GraduateSchool,";
            }
            if (splitMajor)
            {
                orderBy += "u.Major,";
            }
            if (splitFirm)
            {
                orderBy += "u.IsInternExp";
            }
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = " order by " + orderBy.Trim(',');
            }

            var list = newGroupUserDB.GetList(where: " and IsNew=1", orderBy: orderBy).Select(p => p.UserId).ToList();

            int totalcount = list.Count; //未分班总人数
            int groupCount = 0; //每班组数

            int groupYushu = perClassPersonCount % perGroupPersonCount;
            groupCount = (groupYushu == 0)
                             ? (perClassPersonCount / perGroupPersonCount)
                             : ((perClassPersonCount - groupYushu) / perGroupPersonCount + 1);

            var personArrayList = new List<List<int>>(); //班组人员列表
            var classPersonList = new List<int>(); //班级人员列表
            for (int j = 0; j < groupCount; j++)
            {
                personArrayList.Add(new List<int>());
            }

            //初始化班级人员列表和班组人员列表
            for (int i = 0; i < totalcount; i++) //1.遍历所有未分班人员选择一定数量填入班级
            {
                if (classPersonList.Count != perClassPersonCount)
                {
                    classPersonList.Add(list[i]);
                }
            }

            for (int i = 0; i < classPersonList.Count; i++) //3.遍历班级人员将人员填入组
            {
                personArrayList[i % groupCount].Add(classPersonList[i]);
            }


            //获得班级最大索引+1
            int classIndex = newClassDB.GetCurrentIndex(System.DateTime.Now.Year);

            //将人员插入数据库
            //for (int i = 0; i < classCount; i++) //1.遍历所有班级
            //{
            string classno = DtoX(classIndex);
            New_Class classModel = new New_Class();
            classModel.ClassName = classno + "班";
            classModel.PersonCount = perClassPersonCount;
            classModel.Year = System.DateTime.Now.Year;
            classModel.ClassNo = classno;
            classModel.ClassIndex = classIndex;
            classModel.IsDoDelete = 1;
            newClassDB.AddModel(classModel);
            if (classModel.Id > 0)
            {
                for (int j = 0; j < personArrayList.Count; j++) //2.遍历班中组
                {
                    string groupno = DtoX(j, 1);
                    New_Group groupModel = new New_Group();
                    groupModel.ClassId = classModel.Id;
                    groupModel.GroupName = groupno + "组";
                    groupModel.PersonCount = perGroupPersonCount;
                    groupModel.GroupNo = groupno;
                    groupModel.GroupIndex = j;
                    newGroupDB.AddModel(groupModel);
                    if (groupModel.Id > 0)
                    {
                        int currentNo = 0;
                        string yearClassGroup = System.DateTime.Now.Year + classno + groupno;//要生成的学号前缀
                        var numberList =
                            newGroupUserDB.GetNumberList(string.Format(" and u.NumberID like '%{0}%' ", yearClassGroup));
                        foreach (var v in numberList)
                        {
                            if (v.Replace(yearClassGroup, "").Length == 4)
                            {
                                currentNo = int.Parse(v.Replace(yearClassGroup, "")) + 1;
                                break;
                            }
                        }
                        //if (numberList.Count>0 && numberList[0].Length > 4)
                        //{
                        //    currentNo = int.Parse(numberList[0].Substring(numberList[0].Length - 4)) + 1;
                        //}
                        for (int k = 0; k < personArrayList[j].Count; k++) //3.遍历组中人
                        {
                            int no = currentNo + k;
                            New_GroupUser groupUserModel = new New_GroupUser();
                            groupUserModel.ClassId = classModel.Id;
                            groupUserModel.GroupId = groupModel.Id;
                            groupUserModel.UserId = personArrayList[j][k];
                            groupUserModel.NumberID = classModel.Year + classModel.ClassNo + groupModel.GroupNo +
                                                      no.ToString("0000");
                            newGroupUserDB.AddModel(groupUserModel);
                            if (groupUserModel.Id > 0)
                            {
                                Sys_User userModel = userDB.Get(groupUserModel.UserId);
                                //if (userModel.IsChangeNumberId != 1)//当固定标识为可更改时
                                //{
                                userModel.NumberID = groupUserModel.NumberID;
                                userDB.Update(userModel);
                                // }
                            }
                        }
                    }
                }
            }
            // }
        }

    }
}
