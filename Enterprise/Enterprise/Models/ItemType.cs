using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Enterprise.Models
{
    public class ItemType
    {

        [Key]
        public int Id { get; set; }

        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage ="Your type name is short")]
        public string name { get; set; }

        [Required]
        [MinLength(2, ErrorMessage ="image url cannot be that short")]
        public string image { get; set; }

    }
}