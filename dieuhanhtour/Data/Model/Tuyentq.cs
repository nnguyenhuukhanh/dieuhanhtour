using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dieuhanhtour.Data.Model
{
    public class Tuyentq
    {
        [Key]
        public string   Code { get; set; }
        [Column(TypeName = "nvarchar(120)")]
        //[Remote("TuyentqExists", "Tuyentq", ErrorMessage = "Tuyến tq đã tồn tại")]
        public string Tuyen { get; set; }
        public string Tentuyen { get; set; }
    }
}
