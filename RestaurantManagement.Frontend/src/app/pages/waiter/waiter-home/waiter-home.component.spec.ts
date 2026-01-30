import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaiterHomeComponent } from './waiter-home.component';

describe('WaiterHomeComponent', () => {
  let component: WaiterHomeComponent;
  let fixture: ComponentFixture<WaiterHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WaiterHomeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WaiterHomeComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
