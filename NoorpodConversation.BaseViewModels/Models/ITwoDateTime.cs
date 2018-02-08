using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoorpodConversation.BaseViewModels.Models
{
    public interface ITwoDateTime
    {
        ITwoDateTime TwoDateTime { get; set; }
        DateTime StartDatetime { get; set; }
        DateTime EndDatetime { get; set; }
    }
}
