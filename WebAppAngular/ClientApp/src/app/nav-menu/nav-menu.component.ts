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
    this._route.params.subscribe(params => {
      this.getSignedIn();
    });
  }

  //  Check to see if user is signed in. Used for toggling visibility of myProfile page.
  getSignedIn(): void {
    this._postService.getSignedIn(-1).subscribe(response => {
      if (response.success) {
        console.log("signed in: " + response.message + " " + response.userspost);
        this.isSignedIn = true;
        this.getUserId(response.message);  //  The response.message is the identityUserId.
      }
      else {
        console.log("Not signed in");
      }
    });
  }

  //  Get userId. This for the myProfile button in the navbar which takes in the userId as
  //   argument/paramater.
  getUserId(identityId: string): void {
    this._userService.getUserIdByIdentity(identityId).subscribe(response => {
      console.log("UserId: " + response)
      this.userId = response;
    });
  }
}
