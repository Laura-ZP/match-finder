import { Routes } from '@angular/router';
import { HomeComponent } from './componens/home/home.component';
import { RegisterComponent } from './componens/account/register/register.component';
import { LoginComponent } from './componens/account/login/login.component';
import { FooterComponent } from './componens/footer/footer.component';
import { NavbarComponent } from './componens/navbar/navbar.component';
import { MemberComponent } from './componens/member/member.component';
import { NotFoundComponent } from './componens/not-found/not-found.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'account/register', component: RegisterComponent },
    { path: 'account/login', component: LoginComponent },
    { path: 'footer', component: FooterComponent },
    { path: 'navbar', component: NavbarComponent },
    { path: 'members', component: MemberComponent, canActivate: [authGuard] },
    { path: '**', component: NotFoundComponent }
];
