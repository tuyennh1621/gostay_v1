﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Data.Enums
{
    public enum SuggestTourType
    {
        [Description("Quận mà tour xuất phát")]
        QuanFrom = 1,
        [Description("Quận mà tour đến")]
        QuanTo = 2,
        [Description("Tỉnh mà tour xuất phát")]
        TinhFrom = 3,
        [Description("Tỉnh mà tour đến")]
        TinhTo = 4,
        [Description("Tour name")]
        TourName = 5,
    }
}

