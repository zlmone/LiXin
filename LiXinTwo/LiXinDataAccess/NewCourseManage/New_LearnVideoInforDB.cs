using Retech.Core;
using System;
using System.Collections.Generic;
using System.Data;
using Retech.Data;
using System.Linq;
using System.Text;
using LiXinModels.NewCourseManage;

namespace LiXinDataAccess.NewCourseManage
{
    public class New_LearnVideoInforDB : BaseRepository
    {
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model"></param>
        public void SubscribeVedio(New_LearnVideoInfor model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"insert into New_LearnVideoInfor( LearnId,Progress,ResourseId,AttendId,LearnTimes) values (@LearnId,@Progress,@ResourseId,@AttendId,@LearnTimes)";
                var param = new
                {
                    model.LearnId,
                    model.Progress,
                    model.ResourseId,
                    model.AttendId,
                    model.LearnTimes,
                };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }


        /// <summary>
        /// 根据学习视频ID和对应视频资源ID 查找是否已学过
        /// </summary>
        /// <param name="LearnId"></param>
        /// <param name="ResourseId"></param>
        /// <returns></returns>
        public bool IsImport(int LearnId, int ResourseId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere =
                    "select count(*) from New_LearnVideoInfor WHERE LearnId=@LearnId  AND ResourseId=@ResourseId";

                var param = new
                {
                    LearnId,
                    ResourseId
                };
                int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count > 0;
            }
        }

        private static object lockHeper = new Object();
        /// <summary>
        /// 修改播放进度
        /// </summary>
        /// <param name="Progress"></param>
        /// <param name="LearnId"></param>
        /// <param name="ResourseId"></param>
        /// <returns></returns>
        /// 
        public bool UpdateProgress(decimal Progress, int LearnId, int ResourseId)
        {//LearnTimes
            using (IDbConnection conn = OpenConnection())
            {
                string sql = " update New_LearnVideoInfor set Progress=" + Progress + " where LearnId=" + LearnId + " and ResourseId=" + ResourseId;

                lock (lockHeper)
                {
                    return conn.Execute(sql) > 0;
                }


            }
        }

        /// <summary>
        /// 修改播放次数
        /// </summary>
        /// <param name="LearnTimes"></param>
        /// <param name="LearnId"></param>
        /// <param name="ResourseId"></param>
        /// <returns></returns>
        public bool UpdateLearnTimes(int LearnId, int ResourseId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = " update New_LearnVideoInfor set LearnTimes=LearnTimes+1 where LearnId=" + LearnId + " and ResourseId=" + ResourseId;

                return conn.Execute(sql) > 0;

            }
        }


        public List<New_LearnVideoInfor> GetCl_LearnVideoInforList(int LearnId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = " select * from New_LearnVideoInfor where LearnId=" + LearnId;

                return conn.Query<New_LearnVideoInfor>(sql).ToList();
            }
        }



        public New_LearnVideoInfor Get(int LearnId, int ResourseId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = " select * from New_LearnVideoInfor where LearnId=" + LearnId + " and ResourseId=" + ResourseId;

                return conn.Query<New_LearnVideoInfor>(sql).FirstOrDefault();
            }
        }
    }
}
