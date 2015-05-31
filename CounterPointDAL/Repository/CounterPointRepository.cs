using System.Collections.Generic;
using System.Data.SqlClient;
using CounterPointDAL.Repository.AbstractRepository;
using CounterPointDAL.Repository.SqlDbAccess;
using CounterPointDAL.MusicStoreDomainModel;

namespace CounterPointDAL.Repository
{
    public class Repository:IRepository
    {
        private SQLDataQueries queries = new SQLDataQueries();
 
        public IEnumerable<MusicStoreDomainModel.MusicCD> GetAllMusicCDs()
        {
            return queries.GetAllMusicCDs();
        }

        public MusicStoreDomainModel.MusicCD GetMusicCDByProductID(int productId)
        {
            return queries.GetMusicCDByProductID(productId);
        }

        public int? AddSalesToProduct(MusicCD product, SalesLine saleLine)
        {
            return queries.AddSalesLineToProduct(saleLine,product);
        }

        public bool? DeleteSaleLineForProduct(MusicStoreDomainModel.MusicCD product, SalesLine saleLine)
        {
            return queries.DeleteSaleLineForProduct(saleLine,product);
        }

        public SalesLine GetSalesLineByProductID(int productId)
        {
            return queries.GetSalesLineByProductID(productId);
        }
    }
}
