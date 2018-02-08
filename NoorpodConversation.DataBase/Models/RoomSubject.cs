using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace NoorpodConversation.DataBase.Models
{
    public class RoomSubject
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Subject { get; set; }
    }
}
