import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { Observable } from 'rxjs';
import { MemberCardComponent } from '../member-card/member-card.component';
import { MemberService } from '../../../services/member.service';
import { Member } from '../../../models/member.model';

@Component({
  selector: 'app-member-list',
  imports: [
    MatCardModule, MatIconModule, MemberCardComponent
  ],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.scss'
})
export class MemberListComponent {
  memberService = inject(MemberService);
  members: Member[] | undefined;

  ngOnInit(): void {
    this.getAll();
  }

  getAll(): void {
    let allMember$: Observable<Member[]> = this.memberService.getAllMembers();

    allMember$.subscribe({
      next: (res) => {
        this.members = res;
        console.log(res);
      }
    });
  }
}
