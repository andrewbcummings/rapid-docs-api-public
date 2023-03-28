namespace rapid_docs_models.DbModels
{
    public class SurveyInputOption : BaseModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
