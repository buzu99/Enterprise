using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Enterprise.Models
{
    public class ItemType
    {

        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key, Column(Order = 1)]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        [MinLength(2, ErrorMessage ="Your type name is short")]
        public string Name { get; set; }

        //[Required]
        [MinLength(2, ErrorMessage ="image url cannot be that short")]
        public string Image { get; set; }


        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }



    }
}