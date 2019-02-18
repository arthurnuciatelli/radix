using System;
using System.ComponentModel.DataAnnotations;

namespace Radix.Models
{
    public class Evento
    {
        public long Id { get; set; }        
        public long Timestamp { get; set; }
        public string Tag { get; set; }
        public string Valor { get; set; }
        public string Status { get; set; }
    }
}
