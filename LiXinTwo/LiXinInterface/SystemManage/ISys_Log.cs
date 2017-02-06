using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.SystemManage;

namespace LiXinInterface.SystemManage
{
    public interface ISys_Log
    {
        int Add(Sys_Log log);
        List<Sys_Log> GetAllList();

        /// <summary>
        /// 添加信息详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddMessageDetials(Sys_MessageDetails model);


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="logid"></param>
        /// <returns></returns>
        List<Sys_MessageDetails> GetAllList(int logid);

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
        List<Sys_Log> GetAllList(out long totalcount, int lastID, int pageSize,int pageindex, int sortType, List<int> userIDs, string AcceptName = "", string LogContent = "", string sLogTime = "", string eLogTime = "", int LogType = -1, int Status = -1);
    }
}
