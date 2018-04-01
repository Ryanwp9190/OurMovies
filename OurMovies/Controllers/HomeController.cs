using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OurMovies.Models;

namespace OurMovies.Controllers
{
    public class HomeController : Controller
    {

        private IMongoDatabase mongoDatabase;

        public IMongoDatabase GetMongoDatabase()
        {
            MongoClient mongoClient = new MongoClient("mongodb://localhost:27017");
            return mongoClient.GetDatabase("MoviesDB");
        }

        [HttpGet]
        public IActionResult Index()
        {
            mongoDatabase = GetMongoDatabase();
            var result = mongoDatabase.GetCollection<Movie>("Movies").Find(FilterDefinition<Movie>.Empty).ToList();
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Movie movie)
        {
            try
            {
                mongoDatabase = GetMongoDatabase();
                mongoDatabase.GetCollection<Movie>("Movies").InsertOne(movie);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            mongoDatabase = GetMongoDatabase();
            //Fetch details from a Movie id and pass to view
            Movie movie = mongoDatabase.GetCollection<Movie>("Movies").Find<Movie>(k => k.ID == id).FirstOrDefault();

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            mongoDatabase = GetMongoDatabase();

            //Fetch details from a Movie id and pass to view
            Movie movie = mongoDatabase.GetCollection<Movie>("Movies").Find<Movie>(k => k.ID == id).FirstOrDefault();

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);

        }

        [HttpPost]
        public IActionResult Delete(Movie movie)
        {
            try
            {
                mongoDatabase = GetMongoDatabase();
                //Delete movie record
                var result = mongoDatabase.GetCollection<Movie>("Movies").DeleteOne<Movie>(k => k.ID == movie.ID);
                if (result.IsAcknowledged == false)
                {
                    return BadRequest("Unable to Delete Movie " + movie.ID);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            mongoDatabase = GetMongoDatabase();
            //Fetch details based on id and pass to view
            var movie = mongoDatabase.GetCollection<Movie>("Movies").Find<Movie>(k => k.ID == id).FirstOrDefault();
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [HttpPost]
        public IActionResult Edit(Movie movie)
        {
            try
            {
                mongoDatabase = GetMongoDatabase();
                //build where condition
                var filter = Builders<Movie>.Filter.Eq("ID", movie.ID);
                //build update statement
                var updatestatement = Builders<Movie>.Update.Set("ID", movie.ID);
                updatestatement = updatestatement.Set("Title", movie.Title);
                updatestatement = updatestatement.Set("ReleaseDate", movie.ReleaseDate);
                updatestatement = updatestatement.Set("Genre", movie.Genre);
                updatestatement = updatestatement.Set("Price", movie.Price);
                updatestatement = updatestatement.Set("Purchased", movie.Purchased);
                updatestatement = updatestatement.Set("Format", movie.Format);
                //fetch details and pass into view
                var result = mongoDatabase.GetCollection<Movie>("Movies").UpdateOne(filter, updatestatement);
                if (result.IsAcknowledged == false)
                {
                    return BadRequest("Unable to update Movie " + movie.Title);
                }

            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
