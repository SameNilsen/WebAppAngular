import { Component, OnInit } from "@angular/core";
import { IPost } from "./post";
import { IUser } from "../user/user";
import { filter } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { PostService } from "./posts.service";
import { UserService } from "../user/users.service";


@Component({
  selector: "app-posts-component",
  templateUrl: "./posts.component.html"
})

export class PostsComponent implements OnInit{

  viewTitle: string = "MainFeedTable";
  displayImage: boolean = true;
  //listFilter: string = "";
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

  testNavigate() {
    this._router.navigate(["/test"]);
  }

  getPosts(): void {
    console.log("postscomponent1")
    this._postService.getPosts()
      .subscribe(data => {
        console.log("All", JSON.stringify(data));
        console.log("postscomponent2?");
        this.posts = data;
        console.log(this.posts);
        this.filteredPosts = this.posts;
      }
    );
    console.log("it worked?");
  }

  getUsers(): void {
    console.log("postscomponent21")
    this._userService.getUsers()
      .subscribe(data => {
        console.log("All", JSON.stringify(data));
        console.log("postscomponent22?");
        this.users = data;
      }
      );
    console.log("it worked?");
  }

  filteredPosts: IPost[] = this.posts;

  performFilter(filterBy: string): IPost[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.posts.filter((post: IPost) =>
      post.Title.toLocaleLowerCase().includes(filterBy));
  }

  ngOnInit(): void {
    this.getPosts()
  }

}
