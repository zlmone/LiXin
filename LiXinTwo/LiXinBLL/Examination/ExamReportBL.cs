using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;
using LiXinCommon;
using LiXinDataAccess.Examination;
using LiXinDataAccess.User;
using LiXinInterface.Examination;
using LiXinModels.Examination;
using LiXinModels.Examination.DBModel;
using LiXinModels.Examination.ShowModel;
using LiXinModels.User;
using RippleDataAccess.Examination;


namespace LiXinBLL.Examination
{
	public class ExamReportBL : IExamReport
	{
		private readonly ExamReportDB _examReportDB;
		private readonly ExaminationDB _examinationDB;
	    private readonly UserDB _userDB;

	    public ExamReportBL()
		{
			_examReportDB = new ExamReportDB();
			_examinationDB = new ExaminationDB();
			_userDB = new UserDB();
			new ExampaperDB();
			new ExaminationDB();
		}

		#region 磊

		List<tbQuestion> AllQuestion;
		List<tbQuestionSort> AllQuestionSort;
		List<tbExampaper> AllExampaper;
		List<tbExamSendStudent> AllExamSendStudent;

		/// <summary>
		///     获得考试列表（报表模板）
		/// </summary>
		/// <param name="ExamName"></param>
		/// <param name="StartTime"></param>
		/// <param name="EndTime"></param>
		/// <returns></returns>
		public List<RExamination> GetCommonRExamination(string ExamName, string StartTime, string EndTime)
		{
			AllQuestion = _examReportDB.GetAllList<tbQuestion>(true);
			AllQuestionSort = _examReportDB.GetAllList<tbQuestionSort>(true);
			AllExampaper = _examReportDB.GetAllList<tbExampaper>(true);
			AllExamSendStudent = _examinationDB.GettbExamSendStudent();

			var result = new List<RExamination>();
			foreach (tbExamination exam in _examinationDB.GetPublishExam().Where
				(p =>
				 p.ExaminationTitle.Contains(ExamName) && p.ExamEndTime > StartTime.StringToDate(0) &&
				 p.ExamEndTime < (string.IsNullOrEmpty(EndTime) ? EndTime.StringToDate(1) : EndTime.StringToDate(1).AddDays(1).AddSeconds(-1))))
			{
				var tbExampaper = AllExampaper.FirstOrDefault(p => p._id == exam.PaperID);

				var temp = new RExamination
					{
						_id = exam._id,
						ApprovalUser = exam.ApprovalUser,
						CreateTime = exam.CreateTime,
						ExamBeginTime = exam.ExamBeginTime,
						ExamEndTime = exam.ExamEndTime,
						ExaminationTitle = exam.ExaminationTitle,
						tbExamSendStudents = GetCommonRExamSendStudents(exam._id),
						ExamLength = exam.ExamLength,
						ExamRules = exam.ExamRules,
						LastUpdateTime = exam.LastUpdateTime,
						PaperID = exam.PaperID,
						UserID = exam.UserID,
						PassScore = exam.PassScore,
						PercentFlag = exam.PercentFlag,
						PublishedFlag = exam.PublishedFlag,
						RadomOrderFlag = exam.RadomOrderFlag,
						SendMessageFlag = exam.SendMessageFlag,
						ShowType = exam.ShowType,
						Status = exam.Status,
						TestTimes = exam.TestTimes
					};

				if (tbExampaper.ExamType == 0)
					temp.ExamPaperTotalScore = tbExampaper.QuestionList.Sum(p => p.QScore);
				if (tbExampaper.ExamType == 1)
					temp.ExamPaperTotalScore =
						tbExampaper.QuestionRule.Sum(
							p => p.QLevelStr.Split(';').Sum(q => int.Parse(q.Split(':')[1])) * p.QScore);
				result.Add(temp);
			}
			return result;
		}

