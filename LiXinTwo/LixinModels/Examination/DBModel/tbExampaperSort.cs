namespace LiXinModels.Examination.DBModel
{
    /// <summary>
    ///     Exampaper Sort
    /// </summary>
    public class tbExampaperSort
    {
        /// <summary>
        ///     SortID
        /// </summary>
        public int _id { set; get; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptId { set; get; }

        /// <summary>
        ///     Sort FatherID
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