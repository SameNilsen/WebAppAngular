import { Component } from "@angular/core";
import { FormGroup, FormControl, Validators, FormBuilder, ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { PostService } from "./posts.service";
import { IUser, User } from "../user/user";
import { Title } from "@angular/platform-browser";

@Component({
  selector: "app-posts-postform",
  templateUrl: "./postform.component.html",
})

export class PostformComponent {

  postForm: FormGroup;
  isEditMode: boolean = false;
  postId: number = -1;
  
  constructor(private _formbuilder: FormBuilder, private _router: Router, private _postService: PostService, private _route: ActivatedRoute) {
    this.postForm = _formbuilder.group({
      title: ["", Validators.required],
      text: ["", Validators.required],
      //subforum: [""],
      imageUrl: [""]
    });
  }

  onSubmit() {
    console.log("PostCreate form submitted:");
    console.log(this.postForm);
    const newPost = this.postForm.value;
    console.log(newPost);
    newPost.UserId = 4;
    newPost.User = new User();
    newPost.User.UserId = 4;
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
          this.postForm.patchValue({
            title: post.Title,
            text: post.Text,
            //subforum: post.SubForum,  // har ikke fiksa dropdownlist enda
            imageUrl: post.ImageUrl,
          });
        },
        (error: any) => {
          console.log("Error loading post for edit:", error);
        }
      );
  }
}
