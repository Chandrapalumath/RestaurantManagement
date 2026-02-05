import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableSessionComponent } from './table-session.component';
import { CustomerComponent } from '../../user/add-customer/customer.component';

describe('TableSessionComponent', () => {
  let component: TableSessionComponent;
  let fixture: ComponentFixture<TableSessionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TableSessionComponent, CustomerComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(TableSessionComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
