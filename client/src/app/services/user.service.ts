import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Member } from '../models/member.model';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/helpers/apiResponse.model';
import { UserUpdate } from '../models/user-update.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private _http = inject(HttpClient);
  members: Member[] = [];
  private readonly _apiUrl = environment.apiUrl + 'api/user';

  updateUser(userInput: UserUpdate): Observable<ApiResponse> {
    return this._http.put<ApiResponse>(this._apiUrl + 'update', userInput);
  }

  setMainPhoto(url_165: string): Observable<ApiResponse> {
    return this._http.put<ApiResponse>(this._apiUrl + 'set-main-photo', null, { params: new HttpParams().set('photoUrlIn', url_165) });
  }

  deletePhoto(url_165In: string): Observable<ApiResponse> {
    let quertParams = new HttpParams().set('photoUrlIn', url_165In);

    return this._http.put<ApiResponse>(this._apiUrl + 'delete-photo', null, { params: quertParams });
  }
}
