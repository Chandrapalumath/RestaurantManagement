import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChefHomeComponent } from './chef-home.component';

describe('ChefHomeComponent', () => {
  let component: ChefHomeComponent;
  let fixture: ComponentFixture<ChefHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChefHomeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChefHomeComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
