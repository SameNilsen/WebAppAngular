import { Component, OnInit } from "@angular/core";
import { IPost } from "./post";
import { IUser } from "../user/user";
import { Router } from "@angular/router";
import { PostService } from "./posts.service";
import { UserService } from "../user/users.service";


@Component({
  selector: "app-posts-component",
  templateUrl: "./posts.component.html"
})

export class PostsComponent implements OnInit{

  viewTitle: string = "MainFeedTable";

  //  List filter getter and setter:
  private _listFilter: string = "";
  get listFilter(): string {
    return this._listFilter;
  }
  set listFilter(value: string) {
    this._listFilter = value;
    console.log("In setter:", value);
    this.filteredPosts = this.performFilter(value);
  }

  posts: IPost[] = [];
  users: IUser[] = [];

  constructor(private _router: Router, private _postService: PostService, private _userService: UserService) { }

  //  Method for deleting post.
  deletePost(post: IPost): void {
    const confirmDelete = confirm(`Are you sure you want to delete "${post.Title}"?`);
    if (confirmDelete) {
      this._postService.deletePost(post.PostId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              this.filteredPosts = this.filteredPosts.filter(i => i !== post);
            }
          }
          , (error) => {
            console.log("Error deleting post:", error);
          });
    }
  }


  //  Method for fetching all posts. Should be called on init.
  getPosts(): void {
    //  Uses our service injection for posts to call method getPosts(), and listens for response.
    this._postService.getPosts()
      .subscribe(data => {
        console.log("All", JSON.stringify(data));  //  Print result to console.
        this.posts = data;                         //  Sets data in out posts variable.
        this.filteredPosts = this.posts;           //  Also sets it in filteredPosts, so its ready for filtering.
      }
    );
    console.log("it worked?");
  }

  //  Get users for posts.
  getUsers(): void {
    this._userService.getUsers()
      .subscribe(data => {
        console.log("All", JSON.stringify(data));
        this.users = data;
      }
      );
  }

  filteredPosts: IPost[] = this.posts;

  //  The filtering method.
  performFilter(filterBy: string): IPost[] {
    filterBy = filterBy.toLocaleLowerCase();  //  Converts filterString to lowercase.
    return this.posts.filter((post: IPost) =>
      post.Title.toLocaleLowerCase().includes(filterBy));  //  Uses built in filter method.
  }

  //  Function for toggling visibility for filter input field.
  toggleFilter(id: string): void {
    var div = document.getElementById(id)!;
    if (div.style.display == "block") {
      div.style.display = "none";
    }
    else {
      div.style.display = "block";
    }
  }

  //  Method to be called on page load. As a lifecycle hook it executes at the beginning.
  ngOnInit(): void {
    document.documentElement.setAttribute('data-bs-theme', "dark");  //  Sets html document to dark theme.
    this.getPosts()  //  Calls method to get all posts to be displayed in table.
  }

}
