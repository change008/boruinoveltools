using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovelCollProject.model;

namespace NovelCollProject
{
    /// <summary>
    /// 配置的采集信息,配置采集链接,采集id,采集类型等数据,替换数据库配置数据
    /// </summary>
    public class CollectionInfo
    {
        public static List<string> CollctionModelList = new List<string>();

        public static Dictionary<string, string> collectionSortDic = new Dictionary<string, string>();


        static CollectionInfo()
        {
            //玄幻奇幻
            for (int i = 1; i <= 7; i++)
            {
                collectionSortDic.Add(string.Format("http://www.ziyouge.com/fullflag-1-{0}.html", i), "玄幻奇幻");
            }

            collectionSortDic.Add("http://www.ziyouge.com/fullflag-2-1.html", "武侠仙侠");
             

            for (int i = 1; i <= 10; i++)
            {
                collectionSortDic.Add(string.Format("http://www.ziyouge.com/fullflag-3-{0}.html", i), "都市言情");
            }

            for (int i = 1; i <= 7; i++)
            {
                collectionSortDic.Add(string.Format("http://www.ziyouge.com/fullflag-4-{0}.html", i), "古代言情");
            }

            collectionSortDic.Add("http://www.ziyouge.com/fullflag-5-1.html", "军事历史");

            collectionSortDic.Add("http://www.ziyouge.com/fullflag-6-1.html", "网游竞技");

            for (int i = 1; i <= 10; i++)
            {
                collectionSortDic.Add(string.Format("http://www.ziyouge.com/fullflag-7-{0}.html", i), "恐怖灵异");
            }


            collectionSortDic.Add("http://www.ziyouge.com/fullflag-8-1.html", "耽美同人");


            for (int i = 1; i <= 10; i++)
            {
                collectionSortDic.Add(string.Format(" http://www.ziyouge.com/fullflag-9-{0}.html", i), "穿越架空");
            }

            for (int i = 1; i <= 10; i++)
            {
                collectionSortDic.Add(string.Format("http://www.ziyouge.com/fullflag-10-{0}.html", i), "其他");
            }



            //CollctionModelList.Add("http://www.ziyouge.com/zy/11/11375/index.html"); //极品小村医
            //CollctionModelList.Add("http://www.ziyouge.com/zy/11/11122/index.html"); //乡村小医师
            //CollctionModelList.Add("http://www.ziyouge.com/zy/11/11534/index.html"); //天才小农民
            //CollctionModelList.Add("http://www.ziyouge.com/zy/10/10886/index.html"); //逍遥山村
            //CollctionModelList.Add("http://www.ziyouge.com/zy/11/11019/index.html"); //超能乡村医师
            //CollctionModelList.Add("http://www.ziyouge.com/zy/11/11547/index.html"); //极品小财神
            //CollctionModelList.Add("http://www.ziyouge.com/zy/11/11378/index.html"); //绝品小农民
            //CollctionModelList.Add("http://www.ziyouge.com/zy/10/10722/index.html"); //最强农民
            //CollctionModelList.Add("http://www.ziyouge.com/zy/10/10719/index.html"); //村后有块玉米地
            //CollctionModelList.Add("http://www.ziyouge.com/zy/2/2891/index.html"); //我们村的阴阳两界
            //CollctionModelList.Add("http://www.ziyouge.com/zy/2/2274/index.html"); //桃花村那些事儿
            //CollctionModelList.Add("http://www.ziyouge.com/zy/11/11361/index.html"); //逍遥村医 
            //CollctionModelList.Add("http://www.ziyouge.com/zy/10/10729/index.html"); //绝品村医
            //CollctionModelList.Add("http://www.ziyouge.com/zy/0/41/index.html"); //透视狂医
            //CollctionModelList.Add("http://www.ziyouge.com/zy/2/2814/index.html"); //绝品医仙
            //CollctionModelList.Add("http://www.ziyouge.com/zy/2/2821/index.html"); //妙手圣医
            //CollctionModelList.Add("http://www.ziyouge.com/zy/10/10530/index.html"); //神级大村医
            //CollctionModelList.Add("http://www.ziyouge.com/zy/1/1528/index.html"); //极品邪医
            //CollctionModelList.Add("http://www.ziyouge.com/zy/11/11851/index.html"); //村野小神农
            //CollctionModelList.Add("http://www.ziyouge.com/zy/4/4293/index.html"); //山村野花开
            //CollctionModelList.Add("http://www.ziyouge.com/zy/13/13107/index.html"); //医谷奇医
            //CollctionModelList.Add("http://www.ziyouge.com/zy/13/13082/index.html"); //春野小农民
            //CollctionModelList.Add("http://www.ziyouge.com/zy/11/11548/index.html"); //逍遥小村长
            //CollctionModelList.Add("http://www.ziyouge.com/zy/12/12312/index.html"); //神级小村长
            //CollctionModelList.Add("http://www.ziyouge.com/zy/11/11853/index.html"); //乡野小村长
            //CollctionModelList.Add("http://www.ziyouge.com/zy/11/11133/index.html"); //小村那些事
            //CollctionModelList.Add("http://www.ziyouge.com/zy/10/10721/index.html"); //全能小村医 
            //CollctionModelList.Add("http://www.ziyouge.com/zy/11/11128/index.html"); //超能小村医
            //CollctionModelList.Add("http://www.ziyouge.com/zy/10/10346/index.html"); //逍遥小村医
            //CollctionModelList.Add("http://www.ziyouge.com/zy/10/10372/index.html"); //妙手小村医
        }

    }




