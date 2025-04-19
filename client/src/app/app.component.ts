import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { AppUser } from './models/app-user.model';
import { FormBuilder, FormControl, ReactiveFormsModule, FormsModule, Validators } from '@angular/forms';
import { FooterComponent } from "./componens/footer/footer.component";
import { NavbarComponent } from "./componens/navbar/navbar.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink,
    ReactiveFormsModule, FormsModule, FooterComponent, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  
}
