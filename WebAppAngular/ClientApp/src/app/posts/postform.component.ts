import { Component } from "@angular/core";
import { FormGroup, FormControl, Validators, FormBuilder, ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { PostService } from "./posts.service";
import { IUser, User } from "../user/user";

@Component({
  selector: "app-posts-postform",
  templateUrl: "./postform.component.html",
})

export class PostformComponent {  //  Class for both creating and updating posts.

  isSignedIn = false;
  postForm: FormGroup;
  isEditMode: boolean = false;
  postId: number = -1;
  //  Getters and setters for selectedForum (dropdownlist).
  private _selectedForum: string = "";
  get selectedForum(): string {
    return this._selectedForum;
  }
  set selectedForum(value: string) {
    this._selectedForum = value;
    console.log("In setter:", value);
  }
  //  The different categories that make up the subforums.
  subforums = [
    { name: "Gaming" },
    { name: "Sport" },
    { name: "School" },
    { name: "Nature" },
    { name: "Politics" },
    { name: "General" }
  ];
  
  constructor(private _formbuilder: FormBuilder, private _router: Router, private _postService: PostService, private _route: ActivatedRoute) {
    //  group the form.
    this.postForm = _formbuilder.group({
      title: ["", [Validators.required, Validators.minLength(2), Validators.maxLength(80), Validators.pattern("[0-9a-zA-Zæøå.,!? \-]{0,80}")]],
      text: ["", [Validators.required, Validators.minLength(4), Validators.maxLength(200)]],
      upvotecount: [0],
      postdate: [""],
      imageUrl: [""]
    });
    this.selectedForum = this.subforums[5]['name'];  //  Default set to number 5: General.
  }

  get title() {
    return this.postForm.get("title")!;
  }
  get text() {
    return this.postForm.get("text")!;
  }

  //  When submitting the form, either to create or update.
  onSubmit() {
    console.log("PostCreate form submitted:");
    console.log(this.postForm);
    const newPost = this.postForm.value;
    //  Set some values not set by form. Will be revised in controller.
    newPost.UserId = 4;
    newPost.User = new User();
    newPost.User.UserId = 4;
    newPost.SubForum = this.selectedForum;
    if (this.isEditMode) {  //  If updating:
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
    else {  //  If creating:
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
  //  Back button.
  backToPosts() {
    this._router.navigate(["/posts"]);
  }

  ngOnInit(): void {  //  Called on initilization.
    this._route.params.subscribe(params => {
      this.getSignedIn(params["id"]);  //  Call on function to see if signed in.
      if (params["mode"] === "create") {
        this.isEditMode = false; // create mode
      }
      else if (params["mode"] === "edit") {
        this.isEditMode = true; // edit mode
        this.postId = +params["id"]; // convert to number
        this.loadPostForEdit(this.postId);  //  Patch post values.
      }
    });
  }

  //  When updating this is called to patch the values of the post into the form.
  loadPostForEdit(postId: number) {
    this._postService.getPostById(postId)
      .subscribe(
        (post: any) => {
          console.log("Retrived post: ", post);
          //  Set correct option in dropdownlist.
          this.selectedForum = this.subforums[this.subforums.findIndex(x => x.name === post.SubForum)]["name"];
          this.postForm.patchValue({
            title: post.Title,
            text: post.Text,
            upvotecount: post.UpvoteCount,
            postdate: post.PostDate,
            imageUrl: post.ImageUrl,
          });
        },
        (error: any) => {
          console.log("Error loading post for edit:", error);
        }
      );
  }

  //  Function to see if the user is signed in. Should only be able to create or update if signed in.
  getSignedIn(postId: number): void {
    this._postService.getSignedIn(postId).subscribe(response => {
      if (response.success) {
        console.log("signed in: " + response.message + " " + response.userspost);
        this.isSignedIn = true;  //  Yay signed in.
      }
      else {
        console.log("Not signed in");
      }
    });
  }
}
