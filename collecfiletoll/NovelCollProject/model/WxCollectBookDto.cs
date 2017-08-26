using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelCollProject.model
{
    [Serializable()]
    public class WxCollectBookDto
    {
        public string name { get; set; }

        public string intr { get; set; }

        public string coverimgs { get; set; }

        public string tag { get; set; }

        public string uniqueflag { get; set; }

        public int collectionid { get; set; }

        public int contentlen { get; set; }


        public static WxCollectBookDto fromCollectionModel(CollectionModel model)
        {
            WxCollectBookDto dto = new WxCollectBookDto();
            dto.name = model.Name;
            dto.intr = model.Intr;
            dto.coverimgs = model.CoverImgs;
            dto.tag = "";
            dto.uniqueflag = model.UniqueFlag;
            dto.collectionid = model.CollectionId;
            dto.contentlen = model.ContentLen;
            return dto;
        }

    }
}
