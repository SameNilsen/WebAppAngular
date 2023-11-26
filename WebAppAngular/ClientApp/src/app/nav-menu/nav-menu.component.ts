import { Component, OnInit } from '@angular/core';
import { PostService } from "../posts/posts.service";
import { UserService } from "../user/users.service";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  isSignedIn: boolean = false;
  userId: number = -1;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  constructor(private _postService: PostService, private _userService: UserService, private _route: ActivatedRoute) { }

  ngOnInit(): void {
    //document.body.setAttribute('data-bs-theme', "dark");
    //document.documentElement.setAttribute('data-bs-theme', "dark");
    this._route.params.subscribe(params => {
      this.getSignedIn();
    });
  }

  getSignedIn(): void {
    this._postService.getSignedIn(-1).subscribe(response => {
      if (response.success) {
        console.log("signed in: " + response.message + " " + response.userspost);
        //console.log("asda" + this.user.IdentityUserId);
        this.isSignedIn = true;
        this.getUserId(response.message);
      }
      else {
        console.log("Not signed in");
      }
    });
  }

  getUserId(identityId: string): void {
    this._userService.getUserIdByIdentity(identityId).subscribe(response => {
      console.log("UserId: " + response)
      this.userId = response;
    });
  }
}
