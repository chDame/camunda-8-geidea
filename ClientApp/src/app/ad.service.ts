import { Injectable } from '@angular/core';
import { AdItem } from './ad-item';
import { NafathAuthenticationComponent } from './nafathAuthentication/nafathAuthentication.component';
import { EmailAndPasswordComponent } from './emailAndPassword/emailAndPassword.component';
import { OtpComponent } from './otp/otp.component';
import { WizardComponent } from './wizard/wizard.component';
import { CreateAccountComponent } from './createAccount/createAccount.component';
import { SetupStoresComponent } from './setup-stores/setup-stores.component';
import { SelectProductsComponent } from './select-products/select-products.component';

@Injectable()
export class AdService {
  getAds():any {
    return {
      creatAccount: new AdItem(CreateAccountComponent),
      nafathAuthentication: new AdItem(NafathAuthenticationComponent),
      setupStores: new AdItem(WizardComponent),
      selectProducts: new AdItem(WizardComponent),
      checkOtp: new AdItem(OtpComponent),
      registerMailAndPassword: new AdItem(EmailAndPasswordComponent),
      wizardsetupStores: new AdItem(SetupStoresComponent),
      wizardselectProducts: new AdItem(SelectProductsComponent),
    };
  }
}
