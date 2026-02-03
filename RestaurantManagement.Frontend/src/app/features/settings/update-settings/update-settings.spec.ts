import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateSettings } from './update-settings';

describe('UpdateSettings', () => {
  let component: UpdateSettings;
  let fixture: ComponentFixture<UpdateSettings>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateSettings]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateSettings);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
