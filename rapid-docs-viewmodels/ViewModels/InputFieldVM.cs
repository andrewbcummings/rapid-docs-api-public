using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_viewmodels.ViewModels
{
    public class InputFieldVM : BaseVM
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public string Name { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        public string Value { get; set; }

        public string DefaultValue { get; set; }

        public bool Required { get; set; }

        public IEnumerable<InputOptionVM>? Options { get; set; }

        public int Order { get; set; }

        public Guid Id { get; set; }
    }
}