		/// <summary>
		///     得到考试列表，填充内部参与率和通过率字段
		/// </summary>
		/// <param name="ExamName"></param>
		/// <param name="StartTime"></param>
		/// <param name="EndTime"></param>
		/// <returns></returns>
		public List<RExamination> GetJoinAndPassExamReport(string ExamName, string StartTime, string EndTime)
		{
			List<RExamination> result = GetCommonRExamination(ExamName, StartTime, EndTime);

			foreach (RExamination temp in result)
			{
				temp.TotalPerson = temp.tbExamSendStudents.Count;
				temp.JoinPerson = temp.tbExamSendStudents.Where(p => p.DoExamStatus != 0).Count();
				temp.JoinRate = 100 * temp.tbExamSendStudents.Where(p => p.DoExamStatus != 0).Count() /
								temp.tbExamSendStudents.Count;
				temp.PassPerson = temp.tbExamSendStudents.Where(p => p.IsPass == 1).Count();
				if (temp.JoinPerson == 0)
				{
					temp.PassRate = 0;
				}
				else
				{
					temp.PassRate = 100 * temp.PassPerson / temp.JoinPerson;
				}
			}
			return result;
		}

		/// <summary>
		///     得到考试列表，填充内部题型正确率字段
		/// </summary>
		/// <param name="ExamName"></param>
		/// <param name="StartTime"></param>
		/// <param name="EndTime"></param>
		/// <returns></returns>
		public List<RExamination> GetQuestionCorrectReport(string ExamName, string StartTime, string EndTime)
		{
			List<RExamination> result = GetCommonRExamination(ExamName, StartTime, EndTime);
			//有参与考试考生的考试
			result =
				result.Where(p => p.tbExamSendStudents.Where(q => q.StudentAnswerList.Count > 0).Count() > 0).ToList();

			foreach (RExamination temp in result)
			{
				var QuesTypeCorrectRate = new Dictionary<string, int>();

				List<RExamSendStudent> students = temp.tbExamSendStudents;
				//.Where(p => p.StudentAnswerList.Count() > 0)

				temp.QuesTypeCorrectMaxRate = 0;
				temp.QuesTypeCorrectMinRate = 100;
				//每种题型
				foreach (string questionType in Enum.GetNames(typeof(QuestionType)))
				{
					//有此题型的学生
					IEnumerable<RExamSendStudent> questype =
						students.Where(
							p =>
							p.StudentAnswerList.Where(
								q => q.QType == (int)Enum.Parse(typeof(QuestionType), questionType)).Count() > 0);
					if (questype.Count() > 0)
					{
						QuesTypeCorrectRate[questionType] =
							(int)questype.Average(p => 100 *
														p.StudentAnswerList.Where(
															q =>
															q.GetScore > 0 &&
															q.QType ==
															(int)Enum.Parse(typeof(QuestionType), questionType))
														 .Count()
														/
														p.StudentAnswerList.Where(
															q =>
															q.QType ==
															(int)Enum.Parse(typeof(QuestionType), questionType))
														 .Count());

						if (QuesTypeCorrectRate[questionType]
							> temp.QuesTypeCorrectMaxRate)
						{
							temp.QuesTypeCorrectMaxRate = QuesTypeCorrectRate[questionType];
							temp.QuesTypeCorrectMaxRateName = questionType;
						}

						if (QuesTypeCorrectRate[questionType]
							< temp.QuesTypeCorrectMinRate)
						{
							temp.QuesTypeCorrectMinRate = QuesTypeCorrectRate[questionType];
							temp.QuesTypeCorrectMinRateName = questionType;
						}
					}
					else
						QuesTypeCorrectRate[questionType] = -1;
				}

				IEnumerable<KeyValuePair<string, int>> a = QuesTypeCorrectRate.Where(p => p.Value != -1);
				if (a.Count() > 0)
					temp.QuesTypeCorrectAverageRate = (int)a.Average(p => p.Value);
				else
					temp.QuesTypeCorrectAverageRate = 0;
				temp.QuesTypeCorrectRate = QuesTypeCorrectRate;
			}
			return result;
		}

