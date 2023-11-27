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
    //  Create/group the form used for editing comment.
    this.commentForm = _formbuilder.group({
      commenttext: ["", [Validators.required, Validators.minLength(2), Validators.maxLength(80)]],
      commentid: [],
      userid: [],
      postid: [],
      postdate: [],      
    });
    //  Create/group the form used for creating comment.
    this.commentCreateForm = _formbuilder.group({
      commenttext: ["", [Validators.required, Validators.minLength(2), Validators.maxLength(80)]],
      commentid: [],
      userid: [],
      postid: [],
      postdate: [],
    });
  }

  //  Getter method for the commenttext.
  get commenttext() {
    return this.commentCreateForm.get("commenttext")!;
  }

  //  Init method.
  ngOnInit(): void {
    this._route.params.subscribe(params => {
      
      console.log("THIS IS POSTID: " + params["id"]);
      this.loadPost(params["id"]);                 //  Load the post.
      
      this.getSignedIn(params["id"]);              //  Get user if signed in. 
      this.getVoting(params["id"]);                //  Get previous vote for this post.
    });
  }

  //  Check to see if the user is signed in, and if this is the users post. This is useful for showing or
  //   disabling various buttons. The signedInId is used for checking which (if any) comment belong to the
  //    user, so that only the comments user can edit it.
  getSignedIn(postId: number): void {
    this._postService.getSignedIn(postId).subscribe(response => {    //  Use the postservice to perform request.
      if (response.success) {
        console.log("Signed in: true, Id: " + response.message + ", userspost: " + response.userspost);  //  Logging successful response. 
        this.isYourPost = response.userspost;   //  Variable to store wether this post is or isnt the signed in users post.
        this.isSignedIn = true;                 //  The user is signed in.
        this.signedInId = response.message;     //  This is the IdentityUserId from the ApplicationUser.
      }
      else {
        console.log("Not signed in");  //  Log to console.
      }
    });
  }

  //  Gets what the user has previously voted on this post. This is used to style the voting buttons.
  getVoting(postId: number): void {
    this._postService.getVote(postId).subscribe(response => {
      if (response.success) {
        console.log("Previous vote: " + response.vote);  //  Log the users previous vote.
        this.yourVote = response.vote;
        if (this.yourVote == "upvote") {
          document.getElementById("upvoteButton")!.style.color = "rgb(193, 26, 26)"; //  Previously upvoted: Red upvote button.
        }
        else if (this.yourVote == "downvote") {
          document.getElementById("downvoteButton")!.style.color = "rgb(20, 130, 167)"; //  Previously downvoted: blue downvote button.          
        }
      }
      else {
        console.log("Error occured while fetching vote.");  //  Log error message.
        console.log(response.message);
      }
    });
  }

  //  Method for loading post. After fetching the post, it calls on methods for loading user and comments.
  loadPost(postId: number) {
    this._postService.getPostById(postId)
      .subscribe(      //  Warning message saying that "subscribe" is deprecated, but it is whats used in demos.
        (post: any) => {
          console.log("retrived post: ", post);  //  Logs to console the retrived post.
          this.post = post;
          console.log("This is title: ", post.Title);
          this.loadUser(this.post.UserId);  //  When post is loaded, we can load the user aswell.
          //  In project 1 where we didnt have angular, we would probably set comments = post.Comments, but now we use separate function.
          this.getComments(postId); 
        },
        (error: any) => {
          console.log("Error loading post:", error);  //  Log error message.
        }
      );
  }

  //  Fetches user.
  loadUser(userId: number) {
    this._userService.getUserById(userId)
      .subscribe(
        (user: any) => {
          console.log("retrived user: ", user);  //  Logging user.
          this.user = user;
          console.log("This is posts user: ", user.Name);
        },
        (error: any) => {
          console.log("Error loading user:", error);  //  Logging error.
        }
      );
  }

  //  Fetching comments of the post.
  getComments(postId: number): void {
    this._commentService.getCommentsByPostId(postId)  //  Use commentservice to fetch all comments related to this post.
      .subscribe(data => {
        console.log("AllComments", JSON.stringify(data));  //  Log response.
        this.comments = data;
      },
        (error: any) => {
          console.log("Error loading comments:", error);  //  Log error message.
        }
      );
  }

  //  Method used for toggling visibility of comment box.
  toggleCommentBox(id: string): void {
    var div = document.getElementById(id)!;
    //  Sets display style of the comment box in html code to either block or none (visible or non visible).
    if (div.style.display == "block") {
      div.style.display = "none";
    }
    else {
      div.style.display = "block";
    }
  }
  //  Toggle between edit comment mode or view comment mode.
  toggleEdit(id: string): void {
    //  Since the comments (and the html elements showing them) are used in a loop, we use this
    //   editOn + id to find the correct element to toggle. The id is the comments commentId.
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
    console.log("CommentCreate form submitted:");  //  Log succesful creation of form.
    console.log(this.commentCreateForm);
    const newComment = this.commentCreateForm.value;  //  Retrive the comment from form.
    //  Not all attributes are set by form, but are required to be set before sending to server. We therefore set some
    //   dummy values on client side, before they are sent and then properly set in controller.
    newComment.commentid = 0;
    newComment.postid = this.post.PostId;
    newComment.Post = this.post;
    newComment.userid = 2;
    newComment.User = this.post.user;
    
      this._commentService.createComment(newComment).subscribe(response => {  //  Create comment via commentservice.
        if (response.success) {
          console.log(response.message);  //  Log response message.
          location.reload();  //  Istedet for å redirecte et sted.
          //  Normally we would use this._router.navigate(['/detailedpost']); to redirect to the same page.
          //   Unfortunatly when redirecting to the same url as its currently at, the browser thinks that
          //   nothing has changed and wont reload the page, and so the new comment wont be shown. Instead
          //   we use location.reload() to manually reload page.
                 }
        else {
          console.log("Comment creation failed");  //  Log failed creation of comment.
        }
      });
  }

  //  Method for deleting post. Triggers a confirmation alert first.
  deletePost(post: IPost): void {
    const confirmDelete = confirm(`Are you sure you want to delete "${post.Title}"?`);  //  Confirmation alert.
    if (confirmDelete) {  //  If user clicks 'ok', proceed.
      this._postService.deletePost(post.PostId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);  //  Logs confirmation message upon deletion.
              this._router.navigate(["/posts"]);  //  Redirects back to posts page, as this post no longer exist.
            }
          }
          , (error) => {
            console.log("Error deleting post:", error);  //  Log error message.
          });
    }
  }

  //  Method for preparing comment for edit. When clicking edit button this method is called to fetch the
  //   comment to be edited. The values of the comment are patched in a form, and the commenttext is displayed
  //    in the edit comment field.
  readyUpdate(commentId: number) {
    //  First use commentservice to get the comment from server.
    this._commentService.getCommentById(commentId)
      .subscribe(
        (comment: any) => {
          console.log("retrived comment: ", comment);  //  Log the retrived comment.
          this.commentForm.patchValue({  //  Patch the values from comment to form. When editing the comment the commenttext is loaded in so user can edit directly.
            commenttext: comment.CommentText,
            commentid: comment.CommentID,
            postdate: comment.PostDate,
            userid: comment.UserId,
            postid: comment.PostID,
          });
        },
        (error: any) => {
          console.log("Error loading comment for edit:", error);  //  Log error message.
        }
      );
   
  }
   //  Method for submitting edited comment.
  onUpdate() {
    console.log("CommentCreate form submitted:");
    const newComment = this.commentForm.value;
    //  Again, not all attributes are set automatically, but are set here. 
    newComment.Post = this.post;
    newComment.User = this.post.user;
    this._commentService.updateComment(newComment.commentid, newComment).subscribe(response => {  //  Http request to server via commentservice.
      if (response.success) {
        console.log(response.message);
        location.reload();  //  Reloads page.
      }
      else {
        console.log("Comment update failed");  //  Log error message.
      }
    });
  }

  //  Method to delete comment, similar to deleting post.
  deleteComment(comment: IComment): void {
    const confirmDelete = confirm(`Are you sure you want to delete your comment"?`);  //  Confirmation alert message.
    if (confirmDelete) {
      this._commentService.deleteComment(comment.CommentID)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              location.reload();  //  Reload page so comment is gone.
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
              console.log(response.message);  //  Log response.
              location.reload();  //  When successfully voted, reload to show changes in upvote count and votebutton colors.
            }
          }
          , (error) => {
            console.log("Error upvoting:", error);  //  Log error.
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
              console.log(response.message);  //  Log response.
              location.reload();  //  When successfully voted, reload to show changes in upvote count and votebutton colors.
            }
          }
          , (error) => {
            console.log("Error downvoting:", error);  //  Log error.
          });
    }
  }

  
}
