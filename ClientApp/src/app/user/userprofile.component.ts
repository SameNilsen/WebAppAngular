import { Component, OnInit } from "@angular/core";
import { IPost } from "../posts/post";
import { IComment } from "../posts/comment";
import { IUpvote } from "../posts/upvote";
import { IUser, User } from "../user/user";
import { filter } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { ActivatedRoute, Router } from "@angular/router";
import { PostService } from "../posts/posts.service";
import { UserService } from "../user/users.service";


@Component({
  selector: "app-user-component",
  templateUrl: "./userprofile.component.html"
})

export class UserProfileComponent implements OnInit{

  posts: IPost[] = [];
  comments: IComment[] = [];
  votes: IUpvote[] = [];
  //users: IUser[] = [];
  user: IUser = new User();
  collapsed = false;

  constructor(private _router: Router, private _postService: PostService, private _userService: UserService, private _route: ActivatedRoute) { }


  getPosts(id: number): void {
    this._userService.getPosts(id)
      .subscribe(data => {
        console.log("AllPosts", JSON.stringify(data));
        this.posts = data;
        console.log(this.posts);
      }
    );
    console.log("it worked?");
  }

  getComments(id: number): void {
    this._userService.getComments(id)
      .subscribe(data => {
        console.log("AllComments", JSON.stringify(data));
        this.comments = data;
        console.log(this.comments);
      }
      );
    console.log("it worked?");
  }

  getVotes(id: number): void {
    this._userService.getVotes(id)
      .subscribe(data => {
        console.log("AllVotes", JSON.stringify(data));
        this.votes = data;
        console.log(this.votes);
      }
      );
    console.log("it worked?");
  }

  getUser(id: number): void {
    this._userService.getSimplifiedUser(id)
      .subscribe(data => {
        console.log("TheUser", JSON.stringify(data));
        this.user = data;
        console.log(this.user);
      }
      );
    console.log("it worked?");
  }


  ngOnInit(): void {
    this._route.params.subscribe(params => {
      this.getPosts(params["id"]);
      this.getComments(params["id"]);
      this.getVotes(params["id"]);
      this.getUser(params["id"]);
    });
    
  }

  toggle(content: string) {
    this.collapsed = !this.collapsed;
    var section = document.getElementById(content)!;
    console.log("It is now: " + section.style.maxHeight);
    if (section.style.maxHeight) {
      console.log("collapse!");
      section.style.maxHeight = "";
    } else {
      console.log("expand!");
      section.style.maxHeight = section.scrollHeight + "px";
    } 
  }

  expand() {
    console.log("expand!");
    this.collapsed = false;
    var section = document.getElementById("posts")!;
    if (section.style.maxHeight) {
      section.style.maxHeight = "0";
    } else {
      section.style.maxHeight = section.scrollHeight + "px";
    } 
  }

  collapse() {
    console.log("collapse!");
    this.collapsed = true;
    var section = document.getElementById("posts")!;
    if (section.style.maxHeight) {
      section.style.maxHeight = "0";
    } else {
      section.style.maxHeight = section.scrollHeight + "px";
    } 
  }

}
