import { Component, OnInit } from '@angular/core';
import { TaskService } from '../task.service';

@Component({
  selector: 'app-otp',
  templateUrl: './otp.component.html',
})
export class OtpComponent implements OnInit {
  otp: string = '';
  constructor(
    private taskService: TaskService) {}

  ngOnInit() { }
  submitForm() {
    this.taskService.completeTask({
      userOtp: this.otp
    });
  }
}
