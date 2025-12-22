import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { AppUser } from './models/register-user.model';
import { FormBuilder, FormControl, ReactiveFormsModule, FormsModule, Validators } from '@angular/forms';
import { FooterComponent } from "./components/footer/footer.component";
import { NavbarComponent } from "./components/navbar/navbar.component";
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,
    ReactiveFormsModule, FormsModule, FooterComponent, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  accountService = inject(AccountService);

  ngOnInit(): void {
    let loggedInUserStr: string | null = localStorage.getItem('loggedInUser');

    if (loggedInUserStr != null) {
      this.accountService.authorizeLoggedInUser();

      this.accountService.setCurrentUser(JSON.parse(loggedInUserStr));
    }
  }
}
