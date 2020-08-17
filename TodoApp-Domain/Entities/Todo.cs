namespace Todo_App.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tbl_TodoList")]
    public partial class Todo
    {
        public int Id { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Desc { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? CreatedBy { get; set; }
        public int Status { get; set; }

        public int? UpdatedBy { get; set; }

        public virtual Users CreatedByUser { get; set; }

        public virtual Users UpdatedByUser { get; set; }
    }
}
