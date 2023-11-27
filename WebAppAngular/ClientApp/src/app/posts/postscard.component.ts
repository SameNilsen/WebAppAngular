import { Component, OnInit } from "@angular/core";
import { IPost } from "./post";
import { IUser } from "../user/user";
import { Router } from "@angular/router";
import { PostService } from "./posts.service";
import { UserService } from "../user/users.service";


@Component({
  selector: "app-posts-postcard",
  templateUrl: "./postscard.component.html"
})

export class PostsCardComponent implements OnInit {

  viewTitle: string = "MainFeedTable";
  displayImage: boolean = true;
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
  yourVotes: { [id: string]: string; } = {};  //  Map/dictionary of the users vote on each post. Mapped by postId.
  isSignedIn: boolean = false;

  constructor(private _router: Router, private _postService: PostService, private _userService: UserService) { }

  //  Function for deleting post.
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

  //  Function to get all posts.
  getPosts(): void {
    this._postService.getPosts()
      .subscribe(data => {
        console.log("All", JSON.stringify(data));  //  Log response.
        this.posts = data;
        this.filteredPosts = this.posts;  //  Set to variable.
        this.getVoting();  //  Call on function to get all votes by user on these posts.
      }
      );
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

  //  Filters posts.
  performFilter(filterBy: string): IPost[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.posts.filter((post: IPost) =>
      post.Title.toLocaleLowerCase().includes(filterBy));  // return filtered posts by filter input.
  }

  //  Function to toggle on and of visibility of filter input field.
  toggleFilter(id: string): void {
    var div = document.getElementById(id)!;
    if (div.style.display == "block") {
      div.style.display = "none";
    }
    else {
      div.style.display = "block";
    }
  }

  ngOnInit(): void {
    //document.body.setAttribute('data-bs-theme', "dark");
    document.documentElement.setAttribute('data-bs-theme', "dark");
    this.getSignedIn();
    this.getPosts();  //  Get all posts.
  }

  //  Function to see if user is signed in. Used for upvoting.
  getSignedIn(): void {
    this._postService.getSignedIn(-1).subscribe(response => {
      if (response.success) {
        //  Set and log signed in.
        console.log("signed in: " + response.message + " " + response.userspost);
        this.isSignedIn = true;
      }
      else {
        console.log("Not signed in");
      }
    });
  }

  //  Method for get users previous vote for each post. In project 1 we did not have a filter, and
  //   therefore we could just get all votes by user for each post and put it in a list with equal size
  //    as posts list, and when displaying the posts we would know that the correct uservote would have
  //     the same index as the post in their respective list. When filtering this would not work, unless we
  //     fetched the votes from server each time we alter filter. This would be ineffective. So this time
  //     we map each uservote to its post with a dictionary upon initialization, and then getting the correct
  //     uservote from that when needed. 
  getVoting(): void {
    this.posts.forEach((post) => {  //  Iterates through all posts.
      var postId = post.PostId;
      this._postService.getVote(postId).subscribe(response => {  //  Gets the users vote on that post.
        if (response.success) {
          this.yourVotes[postId.toString()] = response.vote;    //  Map it in dictionary.
        }
        else {
          this.yourVotes.postId = "blank";
        }
      });
    });
  }

  //  When clicking upvote button on a post.
  setUpvote(postId: number): void {
    if (this.isSignedIn) {  //  Has to be signed in.
      this._postService.upvotePost(postId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);  //  Log success.
              location.reload();
            }
          }
          , (error) => {
            console.log("Error upvoting: ", error);
          });
    }
  }

  //  When clicking downvote button on a post.
  setDownvote(postId: number): void {
    if (this.isSignedIn) {  //  Must be signed in.
      this._postService.downvotePost(postId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);  //  Log success.
              location.reload();
            }
          }
          , (error) => {
            console.log("Error downvoting:", error);
          });
    }
  }

}
