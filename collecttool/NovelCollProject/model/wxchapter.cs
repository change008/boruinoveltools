using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//对象实体
namespace NovelCollProject.model
{
    //wxchapter
    [Serializable()]
    public class wxchapter
    {
        /// <summary>
        /// 对方章节id
        /// </summary>
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 采集Id--配置的采集id
        /// </summary>
        public int CollectionId
        {
            get;
            set;
        }
        /// <summary>
        /// Intro--简介
        /// </summary>
        public string Intro
        {
            get;
            set;
        }
        /// <summary>
        /// SortOrder--章节序号从1往后递增
        /// </summary>
        public int SortOrder
        {
            get;
            set;
        }
        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// ChapterType 0:免费 1:收费
        /// </summary>
        public int ChapterType
        {
            get;
            set;
        }
        /// <summary>
        /// Pirce 价格:单位分
        /// </summary>
        public int Pirce
        {
            get;
            set;
        }

        /// <summary>
        /// ContentLen
        /// </summary>
        public int ContentLen
        {
            get;
            set;
        }

        /// <summary>
        /// Content
        /// </summary>
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// UniqueFlag--章节唯一标识,可以使用内容进行md5得到
        /// </summary>
        public string UniqueFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 章节采集地址
        /// </summary>
        public string chapterUrl
        {
            get;
            set;
        }
    }
}