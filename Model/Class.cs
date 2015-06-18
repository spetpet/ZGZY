using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZGZY.Model
{
    public class Class
    {
        /// <summary>
        /// class主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 课程开始时间
        /// </summary>
        public DateTime ClassStartDate { get; set; }

        /// <summary>
        /// 课程结束时间
        /// </summary>
        public DateTime ClassFinishDate { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 课程地点
        /// </summary>
        public string ClassAddress { get; set; }

        /// <summary>
        /// 课程类型
        /// </summary>
        public string ClassKind { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }

        public string ClassAbout { get; set; }

        public string CheckType { get; set; }

        public string Teacher { get; set; }

        public string uploadfile { get; set; }

    }
}
