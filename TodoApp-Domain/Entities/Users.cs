namespace Todo_App.Domain.Entities
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tbl_Users")]
    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            Todo = new HashSet<Todo>();
            Todo1 = new HashSet<Todo>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

        [StringLength(150)]
        public string Email { get; set; }

        [StringLength(250)]
        [JsonIgnore]
        public string Hash { get; set; }

        [JsonIgnore]
        public string Token { get; set; }

        [StringLength(150)]
        [JsonIgnore]
        public string Salt { get; set; }

        [JsonIgnore]
        public virtual ICollection<Todo> Todo { get; set; }

        [JsonIgnore]
        public virtual ICollection<Todo> Todo1 { get; set; }

    }
}
