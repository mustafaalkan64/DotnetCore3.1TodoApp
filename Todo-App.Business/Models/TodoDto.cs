using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo_App.Core.Enums;

namespace Todo_App.Business.Models
{

    public class TodoDto
    {
        public string Name { get; set; }

        public string Desc { get; set; }
        public int Status { get; set; } = (int)TodoStatusEnum.Todo;

        [JsonIgnore]
        public DateTime? CreateDate { get; set; }

        [JsonIgnore]
        public DateTime? UpdateDate { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }

    }
}
