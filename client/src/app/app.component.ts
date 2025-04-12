import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppUser } from './models/app-user.model';
import { FormBuilder, FormControl, ReactiveFormsModule, FormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, 
    ReactiveFormsModule, FormsModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  fB = inject(FormBuilder);
  http = inject(HttpClient);

  recivedUser: AppUser | undefined;
  recivedUser1: AppUser | undefined;
  errorMessage: string = '';
  errorMessage1: string = '';
  recivedUsers: AppUser[] | undefined;

  registerFg = this.fB.group({
    emailCtrl: ['', [Validators.required, Validators.email]],
    userNameCtrl: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(30)]],
    ageCtrl: ['', [Validators.required, Validators.min(18), Validators.max(80)]],
    isAliveCtrl: ['', [Validators.required]]
  });

  get EmailCtrl(): FormControl {
    return this.registerFg.get('emailCtrl') as FormControl;
  }

  get UserNameCtrl(): FormControl {
    return this.registerFg.get('userNameCtrl') as FormControl;
  }

  get AgeCtrl(): FormControl {
    return this.registerFg.get('ageCtrl') as FormControl;
  }

  get IsAliveCtrl(): FormControl {
    return this.registerFg.get('isAliveCtrl') as FormControl;
  }

  create(): void {
    let appUser:  AppUser = {
      email: this.EmailCtrl.value,
      userName: this.UserNameCtrl.value,
      age: this.AgeCtrl.value,
      isAlive: this.IsAliveCtrl.value
    }

    this.http.post<AppUser>('http://localhost:5000/api/user/create', appUser).subscribe({
      next: (response: AppUser) => (this.recivedUser = response, console.log(response)),
      error: (err) => (this.errorMessage = err.error, console.log(err.error))
    });
  }

  getAll(): void {
    this.http.get<AppUser[]>('http://localhost:5000/api/user').subscribe({
      next: (response: AppUser[]) => (this.recivedUsers = response, console.log(response))
    });
  }

  getbyUserName(): void {
    this.http.get<AppUser>('http://localhost:5000/api/user/get-by-username/www').subscribe({
      next: (response: AppUser) => (this.recivedUser1 = response, console.log(response)),
      error: (err) => (this.errorMessage = err.error, console.log(err.error))
    });
  }

  update(): void {
    let appUser: AppUser = {
      email: this.EmailCtrl.value,
      userName: this.UserNameCtrl.value,
      age: this.AgeCtrl.value,
      isAlive: this.IsAliveCtrl.value
    }

    this.http.put('http://localhost:5000/api/user/update/67a757e1e2567b9c56107674', appUser).subscribe();
  }

  delete(): void {
    this.http.delete<AppUser>('http://localhost:5000/api/user/delete/67a757e1e2567b9c56107674').subscribe({
      error: (err) => (this.errorMessage1 = err.error, console.log(err.error))
    });
  }
}
