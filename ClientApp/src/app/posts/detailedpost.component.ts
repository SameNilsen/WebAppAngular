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
  commentForm: FormGroup;
  isYourPost: boolean = false;
  isSignedIn: boolean = false;
  yourVote: string = "blank";
  
  constructor(private _router: Router, private _postService: PostService, private _route: ActivatedRoute, private _userService: UserService, private _commentService: CommentService, private _formbuilder: FormBuilder) {
    this.commentForm = _formbuilder.group({
      commenttext: ["", Validators.required],
      commentid: [],
      userid: [],
      postid: [],
      postdate: [],      
    });
  }

  ngOnInit(): void {
    this._route.params.subscribe(params => {
      //this.post = params["id"]
      console.log("THIS IS ID: " + params["id"]);
      this.loadPost(params["id"]);
      //this.loadUser(this.post.UserId);
      this.getSignedIn(params["id"]);
      this.getVoting(params["id"]);
    });
  }

  getSignedIn(postId: number): void {
    this._postService.getSignedIn(postId).subscribe(response => {
      if (response.success) {
        console.log("signed in: " + response.message + " " + response.userspost);
        console.log("type: " + typeof response.userspost);
        this.isYourPost = response.userspost;
        this.isSignedIn = true;
        //this._router.navigate(["/posts"]);
      }
      else {
        console.log("Not signed in");
      }
    });
  }

  getVoting(postId: number): void {
    this._postService.getVote(postId).subscribe(response => {
      if (response.success) {
        console.log("vote: " + response.vote);
        this.yourVote = response.vote;
        if (this.yourVote == "upvote") {
          document.getElementById("upvoteButton")!.style.color = "rgb(193, 26, 26)";
          //document.getElementById("downvoteButton")!.style.color = "inherit";
        }
        else if (this.yourVote == "downvote") {
          document.getElementById("downvoteButton")!.style.color = "rgb(20, 130, 167)";
          //document.getElementById("upvoteButton")!.style.color = "transparent";
        }
      }
      else {
        console.log("Uffda");
        console.log(response.message);
      }
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

  toggleCommentBox(id: string): void {
    var div = document.getElementById(id)!;
    if (div.style.display == "block") {
      div.style.display = "none";
    }
    else {
      div.style.display = "block";
    }
  }
  //  Toggle between edit comment mode or view comment mode.
  toggleEdit(id: string): void {
    var divON = document.getElementById("editON " + id)!;
    var divOFF = document.getElementById("editOFF " + id)!;
    if (divON.style.display == "block") {
      divON.style.display = "none";
      divOFF.style.display = "block";
    }
    else {
      divON.style.display = "block";
      divOFF.style.display = "none";
    }
  }

  onSubmit() {
    if (!this.isSignedIn) { return; }
    console.log("CommentCreate form submitted:");
    console.log(this.commentForm);
    const newComment = this.commentForm.value;
    newComment.commentid = 0;
    newComment.postid = this.post.PostId;
    newComment.Post = this.post;
    newComment.userid = 2;
    newComment.User = this.post.user;
    console.log(newComment, newComment.PostID);
    //const createUrl = "api/item/create";
      this._commentService.createComment(newComment).subscribe(response => {
        if (response.success) {
          console.log(response.message);
          this._router.navigate(["/posts"]);
        }
        else {
          console.log("Comment creation failed");
        }
      });
  }

  deletePost(post: IPost): void {
    const confirmDelete = confirm(`Are you sure you want to delete "${post.Title}"?`);
    if (confirmDelete) {
      this._postService.deletePost(post.PostId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              this._router.navigate(["/posts"]);
            }
          }
          , (error) => {
            console.log("Error deleting post:", error);
          });
    }
  }

  readyUpdate(commentId: number) {
    //console.log("ya");
    this._commentService.getCommentById(commentId)
      .subscribe(
        (comment: any) => {
          console.log("retrived comment: ", comment);
          this.commentForm.patchValue({
            commenttext: comment.CommentText,
            commentid: comment.CommentID,
            postdate: comment.PostDate,
            userid: comment.UserId,
            postid: comment.PostID,
          });
        },
        (error: any) => {
          console.log("Error loading comment for edit:", error);
        }
      );
    //this.commentForm.patchValue({
    //  commenttext: text
    //});
  }

  onUpdate() {
    console.log("CommentCreate form submitted:");
    const newComment = this.commentForm.value;
    newComment.Post = this.post;
    newComment.User = this.post.user;
    console.log(newComment);
    this._commentService.updateComment(newComment.commentid, newComment).subscribe(response => {
      if (response.success) {
        console.log(response.message);
        this._router.navigate(["/posts"]);
      }
      else {
        console.log("Comment update failed");
      }
    });
  }

  deleteComment(comment: IComment): void {
    const confirmDelete = confirm(`Are you sure you want to delete "${comment.CommentID}"?`);
    if (confirmDelete) {
      this._commentService.deleteComment(comment.CommentID)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              this._router.navigate(["/posts"]);
            }
          }
          , (error) => {
            console.log("Error deleting comment:", error);
          });
    }
  }

  setUpvote(): void {
    if (this.isSignedIn) {
      this._postService.upvotePost(this.post.PostId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              //this.ngOnInit();
              //this._router.navigate(["/detailedpost", this.post.PostId]);
              this._router.navigate(["/posts"]);
            }
          }
          , (error) => {
            console.log("Error upvoting:", error);
          });
    }
  }

  setDownvote(): void {
    if (this.isSignedIn) {
      this._postService.downvotePost(this.post.PostId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              //this.ngOnInit();
              //this._router.navigate(["/detailedpost", this.post.PostId]);
              this._router.navigate(["/posts"]);
            }
          }
          , (error) => {
            console.log("Error downvoting:", error);
          });
    }
  }

  
}
