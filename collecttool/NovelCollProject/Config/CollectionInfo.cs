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
        public static List<CollectionModel> CollctionModelList = new List<CollectionModel>();
        static CollectionInfo()
        {
            //采集信息添加到采集列表中
            CollctionModelList.Add(model_1002);
            CollctionModelList.Add(model_1003);
            CollctionModelList.Add(model_1004);
            CollctionModelList.Add(model_1005);
            CollctionModelList.Add(model_1006);
            CollctionModelList.Add(model_1007);
            CollctionModelList.Add(model_1008);
            CollctionModelList.Add(model_1009);
            CollctionModelList.Add(model_1010);
            CollctionModelList.Add(model_1011);
            CollctionModelList.Add(model_1012);
            CollctionModelList.Add(model_1013);
            CollctionModelList.Add(model_1014);
            CollctionModelList.Add(model_1015);
            CollctionModelList.Add(model_1016);
            CollctionModelList.Add(model_1017);
            CollctionModelList.Add(model_1018);
            CollctionModelList.Add(model_1019);
            CollctionModelList.Add(model_1020);
            CollctionModelList.Add(model_1021);
            CollctionModelList.Add(model_1022);
            CollctionModelList.Add(model_1023);
            CollctionModelList.Add(model_1024);

            CollctionModelList.Add(model_1025);
            CollctionModelList.Add(model_1026);
            CollctionModelList.Add(model_1031);
            CollctionModelList.Add(model_1032);
            CollctionModelList.Add(model_1034);
            CollctionModelList.Add(model_1035);

            CollctionModelList.Add(model_1036);
            CollctionModelList.Add(model_1037);
            CollctionModelList.Add(model_1038);
            CollctionModelList.Add(model_1039);
            CollctionModelList.Add(model_1040);
            CollctionModelList.Add(model_1041);
            CollctionModelList.Add(model_1042);
            CollctionModelList.Add(model_1043);


            CollctionModelList.Add(model_1046);
            CollctionModelList.Add(model_1047);


            CollctionModelList.Add(model_1048);
            CollctionModelList.Add(model_1049);

            //CollctionModelList.Add(model_1060);
            CollctionModelList.Add(model_1061);
            CollctionModelList.Add(model_1062);
            CollctionModelList.Add(model_1063);
            CollctionModelList.Add(model_1064);

            CollctionModelList.Add(model_1066);
            CollctionModelList.Add(model_1067);
            CollctionModelList.Add(model_1068);
            CollctionModelList.Add(model_1069);
        }

        #region 采集小说配置
        /// <summary>
        /// 春野小村医_1002 连载中 
        /// </summary>
        public static CollectionModel model_1002 = new CollectionModel(
            1,
            "web_hkslg520",
            "http://www.hkslg520.com/167/167695/?chapterlist",
            1002,
            "春野小村医",
            @"回乡小农民无意得神农咒语，开始了自己强悍的人生。青山绿水，他要打造一片人间乐土。钱要有，美女更要有！村花校花、人妻少妇……等等，我今晚该在哪个房间睡？",
            "http://m.hkslg520.com/modules/article/images/nocover.jpg",
            "chunyexiaocunyi",
            "nothing");

        //public static CollectionModel model_1002 = new CollectionModel(
        //    1,
        //    "web_biquge5200",
        //    "http://www.biquge5200.com/68_68723/?chapterlist",
        //    1002,
        //    "春野小村医",
        //    @"回乡小农民无意得神农咒语，开始了自己强悍的人生。青山绿水，他要打造一片人间乐土。钱要有，美女更要有！村花校花、人妻少妇……等等，我今晚该在哪个房间睡？",
        //    "http://m.hkslg520.com/modules/article/images/nocover.jpg",
        //    "chunyexiaocunyi",
        //    "nothing");


        /// <summary>
        /// 透视小村医_1003 --已完结
        /// </summary>
        public static CollectionModel model_1003 = new CollectionModel(
            2,
            "web_ziyouge",
            "http://www.ziyouge.com/zy/12/12122/index.html?chapterlist",
            1003,
            "透视小村医",
            @"穷屌丝小农民，凭着神针入体，拥有透视功能。 从而，种田，看病，经商雄霸一方。 坐拥花丛，脚踢枭雄。 激情热血，美艳盈怀。 透视小村医，带你装比带你飞。",
            "http://t.ziyouge.com/12/12122/12122s.jpg",
            "toushixiaocunyi",
            "nothing");

        /// <summary>
        /// 美艳冥妻_1004 --已完结
        /// </summary>
        public static CollectionModel model_1004 = new CollectionModel(
            2,
            "web_yikanxiaoshuo",
            "http://www.yikanxiaoshuo.com/yikan/337699/",
            1004,
            "美艳冥妻",
            @"堂哥结婚，新娘子很漂亮，我和几个堂兄弟闹洞房的时候头脑发热做出了荒唐事，以至于喜事变成了丧事……",
            "http://img.yikanxiaoshuo.com/337/337699/337699s.jpg",
            "meiyanmingqi",
            "nothing");

        /// <summary>
        /// 小村那些事_1005 --连载中 //"http://www.wxguan.com/wenzhang/75/75123/?chapterlist",
        /// </summary>
        public static CollectionModel model_1005 = new CollectionModel(
            1,
            "web_lwxs",
            "http://www.lwxs.com/shu/41/41439/?chapterlist",  
            1005,
            "小村那些事",
            @"自从村子里出了个杨小宝，乡村生活开始丰富了起来。比如帮寡妇马老师家里打打旱井。帮美女丽丽赶跑追求她的流氓。帮隔壁雪梅婶婶治愈多年不育的顽疾。帮村里修通了通往镇上的大路。乡亲们，姐妹们，我杨小宝来了！",
            "http://danhuang.zhanlve5.com/static/image/s_cover.jpg",
            "xiaocunnaxieshi",
            "nothing");

        /// <summary>
        /// 妇科小村医_1006 --已完结
        /// </summary>
        public static CollectionModel model_1006 = new CollectionModel(
            2,
            "web_quanshuwu",
            "http://www.quanshuwu.com/book/2714.aspx?chapterlist",
            1006,
            "妇科小村医",
            @"游手好闲的牛大壮意外学到了很多巫术风水等知识，从此，走上了一条风流快活的人生之路。",
            "http://danhuang.zhanlve5.com/static/image/s_cover.jpg",
            "fukexiaocunyi",
            "nothing",
            true);



        /// <summary>
        /// 神棍小村医_1007 --未完结
        /// </summary>
        public static CollectionModel model_1007 = new CollectionModel(
            1,
            "web_ziyouge",
            "http://www.ziyouge.com/zy/13/13066/index.html?chapterlist",
            1007,
            "神棍小村医",
            @"山村少年方小宇，因砸破奇石获异能，从此精通医术、风水、看相和鉴宝。 他点中了事业桃花双旺的风水宝地。开塘办厂建电站，带领乡亲奔小康。护士警花女教师，村花校花留守妇，模特明星女老总，桃花运来挡不住……",
            "http://danhuang.zhanlve5.com/static/image/s_cover.jpg",
            "shengunxiaocunyi",
            "nothing");

        /// <summary>
        /// 我把自己卖给了富婆_1008 --未完结
        /// </summary>
        public static CollectionModel model_1008 = new CollectionModel(
            1,
            "web_lingyu",
            "http://www.lingyu.org/wjsw/26/26325/?chapterlist",
            1008,
            "我把自己卖给了富婆",
            @"白富美出十万想要找个傻子结婚，我为了还债被迫装傻入赘豪门。本以为婚后可以过上梦幻般的生活，可没想到，自己迎来的不是桃花，而是厄运...",
            "http://danhuang.zhanlve5.com/static/image/s_cover.jpg",
            "wobazijimaigeilefupo",
            "nothing");

        /// <summary>
        /// 假面娇妻_1009 --未完结
        /// </summary>
        public static CollectionModel model_1009 = new CollectionModel(
            1,
            "web_qb520",
            "http://www.qb520.org/xiaoshuo/9/9621/?chapterlist",
            1009,
            "假面娇妻",
            @"结婚纪念日妻子晚归，李泽发现了妻子身上的异常。而当李泽确定妻子不仅在上班期间洗过澡，而且身上还少了某样东西时，李泽意识到妻子可能已经出轨……",
            "http://danhuang.zhanlve5.com/static/image/s_cover.jpg",
            "jiamianjiaoqi",
            "nothing");

        /// <summary>
        /// 神秘殡仪馆的一些事 原名 我在殡仪馆工作的那些事_1010 --未完结
        /// </summary>
        public static CollectionModel model_1010 = new CollectionModel(
            1,
            "web_shumanwu",
            "http://www.shumanwu.net/1/1888/index.html?chapterlist",
            1010,
            "神秘殡仪馆的一些事",
            @"有一天殡仪馆里来了一具身名牌的漂亮女尸，我把这套香奈儿偷了回去，送给了女朋友，接着，一连串的邪门事情发生了……接连的死亡，一步步逼近我跟大胖，我们又该如何逃脱死神的追逐？",
            "http://www.shumanwu.net/files/article/image//1/1888/1888s.jpg",
            "shenmibinyiguan",
            "nothing");

        /// <summary>
        /// 沉沦的女友  原名 堕落女友_1011 --已完结
        /// </summary>
        public static CollectionModel model_1011 = new CollectionModel(
            2,
            "web_qqkanshu",
            "http://www.qqkanshu.com/duoluonvyou/?chapterlist",
            1011,
            "沉沦的女友",
            @"我们的感情历经坎坷与猜忌，在不断成熟起来的年纪，我跟女友的感情是不是能够在这种纸醉金迷的世界中义无反顾的走下去，这不是我与女友感情的磨难，更是对现实与世界的抗争。",
            "http://www.qqkanshu//uploads/temp/wd/duoluonvyou.png",
            "chenlunnvyou",
            "nothing");

        /// <summary>
        /// 今晚有料_1012 --连载中, 原今夜有戏
        /// </summary>
        public static CollectionModel model_1012 = new CollectionModel(
            1,
            "web_51cxw",
            "http://www.51cxw.cn/cxw7195/?chapterlist",
            1012,
            "今晚有料",
            @"因为尴尬的事情，我装作聋哑人做了上门女婿。    各位书友要是觉得《今夜有戏》还不错的话请不要忘记向您QQ群和微博里的朋友推荐哦！",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "jinwanyouliao",
            "nothing");

        /// <summary>
        /// 感谢青春_1013 --已完结, 原白姐
        /// </summary>
        public static CollectionModel model_1013 = new CollectionModel(
            2,
            "web_126shu",
            "http://www.126shu.com/65549/?chapterlist",
            1013,
            "感谢青春",
            @"感谢青春，让我在一无所有的年纪，遇到了青春靓丽的你。 ",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "ganxieqingchun",
            "nothing");

        /// <summary>
        /// 多娇山村_1014 --已完结, 
        /// </summary>
        public static CollectionModel model_1014 = new CollectionModel(
            2,
            "web_ybdu",
            "http://www.ybdu.com/xiaoshuo/18/18562/?chapterlist",
            1014,
            "多娇山村",
            @"他是一个小农民，他艳福不浅，美女、熟妇纷至沓来。他外表憨厚，却独具野心，他财运滚滚，赚钱赚到手发软。",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "duojiaoshancun",
            "nothing");
        

        /// <summary>
        /// 血色爱恋 原名 血色浪漫_1015 --已完结, 
        /// </summary>
        public static CollectionModel model_1015 = new CollectionModel(
            2,
            "web_yrnew",
            "http://www.yrnew.net/plugin.php?id=jameson_read&contrl=index&act=book&book_id=66",
            1015,
            "血色爱恋",
            @"十八岁之前，我过着非人的生活，直到尊严被极度践踏的生日夜…… 我努力冲向未来……",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "xueseailian",
            "nothing");

        /// <summary>
        /// 邪魔保镖 原名 妖孽保镖_1016 --已完结, 
        /// </summary>
        public static CollectionModel model_1016 = new CollectionModel(
            2,
            "web_qb5200",
            "http://www.qb5200.org/xiaoshuo/68/68506/?chapterlist",
            1016,
            "邪魔保镖",
            @"这货工作之余揽私活儿，一个人承接多位美女的保护任务；他敢于狂吃窝边草，当然还有窝外的草，以及其他草、所有草。一个妖孽如谜的男人，一群形形色色的女人，演绎了万花丛中的一段段离奇韵事。",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "xiemobaobiao",
            "nothing");

        /// <summary>
        /// 生态园里的爱恋 原名 寡嫂_1017 --连载中, 
        /// </summary>
        public static CollectionModel model_1017 = new CollectionModel(
            1,
            "web_lwtxt",
            "http://www.lwtxt.net/book/48/48138/?chapterlist",
            1017,
            "生态园里的爱恋",
            @"我没有血缘关系的哥不幸死了，为了保护年轻漂亮的嫂子，两年前我因为她差点杀人，逃出山村。 现在回来了，我继续要保护嫂子，跟她一起过人生。但却碰上了生态园的成熟美艳女经理，俏丽的小护士，还有村里的小妖女和生态园的贵妇女客人，让我十八岁的青春很多彩",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "shengtaiyuan",
            "nothing");

        /// <summary>
        /// 同居日记 原名 和小姨子同居的日子_1018--连载中, 
        /// </summary>
        public static CollectionModel model_1018 = new CollectionModel(
            1,
            "web_ouoou",
            "http://www.ouoou.com/ou_26704/?chapterlist",
            1018,
            "同居日记",
            @"周末的早晨我接到一个陌生女孩的来电，电话那头女孩的声音特别软，她自称是我的小姨子，女孩还说让我去机场接她！于是我就开车去了机场，谁料半路竟然下起了暴雨。机场门口我见到了给我打电话的女孩，她穿着湿漉漉的衣服就钻进了我的车里……",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "tongjuriji",
            "nothing",true);

        /// <summary>
        /// 乡村小农民 原名 春野小农民_1019--连载中, 
        /// </summary>
        public static CollectionModel model_1019 = new CollectionModel(
            1,
            "web_yixuanju",
            "http://www.yixuanju.com/book/15752/?chapterlist",
            1019,
            "乡村小农民",
            @"小农民偶得仙家洞府，修炼神功无所不能，透视医术样样都行，立志走上人生巅峰！ 正当刘伟低调发财时，却发现邻家妹妹，隔壁嫂子，火辣白领，娇蛮大小姐，冰山女总裁，双胞胎校花，数不尽的美女纷纷找上门来……",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing", true);

        /// <summary>
        /// 重生官路 原名 官色：攀上女领导_1020--连载中, 
        /// </summary>
        public static CollectionModel model_1020 = new CollectionModel(
            1,
            "web_2wxs",
            "http://www.2wxs.com/xstxt/4515/?chapterlist",
            1020,
            "重生官路",
            @"一个刚被人设计落选的副局长，稀里糊涂的重生到了十几年前。    面对着91年那风云激荡的国际局势，他抓住机遇，利用自己上辈子的记忆和从政经验，从底层科员做起，一步步登上了官场的高峰。    从而打造了一个不可复制的官场神话，真正成为了一个驰骋官场不倒的弄潮儿！",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing", false);

        /// <summary>
        /// 超级小仙医 原名 农民小仙医_1021--连载中, 
        /// </summary>
        public static CollectionModel model_1021 = new CollectionModel(
            1,
            "web_akxs6",
            "http://www.akxs6.com/8/8129/?chapterlist",
            1021,
            "超级小仙医",
            @"……",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing", false);

        /// <summary>
        /// 男公关回忆录 原名 我做男公关的那几年_1022--完结, 
        /// </summary>
        public static CollectionModel model_1022 = new CollectionModel(
            2,
            "web_365if",
            "http://www.365if.com/list/97/97481.html?chapterlist",
            1022,
            "男公关回忆录",
            @"本是富二代，却天降大祸，带走父亲，比我仅大七岁的后妈带走了所有遗产，我刷爆所有信用卡，为了还账，当上了公关，从此多少年，多少年……",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing", false);

        /// <summary>
        /// 乡村美事 原名 乡村大凶器_1023--连载中, 
        /// </summary>
        public static CollectionModel model_1023 = new CollectionModel(
            1,
            "web_yidudu",
            "http://www.yidudu.org/xiangcundaxiongqi/chapter.html?chapterlist",
            1023,
            "乡村美事",
            @"",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing", false);

        
       
        /// <summary>
        /// 那些公司里的事 原名 色即是空_1024--连载中, 
        /// </summary>
        public static CollectionModel model_1024 = new CollectionModel(
            1,
            "web_lwtxt",
            "http://www.lwtxt.net/book/50/50496/?chapterlist",
            1024,
            "那些公司里的事",
            @"那些年在公司里的女人",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 美女村长的超级保镖 原名 女村长的贴身保镖_1025--完结, 
        /// </summary>
        public static CollectionModel model_1025 = new CollectionModel(
            2,
            "web_luoqiu",
            "http://www.luoqiu.com/read/44671/?chapterlist",
            1025,
            "美女村长的超级保镖",
            @"美女村长拥有一个贴身的保镖...",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 护花神龙 原名 护花狂龙_1026--完结, 
        /// </summary>
        public static CollectionModel model_1026 = new CollectionModel(
            2,
            "web_feizw",
            "http://www.feizw.com/Html/4396/index.html?chapterlist",
            1026,
            "护花神龙",
            @"护花神龙...",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 草根的逆袭 原名 屌丝崛起_1031--完结, 
        /// </summary>
        public static CollectionModel model_1031= new CollectionModel(
            2,
            "web_ziyouge",
            "http://www.ziyouge.com/zy/10/10727/index.html?chapterlist",
            1031,
            "草根的逆袭",
            @"已婚主妇的秘密...",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");


        /// <summary>
        /// 村里的神医 原名 乡村小神医_1032--连载, 
        /// </summary>
        public static CollectionModel model_1032 = new CollectionModel(
            1,
            "web_ziyouge",
            "http://www.ziyouge.com/zy/10/10582/index.html?chapterlist",
            1032,
            "村里的神医",
            @"山沟里的穷小子偶得玄妙的长生造化诀，种田、治病、风水、占卜……样样都行，某日修炼小成的穷小子随手占卜了一卦，卦象显示：否极泰来，桃花泛滥！于是……",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 超级高手 原名 超凡高手_1034--连载, 
        /// </summary>
        public static CollectionModel model_1034 = new CollectionModel(
            1,
            "web_ziyouge",
            "http://www.ziyouge.com/zy/7/7675/index.html?chapterlist",
            1034,
            "村里的神医",
            @"佣兵界兵王，为寻找失散多年的妹妹，回归都市，偶遇美女经理。 文能贴身护美女，武能提枪安吾妻。 一代兵王掀起都市新一轮的腥风血雨！……",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 绝色乡村 原名 艳绝乡村_1035--连载, 
        /// </summary>
        public static CollectionModel model_1035 = new CollectionModel(
            1,
            "web_xxbiquge",
            "http://www.xxbiquge.com/2_2226/?chapterlist",
            1035,
            "绝色乡村",
            @"绝色乡村……",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing",true);



        /// <summary>
        /// 乡村官道 原名 官梯_1036--连载, 
        /// </summary>
        public static CollectionModel model_1036 = new CollectionModel(
            1,
            "web_jingcaiyuedu",
            "http://www.jingcaiyuedu.com/book/346075.html?chapterlist",
            1036,
            "乡村官道",
            @"道，可以是一条路，也可以是一条法则；正道，那就是正确的道路或者是正确的法则；为人者，走错了道可以改回来，大不了从头再来；为官者，踏出一步，就是一个脚印，对了，那是份内之事，错了，前面就是万丈深渊；天若有情天亦老，人间正道是沧桑。",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing", true);

        /// <summary>
        /// 潇洒兵神 原名 逍遥兵王_1037--连载, 
        /// </summary>
        public static CollectionModel model_1037 = new CollectionModel(
            1,
            "web_126shu",
            "http://www.126shu.com/14207/?chapterlist",
            1037,
            "潇洒兵神",
            @"为兄弟，他两肋插刀，为女人，他怒为红颜，为国家，他誓死如归",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 冥界新娘 原名 来自阴间的新娘_1038--连载, 
        /// </summary>
        public static CollectionModel model_1038 = new CollectionModel(
            1,
            "web_yznn",
            "http://www.yznn.com/files/article/html/37/37002/index.html?chapterlist",
            1038,
            "冥界新娘",
            @"...",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 上门女婿的一些事 原名 我当上门女婿的那些年_1039--连载, 
        /// </summary>
        public static CollectionModel model_1039 = new CollectionModel(
            1,
            "web_258zw",
            "http://www.258zw.com/html/155657/?chapterlist",
            1039,
            "上门女婿的一些事",
            @"为了钱我做了上门女婿，可是洞房花烛夜，压在新娘身上的男人，却不是我！ 而后，我发现了新娘身上的惊人的秘密……",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 恐怖游戏 原名 绝望游戏_1040--连载, 
        /// </summary>
        public static CollectionModel model_1040 = new CollectionModel(
            1,
            "web_yikanxiaoshuo",
            "http://www.yikanxiaoshuo.com/yikan/388433/",
            1040,
            "恐怖游戏",
            @"微信群里有土豪发红包，要求运气王跟它玩个游戏，竟然是让班花做……",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");


        /// <summary>
        /// 村里的美女老师 原名 乡村女教师_1041--连载, 
        /// </summary>
        public static CollectionModel model_1041 = new CollectionModel(
            1,
            "web_xyshu8",
            "http://www.xyshu8.net/Html/Book/36/36454/Index.html?chapterlist",
            1041,
            "村里的美女老师",
            @"一方山水养一方人，桃水村虽然偏僻，但这里的女人个个都白嫩水灵。……",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 女总裁的超级保镖 原名 女总裁的雇佣兵王_1042--连载, 
        /// </summary>
        public static CollectionModel model_1042 = new CollectionModel(
            1,
            "web_guijj",
            "http://www.guijj.com/mulu/8746.html?chapterlist",
            1042,
            "女总裁的超级保镖",
            @"...",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing",true);
        #endregion

        /// <summary>
        /// 乡村里的一些事 原名 乡村那些事儿_1043--连载, 
        /// </summary>
        public static CollectionModel model_1043 = new CollectionModel(
            1,
            "web_ziyouge",
            "http://www.ziyouge.com/zy/13/13079/index.html?chapterlist",
            1043,
            "乡村里的一些事",
            @"赵铁柱大专毕业回村里，一个个大媳妇小姑娘用各种理由缠上来，让他欲罢不能···",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 美女老板的贴身兵王 原名 女总裁的贴身兵王_1046--连载, 
        /// </summary>
        public static CollectionModel model_1046 = new CollectionModel(
            1,
            "web_freexs",
            "http://www.freexs.org/novel/113/113211/index.html?chapterlist",
            1046,
            "美女老板的贴身兵王",
            @"...",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 妻子的诱惑 原名 倾城娇妻_1047--完结, 
        /// </summary>
        public static CollectionModel model_1047 = new CollectionModel(
            2,
            "web_shoujikanshu",
            "http://www.shoujikanshu.org/xiaoshuo/index_18594.html?chapterlist",
            1047,
            "妻子的诱惑",
            @"...",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");


        /// <summary>
        /// 超级天眼 原名 透视之眼_1048--完结, 
        /// </summary>
        public static CollectionModel model_1048 = new CollectionModel(
            2,
            "web_zwdu",
            "http://www.zwdu.com/book/10743/?chapterlist",
            1048,
            "超级天眼",
            @"...",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 重活一世 原名 修真万万年_1049--连载, 
        /// </summary>
        public static CollectionModel model_1049 = new CollectionModel(
            1,
            "web_luoqiu",
            "http://www.luoqiu.com/read/305269/?chapterlist",
            1049,
            "重活一世",
            @"伤我至亲之人，我一定十倍，百倍报之！侮我，辱我，羞我之人，这一世，我一定要将你们踩在脚下！...",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 张狂的青春 原名 第一次亲密接触_1060--完结 -- 无法采集
        /// </summary>
        public static CollectionModel model_1060 = new CollectionModel(
            1,
            "web_bookbl",
            "http://www.bookbl.com/xs/13386.html?chapterlist",
            1060,
            "张狂的青春",
            @"...",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        


        /// <summary>
        /// 桃花仙医 原名 乡村小仙医_1061--连载, 
        /// </summary>
        public static CollectionModel model_1061 = new CollectionModel(
            1,
            "web_ziyouge",
            "http://www.ziyouge.com/zy/12/12117/index.html?chapterlist",
            1061,
            "桃花仙医",
            @"一不小心秦凡看到村长媳妇偷情，机缘巧合之下获得异能，从此走上发家致富路，各路美女追来，桃花运不断，众美环绕……",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 权利的秘密 原名 权路迷局_1062--连载, 
        /// </summary>
        public static CollectionModel model_1062 = new CollectionModel(
            1,
            "web_52bqg",
            "http://www.52bqg.com/book_27115/?chapterlist",
            1062,
            "权利的秘密",
            @"在基层混迹多年毫无晋升希望的梁健，得到区委女书记的欣赏，从乡镇干部到省级干部的跋涉攀升。从中你能看到现代官场的风尚画面！...",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 我的青春不归路 原名 青春禁岛_1063--连载, 
        /// </summary>
        public static CollectionModel model_1063 = new CollectionModel(
            1,
            "web_ziyouge",
            "http://www.ziyouge.com/zy/11/11344/index.html?chapterlist",
            1063,
            "我的青春不归路",
            @"我先有继父，欺负姐姐；后有继母，欺负我，让我踏上一条青春不归路……",
            "http://danhuang.zhanlve5.com/static/image/mycover_b.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 临时男友 原名 美女总裁的贴身高手_1064--完结, 
        /// </summary>
        public static CollectionModel model_1064 = new CollectionModel(
            2,
            "web_feizw",
            "http://www.feizw.com/Html/11431/Index.html?chapterlist",
            1064,
            "临时男友",
            @"外籍军团最出色的佣兵高飞回到故乡，偶然机会被美女总裁聘为临时男友，从此获得幸运女神的青睐……",
            "http://danhuang.zhanlve5.com/static/image/mycover_b.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 锦绣色香 原名 秀色生香_1066--连载, 
        /// </summary>
        public static CollectionModel model_1066 = new CollectionModel(
            1,
            "web_d9cn",
            "http://www.d9cn.net/html/62/62132/?chapterlist",
            1066,
            "锦绣色香",
            @"...",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 女老板的贴身神兵 原名 霸道女总裁的贴身保镖_1067--连载, 
        /// </summary>
        public static CollectionModel model_1067 = new CollectionModel(
            1,
            "web_52bqg",
            "http://www.52bqg.com/book_86198/?chapterlist",
            1067,
            "女老板的贴身神兵",
            @"兵王回归都市，意外被一个美女劫色。随后去见从小定下娃娃亲的未婚妻，却发现劫色的美女是未婚妻的表妹。我擦！这该如何相处？...",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        /// <summary>
        /// 协议恋人 原名 契约恋人100天_1068--完结, 
        /// </summary>
        public static CollectionModel model_1068 = new CollectionModel(
            2,
            "web_freexs",
            "http://www.freexs.org/novel/74/74496/index.html?chapterlist",
            1068,
            "协议恋人",
            @"...",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");


        /// <summary>
        /// 代孕成妻 原名 豪门锦绣甜妻_1069--完结, 
        /// </summary>
        public static CollectionModel model_1069 = new CollectionModel(
            2,
            "web_wuruo",
            "http://www.wuruo.com/1/1039/?chapterlist",
            1069,
            "代孕成妻",
            @"...",
            "http://danhuang.zhanlve5.com/static/image/mycover_a.jpg",
            "nothing",
            "nothing");

        
    }


    /*采集配置列表
    -c  1002,1005,1007,1008,1009,1010,1012,1017,1018,1019,1020,1021,1023,1024,1032,1034,1035,1036,1037,1038,1039,1040,1041,1042,1043,1046,1049,1061,1062,1063,1066,1067,1068,1069
        */


    /// <summary>
    /// 采集model
    /// </summary>
    [Serializable()]
    public class CollectionModel
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="collectionId"></param>
        /// <param name="name"></param>
        /// <param name="intr"></param>
        /// <param name="coverImgs"></param>
        /// <param name="uniqueFlag"></param>
        /// <param name="remark"></param>
        public CollectionModel(int status, string plugin, string url, int collectionId, string name, string intr, string coverImgs, string uniqueFlag, string remark,bool isUtf8 = false)
        {
            this.status = status;
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
        /// status 状态0:未上线, 1:连载中, 2:已完结, 
        /// </summary>
        public int status { get; set; }

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
