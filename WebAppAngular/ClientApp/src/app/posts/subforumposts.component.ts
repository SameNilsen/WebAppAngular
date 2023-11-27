import { Component, OnInit } from "@angular/core";
import { IPost } from "./post";
import { IUser } from "../user/user";
import { filter } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { ActivatedRoute, Router } from "@angular/router";
import { PostService } from "./posts.service";
import { UserService } from "../user/users.service";


@Component({
  selector: "app-posts-subforumposts",
  templateUrl: "./subforumposts.component.html"
})

export class SubForumPostsComponent implements OnInit{

  viewTitle: string = "SubForumTable";
  displayImage: boolean = true;
  //  Getters and setters for filter.
  private _listFilter: string = "";
  get listFilter(): string {
    return this._listFilter;
  }
  set listFilter(value: string) {
    this._listFilter = value;
    console.log("In setter:", value);
    this.filteredPosts = this.performFilter(value);
  }

  //  Getters and setters for selected forum.
  private _selectedForum: string = "";
  get selectedForum(): string {
    return this._selectedForum;
  }
  set selectedForum(value: string) {
    this._selectedForum = value;
    console.log("In setter:", value);
    //  When the user selects a forum, this is called and directly redirects with the selected forum
    //   as parameter.
    //this.getPosts(value);   //  Doesnt change url :(
    this._router.navigate(["/subforumposts", value]);  //  This does.
  }
  //  The available forums.
  subforums = [
    { name: "Gaming" },
    { name: "Sport" },
    { name: "School" },
    { name: "Nature" },
    { name: "Politics" },
    { name: "General" }
  ];

  posts: IPost[] = [];
  users: IUser[] = [];

  constructor(private _router: Router, private _postService: PostService, private _userService: UserService, private _route: ActivatedRoute) { }

  //  Method to fetch all posts belonging to the selected forum.
  getPosts(forum: string): void {
    this._postService.getSubforumPosts(forum)
      .subscribe(data => {
        console.log("All", JSON.stringify(data));  //  Log response.
        this.posts = data;
        this.filteredPosts = this.posts;
      }
    );
    console.log("it worked?");
  }

  //  Gets the users of the posts.
  getUsers(): void {
    this._userService.getUsers()
      .subscribe(data => {
        console.log("All", JSON.stringify(data));
        this.users = data;
      }
      );
  }

  filteredPosts: IPost[] = this.posts;

  //  Function for filtering the posts.
  performFilter(filterBy: string): IPost[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.posts.filter((post: IPost) =>
      post.Title.toLocaleLowerCase().includes(filterBy));  // Actually filters.
  }

  //  Toggle on and of visibility of filter input box.
  toggleFilter(id: string): void {
    var div = document.getElementById(id)!;
    if (div.style.display == "block") {
      div.style.display = "none";
    }
    else {
      div.style.display = "block";
    }
  }

  ngOnInit(): void {
    this._route.params.subscribe(params => {
      //  Upon initialztion the subforum parameter from url is extracted. We then find the
      //   corresponding value in the subforums list, and sets this as the selected forum in
      //    dropdownlist. Then the posts relating to that subforum is fetched by getPosts() method.
      this.selectedForum = this.subforums[this.subforums.findIndex(x => x.name === params["forum"])]["name"];
      this.getPosts(params["forum"]);  //  Gets posts.
    });
  }

}
