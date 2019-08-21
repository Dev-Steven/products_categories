using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;   // Add this for [NotMapped]

namespace products_categories.Models
{
    public class Association
    {
        [Key]
        public int AssociationId {get;set;}

        public int ProductId {get;set;}

        public int CategoryId {get;set;}

        public Product Product {get;set;}

        public Category Category {get;set;}

    }
}