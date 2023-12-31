import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { IComment } from "./comment";

@Injectable({
  providedIn: "root"
})
export class CommentService {

  private baseUrl = "api/comment/";

  constructor(private _http: HttpClient) { }

  //  Service Injection class which handles http request calls to the server. The baseUrl
  //   string specifies which apiController to be used.

  getComments(): Observable<IComment[]> {
    return this._http.get<IComment[]>(this.baseUrl);
  }

  createComment(newComment: IComment): Observable<any> {
    const createUrl = "api/comment/create";
    return this._http.post<any>(createUrl, newComment);
  }

  getCommentById(commentId: number): Observable<any> {
    const url = `${this.baseUrl}/${commentId}`;
    return this._http.get(url);
  }

  getCommentsByPostId(postId: number): Observable<IComment[]> {
    const url = `${this.baseUrl}/get/${postId}`;
    return this._http.get<IComment[]>(url);
  }

  updateComment(commentId: number, newComment: any): Observable<any> {
    const url = `${this.baseUrl}/update/${commentId}`;
    newComment.commentId = commentId;
    return this._http.put<any>(url, newComment);
  }

  deleteComment(CommentId: number): Observable<any> {
    const url = `${this.baseUrl}/delete/${CommentId}`;
    return this._http.delete(url);
  }
}
