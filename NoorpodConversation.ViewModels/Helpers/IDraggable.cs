using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoorpodConversation.ViewModels.Helpers
{
    public interface IDraggable
    {
        void BeginDrag();
        void EndDrag(bool isFromWindow);
    }
}
