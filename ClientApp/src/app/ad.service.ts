import { Injectable } from '@angular/core';
import { AdItem } from './ad-item';
import { NafathAuthenticationComponent } from './nafathAuthentication/nafathAuthentication.component';
import { EmailAndPasswordComponent } from './emailAndPassword/emailAndPassword.component';
import { OtpComponent } from './otp/otp.component';
import { WizardComponent } from './wizard/wizard.component';
import { CreateAccountComponent } from './createAccount/createAccount.component';

@Injectable()
export class AdService {
  getAds() {
    return {
      creatAccount: new AdItem(CreateAccountComponent),
      nafathAuthentication: new AdItem(NafathAuthenticationComponent),
      setupStores: new AdItem(WizardComponent),
      step1: new AdItem(WizardComponent),
      checkOtp: new AdItem(OtpComponent),
      registerMailAndPassword: new AdItem(EmailAndPasswordComponent),
    };
  }
}
