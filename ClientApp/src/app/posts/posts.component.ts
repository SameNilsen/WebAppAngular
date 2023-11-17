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
  displayImage: boolean = true;    //  Kan fjernes tror jeg.
  //listFilter: string = "";

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

  //  Method for deleting post. May not be in use anymore??
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

  //  Method for fetching all posts. Should be called on init.
  getPosts(): void {
    //  Uses our service injection for posts to call method getPosts(), and listens for response.
    this._postService.getPosts()
      .subscribe(data => {
        console.log("All", JSON.stringify(data));  //  Print result to console.
        console.log("postscomponent2?");
        this.posts = data;                         //  Sets data in out posts variable.
        console.log(this.posts);
        this.filteredPosts = this.posts;           //  Also sets it in filteredPosts, so its ready for filtering.
      }
    );
    console.log("it worked?");
  }

  //  Not in use??
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

  //  The filtering method.
  performFilter(filterBy: string): IPost[] {
    filterBy = filterBy.toLocaleLowerCase();  //  Converts filterString to lowercase.
    return this.posts.filter((post: IPost) =>
      post.Title.toLocaleLowerCase().includes(filterBy));  //  Uses built in filter method.
  }

  //  Method to be called on page load. As a lifecycle hook it executes at the beginning.
  ngOnInit(): void {
    //document.body.setAttribute('data-bs-theme', "dark");
    document.documentElement.setAttribute('data-bs-theme', "dark");  //  Sets html document to dark theme.
    this.getPosts()  //  Calls method to get all posts to be displayed in table.
  }

}
