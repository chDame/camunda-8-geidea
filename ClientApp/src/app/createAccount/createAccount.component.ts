import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { SignalrService } from '../signalR.service';

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
    private signalR: SignalrService
  ) {}

  ngOnInit() {}
  submitForm(): void {
    debugger;
    this.httpCleint
      .post('https://localhost:7009/api/process/createAccount/start', {
        phoneNumber: this.phoneNumber,
        nationalId: this.nationalId,
        initiator: 'demo',
      })
      .subscribe((data: any) => {
        this.listenAboutTasksRequest(data.correlationId.toString());
        console.log(data);
      });
  }
  private listenAboutTasksRequest(correlationId: string): void {
    this.signalR.listenTasks(correlationId);
  }
}
