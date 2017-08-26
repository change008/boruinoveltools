using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelCollProject.model
{
    [Serializable()]
    public class WxCollectChapterDto
    {
        //采集id
        public int collectionid
        {
            get; set;
        }
        //简介
        public string intro
        {
            get; set;
        }
        //章节号
        public int sortorder
        {
            get; set;
        }
        //章节名称
        public string title
        {
            get; set;
        }
        //章节类型
        public int chaptertype
        {
            get; set;
        }
        //章节价格
        public int pirce
        {
            get; set;
        }
        //章节字节		
        public int contentlen
        {
            get; set;
        }

        //存放对方章节唯一标识
        public string uniqueflag
        {
            get; set;
        }

        //内容数据
        public string content
        {
            get; set;
        }



        /// <summary>
        /// 章节目录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static WxCollectChapterDto fromChapter(wxchapter model)
        {
            WxCollectChapterDto dto = new WxCollectChapterDto();
            dto.chaptertype = model.ChapterType;
            dto.collectionid = model.CollectionId;
            dto.content = model.Content;
            dto.contentlen = model.ContentLen;
            dto.intro = model.Intro;
            dto.pirce = model.Pirce;
            dto.sortorder = model.SortOrder;
            dto.title = model.Title;
            dto.uniqueflag = model.Id; //线上的uniqueflag
            return dto;
        }

    }
}
