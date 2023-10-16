import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';

import { AdDirective } from './ad.directive';
import { AdItem } from './ad-item';
import { AdComponent } from './ad.component';
import { SignalrService } from './signalR.service';
import { TaskService } from './task.service';

@Component({
  selector: 'app-ad-banner',
  template: `
    <div class="get-started-page">
      <ng-template adHost></ng-template>
      <!-- <app-createAccount></app-createAccount>
      <app-nafathAuthentication></app-nafathAuthentication>
      <app-emailAndPassword></app-emailAndPassword>
      <app-otp></app-otp>
      <app-wizard></app-wizard> -->
      <br/>
      <hr/>
      <button (click)="reset()">RESET</button>
    </div>
  `,
})
export class AdBannerComponent implements OnInit, OnDestroy {
  @Input() ads!: any;

  currentAdIndex = -1;

  @ViewChild(AdDirective, { static: true }) adHost!: AdDirective;

  private clearTimer: VoidFunction | undefined;
  constructor(
    public signalR: SignalrService,
    private taskService: TaskService,
  ) {}
  ngOnInit(): void {
    this.taskService.loadInitialTask();
    this.getAds();
  }

  reset() {
    this.taskService.reset();
    this.getAds();
  }

  ngOnDestroy() {
    this.clearTimer?.();
  }

  loadComponent(formKey: string) {
    
    // this.currentAdIndex = (this.currentAdIndex + 1) % this.ads.length;
    const adItem = this.ads[formKey];

    const viewContainerRef = this.adHost.viewContainerRef;
    viewContainerRef.clear();

    const componentRef = viewContainerRef.createComponent<AdComponent>(
      adItem.component
    );
    componentRef.instance.data = adItem.data;
  }

  getAds() {
    this.signalR.messages.asObservable().subscribe((data) => {
      if (data && data['formKey']) {
        this.taskService.AddTask(data);
        this.loadComponent(data['formKey']);
      }
    });
  }
}
