using System.Collections.Generic;
using CounterPointDAL.MusicStoreDomainModel;


namespace CounterPointDAL.Repository.AbstractRepository
{
    interface IRepository
    {
        IEnumerable<MusicCD> GetAllMusicCDs();
        MusicCD GetMusicCDByProductID(int productId);
        int? AddSalesToProduct(MusicCD product, SalesLine saleLine);
        bool? DeleteSaleLineForProduct(MusicCD product, SalesLine saleLine);
    }
}
