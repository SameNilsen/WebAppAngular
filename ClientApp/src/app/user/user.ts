import { IComment, Comment } from "../posts/comment";

export interface IUser {
  UserId: number;
  Name: string;
  IdentityUserId: string;
  Credibility: number;
  Comment: IComment;
}
export class User implements IUser {
    UserId: number = 0;
    Name: string = "";
    IdentityUserId: string = "";
    Credibility: number = 0;
    Comment: IComment = new Comment();
}
