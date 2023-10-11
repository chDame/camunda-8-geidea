import { Component, OnInit } from '@angular/core';
import { TaskService } from '../task.service';

@Component({
  selector: 'app-wizard',
  templateUrl: './wizard.component.html',
})
export class WizardComponent implements OnInit {
  steps: any[] = [];
  constructor(private taskService: TaskService) {}

  ngOnInit() {
    this.steps = this.taskService.wizardArray;
  }
}
