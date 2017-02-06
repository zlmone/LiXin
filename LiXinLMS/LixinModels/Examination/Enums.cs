namespace LiXinModels.Examination
{
    /// <summary>
    ///     问题类型
    /// </summary>
    public enum QuestionType
    {
        问答题 = 1,
        单选题 = 2,
        多选题 = 3,
        判断题 = 4,
        填空题 = 5,
        情景题 = 6
    }

    /// <summary>
    ///     问题用途
    /// </summary>
    public enum QuestionScope
    {
        考试 = 1,
        练习 = 2,
        考试和练习 = 3
    }

    /// <summary>
    ///     试题难度
    /// </summary>
    public enum QuestionLevel
    {
        易 = 1,
        中 = 2,
        难 = 3
    }

    /// <summary>
    ///     试卷难度
    /// </summary>
    public enum ExampaperLevel
    {
        低 = 0,
        中 = 1,
        高 = 2
    }

    /// <summary>
    ///     考试状态
    /// </summary>
    public enum ExaminationStatus
    {
        未做 = 0,
        暂存 = 1,
        已提交 = 2,
        已批阅 = 3
    }

    /// <summary>
    ///     发布标志
    /// </summary>
    public enum PublishFlag
    {
        发布 = 0,
        未发布 = 1
    }


    /// <summary>
    ///     是否自动出试卷
    /// </summary>
    public enum AutoCreatePaper
    {
        抽题测试 = 0,
        全部测试 = 1
    }


    /// <summary>
    ///     考试方式
    /// </summary>
    public enum ExaminationWay
    {
        手工出题 = 0,
        自动出题 = 1
    }

    /// <summary>
    ///     试题显示方式
    /// </summary>
    public enum ExaminationShowWay
    {
        整体显示 = 0,
        单题可逆 = 1,
        单题不可逆 = 2
    }
}