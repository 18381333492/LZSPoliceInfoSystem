//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TG_Category
    {
        public int ID { get; set; }
        public int CategoryId { get; set; }
        public string sName { get; set; }
        public string sEnName { get; set; }
        public string sEnNick { get; set; }
        public int iOrder { get; set; }
        public Nullable<int> iTemplateId { get; set; }
        public Nullable<int> iArticleTemplateId { get; set; }
        public Nullable<bool> bIsRedirect { get; set; }
        public string sRedirectUrl { get; set; }
        public bool bIsShowNav { get; set; }
        public Nullable<bool> bIsContentCategory { get; set; }
        public string sPictureSize { get; set; }
        public string sPictureUrl { get; set; }
        public string sDescription { get; set; }
        public string sKeyWord { get; set; }
        public string sContent { get; set; }
    }
}
