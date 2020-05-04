using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Enterprise.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Key]
        public int TypeId { get; set; }

        [Required]
        [Range(1.0, 1000.0,ErrorMessage = "cannot sell an items at a quantity more than 1000")]
        public int Quantity { get; set; }

        [Required]
        [MinLength(2,ErrorMessage = "text too short")]
        public string Quality { get; set; }

        [Required]
        [Range(1.0, 1000000.0, ErrorMessage = "the price of an item must be between 1 and 1000000")]
        public decimal Price { get; set; }

        [Key]
        public int UserId { get; set; }

    }
}