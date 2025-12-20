import { Component, inject, PLATFORM_ID } from '@angular/core';
import { MemberService } from '../../../services/member.service';
import { environment } from '../../../../environments/environment.development';
import { Member } from '../../../models/member.model';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { LoggedIn } from '../../../models/logged-in.model';
import { take } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { PhotoEditorComponent } from '../photo-editor/photo-editor.component';
@Component({
  selector: 'app-user-edit',
  imports: [
    MatCardModule, MatTabsModule, CommonModule, PhotoEditorComponent, MatButtonModule
  ],
  templateUrl: './user-edit.component.html',
  styleUrl: './user-edit.component.scss'
})
export class UserEditComponent {
private _platformId = inject(PLATFORM_ID);
private _memberService = inject(MemberService);

apiUrl = environment.apiUrl;
member: Member | undefined;

getMember(): void {
  if (isPlatformBrowser(this._platformId)) {
    const loggedInUserStr: string | null = localStorage.getItem('loggedInUser');

    if (loggedInUserStr) {
      const loggedInUser: LoggedIn = JSON.parse(loggedInUserStr);

      this._memberService.getByUserName(loggedInUser.userName).pipe(take(1)).subscribe({
        next: (res) => {
          this.member = res;
        }
      })
    }
  }
}
}
