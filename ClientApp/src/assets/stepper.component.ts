import { Component, HostBinding } from '@angular/core';
import { TranslationCatalogues } from 'libs/enum/refrenceDataCatalouge';

@Component({
  selector: 'geidea-stepper',
  templateUrl: './stepper.component.html',
})
export class StepperComponent  {
  stepperImages: StepperImg = {
    done: 'assets/images/done-step.svg',
    inprogress: 'assets/images/inprogress-step.svg',
    dimmed: 'assets/images/dimmed-step.svg',
    onhold: 'assets/images/hold-step.svg',
  };
  readonly TranslationCatalogues = TranslationCatalogues;
  @HostBinding('class') scrollBar='scrollBar'
}
interface StepperImg {
  done: string;
  inprogress: string;
  dimmed: string;
  onhold: string;
}
