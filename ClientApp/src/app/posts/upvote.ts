export interface IUpvote {
  UpvoteId: number;
  Vote: string;
  PostId: number;
  UserId: number;
}
export class Upvote implements IUpvote {
    UpvoteId: number = 0;
    Vote: string = "";
    PostId: number = 0;
    UserId: number = 0;

}
