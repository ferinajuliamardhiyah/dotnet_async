using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;

namespace dotnet_async {
    public class Post {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }

    public class User {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public Address address { get; set; }
    }

    public class Address {
        public string street { get; set; }
        public string suite { get; set; }
        public string city { get; set; }
    }

    public class Movies {
        [JsonProperty ("results")]
        public List<TopRated> movies { get; set; }
    }

    public class TopRated {
        public string title { get; set; }
        public double vote_average { get; set; }
    }

    public class Product {
        public int id { get; set; }
        public string name { get; set; }
        public long price { get; set; }
        public string category { get; set; }
    }

    public class PostUser {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public User user { get; set; }
    }

    class Program {
        static HttpClient client = new HttpClient ();
        static void Main (string[] args) {
            //01.md
            // var post = getPost().GetAwaiter().GetResult();
            // foreach (var item in post)
            // {
            //    Console.WriteLine($"title: {item.title}");
            // }

            //02.md
            // var user = getUser().GetAwaiter().GetResult();
            // foreach (var item in user)
            // {
            //    Console.WriteLine($"alamat: {item.address.street}, {item.address.city}, {item.address.suite}");
            // }

            //03.md
            // var movie = getMovie().GetAwaiter().GetResult();
            // foreach (var item in movie.movies)
            // {
            //    Console.WriteLine($"title: {item.title} - vote: {item.vote_average}");
            // }

            //04.md
            // var userFilter = getUser().GetAwaiter().GetResult().Where(c => c.name.Contains("le"));
            // var final = JsonConvert.SerializeObject(userFilter);
            // Console.WriteLine(final);

            //05.md
            // var movieFilter = getMovie().GetAwaiter().GetResult().movies.Where(c => c.vote_average > 8.4);
            // var final = JsonConvert.SerializeObject(movieFilter);
            // Console.WriteLine(final);

            //06.md
            // var sum = getProduct().GetAwaiter().GetResult().Sum(c => c.price);
            // Console.WriteLine($"total harga: {sum}");

            //07.md
            // var fruits = getProduct().GetAwaiter().GetResult().Where(c => c.category == "fruits");
            // var final = JsonConvert.SerializeObject(fruits);
            // Console.WriteLine(final);

            //08.md
            // var pricey = getProduct().GetAwaiter().GetResult().Where(c => c.price > 70);
            // var final = JsonConvert.SerializeObject(pricey);
            // Console.WriteLine(final);

            //09.md
            var posts = getPost ().GetAwaiter ().GetResult ();
            var users = getUser ().GetAwaiter ().GetResult ();

            var JoinUserandPost = posts.Select(e => {
                return new PostUser() {
                    userId = e.userId,
                    id = e.id,
                    title = e.title,
                    body = e.body,
                    user = users.Find(user => user.id == e.userId)
                };
            }).ToList();

            var final = JsonConvert.SerializeObject(JoinUserandPost);
            Console.WriteLine(final);
        }

        async static Task<List<Post>> getPost () {
            var result = await client.GetStringAsync ("https://jsonplaceholder.typicode.com/posts");
            return JsonConvert.DeserializeObject<List<Post>> (result);
        }

        async static Task<List<User>> getUser () {
            var result = await client.GetStringAsync ("https://jsonplaceholder.typicode.com/users");
            return JsonConvert.DeserializeObject<List<User>> (result);
        }

        async static Task<Movies> getMovie () {
            var result = await client.GetStringAsync ("https://api.themoviedb.org/3/movie/top_rated?api_key=d3afb5ba2f51532b4d77bda3e1fba203&language=en-US&page=1");
            return JsonConvert.DeserializeObject<Movies> (result);
        }

        async static Task<List<Product>> getProduct () {
            var result = await client.GetStringAsync ("https://res.cloudinary.com/sivadass/raw/upload/v1535817394/json/products.json");
            return JsonConvert.DeserializeObject<List<Product>> (result);
        }

    }
}