using System;

namespace LiXinModels.CourseLearn
{
    [Serializable]
    public class Cl_MidAttendce
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// CourseId
        /// </summary>
        public int CourseId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Time
        /// </summary>
        public int Time { get; set; }
    }
}
