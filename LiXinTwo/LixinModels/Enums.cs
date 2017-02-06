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
        /// 课程预订状态，0：无；1：预订；2：指定；3：兼有
        /// </summary>
        public enum CourseIsOrder
        {
            预订 = 1,
            指定 = 2,
            兼有 = 3,
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


        public enum ExamType
        {
            课程评估问卷 = 0,
            普通问卷 = 1,
            讲师评估问卷 = 2,
            新员工评估问卷 = 3
        }

        /// <summary>
        /// 此为公共，有缘人看到不要随便更改
        /// </summary>
        public enum IsNot
        {
            是 = 1,
            否 = 0
        }

        /// <summary>
        /// 是否有实习经验/是否在我所实习 使用 0:是 1：否
        /// </summary>
        public enum YesNot
        {
            是 = 0,
            否 = 1
        }

        /// <summary>
        /// 大写字母-数字对应表
        /// </summary>
        public enum UppercaseLettersNumber
        {
            A = 0,
            B = 1,
            C = 2,
            D = 3,
            E = 4,
            F = 5,
            G = 6,
            H = 7,
            I = 8,
            J = 9,
            K = 10,
            L = 11,
            M = 12,
            N = 13,
            O = 14,
            P = 15,
            Q = 16,
            R = 17,
            S = 18,
            T = 19,
            U = 20,
            V = 21,
            W = 22,
            X = 23,
            Y = 24,
            Z = 25
        }

        /// <summary>
        /// 小写字母-数字对应表
        /// </summary>
        public enum LowercaseLettersNumber
        {
            a = 0,
            b = 1,
            c = 2,
            d = 3,
            e = 4,
            f = 5,
            g = 6,
            h = 7,
            i = 8,
            j = 9,
            k = 10,
            l = 11,
            m = 12,
            n = 13,
            o = 14,
            p = 15,
            q = 16,
            r = 17,
            s = 18,
            t = 19,
            u = 20,
            v = 21,
            w = 22,
            x = 23,
            y = 24,
            z = 25
        }

        /// <summary>
        /// 课程类型
        /// </summary>
        public enum NewCourseType
        {
            集中课程 = 0,
            分组带教 = 1
        }


        /// <summary>
        /// 考勤标示符，谨防有一天他抽风改中文名字。。
        /// </summary>
        public enum AttStatusFlag
        {
            正常 = 0,
            缺勤 = 1,
            早退 = 2,
            迟到 = 3,
            迟到并早退 = 4,
            考勤未上传 = 5
        }


        public enum Sex
        {
            男士 = 0,
            女士 = 1
        }

        public enum SexImport
        {
            男 = 0,
            女 = 1
        }

        public enum ScoreLevel
        {
            较差 = 1,
            改进 = 2,
            标准 = 3,
            良好 = 4,
            优秀 = 5
        }
        /// <summary>
        /// 部门分所考勤标示符
        /// </summary>
        public enum deptAttStatus
        {
            无 = 99,
            待考勤 = 0,
            正常 = 1,
            缺勤 = 2,
            早退 = 4,
            迟到 = 3,
            迟到且早退 = 5
        }

        /// <summary>
        /// 部门开放至全所审批状态
        /// 0:未开放；1：待审批；2：审批通过；3：审批拒绝
        /// </summary>
        public enum OpenApprovalStatus
        {
            未开放 = 0,
            待审批 = 1,
            审批通过 = 2,
            审批拒绝 = 3
        }

        /// <summary>
        /// 课程类型  0 所内  2 注协课程   3 其他形式 4 免修 5 其他有组织形式
        /// </summary>
        public enum courseType
        {
            所内课程 = 0,
            注协课程 = 2,
            其他形式 = 3,
            免修 = 4,
            其他有组织形式 = 5
        }

        /// <summary>
        /// 免修申请内的申请
        /// </summary>
        public enum FreeApprove
        {
            待审批 = 0,
            审批通过 = 1,
            审批退回 = 2
        }
    }
}
