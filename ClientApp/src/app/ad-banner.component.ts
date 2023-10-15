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
    private taskService: TaskService
  ) {}
  ngOnInit(): void {
    this.loadComponent('creatAccount');
    this.getAds();
  }

  ngOnDestroy() {
    this.clearTimer?.();
  }

  loadComponent(formKey: string) {
    console.log(formKey);
    console.log(this.ads);

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

        console.log(data);
      }
    });
  }
}
