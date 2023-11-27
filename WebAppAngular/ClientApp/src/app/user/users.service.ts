import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { IUser } from "./user";

@Injectable({
  providedIn: "root"
})
export class UserService {

  private baseUrl = "api/user/";

  constructor(private _http: HttpClient) { }

  //  Service Injection class which handles http request calls to the server. The baseUrl
  //   string specifies which apiController to be used.

  getUsers(): Observable<IUser[]> {
    return this._http.get<IUser[]>(this.baseUrl);
  }

  createUser(newUser: IUser): Observable<any> {
    const createUrl = "api/user/create";
    return this._http.post<any>(createUrl, newUser);
  }

  getUserById(userId: number): Observable<any> {
    const url = `${this.baseUrl}/${userId}`;
    return this._http.get(url);
  }

  getSimplifiedUser(userId: number): Observable<any> {
    const url = `${this.baseUrl}simpleuser/${userId}`;
    return this._http.get(url);
  }

  getUserIdByIdentity(identityId: string): Observable<any> {
    const url = `${this.baseUrl}getuseridbyidentity/${identityId}`;
    return this._http.get(url);
  }

  updateUser(userId: number, newUser: any): Observable<any> {
    const url = `${this.baseUrl}/update/${userId}`;
    newUser.userId = userId;
    return this._http.put<any>(url, newUser);
  }

  deleteUser(UserId: number): Observable<any> {
    const url = `${this.baseUrl}/delete/${UserId}`;
    return this._http.delete(url);
  }

  getPosts(id: number): Observable<any> {
    const url = `${this.baseUrl}posts/${id}`;
    return this._http.get<any>(url);
  }

  getComments(id: number): Observable<any> {
    const url = `${this.baseUrl}comments/${id}`;
    return this._http.get<any>(url);
  }

  getVotes(id: number): Observable<any> {
    const url = `${this.baseUrl}votes/${id}`;
    return this._http.get<any>(url);
  }
}