    /// <summary>
    /// 采集model
    /// </summary>
    [Serializable()]
    public class CollectionModel
    {
        public CollectionModel()
        {
            this.chapterList = new List<wxchapter>();
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="collectionId"></param>
        /// <param name="name"></param>
        /// <param name="intr"></param>
        /// <param name="coverImgs"></param>
        /// <param name="uniqueFlag"></param>
        /// <param name="remark"></param>
        public CollectionModel(string plugin, string url, int collectionId, string name, string intr, string coverImgs, string uniqueFlag, string remark,bool isUtf8 = false)
        {
            this.Url = url;
            this.CollectionId = collectionId;
            this.Name = name;
            this.Intr = intr;
            this.CoverImgs = coverImgs;
            this.UniqueFlag = uniqueFlag;
            this.Remark = remark;
            this.Plugin = plugin;
            this.chapterList = new List<wxchapter>();
            this.IsUTF8 = isUtf8;
        }
        /// <summary>
        /// url地址--代表章节列表页url地址--章节初始url列表地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 采集组件
        /// </summary>
        public string Plugin { get; set; }

        /// <summary>
        /// 采集id
        /// </summary>
        public int CollectionId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intr { get; set; }

        /// <summary>
        /// 封面图
        /// </summary>
        public string CoverImgs { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string UniqueFlag { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否utf8编码
        /// </summary>
        public bool IsUTF8 { get; set; }

        /// <summary>
        /// 字数
        /// </summary>
        public int ContentLen { get; set; }


        /// <summary>
        /// 章节列表信息
        /// </summary>
        public List<wxchapter> chapterList { get; set; }

        /// <summary>
        /// 根据当前最新章节列表数据&当前线上最新章节id数据获取下一个章节数据,算法就是查找到当前线上最大章节数据然后取其下一个章节的数据信息
        /// </summary>
        /// <param name="chapterList"></param>
        /// <param name="maxchapterid"></param>
        public static wxchapter GetNextChapter(List<wxchapter> chapterList, string maxchapterid)
        {
            if (chapterList == null || chapterList.Count == 0)
            {
                return null;
            }

            if (string.IsNullOrEmpty(maxchapterid))
            {
                return chapterList.First();
            }

            bool isCurrent = false;
            foreach (var item in chapterList)
            {
                if (isCurrent)
                {
                    return item;
                }

                if (String.Equals(item.Id.ToString(), maxchapterid, StringComparison.CurrentCultureIgnoreCase))
                {
                    isCurrent = true;
                }
            }
            return null;
        }
    }
}
