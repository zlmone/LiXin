using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels;
namespace SMTPApp
{
    /// <summary>
    /// 邮件属性
    /// </summary>
    public class EmailInforMation
    {
        private string _From = "";
		private List<string> _Tolist = new List<string>();
		private string _subobject = "";
		private string _context = "";
		private string _username = "";
		private string _password = "";
		private int _iport = 25;
		private string _host = "";

		/// <summary>
		/// 发件人
		/// </summary>
		public string From
		{
			get
			{
				return _From;
			}
			set
			{
				_From = value;
			}
		}

		/// <summary>
		/// 接收人的集合
		/// </summary>
		public List<string> ToList
		{
			get
			{
				return _Tolist;
			}
			set
			{
				_Tolist = value;
			}
		}

		/// <summary>
		/// 邮件主题
		/// </summary>
		public string Subject
		{
			get
			{
				return _subobject;
			}
			set
			{
				_subobject = value;
			}
		}

		/// <summary>
		/// 邮件内容
		/// </summary>
		public string Context
		{
			get
			{
				return _context;
			}
			set
			{
				_context = value;
			}
		}

		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName
		{
			get
			{
				return _username;
			}
			set
			{
				_username = value;
			}
		}

		/// <summary>
		/// 密码
		/// </summary>
		public string PassWord
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
			}
		}

		/// <summary>
		/// 邮件发送的端口号
		/// </summary>
		public int IPort
		{
			get
			{
				return _iport;
			}
			set
			{
				_iport = value;
			}
		}

		/// <summary>
		/// 服务器的地址
		/// </summary>
		public string Host
		{
			get
			{
				return _host;
			}
			set
			{
				_host = value;
			}
		}


		/// <summary>
		/// 1:邮件 0：站内信  2：全部发送
		/// </summary>
		public int SendFlag
		{
			get;
			set;
		}


		/// <summary>
		/// 开始/提交时间
		/// </summary>
		public DateTime startTime
		{
			get;
			set;
		}

		/// <summary>
		/// 结束/截止时间        
		/// </summary>
		public DateTime endTime
		{
			get;
			set;
		}

		/// <summary>
		/// 考试，等等的名字
		/// </summary>
		public string mailName
		{
			get;
			set;
		}

		public List<int> userList
		{
			get;
			set;
		}
	}

	/// <summary>
	/// 邮件支持类型
	/// </summary>
	public enum EmailType : int
	{
		Html = 0,
		Text = 1,

    }
}
