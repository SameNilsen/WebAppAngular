using Microsoft.EntityFrameworkCore;
using OsloMetAngular.Models;

namespace OsloMetAngular.DAL
{
    public static class DBInit
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            PostDbContext context = serviceScope.ServiceProvider.GetRequiredService<PostDbContext>();
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!context.Posts.Any())
            {
                var posts = new List<Post>
                {
                    new Post
                    {
                        Title = "LEGO® Star Wars™: The Skywalker Saga",
                        Text = "What do you think about this game?" +
                        " I have about 50 hours into the game now, but I would like to hear your opinions.",
                        ImageUrl = "assets/images/legostarwars.jpg",
                        //UserId = 1,
                        PostDate = new DateTime(2023, 10, 3, 13, 42, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "Gaming"
                    },
                    new Post
                    {
                        Title = "Halloween is approaching!",
                        Text = "Do you celebrate halloween? If so, how do you celebrate it?",
                        ImageUrl = "assets/images/halloween.jpg",
                        //UserId = 2,
                        PostDate = new DateTime(2023, 10, 5, 11, 22, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "General"
                    },
                    new Post
                    {
                        Title = "Is this a good pokémon team?",
                        Text = "I think I have assembled a good team now, but what do you guys think?",
                        ImageUrl = "assets/images/pokemon_team.png",
                        //UserId = 4,
                        PostDate = new DateTime(2023, 10, 6, 17, 30, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "Gaming"
                    },
                    new Post
                    {
                        Title = "Five Nights at Freddy's Movie",
                        Text = "What do you think about this newly released movie?",
                        ImageUrl = "assets/images/fnaf.jpg",
                        //UserId = 3,
                        PostDate = new DateTime(2023, 10, 8, 19, 7, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "General"
                    },
                    new Post
                    {
                        Title = "Electric planes",
                        Text = "Should we continue researching on eletric planes? The fact that it is good " +
                        "for the environment is a plus, but the battery will be too heavy to fly with.",
                        ImageUrl = "assets/images/plane.jpg",
                        //UserId = 1,
                        PostDate = new DateTime(2023, 10, 10, 16, 10, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "Politics"
                    },
                    new Post
                    {
                        Title = "Which tree is this?",
                        Text = "I have seen this tree everywhere, but I don't know which tree it is.",
                        ImageUrl = "assets/images/tree.jpg",
                        //UserId = 5,
                        PostDate = new DateTime(2023, 10, 11, 9, 58, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "Nature"
                    },
                    new Post
                    {
                        Title = "How do you like the new design on the Witcher 3 cover art?",
                        Text = "Recently when scrolling through the internet, I've noticed that " +
                        "the cover art has changed!",
                        ImageUrl = "assets/images/witcher.png",
                        //UserId = 3,
                        PostDate = new DateTime(2023, 10, 14, 13, 48, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "Gaming"
                    },
                    new Post
                    {
                        Title = "Why does Duolingo teach you weird stuff??",
                        Text = "Every once in a while, it seems that Duolingo gives " +
                        "you these random sentences.",
                        ImageUrl = "assets/images/duolingo.jpg",
                        //UserId = 1,
                        PostDate = new DateTime(2023, 10, 15, 11, 29, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "Politics"
                    },
                    new Post
                    {
                        Title = "Ryokan or hotel?",
                        Text = "Should I stay in a ryokan or in a hotel? Any1 got any experience?",
                        ImageUrl = "assets/images/ryokan.jpg",
                        //UserId = 6,
                        PostDate = new DateTime(2023, 10, 15, 15, 52, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "General"
                    },
                    new Post
                    {
                        Title = "Are these two players good?",
                        Text = "I have been seeing them all over the internet recently, but are they good?",
                        ImageUrl = "assets/images/soccer.jpg",
                        //UserId = 2,
                        PostDate = new DateTime(2023, 10, 19, 12, 18, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "Sport"
                    },
                    new Post
                    {
                        Title = "Can you help me with this math problem?",
                        Text = "Can someone help me solve this math problem? I don't know how to solve it",
                        ImageUrl = "assets/images/math.jpg",
                        //UserId = 4,
                        PostDate = new DateTime(2023, 10, 20, 23, 59, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "School"
                    },
                    new Post
                    {
                        Title = "What flower is this??",
                        Text = "Does anyone know what flower this is?",
                        ImageUrl = "assets/images/flower.jpg",
                        //UserId = 3,
                        PostDate = new DateTime(2023, 10, 22, 22, 21, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "Nature"
                    },
                    new Post
                    {
                        Title = "EpicForum.NO or Reddit?",
                        Text = "I think this website is much better than Reddit, what about you?",
                        ImageUrl = "assets/images/reddit.jpg",
                        //UserId = 6,
                        PostDate = new DateTime(2023, 10, 27, 10, 1, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "General"
                    },
                    new Post
                    {
                        Title = "Physical school books, or digital?",
                        Text = "I've recently bought a ton of books, but now I am regretting my decision. " +
                        "What do you prefer?",
                        ImageUrl = "assets/images/books.jpg",
                        //UserId = 1,
                        PostDate = new DateTime(2023, 10, 28, 15, 6, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "School"
                    },
                };
                context.AddRange(posts);
                context.SaveChanges();
            }

            //if (!context.Customers.Any())
            //{
            //    var customers = new List<Customer>
            //    {
            //        new Customer { Name = "Alice Hansen", Address = "Osloveien 1"},
            //        new Customer { Name = "Bob Johansen", Address = "Oslomet gata 2"},
            //    };
            //    context.AddRange(customers);
            //    context.SaveChanges();
            //}

            //if (!context.Orders.Any())
            //{
            //    var orders = new List<Order>
            //    {
            //        new Order {OrderDate = DateTime.Today.ToString(), CustomerId = 1},
            //        new Order {OrderDate = DateTime.Today.ToString(), CustomerId = 2},
            //    };
            //    context.AddRange(orders);
            //    context.SaveChanges();
            //}

            //if (!context.OrderItems.Any())
            //{
            //    var orderItems = new List<OrderItem>
            //    {
            //        new OrderItem { ItemId = 1, Quantity = 2, OrderId = 1},
            //        new OrderItem { ItemId = 2, Quantity = 1, OrderId = 1},
            //        new OrderItem { ItemId = 3, Quantity = 4, OrderId = 2},
            //    };
            //    foreach (var orderItem in orderItems)
            //    {
            //        var item = context.Items.Find(orderItem.ItemId);
            //        orderItem.OrderItemPrice = orderItem.Quantity * item?.Price ?? 0;
            //    }
            //    context.AddRange(orderItems);
            //    context.SaveChanges();
            //}

            //var ordersToUpdate = context.Orders.Include(o => o.OrderItems);
            //foreach (var order in ordersToUpdate)
            //{
            //    order.TotalPrice = order.OrderItems?.Sum(oi => oi.OrderItemPrice) ?? 0;
            //}
            //context.SaveChanges();
        }
    }
}
