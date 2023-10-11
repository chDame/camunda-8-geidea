import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-emailAndPassword',
  templateUrl: './emailAndPassword.component.html',
})
export class EmailAndPasswordComponent implements OnInit {
  email: string = '';
  password: string = '';
  constructor() {}

  ngOnInit() {}
  submitForm() {}
}