		/// <summary>
		///     得到考试列表，填充内部题库正确率字段
		/// </summary>
		/// <param name="ExamName"></param>
		/// <param name="StartTime"></param>
		/// <param name="EndTime"></param>
		/// <param name="QuesSortTitle"></param>
		/// <returns></returns>
		public List<RExamination> GetQuestionSortCorrectReport(string ExamName, string StartTime, string EndTime,
															   out List<string> QuesSortTitle)
		{
			List<RExamination> result = GetCommonRExamination(ExamName, StartTime, EndTime);
			//有参与考试考生的考试
			result =
				result.Where(p => p.tbExamSendStudents.Where(q => q.StudentAnswerList.Count > 0).Count() > 0).ToList();

			//所有题库名称
			QuesSortTitle = new List<string>();
			if (result.Count > 0)
				QuesSortTitle = result.Aggregate(new List<string>(), (current, exam) => current.Union
																							(exam.tbExamSendStudents
																								 .Aggregate(
																									 new List<string>(),
																									 (currentS, student)
																									 => currentS.Union
																											(student
																												 .StudentAnswerList
																												 .Select
																												 (p =>
																												  p
																													  .QuestionSortTitle)
																												 .Distinct
																												 ())
																												.ToList()))
																							   .ToList());
			foreach (RExamination temp in result)
			{
				var QuesSortCorrectRate = new Dictionary<string, int>();

				List<RExamSendStudent> students = temp.tbExamSendStudents;
				//.Where(p => p.StudentAnswerList.Count() > 0)

				temp.QuesSortCorrectMaxRate = 0;
				temp.QuesSortCorrectMinRate = 100;
				//每种题型
				foreach (string questionSort in QuesSortTitle)
				{
					//有此题型的学生
					IEnumerable<RExamSendStudent> quessort =
						students.Where(
							p => p.StudentAnswerList.Where(q => q.QuestionSortTitle == questionSort).Count() > 0);
					if (quessort.Count() > 0)
					{
						QuesSortCorrectRate[questionSort] =
							(int)quessort.Average(p => 100 *
														p.StudentAnswerList.Where(
															q => q.GetScore > 0 && q.QuestionSortTitle == questionSort)
														 .Count()
														/
														p.StudentAnswerList.Where(
															q => q.QuestionSortTitle == questionSort).Count());

						if (QuesSortCorrectRate[questionSort]
							> temp.QuesSortCorrectMaxRate)
						{
							temp.QuesSortCorrectMaxRate = QuesSortCorrectRate[questionSort];
							temp.QuesSortCorrectMaxRateName = questionSort;
						}

						if (QuesSortCorrectRate[questionSort]
							< temp.QuesSortCorrectMinRate)
						{
							temp.QuesSortCorrectMinRate = QuesSortCorrectRate[questionSort];
							temp.QuesSortCorrectMinRateName = questionSort;
						}
					}
					else
						QuesSortCorrectRate[questionSort] = -1;
				}

				IEnumerable<KeyValuePair<string, int>> a = QuesSortCorrectRate.Where(p => p.Value != -1);
				if (a.Count() > 0)
					temp.QuesSortCorrectAverageRate = (int)a.Average(p => p.Value);
				else
					temp.QuesSortCorrectAverageRate = 0;
				temp.QuesSortCorrectRate = QuesSortCorrectRate;
			}
			return result;
		}

