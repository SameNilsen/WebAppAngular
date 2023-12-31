﻿using Microsoft.EntityFrameworkCore;
using OsloMetAngular.Models;

namespace OsloMetAngular.DAL
{
    public static class DBInit
    {
        /* INITIAL DATA */
        public static void Seed(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            PostDbContext context = serviceScope.ServiceProvider.GetRequiredService<PostDbContext>();

            // Wipe and recreate database
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Initial user data
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new() {
                        Name = "choice.cold@outlook.com",
                        Credebility = 62
                    },
                    new() {
                        Name = "revolution_bit@gmail.com",
                        Credebility = 35
                    },
                    new() {
                        Name = "alex69@gmail.com",
                        Credebility = 22
                    },
                    new() {
                        Name = "pokemonmaster@email.com",
                        Credebility = 31
                    },
                    new() {
                        Name = "alice@outlook.com",
                        Credebility = 45
                    },
                    new() {
                        Name = "steve96@yahoo.com",
                        Credebility = 99
                    }
                };
                context.AddRange(users);
                context.SaveChanges();
            }

            // Initial post data
            if (!context.Posts.Any())
            {
                var posts = new List<Post>
                {
                    new() {
                        Title = "LEGO® Star Wars™: The Skywalker Saga",
                        Text = "What do you think about this game?" +
                        " I have about 50 hours into the game now, but I would like to hear your opinions.",
                        ImageUrl = "assets/images/legostarwars.jpg",
                        UserId = 1,
                        PostDate = new DateTime(2023, 10, 3, 13, 42, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "Gaming"
                    },
                    new() {
                        Title = "Christmas is approaching!",
                        Text = "Do you celebrate christmas? If so, how do you celebrate it?",
                        ImageUrl = "assets/images/jul.jpg",
                        UserId = 2,
                        PostDate = new DateTime(2023, 10, 5, 11, 22, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "General"
                    },
                    new() {
                        Title = "Is this a good pokémon team?",
                        Text = "I think I have assembled a good team now, but what do you guys think?",
                        ImageUrl = "assets/images/pokemon_team.png",
                        UserId = 4,
                        PostDate = new DateTime(2023, 10, 6, 17, 30, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "Gaming"
                    },
                    new() {
                        Title = "Five Nights at Freddy's Movie",
                        Text = "What do you think about this newly released movie?",
                        ImageUrl = "assets/images/fnaf.jpg",
                        UserId = 3,
                        PostDate = new DateTime(2023, 10, 8, 19, 7, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "General"
                    },
                    new() {
                        Title = "Electric planes",
                        Text = "Should we continue researching on eletric planes? The fact that it is good " +
                        "for the environment is a plus, but the battery will be too heavy to fly with.",
                        ImageUrl = "assets/images/plane.jpg",
                        UserId = 1,
                        PostDate = new DateTime(2023, 10, 10, 16, 10, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "Politics"
                    },
                    new() {
                        Title = "Which tree is this?",
                        Text = "I have seen this tree everywhere, but I don't know which tree it is.",
                        ImageUrl = "assets/images/tree.jpg",
                        UserId = 5,
                        PostDate = new DateTime(2023, 10, 11, 9, 58, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "Nature"
                    },
                    new() {
                        Title = "How do you like the new design on the Witcher 3 cover art?",
                        Text = "Recently when scrolling through the internet, I've noticed that " +
                        "the cover art has changed!",
                        ImageUrl = "assets/images/witcher.png",
                        UserId = 3,
                        PostDate = new DateTime(2023, 10, 14, 13, 48, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "Gaming"
                    },
                    new() {
                        Title = "Why does Duolingo teach you weird stuff??",
                        Text = "Every once in a while, it seems that Duolingo gives " +
                        "you these random sentences.",
                        ImageUrl = "assets/images/duolingo.jpg",
                        UserId = 1,
                        PostDate = new DateTime(2023, 10, 15, 11, 29, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "Politics"
                    },
                    new() {
                        Title = "Ryokan or hotel?",
                        Text = "Should I stay in a ryokan or in a hotel? Any1 got any experience?",
                        ImageUrl = "assets/images/ryokan.jpg",
                        UserId = 6,
                        PostDate = new DateTime(2023, 10, 15, 15, 52, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "General"
                    },
                    new() {
                        Title = "Are these two players good?",
                        Text = "I have been seeing them all over the internet recently, but are they good?",
                        ImageUrl = "assets/images/soccer.jpg",
                        UserId = 2,
                        PostDate = new DateTime(2023, 10, 19, 12, 18, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "Sport"
                    },
                    new() {
                        Title = "Can you help me with this math problem?",
                        Text = "Can someone help me solve this math problem? I don't know how to solve it",
                        ImageUrl = "assets/images/math.jpg",
                        UserId = 4,
                        PostDate = new DateTime(2023, 10, 20, 23, 59, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "School"
                    },
                    new() {
                        Title = "What flower is this??",
                        Text = "Does anyone know what flower this is?",
                        ImageUrl = "assets/images/flower.jpg",
                        UserId = 3,
                        PostDate = new DateTime(2023, 10, 22, 22, 21, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "Nature"
                    },
                    new() {
                        Title = "EpicForum.NO or Reddit?",
                        Text = "I think this website is much better than Reddit, what about you?",
                        ImageUrl = "assets/images/reddit.jpg",
                        UserId = 6,
                        PostDate = new DateTime(2023, 10, 27, 10, 1, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "General"
                    },
                    new() {
                        Title = "Physical school books, or digital?",
                        Text = "I've recently bought a ton of books, but now I am regretting my decision. " +
                        "What do you prefer?",
                        ImageUrl = "assets/images/books.jpg",
                        UserId = 1,
                        PostDate = new DateTime(2023, 10, 28, 15, 6, 0).ToString("dd.MM.yyyy HH:mm"),
                        SubForum = "School"
                    },
                };
                context.AddRange(posts);
                context.SaveChanges();
            }

            // Initial comment data
            if (!context.Comments.Any())
            {
                var comments = new List<Comment>
                {
                    new() {
                        CommentText = "Eyo i thinks its legit epic foreal",
                        UserId = 2,
                        PostID = 1,
                        PostDate = new DateTime(2023, 10, 3, 13, 55, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "I don't celebrate it lol",
                        UserId = 1,
                        PostID = 2,
                        PostDate = new DateTime(2023, 10, 5, 11, 51, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "Why did you post this in October?? ",
                        UserId = 6,
                        PostID = 2,
                        PostDate = new DateTime(2023, 10, 6, 14, 19, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "Why do you have 3 fire types??",
                        UserId = 1,
                        PostID = 3,
                        PostDate = new DateTime(2023, 10, 6, 17, 31, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "Bro, please reconsider",
                        UserId = 3,
                        PostID = 3,
                        PostDate = new DateTime(2023, 10, 6, 18, 12, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "It was FIRE, I couldn't sit still bro",
                        UserId = 1,
                        PostID = 4,
                        PostDate = new DateTime(2023, 10, 9, 15, 13, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "Good point, but we should still keep on researching!",
                        UserId = 5,
                        PostID = 5,
                        PostDate = new DateTime(2023, 10, 10, 18, 6, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "I'm not sure myself actually D:",
                        UserId = 3,
                        PostID = 6,
                        PostDate = new DateTime(2023, 10, 13, 8, 2, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "It looks very good",
                        UserId = 2,
                        PostID = 7,
                        PostDate = new DateTime(2023, 10, 14, 15, 22, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "Good art elevates the game suuuu",
                        UserId = 6,
                        PostID = 7,
                        PostDate = new DateTime(2023, 10, 14, 19, 37, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "I KNOW!! It teaches you some weird stuff!",
                        UserId = 5,
                        PostID = 8,
                        PostDate = new DateTime(2023, 10, 15, 11, 30, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "I don't have any experience, but I know it sure is expensive. " +
                        "Maybe you should opt for the hotel option?",
                        UserId = 5,
                        PostID = 9,
                        PostDate = new DateTime(2023, 10, 15, 15, 59, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "The tall one is pretty good I've heard.",
                        UserId = 3,
                        PostID = 10,
                        PostDate = new DateTime(2023, 10, 20, 7, 14, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "Really?",
                        UserId = 1,
                        PostID = 11,
                        PostDate = new DateTime(2023, 10, 21, 22, 55, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "He is just playing around, don't downvote it",
                        UserId = 6,
                        PostID = 11,
                        PostDate = new DateTime(2023, 10, 21, 23, 13, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "Dis is one of the sacred flowers, don't touch it",
                        UserId = 4,
                        PostID = 12,
                        PostDate = new DateTime(2023, 10, 23, 5, 49, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "This website is top tier! Reddit could learn a few things",
                        UserId = 2,
                        PostID = 13,
                        PostDate = new DateTime(2023, 10, 27, 12, 10, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "Hahahaha",
                        UserId = 4,
                        PostID = 13,
                        PostDate = new DateTime(2023, 10, 27, 21, 44, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                    new() {
                        CommentText = "Not even a question, just go for digital- much better",
                        UserId = 5,
                        PostID = 14,
                        PostDate = new DateTime(2023, 10, 28, 16, 29, 0).ToString("dd.MM.yyyy HH:mm")
                    },
                };
                context.AddRange(comments);
                context.SaveChanges();
            }
        }
    }
}
