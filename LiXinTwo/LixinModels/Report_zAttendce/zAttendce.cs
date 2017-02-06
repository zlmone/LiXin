using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Report_zAttendce
{
    public class zAttendce
    {
        public string _deptNames;
        public int _zongNum;
        public int _fenNum;

        #region model

        public int DepartmentId { get; set; }

        public string DeptName { get; set; }

        /// <summary>
        /// 一期考勤缺勤人数统计
        /// </summary>
        public int Att { get; set; }

        /// <summary>
        /// 一期迟到人数统计
        /// </summary>
        public int Late { get; set; }

        /// <summary>
        /// 一期早退人数统计
        /// </summary>
        public int Early { get; set; }

        /// <summary>
        /// 一期迟到且早退人数统计
        /// </summary>
        public int LateAndEarly { get; set; }

        /// <summary>
        /// 二期部门分所缺勤早退
        /// </summary>
        public int Att2 { get; set; }

        /// <summary>
        /// 二期部门分所迟到
        /// </summary>
        public int Late2 { get; set; }

        /// <summary>
        /// 二期部门分所早退
        /// </summary>
        public int Early2 { get; set; }

        /// <summary>
        /// 二期部门分所迟到且早退
        /// </summary>
        public int LateAndEarly2 { get; set; }

        /// <summary>
        /// 二期部门分所视频转播缺勤
        /// </summary>
        public int Att3 { get; set; }

        /// <summary>
        /// 二期部门分所视频转播迟到
        /// </summary>
        public int Late3 { get; set; }

        /// <summary>
        /// 二期部门分所视频转播早退
        /// </summary>
        public int Early3 { get; set; }

        /// <summary>
        /// 二期部门分所视频转播迟到且早退
        /// </summary>
        public int LateAndEarly3 { get; set; }

        /// <summary>
        /// 部门层级关系
        /// </summary>
        public string DeptNames { get; set; }

        public string fenDeptName { get; set; }

        #endregion

        #region 扩展
       
        public int IsZong
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DeptNames))
                {
                    return 0;
                }
                if (DeptNames.Contains("上海"))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
           
        }

        public int ZongIS { set; get; }

        /// <summary>
        /// 缺勤
        /// </summary>
        public int AttSum
        {
            get { return Att + Att2 + Att3; }
        }

        /// <summary>
        /// 迟到
        /// </summary>
        public int LateSum
        {
            get { return Late + Late2 + Late3; }
        }

        /// <summary>
        /// 早退
        /// </summary>
        public int EarlySum
        {
            get { return Early + Early2 + Early3; }
        }

        /// <summary>
        /// 迟到并早退
        /// </summary>
        public int LateAndEarlySum
        {
            get { return LateAndEarly + LateAndEarly2 + LateAndEarly3; }
        }

        public int AttSum2 { set; get; }
        public int LateSum2 { get; set; }
        public int EarlySum2 { get; set; }
        public int LateAndEarlySum2 { get; set; }
        public string deptIDs { get; set; }

        public int sumSum
        {
            get { return AttSum2 + LateSum2 + EarlySum2 + LateAndEarlySum2; }
        }
        #endregion

    }

    
}
