﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_models.DbModels
{
    public class InputOption : BaseModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

    }
}
