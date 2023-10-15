import { Component, OnInit, ViewChild } from '@angular/core';
import { AdDirective } from '../ad.directive';
import { TaskService } from '../task.service';
import { AdService } from '../ad.service';
import { AdComponent } from '../ad.component';

@Component({
  selector: 'app-wizard',
  templateUrl: './wizard.component.html',
})
export class WizardComponent implements OnInit {
  steps: any[] = [];

  @ViewChild(AdDirective, { static: true }) adHost!: AdDirective;

  constructor(private taskService: TaskService, private adService: AdService) {}

  ngOnInit() {
    this.steps = this.taskService.wizardArray;
    if (this.taskService.getCurrentTask()) {
      this.loadComponent(this.taskService.getCurrentTask().formKey);
    }
  }


  selectWizardStep(step: string) {
    this.taskService.selectWizardStep(step);
    this.loadComponent(this.taskService.getCurrentTask().formKey);
  }


  loadComponent(formKey: string) {
    console.log(formKey);

    // this.currentAdIndex = (this.currentAdIndex + 1) % this.ads.length;
    const adItem = this.adService.getAds()["wizard"+formKey];

    const viewContainerRef = this.adHost.viewContainerRef;
    viewContainerRef.clear();

    const componentRef = viewContainerRef.createComponent<AdComponent>(
      adItem.component
    );
    componentRef.instance.data = adItem.data;
  }
}
