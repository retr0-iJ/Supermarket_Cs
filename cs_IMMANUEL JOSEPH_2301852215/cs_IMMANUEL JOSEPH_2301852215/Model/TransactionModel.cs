using System;
using System.Collections.Generic;
using System.Text;

namespace cs_IMMANUEL_JOSEPH_2301852215.Model
{
    public class TransactionModel
    {
        public int TransactionID;
        public string PaymentMethod;
        public List<ProductModel> detailList;
    }
}
