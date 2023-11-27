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
  user: IUser = new User();
  collapsed = false;
  isYourPage: boolean = false;

  constructor(private _router: Router, private _postService: PostService, private _userService: UserService, private _route: ActivatedRoute) { }

  //  Gets all posts belonging to the user.
  getPosts(id: number): void {
    this._userService.getPosts(id)
      .subscribe(data => {
        console.log("AllPosts", JSON.stringify(data));  //  Log response.
        this.posts = data;
      }
    );
  }

  //  Gets all comments belonging to the user.
  getComments(id: number): void {
    this._userService.getComments(id)
      .subscribe(data => {
        console.log("AllComments", JSON.stringify(data));  //  Log response.
        this.comments = data;
      }
      );
  }

  //  Gets all votes belonging to the user.
  getVotes(id: number): void {
    this._userService.getVotes(id)
      .subscribe(data => {
        console.log("AllVotes", JSON.stringify(data));  //  Log response.
        this.votes = data;
      }
      );
  }

  //  Gets the user object for this page.
  getUser(id: number): void {
    this._userService.getSimplifiedUser(id)
      .subscribe(data => {
        console.log("TheUser", JSON.stringify(data));  //  Log response.
        this.user = data;
      }
      );
  }

  //  To be called on Initiliazation.
  ngOnInit(): void {
    this._route.params.subscribe(params => {
      //  Get the posts, comments and votes.
      this.getPosts(params["id"]);
      this.getComments(params["id"]);
      this.getVotes(params["id"]);
      //  Gets the user and if it is signed in.
      this.getUser(params["id"]);
      this.getSignedIn(params["id"]);
    });
    
  }

  //  Check to see if the user is signed in. If signed in and this page is the profile
  //   page for the user, then some additional options will be available in the html.
  getSignedIn(postId: number): void {
    this._postService.getSignedIn(postId).subscribe(response => {
      if (response.success) {
        console.log("signed in: " + response.message + " " + response.userspost);
        if (response.message = this.user.IdentityUserId) {
          this.isYourPage = true;
          console.log("YOUR OWN USERPAGE");  //  Log message.
        }
      }
      else {
        console.log("Not signed in");
      }
    });
  }

  //  Function to toggle visibility of the box with posts, comments and votes.
  toggle(content: string) {  //  content is the id of the html section to be toggled. For examole 'posts'.
    this.collapsed = !this.collapsed;
    var section = document.getElementById(content)!;  //  Get the html section.
    if (section.style.maxHeight) {
      section.style.maxHeight = "";
    } else {
      section.style.maxHeight = section.scrollHeight + "px";
    } 
  }


}
