import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-otp',
  templateUrl: './otp.component.html',
})
export class OtpComponent implements OnInit {
  otp: string = '';
  constructor() {}

  ngOnInit() {}
}
