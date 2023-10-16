import { Component, OnInit } from '@angular/core';
import { SignalrService } from '../signalR.service';
import { HttpClient } from '@angular/common/http';
import { TaskService } from '../task.service';

@Component({
  selector: 'app-nafathAuthentication',
  templateUrl: './nafathAuthentication.component.html',
})
export class NafathAuthenticationComponent implements OnInit {
  nafathAuthentication: boolean = false;
  constructor(
    private httpCleint: HttpClient,
    private signalR: SignalrService,
    private taskService: TaskService
  ) {}

  ngOnInit() {}
  checkNafath(): void {
      this.taskService.completeTask({
        nafathAuthentication: true,
      });
  }
}
