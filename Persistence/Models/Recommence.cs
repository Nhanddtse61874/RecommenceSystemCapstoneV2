using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Models
{
    public class Recommence : BaseModel
    {
        public Recommence()
        {

        }
        public Recommence(int userid, IList<Product> productCodesOfUserLogging, IList<List<Product>> listDataProductCodes)
        {
            UserId = userid;

            ProductCodeOfUserLogging = productCodesOfUserLogging;

            ListDataProductCodes = listDataProductCodes;
        }

        public int UserId { get; set; }

        public ICollection<Product> ProductCodeOfUserLogging { get; set; }

        public IList<List<Product>> ListDataProductCodes { get; set; }
    }

}
