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
    if (this.nafathAuthentication)
      this.taskService.completeTask({
        nafathAuthentication: this.nafathAuthentication,
      });
    // this.httpCleint
    //   .post('https://localhost:7009/api/tasks/jobKey', {
    //     nafathAuthentication: this.nafathAuthentication,
    //   })
    //   .subscribe((data: any) => {
    //     this.signalR.listenTasks(data.correlationId.toString());
    //     console.log(data);
    //   });
  }
}
