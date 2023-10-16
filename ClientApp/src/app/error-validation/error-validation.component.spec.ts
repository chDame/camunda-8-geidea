import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ErrorValidationComponent } from './error-validation.component';

describe('ErrorValidationComponent', () => {
  let component: ErrorValidationComponent;
  let fixture: ComponentFixture<ErrorValidationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ErrorValidationComponent]
    });
    fixture = TestBed.createComponent(ErrorValidationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
