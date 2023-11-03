import { Component } from "@angular/core";
import { FormGroup, FormControl, Validators, FormBuilder, ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { PostService } from "./posts.service";
import { IPost, Post } from "./post";

@Component({
  selector: "app-posts-detailedpost",
  templateUrl: "./detailedpost.component.html",
})

export class DetailedPostComponent {

  post: IPost = new Post();
  
  constructor(private _router: Router, private _postService: PostService, private _route: ActivatedRoute) {
    
  }

  ngOnInit(): void {
    this._route.params.subscribe(params => {
      //this.post = params["id"]
      console.log("THIS IS ID: " + params["id"]);
      this.loadPost(params["id"]);
    });
  }

  loadPost(postId: number) {
    this._postService.getPostById(postId)
      .subscribe(
        (post: any) => {
          console.log("retrived post: ", post);
          this.post = post;
          console.log("This is title: ", post.Title);
        },
        (error: any) => {
          console.log("Error loading post:", error);
        }
      );
  }

  
}
