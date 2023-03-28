using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_models.DbModels
{
    public class InputField : BaseModel
    {
        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Label { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(250)]
        public string Value { get; set; }

        [StringLength(250)]
        public string DefaultValue { get; set; }

        public bool Required { get; set; }

        public IEnumerable<InputOption>? Options { get; set; }

        public int Order { get; set; }

        public Guid Id { get; set; }
    }
}
