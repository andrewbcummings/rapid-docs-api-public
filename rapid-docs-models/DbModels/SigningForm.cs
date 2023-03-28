﻿using System.ComponentModel.DataAnnotations;

namespace rapid_docs_models.DbModels
{
    public class SigningForm : BaseModel //Wizard
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public IEnumerable<SigningFormPage> SigningFormPages { get; set; }
    }
}
