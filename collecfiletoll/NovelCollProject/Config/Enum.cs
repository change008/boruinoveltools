namespace NovelCollProject
{
    public enum ArticleType : short
    {
        Forum = 1,
        News = 2,
        Video = 3,
        SimilarityDelete = -2,
        Pic = 6
    }

    enum PageTypeEnum : short
    {
        NONE,
        StartupPage,
        ListPage1,
        ListPage2,
        ListPage3,
        DetailPage1,
        DetailPage2,
        DetailPage3,
    }

    enum ResponseStatusEnum : short
    {
        Success = 0,
        TextNumberShortByServer = -1,
        RepeatContent = -2,
        NetError = -10,
        TextNumberShortByClient = -11,
        ServerError = -12
    }

    class CollectionFieldName
    {
        public const string Novel_UniqueFlag = "Novel_UniqueFlag";
        public const string Novel_Name = "Novel_Name";
        public const string Novel_Intr = "Novel_Intr";
        public const string Novel_PublisherId = "Novel_PublisherId";
        public const string Novel_PublisherName = "Novel_PublisherName";
        public const string Novel_CoverImgs = "Novel_CoverImgs";
        public const string Novel_Tag = "Novel_Tag";
        public const string Novel_Mark = "Novel_Mark";
        public const string Novel_Sort = "Novel_Sort";
        public const string Novel_ViewCount = "Novel_ViewCount";
        public const string Novel_CommentCount = "Novel_CommentCount";
        public const string Novel_DingCount = "Novel_DingCount";
        public const string Novel_CaiCount = "Novel_CaiCount";
        public const string Novel_ShareCount = "Novel_ShareCount";
        public const string Novel_ContentLen = "Novel_ContentLen";
        public const string Novel_CreateTime = "Novel_CreateTime";
        public const string Novel_UpdateTime = "Novel_UpdateTime";
        public const string Novel_Status = "Novel_Status";

        public const string Chap_Intro = "Chap_Intro";
        public const string Chap_SortOrder = "Chap_SortOrder";
        public const string Chap_Title = "Chap_Title";
        public const string Chap_ChapterType = "Chap_ChapterType";
        public const string Chap_Pirce = "Chap_Pirce";
        public const string Chap_Remark = "Chap_Remark";
        public const string Chap_Status = "Chap_Status";
        public const string Chap_Content = "Chap_Content";
        public const string Chap_UniqueFlag = "Chap_UniqueFlag";
        public const string Chap_ContentLen = "Chap_ContentLen";

        public const string Url = "url";
        public const string CommentUrls = "commenturls";
        public const string CommentsUrl = "commentsurl";
        public const string Comments = "comments";
        public const string NextUrl = "nexturl";
        public const string ExContent = "excontent";
        public const string BookInfo = "BookInfo";
        public const string Items = "items";
        public const string Pages = "pages";
        public const string MultiPages = "multipages";
    }

    /// <summary>
    /// 小说状态
    /// </summary>
    public enum BookStatus : int
    {
        BookStatus_Offline = 0,
        BookStatus_Update = 1,
        BookStatus_Finish = 2,
        BookStatus_Delete = 3,
    }

    /// <summary>
    /// 小说标识
    /// </summary>
    public enum BookMark : int
    {
        BookMark_Normal = 0,
        BookMark_Recomment = 1,
        BookMark_Hot = 2,
        BookMark_Essence = 3,
    }

    /// <summary>
    /// 章节类型
    /// </summary>
    public enum ChapterType : int
    {
        ChapterType_Free = 0,
        ChapterType_Pay = 1,
    }


    /// <summary>
    /// 章节状态
    /// </summary>
    public enum ChapterStatus : int
    {
        ChapterStatus_Offline = 0,
        ChapterStatus_OnLine = 1,
        ChapterStatus_Delete = 2,
    }
}
