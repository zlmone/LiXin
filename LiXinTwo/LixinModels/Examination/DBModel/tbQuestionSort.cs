namespace LiXinModels.Examination.DBModel
{
    /// <summary>
    ///     Question Sort
    /// </summary>
    public class tbQuestionSort
    {
        /// <summary>
        ///     Sort ID
        /// </summary>
        public int _id { set; get; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptId { set; get; }

        /// <summary>
        ///     Father ID
        /// </summary>
        public int FatherID { set; get; }

        /// <summary>
        ///     Sort Title
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        ///     Sort Description
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        ///     Status
        ///     0:zhengchang;1:deleted
        /// </summary>
        public int Status { set; get; }
    }
}