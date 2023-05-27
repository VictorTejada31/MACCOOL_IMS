﻿using Core.Domain.Commons;

namespace Core.Domain.Entities
{
    public class Product : CommonsProperty
    {
        public double SalePrice { get; set; }
        public double PurchasePrice { get; set; }
        public int DefaultProductId { get; set; }

        //Navegation Property

        public DefaultProduct DefaultProduct { get; set; }




    }
}