import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BuyShareComponent } from './buy-share.component';

describe('BuyShareComponent', () => {
  let component: BuyShareComponent;
  let fixture: ComponentFixture<BuyShareComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BuyShareComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BuyShareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
