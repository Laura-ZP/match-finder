import { HttpClient } from '@angular/common/http';
import { inject, Injectable, PLATFORM_ID, signal } from '@angular/core';
import { AppUser } from '../models/app-user.model';
import { map, Observable } from 'rxjs';
import { LoggedIn } from '../models/logged-in.model';
import { Login } from '../models/login.model';
import { Member } from '../models/member.model';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  http = inject(HttpClient);
  private readonly _baseApiUrl: string = 'http://localhost:5000/api/';
  platformId = inject(PLATFORM_ID);
  router = inject(Router);
  loggedInUserSig = signal<LoggedIn | null>(null);

  register(user: AppUser): Observable<LoggedIn | null> {
    return this.http.post<LoggedIn>(this._baseApiUrl + 'account/register', user).pipe(
      map(res => {
        if (res) {
          this.setCurrentUser(res);

          this.router.navigateByUrl('members');

          return res;
        }

        return null;
      })
    );
  }

  login(userInput: Login): Observable<LoggedIn | null> {
    return this.http.post<LoggedIn>(this._baseApiUrl + 'account/login', userInput).pipe(
      map(res => {
        if (res) {
          this.setCurrentUser(res);

          this.router.navigateByUrl('members');

          return res;
        }
        return null;
      })
    );
  }

  getAllMember(): Observable<Member[]> {
    return this.http.get<Member[]>(this._baseApiUrl + 'member');
  }

  authorizeLoggedInUser(): void {
    console.log('ok');

    this.http.get(this._baseApiUrl + 'aacount').subscribe({
      next: (res) => {
        if (res)
          console.log(res);
      },
      error: (err) => {
        console.log(err.error);
        this.logout();
      }
    });
  }

  setCurrentUser(loggedIn: LoggedIn): void {
    this.loggedInUserSig.set(loggedIn);
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('loggedInUser', JSON.stringify(loggedIn));
    }
  }

  logout(): void {
    this.loggedInUserSig.set(null);

    if (isPlatformBrowser(this.platformId)) {
      localStorage.clear();
    }

    this.router.navigateByUrl('account/login');
  }
}
