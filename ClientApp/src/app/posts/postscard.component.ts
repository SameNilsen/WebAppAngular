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
  yourVotes: { [id: string]: string; } = {};
  isSignedIn: boolean = false;

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
        this.getVoting();
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
    //document.body.setAttribute('data-bs-theme', "dark");
    document.documentElement.setAttribute('data-bs-theme', "dark");
    this.getSignedIn();
    this.getPosts()
  }

  getSignedIn(): void {
    this._postService.getSignedIn(-1).subscribe(response => {
      if (response.success) {
        console.log("signed in: " + response.message + " " + response.userspost);
        this.isSignedIn = true;
      }
      else {
        console.log("Not signed in");
      }
    });
  }

  getVoting(): void {
    console.log("START: " + this.posts.length);
    this.posts.forEach((post) => {
      console.log(post.PostId);
      var postId = post.PostId;
      this._postService.getVote(postId).subscribe(response => {
        if (response.success) {
          console.log("vote: " + response.vote + " " + postId);
          this.yourVotes[postId.toString()] = response.vote;
          console.log("vote.: " + this.yourVotes + " " + this.yourVotes[postId.toString()]);
          //if (this.yourVote == "upvote") {
          //  document.getElementById("upvoteButton")!.style.color = "rgb(193, 26, 26)";
          //  //document.getElementById("downvoteButton")!.style.color = "inherit";
          //}
          //else if (this.yourVote == "downvote") {
          //  document.getElementById("downvoteButton")!.style.color = "rgb(20, 130, 167)";
          //  //document.getElementById("upvoteButton")!.style.color = "transparent";
          //}
        }
        else {
          console.log("Uffda, ingen votes for " + postId);
          console.log(response.message);
          this.yourVotes.postId = "blank";
        }
      });
    });
    console.log("Alle posts sjekka: " + this.yourVotes);
  }

  setUpvote(postId: number): void {
    console.log(1111);
    if (this.isSignedIn) {
      this._postService.upvotePost(postId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              //this.ngOnInit();
              //this._router.navigate(["/detailedpost", this.post.PostId]);
              location.reload();
              //this._router.navigate(["/postscard"]);
            }
          }
          , (error) => {
            console.log("Error upvoting:", error);
          });
    }
  }

  setDownvote(postId: number): void {
    if (this.isSignedIn) {
      this._postService.downvotePost(postId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              //this.ngOnInit();
              //this._router.navigate(["/detailedpost", this.post.PostId]);
              location.reload();
              //this._router.navigate(["/postscard"]);
            }
          }
          , (error) => {
            console.log("Error downvoting:", error);
          });
    }
  }

}
