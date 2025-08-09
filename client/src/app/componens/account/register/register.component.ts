import { Component, inject, OnInit } from '@angular/core';
import { AccountService } from '../../../services/account.service';
import { FormBuilder, FormControl, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppUser } from '../../../models/app-user.model';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';

@Component({
  selector: 'app-register',
  imports: [
    // RouterLink,
    ReactiveFormsModule, FormsModule,
    MatButtonModule, MatInputModule, MatFormFieldModule, MatDatepickerModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {
  accountService = inject(AccountService);
  fB = inject(FormBuilder);

  minDate = new Date();
  maxDate = new Date();

  ngOnInit(): void {
    const currentYear = new Date().getFullYear();
    this.minDate = new Date(currentYear - 99, 0, 1);
    this.maxDate = new Date(currentYear - 6, 0, 1);
  }

  registerFg = this.fB.group({
    emailCtrl: ['', [Validators.required, Validators.email]],
    userNameCtrl: [''],
    passwordCtrl: [''],
    confirmPasswordCtrl: [''],
    dateOfBirthCtrl: ['', [Validators.required]],
    genderCtrl: [''],
    cityCtrl: [''],
    countryCtrl: ['']
  })

  get EmailCtrl(): FormControl {
    return this.registerFg.get('emailCtrl') as FormControl;
  }

  get UserNameCtrl(): FormControl {
    return this.registerFg.get('userNameCtrl') as FormControl;
  }

  get PasswordCtrl(): FormControl {
    return this.registerFg.get('passwordCtrl') as FormControl;
  }

  get ConfirmPasswordCtrl(): FormControl {
    return this.registerFg.get('confirmPasswordCtrl') as FormControl;
  }

  get DateOfBirthCtrl(): FormControl {
    return this.registerFg.get('dateOfBirthCtrl') as FormControl;
  }

  get GenderCtrl(): FormControl {
    return this.registerFg.get('genderCtrl') as FormControl;
  }

  get CityCtrl(): FormControl {
    return this.registerFg.get('cityCtrl') as FormControl;
  }

  get CountryCtrl(): FormControl {
    return this.registerFg.get('countryCtrl') as FormControl;
  }

  register(): void {
    const dob: string | undefined = this.getDateOnly(this.DateOfBirthCtrl.value);

    if (this.PasswordCtrl.value === this.ConfirmPasswordCtrl.value) {
      let user: AppUser = {
        email: this.EmailCtrl.value,
        userName: this.UserNameCtrl.value,
        password: this.PasswordCtrl.value,
        confirmPassword: this.ConfirmPasswordCtrl.value,
        dateOfBirth: dob,
        gender: this.GenderCtrl.value,
        city: this.CityCtrl.value,
        country: this.CountryCtrl.value
      }

      let registerResponse$ = this.accountService.register(user);

      registerResponse$.subscribe({
        next: (res) => console.log(res),
        error: (err) => console.log(err.error)
      });
    }
  }

  getDateOnly(dob: string | null): string | undefined {
    if (!dob) return undefined;

    let theDob: Date = new Date(dob);
    return new Date(theDob.setMinutes(theDob.getMinutes() - theDob.getTimezoneOffset())).toISOString().slice(0, 10);
    // gets the first 10 chars from this date YYYY-MM-DDTHH:mm:ss.sssZ the output is YYYY-MM-DD
  }
}