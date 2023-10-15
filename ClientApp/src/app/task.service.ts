import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  currentTask: any;
  listOfTasks: any[] = [];
  wizardArray: string[] = [];
  stepTaskMap: any = {};
  currentStep: string = "";

  constructor(private httpClient: HttpClient) {}

  AddTask(task: any): void {
    this.listOfTasks.push(task);
    if (!this.currentTask) {
      this.currentTask = task;
    }
    if (task.variables.wizard) {
      this.stepTaskMap[task.formKey] = task;
      this.wizardArray = task.variables.steps;
    }
  }
  setCurrentTask(task: any): void {
    this.currentTask = task;
  }
  getCurrentTask(): any {
    return this.currentTask;
  }
  completeTask(body: any): void {
    this.httpClient
      .post(`https://localhost:7009/api/tasks/${this.currentTask.jobKey}`, body)
      .subscribe((data: any) => {
        this.listOfTasks = this.listOfTasks.filter(
          (t) => t.jobKey != this.currentTask.jobKey
        );
        this.currentStep = "";
        this.currentTask = null;
      });
  }

  selectWizardStep(step: string): void {
    this.currentStep = step;
    this.currentTask = this.stepTaskMap[step];
  }
}
