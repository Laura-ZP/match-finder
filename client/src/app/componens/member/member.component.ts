import { Component, inject } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { Member } from '../../models/member.model';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-member',
  imports: [
    MatCardModule, MatIconModule
  ],
  templateUrl: './member.component.html',
  styleUrl: './member.component.scss'
})
export class MemberComponent {
  accountServise = inject(AccountService);
  members: Member[] | undefined;

  ngOnInit(): void {
    this.getAll();
  }

  getAll(): void {
    this.accountServise.getAllMember().subscribe({
      next: (res) => {
        this.members = res;
      }
    });
  }
}
