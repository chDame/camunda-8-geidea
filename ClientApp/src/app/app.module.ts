import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { HeroJobAdComponent } from './hero-job-ad.component';
import { AdBannerComponent } from './ad-banner.component';
import { HeroProfileComponent } from './hero-profile.component';
import { AdDirective } from './ad.directive';
import { AdService } from './ad.service';
import { HttpClientModule } from '@angular/common/http';
import { CreateAccountComponent } from './createAccount/createAccount.component';
import { FormsModule } from '@angular/forms';
import { NafathAuthenticationComponent } from './nafathAuthentication/nafathAuthentication.component';
import { EmailAndPasswordComponent } from './emailAndPassword/emailAndPassword.component';
import { OtpComponent } from './otp/otp.component';
import { WizardComponent } from './wizard/wizard.component';
import { SelectProductsComponent } from './select-products/select-products.component';
import { SetupStoresComponent } from './setup-stores/setup-stores.component';

@NgModule({
  imports: [BrowserModule, HttpClientModule, FormsModule],
  providers: [AdService],
  declarations: [		
    AppComponent,
    AdBannerComponent,
    HeroJobAdComponent,
    HeroProfileComponent,
    AdDirective,
    CreateAccountComponent,
    NafathAuthenticationComponent,
    EmailAndPasswordComponent,
      OtpComponent,
      WizardComponent,
      SelectProductsComponent,
      SetupStoresComponent
   ],
  bootstrap: [AppComponent],
})
export class AppModule {}
