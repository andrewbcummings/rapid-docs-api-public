namespace rapid_docs_viewmodels.ViewModels
{
    public class BaseTemplateFormVM : BaseVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<BaseFormPageVM> FormPages { get; set; }
    }

    public class BaseFormPageVM
    {
        public int Id { get; set; }
        public IEnumerable<InputFieldVM>? InputFields { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PageOrder { get; set; }
    }
}
