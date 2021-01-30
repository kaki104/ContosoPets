using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoPets3.Standard
{
    [Table("tbl_SYS_VIEW")]
    public partial class TblSysView
    {
        public TblSysView()
        {
            TblSysMenu = new HashSet<TblSysMenu>();
        }

        [Key]
        [Column("ViewID")]
        [StringLength(36)]
        public string ViewId { get; set; }
        [Required]
        [StringLength(10)]
        public string ViewType { get; set; }
        [StringLength(10)]
        public string ViewCategoryType { get; set; }
        [StringLength(10)]
        public string AppType { get; set; }
        [Required]
        [StringLength(50)]
        public string ViewName { get; set; }
        [StringLength(200)]
        public string ClassName { get; set; }
        [StringLength(100)]
        public string Desc { get; set; }
        public bool IsUse { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [StringLength(30)]
        public string CreateUser { get; set; }
        [StringLength(50)]
        public string CreateView { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastModifyDate { get; set; }
        [StringLength(30)]
        public string LastModifyUser { get; set; }
        [StringLength(50)]
        public string LastModifyView { get; set; }

        [InverseProperty("View")]
        public virtual ICollection<TblSysMenu> TblSysMenu { get; set; }
    }
}
