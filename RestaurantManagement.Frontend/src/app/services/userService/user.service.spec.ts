import { TestBed } from '@angular/core/testing';

import { AdminCustomerService } from './user.service';

describe('AdminCustomerService', () => {
  let service: AdminCustomerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdminCustomerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
