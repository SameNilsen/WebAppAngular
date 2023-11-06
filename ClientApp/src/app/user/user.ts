import { IComment, Comment } from "../posts/comment";

export interface IUser {
  UserId: number;
  Name: string;
  IdentityUserId: string;
  Credebility: number;
  //Comment: IComment;
}
export class User implements IUser {
    UserId: number = 0;
    Name: string = "";
    IdentityUserId: string = "";
    Credebility: number = 0;
    //Comment: IComment = new Comment();
}
