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
  commentForm: FormGroup;         //  Form for editing comment
  commentCreateForm: FormGroup;   //  Form for creating comment.
  isYourPost: boolean = false;
  isSignedIn: boolean = false;
  signedInId: string = "";
  yourVote: string = "blank";
  
  constructor(private _router: Router, private _postService: PostService, private _route: ActivatedRoute, private _userService: UserService, private _commentService: CommentService, private _formbuilder: FormBuilder) {
    this.commentForm = _formbuilder.group({
      commenttext: ["", [Validators.required, Validators.minLength(2), Validators.maxLength(80)]],
      commentid: [],
      userid: [],
      postid: [],
      postdate: [],      
    });
    this.commentCreateForm = _formbuilder.group({
      commenttext: ["", [Validators.required, Validators.minLength(2), Validators.maxLength(80)]],
      commentid: [],
      userid: [],
      postid: [],
      postdate: [],
    });
  }

  get commenttext() {
    return this.commentCreateForm.get("commenttext")!;
  }

  //  Init method.
  ngOnInit(): void {
    this._route.params.subscribe(params => {
      //this.post = params["id"]
      console.log("THIS IS ID: " + params["id"]);
      this.loadPost(params["id"]);                 //  Load the post.
      //this.loadUser(this.post.UserId);
      this.getSignedIn(params["id"]);              //  Get user if signed in. 
      this.getVoting(params["id"]);                //  Get previous vote for this post.
    });
  }

  //  Check to see if the user is signed in, and if this is the users post. This is useful for showing or
  //   disabling various buttons. The signedInId is used for checking which (if any) comment belong to the
  //    user, so that only the comments user can edit it.
  getSignedIn(postId: number): void {
    this._postService.getSignedIn(postId).subscribe(response => {
      if (response.success) {
        console.log("signed in: " + response.message + " " + response.userspost);
        console.log("type: " + typeof response.userspost);
        this.isYourPost = response.userspost;
        this.isSignedIn = true;
        this.signedInId = response.message;
        //this._router.navigate(["/posts"]);
      }
      else {
        console.log("Not signed in");
      }
    });
  }

  //  Gets what the user has previously voted on this post. This is used to style the voting buttons.
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

  //  Method for loading post. After fetching the post, it calls on methods for loading user and comments.
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

  //  Fetches user.
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

  //  Fetching comments of the post.
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

  //  Method used for toggling visibility of comment box.
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

  //  Method for creating and submitting new comment.
  onSubmit() {
    if (!this.isSignedIn) { return; }  //  If the user is not logged in, abort.
    console.log("CommentCreate form submitted:");
    console.log(this.commentCreateForm);
    const newComment = this.commentCreateForm.value;
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
          location.reload();  //  Istedet for å redirecte et sted.
          //  Hvis man hadde redirecta til /detailedpost her, altså til samme side (samme url) så hadde
          //   ikke browseren oppdatert, siden den tror det bare er samme side. Prøver derfor med
          //    location.reload() istedet, håper ikke det tuller til alt.
          //this._router.navigate(["/posts"]);  
        }
        else {
          console.log("Comment creation failed");
        }
      });
  }

  //  Method for deleting post. Triggers a confirmation alert first.
  deletePost(post: IPost): void {
    const confirmDelete = confirm(`Are you sure you want to delete "${post.Title}"?`);
    if (confirmDelete) {
      this._postService.deletePost(post.PostId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              //location.reload();  //  Istedet for å redirecte et sted.
              this._router.navigate(["/posts"]);  //  Her må vi jo redirecte til posts.
            }
          }
          , (error) => {
            console.log("Error deleting post:", error);
          });
    }
  }

  //  Method for preparing comment for edit. When clicking edit button this method is called to fetch the
  //   comment to be edited. The values of the comment are patched in a form, and the commenttext is displayed
  //    in the edit comment field.
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
   //  Method for submitting edited comment.
  onUpdate() {
    console.log("CommentCreate form submitted:");
    const newComment = this.commentForm.value;
    newComment.Post = this.post;
    newComment.User = this.post.user;
    console.log(newComment);
    this._commentService.updateComment(newComment.commentid, newComment).subscribe(response => {
      if (response.success) {
        console.log(response.message);
        location.reload();  //  Istedet for å redirecte et sted.
        //this._router.navigate(["/posts"]);
      }
      else {
        console.log("Comment update failed");
      }
    });
  }

  //  Method to delete comment, similar to deleting post.
  deleteComment(comment: IComment): void {
    const confirmDelete = confirm(`Are you sure you want to delete "${comment.CommentID}"?`);
    if (confirmDelete) {
      this._commentService.deleteComment(comment.CommentID)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              location.reload();  //  Istedet for å redirecte et sted.
              //this._router.navigate(["/posts"]);
            }
          }
          , (error) => {
            console.log("Error deleting comment:", error);
          });
    }
  }
   //  Method called when clicking upvote button.
  setUpvote(): void {
    if (this.isSignedIn) {  //  Must be signed in to vote.
      this._postService.upvotePost(this.post.PostId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              //this.ngOnInit();
              //this._router.navigate(["/detailedpost", this.post.PostId]);
              location.reload();  //  Istedet for å redirecte et sted.
              //this._router.navigate(["/posts"]);
            }
          }
          , (error) => {
            console.log("Error upvoting:", error);
          });
    }
  }

   //  Method called when clicking upvote button.
  setDownvote(): void {
    if (this.isSignedIn) {  //  Must be signed in to vote.
      this._postService.downvotePost(this.post.PostId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              //this.ngOnInit();
              //this._router.navigate(["/detailedpost", this.post.PostId]);
              location.reload();  //  Istedet for å redirecte et sted.
              //this._router.navigate(["/posts"]);
            }
          }
          , (error) => {
            console.log("Error downvoting:", error);
          });
    }
  }

  
}
