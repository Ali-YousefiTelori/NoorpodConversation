using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoorpodConversation.ViewModels.Helpers
{
    public static class DragAndDropReleaseHelper
    {
        static List<IDraggable> DraggingControls = new List<IDraggable>();

        public static void EndAllDraggingControls()
        {
            foreach (var item in DraggingControls.ToArray())
            {
                item.EndDrag(true);
            }
        }

        public static void AddToBeggins(IDraggable draggable)
        {
            if (DraggingControls.Contains(draggable))
                return;
            DraggingControls.Add(draggable);
        }

        public static void RemoveFromBeggins(IDraggable draggable)
        {
            DraggingControls.Remove(draggable);
        }
    }
}
