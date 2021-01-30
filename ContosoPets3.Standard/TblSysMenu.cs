using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoPets3.Standard
{
    [Table("tbl_SYS_MENU")]
    public partial class TblSysMenu
    {
        [Key]
        [Column("MenuID")]
        [StringLength(36)]
        public string MenuId { get; set; }
        public long Seq { get; set; }
        [StringLength(3)]
        public string Company { get; set; }
        [StringLength(1)]
        public string Factory { get; set; }
        [StringLength(1)]
        public string MenuLevel { get; set; }
        [StringLength(50)]
        public string MenuName { get; set; }
        [StringLength(10)]
        public string SystemClass { get; set; }
        [Column("ParentMenuID")]
        [StringLength(36)]
        public string ParentMenuId { get; set; }
        public short? MenuSeq { get; set; }
        [Column("ImgURL")]
        [StringLength(255)]
        public string ImgUrl { get; set; }
        [StringLength(30)]
        public string ButtonSize { get; set; }
        [Column("ViewID")]
        [StringLength(36)]
        public string ViewId { get; set; }
        [Column("SubURL")]
        [StringLength(255)]
        public string SubUrl { get; set; }
        [Column("HelpURL")]
        [StringLength(255)]
        public string HelpUrl { get; set; }
        public bool IsUse { get; set; }
        [StringLength(10)]
        public string UseClass { get; set; }
        public bool IsMobileUse { get; set; }
        public bool IsPopup { get; set; }
        [StringLength(36)]
        public string SysUserUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [StringLength(30)]
        public string CreateUser { get; set; }
        [StringLength(30)]
        public string CreateView { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastModifyDate { get; set; }
        [StringLength(30)]
        public string LastModifyUser { get; set; }
        [StringLength(30)]
        public string LastModifyView { get; set; }

        [ForeignKey(nameof(SysUserUserId))]
        [InverseProperty(nameof(TblSysUser.TblSysMenu))]
        public virtual TblSysUser SysUserUser { get; set; }
        [ForeignKey(nameof(ViewId))]
        [InverseProperty(nameof(TblSysView.TblSysMenu))]
        public virtual TblSysView View { get; set; }
    }
}
