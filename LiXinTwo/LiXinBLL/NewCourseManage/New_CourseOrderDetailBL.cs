using LiXinDataAccess.NewCourseManage;
using LiXinInterface.NewCourseManage;
using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.User;

namespace LiXinBLL.NewCourseManage
{
    public class New_CourseOrderDetailBL : INew_CourseOrderDetail
    {
        protected static New_CourseOrderDetailDB orderdetailDB;

        public New_CourseOrderDetailBL()
        {
            orderdetailDB = new New_CourseOrderDetailDB();
        }

        public void InsertNew_CourseOrderDetail(New_CourseOrderDetail model)
        {
            orderdetailDB.InsertNew_CourseOrderDetail(model);
        }

        /// <summary>
        /// 删除已存在的课程学习记录
        /// </summary>
        /// <param name="sqlStr">删除条件</param>
        public void DeleteCourseOrder(string sqlStr = "1=2")
        {
            orderdetailDB.DeleteCourseOrder(sqlStr);
        }

        public bool UpdateNew_CourseOrderDetail(New_CourseOrderDetail model)
        {
          return  orderdetailDB.UpdateNew_CourseOrderDetail(model);
        }

        public New_CourseOrderDetail GetSingleNew_CourseOrderDetail(int courseid, int userid)
        {
            return orderdetailDB.GetSingleNew_CourseOrderDetail(courseid,userid);
        }

        public List<New_CourseOrderDetail> GetNew_CourseOrderDetailSeatDetail(string courseid, int userid)
        {
            var list = orderdetailDB.GetNew_CourseOrderDetailSeatDetail(courseid, userid);

            foreach (var item in list)
            {
                item.SeatDetailName = List(item.SeatDetail, userid);
            }

            return list;
        }

        public string List(string seatdetail,int userid)
        {
            if (seatdetail != "")
            { 
                string[] arr = seatdetail.Split(':');

                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].Split(',')[2] == userid.ToString())
                    {
                        return "您的座位是：" + (Convert.ToInt32(arr[i].Split(',')[0]) + 1) + "排" + (Convert.ToInt32(arr[i].Split(',')[1]) + 1) + "座";
                    }
                }
            }
            return "没有你的座位信息";
        }

        /// <summary>
        /// 获取子教室安排的学员
        /// </summary>
        /// <param name="totalCount">总条数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">起始页</param>
        /// <param name="pageLength">每页条数</param>
        /// <returns></returns>
        public List<Sys_User> GetCourseOrderDetailSeatUserDetail(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                             int pageLength = int.MaxValue)
        {
            return orderdetailDB.GetCourseOrderDetailSeatUserDetail(out totalCount, where, startIndex, pageLength);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<New_CourseOrderDetail> GetSearchResult(string where = "1=1")
        {
            return orderdetailDB.GetSearchResult(where);
        }

        /// <summary>
        ///  查看学员在一门课程中是否有多个课程
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<New_CourseOrderDetail> GetCourseOrderDetailIsSingleOrMore(int courseid, int userid)
        {
            return orderdetailDB.GetCourseOrderDetailIsSingleOrMore(courseid,userid);
        }
    }
}
