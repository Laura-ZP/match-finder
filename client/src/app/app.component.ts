import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { AppUser } from './models/app-user.model';
import { FormBuilder, FormControl, ReactiveFormsModule, FormsModule, Validators } from '@angular/forms';
import { FooterComponent } from "./componens/footer/footer.component";
import { NavbarComponent } from "./componens/navbar/navbar.component";
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink,
    ReactiveFormsModule, FormsModule, FooterComponent, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  accountService = inject(AccountService);

  ngOnInit(): void {
    let loggedInUser: string | null = localStorage.getItem('loggedInUser');
    
    if (loggedInUser != null)
      this.accountService.setCurrentUser(JSON.parse(loggedInUser));
  }
}
