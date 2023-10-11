import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';

import { AdDirective } from './ad.directive';
import { AdItem } from './ad-item';
import { AdComponent } from './ad.component';
import { SignalrService } from './signalR.service';

@Component({
  selector: 'app-ad-banner',
  template: `
    <div class="ad-banner-example">
      <h3>Advertisements</h3>
      <ng-template adHost></ng-template>
      <app-createAccount></app-createAccount>
    </div>
  `,
})
export class AdBannerComponent implements OnInit, OnDestroy {
  @Input() ads!: any;

  currentAdIndex = -1;

  @ViewChild(AdDirective, { static: true }) adHost!: AdDirective;

  private clearTimer: VoidFunction | undefined;
  constructor(public signalR: SignalrService) {}
  ngOnInit(): void {
    this.loadComponent('step1');
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
      console.log(data);
      if (data && data['formKey']) {
        this.loadComponent(data['formKey']);
      }
    });
    // const interval = setInterval(() => {
    //   this.loadComponent();
    // }, 3000);
    // this.clearTimer = () => clearInterval(interval);
  }
}
