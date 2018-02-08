using NoorpodConversation.BaseViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// راهنمای نرم افزار برای ذخیره ی داده های پیشفرض
    /// </summary>
    public static class ApplicationHelper
    {
        /// <summary>
        /// کلاس داده های پیشفرض
        /// </summary>
        public static IApplicationHelper Current { get; set; }
    }
}
