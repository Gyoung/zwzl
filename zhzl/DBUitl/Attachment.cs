﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBUitl
{
    public class Attachment
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ModifyTime { get; set; }
    }
}
