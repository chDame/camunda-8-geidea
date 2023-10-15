import { Component, OnInit } from '@angular/core';

import { AdService } from './ad.service';
import { AdItem } from './ad-item';
import { SignalrService } from './signalR.service';

@Component({
  selector: 'app-root',
  template: `
    <div style="height: 100%;width:100%">
      <app-ad-banner [ads]="ads"></app-ad-banner>
    </div>
  `,

  styles: [
    `
      :host {
        width: 100%;
        display: block;
        height: 100%;
        background-color: white;
      }
    `,
  ],
})
export class AppComponent implements OnInit {
  ads: any;

  constructor(
    private adService: AdService,
    public signalRService: SignalrService
  ) {}

  ngOnInit() {
    this.ads = this.adService.getAds();
  }
}
