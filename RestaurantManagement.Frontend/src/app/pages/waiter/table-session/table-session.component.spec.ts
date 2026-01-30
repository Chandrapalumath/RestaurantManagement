import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableSessionComponent } from './table-session.component';

describe('TableSessionComponent', () => {
  let component: TableSessionComponent;
  let fixture: ComponentFixture<TableSessionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TableSessionComponent]
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
