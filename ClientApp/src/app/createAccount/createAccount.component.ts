import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { SignalrService } from '../signalR.service';

@Component({
  selector: 'app-createAccount',
  templateUrl: './createAccount.component.html',
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
    this.httpCleint
      .post('https://localhost:7009/api/process/createAccount/start', {
        phoneNumber: this.phoneNumber,
        nationalId: this.nationalId,
      })
      .subscribe((data: any) => {
        this.listenAboutTasksRequest(data.processInstanceKey.toString());
        console.log(data);
      });
  }
  private listenAboutTasksRequest(processInstanceKey: string): void {
    this.signalR.listenTasks(processInstanceKey);
  }
}
