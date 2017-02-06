namespace LiXinModels.Examination.DBModel
{
    /// <summary>
    ///     Examination'KnowledgeKey
    ///     MongoDB Document of KnowledgeKey
    /// </summary>
    public class tbKnowledgeKey
    {
        /// <summary>
        ///     KnowledgeKeyID
        /// </summary>
        public int _id { set; get; }

        /// <summary>
        ///     KeyName
        /// </summary>
        public string KeyName { set; get; }

        /// <summary>
        ///     Question Number
        /// </summary>
        public int Number { set; get; }

        /// <summary>
        ///     KeyDescription
        /// </summary>
        public string KeyDescription { set; get; }

        /// <summary>
        ///     Status 0:common;1:deleted
        /// </summary>
        public int Status { set; get; }
    }
}