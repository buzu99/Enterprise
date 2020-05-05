using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Enterprise.Models
{
    public class Item
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity) ]
        public int Id { get; set; }

        [Key,Column(Order = 1)]
        public int TypeId { get; set; }

        public ItemType ItemType { get; set; }

        [Required]
        [Range(1.0, 1000.0,ErrorMessage = "cannot sell an items at a quantity more than 1000")]
        public int Quantity { get; set; }

        [Required]
        [MinLength(2,ErrorMessage = "text too short")]
        public string Quality { get; set; }

        [Required]
        [Range(1.0, 1000000.0, ErrorMessage = "the price of an item must be between 1 and 1000000")]
        public decimal Price { get; set; }

        [Key, Column(Order = 2)]
        public int UserId { get; set; }

    }
}