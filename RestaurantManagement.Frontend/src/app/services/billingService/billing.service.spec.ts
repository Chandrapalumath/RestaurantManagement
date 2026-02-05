import { TestBed } from '@angular/core/testing';

import { BillService } from './billing.service';

describe('BillService', () => {
  let service: BillService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BillService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
