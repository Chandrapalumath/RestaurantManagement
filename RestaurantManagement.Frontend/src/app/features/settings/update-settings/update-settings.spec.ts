import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateSettingsComponent } from './update-settings';

describe('UpdateSettings', () => {
  let component: UpdateSettingsComponent;
  let fixture: ComponentFixture<UpdateSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateSettingsComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(UpdateSettingsComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
