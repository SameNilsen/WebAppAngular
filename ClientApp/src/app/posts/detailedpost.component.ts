import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl, Validators, FormBuilder, ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { PostService } from "./posts.service";
import { CommentService } from "./comments.service";
import { UserService } from "../user/users.service";
import { IPost, Post } from "./post";
import { IComment } from "./comment";
import { IUser, User } from "../user/user";

@Component({
  selector: "app-posts-detailedpost",
  templateUrl: "./detailedpost.component.html",
})

export class DetailedPostComponent implements OnInit {

  post: IPost = new Post();
  user: IUser = new User();
  comments: IComment[] = [];
  
  constructor(private _router: Router, private _postService: PostService, private _route: ActivatedRoute, private _userService: UserService, private _commentService: CommentService) {
    
  }

  ngOnInit(): void {
    this._route.params.subscribe(params => {
      //this.post = params["id"]
      console.log("THIS IS ID: " + params["id"]);
      this.loadPost(params["id"]);
      //this.loadUser(this.post.UserId);
    });
  }

  loadPost(postId: number) {
    this._postService.getPostById(postId)
      .subscribe(
        (post: any) => {
          console.log("retrived post: ", post);
          this.post = post;
          console.log("This is title: ", post.Title);
          this.loadUser(this.post.UserId);
          //this.comments = this.post.comments;
          this.getComments(postId); //  Istedet for post.Comments fordi det tuller med minnet.
        },
        (error: any) => {
          console.log("Error loading post:", error);
        }
      );
  }

  loadUser(userId: number) {
    this._userService.getUserById(userId)
      .subscribe(
        (user: any) => {
          console.log("retrived user: ", user);
          this.user = user;
          console.log("This is user: ", user.Name);
        },
        (error: any) => {
          console.log("Error loading user:", error);
        }
      );
  }

  getComments(postId: number): void {
    console.log("Getting the comments");
    this._commentService.getCommentsByPostId(postId)
      .subscribe(data => {
        console.log("AllComments", JSON.stringify(data));
        this.comments = data;
        console.log(this.comments);
        //  Filter kommentarer?
      }
      );
    console.log("it worked?");
  }

  
}
