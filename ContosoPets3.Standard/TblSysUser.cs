using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoPets3.Standard
{
    [Table("tbl_SYS_USER")]
    public partial class TblSysUser
    {
        [Key]
        [Column("UserID")]
        [StringLength(36)]
        public string UserId { get; set; }
        [Required]
        [StringLength(10)]
        public string UserType { get; set; }
        [Required]
        [StringLength(10)]
        public string AuthType { get; set; }
        [Required]
        [StringLength(10)]
        public string AccessType { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(3)]
        public string Company { get; set; }
        [StringLength(1)]
        public string Factory { get; set; }
        [StringLength(30)]
        public string LanguageType { get; set; }
        [Column("ImgURL")]
        [StringLength(255)]
        public string ImgUrl { get; set; }
        [Required]
        [Column("AuthGroupID")]
        [StringLength(30)]
        public string AuthGroupId { get; set; }
        [StringLength(10)]
        public string Division { get; set; }
        [StringLength(10)]
        public string Team { get; set; }
        [Required]
        [StringLength(10)]
        public string WorkStateType { get; set; }
        [Column("LockTimeUI")]
        public int LockTimeUi { get; set; }
        [Column("LockTimeOI")]
        public int LockTimeOi { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastChangedPasswordDate { get; set; }
        public string DashboardTemplate { get; set; }
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
    }
}
