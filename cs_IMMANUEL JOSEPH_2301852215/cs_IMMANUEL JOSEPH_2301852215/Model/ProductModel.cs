using System;
using System.Collections.Generic;
using System.Text;

namespace cs_IMMANUEL_JOSEPH_2301852215.Model
{
    public class ProductModel
    {

        public ProductModel()
        {

        }

        public ProductModel(int ProductID, string ProductName, int ProductPrice, int ProductQty)
        {
            this.ProductID = ProductID;
            this.ProductName = ProductName;
            this.ProductPrice = ProductPrice;
            this.ProductQty = ProductQty;
        }

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public int ProductQty { get; set; }
    }
}
