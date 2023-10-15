import { Component, OnInit } from '@angular/core';
import { TaskService } from '../task.service';

@Component({
  selector: 'app-emailAndPassword',
  templateUrl: './emailAndPassword.component.html',
})
export class EmailAndPasswordComponent implements OnInit {
  email: string = '';
  password: string = '';
  constructor(
    private taskService: TaskService) {}

  ngOnInit() {}
  submitForm() {

    this.taskService.completeTask({
      usermail: this.email,
      userpassword: this.password
    });
}
}