		/// <summary>
		///     得到考试列表，填充内部成绩分布字段
		/// </summary>
		/// <param name="ExamName"></param>
		/// <param name="StartTime"></param>
		/// <param name="EndTime"></param>
		/// <param name="ScoreSize">成绩梯度的分值</param>
		/// <param name="ScoreCount">几个梯度</param>
		/// <returns></returns>
		public List<RExamination> GetExaminationRecordsDistribution(string ExamName, string StartTime, string EndTime,
																	out List<string> distribution, int ScoreSize = 9,
																	int ScoreCount = 5)
		{
			distribution = new List<string>();
			List<RExamination> result = GetCommonRExamination(ExamName, StartTime, EndTime);
			//有参与考试考生的考试
			result =
				result.Where(p => p.tbExamSendStudents.Where(q => q.StudentAnswerList.Count > 0).Count() > 0).ToList();

			//得到分数梯度
			var Distribution = new List<List<int>>();
			int i = 100;
			int n = 0;
			while (i >= 0 && n < ScoreCount)
			{
				string a = i + "-";
				var temp = new List<int> { i };
				i -= ScoreSize;
				if (n == 0)
					i--;
				if (i >= 0 && n != ScoreCount - 1)
				{
					temp.Add(i);
					a += i;
				}
				else
				{
					temp.Add(0);
					a += 0;
				}

				Distribution.Add(temp);
				distribution.Add(a);
				n++;
				i--;
			}

			foreach (RExamination temp in result)
			{
				var RecordsDistribution = new Dictionary<string, int>();

				for (int m = 0; m < Distribution.Count; m++)
				{
					RecordsDistribution[Distribution[m].Max() + "-" + Distribution[m].Min()] =
						temp.tbExamSendStudents.Count(
							p =>
							100 * p.GetTotalScore / temp.ExamPaperTotalScore <= Distribution[m].Max() &&
							100 * p.GetTotalScore / temp.ExamPaperTotalScore >= Distribution[m].Min());// && p.DoExamStatus == 2
				}

				temp.RecordsDistribution = RecordsDistribution;
			}

			return result;
		}

		public RExamination GetExaminationRecordsDistribution(int id, out List<string> distribution, int ScoreSize = 9,
															  int ScoreCount = 5)
		{
			AllQuestion = _examReportDB.GetAllList<tbQuestion>(true);
			AllQuestionSort = _examReportDB.GetAllList<tbQuestionSort>(true);
			AllExampaper = _examReportDB.GetAllList<tbExampaper>(true);
			AllExamSendStudent = _examinationDB.GettbExamSendStudent();
			distribution = new List<string>();

			tbExamination exam = _examinationDB.GetPublishExam().FirstOrDefault(p => p._id == id);
			if (exam == null)
				return null;
			var tbExampaper = AllExampaper.FirstOrDefault(p => p._id == exam.PaperID);

			var result = new RExamination
				{
					_id = exam._id,
					ApprovalUser = exam.ApprovalUser,
					CreateTime = exam.CreateTime,
					ExamBeginTime = exam.ExamBeginTime,
					ExamEndTime = exam.ExamEndTime,
					ExaminationTitle = exam.ExaminationTitle,
					tbExamSendStudents = GetCommonRExamSendStudents(exam._id),
					ExamLength = exam.ExamLength,
					ExamRules = exam.ExamRules,
					LastUpdateTime = exam.LastUpdateTime,
					PaperID = exam.PaperID,
					UserID = exam.UserID,
					PassScore = exam.PassScore,
					PercentFlag = exam.PercentFlag,
					PublishedFlag = exam.PublishedFlag,
					RadomOrderFlag = exam.RadomOrderFlag,
					SendMessageFlag = exam.SendMessageFlag,
					ShowType = exam.ShowType,
					Status = exam.Status,
					TestTimes = exam.TestTimes
				};

			if (tbExampaper.ExamType == 0)
				result.ExamPaperTotalScore = tbExampaper.QuestionList.Sum(p => p.QScore);
			if (tbExampaper.ExamType == 1)
				result.ExamPaperTotalScore =
					tbExampaper.QuestionRule.Sum(
						p => p.QLevelStr.Split(';').Sum(q => int.Parse(q.Split(':')[1])) * p.QScore);

			//得到分数梯度
			var Distribution = new List<List<int>>();
			int i = 100;
			int n = 0;
			while (i >= 0 && n < ScoreCount)
			{
				string a = i + "-";
				var temp = new List<int> { i };
				i -= ScoreSize;
				if (n == 0)
					i--;
				if (i >= 0 && n != ScoreCount - 1)
				{
					temp.Add(i);
					a += i;
				}
				else
				{
					temp.Add(0);
					a += 0;
				}

				Distribution.Add(temp);
				distribution.Add(a);
				n++;
				i--;
			}

			var RecordsDistribution = new Dictionary<string, int>();

			for (int m = 0; m < Distribution.Count; m++)
			{
				RecordsDistribution[Distribution[m].Max() + "-" + Distribution[m].Min()] =
					result.tbExamSendStudents.Count(
						p =>
						100 * p.GetTotalScore / result.ExamPaperTotalScore <= Distribution[m].Max() &&
						100 * p.GetTotalScore / result.ExamPaperTotalScore >= Distribution[m].Min());
			}

			result.RecordsDistribution = RecordsDistribution;
			return result;
		}

