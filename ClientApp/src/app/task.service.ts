import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SignalrService } from './signalR.service';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  currentTask: any;
  listOfTasks: any[] = [];
  wizardArray: string[] = [];
  stepTaskMap: any = {};
  currentStep: string = "";

  constructor(private httpClient: HttpClient,
    private signalR: SignalrService) {}

  AddTask(task: any): void {
    this.listOfTasks.push(task);
    if (!this.currentTask) {
      this.setCurrentTask(task);
    }
    if (task.variables && task.variables.wizard) {
      this.stepTaskMap[task.formKey] = task;
      this.wizardArray = task.variables.steps;
    }
  }
  setCurrentTask(task: any): void {
    this.currentTask = task;
    localStorage.setItem("poCCurrentTask", task.formKey);
  }
  getCurrentTask(): any {
    return this.currentTask;
  }
  completeTask(body: any): void {
    this.httpClient
      .post(`https://localhost:7009/api/tasks/${this.currentTask.jobKey}`, body)
      .subscribe((data: any) => {
        this.currentStep = "";
        this.currentTask = null;
        this.listOfTasks = this.listOfTasks.filter(
          (t) => t.jobKey != this.currentTask.jobKey
        );
      });
  }

  selectWizardStep(step: string): void {
    this.currentStep = step;
    this.setCurrentTask(this.stepTaskMap[step]);
  }

  loadInitialTask(): void {
    let correlationId = localStorage.getItem("poCCorrelationId");
    console.log(correlationId);
    if (correlationId) {
      let currentKey = localStorage.getItem("poCCurrentTask");
      console.log(currentKey);
      this.httpClient.get<any[]>(`https://localhost:7009/api/tasks/${correlationId}`).subscribe((data: any[]) => {
        console.log(data);
        this.listOfTasks = data;
        for (let i = 0; i < this.listOfTasks.length; i++) {
          if (this.listOfTasks[i].formKey == currentKey) {
            this.currentTask = this.listOfTasks[i];
            this.signalR.messages.next(this.currentTask);
            break;
          }
        }
      });
      this.signalR.listenTasks(correlationId);
    } else {
      this.signalR.messages.next({ "formKey": "creatAccount" });
    }
  }

  reset(): void {
    localStorage.removeItem("poCCurrentTask");
    localStorage.removeItem("poCCorrelationId");
    this.signalR.messages.next({ "formKey": "creatAccount"});
  }
}
