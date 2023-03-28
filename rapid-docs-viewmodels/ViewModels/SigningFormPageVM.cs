using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_viewmodels.ViewModels
{
    public class SigningFormPageVM : BaseVM //WizardForm
    {
        public int Id { get; set; }
        public IEnumerable<InputFieldVM>? InputFields { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PageOrder { get; set; }
    }
}
