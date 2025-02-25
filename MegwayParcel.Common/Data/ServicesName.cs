using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegwayParcel.Common.Data
{
	public class ServicesName
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Service_Code { get; set; }
        public bool IsActive { get; set; }=false;
        public int SortOrder { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
    }
}
