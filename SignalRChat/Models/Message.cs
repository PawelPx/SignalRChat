using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SignalRChat.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Text { get; set; }

        [DataType(DataType.Date)]
        public DateTime SendDate { get; set; }
    }
}