		/// <summary>
		///     获得考生答案列表（报表模板）
		/// </summary>
		/// <param name="ReStudentExamAnswers"></param>
		/// <returns></returns>
		public List<RReStudentExamAnswer> GetCommonRReStudentExamAnswer(List<ReStudentExamAnswer> ReStudentExamAnswers)
		{
			var result = new List<RReStudentExamAnswer>();
			foreach (ReStudentExamAnswer temp in ReStudentExamAnswers)
			{
				var q = AllQuestion.FirstOrDefault(p => p._id == temp.Qid);
				var qs = AllQuestionSort.FirstOrDefault(p => p._id == q.QuestionSortID);
				var instance = new RReStudentExamAnswer
					{
						Answer = temp.Answer,
						DoneFlag = temp.DoneFlag,
						Evlution = temp.Evlution,
						GetScore = temp.GetScore,
						Qid = temp.Qid,
						QType = temp.QType,
						QuestionSortID = q.QuestionSortID,
						QuestionSortTitle = qs.Title
					};
				result.Add(instance);
			}
			return result;
		}

		/// <summary>
		///     获得考生列表（报表模板）
		/// </summary>
		/// <param name="examId"></param>
		/// <returns></returns>
		public List<RExamSendStudent> GetCommonRExamSendStudents(int examId)
		{
			List<tbExamSendStudent> tbExamSendStudents = AllExamSendStudent.Where(p => p.RelationID == examId).ToList();
			var result = new List<RExamSendStudent>();

			foreach (tbExamSendStudent temp in tbExamSendStudents)
			{
				var instance = new RExamSendStudent
					{
						_id = temp._id,
						DoExamStatus = temp.DoExamStatus,
						ExamPaperID = temp.ExamPaperID,
						IsPass = temp.IsPass,
						LastUpdateTime = temp.LastUpdateTime,
						RelationID = temp.RelationID,
						SourceType = temp.SourceType,
						Status = temp.Status,
						SubmitTime = temp.SubmitTime,
						TestTime = temp.TestTime,
						TestTimes = temp.TestTimes,
						UserID = temp.UserID,
						StudentAnswerList = GetCommonRReStudentExamAnswer(temp.StudentAnswerList)
					};

				instance.GetTotalScore = instance.StudentAnswerList.Sum(p => p.GetScore);

				result.Add(instance);
			}
			return result;
		}

		#endregion

		#region 成绩排名与答卷 (浩浩专用)

