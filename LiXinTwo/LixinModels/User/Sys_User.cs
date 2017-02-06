using System;
using System.Collections.Generic;

namespace LiXinModels.User
{
    /// <summary>
    ///     Sys_User:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Sys_User
    {
        #region 构造函数
        public Sys_User()
        {
            CoList = new List<Report_Course_show>();
        }
        #endregion
        /// <summary>
        /// 人员下面所有课程
        /// </summary>
        public List<Report_Course_show> CoList { get; set; }

        #region Model
        private int _userid;
        private string _useroldid;
        private string _jobnum;
        private string _username;
        private string _password;
        private string _realname;
        private string _ename;
        private int _sex = 0;
        private string _email;
        private int _deptid = -1;
        private string _deptcode;
        private string _deptname;
        private string _deptpath;
        private int _postid = -1;
        private string _postcode;
        private string _postname;
        private string _postlevel;
        private string _postgrade;
        private DateTime? _freezetime;
        private DateTime? _joindate;
        private DateTime? _passwordfailuretime;
        private int _passwordfailurecount = 0;
        private string _photourl;
        private string _telephone;
        private string _mobilenum;
        private string _jobtitle;
        private string _idcardno;
        private DateTime? _lastlogintime;
        private string _usertype;
        private string _paygrade;
        private string _groupmobilenum;
        private string _probationpaygrade;
        private DateTime? _probationstart = Convert.ToDateTime("2000-01-01");
        private DateTime? _probationend = Convert.ToDateTime("2000-01-01");
        private string _traingrade;
        private string _major;
        private string _graduateschool;
        private string _vdeptid;
        private string _salaryfulfilssystem;
        private string _approvalrole;
        private string _cpa;
        private string _cpano;
        private string _cta;
        private string _cpv;
        private string _iclv;
        private string _realestatevaluers;
        private string _otherprofessional;
        private string _foreignotherqualification;
        private string _otherprofessionsjobtitle;
        private string _sectionleader;
        private string _leaderid;
        private string _cpapractice;
        private string _cpastatus;
        private string _cparelationship;
        private string _subordinatedept;
        private string _subordinatesubstation;
        private string _salaryseries;
        private string _memo;
        private int? _status;
        private int _isdelete = 0;
        private int _isteacher = 0;
        private string _loginid;


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
        /// 职工原ID
        /// A0189
        /// </summary>
        public string UserOldId
        {
            set
            {
                _useroldid = value;
            }
            get
            {
                return _useroldid;
            }
        }
        /// <summary>
        /// 域帐号
        /// A01155
        /// </summary>
        public string LoginId
        {
            set
            {
                _loginid = value;
            }
            get
            {
                return _loginid;
            }
        }
        /// <summary>
        /// 职工ID
        /// HR_ID
        /// A0188
        /// </summary>
        public string JobNum
        {
            set
            {
                _jobnum = value;
            }
            get
            {
                return _jobnum;
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
        /// 员工自助密码
        /// </summary>
        public string Password
        {
            set
            {
                _password = value;
            }
            get
            {
                return _password;
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
        /// 邮箱
        /// Email
        /// </summary>
        public string Email
        {
            set
            {
                _email = value;
            }
            get
            {
                return _email;
            }
        }
        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptId
        {
            set
            {
                _deptid = value;
            }
            get
            {
                return _deptid;
            }
        }
        /// <summary>
        /// 部门代码
        /// </summary>
        public string DeptCode
        {
            set
            {
                _deptcode = value;
            }
            get
            {
                return _deptcode;
            }
        }
        /// <summary>
        /// 部门名称
        /// DEPT_ID
        /// </summary>
        public string DeptName
        {
            set
            {
                _deptname = value;
            }
            get
            {
                return _deptname;
            }
        }
        /// <summary>
        /// 部门路径【同步时计算出来】
        /// </summary>
        public string DeptPath
        {
            set
            {
                _deptpath = value;
            }
            get
            {
                return _deptpath;
            }
        }
        /// <summary>
        /// 岗位ID
        /// </summary>
        public int PostId
        {
            set
            {
                _postid = value;
            }
            get
            {
                return _postid;
            }
        }
        /// <summary>
        /// 岗位代码
        /// </summary>
        public string PostCode
        {
            set
            {
                _postcode = value;
            }
            get
            {
                return _postcode;
            }
        }
        /// <summary>
        /// 岗位名称
        /// GW_NAME
        /// </summary>
        public string PostName
        {
            set
            {
                _postname = value;
            }
            get
            {
                return _postname;
            }
        }
        /// <summary>
        /// 岗位级别
        /// A01048
        /// </summary>
        public string PostLevel
        {
            set
            {
                _postlevel = value;
            }
            get
            {
                return _postlevel;
            }
        }
        /// <summary>
        /// 岗位等级
        /// A01088
        /// </summary>
        public string PostGrade
        {
            set
            {
                _postgrade = value;
            }
            get
            {
                return _postgrade;
            }
        }
        /// <summary>
        /// 冻结时间
        /// </summary>
        public DateTime? FreezeTime
        {
            set
            {
                _freezetime = value;
            }
            get
            {
                return _freezetime;
            }
        }
        /// <summary>
        /// 进立信日期
        /// A01047
        /// </summary>
        public DateTime? JoinDate
        {
            set
            {
                _joindate = value;
            }
            get
            {
                return _joindate;
            }
        }
        /// <summary>
        /// 密码错误时间
        /// </summary>
        public DateTime? PasswordFailureTime
        {
            set
            {
                _passwordfailuretime = value;
            }
            get
            {
                return _passwordfailuretime;
            }
        }
        /// <summary>
        /// 密码错误次数
        /// </summary>
        public int PasswordFailureCount
        {
            set
            {
                _passwordfailurecount = value;
            }
            get
            {
                return _passwordfailurecount;
            }
        }
        /// <summary>
        /// 个人照片
        /// </summary>
        public string PhotoUrl
        {
            set
            {
                _photourl = value;
            }
            get
            {
                return _photourl;
            }
        }
        /// <summary>
        /// 联系电话
        /// A0143
        /// </summary>
        public string Telephone
        {
            set
            {
                _telephone = value;
            }
            get
            {
                return _telephone;
            }
        }
        /// <summary>
        /// 手机
        /// MobileTel
        /// </summary>
        public string MobileNum
        {
            set
            {
                _mobilenum = value;
            }
            get
            {
                return _mobilenum;
            }
        }
        /// <summary>
        /// 职务
        /// A01042
        /// </summary>
        public string JobTitle
        {
            set
            {
                _jobtitle = value;
            }
            get
            {
                return _jobtitle;
            }
        }
        /// <summary>
        /// 身份证号
        /// A0177
        /// </summary>
        public string IdCardNo
        {
            set
            {
                _idcardno = value;
            }
            get
            {
                return _idcardno;
            }
        }
        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public DateTime? LastLoginTime
        {
            set
            {
                _lastlogintime = value;
            }
            get
            {
                return _lastlogintime;
            }
        }
        /// <summary>
        /// 人员类别
        /// A0191
        /// </summary>
        public string UserType
        {
            set
            {
                _usertype = value;
            }
            get
            {
                return _usertype;
            }
        }
        /// <summary>
        /// 薪酬级别
        /// A01105
        /// </summary>
        public string PayGrade
        {
            set
            {
                _paygrade = value;
            }
            get
            {
                return _paygrade;
            }
        }
        /// <summary>
        /// 短号
        /// A01149
        /// </summary>
        public string GroupMobileNum
        {
            set
            {
                _groupmobilenum = value;
            }
            get
            {
                return _groupmobilenum;
            }
        }
        /// <summary>
        /// 试用期薪酬级别
        /// A01202
        /// </summary>
        public string ProbationPayGrade
        {
            set
            {
                _probationpaygrade = value;
            }
            get
            {
                return _probationpaygrade;
            }
        }
        /// <summary>
        /// 试用期开始时间
        /// A01199
        /// </summary>
        public DateTime? ProbationStart
        {
            set
            {
                _probationstart = value;
            }
            get
            {
                return _probationstart;
            }
        }
        /// <summary>
        /// 试用期结束时间
        /// A0144
        /// </summary>
        public DateTime? ProbationEnd
        {
            set
            {
                _probationend = value;
            }
            get
            {
                return _probationend;
            }
        }
        /// <summary>
        /// 培训级别
        /// A01187
        /// </summary>
        public string TrainGrade
        {
            set
            {
                _traingrade = value;
            }
            get
            {
                return _traingrade;
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
        /// 所属虚拟部门
        /// vdept_id
        /// </summary>
        public string VDeptId
        {
            set
            {
                _vdeptid = value;
            }
            get
            {
                return _vdeptid;
            }
        }
        /// <summary>
        /// 薪酬体系
        /// A01099
        /// </summary>
        public string SalaryFulfilsSystem
        {
            set
            {
                _salaryfulfilssystem = value;
            }
            get
            {
                return _salaryfulfilssystem;
            }
        }
        /// <summary>
        /// 审批角色
        /// A01077
        /// </summary>
        public string ApprovalRole
        {
            set
            {
                _approvalrole = value;
            }
            get
            {
                return _approvalrole;
            }
        }
        /// <summary>
        /// 注册会计师
        /// A01111
        /// </summary>
        public string CPA
        {
            set
            {
                _cpa = value;
            }
            get
            {
                return _cpa;
            }
        }
        /// <summary>
        /// 注册会计师证书编号
        /// A01113
        /// </summary>
        public string CPANo
        {
            set
            {
                _cpano = value;
            }
            get
            {
                return _cpano;
            }
        }
        /// <summary>
        /// 注册税务师
        /// A01118
        /// </summary>
        public string CTA
        {
            set
            {
                _cta = value;
            }
            get
            {
                return _cta;
            }
        }
        /// <summary>
        /// 注册资产评估师
        /// A01119
        /// </summary>
        public string CPV
        {
            set
            {
                _cpv = value;
            }
            get
            {
                return _cpv;
            }
        }
        /// <summary>
        /// 土地估价师
        /// A01120
        /// </summary>
        public string ICLV
        {
            set
            {
                _iclv = value;
            }
            get
            {
                return _iclv;
            }
        }
        /// <summary>
        /// 房地产估价师
        /// A01121
        /// </summary>
        public string RealEstateValuers
        {
            set
            {
                _realestatevaluers = value;
            }
            get
            {
                return _realestatevaluers;
            }
        }
        /// <summary>
        /// 其他专业技术资格
        /// A01122
        /// </summary>
        public string OtherProfessional
        {
            set
            {
                _otherprofessional = value;
            }
            get
            {
                return _otherprofessional;
            }
        }
        /// <summary>
        /// 国外其他执业资格
        /// A01123
        /// </summary>
        public string ForeignOtherQualification
        {
            set
            {
                _foreignotherqualification = value;
            }
            get
            {
                return _foreignotherqualification;
            }
        }
        /// <summary>
        /// 其他专业技术职称
        /// A01128
        /// </summary>
        public string OtherProfessionsJobTitle
        {
            set
            {
                _otherprofessionsjobtitle = value;
            }
            get
            {
                return _otherprofessionsjobtitle;
            }
        }
        /// <summary>
        /// 部门负责人
        /// A01157
        /// </summary>
        public string SectionLeader
        {
            set
            {
                _sectionleader = value;
            }
            get
            {
                return _sectionleader;
            }
        }
        /// <summary>
        /// 直接上级ID
        /// A011572
        /// </summary>
        public string LeaderID
        {
            set
            {
                _leaderid = value;
            }
            get
            {
                return _leaderid;
            }
        }
        /// <summary>
        /// CPA执业年限
        /// A01189
        /// </summary>
        public string CPAPractice
        {
            set
            {
                _cpapractice = value;
            }
            get
            {
                return _cpapractice;
            }
        }
        /// <summary>
        /// CPA状态
        /// A01158
        /// </summary>
        public string CPAStatus
        {
            set
            {
                _cpastatus = value;
            }
            get
            {
                return _cpastatus;
            }
        }
        /// <summary>
        /// 注册会计师关系所在地
        /// A01260
        /// </summary>
        public string CPARelationship
        {
            set
            {
                _cparelationship = value;
            }
            get
            {
                return _cparelationship;
            }
        }
        /// <summary>
        /// 所属事业部
        /// A01261
        /// </summary>
        public string SubordinateDept
        {
            set
            {
                _subordinatedept = value;
            }
            get
            {
                return _subordinatedept;
            }
        }
        /// <summary>
        /// 所属分所
        /// A012162
        /// </summary>
        public string SubordinateSubstation
        {
            set
            {
                _subordinatesubstation = value;
            }
            get
            {
                return _subordinatesubstation;
            }
        }
        /// <summary>
        /// 薪酬级数
        /// A01263
        /// </summary>
        public string SalarySeries
        {
            set
            {
                _salaryseries = value;
            }
            get
            {
                return _salaryseries;
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo
        {
            set
            {
                _memo = value;
            }
            get
            {
                return _memo;
            }
        }
        /// <summary>
        /// 是否为讲师
        /// 0：不是
        /// 1：是（内部）
        /// 2：是（外部）
        /// </summary>
        public int IsTeacher
        {
            set
            {
                _isteacher = value;
            }
            get
            {
                return _isteacher;
            }
        }
        /// <summary>
        /// 状态
        /// <para>0：正常</para>
        /// <para>1：冻结（手动冻结；可解冻）</para>
        /// <para>2：冻结（自动冻结；不可解冻）</para>
        /// </summary>
        public int? Status
        {
            set
            {
                _status = value;
            }
            get
            {
                return _status;
            }
        }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDelete
        {
            set
            {
                _isdelete = value;
            }
            get
            {
                return _isdelete;
            }
        }
        /// <summary>
        /// 员工类别
        /// A01147
        /// </summary>
        public string StaffType
        {
            get;
            set;
        }
        private DateTime? _CPACreateDate;

        /// <summary>
        /// CPA注册日期
        ///  A01115
        /// </summary>
        public DateTime? CPACreateDate
        {
            set
            {
                _CPACreateDate = value;
            }
            get
            {
                return _CPACreateDate;
            }
        }
        #endregion Model

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

        public string StatusStr
        {
            get
            {
                if (Status == 0)
                    return "正常";
                else if (Status == 1)
                {
                    return "冻结";
                    //if (FreezeTime.HasValue)
                    //    return "临时冻结";
                    //return "永久冻结";
                }
                else if (Status == 2)
                    return "冻结";
                else
                    return "";
            }
        }

        /// <summary>
        ///     用户的所有角色
        /// </summary>
        public string RoleName
        {
            get;
            set;
        }

        /// <summary>
        ///     用于显示用户的部门信息
        /// </summary>
        public string DeptCodeStr
        {
            get
            {
                if (string.IsNullOrEmpty(DeptName))
                    return "暂无";
                return DeptName;
            }
        }

        /// <summary>
        ///     用于显示用户的岗位信息
        /// </summary>
        public string PostCodeStr
        {
            get
            {
                if (string.IsNullOrEmpty(PostName))
                    return "暂无";
                return PostName;
            }
        }

        /// <summary>
        ///     用于显示用户的头像信息
        /// </summary>
        public string PhotoUrlStr
        {
            get
            {
                if (string.IsNullOrEmpty(PhotoUrl))
                    return "../../Images/photo/default.jpg";
                return PhotoUrl;
            }
        }


        public string JoinDateStr
        {
            get
            {
                return this.JoinDate == null ? "--" : Convert.ToDateTime(this.JoinDate).ToString("yyyy-MM-dd");
            }
        }

        public string teacherType
        {
            get
            {
                return ((Enums.IsTeacher)this.IsTeacher).ToString();
            }
        }

        /// <summary>
        /// 是否CPA 0:否 1:是
        /// </summary>
        public int IsCPA
        {
            get
            {
                if (string.IsNullOrEmpty(CPA))
                {
                    return 0;
                }
                else
                {
                    if (CPA == "是")
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// 培训级别list
        /// </summary>
        public List<string> TraindGradeList
        {
            get;
            set;
        }

        public bool IsUpdate
        {
            get;
            set;
        }

        public string PayGradestr
        {
            get
            {
                return string.IsNullOrEmpty(this.PayGrade) ? "--" : this.PayGrade;
            }

        }

        public string Emailstr
        {
            get
            {
                return string.IsNullOrEmpty(this.Email) ? "--" : this.Email;
            }

        }
        public string Telephonestr
        {
            get
            {
                return string.IsNullOrEmpty(this.Telephone) ? "--" : this.Telephone;
            }

        }
        /// <summary>
        ///  学分
        /// </summary>
        public decimal GetScore { get; set; }
        #region 课程报名

        private int? _isapply;
        /// <summary>
        /// 是否报名
        /// </summary>
        public int? IsApply
        {
            get
            {
                if (_isapply.HasValue)
                {
                    if (_isapply != 0)
                        return _isapply;
                }
                return 0;
            }
            set
            {
                _isapply = value;
            }
        }

        /// <summary>
        /// 是否报名
        /// </summary>
        public string ApplyStr
        {
            get
            {
                if (IsApply > 0)
                {
                    if (CourseLeave == 1)
                    {
                        if (CourseApprovalDateTime.HasValue && CourseApprovalDateTime < CourseApprovalLimitTime && this.CourseLeaveApprovalFlag==1)
                        {
                            return "否";
                        }
                    }
                    return "是";
                }
                return "否";
            }
        }

        public DateTime? CourseApprovalDateTime { get; set; }
        public DateTime? CourseApprovalLimitTime { get; set; }

        /// <summary>
        /// 是否他人指定
        /// </summary>
        public int IsAppoint
        {
            get;
            set;
        }

        /// <summary>
        /// 报名性质
        /// </summary>
        public string ApplyPropertiesStr
        {
            get
            {
                if (IsApply == 1 || IsApply == 2)
                {
                    if (IsAppoint == 1)
                        return "部门指定";
                    if (IsAppoint == 2)
                        return "总所指定";
                    return "自主预订";
                }
                else if (IsApply == 3)
                    return "补预订";
                else
                {
                    if (OrderTime != DateTime.MinValue)
                        return "自主预订";
                    return "——";
                }
            }
        }

        /// <summary>
        /// （二期部门分所）报名性质
        /// </summary>
        public string DepApplyPropertiesStr
        {
            get
            {
                if (IsApply == 1)
                {
                    if (IsAppoint == 1)
                    {
                        return "指定";
                    }
                    else if (IsAppoint == 0)
                    {
                        return "自主预订";
                    }
                    else
                    {
                        return "考勤指定";
                    }
                }
                else if (IsApply == 2)
                    return "补预订";
                else
                {
                    if (OrderTime != DateTime.MinValue)
                        return "自主预订";
                    return "——";
                }
            }
        }

        public int? CourseOrder { get; set; }
        public int? CourseLeave { get; set; }
        public int? CourseLeaveApprovalFlag { get; set; }
        public DateTime OrderTime { get; set; }

        public string OrderTimeStr
        {
            get
            {
                if (OrderTime != DateTime.MinValue)
                {
                    return OrderTime.ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    return "——";
                }
            }
        }

        public string CourseOrderStr
        {
            get
            {
                if (CourseOrder.HasValue)
                {
                    if (CourseOrder == 0)
                        return "已退订";
                    else if (CourseOrder == 1)
                    {
                        if (CourseLeave.HasValue && CourseLeave == 1)
                        {
                            if (CourseLeaveApprovalFlag.HasValue && CourseLeaveApprovalFlag == 1)
                            {
                                return "已请假";
                            }
                        }
                    }
                    return "已报名";
                }
                return "未报名";
            }
        }

        #endregion

        /// <summary>
        /// 是否培训负责人
        /// 0：否；1：是
        /// </summary>
        public int TrainMaster { get; set; }

        /// <summary>
        /// TrainMaster为1时所负责的部门ID集合
        /// </summary>
        public string ManageDeparts { set; get; }

        /// <summary>
        /// 是否总部，0：是；1：否
        /// </summary>
        public int IsMain { get; set; }

        public int totalCount { get; set; }

        public int ordertimes { get; set; }

        /// <summary>
        /// 0：否；1：是
        /// </summary>
        public int IsNew { get; set; }



        /// <summary>
        /// 是否有实习经验 0有 1没有
        /// </summary>
        public int IsInternExp { get; set; }

        /// <summary>
        /// 是否在我所实习  0有  1没有
        /// </summary>
        public int IsOurIntern { get; set; }

        /// <summary>
        /// 实习部门
        /// </summary>
        public string InternDept { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string NumberID { get; set; }

        /// <summary>
        /// 学号是否固定 1：固定不可更改 非1：可更改 
        /// （注：此字段在新进员工班级管理中使用）
        /// </summary>
        public int IsChangeNumberId { get; set; }
        /// <summary>
        /// 老的的培训级别
        /// </summary>
        public string OldTrainGrade { set; get; }



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
        /// 是否在我所实习
        /// </summary>
        public string IsOurInternStr
        {
            get
            {
                return ((Enums.IsNot)this.IsOurIntern).ToString();
            }
        }


        /// <summary>
        /// 实习部门
        /// </summary>
        public string InternDeptStr
        {
            get
            {
                return string.IsNullOrEmpty(InternDept) ? "--" : InternDept;
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

        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 组ID
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// 班级ID
        /// </summary>
        public int ClassId { set; get; }

        public string NumberIDStr
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.NumberID) ? "--" : this.NumberID;
            }

        }

        /// <summary>
        /// 独立考试得分
        /// </summary>
        public decimal examScore { get; set; }

        /// <summary>
        /// 独立考试总分
        /// </summary>
        public decimal examSumScore { get; set; }

        #region 部门课程扩展
        /// <summary>
        /// 0：未退订；1：预定；
        /// </summary>
        public int OrderStatus { get; set; }


        public string OrderStatusStr
        {
            get
            {
                if ((this.OrderStatus == 1 && isleave == 0) || (this.OrderStatus == 1 && isleave == 1 && (approvalflag == 0 || approvalflag == 2)))
                    return "是";
                else
                    return "否";
            }
        }
        public string DepAppointStr
        {
            get
            {
                switch (this.IsAppoint)
                {
                    case 0:
                        return "自主预订";
                    case 1:
                        return "指定";
                    case 2:
                        return "考勤指定";
                    default:
                        return "--";
                }
            }
        }
        public int isleave { get; set; }

        public int approvalflag { get; set; }
        
        /// <summary>
        /// 部门分所分组的名称
        /// </summary>
        public string DepartSetName { get; set; }

        /// <summary>
        /// 部门分所分组的ID
        /// </summary>
        public int DepartSetID { get; set; }

        /// <summary>
        /// 历史上是新进员工
        /// </summary>
        public int IsHistoryNew { get; set; }

        /// <summary>
        /// 成为新进员工的年度
        /// </summary>
        public int NewYear { get; set; }
        #endregion

        #region 学时扩展

        /// <summary>
        ///集中课程学时
        /// </summary>
        public decimal CoScore { get; set; }
        /// <summary>
        ///视频课程学时
        /// </summary>
        public decimal voScore { get; set; }
        /// <summary>
        ///CPA折算目标学时
        /// </summary>
        public decimal CPAToScore { get; set; }
        /// <summary>
        ///部门/分所学时
        /// </summary>
        public decimal DepScore { get; set; }
        
        /// <summary>
        ///所有课程实际学时
        /// </summary>
        public decimal SumScore
        {
            get
            {
                return CoScore + voScore + CPAToScore + DepScore;
            }
        }
        /// <summary>
        ///所有课程有效学时
        /// </summary>
        public decimal EffectScore { get; set; }
        /// <summary>
        ///CPA实际学时
        /// </summary>
        public decimal CPAScore { get; set; }
        /// <summary>
        ///CPA有效学时
        /// </summary>
        public decimal EffectCPAScore { get; set; }
        /// <summary>
        ///年度所内目标学时
        /// </summary>
        public decimal TargetScore { get; set; }

        /// <summary>
        ///年度CPA目标学时
        /// </summary>
        public decimal TargetCPAScore { get; set; }

        /// <summary>
        /// 测试通过次数
        /// </summary>
        public int passNumber { get; set; }

        public int IsFree { get; set; }

        public decimal FreeCPAScore { get; set; }
        #endregion
    }
}