import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';
import { Member } from '../models/member.model';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  http = inject(HttpClient);

  private readonly _baseApiUrl: string = environment.apiUrl + 'api/member/';

  getAllMembers(): Observable<Member[]> {

    let members$: Observable<Member[]>
     = this.http.get<Member[]>(this._baseApiUrl);      

    return members$;
  }

  getByUserName(userNameInput: string): Observable<Member | undefined> {
    return this.http.get<Member>(this._baseApiUrl + 'get-by-username/' + userNameInput);
  }
}
