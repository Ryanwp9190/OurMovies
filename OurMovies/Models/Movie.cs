using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OurMovies.Models
{
    public class Movie
    {
        [BsonId]
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; } //TODO: Need to convert from bson value in attribute
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public bool Purchased { get; set; }
        public string Format { get; set; }
    }
}
