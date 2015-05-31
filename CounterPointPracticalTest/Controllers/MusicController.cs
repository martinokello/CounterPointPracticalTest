using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using CounterPointDAL.Repository;
using CounterPointPracticalTest.Models;

namespace CounterPointPracticalTest.Controllers
{
    public class MusicController : Controller
    {
        //
        // GET: /Music/
        private Repository musicRepository;

        public ActionResult Index()
        {
            var musicModels = GetMusicModel();
            SeedPaging(musicModels, 1);

            var startIndex = ViewBag.CurrentPage == 1 ? 0 : (ViewBag.CurrentPage - 1) * 10;
            var count = 1 < ViewBag.NumberOfPages ? 10 :ViewBag.NumberOfPages % 10 == 0? 10 : musicModels.Count%10;
            var model = musicModels.GetRange(startIndex, count);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int productId)
        {
            musicRepository = new Repository();
            var musicCD = musicRepository.GetMusicCDByProductID(productId);
            var musicModel = new MusicModel();
            musicModel.MusicCD = musicCD;

            return View(musicModel);
        }

        [HttpGet]
        public ActionResult Details(int productId)
        {
            musicRepository = new Repository();
            var musicCD = musicRepository.GetMusicCDByProductID(productId);
            var musicModel = new MusicModel();
            musicModel.MusicCD = musicCD;
            var salesLine = musicRepository.GetSalesLineByProductID(productId);
            musicModel.SalesLine = salesLine;
            return View(musicModel);
        }

        [HttpPost]
        public ActionResult Create(MusicModel model)
        {
            musicRepository = new Repository();
            var success = musicRepository.AddSalesToProduct(model.MusicCD, model.SalesLine);

            return View("SuccessCreateSalesItem");
        }

        [HttpGet]
        public ActionResult Delete(int productId)
        {
            musicRepository = new Repository();

            var musicCD = musicRepository.GetMusicCDByProductID(productId);
            var salesLine = musicRepository.GetSalesLineByProductID(productId);
            musicRepository.DeleteSaleLineForProduct(musicCD, salesLine);
            return View("DeleteSuccess");
        }

        [HttpGet]
        public ActionResult Page(int page)
        {
            var musicModels = GetMusicModel();

            SeedPaging(musicModels, page);

            var startIndex = ViewBag.CurrentPage == 1 ? 0 : (ViewBag.CurrentPage - 1) * 10;
            var count = page < ViewBag.NumberOfPages ? 10 :ViewBag.NumberOfPages % 10 == 0? 10 : musicModels.Count%10;
            var model = musicModels.GetRange(startIndex, count);
            return View("Index", model);
        }

        private void SeedPaging(IList<Models.MusicModel> musicModels,int page)
        {
            
            if (musicModels.Count % 10 > 0) ViewBag.NumberOfPages = musicModels.Count/10 + 1;
            else ViewBag.NumberOfPages = musicModels.Count/10;
            ViewBag.CurrentPage = page;
            ViewBag.StartPage = page < 6 ? 1 : page%5 == 0?page - 4: page - page % 5 + 1;

            ViewBag.FirstDisplay = page > 10;
            ViewBag.NextDisplay = page < ViewBag.NumberOfPages;
            ViewBag.LastDisplay = page < ViewBag.NumberOfPages - 10;
            ViewBag.PreviousDisplay = page > 1;

            ViewBag.FirstPage = 1;
            ViewBag.PreviousPage = page > 1 ? page - 1 : page;
            ViewBag.NextPage = page < ViewBag.NumberOfPages ? page + 1 : page;
            ViewBag.LastPage = ViewBag.NumberOfPages;
        }
        private List<MusicModel> GetMusicModel()
        {
            musicRepository = new Repository();
            var musicModels = new List<MusicModel>();

            var musicCDs = musicRepository.GetAllMusicCDs();
            foreach (var cd in musicCDs)
            {
                var model = new MusicModel();
                model.MusicCD = cd;

                model.SalesLine = musicRepository.GetSalesLineByProductID(cd.ProductId);

                musicModels.Add(model);
            }

            return musicModels;
        }
    }
}
