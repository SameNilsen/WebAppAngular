import { IUser, User } from "../user/user";

export interface IPost {
  PostId: number;
  Title: string;
  Text: string;
  UpvoteCount: number;
  PostDate: string;
  ImageUrl: string;
  SubForum: string;
  UserId: number;
  user: IUser;
}
export class Post implements IPost {
    PostId: number = 0;
    Title: string = "";
    Text: string = "";
    UpvoteCount: number = 0;
    PostDate: string = "";
    ImageUrl: string = "";
    SubForum: string = "";
    UserId: number = 0;
    user: IUser = new User();
}
