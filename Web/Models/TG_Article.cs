
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
    
public partial class TG_Article
{

    public int ID { get; set; }

    public int iCategoryId { get; set; }

    public int iTemplateId { get; set; }

    public string sTitle { get; set; }

    public string sEnTitle { get; set; }

    public string sAuthor { get; set; }

    public string sSize { get; set; }

    public string sLang { get; set; }

    public string sVersion { get; set; }

    public string sSummary { get; set; }

    public string sPictureSize { get; set; }

    public string sPictureUrl { get; set; }

    public Nullable<bool> bIsTop { get; set; }

    public Nullable<bool> bIsSlide { get; set; }

    public Nullable<bool> bIsHot { get; set; }

    public Nullable<System.DateTime> dPublishTime { get; set; }

    public string sDescription { get; set; }

    public string sKeyword { get; set; }

    public string sContent { get; set; }

    public bool bIsDeleted { get; set; }

    public Nullable<System.DateTime> dInsertTime { get; set; }

    public bool bIsRelease { get; set; }

    public string sFileName { get; set; }

    public int iCount { get; set; }

}

}
