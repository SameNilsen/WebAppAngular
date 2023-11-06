import { IUser, User } from "../user/user";

export interface IComment {
  CommentID: number;
  CommentText: string;
  Text: string;
  PostId: number;
  UserId: number;
  PostDate: string;
  user: IUser;
}
export class Comment implements IComment {
    CommentID: number = 0;
    CommentText: string = "";
    Text: string = "";
    PostId: number = 0;
    UserId: number = 0;
    PostDate: string = "";
    user: IUser = new User();
}
