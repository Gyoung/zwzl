﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBUitl
{
    public class Banner
    {
        public string Id { get; set; }

        public string ImgPath { get; set; }

        public string Title { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ModifyTime { get; set; }

    }
}