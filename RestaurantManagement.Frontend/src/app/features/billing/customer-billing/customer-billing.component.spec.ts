import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerBillingComponent } from './customer-billing.component';

describe('CustomerBillingComponent', () => {
  let component: CustomerBillingComponent;
  let fixture: ComponentFixture<CustomerBillingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerBillingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomerBillingComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
