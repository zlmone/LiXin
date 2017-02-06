using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.SystemManage;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using MongoDB.Bson;
using LiXinCommon;

namespace LiXinDataAccess.SystemManage
{
    public class Sys_LogDB : BaseMethod
    {
        public List<Sys_Log> GetAllList()
        {
            return GetAllList<Sys_Log>(Query.And(new[]
                {
                    Query.EQ("IsDelete", 0)
                }));
        }


        public List<Sys_MessageDetails> GetAllDetailsList(int logid)
        {
            return GetAllList<Sys_MessageDetails>(Query.And(new[]
                {
                    Query.EQ("logid", logid)
                }));
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="lastKeyValue">最后一个索引值</param>
        /// <param name="pageSize"></param>
        /// <param name="sortType">排序类型 1升序 -1降序 仅仅针对_id</param>
        /// <param name="userIDs">查询发送人的ID</param>
        /// <param name="AcceptName">接收人</param>
        /// <param name="LogContent">内容</param>
        /// <param name="sLogTime">发送开始时间</param>
        /// <param name="eLogTime">发送结束时间</param>
        /// <param name="LogType">操作类型</param>
        /// <param name="Status">发送成功与否</param>
        /// <returns></returns>
        public List<Sys_Log> GetAllList(out long totalcount, int lastID, int pageSize, int pageindex, int sortType, List<int> userIDs, string AcceptName = "", string LogContent = "", string sLogTime = "", string eLogTime = "", int LogType = -1, int Status = -1)
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

            var querylist = new List<IMongoQuery>();
            if (userIDs.Count > 0)
            {
                querylist.Add(Query.In("CurrentUserId", new BsonArray(userIDs)));
            }
            if (!string.IsNullOrEmpty(AcceptName))
            {
                querylist.Add(Query.Matches("AcceptName", AcceptName));
            }
            if (!string.IsNullOrEmpty(LogContent))
            {
                querylist.Add(Query.Matches("LogContent", LogContent));
            }
            if (LogType != -1)
            {
                querylist.Add(Query.EQ("LogType", LogType));
            }
            if (Status != -1)
            {
                querylist.Add(Query.EQ("Status", Status));
            }
            if (!string.IsNullOrEmpty(sLogTime))
            {
                querylist.Add(Query.GTE("LogTime", sLogTime.StringToDate(0).ToUniversalTime()));
            }
            if (!string.IsNullOrEmpty(eLogTime))
            {
                querylist.Add(Query.LTE("LogTime", eLogTime.StringToDate(0).ToUniversalTime()));
            }


            querylist.Add(Query.And(new[]
                {
                    Query.EQ("IsDelete", 0)
                }));

            var qu = Query.And(querylist);

            return Find<Sys_Log>(out totalcount, qu, "LogTime", null, pageSize, pageindex, sortType);
        }
    }
}
