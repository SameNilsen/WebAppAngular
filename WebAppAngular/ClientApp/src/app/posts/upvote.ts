import { IPost, Post } from "./post";

export interface IUpvote {
  UpvoteId: number;
  Vote: string;
  PostId: number;
  UserId: number;
  post: IPost;
}
export class Upvote implements IUpvote {
    UpvoteId: number = 0;
    Vote: string = "";
    PostId: number = 0;
    UserId: number = 0;
    post: IPost = new Post();
}
