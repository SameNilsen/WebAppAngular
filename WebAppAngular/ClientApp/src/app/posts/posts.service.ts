import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { IPost } from "./post";

@Injectable({
  providedIn: "root"
})
export class PostService {

  private baseUrl = "api/post/";

  constructor(private _http: HttpClient) { }

  //  Service Injection class which handles http request calls to the server. The baseUrl
  //   string specifies which apiController to be used.

  getPosts(): Observable<IPost[]> {
    return this._http.get<IPost[]>(this.baseUrl);
  }

  getSubforumPosts(forum: string): Observable<any> {
    const url = `${this.baseUrl}subforum/${forum}`;
    return this._http.get<any>(url);
  }

  createPost(newPost: IPost): Observable<any> {
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
    return this._http.put<any>(url, newPost);
  }

  deletePost(itemId: number): Observable<any> {
    const url = `${this.baseUrl}/delete/${itemId}`;
    return this._http.delete(url);
  }

  getSignedIn(postId: number): Observable<any> {
    const url = `${this.baseUrl}signedin/${postId}`;
    return this._http.get<any>(url);
  }

  upvotePost(postId: number): Observable<any> {
    const url = `${this.baseUrl}upvote/${postId}`;
    return this._http.get<any>(url);
  }

  downvotePost(postId: number): Observable<any> {
    const url = `${this.baseUrl}downvote/${postId}`;
    return this._http.get<any>(url);
  }

  getVote(postId: number): Observable<any> {
    const url = `${this.baseUrl}getvote/${postId}`;
    return this._http.get<any>(url);
  }
}
