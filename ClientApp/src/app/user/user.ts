export interface IUser {
  UserId: number;
  Name: string;
  IdentityUserId: string;
  Credibility: number;
}
export class User implements IUser {
    UserId: number = 0;
    Name: string = "";
    IdentityUserId: string = "";
    Credibility: number = 0;

}
