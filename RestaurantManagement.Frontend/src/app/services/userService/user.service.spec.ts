import { TestBed } from '@angular/core/testing';

import { AdminCustomerService } from '../../services/userService/user.service';

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
