import { TestBed } from '@angular/core/testing';

import { UserSharesService } from './user-shares.service';

describe('UserSharesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UserSharesService = TestBed.get(UserSharesService);
    expect(service).toBeTruthy();
  });
});
