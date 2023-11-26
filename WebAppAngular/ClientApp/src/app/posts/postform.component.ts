import { Component } from "@angular/core";
import { FormGroup, FormControl, Validators, FormBuilder, ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { PostService } from "./posts.service";
import { IUser, User } from "../user/user";

@Component({
  selector: "app-posts-postform",
  templateUrl: "./postform.component.html",
})

export class PostformComponent {

  isSignedIn = false;
  postForm: FormGroup;
  isEditMode: boolean = false;
  postId: number = -1;
  private _selectedForum: string = "";
  get selectedForum(): string {
    return this._selectedForum;
  }
  set selectedForum(value: string) {
    this._selectedForum = value;
    console.log("In setter:", value);
  }
  subforums = [
    { name: "Gaming" },
    { name: "Sport" },
    { name: "School" },
    { name: "Nature" },
    { name: "Politics" },
    { name: "General" }
  ];
  
  constructor(private _formbuilder: FormBuilder, private _router: Router, private _postService: PostService, private _route: ActivatedRoute) {
    this.postForm = _formbuilder.group({
      title: ["", [Validators.required, Validators.minLength(2), Validators.maxLength(80), Validators.pattern("[0-9a-zA-Zæøå.,!? \-]{0,80}")]],
      text: ["", [Validators.required, Validators.minLength(4), Validators.maxLength(200)]],
      upvotecount: [0],
      postdate: [""],
      //subforum: [""],
      imageUrl: [""]
    });
    this.selectedForum = this.subforums[5]['name'];
  }

  get title() {
    return this.postForm.get("title")!;
  }
  get text() {
    return this.postForm.get("text")!;
  }

  onSubmit() {
    console.log("PostCreate form submitted:");
    console.log(this.postForm);
    const newPost = this.postForm.value;
    console.log("New post1: " + newPost);
    newPost.UserId = 4;
    newPost.User = new User();
    newPost.User.UserId = 4;
    newPost.SubForum = this.selectedForum;
    console.log("New post2: " + newPost);
    //const createUrl = "api/item/create";
    if (this.isEditMode) {
      this._postService.updatePost(this.postId, newPost).subscribe(response => {
        if (response.success) {
          console.log(response.message);
          this._router.navigate(["/posts"]);
        }
        else {
          console.log("Post update failed");
        }
      });
    }
    else {
      this._postService.createPost(newPost).subscribe(response => {
        if (response.success) {
          console.log(response.message);
          this._router.navigate(["/posts"]);
        }
        else {
          console.log("Post creation failed");
        }
      });
    }
  }

  backToPosts() {
    this._router.navigate(["/posts"]);
  }

  ngOnInit(): void {
    this._route.params.subscribe(params => {
      this.getSignedIn(params["id"]);
      if (params["mode"] === "create") {
        this.isEditMode = false; // create mode
      }
      else if (params["mode"] === "edit") {
        this.isEditMode = true; // edit mode
        this.postId = +params["id"]; // convert to number
        this.loadPostForEdit(this.postId);
      }
    });
  }

  loadPostForEdit(postId: number) {
    this._postService.getPostById(postId)
      .subscribe(
        (post: any) => {
          console.log("retrived post: ", post);
          console.log("--" + this.subforums.findIndex(x => x.name === post.SubForum));
          this.selectedForum = this.subforums[this.subforums.findIndex(x => x.name === post.SubForum)]["name"];
          this.postForm.patchValue({
            title: post.Title,
            text: post.Text,
            upvotecount: post.UpvoteCount,
            postdate: post.PostDate,
            //subforum: post.SubForum,  // har ikke fiksa dropdownlist enda
            imageUrl: post.ImageUrl,
          });
        },
        (error: any) => {
          console.log("Error loading post for edit:", error);
        }
      );
  }

  getSignedIn(postId: number): void {
    this._postService.getSignedIn(postId).subscribe(response => {
      if (response.success) {
        console.log("signed in: " + response.message + " " + response.userspost);
        this.isSignedIn = true;
      }
      else {
        console.log("Not signed in");
      }
    });
  }
}
