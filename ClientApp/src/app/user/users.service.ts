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

  updateUser(userId: number, newUser: any): Observable<any> {
    const url = `${this.baseUrl}/update/${userId}`;
    newUser.userId = userId;
    return this._http.put<any>(url, newUser);
  }

  deleteUser(UserId: number): Observable<any> {
    const url = `${this.baseUrl}/delete/${UserId}`;
    return this._http.delete(url);
  }
}
