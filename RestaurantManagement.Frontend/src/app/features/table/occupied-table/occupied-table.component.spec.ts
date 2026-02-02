import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OccupiedTableComponent } from './occupied-table.component';

describe('OccupiedTableComponent', () => {
  let component: OccupiedTableComponent;
  let fixture: ComponentFixture<OccupiedTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OccupiedTableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OccupiedTableComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
