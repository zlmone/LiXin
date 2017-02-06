/* ==============================================================================
 * 类名称：SeatPlan
 * 类描述：座位安排
 * 创建人：Darex
 * 创建时间：2013/7/2 14:03:12
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
* ==============================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels;
using LiXinModels.User;

namespace LiXinBLL
{
    public class SeatPlan
    {
        private static readonly Random R = new Random();

        /// <summary>
        /// 座位安排
        /// </summary>
        /// <param name="model">参数</param>
        public static List<Sys_User> TempArrangeSeat(SeatParaModel model)
        {
            var userlist = new List<Sys_User>();
            //座位
            var seat = new Sys_User[model.Row][];
            //座位数与人数之差，是否会有空位存在
            var difference = model.Row * model.Column - model.DisableSeats.Count - model.Users.Count;

            //填充座位
            for (var i = 0; i < model.Row; i++)
            {
                seat[i] = new Sys_User[model.Column];
                for (var j = 0; j < model.Column; j++)
                {
                    //已经没有人员需要安排座位
                    if (model.Users.Count == 0)
                    {
                        seat[i][j] = null;
                        continue;
                    }
                    //禁用座位
                    if (model.DisableSeats.Exists(p => p.Row == i && p.Column == j))
                    {
                        seat[i][j] = null;
                        continue;
                    }

                    //选定的人员
                    Sys_User user = null;
                    //左邻
                    Sys_User leftUser = null;
                    //上邻
                    Sys_User onUser = null;

                    if (j > 0)
                    {
                        leftUser = seat[i][j - 1];
                    }
                    if (i > 0)
                    {
                        onUser = seat[i - 1][j];
                    }

                    List<Sys_User> siftUsers;
                    //上和左没人时，抽取出现概率最高的人员
                    if (leftUser == null && onUser == null)
                    {
                        siftUsers = SiftHighProbability(model);
                        user = siftUsers[R.Next(siftUsers.Count)];
                        seat[i][j] = user;
                        userlist.Add(user);
                        model.Users.Remove(user);
                        continue;
                    }

                    //安排座位
                    siftUsers = Sift(model, leftUser, onUser, ref difference);
                    if (siftUsers == null)
                    {
                        seat[i][j] = null;
                    }
                    else
                    {
                        user = siftUsers[R.Next(siftUsers.Count)];
                        seat[i][j] = user;
                        userlist.Add(user);
                        model.Users.Remove(user);
                    }
                }
            }

            if (model.IsGroupLink)
            {
                userlist = userlist.OrderBy(p => p.GroupId).ToList();
            }

            return userlist;
        }

        /// <summary>
        /// 座位安排
        /// </summary>
        /// <param name="model">参数</param>
        public static Sys_User[][] ArrangeSeat(SeatParaModel model)
        {
            //座位
            var seat = new Sys_User[model.Row][];
            //座位数与人数之差，是否会有空位存在
            var difference = model.Row * model.Column - model.DisableSeats.Count - model.Users.Count;

            //填充座位
            for (var i = 0; i < model.Row; i++)
            {
                seat[i] = new Sys_User[model.Column];
                for (var j = 0; j < model.Column; j++)
                {
                    //已经没有人员需要安排座位
                    if (model.Users.Count == 0)
                    {
                        seat[i][j] = null;
                        continue;
                    }
                    //禁用座位
                    if (model.DisableSeats.Exists(p => p.Row == i && p.Column == j))
                    {
                        seat[i][j] = null;
                        continue;
                    }

                    //选定的人员
                    Sys_User user = null;
                    //左邻
                    Sys_User leftUser = null;
                    //上邻
                    Sys_User onUser = null;

                    if (j > 0)
                    {
                        leftUser = seat[i][j - 1];
                    }
                    if (i > 0)
                    {
                        onUser = seat[i - 1][j];
                    }

                    List<Sys_User> siftUsers;
                    //上和左没人时，抽取出现概率最高的人员
                    if (leftUser == null && onUser == null)
                    {
                        siftUsers = SiftHighProbability(model);
                        user = siftUsers[R.Next(siftUsers.Count)];
                        seat[i][j] = user;
                        model.Users.Remove(user);
                        continue;
                    }

                    //安排座位
                    siftUsers = Sift(model, leftUser, onUser, ref difference);
                    if (siftUsers == null)
                    {
                        seat[i][j] = null;
                    }
                    else
                    {
                        user = siftUsers[R.Next(siftUsers.Count)];
                        seat[i][j] = user;
                        model.Users.Remove(user);
                    }
                }
            }

            return seat;
        }

        /// <summary>
        /// 筛选人员安排座位
        /// </summary>
        /// <param name="model"></param>
        /// <param name="leftUser"></param>
        /// <param name="onUser"></param>
        /// <param name="difference">座位数与人数之差，是否会有空位存在</param>
        /// <returns></returns>
        private static List<Sys_User> Sift(SeatParaModel model, Sys_User leftUser, Sys_User onUser, ref int difference)
        {
            //筛选前人员
            List<Sys_User> befsiftUsers = model.Users;
            //筛选后的人员  按性别筛选
            List<Sys_User> afsiftUsers = SiftSex(model.IsSexDifferent, leftUser, onUser, befsiftUsers, difference);
            if (afsiftUsers.Count != 0)
            {
                befsiftUsers = afsiftUsers.ToList();
            }
            //未筛选到需要的人员，且有空位存在，返回空位
            else
            {
                if (difference > 0 && model.IsSexDifferent)
                {
                    difference--;
                    return null;
                }
            }


            //是否有实习经验
            afsiftUsers = SiftExp(model.IsHaveExp, leftUser, onUser, befsiftUsers, difference);
            if (afsiftUsers.Count != 0)
            {
                befsiftUsers = afsiftUsers.ToList();
            }
            //未筛选到需要的人员，且有空位存在，返回空位
            else
            {
                if (difference > 0 && model.IsHaveExp)
                {
                    difference--;
                    return null;
                }
            }


            //专业不同
            afsiftUsers = SiftProfessional(model.IsProfessionalDifferent, leftUser, onUser, befsiftUsers, difference);
            if (afsiftUsers.Count != 0)
            {
                befsiftUsers = afsiftUsers.ToList();
            }
            //未筛选到需要的人员，且有空位存在，返回空位
            else
            {
                if (difference > 0 && model.IsProfessionalDifferent)
                {
                    difference--;
                    return null;
                }
            }


            //毕业院校不同
            afsiftUsers = SiftUniversities(model.IsUniversitiesDifferent, leftUser, onUser, befsiftUsers, difference);
            if (afsiftUsers.Count != 0)
            {
                befsiftUsers = afsiftUsers.ToList();
            }
            //未筛选到需要的人员，且有空位存在，返回空位
            else
            {
                if (difference > 0 && model.IsUniversitiesDifferent)
                {
                    difference--;
                    return null;
                }
            }

            //未筛选到需要的人员，且有空位存在，返回空位
            if (difference > 0 && afsiftUsers.Count == 0)
            {
                difference--;
                return null;
            }

            return afsiftUsers.Count > 0 ? afsiftUsers : befsiftUsers;
        }

        #region 筛选条件

        /// <summary>
        /// 出现频率高的人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static List<Sys_User> SiftHighProbability(SeatParaModel model)
        {
            var siftUsers = model.Users;
            //临时存放
            List<Sys_User> maxUsers;
            //性别不同
            if (model.IsSexDifferent)
            {
                maxUsers = new List<Sys_User>();
                var sexs = siftUsers.Select(p => p.Sex).Distinct();
                foreach (var v in sexs)
                {
                    if (siftUsers.Count(p => p.Sex == v) > maxUsers.Count)
                    {
                        maxUsers = siftUsers.FindAll(p => p.Sex == v);
                    }
                }

                siftUsers = maxUsers;
            }
            //事务所经验
            if (model.IsHaveExp)
            {
                maxUsers = new List<Sys_User>();
                var exps = siftUsers.Select(p => p.IsInternExp).Distinct();
                foreach (var v in exps)
                {
                    if (siftUsers.Count(p => p.IsInternExp == v) > maxUsers.Count)
                    {
                        maxUsers = siftUsers.FindAll(p => p.IsInternExp == v);
                    }
                }

                siftUsers = maxUsers;
            }
            //专业不同
            if (model.IsProfessionalDifferent)
            {
                maxUsers = new List<Sys_User>();
                var majors = siftUsers.Select(p => p.Major).Distinct();
                foreach (var v in majors)
                {
                    if (siftUsers.Count(p => p.Major == v) > maxUsers.Count)
                    {
                        maxUsers = siftUsers.FindAll(p => p.Major == v);
                    }
                }

                siftUsers = maxUsers;
            }
            //院校不同
            if (model.IsUniversitiesDifferent)
            {
                maxUsers = new List<Sys_User>();
                var schools = siftUsers.Select(p => p.GraduateSchool).Distinct();
                foreach (var v in schools)
                {
                    if (siftUsers.Count(p => p.GraduateSchool == v) > maxUsers.Count)
                    {
                        maxUsers = siftUsers.FindAll(p => p.GraduateSchool == v);
                    }
                }

                siftUsers = maxUsers;
            }

            return siftUsers;
        }

        /// <summary>
        /// 性别不同安排座位
        /// </summary>
        /// <param name="isSexDifferent"></param>
        /// <param name="leftUser"></param>
        /// <param name="onUser"></param>
        /// <param name="allUser"></param>
        /// <param name="difference"></param>
        /// <returns></returns>
        private static List<Sys_User> SiftSex(bool isSexDifferent, Sys_User leftUser, Sys_User onUser, List<Sys_User> allUser, int difference)
        {
            //最终筛选的结果
            var siftUsers = new List<Sys_User>();
            //性别不同
            if (isSexDifferent)
            {
                var sexs = (from p in allUser
                            where
                                (leftUser == null || p.Sex != leftUser.Sex) &&
                                (onUser == null || p.Sex != onUser.Sex)
                            select p.Sex).Distinct().ToList();

                //不存在不同的类别
                if (sexs.Count == 0)
                {
                    if (leftUser != null && onUser != null)
                    {
                        var firstSchool = leftUser.GraduateSchool;
                        var secondSchool = onUser.GraduateSchool;

                        //两边类型相同
                        if (firstSchool != secondSchool && difference == 0)
                        {
                            var siftFirst = allUser.FindAll(p => p.GraduateSchool == firstSchool);
                            var siftSecond = allUser.FindAll(p => p.GraduateSchool == secondSchool);

                            //那个类型的人多取哪个类型
                            return siftFirst.Count >= siftSecond.Count ? siftFirst : siftSecond;
                        }
                    }

                    return siftUsers;
                }
                //只剩一个不相同的类别
                if (sexs.Count == 1)
                {
                    //上邻和左邻都存在
                    if (leftUser != null && onUser != null)
                    {
                        siftUsers = allUser.FindAll(p => p.Sex == sexs[0]);
                        return siftUsers;
                    }
                    else
                    {
                        //补充类别，补足两项供选择
                        var existSex = -1;
                        if (leftUser != null)
                        {
                            existSex = leftUser.Sex;
                        }
                        if (onUser != null)
                        {
                            existSex = onUser.Sex;
                        }

                        //剩余类别的人员
                        var siftFirst = allUser.FindAll(p => p.Sex == sexs[0]);
                        //补充类别的人员
                        var siftSecond = allUser.FindAll(p => p.Sex == existSex);

                        //优先类型的人多取优先类型，否则按照比例选取
                        return siftFirst.Count >= siftSecond.Count ? siftFirst : SiftGYS(siftFirst, siftSecond);
                    }
                }

                return allUser.FindAll(p => p.Sex == sexs[R.Next(sexs.Count)]);
            }

            return allUser;
        }

        /// <summary>
        /// 事务所实习经验
        /// </summary>
        /// <param name="isHaveExp"></param>
        /// <param name="leftUser"></param>
        /// <param name="onUser"></param>
        /// <param name="allUser"></param>
        /// <param name="difference"></param>
        /// <returns></returns>
        private static List<Sys_User> SiftExp(bool isHaveExp, Sys_User leftUser, Sys_User onUser, List<Sys_User> allUser, int difference)
        {
            //最终筛选的结果
            var siftUsers = new List<Sys_User>();
            //性别不同
            if (isHaveExp)
            {
                var exps = (from p in allUser
                            where
                                (leftUser == null || p.IsInternExp != leftUser.IsInternExp) &&
                                (onUser == null || p.IsInternExp != onUser.IsInternExp)
                            select p.IsInternExp).Distinct().ToList();

                //不存在不同的类别
                if (exps.Count == 0)
                {
                    if (leftUser != null && onUser != null)
                    {
                        var firstSchool = leftUser.GraduateSchool;
                        var secondSchool = onUser.GraduateSchool;

                        //两边类型相同
                        if (firstSchool != secondSchool && difference == 0)
                        {
                            var siftFirst = allUser.FindAll(p => p.GraduateSchool == firstSchool);
                            var siftSecond = allUser.FindAll(p => p.GraduateSchool == secondSchool);

                            //那个类型的人多取哪个类型
                            return siftFirst.Count >= siftSecond.Count ? siftFirst : siftSecond;
                        }
                    }

                    return siftUsers;
                }
                //只剩一个不相同的类别
                if (exps.Count == 1)
                {
                    //上邻和左邻都存在
                    if (leftUser != null && onUser != null)
                    {
                        siftUsers = allUser.FindAll(p => p.IsInternExp == exps[0]);
                        return siftUsers;
                    }
                    else
                    {
                        //补充类别，补足两项供选择
                        var existExp = -1;
                        if (leftUser != null)
                        {
                            existExp = leftUser.IsInternExp;
                        }
                        if (onUser != null)
                        {
                            existExp = onUser.IsInternExp;
                        }

                        //剩余类别的人员
                        var siftFirst = allUser.FindAll(p => p.IsInternExp == exps[0]);
                        //补充类别的人员
                        var siftSecond = allUser.FindAll(p => p.IsInternExp == existExp);

                        //优先类型的人多取优先类型，否则按照比例选取
                        return siftFirst.Count >= siftSecond.Count ? siftFirst : SiftGYS(siftFirst, siftSecond);
                    }
                }

                return allUser.FindAll(p => p.IsInternExp == exps[R.Next(exps.Count)]);
            }

            return allUser;
        }

        /// <summary>
        /// 专业不同
        /// </summary>
        /// <param name="isProfessionalDifferent"></param>
        /// <param name="leftUser"></param>
        /// <param name="onUser"></param>
        /// <param name="allUser"></param>
        /// <param name="difference"></param>
        /// <returns></returns>
        private static List<Sys_User> SiftProfessional(bool isProfessionalDifferent, Sys_User leftUser, Sys_User onUser, List<Sys_User> allUser, int difference)
        {
            //最终筛选的结果
            var siftUsers = new List<Sys_User>();
            //性别不同
            if (isProfessionalDifferent)
            {
                var majors = (from p in allUser
                              where
                                  (leftUser == null || p.Major != leftUser.Major) &&
                                  (onUser == null || p.Major != onUser.Major)
                              select p.Major).Distinct().ToList();

                //不存在不同的类别
                if (majors.Count == 0)
                {
                    if (leftUser != null && onUser != null)
                    {
                        var firstSchool = leftUser.GraduateSchool;
                        var secondSchool = onUser.GraduateSchool;

                        //两边类型相同
                        if (firstSchool != secondSchool && difference == 0)
                        {
                            var siftFirst = allUser.FindAll(p => p.GraduateSchool == firstSchool);
                            var siftSecond = allUser.FindAll(p => p.GraduateSchool == secondSchool);

                            //那个类型的人多取哪个类型
                            return siftFirst.Count >= siftSecond.Count ? siftFirst : siftSecond;
                        }
                    }

                    return siftUsers;
                }
                //只剩一个不相同的类别
                if (majors.Count == 1)
                {
                    //上邻和左邻都存在
                    if (leftUser != null && onUser != null)
                    {
                        siftUsers = allUser.FindAll(p => p.Major == majors[0]);
                        return siftUsers;
                    }
                    else
                    {
                        //补充类别，补足两项供选择
                        var existMajor = "";
                        if (leftUser != null)
                        {
                            existMajor = leftUser.Major;
                        }
                        if (onUser != null)
                        {
                            existMajor = onUser.Major;
                        }

                        //剩余类别的人员
                        var siftFirst = allUser.FindAll(p => p.Major == majors[0]);
                        //补充类别的人员
                        var siftSecond = allUser.FindAll(p => p.Major == existMajor);

                        //优先类型的人多取优先类型，否则按照比例选取
                        return siftFirst.Count >= siftSecond.Count ? siftFirst : SiftGYS(siftFirst, siftSecond);
                    }
                }

                return allUser.FindAll(p => p.Major == majors[R.Next(majors.Count)]);
            }

            return allUser;
        }

        /// <summary>
        /// 毕业院校不同
        /// </summary>
        /// <param name="isHaveExp"></param>
        /// <param name="leftUser"></param>
        /// <param name="onUser"></param>
        /// <param name="allUser"></param>
        /// <param name="difference"></param>
        /// <returns></returns>
        private static List<Sys_User> SiftUniversities(bool isHaveExp, Sys_User leftUser, Sys_User onUser, List<Sys_User> allUser, int difference)
        {
            //最终筛选的结果
            var siftUsers = new List<Sys_User>();
            //性别不同
            if (isHaveExp)
            {
                var schools = (from p in allUser
                               where
                                   (leftUser == null || p.IsInternExp != leftUser.IsInternExp) &&
                                   (onUser == null || p.IsInternExp != onUser.IsInternExp)
                               select p.GraduateSchool).Distinct().ToList();

                //不存在不同的类别
                if (schools.Count == 0)
                {
                    if (leftUser != null && onUser != null)
                    {
                        var firstSchool = leftUser.GraduateSchool;
                        var secondSchool = onUser.GraduateSchool;

                        //两边类型相同
                        if (firstSchool != secondSchool && difference == 0)
                        {
                            var siftFirst = allUser.FindAll(p => p.GraduateSchool == firstSchool);
                            var siftSecond = allUser.FindAll(p => p.GraduateSchool == secondSchool);

                            //那个类型的人多取哪个类型
                            return siftFirst.Count >= siftSecond.Count ? siftFirst : siftSecond;
                        }
                    }

                    return siftUsers;
                }
                //只剩一个不相同的类别
                if (schools.Count == 1)
                {
                    //上邻和左邻都存在
                    if (leftUser != null && onUser != null)
                    {
                        siftUsers = allUser.FindAll(p => p.GraduateSchool == schools[0]);
                        return siftUsers;
                    }
                    else
                    {
                        //补充类别，补足两项供选择
                        var existSchool = "";
                        if (leftUser != null)
                        {
                            existSchool = leftUser.GraduateSchool;
                        }
                        if (onUser != null)
                        {
                            existSchool = onUser.GraduateSchool;
                        }

                        //剩余类别的人员
                        var siftFirst = allUser.FindAll(p => p.GraduateSchool == schools[0]);
                        //补充类别的人员
                        var siftSecond = allUser.FindAll(p => p.GraduateSchool == existSchool);

                        //优先类型的人多取优先类型，否则按照比例选取
                        return siftFirst.Count >= siftSecond.Count ? siftFirst : SiftGYS(siftFirst, siftSecond);
                    }
                }

                return allUser.FindAll(p => p.GraduateSchool == schools[R.Next(schools.Count)]);
            }

            return allUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="first">优先筛选的列</param>
        /// <param name="second">次筛选的列</param>
        /// <returns></returns>
        private static List<Sys_User> SiftGYS(List<Sys_User> first, List<Sys_User> second)
        {
            //人员不存在
            if (first.Count == 0)
            {
                return second;
            }

            //人员不存在
            if (second.Count == 0)
            {
                return first;
            }

            var firstRandom = Math.Min(first.Count, second.Count) * 2;
            var secondRandom = Math.Abs(first.Count - second.Count);

            //选取剩余的类别
            //TODO firstRandom+1 为了凑概率，没有很科学的解释
            if (R.Next(firstRandom + secondRandom) <= (firstRandom + 1))
            {
                return first;
            }

            return second;
        }

        /// <summary>
        /// 最大公约数
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        private static int GYS(int m, int n)
        {
            var max = Math.Max(m, n);
            var min = Math.Min(m, n);
            int p, q;
            p = max;
            q = min;
            int gr;
            while (true)
            {
                if (p % q == 0)
                {
                    gr = q;
                    break;
                }
                else
                {
                    int r = p % q;
                    p = q;
                    q = r;
                }
            }
            return gr;
        }

        #endregion

        /// <summary>
        /// 排座位的参数
        /// </summary>
        [Serializable]
        public class SeatParaModel
        {
            /// <summary>
            /// 行
            /// </summary>
            public int Row { get; set; }
            /// <summary>
            /// 列
            /// </summary>
            public int Column { get; set; }
            /// <summary>
            /// 性别不同
            /// </summary>
            public bool IsSexDifferent { get; set; }
            /// <summary>
            /// 有无事务所实习经验
            /// </summary>
            public bool IsHaveExp { get; set; }
            /// <summary>
            /// 毕业院校不同
            /// </summary>
            public bool IsUniversitiesDifferent { get; set; }
            /// <summary>
            /// 所学专业不同
            /// </summary>
            public bool IsProfessionalDifferent { get; set; }
            /// <summary>
            /// 是否同小组相邻
            /// </summary>
            public bool IsGroupLink { get; set; }
            /// <summary>
            /// 需安排座位的人员
            /// </summary>
            public List<Sys_User> Users { get; set; }
            /// <summary>
            /// 禁用的座位
            /// </summary>
            public List<DisableSeatModel> DisableSeats { get; set; }
        }

        /// <summary>
        /// 禁用座位
        /// </summary>
        [Serializable]
        public class DisableSeatModel
        {
            /// <summary>
            /// 行
            /// </summary>
            public int Row { get; set; }
            /// <summary>
            /// 列
            /// </summary>
            public int Column { get; set; }
        }

        /// <summary>
        /// 用户显示model
        /// </summary>
        [Serializable]
        public class SeatUser
        {
            #region Model
            private int _userid;
            private string _username;
            private string _realname;
            private string _ename;
            private int _sex = 0;
            private string _major;
            private string _graduateschool;

            /// <summary>
            /// id
            /// </summary>
            public int UserId
            {
                set
                {
                    _userid = value;
                }
                get
                {
                    return _userid;
                }
            }
            /// <summary>
            /// 用户名
            /// </summary>
            public string Username
            {
                set
                {
                    _username = value;
                }
                get
                {
                    return _username;
                }
            }
            /// <summary>
            /// 姓名
            /// A0101
            /// </summary>
            public string Realname
            {
                set
                {
                    _realname = value;
                }
                get
                {
                    return _realname;
                }
            }
            /// <summary>
            /// 英文名
            /// </summary>
            public string Ename
            {
                set
                {
                    _ename = value;
                }
                get
                {
                    return _ename;
                }
            }
            /// <summary>
            /// 性别
            /// A0107
            /// </summary>
            public int Sex
            {
                set
                {
                    _sex = value;
                }
                get
                {
                    return _sex;
                }
            }
            /// <summary>
            /// 所学专业
            /// A0161
            /// </summary>
            public string Major
            {
                set
                {
                    _major = value;
                }
                get
                {
                    return _major;
                }
            }
            /// <summary>
            /// 毕业院校
            /// A0160
            /// </summary>
            public string GraduateSchool
            {
                set
                {
                    _graduateschool = value;
                }
                get
                {
                    return _graduateschool;
                }
            }
            /// <summary>
            /// 是否有实习经验 0有 1没有
            /// </summary>
            public int IsInternExp { get; set; }

            #endregion Model

            /// <summary>
            /// 性别
            /// </summary>
            public string SexStr
            {
                get
                {
                    if (Sex == 0)
                        return "男";
                    else if (Sex == 1)
                        return "女";
                    else
                        return "保密";
                }
            }

            /// <summary>
            /// 是否有实习经验
            /// </summary>
            public string IsInternExpStr
            {
                get
                {
                    return ((Enums.IsNot)this.IsInternExp).ToString();
                }
            }

            /// <summary>
            /// 毕业院校
            /// </summary>
            public string GraduateSchoolStr
            {
                get
                {
                    return string.IsNullOrEmpty(GraduateSchool) ? "--" : GraduateSchool;
                }
            }

            /// <summary>
            /// 所学专业
            /// </summary>
            public string MajorStr
            {
                get
                {
                    return string.IsNullOrEmpty(this.Major) ? "--" : this.Major;
                }
            }
        }
    }
}
