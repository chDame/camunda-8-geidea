import { Component, OnInit } from '@angular/core';

import { AdService } from './ad.service';
import { AdItem } from './ad-item';
import { SignalrService } from './signalR.service';

@Component({
  selector: 'app-root',
  template: `
    <div>
      <app-ad-banner [ads]="ads"></app-ad-banner>
    </div>
  `,
})
export class AppComponent implements OnInit {
  ads: AdItem[] = [];

  constructor(
    private adService: AdService,
    public signalRService: SignalrService
  ) {}

  ngOnInit() {
    this.signalRService.connect();

    this.ads = this.adService.getAds();
  }
}
