import { IUser, User } from "../user/user";
import { IPost, Post } from "./post";

export interface IComment {
  CommentID: number;
  CommentText: string;
  PostId: number;
  UserId: number;
  PostDate: string;
  user: IUser;
  post: IPost;
}
export class Comment implements IComment {
    CommentID: number = 0;
    CommentText: string = "";
    PostId: number = 0;
    UserId: number = 0;
    PostDate: string = "";
    user: IUser = new User();
    post: IPost = new Post();
}
