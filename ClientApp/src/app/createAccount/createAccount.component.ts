import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { SignalrService } from '../signalR.service';
import { TaskService } from '../task.service';

@Component({
  selector: 'app-createAccount',
  templateUrl: './createAccount.component.html',
  styles: [
    `
      :host {
        width: 100%;
      }
    `,
  ],
})
export class CreateAccountComponent implements OnInit {
  phoneNumber: string = '';
  nationalId: string = '';
  constructor(
    private httpCleint: HttpClient,
    private signalR: SignalrService,
    private taskService: TaskService
  ) {}

  ngOnInit() {}
  submitForm(): void {
    this.taskService.currentTask = null;
    this.httpCleint
      .post('https://localhost:7009/api/process/createAccount/start', {
        phoneNumber: this.phoneNumber,
        nationalId: this.nationalId,
        initiator: 'demo',
      })
      .subscribe((data: any) => {
        localStorage.setItem("poCCorrelationId", data.correlationId.toString());
        this.listenAboutTasksRequest(data.correlationId.toString());
        console.log(data);
      });
  }
  private listenAboutTasksRequest(correlationId: string): void {
    this.signalR.listenTasks(correlationId);
  }
}
