using AutoMapper;
using rapid_docs_models.DbModels;
using rapid_docs_viewmodels.ViewModels;

namespace rapid_docs_viewmodels
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<User, UserViewModel>(MemberList.None);
            CreateMap<UserViewModel, User>(MemberList.None);

            CreateMap<Signing, SigningVM>(MemberList.None)
                .ForMember(dest => dest.TemplateForm, opt => opt.MapFrom(src => src.SigningForm));
            CreateMap<SigningVM, Signing>(MemberList.None)
                .ForMember(dest => dest.SigningForm, opt => opt.MapFrom(src => src.TemplateForm));
            CreateMap<CreateSigningVM, Signing>(MemberList.None);

            CreateMap<SigningDocument, SigningDocumentVM>(MemberList.None);
            CreateMap<SigningDocumentVM, SigningDocument>(MemberList.None);

            CreateMap<SigningForm, SigningFormVM>(MemberList.None);
            CreateMap<SigningFormVM, SigningForm>(MemberList.None);

            CreateMap<SigningFormPage, SigningFormPageVM>(MemberList.None);
            CreateMap<SigningFormPageVM, SigningFormPage>(MemberList.None);

            CreateMap<SigningFormPage, BaseFormPageVM>(MemberList.None);
            CreateMap<BaseFormPageVM, SigningFormPage>(MemberList.None);

            CreateMap<InputField, InputFieldVM>(MemberList.None);
            CreateMap<InputFieldVM, InputField>(MemberList.None);

            CreateMap<InputOption, InputOptionVM>(MemberList.None);
            CreateMap<InputOptionVM, InputOption>(MemberList.None);

            CreateMap<Thumbnail, ThumbnailVM>(MemberList.None);
            CreateMap<ThumbnailVM, Thumbnail>(MemberList.None);

            CreateMap<Survey, SurveyVM>(MemberList.None)
                .ForMember(dest => dest.TemplateForm, opt => opt.MapFrom(src => src.SurveyForm));
            CreateMap<SurveyVM, Survey>(MemberList.None)
                .ForMember(dest => dest.SurveyForm, opt => opt.MapFrom(src => src.TemplateForm));

            CreateMap<SurveyForm, SurveyFormVM>(MemberList.None);
            CreateMap<SurveyFormVM, SurveyForm>(MemberList.None);

            CreateMap<SurveyFormPage, BaseFormPageVM>(MemberList.None);
            CreateMap<BaseFormPageVM, SurveyFormPage>(MemberList.None);

            CreateMap<SurveyInputField, InputFieldVM>(MemberList.None);
            CreateMap<InputFieldVM, SurveyInputField>(MemberList.None);

            CreateMap<SurveyInputOption, InputOptionVM>(MemberList.None);
            CreateMap<InputOptionVM, SurveyInputOption>(MemberList.None);

            CreateMap<SurveyForm, BaseTemplateFormVM>(MemberList.None)
                .ForMember(dest => dest.FormPages, opt => opt.MapFrom(src => src.SurveyFormPages));
            CreateMap<BaseTemplateFormVM, SurveyForm>(MemberList.None)
                .ForMember(dest => dest.SurveyFormPages, opt => opt.MapFrom(src => src.FormPages));

            CreateMap<SigningForm, BaseTemplateFormVM>(MemberList.None)
                .ForMember(dest => dest.FormPages, opt => opt.MapFrom(src => src.SigningFormPages));
            CreateMap<BaseTemplateFormVM, SigningForm>(MemberList.None)
                .ForMember(dest => dest.SigningFormPages, opt => opt.MapFrom(src => src.FormPages));
        }
    }
}
