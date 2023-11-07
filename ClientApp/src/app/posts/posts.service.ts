import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { IPost } from "./post";
//import { PostListViewModel } from "../../../../ViewModels/PostListViewModel";

@Injectable({
  providedIn: "root"
})
export class PostService {

  private baseUrl = "api/post/";

  constructor(private _http: HttpClient) { }

  getPosts(): Observable<IPost[]> {
    console.log("postsservice1" + this.baseUrl);
    return this._http.get<IPost[]>(this.baseUrl);
  }

  getSubforumPosts(forum: string): Observable<any> {
    const url = `${this.baseUrl}subforum/${forum}`;
    console.log(url + " hello");
    return this._http.get<any>(url);
  }

  createPost(newPost: IPost): Observable<any> {
    console.log(newPost);
    const createUrl = "api/post/create";
    return this._http.post<any>(createUrl, newPost);
  }

  getPostById(postId: number): Observable<any> {
    const url = `${this.baseUrl}/${postId}`;
    return this._http.get(url);
  }

  updatePost(postId: number, newPost: any): Observable<any> {
    const url = `${this.baseUrl}update/${postId}`;
    newPost.postId = postId;
    console.log(url, newPost);
    return this._http.put<any>(url, newPost);
  }

  deletePost(itemId: number): Observable<any> {
    const url = `${this.baseUrl}/delete/${itemId}`;
    return this._http.delete(url);
  }

  getSignedIn(postId: number): Observable<any> {
    //const createUrl = "api/post/signedin";
    const url = `${this.baseUrl}signedin/${postId}`;
    return this._http.get<any>(url);
  }

  upvotePost(postId: number): Observable<any> {
    const url = `${this.baseUrl}upvote/${postId}`;
    console.log(url);
    return this._http.get<any>(url);
  }

  downvotePost(postId: number): Observable<any> {
    const url = `${this.baseUrl}downvote/${postId}`;
    console.log(url);
    return this._http.get<any>(url);
  }

  getVote(postId: number): Observable<any> {
    const url = `${this.baseUrl}getvote/${postId}`;
    console.log(url);
    return this._http.get<any>(url);
  }
}
