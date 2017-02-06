using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class Enums
    {
        public enum PublishFlag
        {
            已发布 = 1,
            未发布 = 0
        }

        public enum LogType
        {
            操作日志 = 0,
            登录日志 = 1,
            错误日志 = 2,
            同步日志 = 3
        }

        public enum LogTrainType
        {
            培训通知 = 0,
            请假记录 = 1
        }


        public enum Day
        {
            上旬 = 1,
            中旬 = 2,
            下旬 = 3
        }


        public enum IsMust
        {
            必修 = 0,
            选修 = 1
        }

        public enum CpaFlag
        {
            视频学时 = 0,
            CPA学时 = 1,
            折算学时 = 2
        }

        /// <summary>
        /// 课程类型
        /// </summary>
        public enum Way
        {
            集中授课 = 1,
            视频课程 = 2,
            CPA课程 = 3,
            部门自学 = 4
        }

        public enum IsSystem
        {
            内 = 1,
            外 = 0
        }

        public enum IsCPA
        {
            是 = 1,
            否 = 0
        }

        public enum IsLeave
        {
            是 = 1,
            否 = 0
        }

        public enum IsTeacher
        {
            内部 = 1,
            外聘 = 2
        }

        /// <summary>
        /// 1：内部培训；2：社会招聘；3：新进员工；4：实习生
        /// </summary>
        public enum Sort
        {
            内部培训 = 1,
            社会招聘 = 2,
            新进员工 = 3,
            实习生 = 4
        }

        public enum QuestionType
        {
            单选题 = 0,
            多选题 = 1,
            主观题 = 2,
            星评题 = 3
        }

        /// <summary>
        /// 课程预订状态，0：无；1：预订；2：指定；3：两者都有
        /// </summary>
        public enum CourseIsOrder
        {
            预订 = 1,
            指定 = 2,
            两者都有 = 3,
            无 = 0
        }

        public enum State
        {
            未开始 = 1,
            进行中 = 2,
            已结束 = 3
        }

        /// <summary>
        /// 审批状态
        /// </summary>
        public enum ApprovalStatus
        {
            待审批 = 0,
            审批通过 = 1,
            审批拒绝 = 2,
            审批超时 = 3
        }
    }
}
