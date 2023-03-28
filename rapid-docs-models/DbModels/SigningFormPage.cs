using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_models.DbModels
{
    public class SigningFormPage : BaseModel //WizardForm
    {
        public int Id { get; set; }
        public IEnumerable<InputField>? InputFields { get; set; }

        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public int PageOrder { get; set; }
    }
}
