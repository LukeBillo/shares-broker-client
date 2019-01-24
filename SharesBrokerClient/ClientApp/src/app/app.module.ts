import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { AuthenticateService } from './authentication/authenticate.service';
import { AuthGuard } from './authentication/authenticate.guard';
import { UnauthorizedInterceptor } from './authentication/unauthorized.interceptor';
import { BasicAuthInterceptor } from './authentication/auth.interceptor';
import { SharesService } from './shares/shares.service';
import { UserSharesService } from './shares/user-shares.service';
import { BuyShareComponent } from './buy-share/buy-share.component';
import { SearchComponent } from './search/search.component';
import { Ng5SliderModule } from 'ng5-slider';
import { SelectDropDownModule } from 'ngx-select-dropdown';
import { DisplaySharesComponent } from './display-shares/display-shares.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    BuyShareComponent,
    SearchComponent,
    DisplaySharesComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'buy', component: BuyShareComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'search', component: SearchComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'display', component: DisplaySharesComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'login', component: LoginComponent, pathMatch: 'full' }
    ]),
    Ng5SliderModule,
    SelectDropDownModule
  ],
  providers: [
    AuthenticateService,
    SharesService,
    UserSharesService,
    AuthGuard,
    { provide: HTTP_INTERCEPTORS, useClass: UnauthorizedInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: BasicAuthInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