		/// <summary>
		///     成绩与排名统计
		/// </summary>
		/// <returns></returns>
		public List<ExamGradeRank> GetGradeRank()
		{

			var gradeRanklist = new List<ExamGradeRank>();

			List<tbExamSendStudent> examStudentList =
				_examinationDB.GetAllList<tbExamSendStudent>(Query.And(Query.EQ("Status", 0)));

			Dictionary<int, Sys_User> dicuser = _userDB.GetList().ToDictionary(p => p.UserId, p => p);

			foreach (tbExamination exam in _examinationDB.GetPublishExam())
			{
				List<tbExamSendStudent> StudentList = examStudentList.Where(p => p.RelationID == exam._id).ToList();
				foreach (tbExamSendStudent model in StudentList)
				{
					if (dicuser.ContainsKey(model.UserID))
					{
						int CorrectAnswerNumber = model.StudentAnswerList.Count(p => p.GetScore > 0); //答对题数
						int WrongAnswerNumber = model.StudentAnswerList.Count(p => p.GetScore == 0 && p.DoneFlag == 1);
						//答错题数
						int sumScore = model.StudentAnswerList.Sum(p => p.GetScore); //得分
						int questionScore = StudentList.Where(p => p.UserID == model.UserID)
											.Sum(p => p.StudentAnswerList.Sum(e => e.Score)); //试题的总分数

						var temp = new ExamGradeRank();

						temp.examtionID = exam._id;
						temp.examinationTitle = exam.ExaminationTitle;
						temp.PercentFlag = exam.PercentFlag;
						if (dicuser.Count == 0 || !dicuser.ContainsKey(model.UserID))
						{
							temp.jobnum = "--";
							temp.realname = "--";
							temp.deptcode = "--";
							temp.postcode = "--";
							temp.deptID = 0;
							temp.postID = 0;
						}
						else
						{
							temp.jobnum = dicuser[model.UserID].JobNum;
							temp.realname = dicuser[model.UserID].Realname;
							temp.deptcode = dicuser[model.UserID].DeptCode;
							temp.postcode = dicuser[model.UserID].PostCode;
							temp.deptID = dicuser[model.UserID].DeptId;
							temp.postID = dicuser[model.UserID].PostId;
						}
						if (model.StudentAnswerList == null || model.StudentAnswerList.Count == 0)
						{
							//已答题数
							temp.hasAnswerNumber = 0;
							//未答题数
							temp.NotAnswerNumber = 0;
						}
						else
						{
							//已答题数
							temp.hasAnswerNumber = model.StudentAnswerList.Count(p => p.DoneFlag == 1);
							//未答题数
							temp.NotAnswerNumber = model.StudentAnswerList.Count(p => p.DoneFlag == 0);
						}

						//答对题数
						temp.CorrectAnswerNumber = CorrectAnswerNumber;
						//答错题数
						temp.WrongAnswerNumber = WrongAnswerNumber;
						//正确率
						temp.CorrectRate = model.StudentAnswerList.Count == 0
											   ? 0
											   : Math.Round(
												   Convert.ToDouble(CorrectAnswerNumber) /
												   Convert.ToDouble(model.StudentAnswerList.Count) * 100, 2);
						//错误率
						temp.WrongRate = model.StudentAnswerList.Count == 0
											 ? 0
											 : Math.Round(
												 Convert.ToDouble(WrongAnswerNumber) /
												 Convert.ToDouble(model.StudentAnswerList.Count) * 100, 2);
						//通过状态
						temp.IsPass = sumScore == 0 ? 0 : (sumScore >= (exam.PassScore / 100) * questionScore ? 1 : 0);
						//得分
						temp.sumSocre = sumScore;
						temp.questionScore = questionScore;
						temp.StartDate = exam.ExamBeginTime.ToLocalTime();
						temp.EndDate = exam.ExamEndTime.ToLocalTime();

						gradeRanklist.Add(temp);
					}

				}

				List<int> list = gradeRanklist.Where(p => p.examinationTitle == exam.ExaminationTitle).OrderByDescending(p => p.sumSocre).Select(p => p.sumSocre).ToList();
				foreach (var item in gradeRanklist.Where(p => p.examinationTitle == exam.ExaminationTitle))
				{
					item.Rank = Rank(item.sumSocre, list);
				}

			}


			return gradeRanklist.Count == 0 ? new List<ExamGradeRank>() : gradeRanklist;
		}

		/// <summary>
		/// 获取某个数在一个集合中的排序名次
		/// </summary>
		/// <param name="num">排序数字</param>
		/// <param name="numList">数列</param>
		/// <returns>名次</returns>
		private int Rank(int num, List<int> numList)
		{
			int index = 0;
			for (int i = 0; i < numList.Count(); i++)
			{
				index++;
				if (num == numList[i])
				{
					break;
				}
			}
			return index;
		}

		#endregion
	}
}