using System.Collections.Generic;
using LiXinDataAccess.AttendceManage;
using LiXinModels.CourseManage;
using LiXinInterface.AttendceManage;
using LiXinModels.CourseLearn;
using LiXinDataAccess.CourseLearn;
using System;
using LiXinDataAccess.SystemManage;
using LiXinModels;
using LiXinDataAccess;
using System.Data;
using System.Linq;
using LiXinModels.User;
using LiXinDataAccess.User;

namespace LiXinBLL.AttendceManage
{
    public class AttendceBL : IAttendce
    {
        private static AttendceDB AttDB;
        private static Cl_AttendceDB clAttDB;
        private static Cl_MakeUpOrderDB clMakeDB;
        private static Cl_MidAttendceDB clMidDB;
        private static Sys_ParamConfigDB paramConfigDB;
        private static CourseOrderDB CoDB;
        private static Cl_CpaLearnStatusDB cpastatDB;
        private static Sys_LeaderDB groupDB;
        private static UserDB userDB;

        public AttendceBL()
        {
            AttDB = new AttendceDB();
            clAttDB = new Cl_AttendceDB();
            clMakeDB = new Cl_MakeUpOrderDB();
            paramConfigDB = new Sys_ParamConfigDB();
            CoDB = new CourseOrderDB();
            clMidDB = new Cl_MidAttendceDB();
            cpastatDB = new Cl_CpaLearnStatusDB();
            groupDB = new Sys_LeaderDB();
            userDB = new UserDB();
        }

