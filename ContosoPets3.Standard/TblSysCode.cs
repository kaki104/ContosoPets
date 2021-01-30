using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoPets3.Standard
{
    [Table("tbl_SYS_CODE")]
    public partial class TblSysCode
    {
        [Key]
        [StringLength(36)]
        public string CommonCodeId { get; set; }
        [StringLength(36)]
        public string CommonCodeCategoryId { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        public short Order { get; set; }
        [Required]
        [StringLength(30)]
        public string Code { get; set; }
        [Required]
        [StringLength(30)]
        public string Value1 { get; set; }
        [StringLength(30)]
        public string Value2 { get; set; }
        [StringLength(30)]
        public string Value3 { get; set; }
        [StringLength(100)]
        public string Desc { get; set; }
        [Required]
        public bool? IsUse { get; set; }
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
