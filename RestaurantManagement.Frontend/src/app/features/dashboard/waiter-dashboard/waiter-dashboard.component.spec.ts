import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaiterDashboardComponent } from './waiter-dashboard.component';

describe('WaiterDashboardComponent', () => {
  let component: WaiterDashboardComponent;
  let fixture: ComponentFixture<WaiterDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WaiterDashboardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WaiterDashboardComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
