export interface IComment {
  CommentId: number;
  CommentText: string;
  Text: string;
  PostId: number;
  UserId: number;
  PostDate: string;
}
export class Comment implements IComment {
    CommentId: number = 0;
    CommentText: string = "";
    Text: string = "";
    PostId: number = 0;
    UserId: number = 0;
    PostDate: string = "";

}
