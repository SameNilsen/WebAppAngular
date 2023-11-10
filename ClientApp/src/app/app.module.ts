import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { PostsComponent } from './posts/posts.component';
import { PostformComponent } from './posts/postform.component';
import { DetailedPostComponent } from './posts/detailedpost.component';
import { SubForumPostsComponent } from './posts/subforumposts.component';
import { UserProfileComponent } from './user/userprofile.component';
import { simpleDateFormat } from './shared/formatDate.pipe';
import { CredsInfoComponent } from './user/credsinfo.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    PostsComponent,
    PostformComponent,
    DetailedPostComponent,
    SubForumPostsComponent,
    UserProfileComponent,
    simpleDateFormat,
    CredsInfoComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ApiAuthorizationModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'posts', component: PostsComponent },
      { path: "postform", component: PostformComponent },
      { path: "postform/:mode/:id", component: PostformComponent },
      { path: "detailedpost", component: DetailedPostComponent },
      { path: "detailedpost/:id", component: DetailedPostComponent },
      { path: "subforumposts", component: SubForumPostsComponent },
      { path: "subforumposts/:forum", component: SubForumPostsComponent },
      { path: "userprofile", component: UserProfileComponent },
      { path: "userprofile/:id", component: UserProfileComponent },
      { path: "credsinfo", component: CredsInfoComponent },
      { path: "**", redirectTo: "", pathMatch: "full" }
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