        /// <summary>
        /// 获取考勤列表(有分页)
        /// </summary>
        /// <returns></returns>
        public List<Co_Course> GetAttendceList(int way, out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            var list = AttDB.GetAttendceList(way, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 获得考勤人员数据列表
        /// </summary>
        /// <returns></returns>
        public List<Cl_Attendce> GetAttendUserList(int CourseId, string Order = "asc", string where = "1=1")
        {
            List<Cl_Attendce> tempcu = AttDB.GetAttendUserList(CourseId, Order, where);
            return tempcu;
        }

        /// <summary>
        /// 添加签到表
        /// </summary>
        /// <param name="CourseId">课程ID</param>
        /// <param name="DataTable"></param>
        public string AddAttendces(int CourseId, DataTable dtlist)
        {
            int cg = 0;
            int sb = 0;
            for (int i = 0; i < dtlist.Rows.Count; i++)
            {
                try
                {
                    int userid = Convert.ToInt32(dtlist.Rows[i][1].ToString());
                    string starttime = dtlist.Rows[i][4].ToString();
                    string endtime = dtlist.Rows[i][5].ToString();
                    if (starttime == "--" || string.IsNullOrEmpty(starttime))
                    {
                        starttime = "2050-1-1";
                    }
                    if (endtime == "--" || string.IsNullOrEmpty(endtime))
                    {
                        endtime = "2000-1-1";
                    }
                    //Co_Course coUser = AttDB.GetCoAndUser(CourseId, userid);
                    Sys_User leamodel = GetLeaderIdByUserId(userid);

                    decimal GetScore = 0;
                    decimal AttLessLength = 0;
                    string whereStr = string.Format(" where cco.CourseId={0} and cco.UserId={1}", CourseId, userid);
                    Cl_CourseOrder Comodel = CoDB.GetmodelbyAtt(whereStr);
                    //判断考勤之后的流程是否已经开始走下去了
                    //if (Comodel == null || Comodel.LearnStatus == 0)
                    //{
                    if (Comodel == null)
                    {
                        Comodel = new Cl_CourseOrder();
                        Comodel.CourseId = CourseId;
                        Comodel.UserId = userid;
                        Comodel.OrderTime = DateTime.Now;
                        Comodel.OrderStatus = 3;
                        Comodel.IsLeave = 0;
                        if (leamodel == null)
                        {
                            Comodel.ApprovalUser = "";
                        }
                        else
                        {
                            Comodel.ApprovalUser = leamodel.JobNum;
                        }
                        Comodel.ApprovalMemo = "";
                        Comodel.ApprovalFlag = 0;
                        Comodel.ApprovalDateTime = DateTime.Now;
                        Comodel.ApprovalLimitTime = DateTime.Now;
                        Comodel.OrderTimes = 0;
                        Comodel.LearnStatus = 0;
                        Comodel.GetScore = GetScore;
                        Comodel.PassStatus = 0;
                        Comodel.IsAppoint = 0;
                        Comodel.OrderEndTime = DateTime.Now;
                        Comodel.AttScore = 0;
                        Comodel.AttFlag = 0;
                        CoDB.Add(Comodel);
                    }
                    else
                    {
                        if (Comodel.OrderStatus == 0 || Comodel.OrderStatus == 2)
                        {
                            Comodel.OrderStatus = 3;
                        }
                        else if (Comodel.OrderStatus == 1 && Comodel.IsLeave == 1 && Comodel.ApprovalFlag == 1 && Comodel.ApprovalLimitTime > Comodel.ApprovalDateTime)
                        {
                            Comodel.OrderStatus = 3;
                        }
                        else if (Comodel.TimeOutLeaveApprovalFlag == 1)
                        {
                            Comodel.OrderStatus = 3;
                        }
                        //Comodel.OrderTime = DateTime.Now;
                        //Comodel.GetScore = GetScore;
                        //Comodel.OrderEndTime = DateTime.Now;
                        CoDB.Update(Comodel);
                    }
                    Cl_Attendce attmodel = clAttDB.GetCl_Attendce(CourseId, userid);
                    if (attmodel == null)
                    {
                        attmodel = new Cl_Attendce();
                        attmodel.CourseId = CourseId;
                        attmodel.UserId = userid;
                        attmodel.StartTime = DateTime.Parse(starttime);
                        attmodel.EndTime = DateTime.Parse(endtime);
                        attmodel.OnlineStartTime = DateTime.Parse(starttime);
                        attmodel.OnlineEndTime = DateTime.Parse(endtime);
                        attmodel.LessLength = AttLessLength;
                        if (leamodel == null)
                        {
                            attmodel.ApprovalUser = "";
                        }
                        else
                        {
                            attmodel.ApprovalUser = leamodel.JobNum;
                        }
                        attmodel.ApprovalMemo = "";
                        attmodel.ApprovalFlag = 0;
                        attmodel.ApprovalDateTime = DateTime.Now;
                        clAttDB.AddAttendce(attmodel);
                    }
                    else
                    {
                        attmodel.StartTime = DateTime.Parse(starttime);
                        attmodel.EndTime = DateTime.Parse(endtime);
                        attmodel.OnlineStartTime = DateTime.Parse(starttime);
                        attmodel.OnlineEndTime = DateTime.Parse(endtime);
                        attmodel.LessLength = AttLessLength;
                        if (leamodel == null)
                        {
                            attmodel.ApprovalUser = "";
                        }
                        else
                        {
                            attmodel.ApprovalUser = leamodel.JobNum;
                        }
                        attmodel.ApprovalMemo = "";
                        attmodel.ApprovalFlag = 0;
                        attmodel.ApprovalDateTime = DateTime.Now;
                        clAttDB.UpdateAttendce(attmodel);
                    }
                    if (dtlist.Columns.Count > 6)
                    {
                        for (int j = 0; j < (dtlist.Columns.Count - 6); j++)
                        {
                            string where = string.Format(" where CourseId={0} and UserId={1} and Time={2}", CourseId, userid, j + 1);
                            Cl_MidAttendce clmmodel = clMidDB.GetModel(where);
                            string lintime = dtlist.Rows[i][j + 6].ToString();
                            if (lintime == "--" || string.IsNullOrEmpty(lintime))
                            {
                                lintime = "2000-1-1";
                            }
                            if (clmmodel == null)
                            {
                                clmmodel = new Cl_MidAttendce();
                                clmmodel.CourseId = CourseId;
                                clmmodel.UserId = userid;
                                clmmodel.CreateTime = DateTime.Parse(lintime);
                                clmmodel.Time = j + 1;
                                clMidDB.Add(clmmodel);
                            }
                            else
                            {
                                clmmodel.CreateTime = DateTime.Parse(lintime);
                                clMidDB.Update(clmmodel);
                            }
                        }
                    }
                    cg += 1;
                    //}
                    //else
                    //{
                    //    sb += 1;
                    //}
                }
                catch (Exception ex)
                {
                    sb += 1;
                }
            }
            return "成功" + cg + "条，失败" + sb + "条";
        }

        /// <summary>
        /// 考勤录入
        /// </summary>
        /// <param name="CourseId">课程ID</param>
        /// <param name="userid">用户ID</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        public void AddAttendce(int CourseId, int userid, string starttime, string endtime)
        {
            //Co_Course coUser = AttDB.GetCoAndUser(CourseId, userid);
            Sys_User leamodel = GetLeaderIdByUserId(userid);
            Cl_Attendce attmodel = clAttDB.GetCl_Attendce(CourseId, userid);
            string whereStr = string.Format(" where CourseId={0} and UserId={1}", CourseId, userid);
            Cl_CourseOrder Comodel = CoDB.Getmodel(whereStr);
            if (Comodel == null)
            {
                Comodel = new Cl_CourseOrder();
                Comodel.CourseId = CourseId;
                Comodel.UserId = userid;
                Comodel.OrderTime = DateTime.Now;
                Comodel.OrderStatus = 1;
                Comodel.IsLeave = 0;
                if (leamodel == null)
                {
                    Comodel.ApprovalUser = "";
                }
                else
                {
                    Comodel.ApprovalUser = leamodel.JobNum;
                }
                Comodel.ApprovalMemo = "";
                Comodel.ApprovalFlag = 0;
                Comodel.ApprovalDateTime = DateTime.Now;
                Comodel.ApprovalLimitTime = DateTime.Now;
                Comodel.OrderTimes = 0;
                Comodel.LearnStatus = 0;
                Comodel.GetScore = 0;
                Comodel.PassStatus = 0;
                Comodel.IsAppoint = 0;
                Comodel.OrderEndTime = DateTime.Now;
                Comodel.AttScore = 0;
                Comodel.AttFlag = 0;
                CoDB.Add(Comodel);
            }
            if (attmodel == null)
            {
                attmodel = new Cl_Attendce();
                attmodel.CourseId = CourseId;
                attmodel.UserId = userid;
                attmodel.StartTime = DateTime.Parse(starttime);
                attmodel.EndTime = DateTime.Parse(endtime);
                attmodel.OnlineStartTime = DateTime.Parse(starttime);
                attmodel.OnlineEndTime = DateTime.Parse(endtime);
                attmodel.LessLength = 0;
                if (leamodel == null)
                {
                    attmodel.ApprovalUser = "";
                }
                else
                {
                    attmodel.ApprovalUser = leamodel.JobNum;
                }
                attmodel.ApprovalMemo = "";
                attmodel.ApprovalFlag = 0;
                attmodel.ApprovalDateTime = DateTime.Now;
                clAttDB.AddAttendce(attmodel);
            }
            else
            {
                attmodel.StartTime = DateTime.Parse(starttime);
                attmodel.EndTime = DateTime.Parse(endtime);
                attmodel.OnlineStartTime = DateTime.Parse(starttime);
                attmodel.OnlineEndTime = DateTime.Parse(endtime);
                attmodel.LessLength = 0;
                if (leamodel == null)
                {
                    attmodel.ApprovalUser = "";
                }
                else
                {
                    attmodel.ApprovalUser = leamodel.JobNum;
                }
                attmodel.ApprovalMemo = "";
                attmodel.ApprovalFlag = 0;
                attmodel.ApprovalDateTime = DateTime.Now;
                clAttDB.UpdateAttendce(attmodel);
            }

        }

        /// <summary>
        /// 补预订
        /// </summary>
        /// <param name="CourseId">课程ID</param>
        /// <param name="userid">用户ID</param>
        public void MakeUpOrder(int CourseId, int userid)
        {
            Co_Course jobmodel = AttDB.GetJobID(CourseId, userid);
            Sys_User leamodel = GetLeaderIdByUserId(userid);
            string where = string.Format(" where CourseId={0} and UserId={1}", CourseId, userid);
            Cl_MakeUpOrder model = clMakeDB.GetModel(where);
            Sys_ParamConfig param = paramConfigDB.GetSys_ParamConfig(23);
            if (model == null)
            {
                model = new Cl_MakeUpOrder();
                model.CourseId = CourseId;
                model.UserId = userid;
                if (leamodel == null)
                {
                    model.ApprovalUser = "";
                }
                else
                {
                    model.ApprovalUser = leamodel.JobNum;
                }
                model.ApprovalMemo = "";
                model.ApprovalFlag = 0;
                model.ApprovalDateTime = DateTime.Now;
                model.ApprovalLimitTime = DateTime.Now.AddHours(Convert.ToDouble(param.ConfigValue));
                model.LeaveUserID = jobmodel.JobNum;
                model.IsTimeOut = 0;
                model.Name = jobmodel.TeacherName;
                model.CourseName = jobmodel.CourseName;
                model.LeaveTime = DateTime.Now;
                model.CourseStartTime = jobmodel.StartTime;
                model.CourseEndTime = jobmodel.EndTime;
                model.AttStartTime = jobmodel.attStartTime;
                model.AttEndTime = jobmodel.attEndTime;
                clMakeDB.Add(model);
            }
            else
            {
                if (leamodel == null)
                {
                    model.ApprovalUser = "";
                }
                else
                {
                    model.ApprovalUser = leamodel.JobNum;
                }
                model.ApprovalMemo = "";
                model.ApprovalFlag = 0;
                model.ApprovalDateTime = DateTime.Now;
                model.ApprovalLimitTime = DateTime.Now.AddHours(Convert.ToDouble(param.ConfigValue));
                model.IsTimeOut = 0;
                model.LeaveUserID = jobmodel.JobNum;
                model.Name = jobmodel.TeacherName;
                model.CourseName = jobmodel.CourseName;
                model.LeaveTime = DateTime.Now;
                model.CourseStartTime = jobmodel.StartTime;
                model.CourseEndTime = jobmodel.EndTime;
                model.AttStartTime = jobmodel.attStartTime;
                model.AttEndTime = jobmodel.attEndTime;
                clMakeDB.Update(model);
            }
        }

        /// <summary>
        /// 判断补预订是否超出规定值
        /// </summary>
        /// <param name="count"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool Cl_MakeUpOrderCount(int count, int userid)
        {
            string where = string.Format("  UserId={0}", userid);
            int cou = clMakeDB.Cl_MakeUpOrderCount(where);
            if (cou < count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 考勤结束计算学时
        /// </summary>
        /// <param name="CourseId"></param>
        /// <param name="userid"></param>
        public void UpScore(int CourseId, int userid, DateTime starttime, DateTime endtime)
        {
            decimal GetScore = 0;
            decimal AttLessLength = 0;
            decimal LessLength = 0;
            Co_Course coUser = AttDB.GetCoAndUser(CourseId, userid);
            Cl_Attendce attmodel = clAttDB.GetCl_Attendce(CourseId, userid);
            Sys_ParamConfig param = paramConfigDB.GetSys_ParamConfig(5);
            //Sys_ParamConfig param1 = paramConfigDB.GetSys_ParamConfig(24);
            //违纪学时配置
            var configstr = param.ConfigValue.ToString();
            //课程学时比例配置
            var configstr1 = coUser.CourseLengthDistribute;
            var cstr1 = configstr1.Split(';');
            //违纪学时组装配置集合
            var configList = new List<TempConfig>();
            foreach (var constr in configstr.Split(';'))
            {
                if (constr != "")
                {
                    var str = constr.Split(',');
                    configList.Add(new TempConfig
                    {
                        EndMinite = Convert.ToInt32(str[2]),
                        Percent = Convert.ToInt32(str[3]),
                        StartMinite = Convert.ToInt32(str[1]),
                        Type = Convert.ToInt32(str[0])
                    });
                }
            }
            //计算违纪学时
            if (coUser.AttFlag == 1 && starttime.Year != 2050)
            {
                LessLength = GetLessLength(configList, coUser.AttFlag, coUser.StartTime, coUser.EndTime, starttime, endtime, coUser.CourseLength);
            }
            else if (coUser.AttFlag == 2 && endtime.Year != 2000)
            {
                LessLength = GetLessLength(configList, coUser.AttFlag, coUser.StartTime, coUser.EndTime, starttime, endtime, coUser.CourseLength);
            }
            else if (coUser.AttFlag == 3 && (starttime.Year != 2050 || endtime.Year != 2000))
            {
                LessLength = GetLessLength(configList, coUser.AttFlag, coUser.StartTime, coUser.EndTime, starttime, endtime, coUser.CourseLength);
            }
            string whereStr = string.Format(" where CourseId={0} and UserId={1}", CourseId, userid);
            Cl_CourseOrder Comodel = CoDB.Getmodel(whereStr);
            //计算课程总学时，违纪学时
            //根据课程编号判断是否第一次学习
            if (AttDB.ExistAtts(CourseId, userid))
            {
                if (coUser.AttFlag == 1 && starttime.Year == 2050)
                {
                    GetScore = 0;
                    AttLessLength = 0;
                }
                else if (coUser.AttFlag == 2 && endtime.Year == 2000)
                {
                    GetScore = 0;
                    AttLessLength = 0;
                }
                else if (coUser.AttFlag == 3 && starttime.Year == 2050 && endtime.Year == 2000)
                {
                    GetScore = 0;
                    AttLessLength = 0;
                }
                else
                {
                    //判断是否有课后评估
                    if (coUser.IsTest == 0 && coUser.IsPing == 0)
                    {
                        GetScore = coUser.CourseLength - LessLength;
                        AttLessLength = 0 - LessLength;
                    }
                    else
                    {
                        decimal attLength = coUser.CourseLength * (Convert.ToDecimal(cstr1[0])) / 100;
                        GetScore = attLength - LessLength;
                        GetScore = Math.Round(GetScore, 2, MidpointRounding.AwayFromZero);
                        AttLessLength = 0 - LessLength;
                    }
                }
                //折算CPA学时
                if (coUser.Way == 1 && coUser.IsCPA == 1)
                {
                    if (Comodel != null && Comodel.AttFlag == 0)
                    {
                        Cl_CpaLearnStatus CPAmodel = cpastatDB.GetCl_CpaLearnStatusByCourseId(CourseId, userid);

                        if (CPAmodel == null)
                        {
                            CPAmodel = new Cl_CpaLearnStatus();
                            CPAmodel.CourseID = CourseId;
                            CPAmodel.UserID = userid;
                            CPAmodel.IsAttFlag = 0;
                            CPAmodel.IsPass = 1;
                            CPAmodel.Progress = 0;
                            CPAmodel.LearnTimes = 0;
                            if (coUser.IsMust == 1)
                            {
                                CPAmodel.GetLength = Math.Round((GetScore * 50 / 100), 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                CPAmodel.GetLength = GetScore;
                            }
                            CPAmodel.CpaFlag = 2;
                            CPAmodel.GradeStatus = 1;
                            cpastatDB.SubscribeCPALearnStatus(CPAmodel);
                        }
                        else
                        {

                            CPAmodel.IsAttFlag = 0;
                            CPAmodel.IsPass = 1;
                            CPAmodel.Progress = 0;
                            CPAmodel.LearnTimes = 0;
                            if (coUser.IsMust == 1)
                            {
                                decimal linshi = Convert.ToDecimal((CPAmodel.GetLength * 2) - Comodel.AttScore + GetScore);
                                CPAmodel.GetLength = Math.Round((linshi * 50 / 100), 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                CPAmodel.GetLength = (CPAmodel.GetLength - Comodel.AttScore) + GetScore;
                            }

                            CPAmodel.CpaFlag = 2;
                            CPAmodel.GradeStatus = 1;
                            cpastatDB.UpdateCPALearnStatus(CPAmodel);
                        }
                    }
                }
            }
            else
            {
                GetScore = 0;
                AttLessLength = 0;
            }
            
            if (Comodel != null && Comodel.AttFlag == 0)
            {
                Comodel.GetScore = GetScore + (Comodel.GetScore-Comodel.AttScore);
                Comodel.AttScore = GetScore;
                CoDB.Update(Comodel);
            }
            if (attmodel != null)
            {
                if (Comodel != null && Comodel.AttFlag == 0)
                {
                    attmodel.LessLength = AttLessLength;
                    clAttDB.UpdateAttendce(attmodel);
                }
            }
        }

        /// <summary>
        /// 获取扣除的学时
        /// </summary>
        /// <param name="configList">配置集合</param>
        /// <param name="startTime">课程开始时间</param>
        /// <param name="endTime">课程结束时间</param>
        /// <param name="attStartTime">考勤开始时间</param>
        /// <param name="attEndTime">考勤结束时间</param>
        /// <returns></returns>
        private decimal GetLessLength(List<TempConfig> configList, int AttFlag, DateTime startTime, DateTime endTime, DateTime attStartTime, DateTime attEndTime, decimal courseLength)
        {
            if (AttFlag > 0)
            {
                if (configList.Count > 0)
                {
                    //上课考勤
                    var st = 0.00M;
                    if (AttFlag == 1 || AttFlag == 3)
                    {
                        var tpcs = new TimeSpan(Convert.ToDateTime(startTime.ToString("yyyy-MM-dd HH:mm")).Ticks);
                        var tpas = attStartTime == Convert.ToDateTime("2000-1-1") ? new TimeSpan(Convert.ToDateTime("2050-1-1").Ticks) : new TimeSpan(Convert.ToDateTime(attStartTime.ToString("yyyy-MM-dd HH:mm")).Ticks);
                        var tps = tpas.Subtract(tpcs).TotalMinutes;
                        //计算上课考勤扣除的学时
                        if (tps > 0)
                        {
                            foreach (var c in configList.Where(p => p.Type == 0))
                            {
                                if (tps >= c.StartMinite && (tps < (c.EndMinite+1) || c.EndMinite == -1))
                                {
                                    st = (decimal)((double)courseLength * c.Percent * 0.01);
                                    break;
                                }
                            }
                        }
                    }

                    //下课考勤
                    var et = 0.00M;
                    if (AttFlag == 2 || AttFlag == 3)
                    {
                        var tpce = new TimeSpan(Convert.ToDateTime(endTime.ToString("yyyy-MM-dd HH:mm")).Ticks);
                        var tpae = new TimeSpan(Convert.ToDateTime(attEndTime.ToString("yyyy-MM-dd HH:mm")).Ticks);
                        var tpe = tpce.Subtract(tpae).TotalMinutes;
                        //计算下课考勤扣除的学时
                        if (tpe > 0)
                        {
                            foreach (var c in configList.Where(p => p.Type == 1))
                            {
                                if (tpe >= c.StartMinite && (tpe < (c.EndMinite+1) || c.EndMinite == -1))
                                {
                                    et = (decimal)((double)courseLength * c.Percent * 0.01);
                                    break;
                                }
                            }
                        }
                    }
                    return et > 0 ? (decimal)et : (decimal)st;
                }
            }
            return 0.00M;
        }

        /// <summary>
        /// 在走审批流程时，通过用户userId获取真正的审批人HRID
        /// <para>1.有指定则查找指定领导返回</para>
        /// <para>2.如没指定则查找本身的Leader返回</para>
        /// <para>3.以上都没有返回null</para>
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="flag">标志  0：按顺序操作；1：跳过第2步</param>
        /// <returns>
        /// 返回审批人HRID
        /// </returns>
        private Sys_User GetLeaderIdByUserId(int userId, int flag = 0)
        {
            var leaderList = groupDB.GetGroupList(string.Format(" id in (select groupid from Sys_UserLinkLeader where userid = {0})", userId));
            if (leaderList.Count > 0)
            {
                var leader = userDB.Get(leaderList[0].UserId);
                if (leader != null && leader.JobNum.Trim() != "")
                    return leader;
            }
            if (flag == 0)
            {
                var tmp = userDB.GetList(string.Format(" jobnum = (select LeaderID from Sys_User where userId = {0}) and jobnum <> ''", userId));
                if (tmp.Count > 0)
                    return tmp[0];
            }
            return null;
        }

        /// <summary>
        /// 判断是否第一次学习课程
        /// </summary>
        /// <param name="CourseId">课程</param>
        /// <param name="UserId">用户</param>
        /// <returns></returns>
        public bool ExistAtts(int CourseId, int UserId)
        {
            return AttDB.ExistAtts(CourseId, UserId);
        }

        /// <summary>
        /// 获得消息发送人员信息
        /// </summary>
        /// <returns></returns>
        public List<Message> GetUserinfo(int CourseId, string Users)
        {
            List<Message> Utemp = AttDB.GetUserinfo(CourseId, Users);
            return Utemp;
        }

        /// <summary>
        /// 获取课程考勤状态
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public int GetAttStatus(int CourseId)
        {
            return AttDB.GetAttStatus(CourseId);
        }

        /// <summary>
        /// 更新课程考勤状态
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public bool UpdateAttStatus(int CourseId)
        {
            return AttDB.UpdateAttStatus(CourseId);
        }
    }
}
