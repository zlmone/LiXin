namespace LiXinModels.Examination.ShowModel
{
    /// <summary>
    ///     tree data model
    /// </summary>
    public class TreeModel
    {
        /// <summary>
        ///     id
        /// </summary>
        public int nodeid { set; get; }

        /// <summary>
        ///     name
        /// </summary>
        public string name { set; get; }

        /// <summary>
        ///     click
        /// </summary>
        public string click { set; get; }

        /// <summary>
        ///     open picture
        /// </summary>
        public string iconOpen { set; get; }

        /// <summary>
        ///     close picture
        /// </summary>
        public string iconClose { set; get; }

        /// <summary>
        ///     css
        /// </summary>
        public string css { set; get; }
    }
}