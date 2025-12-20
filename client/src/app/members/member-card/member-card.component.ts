import { Component, input } from '@angular/core';
import { Member } from '../../models/member.model';

@Component({
  selector: 'app-member-card',
  imports: [],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.scss'
})
export class MemberCardComponent {
@input('memberInput') memberIn: Member | undefined;
}
