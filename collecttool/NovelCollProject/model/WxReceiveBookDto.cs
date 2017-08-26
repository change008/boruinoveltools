using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelCollProject.model
{
    /// <summary>
    /// 请求图书状态返回的数据结构
    /// </summary>
    public class WxReceiveBookDto
    {
        public int collectionid { get; set; }

        public int id { get; set; }

        public string name { get; set; }

        public string maxchapterid { get; set; }

        public int status { get; set; }

    }
}
