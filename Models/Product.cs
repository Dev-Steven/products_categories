using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;   // Add this for [NotMapped]

namespace products_categories.Models
{
    public class Product
    {
        [Key]
        public int ProductId {get;set;}

        [Required]
        public string Name {get;set;}

        [Required]
        public string Description {get;set;}

        public decimal Price {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdateAt {get;set;} = DateTime.Now;

        public List<Association> Groups {get;set;}
    }
}